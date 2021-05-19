using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIS.Business;
using SIS.Entity;
using SIS.Principal.Models;
using System.Diagnostics;

namespace SIS.Principal.Controllers
{
    public class SeguridadController : Controller
    {
        Authentication Authentication = new Authentication();
        BUsuario BUsuario = BUsuario.ObtenerInstancia();
        BPerfil Perfil = new BPerfil();
        BGeneral General = new BGeneral();
        // GET: Seguridad
     
        public ActionResult Login()

        {
            if (Authentication.UserLogued != null)
            {
                return RedirectToAction("Principal", "Seguridad");
            }
            return View();
        }

        public ActionResult DefaultError()
        {
            return View();
        }
        public ActionResult Error404()
        {
            return View();
        }
        public ActionResult Menu()
        {
            return View();
        }
        [SessionFilter]
        public ActionResult Principal()
        {
            return View();
        }
        public ActionResult Usuarios()
        {
            return View();
        }
        public ActionResult Perfiles()
        {
            return View();
        }

        public ActionResult Empresa()
        {
            return View();

        }
        public ActionResult CreateEmpresa(int? data)
        {
            int id;
            id = data.GetValueOrDefault();
            ViewBag.IdEmpresa = id;
            return View();

        }
        [SessionFilter]
        public ActionResult AccesosPorPerfil(int id)
        {
            int perfil = Authentication.UserLogued.Perfil.Id;
            ViewBag.Perfil = Perfil.BuscarPerfilPorId(id);
            ViewBag.MenuPerfil = Perfil.ListarAccesosPorPerfil(id, perfil);
            return View();
        }

        [SessionFilter]
        public ActionResult VistaPrevia()
        {
            ViewBag.IdEmpresa = Authentication.UserLogued.Empresa.Id;
            ViewBag.Ruc = Authentication.UserLogued.Empresa.RUC;
            ViewBag.RazonSocial = Authentication.UserLogued.Empresa.RazonSocial;
            //ViewBag.Sucursal = Authentication.UserLogued.Sucursal.IdSucursal;
            ViewBag.IdUsuario = Authentication.UserLogued.Id;
            return View();
        }

        [HttpPost]
        public ActionResult Login(EUsuario Model)
        {
            try
            {
                if (!string.IsNullOrEmpty(Model.Usuario) && !string.IsNullOrEmpty(Model.Password))
                {
                    Authentication.UserLogued = BUsuario.Login(Model.Usuario, Model.Password);
                    if (Authentication.UserLogued.Respuesta == 2)
                    {
                        Authentication.UserLogued.Menu = BUsuario.ListarMenuPorUsuario(Model.Usuario, Model.RUC);
                        if (Authentication.UserLogued.Menu.Count > 0)
                        {
                            var now = DateTime.Now;
                            
                            // Guardado de actividad en sesión
                            Authentication.SessionCookie = new HttpCookie("SessionCookie"); // Creación de cookie
                            Authentication.SessionCookie.Expires = now.AddDays(1); //expire en 24H 

                            Authentication.SessionCookie.Values.Add("Usuario", Model.Usuario); // Guardado del usuario en sesión 
                            Authentication.SessionCookie.Values.Add("RUC", Model.RUC); // Guardado del ruc en sesión

                            Debug.WriteLine(Authentication.SessionCookie.Values);
                            Response.Cookies.Add(Authentication.SessionCookie);

                            Session.Timeout = (int)Authentication.SessionCookie.Expires .Subtract(now).TotalMinutes; //monutos de 
                            Session["Usuario"] = Authentication.UserLogued;

                          
                            //if (Authentication.UserLogued.Id == 1)
                            //{
                            //    return RedirectToAction("Principal", "Seguridad");
                            //}
                            //else
                            //{

                            //}
                            return RedirectToAction("Principal", "Seguridad");
                        }
                        else
                        {
                            ViewBag.Message = "El rol que usted posee asignado no tiene permisos asignados.";
                        }
                    }
                    else
                    {
                        ViewBag.Message = "Credenciales Incorrectas";
                    }
                }
                else
                {
                    ViewBag.Message = "Debe ingresar un usuario y contraseña.";
                }
            }
            catch (Exception Exception)
            {
                ViewBag.Message = Exception.Message;
            }
            return View();
        }

 
        public ActionResult ImpresarSistema(string lstSucursal, string TextSucursal, string txtrazon)
        {
            Authentication.SessionCookie.Values.Add("IdSucursal", lstSucursal);
            Authentication.SessionCookie.Values.Add("NombreSucursal", TextSucursal);
            Response.Cookies.Add(Authentication.SessionCookie);
            Session["IdSucursal"] = lstSucursal;
            Session["NombreSucursal"] = TextSucursal;
            return RedirectToAction("Principal", "Seguridad");

        }


        public ActionResult Logout()
        {
            // Eliminación de cookie
            Authentication.SessionCookie = new HttpCookie("SessionCookie");
            Authentication.SessionCookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(Authentication.SessionCookie);

            // Cierre de sesión
            Session.Clear();
            Session.RemoveAll();
            Session.Abandon();

            return RedirectToAction("Login", "Seguridad");
        }
        //Mantenimiento  usuario
        [HttpPost]
        public void InstUsuario(EUsuario oDatos, HttpPostedFileBase documento)
        {
            try
            {
                var usuario = Authentication.UserLogued.Usuario;
                oDatos.Empresa = new EEmpresa
                {
                    RUC = Authentication.UserLogued.Empresa.RUC
                };

                if (documento != null)
                {



                    string adjunto = DateTime.Now.ToString("yyyyMMddHHmmss") + documento.FileName;
                    documento.SaveAs(Server.MapPath("~/Imagenes/Usuario/" + adjunto));

                    oDatos.Imagen = adjunto;
                    Utils.WriteMessage(BUsuario.InstUsuario(oDatos, usuario));
                }
                else
                {

                    Utils.WriteMessage(BUsuario.InstUsuario(oDatos, usuario));

                }


            }
            catch (Exception Exception)
            {
                Utils.Write(
                    ResponseType.JSON,
                    "{ Code: 2, ErrorMessage: \"" + Exception.Message + "\" }"
                );
            }
        }

        [HttpPost]
        public void ListaEditUsuario(int Id)
        {
            try
            {

                int empresa = Authentication.UserLogued.Empresa.Id;
                var model = BUsuario.ListaEditUsuario(Id, empresa);
                if (model.Imagen == "")
                {
                    model.Imagen = $"/Imagenes/Usuario/avatar.jpg";
                }
                else
                {
                    model.Imagen = $"/Imagenes/Usuario/{model.Imagen}";
                }


                Utils.Write(
                    ResponseType.JSON,
                    model
                );
            }
            catch (Exception Exception)
            {
                Utils.Write(
                    ResponseType.JSON,
                    "{ Code: 1, ErrorMessage: \"" + Exception.Message + "\" }"
                );
            }
        }

        public ActionResult ListaUsuario()
        {

            int empresa = Authentication.UserLogued.Empresa.Id;
            var List = BUsuario.ListaUsuario(empresa);
            return Json(new { data = List }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void Eliminar(int Id, int IdFlag)
        {
            try
            {
                var Usuario = Authentication.UserLogued.Usuario;
                Utils.WriteMessage(BUsuario.Eliminar(Id, IdFlag, Usuario));
            }
            catch (Exception Exception)
            {
                Utils.Write(
                    ResponseType.JSON,
                    "{ Code: 2, ErrorMessage: \"" + Exception.Message + "\" }"
                );
            }
        }

        [HttpPost]
        public void Insertar_Perfil(EPerfil oDato, string Usuario)
        {
            try
            {
                Usuario = Authentication.UserLogued.Usuario;
                oDato.Empresa.Id = Authentication.UserLogued.Empresa.Id;
                Utils.WriteMessage(Perfil.Insertar_Perfil(oDato, Usuario));
            }
            catch (Exception Exception)
            {
                Utils.Write(
                    ResponseType.JSON,
                    "{ Code: 2, ErrorMessage: \"" + Exception.Message + "\" }"
                );
            }
        }

        public ActionResult ListaPerfil()
        {
            int Empresa = Authentication.UserLogued.Empresa.Id;

            var List = Perfil.ListarPerfil(Empresa);
            return Json(new { data = List }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public void ListaEditPerfil(int Id)
        {
            try
            {
                Utils.Write(
                    ResponseType.JSON,
                    Perfil.ListaEditPerfil(Id)
                );
            }
            catch (Exception Exception)
            {
                Utils.Write(
                    ResponseType.JSON,
                    "{ Code: 1, ErrorMessage: \"" + Exception.Message + "\" }"
                );
            }
        }

        [HttpPost]
        public void ActualizarAccesosPorPerfil(int Id, string Menus)
        {
            try
            {
                Utils.WriteMessage(Perfil.ActualizarAccesosPorPerfil(Id, Menus));
            }
            catch (Exception Exception)
            {
                Utils.WriteMessage("error|" + Exception.Message);
            }
        }
        [HttpPost]
        public void CambiarPassword(string newpass, string oldpass)
        {
            try
            {
                Utils.WriteMessage(BUsuario.CambiarPassword(Authentication.UserLogued.Id, newpass, oldpass));
            }
            catch (Exception Exception)
            {
                Utils.Write(
                    ResponseType.JSON,
                   "{Code: 1, ErrorMessage: \"" + Exception.Message + "\" }");

            }
        }

        public ActionResult ListadoMenu()
        {
            var List = General.ListaMenu();
            return Json(new { data = List }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public void BuscarMenuId(int Id)
        {
            try
            {
                Utils.Write(
                    ResponseType.JSON,
                    General.ListaEditMenu(Id)
                );
            }
            catch (Exception Exception)
            {
                Utils.Write(
                    ResponseType.JSON,
                    "{ Code: 1, ErrorMessage: \"" + Exception.Message + "\" }"
                );
            }
        }

        [HttpPost]
        public void RegistrarMenu(EMenu eMenu, string Usuario)
        {
            try
            {
                Usuario = Authentication.UserLogued.Usuario;
                Utils.WriteMessage(General.Insertar_UpdateMenu(eMenu, Usuario)
                    );
            }
            catch (Exception Exception)
            {
                Utils.Write(
                    ResponseType.JSON,
                    "{ Code: 2, ErrorMessage: \"" + Exception.Message + "\" }"
                );
            }
        }
    }
}