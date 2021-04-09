using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIS.Business;
using SIS.Entity;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using SIS.Principal.Models;

using ZXing;
using System.Drawing.Imaging;
using ZXing.Common;
using System.Security.Authentication;

namespace SIS.Principal.Controllers
{
    public class VentaController : Controller
    {
        Authentication Authentication = new Authentication();
        BVenta Venta = new BVenta();
        BMantenimiento Mantenimiento = new BMantenimiento();
        Conversion Convertir = new Conversion();
        EnviarCorreo EnviarCorreo = new EnviarCorreo();
        BConfiguracion Configuracion = new BConfiguracion();
        BGeneral General = new BGeneral();
        // GET: Venta

        public ActionResult Nuevo()
        {
            return View();
        }
        public ActionResult Lista()
        {
            return View();
        }
        public ActionResult PostVenta()
        {
            ViewBag.idSucursal = Authentication.UserLogued.Sucursal.IdSucursal;
            return View();
        }

        public ActionResult ListaPostVenta()
        {

            return View();

        }
        public ActionResult Apertura()
        {
            return View();
        }
        public ActionResult CierreCaja()
        {
            return View();
        }
        public ActionResult EmiNotCredito()
        {
            return View();
        }
        public ActionResult ListCreditDebito()
        {
            return View();
        }
        public ActionResult ControlEnvios()
        {
            return View();
        }
        public ActionResult VentasTipoPago()
        {
            return View();
        }
        public ActionResult ElimiarVenta()
        {
            return View();
        }  
        public ActionResult GuiaRemision()
        {
            return View();
        }   
        public ActionResult ListaGuia()
        {
            return View();
        }
        [HttpPost]
        public void SerieNumero(int documento)
        {
            try
            {
                int empresa = Authentication.UserLogued.Empresa.Id;
                int sucursal = Authentication.UserLogued.Sucursal.IdSucursal;
                Utils.Write(
                    ResponseType.JSON,
                    Venta.SerieNuroDocumento(documento, empresa, sucursal)
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
        public void InstRegistrarVenta(EVenta oDatos, List<EVentaDetalle> Detalle)
        {
            try
            {
                var Usuario = Authentication.UserLogued.Usuario;
                oDatos.empresa.Id = Authentication.UserLogued.Empresa.Id;
                oDatos.Sucursal.IdSucursal = Authentication.UserLogued.Sucursal.IdSucursal;
                EEmpresa Empresa = Mantenimiento.ListaEditEmpresa(oDatos.empresa.Id);
                var ruta = Server.MapPath("~/Comprobantes/");
                var rutaServidor = Server.MapPath("~/Certificado/" + Empresa.Certificado);
                var claveCertificado = Empresa.ClaveCertificado;

                var Stock = Configuracion.ListarConfiguraciones(1, oDatos.empresa.Id, oDatos.Sucursal.IdSucursal);
                oDatos.vstock = int.Parse(Stock[0].Valor);

                var sMensaje = Venta.RegistrarVenta(oDatos, Detalle, Usuario);
                Utils.WriteMessage(sMensaje, Utils.WithAdditionals);
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
        public void InstRegistrarPost(EVenta oDatos, List<EPago> pago, List<EVentaDetalle> Detalle)
        {
            try
            {
                var Usuario = Authentication.UserLogued.Usuario;
                oDatos.empresa.Id = Authentication.UserLogued.Empresa.Id;

                EEmpresa Empresa = Mantenimiento.ListaEditEmpresa(oDatos.empresa.Id);
                oDatos.Sucursal.IdSucursal = Authentication.UserLogued.Sucursal.IdSucursal;
                var ruta = Server.MapPath("~/Comprobantes/");
                var rutaServidor = Server.MapPath("~/Certificado/" + Empresa.Certificado);
                var claveCertificado = Empresa.ClaveCertificado;

                var Stock = Configuracion.ListarConfiguraciones(1, oDatos.empresa.Id, oDatos.Sucursal.IdSucursal);
                oDatos.vstock =int.Parse(Stock[0].Valor);

                var sMensaje = Venta.RegistrarPost(oDatos, pago, Detalle, Usuario);
                Utils.WriteMessage(sMensaje, Utils.WithAdditionals);
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
        public void ListaVenta(string filtro, string FechaIncio, string FechaFin, int numPag, int allReg, int Cant)
        {
            try
            {
                int empresa = Authentication.UserLogued.Empresa.Id;
                int sucursal = Authentication.UserLogued.Sucursal.IdSucursal;
                Utils.Write(
                    ResponseType.JSON,
                    Venta.ListaVenta(filtro, empresa, sucursal, FechaIncio, FechaFin, numPag, allReg, Cant)
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
        public void ListaVentaPOST(string filtro, string FechaIncio, string FechaFin, int numPag, int allReg, int Cant)
        {
            try
            {
                int empresa = Authentication.UserLogued.Empresa.Id;
                int sucursal = Authentication.UserLogued.Sucursal.IdSucursal;
                Utils.Write(
                    ResponseType.JSON,
                    Venta.ListaVentaPOST(filtro, empresa, sucursal, FechaIncio, FechaFin, numPag, allReg, Cant)
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
        public void ListaVentaPOSEnvios(string filtro, string FechaIncio, string FechaFin, int numPag, int allReg, int Cant)
        {
            try
            {
                int empresa = Authentication.UserLogued.Empresa.Id;
                int sucursal = Authentication.UserLogued.Sucursal.IdSucursal;
                Utils.Write(
                    ResponseType.JSON,
                    Venta.ListaVentaPOSEnvios(filtro, empresa, sucursal, FechaIncio, FechaFin, numPag, allReg, Cant)
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
        public void EstadoEnviadoPOS(int IdVenta, string Motivo, int Flag)
        {
            try
            {
                var Usuario = Authentication.UserLogued.Usuario;
                int Empresa = Authentication.UserLogued.Empresa.Id;

                Utils.WriteMessage(Venta.EstadoEnviadoPOS(IdVenta, Empresa, Motivo, Flag, Usuario));
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
        public void ConsultaStock(int idProducto, int almacen, double iCantidad)
        {
            try
            {
                var empresa = Authentication.UserLogued.Empresa.Id;
                var sucursal = Authentication.UserLogued.Sucursal.IdSucursal;
                
                var Stock = Configuracion.ListarConfiguraciones(1, empresa, sucursal);
                var vstock = int.Parse(Stock[0].Valor);

                var mensaje = Venta.VerificarStock(empresa, almacen, sucursal, idProducto, iCantidad, vstock);
                Utils.WriteMessage(mensaje);
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
        public void ListaComboUsuario()
        {
            try
            {
                int empresa = Authentication.UserLogued.Empresa.Id;
                int IdSucursal = Authentication.UserLogued.Sucursal.IdSucursal;
                string Usuario = Authentication.UserLogued.Usuario;
                Utils.Write(ResponseType.JSON, Venta.ListarUsuarios(empresa, IdSucursal, Usuario));
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
        public void ListadoCaja()
        {
            try
            {
                int empresa = Authentication.UserLogued.Empresa.Id;
                int IdSucursal = Authentication.UserLogued.Sucursal.IdSucursal;
                Utils.Write(ResponseType.JSON, Mantenimiento.ListaCaja(empresa, IdSucursal));
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
        public void ListaMoneda()
        {
            try
            {
                Utils.Write(ResponseType.JSON, Venta.ListarMoneda());
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
        public void ListadoApertura()
        {
            try
            {
                int empresa = Authentication.UserLogued.Empresa.Id;
                int IdSucursal = Authentication.UserLogued.Sucursal.IdSucursal;
                string Usuario = Authentication.UserLogued.Usuario;
                Utils.Write(ResponseType.JSON, Venta.ListaApertura(empresa, IdSucursal, Usuario));
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
        public void RegistrarApertura(EApertura Apertura)
        {
            try
            {
                var Usuario = Authentication.UserLogued.Usuario;
                var empresa = Authentication.UserLogued.Empresa.Id;
                int IdSucursal = Authentication.UserLogued.Sucursal.IdSucursal;
                Utils.WriteMessage(Venta.Insertar_AperturaCaja(Apertura, Usuario, empresa, IdSucursal));
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
        public void ListaAperturaTodo(int numPag, int allReg, int Cant)
        {
            try
            {
                int empresa = Authentication.UserLogued.Empresa.Id;
                string Usuario = Authentication.UserLogued.Usuario;
                int IdSucursal = Authentication.UserLogued.Sucursal.IdSucursal;
                Utils.Write(ResponseType.JSON, Venta.ListaAperturaTodo(empresa, IdSucursal, Usuario, numPag, allReg, Cant));
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
        public void ObtenerComprobante(int Id)
        {
            try
            {
                int empresa = Authentication.UserLogued.Empresa.Id;
                Utils.Write(
                    ResponseType.JSON,
                         Venta.ImprimirFacBolPOST(Id, empresa));
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
        public void ObtenerComprobanteVenta(int Id)
        {
            try
            {
                int empresa = Authentication.UserLogued.Empresa.Id;
                Utils.Write(
                    ResponseType.JSON,
                         Venta.ImprimirFacBol(Id, empresa));
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
        public void InstNotaCreditoDebito(ENotaCreditoDebito oDatos, List<EDetalleCretioDebito> Detalle)
        {
            try
            {
                var Usuario = Authentication.UserLogued.Usuario;
                oDatos.Empresa.Id = Authentication.UserLogued.Empresa.Id;
                oDatos.Sucursal.IdSucursal = Authentication.UserLogued.Sucursal.IdSucursal;

                var sMensaje = Venta.InstNotaCreditoDebito(oDatos, Detalle, Usuario);
                Utils.WriteMessage(sMensaje, Utils.WithAdditionals);
            }
            catch (Exception Exception)
            {
                Utils.Write(
                    ResponseType.JSON,
                    "{ Code: 2, ErrorMessage: \"" + Exception.Message + "\" }"
                );
            }
        }


        public void ListaNotaCreditoDebito(string filtro, string FechaIncio, string FechaFin, int numPag, int allReg, int Cant)
        {
            try
            {
                int empresa = Authentication.UserLogued.Empresa.Id;
                int sucursal = Authentication.UserLogued.Sucursal.IdSucursal;
                Utils.Write(
                    ResponseType.JSON,
                    Venta.ListaNotaCreditoDebito(filtro, empresa, sucursal, FechaIncio, FechaFin, numPag, allReg, Cant)
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
        public void anularVenta(EVenta oDatos, List<EVentaDetalle> Detalle, string usuario)
        {
            try
            {
                var Usuario = Authentication.UserLogued.Usuario;
                oDatos.empresa.Id = Authentication.UserLogued.Empresa.Id;
                oDatos.Sucursal.IdSucursal = Authentication.UserLogued.Sucursal.IdSucursal;

                var sMensaje = Venta.elimiarVenta(oDatos, Detalle, Usuario);
                Utils.WriteMessage(sMensaje);
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
        public void Buscartransporte(string pRuc, string Filtro2)
        {
            try
            {
                Utils.Write(
                    ResponseType.JSON,
                    General.ListaTransRuc(pRuc, Filtro2)
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
        public void ListaPrecio(int comienzo, int iMedia, int producto, int cliente)
        {
            try
            {
                Utils.Write(
                    ResponseType.JSON,
                    Venta.ListaPrecio(comienzo, iMedia, producto, cliente)
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
        //reporte
        #region Reporte PDF
        public ActionResult ReportePDF(string filtro, string FechaIncio, string FechaFin, int numPag, int allReg, int Cant, int venta)
        {
            Document pdfDoc = new Document(PageSize.A4, 5, 5, 10, 10);
            int empresa = Authentication.UserLogued.Empresa.Id;
            int sucursal = Authentication.UserLogued.Sucursal.IdSucursal;
            List<EVenta> ListaCompra;
            if (venta == 1)
            {
                ListaCompra = Venta.ListaVenta(filtro, empresa, sucursal, FechaIncio, FechaFin, numPag, allReg, Cant);
            }
            else
            {
                ListaCompra = Venta.ListaVentaPOST(filtro, empresa, sucursal, FechaIncio, FechaFin, numPag, allReg, Cant);

            }


            if (ListaCompra.Count > 0)
            {
                //Permite visualizar el contenido del documento que es descargado en "Mis descargas"
                PdfWriter.GetInstance(pdfDoc, System.Web.HttpContext.Current.Response.OutputStream);

                //Definiendo parametros para la fuente de la cabecera y pie de pagina
                Font fuente = FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, Color.BLACK);

                //Se define la cabecera del documento
                HeaderFooter cabecera = new HeaderFooter(new Phrase("Fecha: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString(), fuente), false);//'el valor es false porque no habra numeración
                pdfDoc.Header = cabecera;
                cabecera.Border = 0;// Rectangle.BOTTOM_BORDER
                cabecera.Alignment = HeaderFooter.ALIGN_RIGHT;


                HeaderFooter pie = new HeaderFooter(new Phrase("pagia", fuente), true);

                pdfDoc.Footer = pie;
                pie.Border = Rectangle.TOP_BORDER;
                pie.Alignment = HeaderFooter.ALIGN_RIGHT;

                //Open PDF Document to write data 
                pdfDoc.Open();

                PdfPTable tblPrueba = new PdfPTable(1);
                tblPrueba.WidthPercentage = 100;


                //Se define la fuente para el texto del reporte
                Font _standardFont = FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD);

                PdfPTable Tableitems = new PdfPTable(14);
                Tableitems.WidthPercentage = 100;
                Tableitems.HorizontalAlignment = Element.ALIGN_JUSTIFIED;

                PdfPCell celCospan = new PdfPCell(new Phrase("REPORTE VENTA"));
                celCospan.Colspan = 14;
                celCospan.Border = 0;
                celCospan.HorizontalAlignment = 1;
                Tableitems.AddCell(celCospan);


                PdfPCell cEspacio = new PdfPCell(new Phrase("    "));
                cEspacio.Colspan = 14;
                cEspacio.Border = 0;
                cEspacio.HorizontalAlignment = 1;
                Tableitems.AddCell(cEspacio);

                Single[] celdas2 = new Single[] { 1.0F, 1.1F, 1.2F, 1.0F, 2.0F, 1.0F, 1.2F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F };
                Tableitems.SetWidths(celdas2);

                PdfPCell cTituloCant = new PdfPCell(new Phrase("Serie", _standardFont));
                cTituloCant.Border = 1;
                cTituloCant.BorderWidthBottom = 1;
                cTituloCant.HorizontalAlignment = 1;
                cTituloCant.Padding = 5;
                Tableitems.AddCell(cTituloCant);

                PdfPCell cTituloNumero = new PdfPCell(new Phrase("Numero", _standardFont));
                cTituloNumero.Border = 1;
                cTituloNumero.BorderWidthBottom = 1;
                cTituloNumero.HorizontalAlignment = 1;
                cTituloNumero.Padding = 5;
                Tableitems.AddCell(cTituloNumero);


                PdfPCell cTituloDocumento = new PdfPCell(new Phrase("Doc.", _standardFont));
                cTituloDocumento.Border = 1;
                cTituloDocumento.BorderWidthBottom = 1;
                cTituloDocumento.HorizontalAlignment = 1;
                cTituloDocumento.Padding = 5;
                Tableitems.AddCell(cTituloDocumento);

                PdfPCell CTituloruc = new PdfPCell(new Phrase("ruc", _standardFont));
                CTituloruc.Border = 1;
                CTituloruc.BorderWidthBottom = 1;
                CTituloruc.HorizontalAlignment = 1;
                CTituloruc.Padding = 5;
                Tableitems.AddCell(CTituloruc);

                PdfPCell CTituloSerie = new PdfPCell(new Phrase("Client", _standardFont));
                CTituloSerie.Border = 1;
                CTituloSerie.BorderWidthBottom = 1;
                CTituloSerie.HorizontalAlignment = 1;
                CTituloSerie.Padding = 5;
                Tableitems.AddCell(CTituloSerie);

                PdfPCell cTipoPago = new PdfPCell(new Phrase("Mon.", _standardFont));
                cTipoPago.Border = 1;
                cTipoPago.BorderWidthBottom = 1;
                cTipoPago.HorizontalAlignment = 1;
                cTipoPago.Padding = 5;
                Tableitems.AddCell(cTipoPago);

                PdfPCell cMonedaT = new PdfPCell(new Phrase("Fecha", _standardFont));
                cMonedaT.Border = 1;
                cMonedaT.BorderWidthBottom = 1;
                cMonedaT.HorizontalAlignment = 1;
                cMonedaT.Padding = 5;
                Tableitems.AddCell(cMonedaT);


                PdfPCell cFechaRegistro = new PdfPCell(new Phrase("Cant.", _standardFont));
                cFechaRegistro.Border = 1;
                cFechaRegistro.BorderWidthBottom = 1;
                cFechaRegistro.HorizontalAlignment = 1;
                cFechaRegistro.Padding = 5;
                Tableitems.AddCell(cFechaRegistro);

                PdfPCell cFechaPago = new PdfPCell(new Phrase("Ope. Grav.", _standardFont));
                cFechaPago.Border = 1;
                cFechaPago.BorderWidthBottom = 1;
                cFechaPago.HorizontalAlignment = 1;
                cFechaPago.Padding = 5;
                Tableitems.AddCell(cFechaPago);

                PdfPCell cSubTotal = new PdfPCell(new Phrase("Ope. Exone.", _standardFont));
                cSubTotal.Border = 1;
                cSubTotal.BorderWidthBottom = 1;
                cSubTotal.HorizontalAlignment = 1;
                cSubTotal.Padding = 5;
                Tableitems.AddCell(cSubTotal);

                PdfPCell cIGV = new PdfPCell(new Phrase("Ope. Ina.", _standardFont));
                cIGV.Border = 1;
                cIGV.BorderWidthBottom = 1;
                cIGV.HorizontalAlignment = 1;
                cIGV.Padding = 5;
                Tableitems.AddCell(cIGV);

                PdfPCell cTitulo = new PdfPCell(new Phrase("IGV", _standardFont));
                cTitulo.Border = 1;
                cTitulo.BorderWidthBottom = 1;
                cTitulo.HorizontalAlignment = 1;
                cTitulo.Padding = 5;
                Tableitems.AddCell(cTitulo);


                PdfPCell cTituloTotal = new PdfPCell(new Phrase("Total", _standardFont));
                cTituloTotal.Border = 1;
                cTituloTotal.BorderWidthBottom = 1;
                cTituloTotal.HorizontalAlignment = 1;
                cTituloTotal.Padding = 5;
                Tableitems.AddCell(cTituloTotal);


                PdfPCell cTituloDescuento = new PdfPCell(new Phrase("Desc.", _standardFont));
                cTituloDescuento.Border = 1;
                cTituloDescuento.BorderWidthBottom = 1;
                cTituloDescuento.HorizontalAlignment = 1;
                cTituloDescuento.Padding = 5;
                Tableitems.AddCell(cTituloDescuento);

                Font letrasDatosTabla = FontFactory.GetFont(FontFactory.HELVETICA, 7, Font.NORMAL);

                double IGV = 0, SubTotal = 0, Total = 0, exonerada = 0, inafecta = 0, descuento = 0;

                foreach (EVenta cuentaPago in ListaCompra)
                {


                    PdfPCell cSerie = new PdfPCell(new Phrase(cuentaPago.serie, letrasDatosTabla));
                    cSerie.Border = 0;
                    cSerie.Padding = 2;
                    cSerie.PaddingBottom = 2;
                    cSerie.HorizontalAlignment = 1;
                    Tableitems.AddCell(cSerie);



                    PdfPCell cnumero = new PdfPCell(new Phrase(cuentaPago.numero, letrasDatosTabla));
                    cnumero.Border = 0;
                    cnumero.Padding = 2;
                    cnumero.PaddingBottom = 2;
                    cnumero.HorizontalAlignment = 1;
                    Tableitems.AddCell(cnumero);

                    PdfPCell Cdocumento = new PdfPCell(new Phrase(cuentaPago.Documento.Nombre, letrasDatosTabla));
                    Cdocumento.Border = 0;
                    Cdocumento.Padding = 2;
                    Cdocumento.PaddingBottom = 2;
                    Cdocumento.HorizontalAlignment = 1;
                    Tableitems.AddCell(Cdocumento);


                    PdfPCell NroDocumento = new PdfPCell(new Phrase(cuentaPago.cliente.NroDocumento, letrasDatosTabla));
                    NroDocumento.Border = 0;
                    NroDocumento.Padding = 2;
                    NroDocumento.PaddingBottom = 2;
                    NroDocumento.HorizontalAlignment = 1;
                    Tableitems.AddCell(NroDocumento);

                    PdfPCell Nombre = new PdfPCell(new Phrase(cuentaPago.cliente.Nombre, letrasDatosTabla));
                    Nombre.Border = 0;
                    Nombre.Padding = 2;
                    Nombre.PaddingBottom = 2;
                    Nombre.HorizontalAlignment = 1;
                    Tableitems.AddCell(Nombre);


                    PdfPCell cellTipoPago = new PdfPCell(new Phrase(cuentaPago.moneda.Nombre, letrasDatosTabla));
                    cellTipoPago.Border = 0;
                    cellTipoPago.Padding = 2;
                    cellTipoPago.PaddingBottom = 2;
                    cellTipoPago.HorizontalAlignment = 1;
                    Tableitems.AddCell(cellTipoPago);

                    PdfPCell cFecha = new PdfPCell(new Phrase(cuentaPago.fechaEmision, letrasDatosTabla));
                    cFecha.Border = 0;
                    cFecha.Padding = 2;
                    cFecha.PaddingBottom = 2;
                    cFecha.HorizontalAlignment = 1;
                    Tableitems.AddCell(cFecha);


                    PdfPCell cellMoneda = new PdfPCell(new Phrase(cuentaPago.cantidad.ToString(), letrasDatosTabla));
                    cellMoneda.Border = 0;
                    cellMoneda.Padding = 2;
                    cellMoneda.PaddingBottom = 2;
                    cellMoneda.HorizontalAlignment = 1;
                    Tableitems.AddCell(cellMoneda);


                    PdfPCell cellFechaRegistro = new PdfPCell(new Phrase(Math.Round(cuentaPago.grabada, 2).ToString(), letrasDatosTabla));
                    cellFechaRegistro.Border = 0;
                    cellFechaRegistro.Padding = 2;
                    cellFechaRegistro.PaddingBottom = 2;
                    cellFechaRegistro.HorizontalAlignment = 1;
                    Tableitems.AddCell(cellFechaRegistro);


                    PdfPCell cellFSubTotal = new PdfPCell(new Phrase(Math.Round(cuentaPago.exonerada, 2).ToString(), letrasDatosTabla));
                    cellFSubTotal.Border = 0;
                    cellFSubTotal.Padding = 2;
                    cellFSubTotal.PaddingBottom = 2;
                    cellFSubTotal.HorizontalAlignment = 1;
                    Tableitems.AddCell(cellFSubTotal);

                    PdfPCell cellIGV = new PdfPCell(new Phrase(Math.Round(cuentaPago.inafecta, 2).ToString(), letrasDatosTabla));
                    cellIGV.Border = 0;
                    cellIGV.Padding = 2;
                    cellIGV.PaddingBottom = 2;
                    cellIGV.HorizontalAlignment = 1;
                    Tableitems.AddCell(cellIGV);

                    PdfPCell cigvcel = new PdfPCell(new Phrase(Math.Round(cuentaPago.igv, 2).ToString(), letrasDatosTabla));
                    cigvcel.Border = 0;
                    cigvcel.Padding = 2;
                    cigvcel.PaddingBottom = 2;
                    cigvcel.HorizontalAlignment = 1;
                    Tableitems.AddCell(cigvcel);

                    PdfPCell cNroDocumento = new PdfPCell(new Phrase(Math.Round(cuentaPago.total, 2).ToString(), letrasDatosTabla));
                    cNroDocumento.Border = 0;
                    cNroDocumento.Padding = 2;
                    cNroDocumento.PaddingBottom = 2;
                    cNroDocumento.HorizontalAlignment = 1;
                    Tableitems.AddCell(cNroDocumento);


                    PdfPCell sdecuento = new PdfPCell(new Phrase(Math.Round(cuentaPago.descuento, 2).ToString(), letrasDatosTabla));
                    sdecuento.Border = 0;
                    sdecuento.Padding = 2;
                    sdecuento.PaddingBottom = 2;
                    sdecuento.HorizontalAlignment = 1;
                    Tableitems.AddCell(sdecuento);


                    Total += Convert.ToDouble(cuentaPago.total);
                    IGV += Convert.ToDouble(cuentaPago.igv);
                    SubTotal += Convert.ToDouble(cuentaPago.grabada);
                    exonerada += Convert.ToDouble(cuentaPago.exonerada);
                    inafecta += Convert.ToDouble(cuentaPago.inafecta);
                    descuento += Convert.ToDouble(cuentaPago.descuento);
                }

                PdfPCell cCeldaDetalle = new PdfPCell(Tableitems);
                cCeldaDetalle.Border = 1;
                cCeldaDetalle.BorderWidthBottom = 1;
                cCeldaDetalle.HorizontalAlignment = 1;
                cCeldaDetalle.Padding = 5;
                tblPrueba.AddCell(cCeldaDetalle);


                PdfPCell cCeldaTotal = new PdfPCell(new Phrase("     "));
                cCeldaTotal.Colspan = 10;
                cCeldaTotal.Border = 0;
                cCeldaTotal.HorizontalAlignment = 0;
                tblPrueba.AddCell(cCeldaTotal);

                PdfPTable TableValor = new PdfPTable(1);
                TableValor.WidthPercentage = 100;

                float[] celdas = new float[] { 5.0F };
                TableValor.SetWidths(celdas);



                PdfPCell cTTotales = new PdfPCell(new Phrase("Ope. gravada:   " + string.Format("{0:n}", SubTotal), _standardFont));
                cTTotales.Border = 0;
                cTTotales.Colspan = 2;
                cTTotales.HorizontalAlignment = 2; // 0 = izquierda, 1 = centro, 2 = derecha
                TableValor.AddCell(cTTotales);

                PdfPCell cexonerada = new PdfPCell(new Phrase("Ope. exonerada:   " + string.Format("{0:n}", exonerada), _standardFont));
                cexonerada.Border = 0;
                cexonerada.Colspan = 2;
                cexonerada.HorizontalAlignment = 2; // 0 = izquierda, 1 = centro, 2 = derecha
                TableValor.AddCell(cexonerada);

                PdfPCell cinafecta = new PdfPCell(new Phrase("Ope. inafecta:   " + string.Format("{0:n}", inafecta), _standardFont));
                cinafecta.Border = 0;
                cinafecta.Colspan = 2;
                cinafecta.HorizontalAlignment = 2; // 0 = izquierda, 1 = centro, 2 = derecha
                TableValor.AddCell(cinafecta);

                PdfPCell sIGV = new PdfPCell(new Phrase("IGV:   " + string.Format("{0:n}", IGV), _standardFont));
                sIGV.Border = 0;
                sIGV.Colspan = 2;
                sIGV.HorizontalAlignment = 2; // 0 = izquierda, 1 = centro, 2 = derecha
                TableValor.AddCell(sIGV);

                PdfPCell cTotales = new PdfPCell(new Phrase("Total:   " + string.Format("{0:n}", Total), _standardFont));
                cTotales.Border = 0;
                cTotales.Colspan = 2;
                cTotales.HorizontalAlignment = 2; // 0 = izquierda, 1 = centro, 2 = derecha
                TableValor.AddCell(cTotales);

                PdfPCell cTotalesDescuento = new PdfPCell(new Phrase("Descuento:   " + string.Format("{0:n}", descuento), _standardFont));
                cTotalesDescuento.Border = 0;
                cTotalesDescuento.Colspan = 2;
                cTotalesDescuento.HorizontalAlignment = 2; // 0 = izquierda, 1 = centro, 2 = derecha
                TableValor.AddCell(cTotalesDescuento);

                PdfPCell cCeldaValor = new PdfPCell(TableValor);
                cCeldaValor.Colspan = 2;
                cCeldaValor.Border = 0;
                cCeldaValor.BorderWidthBottom = 1;
                cCeldaValor.PaddingBottom = 2;
                tblPrueba.AddCell(cCeldaValor);


                pdfDoc.Add(tblPrueba);

                // Close your PDF 
                pdfDoc.Close();



                Response.ContentType = "application/pdf";

                //Set default file Name as current datetime 
                Response.AddHeader("content-disposition", "attachment; filename=ReporteCotizacion.pdf");
                Response.Write(pdfDoc);
                Response.Flush();
                Response.End();
            }
            else
            {
                throw new Exception("No se encontró ningún registro.");

            }
            return View();
        }
        #endregion

        #region Reporte Excel control de envios
        public ActionResult ReporteExcelControlEnvios(string cliente, string FechaIncio, string FechaFin, int numPag, int allReg, int Cant, int venta)
        {
            try
            {
                //Ruta de la plantilla
                FileInfo fTemplateFile = new FileInfo(Server.MapPath("/") + "Reporte/Venta/excel/" + "ControlEnviosVentas.xlsx");

                var Usuario = Authentication.UserLogued.Usuario;
                int Empresa = Authentication.UserLogued.Empresa.Id;
                int sucursal = Authentication.UserLogued.Sucursal.IdSucursal;
                //Ruta del nuevo documento
                FileInfo fNewFile = new FileInfo(Server.MapPath("/") + "Reporte/Venta/excel/" + "Venta" + "_" + Usuario + ".xlsx");

                List<EVenta> ListaCompra;
                if (venta == 1)
                {
                    ListaCompra = Venta.ListaVenta(cliente, Empresa, sucursal, FechaIncio, FechaFin, numPag, allReg, Cant);

                }
                else
                {
                    ListaCompra = Venta.ListaVentaPOSEnvios(cliente, Empresa, sucursal, FechaIncio, FechaFin, numPag, allReg, Cant);
                }
                ExcelPackage pck = new ExcelPackage(fNewFile, fTemplateFile);

                var ws = pck.Workbook.Worksheets[1];
                ////** aqui
                var path = Server.MapPath("/") + "assets/images/LogoDefault.png";

                int rowIndex = 0;//numeros
                int colIndex = 3; //letras
                int Height = 220;
                int Width = 120;

                DateTime today = DateTime.Now;
                ws.Cells[6, 2].Value = today;
                ws.Cells[7, 2].Value = Usuario;
                int iFilaDetIni = 10;
                int starRow = 1;
                double SubTotal = 0, IGV = 0, Total = 0, exonerada = 0, inafecta = 0, descuento = 0, envio = 0, totPagar = 0;
                foreach (EVenta Detalle in ListaCompra)
                {
                    ws.Cells[iFilaDetIni + starRow, 1].Value = Detalle.serie;
                    ws.Cells[iFilaDetIni + starRow, 2].Value = Detalle.numero;
                    ws.Cells[iFilaDetIni + starRow, 3].Value = Detalle.Documento.Nombre;
                    ws.Cells[iFilaDetIni + starRow, 4].Value = Detalle.cliente.Nombre;
                    ws.Cells[iFilaDetIni + starRow, 5].Value = Detalle.cliente.NroDocumento;
                    ws.Cells[iFilaDetIni + starRow, 6].Value = Detalle.moneda.Nombre;
                    ws.Cells[iFilaDetIni + starRow, 7].Value = Detalle.fechaEmision;
                    ws.Cells[iFilaDetIni + starRow, 8].Value = Math.Round(Convert.ToDouble(Detalle.cantidad), 2);
                    ws.Cells[iFilaDetIni + starRow, 9].Value = Math.Round(Convert.ToDouble(Detalle.grabada), 2);
                    ws.Cells[iFilaDetIni + starRow, 10].Value = Math.Round(Convert.ToDouble(Detalle.igv), 2);
                    ws.Cells[iFilaDetIni + starRow, 11].Value = Math.Round(Convert.ToDouble(Detalle.total), 2);
                    ws.Cells[iFilaDetIni + starRow, 12].Value = Math.Round(Convert.ToDouble(Detalle.descuento), 2);
                    ws.Cells[iFilaDetIni + starRow, 13].Value = Math.Round(Convert.ToDouble(Detalle.CostoEnvio), 2);
                    ws.Cells[iFilaDetIni + starRow, 14].Value = Math.Round(Convert.ToDouble(Detalle.total + Detalle.CostoEnvio), 2);
                    ws.Cells[iFilaDetIni + starRow, 15].Value = Detalle.Envio.Nombre;
                    ws.Cells[iFilaDetIni + starRow, 16].Value = Detalle.fechaEnvio;
                    ws.Cells[iFilaDetIni + starRow, 17].Value = Detalle.EstadoEnvio;
                    ws.Cells[iFilaDetIni + starRow, 18].Value = Detalle.Nombre;
                    ws.Cells[iFilaDetIni + starRow, 19].Value = Detalle.observacion;
                    ws.Cells[iFilaDetIni + starRow, 20].Value = Detalle.sCanalesVenta;

                    SubTotal += Convert.ToDouble(Detalle.grabada);
                    exonerada += Convert.ToDouble(Detalle.exonerada);
                    inafecta += Convert.ToDouble(Detalle.inafecta);
                    IGV += Convert.ToDouble(Detalle.igv);
                    Total += Convert.ToDouble(Detalle.total);
                    descuento += Convert.ToDouble(Detalle.descuento);
                    envio += Convert.ToDouble(Detalle.CostoEnvio);
                    totPagar += Convert.ToDouble(Detalle.total + Detalle.CostoEnvio);
                    starRow++;
                }

                int Count = starRow + 1;
                ws.Cells[iFilaDetIni + (Count - 1), 8].Value = "Totales";
                ws.Cells[iFilaDetIni + (Count - 1), 9].Value = Math.Round(Convert.ToDouble(SubTotal), 2);
                ws.Cells[iFilaDetIni + (Count - 1), 10].Value = Math.Round(Convert.ToDouble(IGV), 2);
                ws.Cells[iFilaDetIni + (Count - 1), 11].Value = Math.Round(Convert.ToDouble(Total), 2);
                ws.Cells[iFilaDetIni + (Count - 1), 12].Value = Math.Round(Convert.ToDouble(descuento), 2);
                ws.Cells[iFilaDetIni + (Count - 1), 13].Value = Math.Round(Convert.ToDouble(envio), 2);
                ws.Cells[iFilaDetIni + (Count - 1), 14].Value = Math.Round(Convert.ToDouble(totPagar), 2);

                string modelRange = "A" + System.Convert.ToString(iFilaDetIni) + ":T" + (iFilaDetIni + Count - 1).ToString();
                var modelTable = ws.Cells[modelRange];
                modelTable.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                modelTable.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                modelTable.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                modelTable.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                //Guardando Archivo...
                pck.Save();
                //  Liberando...
                pck.Dispose();

                OfficeOpenXml.ExcelPackage pcks = new OfficeOpenXml.ExcelPackage(fNewFile, false);

                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=" + "ReporteControlEnviosVentas" + "_" + Usuario + ".xlsx");

                MemoryStream memoryStream = new MemoryStream();
                pcks.SaveAs(memoryStream);
                memoryStream.WriteTo(Response.OutputStream);
                Response.End();
                pcks.Dispose();

                System.IO.File.Delete(fNewFile.FullName);
                return View();
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        #endregion

        #region Reporte Excel
        public ActionResult ReporteExcel(string cliente, string FechaIncio, string FechaFin, int numPag, int allReg, int Cant, int venta)
        {
            try
            {
                //Ruta de la plantilla
                FileInfo fTemplateFile = new FileInfo(Server.MapPath("/") + "Reporte/Venta/excel/" + "Venta.xlsx");

                var Usuario = Authentication.UserLogued.Usuario;
                int Empresa = Authentication.UserLogued.Empresa.Id;
                int sucursal = Authentication.UserLogued.Sucursal.IdSucursal;
                //Ruta del nuevo documento
                FileInfo fNewFile = new FileInfo(Server.MapPath("/") + "Reporte/Venta/excel/" + "Venta" + "_" + Usuario + ".xlsx");

                List<EVenta> ListaCompra;
                if (venta == 1)
                {
                    ListaCompra = Venta.ListaVenta(cliente, Empresa, sucursal, FechaIncio, FechaFin, numPag, allReg, Cant);

                }
                else
                {
                    ListaCompra = Venta.ListaVentaPOST(cliente, Empresa, sucursal, FechaIncio, FechaFin, numPag, allReg, Cant);
                }
                ExcelPackage pck = new ExcelPackage(fNewFile, fTemplateFile);

                var ws = pck.Workbook.Worksheets[1];
                ////** aqui
                var path = Server.MapPath("/") + "assets/images/LogoDefault.png";

                int rowIndex = 0;//numeros
                int colIndex = 3; //letras
                int Height = 220;
                int Width = 120;
                /*
                System.Drawing.Image logo = System.Drawing.Image.FromFile(path);
                ExcelPicture pic = ws.Drawings.AddPicture("Sample", logo);
                pic.SetPosition(rowIndex, 0, colIndex, 0);
                //pic.SetPosition(PixelTop, PixelLeft);
                pic.SetSize(Height, Width);
                //// aqui

                */

                DateTime today = DateTime.Now;
                ws.Cells[6, 2].Value = today;
                ws.Cells[7, 2].Value = Usuario;
                int iFilaDetIni = 10;
                int starRow = 1;
                double SubTotal = 0, IGV = 0, Total = 0, exonerada = 0, inafecta = 0, descuento = 0, envio = 0, totPagar =0;
                foreach (EVenta Detalle in ListaCompra)
                {
                    ws.Cells[iFilaDetIni + starRow, 1].Value = Detalle.serie;
                    ws.Cells[iFilaDetIni + starRow, 2].Value = Detalle.numero;
                    ws.Cells[iFilaDetIni + starRow, 3].Value = Detalle.Documento.Nombre;
                    ws.Cells[iFilaDetIni + starRow, 4].Value = Detalle.cliente.Nombre;
                    ws.Cells[iFilaDetIni + starRow, 5].Value = Detalle.cliente.NroDocumento;
                    ws.Cells[iFilaDetIni + starRow, 6].Value = Detalle.cliente.Direccion;
                    ws.Cells[iFilaDetIni + starRow, 7].Value = Detalle.cliente.Email;
                    ws.Cells[iFilaDetIni + starRow, 8].Value = Detalle.cliente.Telefono;
                    ws.Cells[iFilaDetIni + starRow, 9].Value = Detalle.moneda.Nombre;
                    ws.Cells[iFilaDetIni + starRow, 10].Value = Detalle.fechaEmision;
                    ws.Cells[iFilaDetIni + starRow, 11].Value = Math.Round(Convert.ToDouble(Detalle.cantidad), 2);
                    ws.Cells[iFilaDetIni + starRow, 12].Value = Math.Round(Convert.ToDouble(Detalle.grabada), 2);
                    ws.Cells[iFilaDetIni + starRow, 13].Value = Math.Round(Convert.ToDouble(Detalle.inafecta), 2);
                    ws.Cells[iFilaDetIni + starRow, 14].Value = Math.Round(Convert.ToDouble(Detalle.exonerada), 2);
                    ws.Cells[iFilaDetIni + starRow, 15].Value = Math.Round(Convert.ToDouble(Detalle.igv), 2);
                    ws.Cells[iFilaDetIni + starRow, 16].Value = Math.Round(Convert.ToDouble(Detalle.total), 2);
                    ws.Cells[iFilaDetIni + starRow, 17].Value = Math.Round(Convert.ToDouble(Detalle.descuento), 2);
                    ws.Cells[iFilaDetIni + starRow, 18].Value = Math.Round(Convert.ToDouble(Detalle.CostoEnvio), 2);
                    ws.Cells[iFilaDetIni + starRow, 19].Value = Math.Round(Convert.ToDouble(Detalle.total + Detalle.CostoEnvio), 2);
                    ws.Cells[iFilaDetIni + starRow, 20].Value = Detalle.Text;
                    ws.Cells[iFilaDetIni + starRow, 21].Value = Detalle.Nombre;
                    ws.Cells[iFilaDetIni + starRow, 22].Value = Detalle.observacion;
                    ws.Cells[iFilaDetIni + starRow, 23].Value = Detalle.Comprobante.Nombre;
                    ws.Cells[iFilaDetIni + starRow, 24].Value = Detalle.sCanalesVenta;

                    SubTotal += Convert.ToDouble(Detalle.grabada);
                    exonerada += Convert.ToDouble(Detalle.exonerada);
                    inafecta += Convert.ToDouble(Detalle.inafecta);
                    IGV += Convert.ToDouble(Detalle.igv);
                    Total += Convert.ToDouble(Detalle.total);
                    descuento += Convert.ToDouble(Detalle.descuento);
                    envio += Convert.ToDouble(Detalle.CostoEnvio);
                    totPagar += Convert.ToDouble(Detalle.total + Detalle.CostoEnvio);
                    starRow++;
                }

                int Count = starRow + 1;
                ws.Cells[iFilaDetIni + (Count - 1), 11].Value = "Totales";
                ws.Cells[iFilaDetIni + (Count - 1), 12].Value = Math.Round(Convert.ToDouble(SubTotal), 2);
                ws.Cells[iFilaDetIni + (Count - 1), 13].Value = Math.Round(Convert.ToDouble(inafecta), 2);
                ws.Cells[iFilaDetIni + (Count - 1), 14].Value = Math.Round(Convert.ToDouble(exonerada), 2);
                ws.Cells[iFilaDetIni + (Count - 1), 15].Value = Math.Round(Convert.ToDouble(IGV), 2);
                ws.Cells[iFilaDetIni + (Count - 1), 16].Value = Math.Round(Convert.ToDouble(Total), 2);
                ws.Cells[iFilaDetIni + (Count - 1), 17].Value = Math.Round(Convert.ToDouble(descuento), 2);
                ws.Cells[iFilaDetIni + (Count - 1), 18].Value = Math.Round(Convert.ToDouble(envio), 2);
                ws.Cells[iFilaDetIni + (Count - 1), 19].Value = Math.Round(Convert.ToDouble(totPagar), 2);

                string modelRange = "A" + System.Convert.ToString(iFilaDetIni) + ":X" + (iFilaDetIni + Count - 1).ToString();
                var modelTable = ws.Cells[modelRange];
                modelTable.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                modelTable.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                modelTable.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                modelTable.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                //Guardando Archivo...
                pck.Save();
                //  Liberando...
                pck.Dispose();

                OfficeOpenXml.ExcelPackage pcks = new OfficeOpenXml.ExcelPackage(fNewFile, false);

                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=" + "ReporteVenta" + "_" + Usuario + ".xlsx");

                MemoryStream memoryStream = new MemoryStream();
                pcks.SaveAs(memoryStream);
                memoryStream.WriteTo(Response.OutputStream);
                Response.End();
                pcks.Dispose();

                System.IO.File.Delete(fNewFile.FullName);
                return View();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        #region Imprimri facBol
        public ActionResult imprimirFacBol(int Id, int Envio, int venta)
        {
            Document pdfDoc = new Document(PageSize.A4, 10, 10, 20, 20);
            MemoryStream ns = new MemoryStream();
            int IdEmpresa = Authentication.UserLogued.Empresa.Id;
            EEmpresa Empresa = Mantenimiento.ListaEditEmpresa(IdEmpresa);
            //  List<EVentaDetalle> imprimir = Venta.ImprimirFacBol(Id, IdEmpresa);
            List<EVentaDetalle> imprimir = null;
            if (venta == 1)
            {
                imprimir = Venta.ImprimirFacBol(Id, IdEmpresa);
            }
            else
            {
                imprimir = Venta.ImprimirFacBolPOST(Id, IdEmpresa);

            }


            try
            {
                //Permite visualizar el contenido del documento que es descargado en "Mis descargas"
                PdfWriter.GetInstance(pdfDoc, System.Web.HttpContext.Current.Response.OutputStream);
                //Ruta dentro del servidor, aqui se guardara el PDF
                var path = Server.MapPath("/") + "Reporte/Venta/pdf" + "_Documento_" + "Venta" + imprimir[0].Venta.serie + "-" + imprimir[0].Venta.serie + ".pdf";

                // Se utiliza system.IO para crear o sobreescribe el archivo si existe
                FileStream file = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);

                //'iTextSharp para escribir en el documento PDF, sin esto no podemos ver el contenido
                PdfWriter.GetInstance(pdfDoc, file);
                PdfWriter pw = PdfWriter.GetInstance(pdfDoc, ns);
                //  'Open PDF Document to write data 
                pdfDoc.Open();
                pdfDoc.SetMargins(35f, 25f, 25f, 35f);
                pdfDoc.NewPage();

                //'Armando el diseño de la factura
                iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 8, iTextSharp.text.Font.NORMAL, Color.BLACK);
                iTextSharp.text.Font _standardFontTabla = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 8, iTextSharp.text.Font.BOLD, Color.WHITE);
                iTextSharp.text.Font _standarTitulo = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 12, iTextSharp.text.Font.BOLD, Color.BLACK);
                iTextSharp.text.Font _standarTexto = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 9, iTextSharp.text.Font.BOLD, Color.BLACK);
                Font _standarTextoNegrita = new Font(Font.HELVETICA, 10, Font.BOLD, Color.BLACK);


                PdfPTable tblPrueba = new PdfPTable(2);
                tblPrueba.WidthPercentage = 100;

                PdfPTable TableCabecera = new PdfPTable(3);
                TableCabecera.WidthPercentage = 100;

                float[] celdasCabecera = new float[] { 0.1F, 0.4F, 0.2F };
                TableCabecera.SetWidths(celdasCabecera);


                PdfPTable TableQR = new PdfPTable(1);
                TableQR.WidthPercentage = 100;


                float[] celdasQR = new float[] { 2.0F };
                TableQR.SetWidths(celdasQR);

                PdfPCell celCospan = new PdfPCell();
                celCospan.Colspan = 3;
                celCospan.Border = 0;
                TableCabecera.AddCell(celCospan);

                //'AGREGANDO IMAGEN:      

                string imagePath = "";
                if (Empresa.Logo == "")
                {
                    imagePath = Server.MapPath("/assets/images/LogoDefault.PNG");
                }
                else
                {
                    imagePath = Server.MapPath("/Imagenes/Empresa/" + Empresa.Logo);
                }

                //imagePath = Server.MapPath("/assets/images/LogoDefault.PNG");
                iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imagePath);
                jpg.ScaleToFit(90.0F, 100.0F);

                PdfPTable tableIzqigm = new PdfPTable(1);
                PdfPCell celImg = new PdfPCell(jpg);
                celImg.HorizontalAlignment = 0;
                celImg.Border = 0;
                tableIzqigm.AddCell(celImg);

                PdfPCell celIzqs = new PdfPCell(tableIzqigm);
                celIzqs.Border = 0;
                TableCabecera.AddCell(celIzqs);

                PdfPTable tableIzq = new PdfPTable(1);
                tableIzq.TotalWidth = 250.0F;
                tableIzq.LockedWidth = true;
                tableIzq.SpacingBefore = 150.0F;
                tableIzq.HorizontalAlignment = Element.ALIGN_CENTER;

                PdfPCell celInfo = new PdfPCell(new Phrase(Empresa.Nombre, _standarTitulo));
                celInfo.HorizontalAlignment = 1;
                celInfo.Border = 0;
                tableIzq.AddCell(celInfo);

                PdfPCell celContacto = new PdfPCell(new Phrase(Empresa.PaginaWeb, _standarTexto));
                celContacto.HorizontalAlignment = 1;
                celContacto.Border = 0;
                // celContacto.Colspan = 2;
                tableIzq.AddCell(celContacto);

                PdfPCell celDireccion = new PdfPCell(new Phrase(Empresa.Direccion + " " + Empresa.Telefono + "| Celular:" + Empresa.Celular, _standarTexto));
                celDireccion.HorizontalAlignment = 1;
                celDireccion.Border = 0;

                //celDireccion.Colspan = 2;
                tableIzq.AddCell(celDireccion);

                PdfPCell celEmail = new PdfPCell(new Phrase("E-mail:" + Empresa.Correo, _standarTexto));
                celEmail.HorizontalAlignment = 1;
                celEmail.Border = 0;
                //celTelefono.Colspan = 2;
                tableIzq.AddCell(celEmail);

                PdfPCell celIzq = new PdfPCell(tableIzq);
                celIzq.Border = 0;
                TableCabecera.AddCell(celIzq);

                PdfPTable tableDer = new PdfPTable(1);



                PdfPCell celEspacio1 = new PdfPCell(new Phrase(""));
                celEspacio1.Border = 0;
                tableDer.AddCell(celEspacio1);

                //


                PdfPCell cRucs = new PdfPCell(new Phrase("RUC - " + Empresa.RUC, _standarTitulo));
                cRucs.Border = 0;
                cRucs.HorizontalAlignment = 1;
                cRucs.BorderWidthRight = 1f;
                cRucs.BorderWidthLeft = 1f;
                cRucs.BorderWidthTop = 1f;
                cRucs.Padding = 8;
                tableDer.AddCell(cRucs);

                PdfPCell cTipoDoc = new PdfPCell(new Phrase(imprimir[0].Comprobante.Nombre + " ELECTRONICA", _standarTitulo));
                cTipoDoc.HorizontalAlignment = 1;
                cTipoDoc.Border = 0;
                cTipoDoc.BorderWidthRight = 1f;
                cTipoDoc.BorderWidthLeft = 1f;
                tableDer.AddCell(cTipoDoc);

                PdfPCell cNumFac = new PdfPCell(new Phrase("N°: " + imprimir[0].Venta.serie + "-" + imprimir[0].Venta.numero, _standarTitulo));
                cNumFac.Border = 0;
                cNumFac.HorizontalAlignment = 1;
                cNumFac.BorderWidthRight = 1f;
                cNumFac.BorderWidthLeft = 1f;
                cNumFac.BorderWidthBottom = 1f;
                tableDer.AddCell(cNumFac);

                PdfPCell celDer = new PdfPCell(tableDer);
                celDer.Border = 0;
                TableCabecera.AddCell(celDer);

                PdfPCell cCeldaDetallec = new PdfPCell(TableCabecera);
                cCeldaDetallec.Colspan = 2;
                cCeldaDetallec.Border = 0;
                cCeldaDetallec.BorderWidthBottom = 0;
                tblPrueba.AddCell(cCeldaDetallec);

                PdfPCell cEspacio = new PdfPCell(new Phrase("      "));
                cEspacio.Colspan = 2;
                cEspacio.Border = 0;
                cEspacio.HorizontalAlignment = 0;
                tblPrueba.AddCell(cEspacio);

                PdfPTable TableCab = new PdfPTable(2);
                ///TableCab.WidthPercentage = 100;
                float[] celdas1 = new float[] { 0.75F, 2.0F };
                TableCab.TotalWidth = 325.0F;
                TableCab.LockedWidth = true;
                TableCab.SpacingBefore = 300.0F;
                TableCab.SetWidths(celdas1);
                TableCab.HorizontalAlignment = Element.ALIGN_LEFT;

                PdfPCell cTitulo2 = new PdfPCell(new Phrase("SEÑOR(ES):", _standardFont));
                cTitulo2.Border = 0;
                cTitulo2.HorizontalAlignment = 0;
                cTitulo2.BorderWidthTop = 1f;
                cTitulo2.BorderWidthLeft = 1f;
                cTitulo2.Padding = 5;
                TableCab.AddCell(cTitulo2);

                PdfPCell cRuc = new PdfPCell(new Phrase(imprimir[0].Venta.cliente.Nombre, _standardFont));
                cRuc.Border = 0;
                cRuc.HorizontalAlignment = 0;
                cRuc.BorderWidthTop = 1f;
                cRuc.BorderWidthRight = 1f;
                cRuc.Padding = 5;
                TableCab.AddCell(cRuc);

                PdfPCell cTituloEspacio = new PdfPCell(new Phrase());
                cTituloEspacio.Border = 0;
                cTituloEspacio.HorizontalAlignment = 0;
                cTituloEspacio.BorderWidthLeft = 1f;
                TableCab.AddCell(cTituloEspacio);

                PdfPCell cRucEspacio = new PdfPCell(new Phrase());
                cRucEspacio.Border = 0;
                cRucEspacio.HorizontalAlignment = 0;
                cRucEspacio.BorderWidthRight = 1f;
                TableCab.AddCell(cRucEspacio);

                PdfPCell cTitulo3 = new PdfPCell(new Phrase("DIRECCION:", _standardFont));
                cTitulo3.Border = 0;
                cTitulo3.HorizontalAlignment = 0;
                cTitulo3.BorderWidthLeft = 1f;
                cTitulo3.Padding = 5;
                TableCab.AddCell(cTitulo3);

                PdfPCell cDireccion = new PdfPCell(new Phrase(imprimir[0].Venta.cliente.Direccion, _standardFont));
                cDireccion.Border = 0;
                cDireccion.HorizontalAlignment = 0;
                cDireccion.BorderWidthRight = 1f;
                cDireccion.Padding = 5;
                TableCab.AddCell(cDireccion);

                PdfPCell cTituloEspacioPago = new PdfPCell(new Phrase());
                cTituloEspacioPago.Border = 0;
                cTituloEspacioPago.HorizontalAlignment = 0;
                cTituloEspacioPago.BorderWidthLeft = 1f;
                TableCab.AddCell(cTituloEspacioPago);

                PdfPCell cRucEspacios = new PdfPCell(new Phrase());
                cRucEspacios.Border = 0;
                cRucEspacios.HorizontalAlignment = 0;
                cRucEspacios.BorderWidthRight = 1f;
                TableCab.AddCell(cRucEspacios);

                PdfPCell cTiPago = new PdfPCell(new Phrase("CONDICIONES DE PAGO:", _standardFont));
                cTiPago.Border = 0;
                cTiPago.HorizontalAlignment = 0;
                cTiPago.BorderWidthLeft = 1f;
                cTiPago.BorderWidthBottom = 1f;
                cTiPago.Padding = 5;
                cTiPago.MinimumHeight = 30;
                TableCab.AddCell(cTiPago);

                PdfPCell cPago = new PdfPCell(new Phrase(imprimir[0].condicionPago.ToString(), _standardFont));
                cPago.Border = 0;
                cPago.HorizontalAlignment = 0;
                cPago.BorderWidthRight = 1f;
                cPago.BorderWidthBottom = 1f;
                cPago.Padding = 5;
                cTiPago.MinimumHeight = 30;
                TableCab.AddCell(cPago);

                PdfPCell cEspacio1 = new PdfPCell(new Phrase());
                cEspacio1.Colspan = 4;
                cEspacio1.Border = 0;
                cEspacio1.HorizontalAlignment = 0;
                //cEspacio1.MinimumHeight = 35;
                TableCab.AddCell(cEspacio1);

                PdfPCell cCeldaDetallecs = new PdfPCell(TableCab);
                cCeldaDetallecs.Colspan = 0;
                cCeldaDetallecs.Border = 0;
                cCeldaDetallecs.PaddingBottom = 2;
                tblPrueba.AddCell(cCeldaDetallecs);

                PdfPTable Tabledoble = new PdfPTable(2);
                float[] celdases = new float[] { 1F, 1.5F };
                Tabledoble.SetWidths(celdases);
                Tabledoble.TotalWidth = 200.0F;
                Tabledoble.LockedWidth = true;
                Tabledoble.SpacingBefore = 200.0F;
                Tabledoble.HorizontalAlignment = Element.ALIGN_RIGHT;


                PdfPCell cTituloFecha = new PdfPCell(new Phrase("FECHA EMISIÓN:", _standardFont));
                cTituloFecha.Border = 0;
                cTituloFecha.HorizontalAlignment = 0;
                cTituloFecha.PaddingTop = 2;
                cTituloFecha.BorderWidthLeft = 1f;
                cTituloFecha.BorderWidthTop = 1f;
                cTituloFecha.BorderWidthBottom = 1f;
                cTituloFecha.Padding = 5;
                Tabledoble.AddCell(cTituloFecha);

                PdfPCell cFchaE = new PdfPCell(new Phrase(imprimir[0].Venta.fechaEmision, _standardFont));
                cFchaE.Border = 0; // borde cero
                cFchaE.HorizontalAlignment = 0;
                cFchaE.PaddingTop = 2;
                cFchaE.BorderWidthRight = 1f;
                cFchaE.BorderWidthBottom = 1f;
                cFchaE.BorderWidthTop = 1f;
                cFchaE.Padding = 5;
                Tabledoble.AddCell(cFchaE);

                PdfPCell espacio = new PdfPCell(new Phrase(""));
                espacio.Border = 0;
                espacio.HorizontalAlignment = 1; // 0 = izquierda, 1 = centro, 2 = derecha
                espacio.Padding = 5;
                Tabledoble.AddCell(espacio);

                PdfPCell espacio1 = new PdfPCell(new Phrase(""));
                espacio1.Border = 0;
                espacio1.HorizontalAlignment = 1; // 0 = izquierda, 1 = centro, 2 = derecha
                espacio1.Padding = 5;
                Tabledoble.AddCell(espacio1);

                PdfPCell cTitulo1 = new PdfPCell(new Phrase("N° RUC:", _standardFont));
                cTitulo1.Border = 0;
                cTitulo1.HorizontalAlignment = 0;
                cTitulo1.PaddingTop = 2;
                cTitulo1.BorderWidthTop = 1f;
                cTitulo1.BorderWidthLeft = 1f;
                cTitulo1.Padding = 5;
                Tabledoble.AddCell(cTitulo1);

                PdfPCell cRazonsocial = new PdfPCell(new Phrase(imprimir[0].Venta.cliente.NroDocumento, _standardFont));
                cRazonsocial.Border = 0; // borde cero
                cRazonsocial.HorizontalAlignment = 0;
                cRazonsocial.PaddingTop = 2;
                cRazonsocial.BorderWidthTop = 1f;
                cRazonsocial.BorderWidthRight = 1f;
                cRazonsocial.Padding = 5;
                Tabledoble.AddCell(cRazonsocial);


                PdfPCell cTitulocom = new PdfPCell(new Phrase("O. COMPRA:", _standardFont));
                cTitulocom.BorderWidthBottom = 1;
                cTitulocom.Border = 0;
                cTitulocom.HorizontalAlignment = 0; // 0 = izquierda, 1 = centro, 2 = derecha
                cTitulocom.Padding = 5;
                cTitulocom.BorderWidthLeft = 1f;
                Tabledoble.AddCell(cTitulocom);

                PdfPCell cTitulcr = new PdfPCell(new Phrase("", _standardFont));
                cTitulcr.Border = 0;
                cTitulcr.BorderWidthBottom = 0;
                cTitulcr.HorizontalAlignment = 0; // 0 = izquierda, 1 = centro, 2 = derecha
                cTitulcr.Padding = 5;
                cTitulcr.BorderWidthRight = 1f;
                Tabledoble.AddCell(cTitulcr);

                PdfPCell cTituloCodigos = new PdfPCell(new Phrase("GUIA:", _standardFont));
                cTituloCodigos.Border = 0;
                cTituloCodigos.HorizontalAlignment = 0; // 0 = izquierda, 1 = centro, 2 = derecha
                cTituloCodigos.Padding = 5;
                cTituloCodigos.BorderWidthLeft = 1f;
                cTituloCodigos.BorderWidthBottom = 1;
                Tabledoble.AddCell(cTituloCodigos);

                PdfPCell cTitul = new PdfPCell(new Phrase(imprimir[0].Guia.serie, _standardFont));
                cTitul.Border = 0;
                cTitul.BorderWidthBottom = 1;
                cTitul.HorizontalAlignment = 0; // 0 = izquierda, 1 = centro, 2 = derecha
                cTitul.Padding = 5;
                cTitul.BorderWidthRight = 1f;
                Tabledoble.AddCell(cTitul);


                PdfPCell cEspacio2 = new PdfPCell(new Phrase());
                cEspacio2.Colspan = 4;
                cEspacio2.Border = 0;
                cEspacio2.HorizontalAlignment = 0;
                //cEspacio1.MinimumHeight = 35;
                Tabledoble.AddCell(cEspacio2);

                PdfPCell cCelda = new PdfPCell(Tabledoble);
                cCelda.Colspan = 0;
                cCelda.Border = 0;
                cCelda.PaddingBottom = 2;
                tblPrueba.AddCell(cCelda);

                PdfPTable Tableitems = new PdfPTable(6);
                Tableitems.WidthPercentage = 100;

                float[] celdas2 = new float[] { 1.0F, 1.0F, 2.0F, 1.0F, 1.0F, 1.0F };
                Tableitems.SetWidths(celdas2);

                PdfPCell cTituloCodigo = new PdfPCell(new Phrase("CANT", _standardFontTabla));
                cTituloCodigo.BorderWidthBottom = 1;
                cTituloCodigo.HorizontalAlignment = 1; // 0 = izquierda, 1 = centro, 2 = derecha
                cTituloCodigo.Padding = 5;
                cTituloCodigo.BackgroundColor = new Color(System.Drawing.Color.MidnightBlue);
                Tableitems.AddCell(cTituloCodigo);

                PdfPCell cTituloDesc = new PdfPCell(new Phrase("UNID", _standardFontTabla));

                cTituloDesc.BorderWidthBottom = 1;
                cTituloDesc.HorizontalAlignment = 1; // 0 = izquierda, 1 = centro, 2 = derecha
                cTituloDesc.Padding = 5;
                cTituloDesc.BackgroundColor = new Color(System.Drawing.Color.MidnightBlue);
                Tableitems.AddCell(cTituloDesc);

                PdfPCell cTituloCant = new PdfPCell(new Phrase("DESCRIPCION", _standardFontTabla));

                cTituloCant.BorderWidthBottom = 1;
                cTituloCant.HorizontalAlignment = 1; // 0 = izquierda, 1 = centro, 2 = derecha
                cTituloCant.Padding = 5;
                cTituloCant.BackgroundColor = new Color(System.Drawing.Color.MidnightBlue);
                Tableitems.AddCell(cTituloCant);

                PdfPCell cTituloPrecio = new PdfPCell(new Phrase("PRECIO UNITARIO", _standardFontTabla));
                cTituloPrecio.BorderWidthBottom = 1;
                cTituloPrecio.HorizontalAlignment = 1; // 0 = izquierda, 1 = centro, 2 = derecha
                cTituloPrecio.Padding = 5;
                cTituloPrecio.BackgroundColor = new Color(System.Drawing.Color.MidnightBlue);
                Tableitems.AddCell(cTituloPrecio);

                PdfPCell cTituloDescuento = new PdfPCell(new Phrase("DESCUENTO", _standardFontTabla));
                cTituloDescuento.BorderWidthBottom = 1;
                cTituloDescuento.HorizontalAlignment = 1; // 0 = izquierda, 1 = centro, 2 = derecha
                cTituloDescuento.Padding = 5;
                cTituloDescuento.BackgroundColor = new Color(System.Drawing.Color.MidnightBlue);
                Tableitems.AddCell(cTituloDescuento);

                PdfPCell cTituloSubT = new PdfPCell(new Phrase("TOTAL", _standardFontTabla));

                cTituloSubT.BorderWidthBottom = 1;
                cTituloSubT.HorizontalAlignment = 1; // 0 = izquierda, 1 = centro, 2 = derecha
                cTituloSubT.Padding = 5;
                cTituloSubT.BackgroundColor = new Color(System.Drawing.Color.MidnightBlue);
                Tableitems.AddCell(cTituloSubT);

                foreach (EVentaDetalle ListaRep in imprimir)
                {

                    PdfPCell cCodigo = new PdfPCell(new Phrase(ListaRep.cantidad.ToString(), _standardFont));
                    cCodigo.Border = 0;
                    cCodigo.Padding = 2;
                    cCodigo.PaddingBottom = 2;
                    cCodigo.HorizontalAlignment = 1;
                    cCodigo.Border = Rectangle.LEFT_BORDER;
                    Tableitems.AddCell(cCodigo);

                    PdfPCell cNombreMat = new PdfPCell(new Phrase(ListaRep.material.Unidad.Nombre, _standardFont));
                    cNombreMat.Border = 0;
                    cNombreMat.Padding = 2;
                    cNombreMat.PaddingBottom = 2;
                    cNombreMat.HorizontalAlignment = 1;
                    cNombreMat.Border = Rectangle.LEFT_BORDER;
                    Tableitems.AddCell(cNombreMat);

                    PdfPCell cCantidad = new PdfPCell(new Phrase(ListaRep.material.Nombre, _standardFont));
                    cCantidad.Border = 0;
                    cCantidad.Padding = 2;
                    cCantidad.PaddingBottom = 2;
                    cCantidad.HorizontalAlignment = 0;
                    cCantidad.Border = Rectangle.LEFT_BORDER;
                    Tableitems.AddCell(cCantidad);

                    PdfPCell cPrecio = new PdfPCell(new Phrase(string.Format("{0:n}", ListaRep.precio), _standardFont));
                    cPrecio.Border = 0;
                    cPrecio.Padding = 2;
                    cPrecio.PaddingBottom = 2;
                    cPrecio.HorizontalAlignment = 1;
                    cPrecio.Border = Rectangle.LEFT_BORDER;
                    Tableitems.AddCell(cPrecio);

                    PdfPCell celldescuento = new PdfPCell(new Phrase(string.Format("{0:n}", ListaRep.descuento), _standardFont));
                    celldescuento.Border = 0;
                    celldescuento.Padding = 2;
                    celldescuento.PaddingBottom = 2;
                    celldescuento.HorizontalAlignment = 1;
                    celldescuento.Border = Rectangle.LEFT_BORDER;
                    Tableitems.AddCell(celldescuento);

                    PdfPCell cImporte = new PdfPCell(new Phrase(string.Format("{0:n}", ListaRep.Importe), _standardFont));
                    cImporte.Border = 0;
                    cImporte.Padding = 2;
                    cImporte.PaddingBottom = 2;
                    cImporte.HorizontalAlignment = 1;
                    cImporte.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                    Tableitems.AddCell(cImporte);
                }


                PdfPCell cCeldaDetalle = new PdfPCell(Tableitems);
                cCeldaDetalle.Colspan = 4;
                cCeldaDetalle.Border = 0;
                cCeldaDetalle.BorderWidthBottom = 1;
                cCeldaDetalle.PaddingBottom = 4;
                cCeldaDetalle.FixedHeight = 400f; //460f
                tblPrueba.AddCell(cCeldaDetalle);

                PdfPTable TableValorLetra = new PdfPTable(1);
                TableValorLetra.WidthPercentage = 100;

                PdfPCell cLetras = new PdfPCell(new Phrase("SON: " + Convertir.enletras(imprimir[0].Venta.total.ToString()) + " " + imprimir[0].Venta.moneda.Nombre, _standardFont));
                cLetras.Border = 0;
                cLetras.Padding = 2;
                cLetras.PaddingBottom = 2;
                cLetras.HorizontalAlignment = 0;
                cLetras.Colspan = 1;
                TableValorLetra.AddCell(cLetras);


                PdfPCell cCeldaLetras = new PdfPCell(TableValorLetra);
                cCeldaLetras.Colspan = 0;
                cCeldaLetras.Border = 0;
                cCeldaLetras.PaddingBottom = 5;
                tblPrueba.AddCell(cCeldaLetras);

                PdfPCell cCeldaTotal = new PdfPCell(new Phrase());
                cCeldaTotal.Colspan = 10;
                cCeldaTotal.Border = 0;
                cCeldaTotal.HorizontalAlignment = 0;
                tblPrueba.AddCell(cCeldaTotal);

                PdfPTable TableValorqr = new PdfPTable(1);
                // TableValorqr.WidthPercentage = 100;
                float[] celdasqr = new float[] { 3.0F };
                TableValorqr.SetWidths(celdasqr);

                string InfoCodigoBarras = Empresa.RUC + "|" +
                                       imprimir[0].Venta.serie + "-" + imprimir[0].Venta.numero + "|" +
                                       imprimir[0].Venta.fechaEmision + "|" +
                                       imprimir[0].Venta.cliente.Nombre + "|" +
                                       imprimir[0].Venta.cliente.NroDocumento + "|" +
                                      "grabada:" + imprimir[0].Venta.grabada + "|" +
                                      "exonerada:" + imprimir[0].Venta.exonerada + "|" +
                                      "inafecta:" + imprimir[0].Venta.inafecta + "|" +
                                      "gratuita:" + imprimir[0].Venta.gratuita + "|" +
                                      "igv:" + imprimir[0].Venta.igv + "|" +
                                      "total:" + imprimir[0].Venta.total + "|" +
                                      "descuento:" + imprimir[0].Venta.descuento;
                

                byte[] imageByteData = GenerateBarCodeZXing(InfoCodigoBarras);

                System.Drawing.Image imagen = byteArrayToImage(imageByteData);
                iTextSharp.text.Image pdfImage = iTextSharp.text.Image.GetInstance(imagen, System.Drawing.Imaging.ImageFormat.Jpeg);
                pdfImage.ScaleToFit(90.0F, 70.0F);


                PdfPCell celImg2 = new PdfPCell(pdfImage);
                celImg2.HorizontalAlignment = 0;
                celImg2.Border = 0;
                TableValorqr.AddCell(celImg2);

                PdfPCell cDigesValue = new PdfPCell(new Phrase(imprimir[0].Venta.DigestValue, _standardFont));
                cDigesValue.Border = 0;
                cDigesValue.Padding = 2;
                cDigesValue.PaddingBottom = 2;
                cDigesValue.HorizontalAlignment = 0;
                cDigesValue.Colspan = 1;
                TableValorqr.AddCell(cDigesValue);

                PdfPCell cResolucion = new PdfPCell(new Phrase(imprimir[0].Venta.resolucion, _standardFont));
                cResolucion.Border = 0;
                cResolucion.Padding = 2;
                cResolucion.PaddingBottom = 2;
                cResolucion.HorizontalAlignment = 0;
                cResolucion.Colspan = 1;
                TableValorqr.AddCell(cResolucion);

                PdfPCell cCeldaValorss = new PdfPCell(TableValorqr);
                cCeldaValorss.Colspan = 0;
                cCeldaValorss.Border = 0;
                cCeldaValorss.PaddingBottom = 4;
                tblPrueba.AddCell(cCeldaValorss);

                PdfPTable TableValor = new PdfPTable(2);
                TableValor.TotalWidth = 225.0F;
                TableValor.LockedWidth = true;
                TableValor.SpacingBefore = 180.0F;
                TableValor.HorizontalAlignment = Element.ALIGN_RIGHT;
                //TableValor.WidthPercentage = 100;

                float[] celdas = new float[] { 1.0F, 1.0F };
                TableValor.SetWidths(celdas);

                PdfPCell cTituloSub = new PdfPCell(new Phrase("Ope. Gravada", _standarTextoNegrita));
                cTituloSub.Border = 0;
                // cTituloSub.Colspan = 4;
                cTituloSub.HorizontalAlignment = 0; // 0 = izquierda, 1 = centro, 2 = derecha
                cTituloSub.BorderWidthLeft = 1f;
                cTituloSub.BorderWidthTop = 1f;
                cTituloSub.Padding = 2;
                TableValor.AddCell(cTituloSub);

                PdfPCell cCeldaSubtotal = new PdfPCell(new Phrase(string.Format("{0:n}", imprimir[0].Venta.grabada), _standarTextoNegrita));
                cCeldaSubtotal.Border = 0;
                // cCeldaSubtotal.Colspan = 2
                cCeldaSubtotal.HorizontalAlignment = 2;
                cCeldaSubtotal.BorderWidthRight = 1f;
                cCeldaSubtotal.BorderWidthTop = 1f;
                cCeldaSubtotal.Padding = 2;
                TableValor.AddCell(cCeldaSubtotal);

                PdfPCell cexonerada = new PdfPCell(new Phrase("Ope. Exonerada ", _standarTextoNegrita));
                cexonerada.Border = 0;
                // cTituloSub.Colspan = 4;
                cexonerada.HorizontalAlignment = 0; // 0 = izquierda, 1 = centro, 2 = derecha
                cexonerada.BorderWidthLeft = 1f;
                //  cexonerada.BorderWidthTop = 1f;
                cexonerada.Padding = 2;
                TableValor.AddCell(cexonerada);

                PdfPCell celexonerada = new PdfPCell(new Phrase(string.Format("{0:n}", imprimir[0].Venta.exonerada), _standarTextoNegrita));
                celexonerada.Border = 0;
                // cCeldaSubtotal.Colspan = 2
                celexonerada.HorizontalAlignment = 2;
                celexonerada.BorderWidthRight = 1f;
                //  celexonerada.BorderWidthTop = 1f;
                celexonerada.Padding = 2;
                TableValor.AddCell(celexonerada);

                PdfPCell cinafecta = new PdfPCell(new Phrase("Ope. Inafecta ", _standarTextoNegrita));
                cinafecta.Border = 0;
                // cTituloSub.Colspan = 4;
                cinafecta.HorizontalAlignment = 0; // 0 = izquierda, 1 = centro, 2 = derecha
                cinafecta.BorderWidthLeft = 1f;
                // cinafecta.BorderWidthTop = 1f;
                cinafecta.Padding = 2;
                TableValor.AddCell(cinafecta);

                PdfPCell cCelinafecta = new PdfPCell(new Phrase(string.Format("{0:n}", imprimir[0].Venta.inafecta), _standarTextoNegrita));
                cCelinafecta.Border = 0;
                // cCeldaSubtotal.Colspan = 2
                cCelinafecta.HorizontalAlignment = 2;
                cCelinafecta.BorderWidthRight = 1f;
                // cCelinafecta.BorderWidthTop = 1f;
                cCelinafecta.Padding = 2;
                TableValor.AddCell(cCelinafecta);


                PdfPCell cgratuita = new PdfPCell(new Phrase("Ope. Gratuita ", _standarTextoNegrita));
                cgratuita.Border = 0;
                // cTituloSub.Colspan = 4;
                cgratuita.HorizontalAlignment = 0; // 0 = izquierda, 1 = centro, 2 = derecha
                cgratuita.BorderWidthLeft = 1f;
                // cinafecta.BorderWidthTop = 1f;
                cgratuita.Padding = 2;
                TableValor.AddCell(cgratuita);

                PdfPCell cCelcgratuita = new PdfPCell(new Phrase(string.Format("{0:n}", imprimir[0].Venta.gratuita), _standarTextoNegrita));
                cCelcgratuita.Border = 0;
                // cCeldaSubtotal.Colspan = 2
                cCelcgratuita.HorizontalAlignment = 2;
                cCelcgratuita.BorderWidthRight = 1f;
                // cCelinafecta.BorderWidthTop = 1f;
                cCelcgratuita.Padding = 2;
                TableValor.AddCell(cCelcgratuita);


                PdfPCell cdescuento = new PdfPCell(new Phrase("Total Descuento", _standarTextoNegrita));
                cdescuento.Border = 0;
                // cTituloSub.Colspan = 4;
                cdescuento.HorizontalAlignment = 0; // 0 = izquierda, 1 = centro, 2 = derecha
                cdescuento.BorderWidthLeft = 1f;
                //cdescuento.BorderWidthTop = 1f;
                cdescuento.Padding = 2;
                TableValor.AddCell(cdescuento);

                PdfPCell celcdescuento = new PdfPCell(new Phrase(string.Format("{0:n}", imprimir[0].Venta.descuento), _standarTextoNegrita));
                celcdescuento.Border = 0;
                // cCeldaSubtotal.Colspan = 2
                celcdescuento.HorizontalAlignment = 2;
                celcdescuento.BorderWidthRight = 1f;
                // celcdescuento.BorderWidthTop = 1f;
                celcdescuento.Padding = 2;
                TableValor.AddCell(celcdescuento);


                PdfPCell cTituloIgv = new PdfPCell(new Phrase("Total I.G.V (18%)", _standarTextoNegrita));
                cTituloIgv.Border = 0;
                // cTituloIgv.Colspan = 4;
                cTituloIgv.HorizontalAlignment = 0; // 0 = izquierda, 1 = centro, 2 = derecha
                cTituloIgv.BorderWidthLeft = 1f;
                cTituloIgv.Padding = 2;
                TableValor.AddCell(cTituloIgv);

                PdfPCell cCeldaIgv = new PdfPCell(new Phrase(string.Format("{0:n}", imprimir[0].Venta.igv), _standarTextoNegrita));
                cCeldaIgv.Border = 0;
                cCeldaIgv.HorizontalAlignment = 2;
                cCeldaIgv.BorderWidthRight = 1f;
                cCeldaIgv.Padding = 2;
                TableValor.AddCell(cCeldaIgv);

                PdfPCell cTituloImporte = new PdfPCell(new Phrase("IMPORTE TOTAL ", _standarTextoNegrita));
                cTituloImporte.Border = 0;
                // cTituloImporte.Colspan = 4;
                cTituloImporte.HorizontalAlignment = 0; // 0 = izquierda, 1 = centro, 2 = derecha
                cTituloImporte.BorderWidthLeft = 1f;
                cTituloImporte.Padding = 2;
                TableValor.AddCell(cTituloImporte);

                PdfPCell cCeldaTotalDoc = new PdfPCell(new Phrase(string.Format("{0:n}", imprimir[0].Venta.total), _standarTextoNegrita));
                cCeldaTotalDoc.Border = 0;
                cCeldaTotalDoc.HorizontalAlignment = 2;
                cCeldaTotalDoc.BorderWidthRight = 1f;
                cCeldaTotalDoc.Padding = 2;
                TableValor.AddCell(cCeldaTotalDoc);

                //costo envio
                PdfPCell cTituloCostoEnvio = new PdfPCell(new Phrase("Costo envío ", _standarTextoNegrita));
                cTituloCostoEnvio.Border = 0;
                cTituloCostoEnvio.HorizontalAlignment = 0;
                cTituloCostoEnvio.BorderWidthLeft = 1f;
                cTituloCostoEnvio.Padding = 2;
                TableValor.AddCell(cTituloCostoEnvio);

                PdfPCell cCeldaCostoEnvio = new PdfPCell(new Phrase(string.Format("{0:n}", imprimir[0].Venta.CostoEnvio), _standarTextoNegrita));
                cCeldaCostoEnvio.Border = 0;
                cCeldaCostoEnvio.HorizontalAlignment = 2;
                cCeldaCostoEnvio.BorderWidthRight = 1f;
                cCeldaCostoEnvio.Padding = 2;
                TableValor.AddCell(cCeldaCostoEnvio);

                //TOTAL A PAGAR
                PdfPCell cTituloTotPagar = new PdfPCell(new Phrase("TOTAL A PAGAR ", _standarTextoNegrita));
                cTituloTotPagar.Border = 0;
                cTituloTotPagar.HorizontalAlignment = 0;
                cTituloTotPagar.BorderWidthLeft = 1f;
                cTituloTotPagar.Padding = 2;
                TableValor.AddCell(cTituloTotPagar);

                PdfPCell cCeldaTotPagar = new PdfPCell(new Phrase(string.Format("{0:n}", (imprimir[0].Venta.total + imprimir[0].Venta.CostoEnvio)), _standarTextoNegrita));
                cCeldaTotPagar.Border = 0;
                cCeldaTotPagar.HorizontalAlignment = 2;
                cCeldaTotPagar.BorderWidthRight = 1f;
                cCeldaTotPagar.Padding = 2;
                TableValor.AddCell(cCeldaTotPagar);

                //
                PdfPCell cTituloMoneda = new PdfPCell(new Phrase("MONEDA", _standarTextoNegrita));
                cTituloMoneda.Border = 0;
                // cTituloMoneda.Colspan = 4;
                cTituloMoneda.HorizontalAlignment = 0; // 0 = izquierda, 1 = centro, 2 = derecha
                cTituloMoneda.BorderWidthLeft = 1f;
                cTituloMoneda.BorderWidthBottom = 1f;
                cTituloMoneda.Padding = 2;
                cTituloMoneda.MinimumHeight = 15;
                TableValor.AddCell(cTituloMoneda);

                PdfPCell cCeldaMoneda = new PdfPCell(new Phrase(imprimir[0].Venta.moneda.Nombre, _standarTextoNegrita));
                cCeldaMoneda.Border = 0;
                cCeldaMoneda.HorizontalAlignment = 2;
                cCeldaMoneda.BorderWidthRight = 1f;
                cCeldaMoneda.BorderWidthBottom = 1f;
                cCeldaMoneda.Padding = 2;
                cCeldaMoneda.MinimumHeight = 15;
                TableValor.AddCell(cCeldaMoneda);

                PdfPCell cCeldaValor = new PdfPCell(TableValor);
                cCeldaValor.Colspan = 5;
                cCeldaValor.Border = 0;
                cCeldaValor.PaddingBottom = 4;
                tblPrueba.AddCell(cCeldaValor);

                //Generar Codigo de barra

                pdfDoc.Add(tblPrueba);

                // Close your PDF 
                pdfDoc.Close();

                byte[] bysStream = ns.ToArray();
                ns = new MemoryStream();
                ns.Write(bysStream, 0, bysStream.Length);
                ns.Position = 0;
                if (Envio == 0)//enviar documento
                {
                    var asunto = "Gracias por su compra";
                    var mensaje = "Envió automático de comprobante de venta";

                    var archivo = path;
                    if (imprimir[0].Venta.cliente.Email != "-" && imprimir[0].Venta.cliente.Email != "")
                    {
                        EnviarCorreo.SendMailFactura(asunto, mensaje, imprimir[0].Venta.cliente.Email, archivo, Empresa.Correo, Empresa.Nombre, Empresa.Contrasenia);
                    }

                }
                else
                {
                    System.IO.File.Delete(path);
                }

                // System.IO.File.Delete(path);


                return new FileStreamResult(ns, "application/pdf");
                //return View();
            }
            catch (Exception Exception)
            {
                throw Exception;
            }

        }
        #endregion

        #region tikect
        public ActionResult ImprimirFacturaBolTikect(int Id)
        {
            //Armar documento
            Rectangle rec = new Rectangle(500, 3000);
            Document pdfDoc = new Document(rec, 5, 5, 5, 5);
            MemoryStream ns = new MemoryStream();
            int IdEmpresa = Authentication.UserLogued.Empresa.Id;
            EEmpresa Empresa = Mantenimiento.ListaEditEmpresa(IdEmpresa);
            List<EVentaDetalle> imprimir = Venta.ImprimirFacBol(Id, IdEmpresa);

            try
            {
                //Permite visualizar el contenido del documento que es descargado en "Mis descargas"
                PdfWriter.GetInstance(pdfDoc, System.Web.HttpContext.Current.Response.OutputStream);
                //Ruta dentro del servidor, aqui se guardara el PDF
                var path = Server.MapPath("/") + "Reporte/Venta/" + "Documento_" + imprimir[0].Venta.serie + "-" + imprimir[0].Venta.numero + "BaucherFac" + ".pdf";

                // Se utiliza system.IO para crear o sobreescribe el archivo si existe
                FileStream file = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);

                //'iTextSharp para escribir en el documento PDF, sin esto no podemos ver el contenido
                PdfWriter.GetInstance(pdfDoc, file);
                PdfWriter pw = PdfWriter.GetInstance(pdfDoc, ns);
                //  'Open PDF Document to write data 
                pdfDoc.Open();
                //'Armando el diseño de la factura

                Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 22, iTextSharp.text.Font.BOLD, Color.BLACK);
                Font _standarTitulo = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 25, iTextSharp.text.Font.BOLD, Color.BLACK);
                Font _standarTexto = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 22, iTextSharp.text.Font.NORMAL, Color.BLACK);
                Font _standarLetras = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 16, iTextSharp.text.Font.BOLD, Color.BLACK);
                PdfPTable tblPrueba = new PdfPTable(2);
                tblPrueba.WidthPercentage = 100;
                PdfPTable tblPrueba1 = new PdfPTable(1);
                tblPrueba1.WidthPercentage = 100;

                PdfPTable TableCabecera = new PdfPTable(1);
                TableCabecera.WidthPercentage = 100;



                PdfPTable TableQR = new PdfPTable(1);
                TableQR.WidthPercentage = 100;


                float[] celdasQR = new float[] { 2.0F };
                TableQR.SetWidths(celdasQR);


                string imagePath = "";
                if (Empresa.Logo == "")
                {
                    imagePath = Server.MapPath("/assets/images/LogoDefault.PNG");
                }
                else
                {
                    imagePath = Server.MapPath("/Imagenes/Empresa/" + Empresa.Logo);
                }

                iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imagePath);
                jpg.ScaleToFit(360.0F, 200.0F);

                PdfPTable tableIzq = new PdfPTable(1);
                PdfPCell celImg = new PdfPCell(jpg);
                celImg.HorizontalAlignment = 1;
                celImg.Border = 0;
                TableCabecera.AddCell(celImg);

                //PdfPTable tableIzq = new PdfPTable(1);

                PdfPCell CelRu = new PdfPCell(new Phrase(Empresa.RUC, _standarTitulo));
                CelRu.HorizontalAlignment = 1;
                CelRu.Border = 0;
                tableIzq.AddCell(CelRu);

                PdfPCell CelRazon = new PdfPCell(new Phrase(Empresa.Nombre, _standarTitulo));
                CelRazon.HorizontalAlignment = 1;
                CelRazon.Border = 0;
                tableIzq.AddCell(CelRazon);


                PdfPCell CelDireccion = new PdfPCell(new Phrase(Empresa.Direccion, _standarTitulo));
                CelDireccion.HorizontalAlignment = 1;
                CelDireccion.Border = 0;
                tableIzq.AddCell(CelDireccion);

                PdfPCell celSucursal = new PdfPCell(new Phrase("SUCURSAL: " + imprimir[0].Sucursal.Nombre, _standarTitulo));
                celSucursal.HorizontalAlignment = 1;
                celSucursal.Border = 0;
                tableIzq.AddCell(celSucursal);

                PdfPCell celDireccionSucursal = new PdfPCell(new Phrase("SUCURSAL DIREC.: " + imprimir[0].Sucursal.Direcciones, _standarTitulo));
                celDireccionSucursal.HorizontalAlignment = 1;
                celDireccionSucursal.Border = 0;
                tableIzq.AddCell(celDireccionSucursal);

                PdfPCell celIzq = new PdfPCell(tableIzq);
                celIzq.Border = 0;
                TableCabecera.AddCell(celIzq);


                PdfPCell cCeldaDetallec = new PdfPCell(TableCabecera);
                cCeldaDetallec.Colspan = 2;
                cCeldaDetallec.Border = 0;
                cCeldaDetallec.BorderWidthBottom = 0;
                tblPrueba.AddCell(cCeldaDetallec);



                PdfPCell cCeldaValorLinea = new PdfPCell();
                cCeldaValorLinea.Colspan = 2;
                cCeldaValorLinea.Border = 0;
                cCeldaValorLinea.BorderWidthBottom = 1;
                tblPrueba1.AddCell(cCeldaValorLinea);



                float[] celdas0 = new float[] { 2.0F };
                tblPrueba1.SetWidths(celdas0);

                PdfPCell cTitulo1 = new PdfPCell(new Phrase(imprimir[0].Comprobante.Nombre + "  " + imprimir[0].Venta.serie + "-" + imprimir[0].Venta.numero, _standardFont));
                cTitulo1.Border = 0;
                cTitulo1.HorizontalAlignment = 1;
                cTitulo1.PaddingTop = 2;
                tblPrueba1.AddCell(cTitulo1);

                PdfPCell cTitulo4 = new PdfPCell(new Phrase("Fecha Emisión: " + imprimir[0].Venta.fechaEmision, _standardFont));
                cTitulo4.Border = 0;
                cTitulo4.HorizontalAlignment = 1;
                tblPrueba1.AddCell(cTitulo4);

                PdfPCell cecenttro = new PdfPCell(tblPrueba1);
                cecenttro.Border = 0;
                TableCabecera.AddCell(cecenttro);

                float[] celdas1 = new float[] { 2.0F, 5.0F };
                tblPrueba.SetWidths(celdas1);


                PdfPCell cTitulC = new PdfPCell(new Phrase("Ruc Clinte: ", _standardFont));
                cTitulC.Border = 0;
                cTitulC.HorizontalAlignment = 0;
                tblPrueba.AddCell(cTitulC);

                PdfPCell cTituloDoc = new PdfPCell(new Phrase(imprimir[0].Venta.cliente.NroDocumento, _standardFont));
                cTituloDoc.Border = 0;
                cTituloDoc.HorizontalAlignment = 0;
                tblPrueba.AddCell(cTituloDoc);


                PdfPCell cTitulo6 = new PdfPCell(new Phrase("Cliente: ", _standardFont));
                cTitulo6.Border = 0;
                cTitulo6.HorizontalAlignment = 0;
                tblPrueba.AddCell(cTitulo6);

                PdfPCell TituloCliente = new PdfPCell(new Phrase(imprimir[0].Venta.cliente.Nombre, _standardFont));
                TituloCliente.Border = 0;
                TituloCliente.HorizontalAlignment = 0;
                tblPrueba.AddCell(TituloCliente);

                PdfPCell cTituloDi = new PdfPCell(new Phrase("Dirección: ", _standardFont));
                cTituloDi.Border = 0;
                cTituloDi.HorizontalAlignment = 0;
                tblPrueba.AddCell(cTituloDi);

                PdfPCell TitulDir = new PdfPCell(new Phrase(imprimir[0].Venta.cliente.Direccion, _standardFont));
                TitulDir.Border = 0;
                TitulDir.HorizontalAlignment = 0;
                tblPrueba.AddCell(TitulDir);


                PdfPCell cEspacio = new PdfPCell(new Phrase(" "));
                cEspacio.Colspan = 2;
                cEspacio.Border = 0;
                cEspacio.HorizontalAlignment = 0;
                tblPrueba.AddCell(cEspacio);

                PdfPTable Tableitems = new PdfPTable(6);
                Tableitems.WidthPercentage = 100;

                float[] celdas2 = new float[] { 0.9F, 0.8F, 1.8F, 1.3F, 0.9F, 1.3F };
                Tableitems.SetWidths(celdas2);
                PdfPCell cTituloDesc = new PdfPCell(new Phrase("Cant", _standardFont));
                cTituloDesc.Border = 1;
                cTituloDesc.BorderWidthBottom = 1;
                cTituloDesc.HorizontalAlignment = 1; // 0 = izquierda, 1 = centro, 2 = derecha
                cTituloDesc.Padding = 6;
                Tableitems.AddCell(cTituloDesc);

                PdfPCell cTituloCant = new PdfPCell(new Phrase("Und", _standardFont));
                cTituloCant.Border = 1;
                cTituloCant.BorderWidthBottom = 1;
                cTituloCant.HorizontalAlignment = 1; // 0 = izquierda, 1 = centro, 2 = derecha
                cTituloCant.Padding = 6;
                Tableitems.AddCell(cTituloCant);

                PdfPCell cTituloPrecio = new PdfPCell(new Phrase("Desc", _standardFont));
                cTituloPrecio.Border = 1;
                cTituloPrecio.BorderWidthBottom = 1;
                cTituloPrecio.HorizontalAlignment = 0; // 0 = izquierda, 1 = centro, 2 = derecha
                cTituloPrecio.Padding = 6;
                Tableitems.AddCell(cTituloPrecio);

                PdfPCell cTituloSubT = new PdfPCell(new Phrase("Pre. Unit", _standardFont));
                cTituloSubT.Border = 1;
                cTituloSubT.BorderWidthBottom = 1;
                cTituloSubT.HorizontalAlignment = 1; // 0 = izquierda, 1 = centro, 2 = derecha
                cTituloSubT.Padding = 6;
                Tableitems.AddCell(cTituloSubT);

                PdfPCell cTitulodesc = new PdfPCell(new Phrase("desc", _standardFont));
                cTitulodesc.Border = 1;
                cTitulodesc.BorderWidthBottom = 1;
                cTitulodesc.HorizontalAlignment = 1; // 0 = izquierda, 1 = centro, 2 = derecha
                cTitulodesc.Padding = 6;
                Tableitems.AddCell(cTitulodesc);

                PdfPCell cTituloSubTimport = new PdfPCell(new Phrase("Imp.", _standardFont));
                cTituloSubTimport.Border = 1;
                cTituloSubTimport.BorderWidthBottom = 1;
                cTituloSubTimport.HorizontalAlignment = 1; // 0 = izquierda, 1 = centro, 2 = derecha
                cTituloSubTimport.Padding = 6;
                Tableitems.AddCell(cTituloSubTimport);

                foreach (EVentaDetalle ListaRep in imprimir)
                {

                    PdfPCell cCodigo = new PdfPCell(new Phrase(ListaRep.cantidad.ToString(), _standarTexto));
                    cCodigo.Border = 0;
                    cCodigo.Padding = 2;
                    cCodigo.PaddingBottom = 2;
                    cCodigo.HorizontalAlignment = 1;
                    Tableitems.AddCell(cCodigo);

                    PdfPCell cNombreMat = new PdfPCell(new Phrase(ListaRep.material.Unidad.Nombre, _standarTexto));
                    cNombreMat.Border = 0;
                    cNombreMat.Padding = 2;
                    cNombreMat.PaddingBottom = 2;
                    cNombreMat.HorizontalAlignment = 1;
                    Tableitems.AddCell(cNombreMat);

                    PdfPCell cCantidad = new PdfPCell(new Phrase(ListaRep.material.Nombre, _standarTexto));
                    cCantidad.Border = 0;
                    cCantidad.Padding = 2;
                    cCantidad.PaddingBottom = 2;
                    cCantidad.HorizontalAlignment = 0;
                    Tableitems.AddCell(cCantidad);

                    PdfPCell cPrecio = new PdfPCell(new Phrase(string.Format("{0:n}", ListaRep.precio), _standarTexto));
                    cPrecio.Border = 0;
                    cPrecio.Padding = 2;
                    cPrecio.PaddingBottom = 2;
                    cPrecio.HorizontalAlignment = 1;
                    Tableitems.AddCell(cPrecio);

                    PdfPCell cdescuento = new PdfPCell(new Phrase(string.Format("{0:n}", ListaRep.descuento), _standarTexto));
                    cdescuento.Border = 0;
                    cdescuento.Padding = 2;
                    cdescuento.PaddingBottom = 2;
                    cdescuento.HorizontalAlignment = 1;
                    Tableitems.AddCell(cdescuento);

                    PdfPCell cImporte = new PdfPCell(new Phrase(string.Format("{0:n}", ListaRep.Importe), _standarTexto));
                    cImporte.Border = 0;
                    cImporte.Padding = 2;
                    cImporte.PaddingBottom = 2;
                    cImporte.HorizontalAlignment = 1;
                    Tableitems.AddCell(cImporte);
                }

                PdfPCell cCeldaDetalle = new PdfPCell(Tableitems);
                cCeldaDetalle.Colspan = 6;
                cCeldaDetalle.Border = 0;
                cCeldaDetalle.BorderWidthBottom = 1;
                cCeldaDetalle.PaddingBottom = 6;
                tblPrueba.AddCell(cCeldaDetalle);

                PdfPTable TableValorLetra = new PdfPTable(1);
                TableValorLetra.WidthPercentage = 100;

                PdfPCell cLetras = new PdfPCell(new Phrase("SON: " + Convertir.enletras(imprimir[0].Venta.total.ToString()) + " " + imprimir[0].Venta.moneda.Nombre, _standarLetras));
                cLetras.Border = 0;
                // cLetras.Padding = 6;
                //  cLetras.PaddingBottom = 6;
                cLetras.HorizontalAlignment = 0;
                // cLetras.Colspan = 6;
                TableValorLetra.AddCell(cLetras);


                PdfPCell cCeldaLetras = new PdfPCell(TableValorLetra);
                cCeldaLetras.Colspan = 6;
                cCeldaLetras.Border = 0;
                cCeldaLetras.PaddingBottom = 5;
                tblPrueba.AddCell(cCeldaLetras);


                PdfPCell cCeldaTotal = new PdfPCell(new Phrase("     "));
                cCeldaTotal.Colspan = 6;
                cCeldaTotal.Border = 0;
                cCeldaTotal.HorizontalAlignment = 0;
                tblPrueba.AddCell(cCeldaTotal);

                PdfPTable TableValor = new PdfPTable(2);
                TableValor.WidthPercentage = 100;

                float[] celdas = new float[] { 3.0F, 1.0F };
                TableValor.SetWidths(celdas);

                PdfPCell cTituloSub = new PdfPCell(new Phrase("Ope. GRAVADA:", _standarTexto));
                cTituloSub.Border = 0;
                cTituloSub.HorizontalAlignment = 2; // 0 = izquierda, 1 = centro, 2 = derecha
                TableValor.AddCell(cTituloSub);

                PdfPCell cCeldaSubtotal = new PdfPCell(new Phrase(string.Format("{0:n}", imprimir[0].Venta.grabada), _standarTexto));
                cCeldaSubtotal.Border = 0;
                // cCeldaSubtotal.Colspan = 2
                cCeldaSubtotal.HorizontalAlignment = 2;
                TableValor.AddCell(cCeldaSubtotal);

                PdfPCell cTituloInafecta = new PdfPCell(new Phrase("Ope. INAFECTA:", _standarTexto));
                cTituloInafecta.Border = 0;
                cTituloInafecta.HorizontalAlignment = 2; // 0 = izquierda, 1 = centro, 2 = derecha
                TableValor.AddCell(cTituloInafecta);

                PdfPCell cCeldainafecta = new PdfPCell(new Phrase(string.Format("{0:n}", imprimir[0].Venta.inafecta), _standarTexto));
                cCeldainafecta.Border = 0;
                // cCeldaSubtotal.Colspan = 2
                cCeldainafecta.HorizontalAlignment = 2;
                TableValor.AddCell(cCeldainafecta);

                PdfPCell cTituloExonerada = new PdfPCell(new Phrase("Ope. EXONERADA:", _standarTexto));
                cTituloExonerada.Border = 0;
                cTituloExonerada.HorizontalAlignment = 2; // 0 = izquierda, 1 = centro, 2 = derecha
                TableValor.AddCell(cTituloExonerada);

                PdfPCell cCeldaexonerada = new PdfPCell(new Phrase(string.Format("{0:n}", imprimir[0].Venta.exonerada), _standarTexto));
                cCeldaexonerada.Border = 0;
                cCeldaexonerada.HorizontalAlignment = 2;
                TableValor.AddCell(cCeldaexonerada);


                PdfPCell cTituloGratuita = new PdfPCell(new Phrase("Ope. GRATUITA:", _standarTexto));
                cTituloGratuita.Border = 0;
                cTituloGratuita.HorizontalAlignment = 2; // 0 = izquierda, 1 = centro, 2 = derecha
                TableValor.AddCell(cTituloGratuita);
                PdfPCell cCeldagratuita = new PdfPCell(new Phrase(string.Format("{0:n}", imprimir[0].Venta.gratuita), _standarTexto));
                cCeldagratuita.Border = 0;
                cCeldagratuita.HorizontalAlignment = 2;
                TableValor.AddCell(cCeldagratuita);


                PdfPCell cTituloIgv = new PdfPCell(new Phrase("I.G.V:", _standarTexto));
                cTituloIgv.Border = 0;
                cTituloIgv.HorizontalAlignment = 2; // 0 = izquierda, 1 = centro, 2 = derecha
                TableValor.AddCell(cTituloIgv);

                PdfPCell cCeldaIgv = new PdfPCell(new Phrase(string.Format("{0:n}", imprimir[0].Venta.igv), _standarTexto));
                cCeldaIgv.Border = 0;
                cCeldaIgv.HorizontalAlignment = 2;
                TableValor.AddCell(cCeldaIgv);

                PdfPCell CDESCUENTO = new PdfPCell(new Phrase("DESCUENTO:", _standarTexto));
                CDESCUENTO.Border = 0;
                CDESCUENTO.HorizontalAlignment = 2; // 0 = izquierda, 1 = centro, 2 = derecha
                TableValor.AddCell(CDESCUENTO);

                PdfPCell CVELCDESCUENTO = new PdfPCell(new Phrase(string.Format("{0:n}", imprimir[0].Venta.descuento), _standarTexto));
                CVELCDESCUENTO.Border = 0;
                CVELCDESCUENTO.HorizontalAlignment = 2;
                TableValor.AddCell(CVELCDESCUENTO);


                PdfPCell cTituloImporte = new PdfPCell(new Phrase("IMPORTE TOTAL:", _standarTexto));
                cTituloImporte.Border = 0;
                //cTituloImporte.Colspan = 4;
                cTituloImporte.HorizontalAlignment = 2; // 0 = izquierda, 1 = centro, 2 = derecha
                TableValor.AddCell(cTituloImporte);

                PdfPCell cCeldaTotalDoc = new PdfPCell(new Phrase(string.Format("{0:n}", imprimir[0].Venta.total), _standarTexto));
                cCeldaTotalDoc.Border = 0;
                cCeldaTotalDoc.HorizontalAlignment = 2;
                TableValor.AddCell(cCeldaTotalDoc);

                PdfPCell cTituloMoneda = new PdfPCell(new Phrase("MONEDA:", _standarTexto));
                cTituloMoneda.Border = 0;
                cTituloMoneda.HorizontalAlignment = 2; // 0 = izquierda, 1 = centro, 2 = derecha
                TableValor.AddCell(cTituloMoneda);

                PdfPCell celMoneda = new PdfPCell(new Phrase(string.Format("{0:n}", imprimir[0].Venta.moneda.Nombre), _standarTexto));
                celMoneda.Border = 0;
                celMoneda.HorizontalAlignment = 2;
                TableValor.AddCell(celMoneda);

                PdfPCell cCeldaValor = new PdfPCell(TableValor);
                cCeldaValor.Colspan = 2;
                cCeldaValor.Border = 0;
                cCeldaValor.BorderWidthBottom = 1;
                cCeldaValor.PaddingBottom = 4;
                tblPrueba.AddCell(cCeldaValor);

                //Generar Codigo de barra
                PdfPCell cCeldaEspacio = new PdfPCell(new Phrase("     "));
                cCeldaEspacio.Colspan = 2;
                cCeldaEspacio.Border = 0;
                cCeldaEspacio.BorderWidthBottom = 0;
                tblPrueba.AddCell(cCeldaEspacio);

                string InfoCodigoBarras = Empresa.RUC + "|" +
                                            imprimir[0].Venta.serie + "-" + imprimir[0].Venta.numero + "|" +
                                            imprimir[0].Venta.fechaEmision + "|" +
                                            imprimir[0].Venta.cliente.Nombre + "|" +
                                            imprimir[0].Venta.cliente.NroDocumento + "|" +
                                           "grabada: " + imprimir[0].Venta.grabada + "|" +
                                           "exonerada: " + imprimir[0].Venta.exonerada + "|" +
                                           "inafecta: " + imprimir[0].Venta.inafecta + "|" +
                                           "gratuita: " + imprimir[0].Venta.gratuita + "|" +
                                           "igv: " + imprimir[0].Venta.igv + "|" +
                                           "total: " + imprimir[0].Venta.total + "|" +
                                           "descuento: " + imprimir[0].Venta.descuento;
                byte[] imageByteData = GenerateBarCodeZXing(InfoCodigoBarras);

                System.Drawing.Image imagen = byteArrayToImage(imageByteData);
                Image pdfImage = Image.GetInstance(imagen, ImageFormat.Jpeg);
                pdfImage.ScaleToFit(360.0F, 200.0F);


                PdfPCell celImg2 = new PdfPCell(pdfImage);
                celImg2.HorizontalAlignment = 1;
                celImg2.Border = 0;
                TableQR.AddCell(celImg2);

                PdfPCell cResolucion = new PdfPCell(new Phrase(imprimir[0].Venta.resolucion, _standarTexto));
                cResolucion.Border = 0;
                cResolucion.Padding = 2;
                cResolucion.PaddingBottom = 2;
                cResolucion.HorizontalAlignment = 1;
                TableQR.AddCell(cResolucion);

                PdfPCell cCeldaQR = new PdfPCell(TableQR);
                cCeldaQR.Colspan = 2;
                cCeldaQR.Border = 0;
                cCeldaQR.BorderWidthBottom = 0;
                tblPrueba.AddCell(cCeldaQR);

                pdfDoc.Add(tblPrueba);

                // Close your PDF 
                pdfDoc.Close();

                byte[] bysStream = ns.ToArray();
                ns = new MemoryStream();
                ns.Write(bysStream, 0, bysStream.Length);
                ns.Position = 0;

                System.IO.File.Delete(path);


                return new FileStreamResult(ns, "application/pdf");
            }
            catch (Exception Exception)
            {
                throw Exception;
            }

        }
        #endregion

        #region tikect
        public ActionResult ImprimirFacturaBolTikectPOST(int Id)
        {
            //Armar documento
            Rectangle rec = new Rectangle(500, 3000);
            Document pdfDoc = new Document(rec, 5, 5, 5, 5);
            MemoryStream ns = new MemoryStream();
            int IdEmpresa = Authentication.UserLogued.Empresa.Id;
            EEmpresa Empresa = Mantenimiento.ListaEditEmpresa(IdEmpresa);
            List<EVentaDetalle> imprimir = Venta.ImprimirFacBolPOST(Id, IdEmpresa);

            try
            {
                //Permite visualizar el contenido del documento que es descargado en "Mis descargas"
                PdfWriter.GetInstance(pdfDoc, System.Web.HttpContext.Current.Response.OutputStream);
                //Ruta dentro del servidor, aqui se guardara el PDF
                var path = Server.MapPath("/") + "Reporte/Venta/" + "Documento_" + imprimir[0].Venta.serie + "-" + imprimir[0].Venta.numero + "BaucherFac" + ".pdf";

                // Se utiliza system.IO para crear o sobreescribe el archivo si existe
                FileStream file = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);

                //'iTextSharp para escribir en el documento PDF, sin esto no podemos ver el contenido
                PdfWriter.GetInstance(pdfDoc, file);
                PdfWriter pw = PdfWriter.GetInstance(pdfDoc, ns);
                //  'Open PDF Document to write data 
                pdfDoc.Open();
                //'Armando el diseño de la factura

                Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 22, iTextSharp.text.Font.BOLD, Color.BLACK);
                Font _standarTitulo = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 25, iTextSharp.text.Font.BOLD, Color.BLACK);
                Font _standarTexto = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 22, iTextSharp.text.Font.NORMAL, Color.BLACK);
                Font _standarLetras = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 16, iTextSharp.text.Font.BOLD, Color.BLACK);
                PdfPTable tblPrueba = new PdfPTable(2);
                tblPrueba.WidthPercentage = 100;
                PdfPTable tblPrueba1 = new PdfPTable(1);
                tblPrueba1.WidthPercentage = 100;

                PdfPTable TableCabecera = new PdfPTable(1);
                TableCabecera.WidthPercentage = 100;



                PdfPTable TableQR = new PdfPTable(1);
                TableQR.WidthPercentage = 100;


                float[] celdasQR = new float[] { 2.0F };
                TableQR.SetWidths(celdasQR);


                string imagePath = "";
                //if (Empresa.Logo == "")
                //{
                //    imagePath = Server.MapPath("/assets/images/LogoDefault.PNG");
                //}
                //else
                //{
                //    imagePath = Server.MapPath("/Imagenes/Empresa/" + Empresa.Logo);
                //}
                imagePath = Server.MapPath("/assets/images/LogoDefault.PNG");
                iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imagePath);
                jpg.ScaleToFit(360.0F, 200.0F);

                PdfPTable tableIzq = new PdfPTable(1);
                PdfPCell celImg = new PdfPCell(jpg);
                celImg.HorizontalAlignment = 1;
                celImg.Border = 0;
                TableCabecera.AddCell(celImg);

                //PdfPTable tableIzq = new PdfPTable(1);

                PdfPCell CelRu = new PdfPCell(new Phrase(Empresa.RUC, _standarTitulo));
                CelRu.HorizontalAlignment = 1;
                CelRu.Border = 0;
                tableIzq.AddCell(CelRu);

                PdfPCell CelRazon = new PdfPCell(new Phrase(Empresa.Nombre, _standarTitulo));
                CelRazon.HorizontalAlignment = 1;
                CelRazon.Border = 0;
                tableIzq.AddCell(CelRazon);


                PdfPCell CelDireccion = new PdfPCell(new Phrase(Empresa.Direccion, _standarTitulo));
                CelDireccion.HorizontalAlignment = 1;
                CelDireccion.Border = 0;
                tableIzq.AddCell(CelDireccion);

                PdfPCell celSucursal = new PdfPCell(new Phrase("SUCURSAL: " + imprimir[0].Sucursal.Nombre, _standarTitulo));
                celSucursal.HorizontalAlignment = 1;
                celSucursal.Border = 0;
                tableIzq.AddCell(celSucursal);

                PdfPCell celDireccionSucursal = new PdfPCell(new Phrase("SUCURSAL DIREC.: " + imprimir[0].Sucursal.Direcciones, _standarTitulo));
                celDireccionSucursal.HorizontalAlignment = 1;
                celDireccionSucursal.Border = 0;
                tableIzq.AddCell(celDireccionSucursal);

                PdfPCell celIzq = new PdfPCell(tableIzq);
                celIzq.Border = 0;
                TableCabecera.AddCell(celIzq);


                PdfPCell cCeldaDetallec = new PdfPCell(TableCabecera);
                cCeldaDetallec.Colspan = 2;
                cCeldaDetallec.Border = 0;
                cCeldaDetallec.BorderWidthBottom = 0;
                tblPrueba.AddCell(cCeldaDetallec);



                PdfPCell cCeldaValorLinea = new PdfPCell();
                cCeldaValorLinea.Colspan = 2;
                cCeldaValorLinea.Border = 0;
                cCeldaValorLinea.BorderWidthBottom = 1;
                tblPrueba1.AddCell(cCeldaValorLinea);



                float[] celdas0 = new float[] { 2.0F };
                tblPrueba1.SetWidths(celdas0);

                PdfPCell cTitulo1 = new PdfPCell(new Phrase(imprimir[0].Comprobante.Nombre + "  " + imprimir[0].Venta.serie + "-" + imprimir[0].Venta.numero, _standardFont));
                cTitulo1.Border = 0;
                cTitulo1.HorizontalAlignment = 1;
                cTitulo1.PaddingTop = 2;
                tblPrueba1.AddCell(cTitulo1);

                PdfPCell cTitulo4 = new PdfPCell(new Phrase("Fecha Emisión: " + imprimir[0].Venta.fechaEmision, _standardFont));
                cTitulo4.Border = 0;
                cTitulo4.HorizontalAlignment = 1;
                tblPrueba1.AddCell(cTitulo4);

                PdfPCell cecenttro = new PdfPCell(tblPrueba1);
                cecenttro.Border = 0;
                TableCabecera.AddCell(cecenttro);

                float[] celdas1 = new float[] { 2.0F, 5.0F };
                tblPrueba.SetWidths(celdas1);


                PdfPCell cTitulC = new PdfPCell(new Phrase("Ruc Clinte: ", _standardFont));
                cTitulC.Border = 0;
                cTitulC.HorizontalAlignment = 0;
                tblPrueba.AddCell(cTitulC);

                PdfPCell cTituloDoc = new PdfPCell(new Phrase(imprimir[0].Venta.cliente.NroDocumento, _standardFont));
                cTituloDoc.Border = 0;
                cTituloDoc.HorizontalAlignment = 0;
                tblPrueba.AddCell(cTituloDoc);


                PdfPCell cTitulo6 = new PdfPCell(new Phrase("Cliente: ", _standardFont));
                cTitulo6.Border = 0;
                cTitulo6.HorizontalAlignment = 0;
                tblPrueba.AddCell(cTitulo6);

                PdfPCell TituloCliente = new PdfPCell(new Phrase(imprimir[0].Venta.cliente.Nombre, _standardFont));
                TituloCliente.Border = 0;
                TituloCliente.HorizontalAlignment = 0;
                tblPrueba.AddCell(TituloCliente);

                PdfPCell cTituloDi = new PdfPCell(new Phrase("Dirección: ", _standardFont));
                cTituloDi.Border = 0;
                cTituloDi.HorizontalAlignment = 0;
                tblPrueba.AddCell(cTituloDi);

                PdfPCell TitulDir = new PdfPCell(new Phrase(imprimir[0].Venta.cliente.Direccion, _standardFont));
                TitulDir.Border = 0;
                TitulDir.HorizontalAlignment = 0;
                tblPrueba.AddCell(TitulDir);


                PdfPCell cEspacio = new PdfPCell(new Phrase(" "));
                cEspacio.Colspan = 2;
                cEspacio.Border = 0;
                cEspacio.HorizontalAlignment = 0;
                tblPrueba.AddCell(cEspacio);

                PdfPTable Tableitems = new PdfPTable(6);
                Tableitems.WidthPercentage = 100;

                float[] celdas2 = new float[] { 0.9F, 0.8F, 1.8F, 1.3F, 0.9F, 1.3F };
                Tableitems.SetWidths(celdas2);
                PdfPCell cTituloDesc = new PdfPCell(new Phrase("Cant", _standardFont));
                cTituloDesc.Border = 1;
                cTituloDesc.BorderWidthBottom = 1;
                cTituloDesc.HorizontalAlignment = 1; // 0 = izquierda, 1 = centro, 2 = derecha
                cTituloDesc.Padding = 6;
                Tableitems.AddCell(cTituloDesc);

                PdfPCell cTituloCant = new PdfPCell(new Phrase("Und", _standardFont));
                cTituloCant.Border = 1;
                cTituloCant.BorderWidthBottom = 1;
                cTituloCant.HorizontalAlignment = 1; // 0 = izquierda, 1 = centro, 2 = derecha
                cTituloCant.Padding = 6;
                Tableitems.AddCell(cTituloCant);

                PdfPCell cTituloPrecio = new PdfPCell(new Phrase("Desc", _standardFont));
                cTituloPrecio.Border = 1;
                cTituloPrecio.BorderWidthBottom = 1;
                cTituloPrecio.HorizontalAlignment = 0; // 0 = izquierda, 1 = centro, 2 = derecha
                cTituloPrecio.Padding = 6;
                Tableitems.AddCell(cTituloPrecio);

                PdfPCell cTituloSubT = new PdfPCell(new Phrase("Pre. Unit", _standardFont));
                cTituloSubT.Border = 1;
                cTituloSubT.BorderWidthBottom = 1;
                cTituloSubT.HorizontalAlignment = 1; // 0 = izquierda, 1 = centro, 2 = derecha
                cTituloSubT.Padding = 6;
                Tableitems.AddCell(cTituloSubT);

                PdfPCell cTitulodesc = new PdfPCell(new Phrase("desc", _standardFont));
                cTitulodesc.Border = 1;
                cTitulodesc.BorderWidthBottom = 1;
                cTitulodesc.HorizontalAlignment = 1; // 0 = izquierda, 1 = centro, 2 = derecha
                cTitulodesc.Padding = 6;
                Tableitems.AddCell(cTitulodesc);

                PdfPCell cTituloSubTimport = new PdfPCell(new Phrase("Imp.", _standardFont));
                cTituloSubTimport.Border = 1;
                cTituloSubTimport.BorderWidthBottom = 1;
                cTituloSubTimport.HorizontalAlignment = 1; // 0 = izquierda, 1 = centro, 2 = derecha
                cTituloSubTimport.Padding = 6;
                Tableitems.AddCell(cTituloSubTimport);

                foreach (EVentaDetalle ListaRep in imprimir)
                {

                    PdfPCell cCodigo = new PdfPCell(new Phrase(ListaRep.cantidad.ToString(), _standarTexto));
                    cCodigo.Border = 0;
                    cCodigo.Padding = 2;
                    cCodigo.PaddingBottom = 2;
                    cCodigo.HorizontalAlignment = 1;
                    Tableitems.AddCell(cCodigo);

                    PdfPCell cNombreMat = new PdfPCell(new Phrase(ListaRep.material.Unidad.Nombre, _standarTexto));
                    cNombreMat.Border = 0;
                    cNombreMat.Padding = 2;
                    cNombreMat.PaddingBottom = 2;
                    cNombreMat.HorizontalAlignment = 1;
                    Tableitems.AddCell(cNombreMat);

                    PdfPCell cCantidad = new PdfPCell(new Phrase(ListaRep.material.Nombre, _standarTexto));
                    cCantidad.Border = 0;
                    cCantidad.Padding = 2;
                    cCantidad.PaddingBottom = 2;
                    cCantidad.HorizontalAlignment = 0;
                    Tableitems.AddCell(cCantidad);

                    PdfPCell cPrecio = new PdfPCell(new Phrase(string.Format("{0:n}", ListaRep.precio), _standarTexto));
                    cPrecio.Border = 0;
                    cPrecio.Padding = 2;
                    cPrecio.PaddingBottom = 2;
                    cPrecio.HorizontalAlignment = 1;
                    Tableitems.AddCell(cPrecio);

                    PdfPCell cdescuento = new PdfPCell(new Phrase(string.Format("{0:n}", ListaRep.descuento), _standarTexto));
                    cdescuento.Border = 0;
                    cdescuento.Padding = 2;
                    cdescuento.PaddingBottom = 2;
                    cdescuento.HorizontalAlignment = 1;
                    Tableitems.AddCell(cdescuento);

                    PdfPCell cImporte = new PdfPCell(new Phrase(string.Format("{0:n}", ListaRep.Importe), _standarTexto));
                    cImporte.Border = 0;
                    cImporte.Padding = 2;
                    cImporte.PaddingBottom = 2;
                    cImporte.HorizontalAlignment = 1;
                    Tableitems.AddCell(cImporte);
                }

                PdfPCell cCeldaDetalle = new PdfPCell(Tableitems);
                cCeldaDetalle.Colspan = 6;
                cCeldaDetalle.Border = 0;
                cCeldaDetalle.BorderWidthBottom = 1;
                cCeldaDetalle.PaddingBottom = 6;
                tblPrueba.AddCell(cCeldaDetalle);

                PdfPTable TableValorLetra = new PdfPTable(1);
                TableValorLetra.WidthPercentage = 100;

                PdfPCell cLetras = new PdfPCell(new Phrase("SON: " + Convertir.enletras(imprimir[0].Venta.total.ToString()) + " " + imprimir[0].Venta.moneda.Nombre, _standarLetras));
                cLetras.Border = 0;
                // cLetras.Padding = 6;
                //  cLetras.PaddingBottom = 6;
                cLetras.HorizontalAlignment = 0;
                // cLetras.Colspan = 6;
                TableValorLetra.AddCell(cLetras);


                PdfPCell cCeldaLetras = new PdfPCell(TableValorLetra);
                cCeldaLetras.Colspan = 6;
                cCeldaLetras.Border = 0;
                cCeldaLetras.PaddingBottom = 5;
                tblPrueba.AddCell(cCeldaLetras);


                PdfPCell cCeldaTotal = new PdfPCell(new Phrase("     "));
                cCeldaTotal.Colspan = 6;
                cCeldaTotal.Border = 0;
                cCeldaTotal.HorizontalAlignment = 0;
                tblPrueba.AddCell(cCeldaTotal);

                PdfPTable TableValor = new PdfPTable(2);
                TableValor.WidthPercentage = 100;

                float[] celdas = new float[] { 3.0F, 1.0F };
                TableValor.SetWidths(celdas);

                PdfPCell cTituloSub = new PdfPCell(new Phrase("Ope. GRAVADA:", _standarTexto));
                cTituloSub.Border = 0;
                cTituloSub.HorizontalAlignment = 2; // 0 = izquierda, 1 = centro, 2 = derecha
                TableValor.AddCell(cTituloSub);

                PdfPCell cCeldaSubtotal = new PdfPCell(new Phrase(string.Format("{0:n}", imprimir[0].Venta.grabada), _standarTexto));
                cCeldaSubtotal.Border = 0;
                // cCeldaSubtotal.Colspan = 2
                cCeldaSubtotal.HorizontalAlignment = 2;
                TableValor.AddCell(cCeldaSubtotal);

                PdfPCell cTituloInafecta = new PdfPCell(new Phrase("Ope. INAFECTA:", _standarTexto));
                cTituloInafecta.Border = 0;
                cTituloInafecta.HorizontalAlignment = 2; // 0 = izquierda, 1 = centro, 2 = derecha
                TableValor.AddCell(cTituloInafecta);

                PdfPCell cCeldainafecta = new PdfPCell(new Phrase(string.Format("{0:n}", imprimir[0].Venta.inafecta), _standarTexto));
                cCeldainafecta.Border = 0;
                // cCeldaSubtotal.Colspan = 2
                cCeldainafecta.HorizontalAlignment = 2;
                TableValor.AddCell(cCeldainafecta);

                PdfPCell cTituloExonerada = new PdfPCell(new Phrase("Ope. EXONERADA:", _standarTexto));
                cTituloExonerada.Border = 0;
                cTituloExonerada.HorizontalAlignment = 2; // 0 = izquierda, 1 = centro, 2 = derecha
                TableValor.AddCell(cTituloExonerada);

                PdfPCell cCeldaexonerada = new PdfPCell(new Phrase(string.Format("{0:n}", imprimir[0].Venta.exonerada), _standarTexto));
                cCeldaexonerada.Border = 0;
                cCeldaexonerada.HorizontalAlignment = 2;
                TableValor.AddCell(cCeldaexonerada);


                PdfPCell cTituloGratuita = new PdfPCell(new Phrase("Ope. GRATUITA:", _standarTexto));
                cTituloGratuita.Border = 0;
                cTituloGratuita.HorizontalAlignment = 2; // 0 = izquierda, 1 = centro, 2 = derecha
                TableValor.AddCell(cTituloGratuita);
                PdfPCell cCeldagratuita = new PdfPCell(new Phrase(string.Format("{0:n}", imprimir[0].Venta.gratuita), _standarTexto));
                cCeldagratuita.Border = 0;
                cCeldagratuita.HorizontalAlignment = 2;
                TableValor.AddCell(cCeldagratuita);


                PdfPCell cTituloIgv = new PdfPCell(new Phrase("I.G.V:", _standarTexto));
                cTituloIgv.Border = 0;
                cTituloIgv.HorizontalAlignment = 2; // 0 = izquierda, 1 = centro, 2 = derecha
                TableValor.AddCell(cTituloIgv);

                PdfPCell cCeldaIgv = new PdfPCell(new Phrase(string.Format("{0:n}", imprimir[0].Venta.igv), _standarTexto));
                cCeldaIgv.Border = 0;
                cCeldaIgv.HorizontalAlignment = 2;
                TableValor.AddCell(cCeldaIgv);

                PdfPCell CDESCUENTO = new PdfPCell(new Phrase("DESCUENTO:", _standarTexto));
                CDESCUENTO.Border = 0;
                CDESCUENTO.HorizontalAlignment = 2; // 0 = izquierda, 1 = centro, 2 = derecha
                TableValor.AddCell(CDESCUENTO);

                PdfPCell CVELCDESCUENTO = new PdfPCell(new Phrase(string.Format("{0:n}", imprimir[0].Venta.descuento), _standarTexto));
                CVELCDESCUENTO.Border = 0;
                CVELCDESCUENTO.HorizontalAlignment = 2;
                TableValor.AddCell(CVELCDESCUENTO);


                PdfPCell cTituloImporte = new PdfPCell(new Phrase("IMPORTE TOTAL:", _standarTexto));
                cTituloImporte.Border = 0;
                //cTituloImporte.Colspan = 4;
                cTituloImporte.HorizontalAlignment = 2; // 0 = izquierda, 1 = centro, 2 = derecha
                TableValor.AddCell(cTituloImporte);

                PdfPCell cCeldaTotalDoc = new PdfPCell(new Phrase(string.Format("{0:n}", imprimir[0].Venta.total), _standarTexto));
                cCeldaTotalDoc.Border = 0;
                cCeldaTotalDoc.HorizontalAlignment = 2;
                TableValor.AddCell(cCeldaTotalDoc);

                PdfPCell cTituloMoneda = new PdfPCell(new Phrase("MONEDA:", _standarTexto));
                cTituloMoneda.Border = 0;
                cTituloMoneda.HorizontalAlignment = 2; // 0 = izquierda, 1 = centro, 2 = derecha
                TableValor.AddCell(cTituloMoneda);

                PdfPCell celMoneda = new PdfPCell(new Phrase(string.Format("{0:n}", imprimir[0].Venta.moneda.Nombre), _standarTexto));
                celMoneda.Border = 0;
                celMoneda.HorizontalAlignment = 2;
                TableValor.AddCell(celMoneda);

                PdfPCell cCeldaValor = new PdfPCell(TableValor);
                cCeldaValor.Colspan = 2;
                cCeldaValor.Border = 0;
                cCeldaValor.BorderWidthBottom = 1;
                cCeldaValor.PaddingBottom = 4;
                tblPrueba.AddCell(cCeldaValor);

                //Generar Codigo de barra
                PdfPCell cCeldaEspacio = new PdfPCell(new Phrase("     "));
                cCeldaEspacio.Colspan = 2;
                cCeldaEspacio.Border = 0;
                cCeldaEspacio.BorderWidthBottom = 0;
                tblPrueba.AddCell(cCeldaEspacio);

                string InfoCodigoBarras = Empresa.RUC + "|" +
                                            imprimir[0].Venta.serie + "-" + imprimir[0].Venta.numero + "|" +
                                            imprimir[0].Venta.fechaEmision + "|" +
                                            imprimir[0].Venta.cliente.Nombre + "|" +
                                            imprimir[0].Venta.cliente.NroDocumento + "|" +
                                           "grabada: " + imprimir[0].Venta.grabada + "|" +
                                           "exonerada: " + imprimir[0].Venta.exonerada + "|" +
                                           "inafecta: " + imprimir[0].Venta.inafecta + "|" +
                                           "gratuita: " + imprimir[0].Venta.gratuita + "|" +
                                           "igv: " + imprimir[0].Venta.igv + "|" +
                                           "total: " + imprimir[0].Venta.total + "|" +
                                           "descuento: " + imprimir[0].Venta.descuento;
                byte[] imageByteData = GenerateBarCodeZXing(InfoCodigoBarras);

                System.Drawing.Image imagen = byteArrayToImage(imageByteData);
                Image pdfImage = Image.GetInstance(imagen, ImageFormat.Jpeg);
                pdfImage.ScaleToFit(360.0F, 200.0F);


                PdfPCell celImg2 = new PdfPCell(pdfImage);
                celImg2.HorizontalAlignment = 1;
                celImg2.Border = 0;
                TableQR.AddCell(celImg2);

                PdfPCell cResolucion = new PdfPCell(new Phrase(imprimir[0].Venta.resolucion, _standarTexto));
                cResolucion.Border = 0;
                cResolucion.Padding = 2;
                cResolucion.PaddingBottom = 2;
                cResolucion.HorizontalAlignment = 1;
                TableQR.AddCell(cResolucion);

                PdfPCell cCeldaQR = new PdfPCell(TableQR);
                cCeldaQR.Colspan = 2;
                cCeldaQR.Border = 0;
                cCeldaQR.BorderWidthBottom = 0;
                tblPrueba.AddCell(cCeldaQR);

                pdfDoc.Add(tblPrueba);

                // Close your PDF 
                pdfDoc.Close();

                byte[] bysStream = ns.ToArray();
                ns = new MemoryStream();
                ns.Write(bysStream, 0, bysStream.Length);
                ns.Position = 0;

                System.IO.File.Delete(path);


                return new FileStreamResult(ns, "application/pdf");
            }
            catch (Exception Exception)
            {
                throw Exception;
            }

        }
        #endregion

        #region Imprimir nota credito
        public ActionResult ImprimirNotaCreditoDebito(string Codigo, int Envio)
        {

            Document pdfDoc = new Document(PageSize.A4, 10, 10, 20, 20);
            MemoryStream ns = new MemoryStream();
            int IdEmpresa = Authentication.UserLogued.Empresa.Id;
            EEmpresa Empresa = Mantenimiento.ListaEditEmpresa(IdEmpresa);
            int empresa = Authentication.UserLogued.Empresa.Id;
            int sucursal = Authentication.UserLogued.Sucursal.IdSucursal;
            List<ENotaCreditoDebito> debito = new List<ENotaCreditoDebito>();
            debito = Venta.ObtenerCreditoDebito(int.Parse(Codigo.Split('|')[0]), empresa, sucursal, int.Parse(Codigo.Split('|')[1]));

            try
            {


                //Permite visualizar el contenido del documento que es descargado en "Mis descargas"
                PdfWriter.GetInstance(pdfDoc, System.Web.HttpContext.Current.Response.OutputStream);
                //Ruta dentro del servidor, aqui se guardara el PDF 
                var path = Server.MapPath("/") + "Reporte/Venta/pdf" + "_Documento_" + "NotaCredito" + debito[0].Venta.serie + "-" + debito[0].Venta.numero + ".pdf";

                // Se utiliza system.IO para crear o sobreescribe el archivo si existe
                FileStream file = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);

                //'iTextSharp para escribir en el documento PDF, sin esto no podemos ver el contenido
                PdfWriter.GetInstance(pdfDoc, file);
                PdfWriter pw = PdfWriter.GetInstance(pdfDoc, ns);
                //  'Open PDF Document to write data 
                pdfDoc.Open();
                pdfDoc.SetMargins(35f, 25f, 25f, 35f);
                pdfDoc.NewPage();
                //'Armando el diseño de la factura

                iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 8, iTextSharp.text.Font.NORMAL, Color.BLACK);
                iTextSharp.text.Font _standardFontTabla = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 8, iTextSharp.text.Font.BOLD, Color.WHITE);
                iTextSharp.text.Font _standarTitulo = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 12, iTextSharp.text.Font.BOLD, Color.BLACK);
                iTextSharp.text.Font _standarTexto = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 9, iTextSharp.text.Font.BOLD, Color.BLACK);
                iTextSharp.text.Font _standarTextoNegrita = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 10, iTextSharp.text.Font.BOLD, Color.BLACK);


                PdfPTable tblPrueba = new PdfPTable(2);
                tblPrueba.WidthPercentage = 100;

                PdfPTable TableCabecera = new PdfPTable(3);
                TableCabecera.WidthPercentage = 100;

                float[] celdasCabecera = new float[] { 0.1F, 0.4F, 0.2F };
                TableCabecera.SetWidths(celdasCabecera);


                PdfPTable TableQR = new PdfPTable(1);
                TableQR.WidthPercentage = 100;


                float[] celdasQR = new float[] { 2.0F };
                TableQR.SetWidths(celdasQR);

                PdfPCell celCospan = new PdfPCell();
                celCospan.Colspan = 3;
                celCospan.Border = 0;
                TableCabecera.AddCell(celCospan);

                //'AGREGANDO IMAGEN:      

                string imagePath = "";
                if (Empresa.Logo == "")
                {
                    imagePath = Server.MapPath("/assets/images/LogoDefault.PNG");
                }
                else
                {
                    imagePath = Server.MapPath("/Imagenes/Empresa/" + Empresa.Logo);
                }

                iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imagePath);
                jpg.ScaleToFit(90.0F, 100.0F);

                PdfPTable tableIzqigm = new PdfPTable(1);
                PdfPCell celImg = new PdfPCell(jpg);
                celImg.HorizontalAlignment = 0;
                celImg.Border = 0;
                tableIzqigm.AddCell(celImg);

                PdfPCell celIzqs = new PdfPCell(tableIzqigm);
                celIzqs.Border = 0;
                TableCabecera.AddCell(celIzqs);

                PdfPTable tableIzq = new PdfPTable(1);
                tableIzq.TotalWidth = 250.0F;
                tableIzq.LockedWidth = true;
                tableIzq.SpacingBefore = 150.0F;
                tableIzq.HorizontalAlignment = Element.ALIGN_CENTER;




                PdfPCell celInfo = new PdfPCell(new Phrase(Empresa.Nombre, _standarTitulo));
                celInfo.HorizontalAlignment = 1;
                celInfo.Border = 0;
                tableIzq.AddCell(celInfo);

                PdfPCell celContacto = new PdfPCell(new Phrase(Empresa.PaginaWeb, _standarTexto));
                celContacto.HorizontalAlignment = 1;
                celContacto.Border = 0;
                // celContacto.Colspan = 2;
                tableIzq.AddCell(celContacto);

                PdfPCell celDireccion = new PdfPCell(new Phrase(Empresa.Direccion + " " + Empresa.Telefono + "| Celular:" + Empresa.Celular, _standarTexto));
                celDireccion.HorizontalAlignment = 1;
                celDireccion.Border = 0;

                //celDireccion.Colspan = 2;
                tableIzq.AddCell(celDireccion);

                //PdfPCell celTelefono = new PdfPCell(new Phrase("Telf.: " +, _standarTexto));
                //celTelefono.HorizontalAlignment = 1;
                //celTelefono.Border = 0;
                ////celTelefono.Colspan = 2;
                //tableIzq.AddCell(celTelefono);

                PdfPCell celEmail = new PdfPCell(new Phrase("E-mail:" + Empresa.Correo, _standarTexto));
                celEmail.HorizontalAlignment = 1;
                celEmail.Border = 0;
                //celTelefono.Colspan = 2;
                tableIzq.AddCell(celEmail);

                PdfPCell celIzq = new PdfPCell(tableIzq);
                celIzq.Border = 0;
                TableCabecera.AddCell(celIzq);

                PdfPTable tableDer = new PdfPTable(1);



                PdfPCell celEspacio1 = new PdfPCell(new Phrase(""));
                celEspacio1.Border = 0;
                tableDer.AddCell(celEspacio1);

                //


                PdfPCell cRucs = new PdfPCell(new Phrase("RUC - " + Empresa.RUC, _standarTitulo));
                cRucs.Border = 0;
                cRucs.HorizontalAlignment = 1;
                cRucs.BorderWidthRight = 1f;
                cRucs.BorderWidthLeft = 1f;
                cRucs.BorderWidthTop = 1f;
                cRucs.Padding = 8;
                tableDer.AddCell(cRucs);

                PdfPCell cTipoDoc = new PdfPCell(new Phrase(debito[0].TipoDocumento.Nombre + " " + "ELECTRONICA", _standarTitulo));
                cTipoDoc.HorizontalAlignment = 1;
                cTipoDoc.Border = 0;
                cTipoDoc.BorderWidthRight = 1f;
                cTipoDoc.BorderWidthLeft = 1f;
                tableDer.AddCell(cTipoDoc);

                PdfPCell cNumFac = new PdfPCell(new Phrase("N°: " + debito[0].Serie + '-' + debito[0].Numero, _standarTexto));
                cNumFac.Border = 0;
                cNumFac.HorizontalAlignment = 1;
                cNumFac.BorderWidthRight = 1f;
                cNumFac.BorderWidthLeft = 1f;
                cNumFac.BorderWidthBottom = 1f;
                tableDer.AddCell(cNumFac);

                PdfPCell celDer = new PdfPCell(tableDer);
                celDer.Border = 0;
                TableCabecera.AddCell(celDer);

                PdfPCell cCeldaDetallec = new PdfPCell(TableCabecera);
                cCeldaDetallec.Colspan = 2;
                cCeldaDetallec.Border = 0;
                cCeldaDetallec.BorderWidthBottom = 0;
                tblPrueba.AddCell(cCeldaDetallec);

                PdfPCell cEspacio = new PdfPCell(new Phrase("      "));
                cEspacio.Colspan = 2;
                cEspacio.Border = 0;
                cEspacio.HorizontalAlignment = 0;
                tblPrueba.AddCell(cEspacio);

                PdfPTable TableCab = new PdfPTable(2);
                ///TableCab.WidthPercentage = 100;
                float[] celdas1 = new float[] { 0.75F, 2.0F };
                TableCab.SetWidths(celdas1);

                PdfPCell cTitulo2 = new PdfPCell(new Phrase("SEÑOR(ES):", _standardFont));
                cTitulo2.Border = 0;
                cTitulo2.HorizontalAlignment = 0;
                cTitulo2.BorderWidthTop = 1f;
                cTitulo2.BorderWidthLeft = 1f;
                cTitulo2.Padding = 5;
                TableCab.AddCell(cTitulo2);

                PdfPCell cRuc = new PdfPCell(new Phrase(debito[0].cliente.Nombre, _standardFont));
                cRuc.Border = 0;
                cRuc.HorizontalAlignment = 0;
                cRuc.BorderWidthTop = 1f;
                cRuc.BorderWidthRight = 1f;
                cRuc.Padding = 5;
                TableCab.AddCell(cRuc);

                PdfPCell cTituloEspacio = new PdfPCell(new Phrase());
                cTituloEspacio.Border = 0;
                cTituloEspacio.HorizontalAlignment = 0;
                cTituloEspacio.BorderWidthLeft = 1f;
                TableCab.AddCell(cTituloEspacio);

                PdfPCell cRucEspacio = new PdfPCell(new Phrase());
                cRucEspacio.Border = 0;
                cRucEspacio.HorizontalAlignment = 0;
                cRucEspacio.BorderWidthRight = 1f;
                TableCab.AddCell(cRucEspacio);

                PdfPCell cTitulo3 = new PdfPCell(new Phrase("DIRECCION:", _standardFont));
                cTitulo3.Border = 0;
                cTitulo3.HorizontalAlignment = 0;
                cTitulo3.BorderWidthLeft = 1f;
                cTitulo3.Padding = 5;
                TableCab.AddCell(cTitulo3);

                PdfPCell cDireccion = new PdfPCell(new Phrase(debito[0].cliente.Direccion, _standardFont));
                cDireccion.Border = 0;
                cDireccion.HorizontalAlignment = 0;
                cDireccion.BorderWidthRight = 1f;
                cDireccion.Padding = 5;
                TableCab.AddCell(cDireccion);

                PdfPCell cTituloEspacioPago = new PdfPCell(new Phrase());
                cTituloEspacioPago.Border = 0;
                cTituloEspacioPago.HorizontalAlignment = 0;
                cTituloEspacioPago.BorderWidthLeft = 1f;
                TableCab.AddCell(cTituloEspacioPago);

                PdfPCell cRucEspacios = new PdfPCell(new Phrase());
                cRucEspacios.Border = 0;
                cRucEspacios.HorizontalAlignment = 0;
                cRucEspacios.BorderWidthRight = 1f;
                TableCab.AddCell(cRucEspacios);

                PdfPCell cTiPago = new PdfPCell(new Phrase("CONDICIONES DE PAGO:", _standardFont));
                cTiPago.Border = 0;
                cTiPago.HorizontalAlignment = 0;
                cTiPago.BorderWidthLeft = 1f;
                cTiPago.BorderWidthBottom = 1f;
                cTiPago.Padding = 5;
                cTiPago.MinimumHeight = 30;
                TableCab.AddCell(cTiPago);

                PdfPCell cPago = new PdfPCell(new Phrase("CONTADO", _standardFont));
                cPago.Border = 0;
                cPago.HorizontalAlignment = 0;
                cPago.BorderWidthRight = 1f;
                cPago.BorderWidthBottom = 1f;
                cPago.Padding = 5;
                cTiPago.MinimumHeight = 30;
                TableCab.AddCell(cPago);

                PdfPCell cEspacio1 = new PdfPCell(new Phrase());
                cEspacio1.Colspan = 4;
                cEspacio1.Border = 0;
                cEspacio1.HorizontalAlignment = 0;
                //cEspacio1.MinimumHeight = 35;
                TableCab.AddCell(cEspacio1);

                PdfPCell cCeldaDetallecs = new PdfPCell(TableCab);
                cCeldaDetallecs.Colspan = 0;
                cCeldaDetallecs.Border = 0;
                cCeldaDetallecs.PaddingBottom = 2;
                tblPrueba.AddCell(cCeldaDetallecs);

                PdfPTable Tabledoble = new PdfPTable(2);
                float[] celdases = new float[] { 1F, 2.0F };
                Tabledoble.SetWidths(celdases);
                Tabledoble.TotalWidth = 250.0F;
                Tabledoble.LockedWidth = true;
                Tabledoble.SpacingBefore = 200.0F;
                Tabledoble.HorizontalAlignment = Element.ALIGN_RIGHT;


                PdfPCell cTituloFecha = new PdfPCell(new Phrase("FECHA EMISIÓN:", _standardFont));
                cTituloFecha.Border = 0;
                cTituloFecha.HorizontalAlignment = 0;
                cTituloFecha.PaddingTop = 2;
                cTituloFecha.BorderWidthLeft = 1f;
                cTituloFecha.BorderWidthTop = 1f;
                cTituloFecha.BorderWidthBottom = 1f;
                cTituloFecha.Padding = 5;
                Tabledoble.AddCell(cTituloFecha);

                PdfPCell cFchaE = new PdfPCell(new Phrase(debito[0].sFechaEmision, _standardFont));
                cFchaE.Border = 0; // borde cero
                cFchaE.HorizontalAlignment = 0;
                cFchaE.PaddingTop = 2;
                cFchaE.BorderWidthRight = 1f;
                cFchaE.BorderWidthBottom = 1f;
                cFchaE.BorderWidthTop = 1f;
                cFchaE.Padding = 5;
                Tabledoble.AddCell(cFchaE);

                PdfPCell espacio = new PdfPCell(new Phrase(""));
                espacio.Border = 0;
                espacio.HorizontalAlignment = 1; // 0 = izquierda, 1 = centro, 2 = derecha
                espacio.Padding = 5;
                Tabledoble.AddCell(espacio);

                PdfPCell espacio1 = new PdfPCell(new Phrase(""));
                espacio1.Border = 0;
                espacio1.HorizontalAlignment = 1; // 0 = izquierda, 1 = centro, 2 = derecha
                espacio1.Padding = 5;
                Tabledoble.AddCell(espacio1);

                PdfPCell cTitulo1 = new PdfPCell(new Phrase("N° RUC:", _standardFont));
                cTitulo1.Border = 0;
                cTitulo1.HorizontalAlignment = 0;
                cTitulo1.PaddingTop = 2;
                cTitulo1.BorderWidthTop = 1f;
                cTitulo1.BorderWidthLeft = 1f;
                cTitulo1.Padding = 5;
                Tabledoble.AddCell(cTitulo1);

                PdfPCell cRazonsocial = new PdfPCell(new Phrase(debito[0].cliente.NroDocumento, _standardFont));
                cRazonsocial.Border = 0; // borde cero
                cRazonsocial.HorizontalAlignment = 0;
                cRazonsocial.PaddingTop = 2;
                cRazonsocial.BorderWidthTop = 1f;
                cRazonsocial.BorderWidthRight = 1f;
                cRazonsocial.Padding = 5;
                Tabledoble.AddCell(cRazonsocial);

                PdfPCell cTitulocom = new PdfPCell(new Phrase("Numero Referencia: ", _standardFont));
                cTitulocom.BorderWidthBottom = 1;
                cTitulocom.Border = 0;
                cTitulocom.HorizontalAlignment = 0; // 0 = izquierda, 1 = centro, 2 = derecha
                cTitulocom.Padding = 5;
                cTitulocom.BorderWidthLeft = 1f;
                Tabledoble.AddCell(cTitulocom);

                PdfPCell cTitulcr = new PdfPCell(new Phrase(debito[0].Venta.serie + "-" + debito[0].Venta.numero, _standardFont));
                cTitulcr.Border = 0;
                cTitulcr.BorderWidthBottom = 0;
                cTitulcr.HorizontalAlignment = 0; // 0 = izquierda, 1 = centro, 2 = derecha
                cTitulcr.Padding = 5;
                cTitulcr.BorderWidthRight = 1f;
                Tabledoble.AddCell(cTitulcr);

                PdfPCell cTituloCodigos = new PdfPCell(new Phrase("Motivo: ", _standardFont));
                cTituloCodigos.Border = 0;
                cTituloCodigos.HorizontalAlignment = 0; // 0 = izquierda, 1 = centro, 2 = derecha
                cTituloCodigos.Padding = 5;
                cTituloCodigos.BorderWidthLeft = 1f;
                cTituloCodigos.BorderWidthBottom = 1;
                Tabledoble.AddCell(cTituloCodigos);

                PdfPCell cTitul = new PdfPCell(new Phrase(debito[0].Motivo, _standardFont));
                cTitul.Border = 0;
                cTitul.BorderWidthBottom = 1;
                cTitul.HorizontalAlignment = 0; // 0 = izquierda, 1 = centro, 2 = derecha
                cTitul.Padding = 5;
                cTitul.BorderWidthRight = 1f;
                Tabledoble.AddCell(cTitul);

                PdfPCell cEspacio2 = new PdfPCell(new Phrase());
                cEspacio2.Colspan = 4;
                cEspacio2.Border = 0;
                cEspacio2.HorizontalAlignment = 0;
                //cEspacio1.MinimumHeight = 35;
                Tabledoble.AddCell(cEspacio2);

                PdfPCell cCelda = new PdfPCell(Tabledoble);
                cCelda.Colspan = 0;
                cCelda.Border = 0;
                cCelda.PaddingBottom = 2;
                tblPrueba.AddCell(cCelda);

                PdfPTable Tableitems = new PdfPTable(5);
                Tableitems.WidthPercentage = 100;

                float[] celdas2 = new float[] { 1.0F, 1.0F, 2.0F, 1.0F, 1.0F, };
                Tableitems.SetWidths(celdas2);

                PdfPCell cTituloCodigo = new PdfPCell(new Phrase("CANT", _standardFontTabla));
                cTituloCodigo.BorderWidthBottom = 1;
                cTituloCodigo.HorizontalAlignment = 1; // 0 = izquierda, 1 = centro, 2 = derecha
                cTituloCodigo.Padding = 5;
                cTituloCodigo.BackgroundColor = new Color(System.Drawing.Color.MidnightBlue);
                Tableitems.AddCell(cTituloCodigo);

                PdfPCell cTituloDesc = new PdfPCell(new Phrase("UNID", _standardFontTabla));

                cTituloDesc.BorderWidthBottom = 1;
                cTituloDesc.HorizontalAlignment = 1; // 0 = izquierda, 1 = centro, 2 = derecha
                cTituloDesc.Padding = 5;
                cTituloDesc.BackgroundColor = new Color(System.Drawing.Color.MidnightBlue);
                Tableitems.AddCell(cTituloDesc);

                PdfPCell cTituloCant = new PdfPCell(new Phrase("DESCRIPCION", _standardFontTabla));

                cTituloCant.BorderWidthBottom = 1;
                cTituloCant.HorizontalAlignment = 1; // 0 = izquierda, 1 = centro, 2 = derecha
                cTituloCant.Padding = 5;
                cTituloCant.BackgroundColor = new Color(System.Drawing.Color.MidnightBlue);
                Tableitems.AddCell(cTituloCant);

                PdfPCell cTituloPrecio = new PdfPCell(new Phrase("PRECIO UNITARIO", _standardFontTabla));

                cTituloPrecio.BorderWidthBottom = 1;
                cTituloPrecio.HorizontalAlignment = 1; // 0 = izquierda, 1 = centro, 2 = derecha
                cTituloPrecio.Padding = 5;
                cTituloPrecio.BackgroundColor = new Color(System.Drawing.Color.MidnightBlue);
                Tableitems.AddCell(cTituloPrecio);

                PdfPCell cTituloSubT = new PdfPCell(new Phrase("TOTAL", _standardFontTabla));

                cTituloSubT.BorderWidthBottom = 1;
                cTituloSubT.HorizontalAlignment = 1; // 0 = izquierda, 1 = centro, 2 = derecha
                cTituloSubT.Padding = 5;
                cTituloSubT.BackgroundColor = new Color(System.Drawing.Color.MidnightBlue);
                Tableitems.AddCell(cTituloSubT);


                foreach (ENotaCreditoDebito ListaRep in debito)
                {
                    PdfPCell cCodigo = new PdfPCell(new Phrase(ListaRep.cantidad.ToString(), _standardFont));
                    cCodigo.Border = 0;
                    cCodigo.Padding = 2;
                    cCodigo.PaddingBottom = 2;
                    cCodigo.HorizontalAlignment = 1;
                    cCodigo.Border = Rectangle.LEFT_BORDER;
                    Tableitems.AddCell(cCodigo);

                    PdfPCell cNombreMat = new PdfPCell(new Phrase(ListaRep.detalle.Material.Unidad.Nombre, _standardFont));
                    cNombreMat.Border = 0;
                    cNombreMat.Padding = 2;
                    cNombreMat.PaddingBottom = 2;
                    cNombreMat.HorizontalAlignment = 1;
                    cNombreMat.Border = Rectangle.LEFT_BORDER;
                    Tableitems.AddCell(cNombreMat);

                    PdfPCell cCantidad = new PdfPCell(new Phrase(ListaRep.detalle.Material.Nombre, _standardFont));
                    cCantidad.Border = 0;
                    cCantidad.Padding = 2;
                    cCantidad.PaddingBottom = 2;
                    cCantidad.HorizontalAlignment = 0;
                    cCantidad.Border = Rectangle.LEFT_BORDER;
                    Tableitems.AddCell(cCantidad);

                    PdfPCell cPrecio = new PdfPCell(new Phrase(string.Format("{0:n}", ListaRep.detalle.precio), _standardFont));
                    cPrecio.Border = 0;
                    cPrecio.Padding = 2;
                    cPrecio.PaddingBottom = 2;
                    cPrecio.HorizontalAlignment = 1;
                    cPrecio.Border = Rectangle.LEFT_BORDER;
                    Tableitems.AddCell(cPrecio);

                    PdfPCell cImporte = new PdfPCell(new Phrase(string.Format("{0:n}", ListaRep.detalle.Importe), _standardFont));
                    cImporte.Border = 0;
                    cImporte.Padding = 2;
                    cImporte.PaddingBottom = 2;
                    cImporte.HorizontalAlignment = 1;
                    cImporte.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                    Tableitems.AddCell(cImporte);
                }


                PdfPCell cCeldaDetalle = new PdfPCell(Tableitems);
                cCeldaDetalle.Colspan = 4;
                cCeldaDetalle.Border = 0;
                cCeldaDetalle.BorderWidthBottom = 1;
                cCeldaDetalle.PaddingBottom = 4;
                cCeldaDetalle.FixedHeight = 460f;
                tblPrueba.AddCell(cCeldaDetalle);

                PdfPTable TableValorLetra = new PdfPTable(1);
                TableValorLetra.WidthPercentage = 100;

                PdfPCell cLetras = new PdfPCell(new Phrase("SON: " + Convertir.enletras(debito[0].totalVenta.ToString()) + " " + debito[0].Moneda.Nombre, _standardFont));
                cLetras.Border = 0;
                cLetras.Padding = 2;
                cLetras.PaddingBottom = 2;
                cLetras.HorizontalAlignment = 0;
                cLetras.Colspan = 1;
                TableValorLetra.AddCell(cLetras);


                PdfPCell cCeldaLetras = new PdfPCell(TableValorLetra);
                cCeldaLetras.Colspan = 0;
                cCeldaLetras.Border = 0;
                cCeldaLetras.PaddingBottom = 5;
                tblPrueba.AddCell(cCeldaLetras);

                PdfPCell cCeldaTotal = new PdfPCell(new Phrase());
                cCeldaTotal.Colspan = 10;
                cCeldaTotal.Border = 0;
                cCeldaTotal.HorizontalAlignment = 0;
                tblPrueba.AddCell(cCeldaTotal);


                PdfPTable TableValorqr = new PdfPTable(1);
                // TableValorqr.WidthPercentage = 100;
                float[] celdasqr = new float[] { 3.0F };
                TableValorqr.SetWidths(celdasqr);

                string InfoCodigoBarras = Empresa.RUC + "|" +
                                        debito[0].Serie + "|" +
                                        debito[0].sFechaEmision + "|" +
                                        debito[0].cliente.Nombre + "|" +
                                        debito[0].cliente.NroDocumento + "|" +
                                        debito[0].grabada + "|" +
                                        debito[0].exonerada + "|" +
                                        debito[0].inafecta + "|" +
                                        debito[0].igv + "|" +
                                        debito[0].totalVenta + "|";
                byte[] imageByteData = GenerateBarCodeZXing(InfoCodigoBarras);

                System.Drawing.Image imagen = byteArrayToImage(imageByteData);
                iTextSharp.text.Image pdfImage = iTextSharp.text.Image.GetInstance(imagen, System.Drawing.Imaging.ImageFormat.Jpeg);
                pdfImage.ScaleToFit(90.0F, 70.0F);


                PdfPCell celImg2 = new PdfPCell(pdfImage);
                celImg2.HorizontalAlignment = 0;
                celImg2.Border = 0;
                TableValorqr.AddCell(celImg2);


                PdfPCell cCeldaValorss = new PdfPCell(TableValorqr);
                cCeldaValorss.Colspan = 0;
                cCeldaValorss.Border = 0;
                cCeldaValorss.PaddingBottom = 4;
                tblPrueba.AddCell(cCeldaValorss);

                PdfPTable TableValor = new PdfPTable(2);
                TableValor.TotalWidth = 225.0F;
                TableValor.LockedWidth = true;
                TableValor.SpacingBefore = 180.0F;
                TableValor.HorizontalAlignment = Element.ALIGN_RIGHT;
                //TableValor.WidthPercentage = 100;

                float[] celdas = new float[] { 1.0F, 1.0F };
                TableValor.SetWidths(celdas);

                PdfPCell cTituloSub = new PdfPCell(new Phrase("Total Ope. Gravadas ", _standarTextoNegrita));
                cTituloSub.Border = 0;
                // cTituloSub.Colspan = 4;
                cTituloSub.HorizontalAlignment = 0; // 0 = izquierda, 1 = centro, 2 = derecha
                cTituloSub.BorderWidthLeft = 1f;
                cTituloSub.BorderWidthTop = 1f;
                cTituloSub.Padding = 2;
                TableValor.AddCell(cTituloSub);

                PdfPCell cCeldaSubtotal = new PdfPCell(new Phrase(string.Format("{0:n}", debito[0].grabada), _standarTextoNegrita));
                cCeldaSubtotal.Border = 0;
                // cCeldaSubtotal.Colspan = 2
                cCeldaSubtotal.HorizontalAlignment = 2;
                cCeldaSubtotal.BorderWidthRight = 1f;
                cCeldaSubtotal.BorderWidthTop = 1f;
                cCeldaSubtotal.Padding = 2;
                TableValor.AddCell(cCeldaSubtotal);


                PdfPCell cTituloInafecta = new PdfPCell(new Phrase("Total Ope. Inafectas ", _standarTextoNegrita));
                cTituloInafecta.Border = 0;
                //cTituloInafecta.Colspan = 4;
                cTituloInafecta.HorizontalAlignment = 0; // 0 = izquierda, 1 = centro, 2 = derecha
                cTituloInafecta.BorderWidthLeft = 1f;
                cTituloInafecta.Padding = 2;
                TableValor.AddCell(cTituloInafecta);

                PdfPCell cCeldaSubInafecta = new PdfPCell(new Phrase(string.Format("{0:n}", debito[0].inafecta), _standarTextoNegrita));
                cCeldaSubInafecta.Border = 0;
                // cCeldaSubtotal.Colspan = 2
                cCeldaSubInafecta.HorizontalAlignment = 2;
                cCeldaSubInafecta.BorderWidthRight = 1f;
                cCeldaSubInafecta.Padding = 2;
                TableValor.AddCell(cCeldaSubInafecta);


                PdfPCell cTituloExoneradas = new PdfPCell(new Phrase("Total Ope. Exoneradas ", _standarTextoNegrita));
                cTituloExoneradas.Border = 0;
                // cTituloExoneradas.Colspan = 4;
                cTituloExoneradas.HorizontalAlignment = 0; // 0 = izquierda, 1 = centro, 2 = derecha
                cTituloExoneradas.BorderWidthLeft = 1f;
                cTituloExoneradas.Padding = 2;
                TableValor.AddCell(cTituloExoneradas);

                PdfPCell cCeldaSubExonerdas = new PdfPCell(new Phrase(string.Format("{0:n}", debito[0].exonerada), _standarTextoNegrita));
                cCeldaSubExonerdas.Border = 0;
                // cCeldaSubtotal.Colspan = 2
                cCeldaSubExonerdas.HorizontalAlignment = 2;
                cCeldaSubExonerdas.BorderWidthRight = 1f;
                cCeldaSubExonerdas.Padding = 2;
                TableValor.AddCell(cCeldaSubExonerdas);


                PdfPCell cTituloDescuento = new PdfPCell(new Phrase("Total Descuento ", _standarTextoNegrita));
                cTituloDescuento.Border = 0;
                ///cTituloDescuento.Colspan = 4;
                cTituloDescuento.HorizontalAlignment = 0; // 0 = izquierda, 1 = centro, 2 = derecha
                cTituloDescuento.BorderWidthLeft = 1f;
                cTituloDescuento.Padding = 2;
                TableValor.AddCell(cTituloDescuento);

                PdfPCell cCeldaSubDescuento = new PdfPCell(new Phrase(string.Format("{0:n}", debito[0].descuento), _standarTextoNegrita));
                cCeldaSubDescuento.Border = 0;
                // cCeldaSubtotal.Colspan = 2
                cCeldaSubDescuento.HorizontalAlignment = 2;
                cCeldaSubDescuento.BorderWidthRight = 1f;
                cCeldaSubDescuento.Padding = 2;
                TableValor.AddCell(cCeldaSubDescuento);

                PdfPCell cTituloIgv = new PdfPCell(new Phrase("Total I.G.V (18%)", _standarTextoNegrita));
                cTituloIgv.Border = 0;
                // cTituloIgv.Colspan = 4;
                cTituloIgv.HorizontalAlignment = 0; // 0 = izquierda, 1 = centro, 2 = derecha
                cTituloIgv.BorderWidthLeft = 1f;
                cTituloIgv.Padding = 2;
                TableValor.AddCell(cTituloIgv);

                PdfPCell cCeldaIgv = new PdfPCell(new Phrase(string.Format("{0:n}", debito[0].igv), _standarTextoNegrita));
                cCeldaIgv.Border = 0;
                cCeldaIgv.HorizontalAlignment = 2;
                cCeldaIgv.BorderWidthRight = 1f;
                cCeldaIgv.Padding = 2;
                TableValor.AddCell(cCeldaIgv);

                PdfPCell cTituloImporte = new PdfPCell(new Phrase("IMPORTE TOTAL ", _standarTextoNegrita));
                cTituloImporte.Border = 0;
                // cTituloImporte.Colspan = 4;
                cTituloImporte.HorizontalAlignment = 0; // 0 = izquierda, 1 = centro, 2 = derecha
                cTituloImporte.BorderWidthLeft = 1f;
                cTituloImporte.Padding = 2;
                TableValor.AddCell(cTituloImporte);

                PdfPCell cCeldaTotalDoc = new PdfPCell(new Phrase(string.Format("{0:n}", debito[0].totalVenta), _standarTextoNegrita));
                cCeldaTotalDoc.Border = 0;
                cCeldaTotalDoc.HorizontalAlignment = 2;
                cCeldaTotalDoc.BorderWidthRight = 1f;
                cCeldaTotalDoc.Padding = 2;
                TableValor.AddCell(cCeldaTotalDoc);

                //
                PdfPCell cTituloMoneda = new PdfPCell(new Phrase("MONEDA", _standarTextoNegrita));
                cTituloMoneda.Border = 0;
                // cTituloMoneda.Colspan = 4;
                cTituloMoneda.HorizontalAlignment = 0; // 0 = izquierda, 1 = centro, 2 = derecha
                cTituloMoneda.BorderWidthLeft = 1f;
                cTituloMoneda.BorderWidthBottom = 1f;
                cTituloMoneda.Padding = 2;
                cTituloMoneda.MinimumHeight = 15;
                TableValor.AddCell(cTituloMoneda);

                PdfPCell cCeldaMoneda = new PdfPCell(new Phrase(debito[0].Moneda.Nombre, _standarTextoNegrita));
                cCeldaMoneda.Border = 0;
                cCeldaMoneda.HorizontalAlignment = 2;
                cCeldaMoneda.BorderWidthRight = 1f;
                cCeldaMoneda.BorderWidthBottom = 1f;
                cCeldaMoneda.Padding = 2;
                cCeldaMoneda.MinimumHeight = 15;
                TableValor.AddCell(cCeldaMoneda);

                PdfPCell cCeldaValor = new PdfPCell(TableValor);
                cCeldaValor.Colspan = 5;
                cCeldaValor.Border = 0;
                cCeldaValor.PaddingBottom = 4;
                tblPrueba.AddCell(cCeldaValor);

                //Generar Codigo de barra

                pdfDoc.Add(tblPrueba);

                // Close your PDF 
                pdfDoc.Close();

                byte[] bysStream = ns.ToArray();
                ns = new MemoryStream();
                ns.Write(bysStream, 0, bysStream.Length);
                ns.Position = 0;
                if (Envio == 0)//enviar documento
                {
                    var asunto = "Gracias por su compra";
                    var mensaje = "Envió automático de Nota de Crédito";

                    var archivo = path;
                    if (debito[0].Venta.cliente.Email != "-" && debito[0].Venta.cliente.Email != "")
                    {
                        EnviarCorreo.SendMailFactura(asunto, mensaje, debito[0].cliente.Email, archivo, Empresa.Correo, Empresa.Nombre, Empresa.Contrasenia);
                    }

                }
                else
                {
                    System.IO.File.Delete(path);
                }
                return new FileStreamResult(ns, "application/pdf");
            }
            catch (Exception Exception)
            {
                throw Exception;
            }



        }
        #endregion
        private byte[] GenerateBarCodeZXing(string data)
        {
            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new EncodingOptions
                {
                    Width = 100,
                    Height = 100,
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
        public System.Drawing.Image byteArrayToImage(byte[] byteArrayIn)
        {
            using (MemoryStream mStream = new MemoryStream(byteArrayIn))
            {
                return System.Drawing.Image.FromStream(mStream);
            }
        }

    }
}