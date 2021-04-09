using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using SIS.Entity;
using SIS.Business;
using System.Web.SessionState;
namespace SIS.Principal.Models
{
    public class Authentication
    {
        private BUsuario BUsuario = BUsuario.ObtenerInstancia();

        public const int SinPermisos = 1;
        public const int DirectorioActivo = 2;
        public const int ServidorIris = 3;

        public const string SessionCookieName = "SessionCookie";
        public const string SessionUser = "UsuarioLogueado";

        public EUsuario UserLogued { get; set; }
        public int idComprobantePago { get; set; }
        public int idTipoComprobantePago { get; set; }

        public HttpRequest Request { get; set; }
        public HttpResponse Response { get; set; }
        public HttpCookie SessionCookie { get; set; }
        public HttpSessionState Session { get; set; }

        public Authentication()
        {
            Request = HttpContext.Current.Request;
            Response = HttpContext.Current.Response;
            SessionCookie = Request.Cookies["SessionCookie"];
            idComprobantePago = 0;

            AssignUser();

            if (UserLogued != null)
            {
                Session = HttpContext.Current.Session;
            }
            else
            {
                //sale de sesionm
            }
               
        }
        public void RestartSession()
        {
            if (UserLogued != null)
            {
                Session.Timeout = (int)(DateTime.Now.AddDays(1) - DateTime.Now).TotalSeconds; // Definición del tiempo de sesión con respecto a la cookie
                Session.Add("Usuario", UserLogued); // Guardado del usuario logueado en sesión
                return;
            }
            //throw new InvalidOperationException("There is not an user logued to restart session.");
        }

        private void AssignUser()
        {
            // Si existe un usuario logueado
            if (SessionCookie != null)
            {
                // Se captura un usuario
                string sUsuario = SessionCookie.Values["Usuario"];
                string ruc = SessionCookie.Values["RUC"];

                UserLogued = BUsuario.BuscarPorUsuario(sUsuario, ruc);
                string idsucursal = SessionCookie.Values["IdSucursal"];
                string nombreSucursal = SessionCookie.Values["NombreSucursal"];
                
                //if (idsucursal != null)
                //{
                //    UserLogued.Sucursal.IdSucursal = int.Parse(idsucursal);
                //    UserLogued.Sucursal.Nombre = nombreSucursal;
                //}


                List<EMenu> lPermisos = BUsuario.ListarMenuPorUsuario(sUsuario, ruc);
                if (lPermisos.Count > 0)
                {
                    UserLogued.Menu = lPermisos;
                }
            }
        }
        public bool IsValidView(long Id)
        {
            if (UserLogued != null)
            {
                foreach (EMenu Permiso in UserLogued.Menu)
                {
                    if (Id == Permiso.Id)
                        return true;
                }
            }
            return false;
        }
    }
}