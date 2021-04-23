using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using OfficeOpenXml;
using SIS.Business;
using SIS.Entity;
using SIS.Principal.Models;
namespace SIS.Principal.Controllers
{
    public class GeneralController : Controller
    {

        BGeneral General = new BGeneral();
        Authentication Authentication = new Authentication();
        public ActionResult ListTareas()
        {
            return View();
        }

        [HttpPost]
        public void ListaTareas(string stado, string etiqueta, string tarea, string label, string FechaIncio, string FechaFin, int numPag, int allReg, int Cant)
        {
            try
            {

                Utils.Write(
                    ResponseType.JSON,
                    General.ListaTarea(stado, etiqueta, tarea, label, FechaIncio, FechaFin, numPag, allReg, Cant)
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

        #region Reporte Excel 
        public ActionResult ReporteExcel(string stado, string etiqueta, string tarea, string label, string FechaIncio, string FechaFin, int numPag, int allReg, int Cant)
        {
            try
            {
                //Ruta de la plantilla
                FileInfo fTemplateFile = new FileInfo(Server.MapPath("/") + "Reporte/Venta/excel/" + "Tarea.xlsx");
                var Usuario = Authentication.UserLogued.Usuario;

                //Ruta del nuevo documento
                FileInfo fNewFile = new FileInfo(Server.MapPath("/") + "Reporte/Venta/excel/" + "Tarea" + "_" + Usuario + ".xlsx");

                List<ETareas> ListaCompra;


                ListaCompra = General.ListaTarea(stado, etiqueta, tarea, label, FechaIncio, FechaFin, numPag, allReg, Cant);



                ExcelPackage pck = new ExcelPackage(fNewFile, fTemplateFile);

                var ws = pck.Workbook.Worksheets[1];
                ////** aqui


                DateTime today = DateTime.Now;
                ws.Cells[6, 2].Value = today;
                ws.Cells[7, 2].Value = Usuario;
                int iFilaDetIni = 10;
                int starRow = 1;
                foreach (ETareas Detalle in ListaCompra)
                {
                    ws.Cells[iFilaDetIni + starRow, 1].Value = Detalle.item;
                    ws.Cells[iFilaDetIni + starRow, 2].Value = Detalle.stado;
                    ws.Cells[iFilaDetIni + starRow, 3].Value = Detalle.etiqueta;
                    ws.Cells[iFilaDetIni + starRow, 4].Value = Detalle.tarea;
                    ws.Cells[iFilaDetIni + starRow, 5].Value = Detalle.label + "-" + Detalle.descripcion;
                    ws.Cells[iFilaDetIni + starRow, 6].Value = Detalle.id;
                    ws.Cells[iFilaDetIni + starRow, 7].Value = Detalle.direcion;
                    ws.Cells[iFilaDetIni + starRow, 8].Value = Detalle.inicio;
                    ws.Cells[iFilaDetIni + starRow, 9].Value = Detalle.fin;
                    ws.Cells[iFilaDetIni + starRow, 10].Value = Detalle.llegada;
                    ws.Cells[iFilaDetIni + starRow, 11].Value = Detalle.duracion;
                    starRow++;
                }

                int Count = starRow + 1;

                string modelRange = "A" + System.Convert.ToString(iFilaDetIni) + ":K" + (iFilaDetIni + Count - 1).ToString();
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
                Response.AddHeader("content-disposition", "attachment;  filename=" + "ReporteTarea" + "_" + Usuario + ".xlsx");

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

        #region Reporte PDF
        public ActionResult ReportePDF(string stado, string etiqueta, string tarea, string label, string FechaIncio, string FechaFin, int numPag, int allReg, int Cant)
        {
            Document pdfDoc = new Document(PageSize.A4.Rotate(), 5, 5, 10, 10);

            List<ETareas> ListaCompra;


            ListaCompra = General.ListaTarea(stado, etiqueta, tarea, label, FechaIncio, FechaFin, numPag, allReg, Cant);



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

                PdfPTable Tableitems = new PdfPTable(11);
                Tableitems.WidthPercentage = 100;
                Tableitems.HorizontalAlignment = Element.ALIGN_JUSTIFIED;

                PdfPCell celCospan = new PdfPCell(new Phrase("REPORTE VENTA"));
                celCospan.Colspan = 11;
                celCospan.Border = 0;
                celCospan.HorizontalAlignment = 1;
                Tableitems.AddCell(celCospan);


                PdfPCell cEspacio = new PdfPCell(new Phrase("    "));
                cEspacio.Colspan = 11;
                cEspacio.Border = 0;
                cEspacio.HorizontalAlignment = 1;
                Tableitems.AddCell(cEspacio);

                Single[] celdas2 = new Single[] { 0.5F, 1.0F, 1.0F, 1.5F, 1.5F, 0.6F, 2.0F, 1.3F, 1.3F, 1.3F, 1.0F };
                Tableitems.SetWidths(celdas2);

                PdfPCell cTituloCant = new PdfPCell(new Phrase("Item", _standardFont));
                cTituloCant.Border = 1;
                cTituloCant.BorderWidthBottom = 1;
                cTituloCant.HorizontalAlignment = 1;
                cTituloCant.Padding = 5;
                Tableitems.AddCell(cTituloCant);




                PdfPCell cTituloDocumento = new PdfPCell(new Phrase("Estado", _standardFont));
                cTituloDocumento.Border = 1;
                cTituloDocumento.BorderWidthBottom = 1;
                cTituloDocumento.HorizontalAlignment = 1;
                cTituloDocumento.Padding = 5;
                Tableitems.AddCell(cTituloDocumento);

                PdfPCell CTituloruc = new PdfPCell(new Phrase("Etiqueta", _standardFont));
                CTituloruc.Border = 1;
                CTituloruc.BorderWidthBottom = 1;
                CTituloruc.HorizontalAlignment = 1;
                CTituloruc.Padding = 5;
                Tableitems.AddCell(CTituloruc);

                PdfPCell CTituloSerie = new PdfPCell(new Phrase("Tarea", _standardFont));
                CTituloSerie.Border = 1;
                CTituloSerie.BorderWidthBottom = 1;
                CTituloSerie.HorizontalAlignment = 1;
                CTituloSerie.Padding = 5;
                Tableitems.AddCell(CTituloSerie);

                PdfPCell cTipoPago = new PdfPCell(new Phrase("Descripcion", _standardFont));
                cTipoPago.Border = 1;
                cTipoPago.BorderWidthBottom = 1;
                cTipoPago.HorizontalAlignment = 1;
                cTipoPago.Padding = 5;
                Tableitems.AddCell(cTipoPago);

                PdfPCell cMonedaT = new PdfPCell(new Phrase("Id", _standardFont));
                cMonedaT.Border = 1;
                cMonedaT.BorderWidthBottom = 1;
                cMonedaT.HorizontalAlignment = 1;
                cMonedaT.Padding = 5;
                Tableitems.AddCell(cMonedaT);


                PdfPCell cFechaRegistro = new PdfPCell(new Phrase("Direccion", _standardFont));
                cFechaRegistro.Border = 1;
                cFechaRegistro.BorderWidthBottom = 1;
                cFechaRegistro.HorizontalAlignment = 1;
                cFechaRegistro.Padding = 5;
                Tableitems.AddCell(cFechaRegistro);

                PdfPCell cFechaPago = new PdfPCell(new Phrase("Fecha ini.", _standardFont));
                cFechaPago.Border = 1;
                cFechaPago.BorderWidthBottom = 1;
                cFechaPago.HorizontalAlignment = 1;
                cFechaPago.Padding = 5;
                Tableitems.AddCell(cFechaPago);

                PdfPCell cSubTotal = new PdfPCell(new Phrase("Fecha fin.", _standardFont));
                cSubTotal.Border = 1;
                cSubTotal.BorderWidthBottom = 1;
                cSubTotal.HorizontalAlignment = 1;
                cSubTotal.Padding = 5;
                Tableitems.AddCell(cSubTotal);

                PdfPCell cIGV = new PdfPCell(new Phrase("Fecha lleg.", _standardFont));
                cIGV.Border = 1;
                cIGV.BorderWidthBottom = 1;
                cIGV.HorizontalAlignment = 1;
                cIGV.Padding = 5;
                Tableitems.AddCell(cIGV);

                PdfPCell cTitulo = new PdfPCell(new Phrase("Duracion", _standardFont));
                cTitulo.Border = 1;
                cTitulo.BorderWidthBottom = 1;
                cTitulo.HorizontalAlignment = 1;
                cTitulo.Padding = 5;
                Tableitems.AddCell(cTitulo);


                Font letrasDatosTabla = FontFactory.GetFont(FontFactory.HELVETICA, 7, Font.NORMAL);



                foreach (ETareas cuentaPago in ListaCompra)
                {


                    PdfPCell cSerie = new PdfPCell(new Phrase(cuentaPago.item.ToString(), letrasDatosTabla));
                    cSerie.Border = 0;
                    cSerie.Padding = 2;
                    cSerie.PaddingBottom = 2;
                    cSerie.HorizontalAlignment = 1;
                    Tableitems.AddCell(cSerie);



                    PdfPCell cnumero = new PdfPCell(new Phrase(cuentaPago.stado, letrasDatosTabla));
                    cnumero.Border = 0;
                    cnumero.Padding = 2;
                    cnumero.PaddingBottom = 2;
                    cnumero.HorizontalAlignment = 1;
                    Tableitems.AddCell(cnumero);

                    PdfPCell Cdocumento = new PdfPCell(new Phrase(cuentaPago.etiqueta, letrasDatosTabla));
                    Cdocumento.Border = 0;
                    Cdocumento.Padding = 2;
                    Cdocumento.PaddingBottom = 2;
                    Cdocumento.HorizontalAlignment = 1;
                    Tableitems.AddCell(Cdocumento);


                    PdfPCell NroDocumento = new PdfPCell(new Phrase(cuentaPago.tarea, letrasDatosTabla));
                    NroDocumento.Border = 0;
                    NroDocumento.Padding = 2;
                    NroDocumento.PaddingBottom = 2;
                    NroDocumento.HorizontalAlignment = 1;
                    Tableitems.AddCell(NroDocumento);

                    PdfPCell Nombre = new PdfPCell(new Phrase(cuentaPago.label + " - " + cuentaPago.descripcion, letrasDatosTabla));
                    Nombre.Border = 0;
                    Nombre.Padding = 2;
                    Nombre.PaddingBottom = 2;
                    Nombre.HorizontalAlignment = 1;
                    Tableitems.AddCell(Nombre);


                    PdfPCell cellTipoPago = new PdfPCell(new Phrase(cuentaPago.id, letrasDatosTabla));
                    cellTipoPago.Border = 0;
                    cellTipoPago.Padding = 2;
                    cellTipoPago.PaddingBottom = 2;
                    cellTipoPago.HorizontalAlignment = 1;
                    Tableitems.AddCell(cellTipoPago);

                    PdfPCell cFecha = new PdfPCell(new Phrase(cuentaPago.direcion, letrasDatosTabla));
                    cFecha.Border = 0;
                    cFecha.Padding = 2;
                    cFecha.PaddingBottom = 2;
                    cFecha.HorizontalAlignment = 1;
                    Tableitems.AddCell(cFecha);


                    PdfPCell cellMoneda = new PdfPCell(new Phrase(cuentaPago.inicio, letrasDatosTabla));
                    cellMoneda.Border = 0;
                    cellMoneda.Padding = 2;
                    cellMoneda.PaddingBottom = 2;
                    cellMoneda.HorizontalAlignment = 1;
                    Tableitems.AddCell(cellMoneda);


                    PdfPCell cellFechaRegistro = new PdfPCell(new Phrase(cuentaPago.fin, letrasDatosTabla));
                    cellFechaRegistro.Border = 0;
                    cellFechaRegistro.Padding = 2;
                    cellFechaRegistro.PaddingBottom = 2;
                    cellFechaRegistro.HorizontalAlignment = 1;
                    Tableitems.AddCell(cellFechaRegistro);


                    PdfPCell cellFSubTotal = new PdfPCell(new Phrase(cuentaPago.llegada, letrasDatosTabla));
                    cellFSubTotal.Border = 0;
                    cellFSubTotal.Padding = 2;
                    cellFSubTotal.PaddingBottom = 2;
                    cellFSubTotal.HorizontalAlignment = 1;
                    Tableitems.AddCell(cellFSubTotal);

                    PdfPCell cellIGV = new PdfPCell(new Phrase(cuentaPago.duracion.ToString(), letrasDatosTabla));
                    cellIGV.Border = 0;
                    cellIGV.Padding = 2;
                    cellIGV.PaddingBottom = 2;
                    cellIGV.HorizontalAlignment = 1;
                    Tableitems.AddCell(cellIGV);


                }

                PdfPCell cCeldaDetalle = new PdfPCell(Tableitems);
                cCeldaDetalle.Border = 1;
                cCeldaDetalle.BorderWidthBottom = 1;
                cCeldaDetalle.HorizontalAlignment = 1;
                cCeldaDetalle.Padding = 5;
                tblPrueba.AddCell(cCeldaDetalle);



                pdfDoc.Add(tblPrueba);

                // Close your PDF 
                pdfDoc.Close();



                Response.ContentType = "application/pdf";

                //Set default file Name as current datetime 
                Response.AddHeader("content-disposition", "attachment; filename=ReporteTarea.pdf");
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
    }
}
