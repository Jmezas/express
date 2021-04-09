using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIS.Entity;
using SIS.Factory;
using System.Data.SqlClient;
using System.Data;

namespace SIS.Data
{
    public class DCotizacion : DBHelper
    {
        private static DCotizacion Instancia;
        private DataBase BaseDeDatos;

        public DCotizacion(DataBase BaseDeDatos) : base(BaseDeDatos)
        {
            this.BaseDeDatos = BaseDeDatos;
        }

        public static DCotizacion ObtenerInstancia(DataBase BaseDeDatos)
        {
            if (Instancia == null)
            {
                Instancia = new DCotizacion(BaseDeDatos);
            }
            return Instancia;
        }

        public string RegistrarCotizacion(ECotizacion oDatos, List<EDetCotizacion> Detalle, string Usuario)
        {
            using (var Connection = GetConnection(BaseDeDatos))
            {
                string sMensaje = "";
                try
                {
                    Connection.Open();
                    SqlTransaction tran = (SqlTransaction)Connection.BeginTransaction();
                    SetQuery("FAC_InstCotizacion");
                    CreateHelper(Connection, tran);
                    AddInParameter("@iIdCotizacion", oDatos.Idcotzacion);
                    AddInParameter("@idEmpresa", oDatos.empresa.Id);
                    AddInParameter("@iIdDocumento", oDatos.Documento.IdDocumento);
                    AddInParameter("@iIdCliente", oDatos.cliente.IdCliente);
                    AddInParameter("@iIdMoneda", oDatos.moneda.IdMoneda); 
                    AddInParameter("@dFechaPago", DateTime.Parse(oDatos.fechaPago));
                    AddInParameter("@dFechaEmision", DateTime.Parse(oDatos.fechaEmision));
                    AddInParameter("@sSerie", oDatos.serie);
                    AddInParameter("@sNroDoc", oDatos.numero);
                    AddInParameter("@fCantidadCab", oDatos.cantidad);
                    AddInParameter("@fOperGrabada", oDatos.grabada);
                    AddInParameter("@fOperInafecta", (oDatos.inafecta));
                    AddInParameter("@fOperExoneradas", (oDatos.exonerada));
                    AddInParameter("@fIGVCab", (oDatos.igv));
                    AddInParameter("@fTotalCab", oDatos.total);
                    AddInParameter("@TotalDescuento", oDatos.descuento);
                    AddInParameter("@nCambio", oDatos.cambio);
                    AddInParameter("@sObservacion", oDatos.observacion,AllowNull);
                    AddInParameter("@sCodReg", Usuario);
                    AddOutParameter("@Mensaje", (DbType)SqlDbType.VarChar);
                    ExecuteQuery();
                    sMensaje = GetOutput("@Mensaje").ToString();
                    string[] vMensaje = sMensaje.Split('|');
                    if (vMensaje[0].Equals("success"))
                    {
                        string[] vValues = vMensaje[2].Split('&');
                        int iIVenta = int.Parse(vValues[0]);

                        string[] dMensaje;
                        foreach (EDetCotizacion oDetalle in Detalle)
                        {
                            SetQuery("FAC_inst_detCotizacion");
                            CreateHelper(Connection, tran);
                            AddInParameter("@iIdCotizacion", iIVenta);
                            AddInParameter("@iIdMat", oDetalle.material.IdMaterial);
                            AddInParameter("@fCantidad", oDetalle.cantidad);
                            AddInParameter("@fPrecio", oDetalle.precio);
                            AddInParameter("@fImporte", oDetalle.Importe);
                            AddInParameter("@nPorcDscto", oDetalle.descuentoPor);
                            AddInParameter("@nDescuento", oDetalle.descuento);
                            AddInParameter("@operacion", oDetalle.operacion);
                            AddInParameter("@usuario", Usuario);
                            AddOutParameter("@Mensaje", (DbType)SqlDbType.VarChar);
                            ExecuteQuery();
                            dMensaje = GetOutput("@Mensaje").ToString().Split('|');
                            if (!dMensaje[0].Equals("success"))
                            {
                                throw new Exception();
                            }
                        }
                    }
                    else
                    {
                        throw new Exception();
                    }
                    tran.Commit();
                    return sMensaje;
                }
                catch (Exception Exception)
                {
                    sMensaje = "error|" + Exception.Message;
                    return sMensaje;
                }

                finally { Connection.Close(); }
            }
        }

        public List<ECotizacion> ListaCotizacion(string cliente, int moneda, int Empresa, string FechaIncio, string FechaFin, int numPag, int allReg, int Cant)
        {
            List<ECotizacion> oDatos = new List<ECotizacion>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("FAC_ListaCotizacion");
                    CreateHelper(Connection);
                    AddInParameter("@cliente", cliente);
                    AddInParameter("@Moneda", moneda);
                    AddInParameter("@IdEmpresa", Empresa);
                    AddInParameter("@FechaInicio", FechaIncio);
                    AddInParameter("@FechaFin", FechaFin);
                    AddInParameter("@numPagina", numPag);
                    AddInParameter("@allReg", allReg);
                    AddInParameter("@iCantFilas", Cant);

                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            ECotizacion obj = new ECotizacion();
                            obj.Idcotzacion = int.Parse(Reader["iIdCotizacion"].ToString());
                            obj.serie = Reader["serie"].ToString();
                            obj.cliente.Nombre = Reader["sRazonSocial"].ToString();
                            obj.cliente.NroDocumento = Reader["sNroDoc"].ToString();
                            obj.moneda.Nombre = Reader["moneda"].ToString();
                   
                            obj.fechaEmision = Reader["dFechaEmision"].ToString(); 
                   
                            obj.cantidad = float.Parse(Reader["fCantidadCab"].ToString());
                            obj.grabada = float.Parse(Reader["fOperGrabada"].ToString());
                            obj.inafecta = float.Parse(Reader["fOperExoneradas"].ToString());
                            obj.exonerada = float.Parse(Reader["fOperInafecta"].ToString());
                            obj.igv = float.Parse(Reader["fIGVCab"].ToString());
                            obj.total = float.Parse(Reader["fTotalCab"].ToString());
                            obj.descuento = float.Parse(Reader["TotalDescuento"].ToString());
                            obj.observacion = (Reader["sObservacion"].ToString());
                            obj.TotalR = int.Parse(Reader["Total"].ToString());
                            obj.item = int.Parse(Reader["item"].ToString());
                            obj.TotalPagina = int.Parse(Reader["totalPaginas"].ToString());
                            oDatos.Add(obj);
                        }
                    }
                }
                catch (Exception Exception)
                {
                    throw Exception;
                }
                finally
                {
                    Connection.Close();
                }
                return oDatos;
            }
        }


        public List<EDetCotizacion> ImpirmirCotizacion(int Idoc)
        {
            List<EDetCotizacion> oDatos = new List<EDetCotizacion>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("FAC_ImprimirCotizacion");
                    CreateHelper(Connection);
                    AddInParameter("@IdCotizacion", Idoc); 
                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            EDetCotizacion obj = new EDetCotizacion();
                            obj.Idcotizacion = int.Parse(Reader["iIdCotizacion"].ToString());
                            obj.Cotizacion.cliente.NroDocumento = Reader["sNroDoc"].ToString();
                            obj.Cotizacion.cliente.Nombre = Reader["sRazonSocial"].ToString();
                            obj.Cotizacion.Documento.Nombre = Reader["Pago"].ToString();
                            obj.Cotizacion.fechaPago = Reader["dFechaPago"].ToString();
                            obj.Cotizacion.cliente.Direccion = Reader["sDireccion"].ToString();
                            obj.Cotizacion.moneda.Nombre = Reader["moneda"].ToString();
                            obj.Cotizacion.fechaEmision = Reader["dFechaEmision"].ToString();
                            obj.Cotizacion.serie = Reader["serie"].ToString();
                            obj.Cotizacion.cantidad = float.Parse(Reader["fCantidadCab"].ToString());
                            obj.Cotizacion.grabada = float.Parse(Reader["fOperGrabada"].ToString());
                            obj.Cotizacion.exonerada = float.Parse(Reader["fOperExoneradas"].ToString());
                            obj.Cotizacion.inafecta = float.Parse(Reader["fOperInafecta"].ToString());
                            obj.Cotizacion.igv = float.Parse(Reader["fIGVCab"].ToString());
                            obj.Cotizacion.total = float.Parse(Reader["fTotalCab"].ToString());
                            obj.Cotizacion.descuento = float.Parse(Reader["TotalDescuento"].ToString());
                            obj.Cotizacion.observacion = (Reader["sObservacion"].ToString());
                            obj.material.Codigo = (Reader["sCodMaterial"].ToString());
                            obj.material.Nombre = (Reader["sNomMaterial"].ToString());
                            obj.material.Unidad.Nombre = (Reader["sNombreUMD"].ToString());
                            obj.cantidad = float.Parse(Reader["fCantidad"].ToString());
                            obj.precio = float.Parse(Reader["fprecio"].ToString());
                            obj.Importe = float.Parse(Reader["fImporte"].ToString());
                            obj.descuento = float.Parse(Reader["nPorcDscto"].ToString());
                            obj.descuentoPor = float.Parse(Reader["nDescuento"].ToString()); 
                            oDatos.Add(obj);
                        }
                    }
                }
                catch (Exception Exception)
                {
                    throw Exception;
                }
                finally
                {
                    Connection.Close();
                }
                return oDatos;
            }
        }

    }
}
