using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIS.Business;
using SIS.Entity;
using SIS.Principal.Models;
using RestSharp;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Web.Configuration;
using ZXing;
using System.IO;
using System.Drawing.Imaging;


namespace SIS.Principal.Controllers
{
    public class SharedController : Controller
    {
        Authentication Authentication = new Authentication();
        BGeneral General = new BGeneral();
        // GET: Shared



        public ActionResult ImprimirEtiqueta()
        {

            ViewBag.Lista = TempData["oDatos"];
            return View();
        }

        //public ActionResult Configuracion()
        //{
        //    return View();
        //}

        [HttpPost]
        public void ImprimirBarra(List<EBarra> sLista)
        { 
            List<EBarra> oDatos = new List<EBarra>();
            for (int i = 0; i < sLista.Count; i++)
            {
                EBarra obj = new EBarra();
                byte[] imageByteData = GenerateBarCodeZXing(sLista[i].codigo);
                string imageBase64Data = Convert.ToBase64String(imageByteData);
                string imageDataURL = string.Format("data:image/png;base64,{0}", imageBase64Data);
                obj.codigo = sLista[i].codigo;
                obj.imgen = imageDataURL;
                obj.nombre = sLista[i].nombre;
                obj.precio = sLista[i].precio;
                obj.cantidad = sLista[i].cantidad;

                oDatos.Add(obj);
            }
            TempData["oDatos"] = oDatos;
            Utils.WriteMessage("success|correcto");

            //  return RedirectToAction("ImprimirEtiqueta", "Shared");
        }

        private byte[] GenerateBarCodeZXing(string data)
        {
            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.CODE_128,
                Options = new ZXing.Common.EncodingOptions
                {
                    Width = 60,
                    Height = 40,
                    Margin = 0,
                    PureBarcode = true
                }
            };
            var imgBitmap = writer.Write(data);
            using (var stream = new MemoryStream())
            {
                imgBitmap.Save(stream, ImageFormat.Png);
                return stream.ToArray();
            }
        }

        [HttpPost]
        public void ListaCombo(int Id)
        {
            try
            {
                int empresa = Authentication.UserLogued.Empresa.Id;
                Utils.Write(
                    ResponseType.JSON,
                    General.CBOLista(Id, empresa)
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
        public void ListaComboId(int flag, int Id)
        {
            try
            {
                int empresa = Authentication.UserLogued.Empresa.Id;              
                Utils.Write(
                    ResponseType.JSON,
                    General.CBOListaId(flag, Id, empresa));
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
        public void ListaComboAlmacenes()
        {
            try
            {
                int empresa = Authentication.UserLogued.Empresa.Id;
                string usuario = Authentication.UserLogued.Usuario;
                Utils.Write(
                    ResponseType.JSON,
                    General.ListaComboAlmacenes(empresa, usuario));
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
        public void ListarUbigeo(string Acction, string idPais, string IdDep, string IdProv, string IdDis)
        {
            try
            {
                Utils.Write(
                    ResponseType.JSON,
                    General.ListarUbigeo(Acction, idPais, IdDep, IdProv, IdDis));
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
        public void ListaEmpresaLogin()
        {
            try
            {

                Utils.Write(
                    ResponseType.JSON,
                    General.ListaEmpresaLogin());
            }
            catch (Exception Exception)
            {
                Utils.Write(
                    ResponseType.JSON,
                    "{ Code: 1, ErrorMessage: \"" + Exception.Message + "\" }"
                );
            }
        }

        

        

    }
}