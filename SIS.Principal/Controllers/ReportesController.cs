using OfficeOpenXml;
using SIS.Business;
using SIS.Entity;
using SIS.Principal.Models;
using System;
using System.Collections.Generic;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIS.Principal.Controllers
{
    public class ReportesController : Controller
    {
        Authentication Authentication = new Authentication();
        BReportes Venta = new BReportes();
        BGeneral General = new BGeneral();
        BMantenimiento Mantenimiento = new BMantenimiento();
        BGestion Gestion = new BGestion();
        // GET: Reportes
        public ActionResult VentasXProducto()
        {
            return View();
        }
        public ActionResult ReporteVentasGeneral()
        {
            return View();
        }
        public ActionResult ProductoMasVendido()
        {
            return View();
        }
        public ActionResult VentasTipoPago()
        {
            return View();
        }

        [HttpPost]
        public void ResumenVentasXProducto(int flag,string filtro, string FechaIncio, string FechaFin, int numPag, int allReg, int Cant, string vendedor, int sucursal)
        {
            try
            {
                int empresa = Authentication.UserLogued.Empresa.Id;
                
                //FLAG = 1 ES VENTAS POS
                if (flag == 1) {
                    //VENTAS POS
                    Utils.Write(
                        ResponseType.JSON,
                        Venta.ResumenVentasXProductoPOS(filtro, empresa,FechaIncio, FechaFin, numPag, allReg, Cant,vendedor,sucursal)
                    );
                }
                else
                {
                    //VENTAS REGULARES
                    //falta realizar
                    Utils.Write(
                        ResponseType.JSON,
                        Venta.ResumenVentasXProductoRegular(filtro, empresa, FechaIncio, FechaFin, numPag, allReg, Cant, vendedor, sucursal)
                    );
                }
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
        public void VentasGenerales(int flag, string filtro, string FechaIncio, string FechaFin, int numPag, int allReg, int Cant,int sucursal)
        {
            try
            {
                int empresa = Authentication.UserLogued.Empresa.Id;
                //int sucursal = Authentication.UserLogued.Sucursal.IdSucursal;
                //FLAG = 1 ES VENTAS POS
                if (flag == 1)
                {
                    //VENTAS POS
                    Utils.Write(
                        ResponseType.JSON,
                        Venta.VentasGeneralesPos(filtro, empresa, sucursal, FechaIncio, FechaFin, numPag, allReg, Cant)
                    );
                }
                else
                {
                    //VENTAS REGULARES
                    //falta realizar
                    Utils.Write(
                        ResponseType.JSON,
                        Venta.VentasGeneralesRegular(filtro, empresa, sucursal, FechaIncio, FechaFin, numPag, allReg, Cant)
                    );
                }
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
        public void VentasPorTipoPago(int flag, string filtro, string FechaIncio, string FechaFin, int numPag, int allReg, int Cant, int sucursal)
        {
            try
            {
                int empresa = Authentication.UserLogued.Empresa.Id;
                //int sucursal = Authentication.UserLogued.Sucursal.IdSucursal;
                //FLAG = 1 ES VENTAS POS
                if (flag == 1)
                {
                    //VENTAS POS
                    Utils.Write(
                        ResponseType.JSON,
                        Venta.VentasPorTipoPagoPos(filtro, empresa, sucursal, FechaIncio, FechaFin, numPag, allReg, Cant)
                    );
                }
                else
                {
                    //VENTAS REGULARES
                    //falta realizar
                    Utils.Write(
                        ResponseType.JSON,
                        Venta.VentasPorTipoPagoRegular(filtro, empresa, sucursal, FechaIncio, FechaFin, numPag, allReg, Cant)
                    );
                }
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
        public void ListaProductoMasVendido()
        {
            try
            {
                int empresa = Authentication.UserLogued.Empresa.Id;
                Utils.Write(
                     ResponseType.JSON,
                     Venta.ListaProductoMasVendido(empresa)
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


        #region Reporte Excel Resumen de ventas por producto
        public ActionResult ResumenExcelVentasXProductos(int venta,string cliente, string FechaIncio, string FechaFin, int numPag, int allReg, int Cant, string vendedor, int sucursal)
        {
            try
            {
                //Ruta de la plantilla
                FileInfo fTemplateFile = new FileInfo(Server.MapPath("/") + "Reporte/Venta/excel/" + "ResumenVentasXproductos.xlsx");

                var Usuario = Authentication.UserLogued.Usuario;
                int Empresa = Authentication.UserLogued.Empresa.Id;
               // int sucursal = Authentication.UserLogued.Sucursal.IdSucursal;
                //Ruta del nuevo documento
                FileInfo fNewFile = new FileInfo(Server.MapPath("/") + "Reporte/Venta/excel/" + "Venta" + "_" + Usuario + ".xlsx");

                List<EVentaDetalle> ListaCompra;


                if (venta == 1)
                {
                    //VENTAS POS
                    ListaCompra = Venta.ResumenVentasXProductoPOS(cliente, Empresa, FechaIncio, FechaFin, numPag, allReg, Cant, vendedor, sucursal);

                }
                else
                {
                    //VENTAS REGULAR
                    ListaCompra = Venta.ResumenVentasXProductoRegular(cliente, Empresa, FechaIncio, FechaFin, numPag, allReg, Cant, vendedor, sucursal);
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
                double cantidad = 0, precio = 0, descuento = 0, precDscto = 0, importe = 0, importeSnIgv=0;

                foreach (EVentaDetalle Detalle in ListaCompra)
                {
                    ws.Cells[iFilaDetIni + starRow, 1].Value = Detalle.Venta.vendedor;
                    ws.Cells[iFilaDetIni + starRow, 2].Value = Detalle.Sucursal.Nombre;
                    ws.Cells[iFilaDetIni + starRow, 3].Value = Detalle.Venta.fechaEmision;
                    ws.Cells[iFilaDetIni + starRow, 4].Value = Detalle.Venta.serie + '-' + Detalle.Venta.numero;
                    ws.Cells[iFilaDetIni + starRow, 5].Value = Detalle.Venta.moneda.Nombre;
                    ws.Cells[iFilaDetIni + starRow, 6].Value = Detalle.Venta.cliente.NroDocumento + '-' + Detalle.Venta.cliente.Nombre;
                    ws.Cells[iFilaDetIni + starRow, 7].Value = Detalle.material.Codigo;
                    ws.Cells[iFilaDetIni + starRow, 8].Value = Detalle.material.Nombre;
                    ws.Cells[iFilaDetIni + starRow, 9].Value = Detalle.material.Unidad.Nombre;
                    ws.Cells[iFilaDetIni + starRow, 10].Value = Detalle.material.Marca.Nombre;
                    ws.Cells[iFilaDetIni + starRow, 11].Value = Detalle.material.Modelo.Nombre;
                    ws.Cells[iFilaDetIni + starRow, 12].Value = Detalle.material.Etipo.Nombre;
                    ws.Cells[iFilaDetIni + starRow, 13].Value = Detalle.material.EColor.Nombre;
                    ws.Cells[iFilaDetIni + starRow, 14].Value = Detalle.material.Talla.Nombre;
                    ws.Cells[iFilaDetIni + starRow, 15].Value = Detalle.material.Temporada.Nombre;
                    ws.Cells[iFilaDetIni + starRow, 16].Value = Detalle.material.Categoria.Nombre;
                    ws.Cells[iFilaDetIni + starRow, 17].Value = Math.Round(Convert.ToDouble(Detalle.cantidad), 2);
                    ws.Cells[iFilaDetIni + starRow, 18].Value = Math.Round(Convert.ToDouble(Detalle.precio), 2);
                    ws.Cells[iFilaDetIni + starRow, 19].Value = Math.Round(Convert.ToDouble(Detalle.descuento), 2);
                    ws.Cells[iFilaDetIni + starRow, 20].Value = Math.Round(Convert.ToDouble(Detalle.precio - Detalle.descuento), 2);
                    ws.Cells[iFilaDetIni + starRow, 21].Value = Math.Round(Convert.ToDouble(Detalle.Importe), 2);
                    ws.Cells[iFilaDetIni + starRow, 22].Value = Math.Round(Convert.ToDouble(Detalle.Importe / 1.18), 3);
                    //ws.Cells[iFilaDetIni + starRow, 22].Value = Math.Round(Convert.ToDouble(Detalle.Venta.CostoEnvio), 2);
                    //ws.Cells[iFilaDetIni + starRow, 23].Value = Detalle.Venta.Envio.Nombre;
                    //ws.Cells[iFilaDetIni + starRow, 24].Value = Detalle.Venta.fechaEnvio;
                    //ws.Cells[iFilaDetIni + starRow, 25].Value = Detalle.Venta.sCanalesVenta;

                    cantidad += Convert.ToDouble(Detalle.cantidad);
                    precio += Convert.ToDouble(Detalle.precio);
                    descuento += Convert.ToDouble(Detalle.descuento);
                    precDscto += Convert.ToDouble(Detalle.precio - Detalle.descuento);
                    importe += Convert.ToDouble(Detalle.Importe);
                    importeSnIgv += Convert.ToDouble(Detalle.Importe / 1.18);
                    
                    //costoEnvio += Convert.ToDouble(Detalle.Venta.CostoEnvio);
                    starRow++;
                }

                int Count = starRow + 1;
                ws.Cells[iFilaDetIni + (Count - 1), 16].Value = "Totales";
                ws.Cells[iFilaDetIni + (Count - 1), 17].Value = Math.Round(Convert.ToDouble(cantidad), 2);
                ws.Cells[iFilaDetIni + (Count - 1), 18].Value = Math.Round(Convert.ToDouble(precio), 2);
                ws.Cells[iFilaDetIni + (Count - 1), 19].Value = Math.Round(Convert.ToDouble(descuento), 2);
                ws.Cells[iFilaDetIni + (Count - 1), 20].Value = Math.Round(Convert.ToDouble(precDscto), 2);
                ws.Cells[iFilaDetIni + (Count - 1), 21].Value = Math.Round(Convert.ToDouble(importe), 2);
                ws.Cells[iFilaDetIni + (Count - 1), 22].Value = Math.Round(Convert.ToDouble(importeSnIgv), 2);

                string modelRange = "A" + System.Convert.ToString(iFilaDetIni) + ":v" + (iFilaDetIni + Count - 1).ToString();
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
                Response.AddHeader("content-disposition", "attachment;  filename=" + "ResumenVentasXproductos" + "_" + Usuario + ".xlsx");

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

        #region Reporte Ventas Generales
        public ActionResult ReporteVentasGeneralesExcel(string cliente, string FechaIncio, string FechaFin, int numPag, int allReg, int Cant, int venta, int sucursal)
        {
            try
            {
                //Ruta de la plantilla
                FileInfo fTemplateFile = new FileInfo(Server.MapPath("/") + "Reporte/Venta/excel/" + "ReporteVentasGeneral.xlsx");

                var Usuario = Authentication.UserLogued.Usuario;
                int Empresa = Authentication.UserLogued.Empresa.Id;
                //int sucursal = Authentication.UserLogued.Sucursal.IdSucursal;
                //Ruta del nuevo documento
                FileInfo fNewFile = new FileInfo(Server.MapPath("/") + "Reporte/Venta/excel/" + "Venta" + "_" + Usuario + ".xlsx");

                List<EVenta> ListaCompra;
                if (venta == 1)
                {
                    ListaCompra = Venta.VentasGeneralesPos(cliente, Empresa, sucursal, FechaIncio, FechaFin, numPag, allReg, Cant);

                }
                else
                {
                    ListaCompra = Venta.VentasGeneralesRegular(cliente, Empresa, sucursal, FechaIncio, FechaFin, numPag, allReg, Cant);
                }
                ExcelPackage pck = new ExcelPackage(fNewFile, fTemplateFile);

                var ws = pck.Workbook.Worksheets[1];
                ////** aqui
                //var path = Server.MapPath("/") + "assets/images/LogoDefault.png";

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
                    ws.Cells[iFilaDetIni + starRow, 1].Value = Detalle.Documento.Nombre; 
                    ws.Cells[iFilaDetIni + starRow, 2].Value = Detalle.serie; 
                    ws.Cells[iFilaDetIni + starRow, 3].Value = Detalle.numero;
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
                Response.AddHeader("content-disposition", "attachment;  filename=" + "ReporteVentasGeneral" + "_" + Usuario + ".xlsx");

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

        #region Reporte Ventas por tipo de pago
        public ActionResult ReporteVentasPorTipoPagoExcel(string filtro, string FechaIncio, string FechaFin, int numPag, int allReg, int Cant, int venta, int sucursal)
        {
            try
            {
                //Ruta de la plantilla
                FileInfo fTemplateFile = new FileInfo(Server.MapPath("/") + "Reporte/Venta/excel/" + "ReporteVentasPorTipoPago.xlsx");

                var Usuario = Authentication.UserLogued.Usuario;
                int Empresa = Authentication.UserLogued.Empresa.Id;
                //int sucursal = Authentication.UserLogued.Sucursal.IdSucursal;
                //Ruta del nuevo documento
                FileInfo fNewFile = new FileInfo(Server.MapPath("/") + "Reporte/Venta/excel/" + "Venta" + "_" + Usuario + ".xlsx");

                List<EVentaDetalle> ListaCompra;
                if (venta == 1)
                {
                    ListaCompra = Venta.VentasPorTipoPagoPos(filtro,Empresa, sucursal, FechaIncio, FechaFin, numPag, allReg, Cant);

                }
                else
                {
                    ListaCompra = Venta.VentasPorTipoPagoRegular(filtro,Empresa, sucursal, FechaIncio, FechaFin, numPag, allReg, Cant);
                }
                ExcelPackage pck = new ExcelPackage(fNewFile, fTemplateFile);

                var ws = pck.Workbook.Worksheets[1];
                ////** aqui
                //var path = Server.MapPath("/") + "assets/images/LogoDefault.png";

                int rowIndex = 0;//numeros
                int colIndex = 3; //letras
                int Height = 220;
                int Width = 120;

                DateTime today = DateTime.Now;
                ws.Cells[6, 2].Value = today;
                ws.Cells[7, 2].Value = Usuario;
                int iFilaDetIni = 10;
                int starRow = 1;
                double monto = 0;
                foreach (EVentaDetalle Detalle in ListaCompra)
                {
                    ws.Cells[iFilaDetIni + starRow, 1].Value = Detalle.Venta.serie;
                    ws.Cells[iFilaDetIni + starRow, 2].Value = Detalle.Venta.numero;
                    ws.Cells[iFilaDetIni + starRow, 3].Value = Detalle.Venta.fechaEmision;
                    ws.Cells[iFilaDetIni + starRow, 4].Value = Detalle.Venta.Text;
                    ws.Cells[iFilaDetIni + starRow, 5].Value = Detalle.Venta.TextBanco;
                    ws.Cells[iFilaDetIni + starRow, 6].Value = Math.Round(Convert.ToDouble(Detalle.precio), 2);
                    ws.Cells[iFilaDetIni + starRow, 7].Value = Detalle.Venta.moneda.Nombre;
                    ws.Cells[iFilaDetIni + starRow, 8].Value = Detalle.Sucursal.Nombre;

                    monto += Convert.ToDouble(Detalle.precio);
                    starRow++;
                }

                int Count = starRow + 1;
                ws.Cells[iFilaDetIni + (Count - 1), 5].Value = "Totales";
                ws.Cells[iFilaDetIni + (Count - 1), 6].Value = Math.Round(Convert.ToDouble(monto), 2);

                string modelRange = "A" + System.Convert.ToString(iFilaDetIni) + ":H" + (iFilaDetIni + Count - 1).ToString();
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
                Response.AddHeader("content-disposition", "attachment;  filename=" + "ReporteVentasPorTipoPago" + "_" + Usuario + ".xlsx");

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

        #region guia
        public ActionResult ImprimirGuia(int Codigo)
        {
            Document pdfDoc = new Document(PageSize.A4, 10, 10, 20, 20);

            int IdEmpresa = Authentication.UserLogued.Empresa.Id;
            EEmpresa Empresa = Mantenimiento.ListaEditEmpresa(IdEmpresa);
            MemoryStream ns = new MemoryStream();
            List<EGuia> oGuia = Gestion.ListaGuiaId(Codigo);

            try
            {
                //Permite visualizar el contenido del documento que es descargado en "Mis descargas"
                PdfWriter.GetInstance(pdfDoc, System.Web.HttpContext.Current.Response.OutputStream);
                //Ruta dentro del servidor, aqui se guardara el PDF
                var path = Server.MapPath("/") + "Reporte/Venta/" + "Documento_" + Codigo + "_OC_" + oGuia[0].serie + ".pdf";

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


                PdfPTable tblPrueba = new PdfPTable(1);
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

                PdfPCell celInfo = new PdfPCell(new Phrase(Empresa.RazonSocial, _standarTitulo));
                celInfo.HorizontalAlignment = 1;
                celInfo.Border = 0;
                tableIzq.AddCell(celInfo);

                PdfPCell celContacto = new PdfPCell(new Phrase("www.biofer.com.pe", _standarTexto));
                celContacto.HorizontalAlignment = 1;
                celContacto.Border = 0;
                // celContacto.Colspan = 2;
                tableIzq.AddCell(celContacto);

                PdfPCell celDireccion = new PdfPCell(new Phrase(Empresa.Direccion + " " + Empresa.Telefono + "| Celular:" + Empresa.Celular, _standarTexto));
                celDireccion.HorizontalAlignment = 1;
                celDireccion.Border = 0;
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

                PdfPCell cTipoDoc = new PdfPCell(new Phrase("GUIA - " + "ELECTRONICA", _standarTitulo));
                cTipoDoc.HorizontalAlignment = 1;
                cTipoDoc.Border = 0;
                cTipoDoc.BorderWidthRight = 1f;
                cTipoDoc.BorderWidthLeft = 1f;
                tableDer.AddCell(cTipoDoc);

                PdfPCell cNumFac = new PdfPCell(new Phrase("N°: " + oGuia[0].serie, _standarTitulo));
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
                TableCab.WidthPercentage = 100;
                float[] celdas1 = new float[] { 1F, 5.0F };
                //TableCab.TotalWidth = 325.0F;
                //TableCab.LockedWidth = true;
                // TableCab.SpacingBefore = 300.0F;
                TableCab.SetWidths(celdas1);
                TableCab.HorizontalAlignment = Element.ALIGN_LEFT;

                PdfPCell cTitulo2 = new PdfPCell(new Phrase("Fecha de Emisión:", _standardFont));
                cTitulo2.Border = 0;
                cTitulo2.HorizontalAlignment = 0;
                cTitulo2.BorderWidthTop = 1f;
                cTitulo2.BorderWidthLeft = 1f;
                cTitulo2.Padding = 5;
                TableCab.AddCell(cTitulo2);

                PdfPCell cRuc = new PdfPCell(new Phrase(oGuia[0].FechaRegistro, _standardFont));
                cRuc.Border = 0;
                cRuc.HorizontalAlignment = 0;
                cRuc.BorderWidthTop = 1f;
                cRuc.BorderWidthRight = 1f;
                cRuc.Padding = 5;
                TableCab.AddCell(cRuc);

                PdfPCell cTitulo3 = new PdfPCell(new Phrase("Direccion de punto de partida:", _standardFont));
                cTitulo3.Border = 0;
                cTitulo3.HorizontalAlignment = 0;
                cTitulo3.BorderWidthLeft = 1f;
                cTitulo3.Padding = 5;
                TableCab.AddCell(cTitulo3);

                PdfPCell cDireccion = new PdfPCell(new Phrase(oGuia[0].DirecionSalida, _standardFont));
                cDireccion.Border = 0;
                cDireccion.HorizontalAlignment = 0;
                cDireccion.BorderWidthRight = 1f;
                cDireccion.Padding = 5;
                TableCab.AddCell(cDireccion);

                PdfPCell cTituloEspacioPago = new PdfPCell(new Phrase("DATOS DEL DESTINATARIO", _standarTexto));
                cTituloEspacioPago.Border = 0;
                cTituloEspacioPago.HorizontalAlignment = 0;
                cTituloEspacioPago.BorderWidthLeft = 1f;
                cTituloEspacioPago.BorderWidthRight = 1f;
                cTituloEspacioPago.Padding = 5;
                cTituloEspacioPago.Colspan = 2;
                TableCab.AddCell(cTituloEspacioPago);




                PdfPCell cRazoncliente = new PdfPCell(new Phrase("Nombre o Razon social:", _standardFont));
                //cRazoncliente.Colspan = 4;
                cRazoncliente.Border = 0;
                cRazoncliente.BorderWidthLeft = 1f;
                cRazoncliente.Padding = 5;
                TableCab.AddCell(cRazoncliente);

                PdfPCell cRazonclientedet = new PdfPCell(new Phrase(oGuia[0].Cliente.Nombre, _standardFont));
                cRazonclientedet.Border = 0;
                cRazonclientedet.HorizontalAlignment = 0;
                cRazonclientedet.BorderWidthRight = 1f;
                cRazonclientedet.Padding = 5;
                TableCab.AddCell(cRazonclientedet);

                PdfPCell cDomicilio = new PdfPCell(new Phrase("Domicilio del punto de llegada:", _standardFont));
                //  cDomicilio.Colspan = 4;
                cDomicilio.Border = 0;
                cDomicilio.HorizontalAlignment = 0;
                cDomicilio.BorderWidthLeft = 1f;
                cDomicilio.Padding = 5;
                TableCab.AddCell(cDomicilio);

                PdfPCell cDomiciliodet = new PdfPCell(new Phrase(oGuia[0].tramo2, _standardFont));
                cDomiciliodet.Border = 0;
                cDomiciliodet.HorizontalAlignment = 0;
                cDomiciliodet.BorderWidthRight = 1f;
                cDomiciliodet.Padding = 5;
                TableCab.AddCell(cDomiciliodet);

                PdfPCell CRUC = new PdfPCell(new Phrase("RUC:", _standardFont));
                // CRUC.Colspan = 4;
                CRUC.Border = 0;
                CRUC.HorizontalAlignment = 0;
                CRUC.Padding = 5;
                CRUC.BorderWidthLeft = 1f;
                TableCab.AddCell(CRUC);

                PdfPCell CRUdt = new PdfPCell(new Phrase(oGuia[0].Cliente.NroDocumento + "               " + "Factura:    " + oGuia[0].Documento.Nombre, _standardFont));
                CRUdt.Border = 0;
                CRUdt.HorizontalAlignment = 0;
                CRUdt.BorderWidthRight = 1f;
                CRUdt.Padding = 5;
                TableCab.AddCell(CRUdt);

                PdfPCell cRucEspacios = new PdfPCell(new Phrase(""));
                cRucEspacios.Border = 0;
                cRucEspacios.HorizontalAlignment = 0;
                cRucEspacios.BorderWidthLeft = 1f;
                cRucEspacios.BorderWidthBottom = 1f;
                cRucEspacios.BorderWidthRight = 1f;
                cRucEspacios.Padding = 5;
                cRucEspacios.Colspan = 2;
                TableCab.AddCell(cRucEspacios);

                PdfPCell cCeldaDetallecs = new PdfPCell(TableCab);
                cCeldaDetallecs.Colspan = 0;
                cCeldaDetallecs.Border = 0;
                cCeldaDetallecs.PaddingBottom = 2;
                tblPrueba.AddCell(cCeldaDetallecs);



                PdfPTable Tableitems = new PdfPTable(7);
                Tableitems.WidthPercentage = 100;

                float[] celdas2 = new float[] { 1.0F, 2.7F, 1.0F, 1.2F, 1.3F, 1.3F, 1.2F };
                Tableitems.SetWidths(celdas2);

                PdfPCell cTituloCodigo = new PdfPCell(new Phrase("CODIGO", _standardFontTabla));
                cTituloCodigo.BorderWidthBottom = 1;
                cTituloCodigo.HorizontalAlignment = 1; // 0 = izquierda, 1 = centro, 2 = derecha
                cTituloCodigo.Padding = 5;
                cTituloCodigo.BackgroundColor = new Color(System.Drawing.Color.MidnightBlue);
                Tableitems.AddCell(cTituloCodigo);

                PdfPCell cTituloDesc = new PdfPCell(new Phrase("PRODUCTO", _standardFontTabla));

                cTituloDesc.BorderWidthBottom = 1;
                cTituloDesc.HorizontalAlignment = 1; // 0 = izquierda, 1 = centro, 2 = derecha
                cTituloDesc.Padding = 5;
                cTituloDesc.BackgroundColor = new Color(System.Drawing.Color.MidnightBlue);
                Tableitems.AddCell(cTituloDesc);

                PdfPCell cTituloCant = new PdfPCell(new Phrase("BULTO", _standardFontTabla));

                cTituloCant.BorderWidthBottom = 1;
                cTituloCant.HorizontalAlignment = 1; // 0 = izquierda, 1 = centro, 2 = derecha
                cTituloCant.Padding = 5;
                cTituloCant.BackgroundColor = new Color(System.Drawing.Color.MidnightBlue);
                Tableitems.AddCell(cTituloCant);

                PdfPCell cTituloPrecio = new PdfPCell(new Phrase("ENVASE", _standardFontTabla));

                cTituloPrecio.BorderWidthBottom = 1;
                cTituloPrecio.HorizontalAlignment = 1; // 0 = izquierda, 1 = centro, 2 = derecha
                cTituloPrecio.Padding = 5;
                cTituloPrecio.BackgroundColor = new Color(System.Drawing.Color.MidnightBlue);
                Tableitems.AddCell(cTituloPrecio);

                PdfPCell cTituloSubT = new PdfPCell(new Phrase("ENPAQUE", _standardFontTabla));
                cTituloSubT.BorderWidthBottom = 1;
                cTituloSubT.HorizontalAlignment = 1; // 0 = izquierda, 1 = centro, 2 = derecha
                cTituloSubT.Padding = 5;
                cTituloSubT.BackgroundColor = new Color(System.Drawing.Color.MidnightBlue);
                Tableitems.AddCell(cTituloSubT);

                PdfPCell cTituloUNIDAD = new PdfPCell(new Phrase("UNIDAD", _standardFontTabla));
                cTituloUNIDAD.BorderWidthBottom = 1;
                cTituloUNIDAD.HorizontalAlignment = 1; // 0 = izquierda, 1 = centro, 2 = derecha
                cTituloUNIDAD.Padding = 5;
                cTituloUNIDAD.BackgroundColor = new Color(System.Drawing.Color.MidnightBlue);
                Tableitems.AddCell(cTituloUNIDAD);

                PdfPCell cTituloCantidad = new PdfPCell(new Phrase("CANTIDAD", _standardFontTabla));
                cTituloCantidad.BorderWidthBottom = 1;
                cTituloCantidad.HorizontalAlignment = 1; // 0 = izquierda, 1 = centro, 2 = derecha
                cTituloCantidad.Padding = 5;
                cTituloCantidad.BackgroundColor = new Color(System.Drawing.Color.MidnightBlue);
                Tableitems.AddCell(cTituloCantidad);

                foreach (EGuia ListaRep in oGuia)
                {

                    PdfPCell cItem = new PdfPCell(new Phrase(ListaRep.detalle.Material.Codigo.ToString(), _standardFont));
                    cItem.Border = 0;
                    cItem.Padding = 2;
                    cItem.PaddingBottom = 2;
                    cItem.HorizontalAlignment = 1;
                    cItem.Border = Rectangle.LEFT_BORDER;
                    Tableitems.AddCell(cItem);

                    PdfPCell cCodigo = new PdfPCell(new Phrase(ListaRep.detalle.Material.Nombre, _standardFont));
                    cCodigo.Border = 0;
                    cCodigo.Padding = 2;
                    cCodigo.PaddingBottom = 2;
                    cCodigo.HorizontalAlignment = 0;
                    cCodigo.Border = Rectangle.LEFT_BORDER;
                    Tableitems.AddCell(cCodigo);

                    PdfPCell cNombreMat = new PdfPCell(new Phrase(ListaRep.detalle.Bulto.ToString(), _standardFont));
                    cNombreMat.Border = 0;
                    cNombreMat.Padding = 2;
                    cNombreMat.PaddingBottom = 2;
                    cNombreMat.HorizontalAlignment = 1;
                    cNombreMat.Border = Rectangle.LEFT_BORDER;
                    Tableitems.AddCell(cNombreMat);

                    PdfPCell cCantidad = new PdfPCell(new Phrase(ListaRep.detalle.Material.Unidad.envase, _standardFont));
                    cCantidad.Border = 0;
                    cCantidad.Padding = 2;
                    cCantidad.PaddingBottom = 2;
                    cCantidad.HorizontalAlignment = 1;
                    cCantidad.Border = Rectangle.LEFT_BORDER;
                    Tableitems.AddCell(cCantidad);

                    PdfPCell cPrecio = new PdfPCell(new Phrase(ListaRep.detalle.Material.Unidad.Empaque, _standardFont));
                    cPrecio.Border = 0;
                    cPrecio.Padding = 2;
                    cPrecio.PaddingBottom = 2;
                    cPrecio.HorizontalAlignment = 1;
                    cPrecio.Border = Rectangle.LEFT_BORDER;
                    Tableitems.AddCell(cPrecio);

                    PdfPCell cUMNombre = new PdfPCell(new Phrase(ListaRep.detalle.Material.Unidad.Nombre, _standardFont));
                    cUMNombre.Border = 0;
                    cUMNombre.Padding = 2;
                    cUMNombre.PaddingBottom = 2;
                    cUMNombre.HorizontalAlignment = 1;
                    cUMNombre.Border = Rectangle.LEFT_BORDER;
                    Tableitems.AddCell(cUMNombre);


                    PdfPCell cCantidads = new PdfPCell(new Phrase(ListaRep.detalle.Cantidad.ToString(), _standardFont));
                    cCantidads.Border = 0;
                    cCantidads.Padding = 2;
                    cCantidads.PaddingBottom = 2;
                    cCantidads.HorizontalAlignment = 2;
                    cCantidads.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                    Tableitems.AddCell(cCantidads);

                }


                PdfPCell cCeldaDetalle = new PdfPCell(Tableitems);
                cCeldaDetalle.Colspan = 4;
                cCeldaDetalle.Border = 0;
                cCeldaDetalle.BorderWidthBottom = 1;
                cCeldaDetalle.PaddingBottom = 4;
                cCeldaDetalle.FixedHeight = 360f;
                tblPrueba.AddCell(cCeldaDetalle);

                PdfPTable TableDescripcion = new PdfPTable(3);
                TableDescripcion.WidthPercentage = 100;

                float[] CelDescripcion = new float[] { 0.5F, 0.5F, 4.0F };
                TableDescripcion.SetWidths(CelDescripcion);

                PdfPCell celNota0 = new PdfPCell(new Phrase("", _standardFont));
                celNota0.Border = 0;
                celNota0.HorizontalAlignment = 0;
                celNota0.PaddingTop = 5;
                TableDescripcion.AddCell(celNota0);

                PdfPCell celNotaTitulo = new PdfPCell(new Phrase("NOTA:  " + oGuia[0].Nota, _standardFont));
                celNotaTitulo.Border = 0;
                celNotaTitulo.Colspan = 2;
                celNotaTitulo.HorizontalAlignment = 0;
                celNotaTitulo.PaddingTop = 5;
                TableDescripcion.AddCell(celNotaTitulo);


                PdfPCell cenvio0 = new PdfPCell(new Phrase("", _standardFont));
                cenvio0.Border = 0;
                cenvio0.HorizontalAlignment = 0;
                cenvio0.PaddingTop = 5;
                TableDescripcion.AddCell(cenvio0);

                PdfPCell cmotivo = new PdfPCell(new Phrase("MOTIVO DE ENVIO:  " + oGuia[0].Motivo.ToUpper(), _standardFont));
                cmotivo.Border = 0;
                cmotivo.Colspan = 2;
                cmotivo.HorizontalAlignment = 0;
                cmotivo.PaddingTop = 5;
                TableDescripcion.AddCell(cmotivo);




                PdfPCell ctramos0 = new PdfPCell(new Phrase("", _standardFont));
                ctramos0.Border = 0;
                ctramos0.HorizontalAlignment = 0;
                ctramos0.PaddingTop = 5;
                TableDescripcion.AddCell(ctramos0);

                PdfPCell ctramos1 = new PdfPCell(new Phrase("TRAMO 1:", _standardFont));
                ctramos1.Border = 0;
                ctramos1.HorizontalAlignment = 0;
                ctramos1.PaddingTop = 5;
                TableDescripcion.AddCell(ctramos1);

                PdfPCell Ctramo1des = new PdfPCell(new Phrase(oGuia[0].DirecionSalida, _standardFont));
                Ctramo1des.Border = 0; // borde cero
                Ctramo1des.HorizontalAlignment = 0;
                Ctramo1des.PaddingTop = 5;
                TableDescripcion.AddCell(Ctramo1des);

                PdfPCell Ctramo0 = new PdfPCell(new Phrase("", _standardFont));
                Ctramo0.Border = 0;
                Ctramo0.HorizontalAlignment = 0;
                Ctramo0.PaddingTop = 5;
                TableDescripcion.AddCell(Ctramo0);

                PdfPCell Ctramo2 = new PdfPCell(new Phrase("TRAMO 2:", _standardFont));
                Ctramo2.Border = 0;
                Ctramo2.HorizontalAlignment = 0;
                Ctramo2.PaddingTop = 5;
                TableDescripcion.AddCell(Ctramo2);

                PdfPCell Ctramo2Des = new PdfPCell(new Phrase(oGuia[0].tramo2, _standardFont));
                Ctramo2Des.Border = 0; // borde cero
                Ctramo2Des.HorizontalAlignment = 0;
                Ctramo2Des.PaddingTop = 5;
                TableDescripcion.AddCell(Ctramo2Des);

                PdfPCell Celdedes = new PdfPCell(TableDescripcion);
                Celdedes.Colspan = 12;
                Celdedes.Border = 0;
                Celdedes.MinimumHeight = 20f;
                tblPrueba.AddCell(Celdedes);

                PdfPTable Tablefooter = new PdfPTable(12);
                Tablefooter.WidthPercentage = 100;

                float[] CelFooter = new float[] { 1.1F, 0.5F, 0.5F, 0.5F, 0.5F, 0.5F, 0.5F, 0.5F, 0.5F, 0.5F, 0.5F, 0.5F };
                Tablefooter.SetWidths(CelFooter);



                PdfPCell cCaja = new PdfPCell(new Phrase("Cajas:", _standardFont));
                cCaja.Border = 0;
                cCaja.HorizontalAlignment = 2;
                cCaja.PaddingTop = 10;
                Tablefooter.AddCell(cCaja);

                PdfPCell cCajas = new PdfPCell(new Phrase(oGuia[0].Caja.ToString(), _standardFont));
                cCajas.Border = 0; // borde cero
                cCajas.HorizontalAlignment = 0;
                cCajas.PaddingTop = 10;
                Tablefooter.AddCell(cCajas);

                PdfPCell cBidone = new PdfPCell(new Phrase("Bidones:", _standardFont));
                cBidone.Border = 0;
                cBidone.HorizontalAlignment = 2;
                cBidone.PaddingTop = 10;
                Tablefooter.AddCell(cBidone);

                PdfPCell cBidones = new PdfPCell(new Phrase(oGuia[0].Bidones.ToString(), _standardFont));
                cBidones.Border = 0; // borde cero
                cBidones.HorizontalAlignment = 0;
                cBidones.PaddingTop = 10;
                Tablefooter.AddCell(cBidones);

                PdfPCell cSaco = new PdfPCell(new Phrase("Sacos:", _standardFont));
                cSaco.Border = 0;
                cSaco.HorizontalAlignment = 2;
                cSaco.PaddingTop = 10;
                Tablefooter.AddCell(cSaco);

                PdfPCell cSacos = new PdfPCell(new Phrase(oGuia[0].Sacos.ToString(), _standardFont));
                cSacos.Border = 0; // borde cero
                cSacos.HorizontalAlignment = 0;
                cSacos.PaddingTop = 10;
                Tablefooter.AddCell(cSacos);

                PdfPCell cCilindo = new PdfPCell(new Phrase("Cilindros:", _standardFont));
                cCilindo.Border = 0;
                cCilindo.HorizontalAlignment = 2;
                cCilindo.PaddingTop = 10;
                Tablefooter.AddCell(cCilindo);

                PdfPCell cCilindos = new PdfPCell(new Phrase(oGuia[0].Cilindro.ToString(), _standardFont));
                cCilindos.Border = 0; // borde cero
                cCilindos.HorizontalAlignment = 0;
                cCilindos.PaddingTop = 10;
                Tablefooter.AddCell(cCilindos);

                PdfPCell cBulto = new PdfPCell(new Phrase("Bultos:", _standardFont));
                cBulto.Border = 0;
                cBulto.HorizontalAlignment = 2;
                cBulto.PaddingTop = 10;
                Tablefooter.AddCell(cBulto);

                PdfPCell cBultos = new PdfPCell(new Phrase(oGuia[0].bulto.ToString(), _standardFont));
                cBultos.Border = 0; // borde cero
                cBultos.HorizontalAlignment = 0;
                cBultos.PaddingTop = 10;
                Tablefooter.AddCell(cBultos);

                PdfPCell cPeso = new PdfPCell(new Phrase("Peso:", _standardFont));
                cPeso.Border = 0;
                cPeso.HorizontalAlignment = 2;
                cPeso.PaddingTop = 10;
                Tablefooter.AddCell(cPeso);

                PdfPCell cPesos = new PdfPCell(new Phrase(oGuia[0].Peso + "kg", _standardFont));
                cPesos.Border = 0; // borde cero
                cPesos.HorizontalAlignment = 0;
                cPesos.PaddingTop = 10;
                Tablefooter.AddCell(cPesos);
                //
                PdfPCell CceldaFooter = new PdfPCell(Tablefooter);
                CceldaFooter.Colspan = 2;
                CceldaFooter.Border = 0;
                // CeldeFecha.BorderWidthBottom = 1;
                tblPrueba.AddCell(CceldaFooter);

                PdfPCell cEspacioss = new PdfPCell(new Phrase());
                cEspacioss.Colspan = 2;
                cEspacioss.Border = 0;
                cEspacioss.MinimumHeight = 10;
                tblPrueba.AddCell(cEspacioss);


                PdfPTable Tabletrans = new PdfPTable(1);
                Tabletrans.WidthPercentage = 100;
                float[] celTrans = new float[] { 5f };
                Tabletrans.SetWidths(celTrans);

                PdfPCell cDatostrans = new PdfPCell(new Phrase("DATOS DEL TRANSPORTE", _standarTexto));
                cDatostrans.Border = 0; // borde cero
                cDatostrans.HorizontalAlignment = 0;

                cDatostrans.BorderWidthRight = 1f;
                cDatostrans.BorderWidthLeft = 1f;
                cDatostrans.BorderWidthTop = 1f;
                cDatostrans.Padding = 5;
                Tabletrans.AddCell(cDatostrans);

                PdfPCell Razon = new PdfPCell(new Phrase("Nombre o Razón Social del Transporte:     " + oGuia[0].Transporte.RazonSocial + "            RUC:       " + oGuia[0].Transporte.Ruc, _standardFont));
                Razon.Border = 0;
                Razon.HorizontalAlignment = 0;

                Razon.BorderWidthRight = 1f;
                Razon.BorderWidthLeft = 1f;
                Razon.Padding = 5;
                Tabletrans.AddCell(Razon);



                PdfPCell cdireccion = new PdfPCell(new Phrase("Domicilio:            " + oGuia[0].Transporte.Direccion, _standardFont));
                cdireccion.Border = 0; // borde cero
                cdireccion.HorizontalAlignment = 0;

                cdireccion.BorderWidthRight = 1f;
                cdireccion.BorderWidthLeft = 1f;
                cdireccion.BorderWidthBottom = 1f;
                cdireccion.Padding = 5;
                Tabletrans.AddCell(cdireccion);


                PdfPCell Cceldtr = new PdfPCell(Tabletrans);
                Cceldtr.Colspan = 2;
                Cceldtr.Border = 0;
                // CeldeFecha.BorderWidthBottom = 1;
                tblPrueba.AddCell(Cceldtr);

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

    }
}