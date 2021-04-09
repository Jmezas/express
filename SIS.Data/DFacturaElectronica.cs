using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SIS.Factory;
using SIS.Entity;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.Security.Cryptography.Xml;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.IO;
using Ionic.Zip;

namespace SIS.Data
{
    public class DFacturaElectronica : DBHelper
    {
        private static DFacturaElectronica Instancia;
        private DataBase BaseDeDatos;

        public DFacturaElectronica(DataBase BaseDeDatos) : base(BaseDeDatos)
        {
            this.BaseDeDatos = BaseDeDatos;
        }

        public static DFacturaElectronica ObtenerInstancia(DataBase BaseDeDatos)
        {
            if (Instancia == null)
            {
                Instancia = new DFacturaElectronica(BaseDeDatos);
            }
            return Instancia;
        }

        public List<EVenta> ListarComprobante(int Comienzo, int Medida, int empresa, int Sucursal, string FechaEmi, int tipo)
        {
            List<EVenta> oDatos = new List<EVenta>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("FAC_FiltrarComprobantes");
                    CreateHelper(Connection);
                    AddInParameter("@iComienzo", Comienzo);
                    AddInParameter("@iMedida", Medida);
                    AddInParameter("@iSucursal", Sucursal);
                    AddInParameter("@iEmpresa", empresa);
                    AddInParameter("@fechaEmi", FechaEmi);
                    AddInParameter("@Tipo", tipo);
                    using (var Reader = ExecuteReader())
                    {

                        while (Reader.Read())
                        {
                            EVenta oComprobante = new EVenta();
                            oComprobante.Id = int.Parse(Reader["iIdVenta"].ToString());
                            oComprobante.empresa.RUC = Reader["sRuc"].ToString();
                            oComprobante.Documento.Nombre = Reader["stipoDocumento"].ToString();
                            oComprobante.serie = Reader["serie"].ToString();
                            oComprobante.fechaEmision = Reader["dFecEmision"].ToString();
                            oComprobante.tipo = int.Parse(Reader["tipo"].ToString());
                            oComprobante.total = float.Parse(Reader["nTotalCab"].ToString());
                            oComprobante.TotalR = int.Parse(Reader["Total"].ToString());
                            oDatos.Add(oComprobante);

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
        public List<EVenta> ListarBoletaResumen(int Comienzo, int Medida, int empresa, int Sucursal, string FechaEmi, int tipo)
        {
            List<EVenta> oDatos = new List<EVenta>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("FAC_FiltrarBoleta");
                    CreateHelper(Connection);
                    AddInParameter("@iComienzo", Comienzo);
                    AddInParameter("@iMedida", Medida);
                    AddInParameter("@iSucursal", Sucursal);
                    AddInParameter("@iEmpresa", empresa);
                    AddInParameter("@fechaEmi", FechaEmi);
                    AddInParameter("@Tipo", tipo);
                    using (var Reader = ExecuteReader())
                    {

                        while (Reader.Read())
                        {
                            EVenta oComprobante = new EVenta();
                            oComprobante.Id = int.Parse(Reader["iIdVenta"].ToString());
                            oComprobante.empresa.RUC = Reader["sRuc"].ToString();
                            oComprobante.Documento.Nombre = Reader["stipoDocumento"].ToString();
                            oComprobante.serie = Reader["serie"].ToString();
                            oComprobante.fechaEmision = Reader["dFecEmision"].ToString();
                            oComprobante.tipo = int.Parse(Reader["tipo"].ToString());
                            oComprobante.total = float.Parse(Reader["nTotalCab"].ToString());
                            oComprobante.TotalR = int.Parse(Reader["Total"].ToString());
                            oDatos.Add(oComprobante);

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
        public string GeneraResumen(int empresa, string fecha, string usuario)
        {
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SqlTransaction tran = (SqlTransaction)Connection.BeginTransaction();
                    SetQuery("FAC_InstResumenDiario");
                    CreateHelper(Connection, tran);
                    AddInParameter("@IdEmpresa", empresa);
                    AddInParameter("@fechaEmi", fecha);
                    AddInParameter("@Usuario", usuario);
                    AddOutParameter("@Mensaje", (DbType)SqlDbType.VarChar);
                    ExecuteQuery();
                    var smensaje = GetOutput("@Mensaje").ToString();
                    tran.Commit();
                    return smensaje;
                }
                catch (Exception Exception)
                {
                    throw Exception;
                }
                finally
                {
                    Connection.Close();
                }
            }
        }
        public string GeneraResumenDetalle(int idResumen, int idVenta, int tipoVenta, string usuario, int tipo)
        {
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SqlTransaction tran = (SqlTransaction)Connection.BeginTransaction();
                    SetQuery("FAC_InsResumen_Det");
                    CreateHelper(Connection, tran);
                    AddInParameter("@IdResumen", idResumen);
                    AddInParameter("@IdVenta", idVenta);
                    AddInParameter("@TipoVenta", tipoVenta);
                    AddInParameter("@Usuario", usuario);
                    AddInParameter("@Tipo", tipo);
                    AddOutParameter("@Mensaje", (DbType)SqlDbType.VarChar);
                    ExecuteQuery();
                    var smensaje = GetOutput("@Mensaje").ToString();
                    tran.Commit();
                    return smensaje;
                }
                catch (Exception Exception)
                {
                    throw Exception;
                }
                finally
                {
                    Connection.Close();
                }
            }
        }


        public List<EVenta> FAC_ArmarXMLResumen(int idResumen)
        {
            List<EVenta> oDatos = new List<EVenta>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("FAC_ArmarXMLResumen");
                    CreateHelper(Connection);
                    AddInParameter("@IdResumen", idResumen);
                    using (var Reader = ExecuteReader())
                    {

                        while (Reader.Read())
                        {
                            EVenta obj = new EVenta();
                            obj.empresa.RUC = Reader["sRuc"].ToString();
                            obj.empresa.RazonSocial = Reader["sRazonSocialE"].ToString();
                            obj.empresa.Direccion = Reader["sDirecion"].ToString();
                            obj.empresa.Telefono = Reader["sTelefono"].ToString();
                            obj.empresa.Ubigeo = Reader["sUbigeo"].ToString();
                            obj.empresa.Departamento = Reader["Departamentoemi"].ToString();
                            obj.empresa.Provincia = Reader["Provinciaemi"].ToString();
                            obj.empresa.Distrito = Reader["Distritoemi"].ToString();

                            obj.sCodigo = Reader["sIdentificadorRe"].ToString();
                            obj.Documento.codigo = Reader["tipodocumento"].ToString();
                            obj.fechaEmision = Reader["dFechaRegistro"].ToString();

                            obj.cliente.Id = int.Parse(Reader["stipoDocumentocli"].ToString());
                            obj.cliente.NroDocumento = Reader["sDocumentoCli"].ToString();
                            obj.estadoComprobante = Reader["iTipoEstado"].ToString();
                            obj.serie = Reader["sSerie"].ToString();
                            obj.numero = Reader["inumero"].ToString();
                            obj.moneda.Nombre = Reader["sMoneda"].ToString();
                            obj.total = float.Parse(Reader["fTotal"].ToString());
                            obj.descuento = float.Parse(Reader["fDescuento"].ToString());
                            obj.igv = float.Parse(Reader["fIGV"].ToString());
                            obj.isc = float.Parse(Reader["ftotalIsc"].ToString());
                            obj.otros = float.Parse(Reader["ftotalOtrosImpuestos"].ToString());
                            obj.grabada = float.Parse(Reader["fGrabada"].ToString());
                            obj.exonerada = float.Parse(Reader["fExonerada"].ToString());
                            obj.inafecta = float.Parse(Reader["fInafecta"].ToString());
                            obj.gratuita = float.Parse(Reader["fGratuita"].ToString());
                             
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

        public string ActualizarResumen(int IdResumen, string codigo)
        {
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SqlTransaction tran = (SqlTransaction)Connection.BeginTransaction();
                    SetQuery("FAC_ActulizarResumen");
                    CreateHelper(Connection, tran);
                    AddInParameter("@IdResumen", IdResumen); 
                    AddInParameter("@codigo", codigo); 
                    AddOutParameter("@Mensaje", (DbType)SqlDbType.VarChar);
                    ExecuteQuery();
                    var smensaje = GetOutput("@Mensaje").ToString();
                    tran.Commit();
                    return smensaje;
                }
                catch (Exception Exception)
                {
                    throw Exception;
                }
                finally
                {
                    Connection.Close();
                }
            }
        }


        public List<EVentaDetalle> oListaEnvioSunat(int IdFactura, int tipo)
        {
            List<EVentaDetalle> oDatos = new List<EVentaDetalle>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("FAC_ListaEnvioFactura");
                    CreateHelper(Connection);
                    AddInParameter("@IdVenta", IdFactura);
                    AddInParameter("@TipoVenta", tipo);
                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            EVentaDetalle obj = new EVentaDetalle();
                            obj.Venta.serie = Reader["idDocumento"].ToString();
                            obj.Venta.fechaEmision = Reader["dFecEmision"].ToString();
                            obj.Venta.condicionPago = Reader["sCondicionPago"].ToString();
                            obj.Venta.fechaPago = Reader["dFechaVencimiento"].ToString();
                            obj.Venta.hora = Reader["horaEmision"].ToString();
                            obj.Venta.moneda.Nombre = Reader["sCodigoISOMoneda"].ToString();
                            obj.Venta.Documento.Nombre = Reader["stipoDocumento"].ToString();
                            obj.Venta.grabada = float.Parse(Reader["OpeGrabada"].ToString());
                            obj.Venta.gratuita = float.Parse(Reader["OpeGratuita"].ToString());
                            obj.Venta.exonerada = float.Parse(Reader["OpeExoneradas"].ToString());
                            obj.Venta.inafecta = float.Parse(Reader["OpeInafecta"].ToString());
                            obj.Venta.total = float.Parse(Reader["nTotalCab"].ToString());
                            obj.Venta.igv = float.Parse(Reader["nIgvCab"].ToString());
                            obj.Venta.descuento = float.Parse(Reader["TotalDescuento"].ToString());

                            //receptor
                            obj.Empresa.RUC = Reader["sRuc"].ToString();
                            obj.Empresa.RazonSocial = Reader["sRazonSocialE"].ToString();
                            obj.Empresa.sCodigo = Reader["idEmpresa"].ToString();
                            obj.Empresa.Direccion = Reader["sDirecion"].ToString();
                            obj.Empresa.Telefono = Reader["sTelefono"].ToString();
                            obj.Empresa.Ubigeo = Reader["sUbigeo"].ToString();
                            obj.Empresa.Departamento = Reader["Departamentoemi"].ToString();
                            obj.Empresa.Provincia = Reader["Provinciaemi"].ToString();
                            obj.Empresa.Distrito = Reader["Distritoemi"].ToString();

                            //emisor
                            obj.Venta.cliente.NroDocumento = Reader["sRucCliente"].ToString();
                            obj.Venta.cliente.Id = int.Parse(Reader["tipoDocumento"].ToString());
                            obj.Venta.cliente.Nombre = Reader["sRazonSocial"].ToString();
                            obj.Venta.cliente.Ubigeo.sCodigo = Reader["ubigeo"].ToString();
                            obj.Venta.cliente.Ubigeo.CodigoDepartamento = Reader["Departamento"].ToString();
                            obj.Venta.cliente.Ubigeo.CodigoProvincia = Reader["Provincia"].ToString();
                            obj.Venta.cliente.Ubigeo.CodigoDistrito = Reader["Distrito"].ToString();
                            obj.Venta.cliente.Direccion = Reader["sDireccionCliente"].ToString();
                            obj.Venta.cliente.DireccionSucursal = Reader["sDireccionCliente"].ToString();
                            obj.Venta.cliente.Telefono = Reader["sTelefono"].ToString();

                            //detalle
                            obj.cantidad = float.Parse(Reader["nCantidad"].ToString());
                            obj.material.Unidad.CodigSunat = Reader["sUnidadMedida"].ToString();
                            obj.material.Codigo = Reader["sCodMaterial"].ToString();
                            obj.material.Nombre = Reader["sNombreProducto"].ToString();
                            obj.precio = float.Parse(Reader["nPrecio"].ToString());
                            obj.Importe = float.Parse(Reader["fImporte"].ToString());
                            obj.prefioReferencial = float.Parse(Reader["precioreferencial"].ToString());
                            obj.tipoPrecio = (Reader["tipoprecio"].ToString());
                            obj.tipoImpuesto = Reader["tipoImpuesto"].ToString();
                            obj.codigoPrecio = Reader["sCodigoTipoPrecioVenta"].ToString();
                            obj.impuesto = float.Parse(Reader["impuesto"].ToString());
                            obj.codigoigv = Reader["sCodigoAfectaIGV"].ToString();
                            obj.Total = float.Parse(Reader["importe"].ToString());
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

        public string RegistrarPost(int IdVenta, string comprobante, int tipo)
        {
            using (var Connection = GetConnection(BaseDeDatos))
            {
                string sMensaje = "";
                try
                {
                    Connection.Open();
                    SqlTransaction tran = (SqlTransaction)Connection.BeginTransaction();
                    SetQuery("FAC_ActulizarComprobante");
                    CreateHelper(Connection, tran);
                    AddInParameter("@idVenta", IdVenta);
                    AddInParameter("@Comprbante", comprobante);
                    AddInParameter("@TipoVenta", tipo);
                    AddOutParameter("@Mensaje", (DbType)SqlDbType.VarChar);
                    ExecuteQuery();
                    sMensaje = GetOutput("@Mensaje").ToString();
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

        public List<EGuia> ListarGuia(int Comienzo, int Medida, int empresa, int Sucursal, string FechaEmi)
        {
            List<EGuia> oDatos = new List<EGuia>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("FAC_FiltrarGia");
                    CreateHelper(Connection);
                    AddInParameter("@iComienzo", Comienzo);
                    AddInParameter("@iMedida", Medida);
                    AddInParameter("@iSucursal", Sucursal);
                    AddInParameter("@iEmpresa", empresa);
                    AddInParameter("@fechaEmi", FechaEmi);
                    using (var Reader = ExecuteReader())
                    {

                        while (Reader.Read())
                        {
                            EGuia oComprobante = new EGuia();
                            oComprobante.idGuia = int.Parse(Reader["IdGuia"].ToString());
                            oComprobante.Item = int.Parse(Reader["Row"].ToString());
                            oComprobante.empresa.RUC = Reader["sruc"].ToString();
                            oComprobante.Documento.Nombre = Reader["sCodigoTipoDocumento"].ToString();
                            oComprobante.serie = Reader["serie"].ToString();
                            oComprobante.FechaRegistro = Reader["dFechaEmision"].ToString();
                            oComprobante.Total = int.Parse(Reader["Total"].ToString());

                            oDatos.Add(oComprobante);

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

        public string ActulizarGuia(int IdGuia, string comprobante)
        {
            using (var Connection = GetConnection(BaseDeDatos))
            {
                string sMensaje = "";
                try
                {
                    Connection.Open();
                    SqlTransaction tran = (SqlTransaction)Connection.BeginTransaction();
                    SetQuery("FAC_ActualizarGuia");
                    CreateHelper(Connection, tran);
                    AddInParameter("@IdGuia", IdGuia);
                    AddInParameter("@Comprobante", comprobante);
                    AddOutParameter("@Mensaje", (DbType)SqlDbType.VarChar);
                    ExecuteQuery();
                    sMensaje = GetOutput("@Mensaje").ToString();
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

        public List<EVenta> ListarComprobanteAnular(int Comienzo, int Medida, int empresa, int Sucursal, string FechaEmi, int tipo)
        {
            List<EVenta> oDatos = new List<EVenta>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("FAC_FiltrarComprobantesAnular");
                    CreateHelper(Connection);
                    AddInParameter("@iComienzo", Comienzo);
                    AddInParameter("@iMedida", Medida);
                    AddInParameter("@iSucursal", Sucursal);
                    AddInParameter("@iEmpresa", empresa);
                    AddInParameter("@fechaEmi", FechaEmi);
                    AddInParameter("@Tipo", tipo);
                    using (var Reader = ExecuteReader())
                    {

                        while (Reader.Read())
                        {
                            EVenta oComprobante = new EVenta();
                            oComprobante.Id = int.Parse(Reader["iIdVenta"].ToString());
                            oComprobante.empresa.RUC = Reader["sRuc"].ToString();
                            oComprobante.Documento.Nombre = Reader["stipoDocumento"].ToString();
                            oComprobante.serie = Reader["serie"].ToString();
                            oComprobante.fechaEmision = Reader["dFecEmision"].ToString();
                            oComprobante.tipo = int.Parse(Reader["tipo"].ToString());
                            oComprobante.total = float.Parse(Reader["nTotalCab"].ToString());
                            oComprobante.TotalR = int.Parse(Reader["Total"].ToString());
                            oDatos.Add(oComprobante);

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

        public List<EVenta> ListarComprobanteAnulados(int Comienzo, int Medida, int empresa, int Sucursal, string FechaEmi, int tipo)
        {
            List<EVenta> oDatos = new List<EVenta>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("FAC_FiltrarComprobantesAnular");
                    CreateHelper(Connection);
                    AddInParameter("@iComienzo", Comienzo);
                    AddInParameter("@iMedida", Medida);
                    AddInParameter("@iSucursal", Sucursal);
                    AddInParameter("@iEmpresa", empresa);
                    AddInParameter("@fechaEmi", FechaEmi);
                    AddInParameter("@Tipo", tipo);
                    using (var Reader = ExecuteReader())
                    {

                        while (Reader.Read())
                        {
                            EVenta oComprobante = new EVenta();
                            oComprobante.Id = int.Parse(Reader["iIdVenta"].ToString());
                            oComprobante.empresa.RUC = Reader["sRuc"].ToString();
                            oComprobante.Documento.Nombre = Reader["stipoDocumento"].ToString();
                            oComprobante.serie = Reader["serie"].ToString();
                            oComprobante.fechaEmision = Reader["dFecEmision"].ToString();
                            oComprobante.tipo = int.Parse(Reader["tipo"].ToString());
                            oComprobante.total = float.Parse(Reader["nTotalCab"].ToString());
                            oComprobante.TotalR = int.Parse(Reader["Total"].ToString());
                            oDatos.Add(oComprobante);

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

        public string GenerarComucacionbaja(int IdEmpresa, string fecha, string usuario)
        {
            using (var Connection = GetConnection(BaseDeDatos))
            {
                string sMensaje = "";
                try
                {
                    Connection.Open();
                    SqlTransaction tran = (SqlTransaction)Connection.BeginTransaction();
                    SetQuery("FAC_GenerarComuBaja");
                    CreateHelper(Connection, tran);
                    AddInParameter("@IdEmpresa", IdEmpresa);
                    AddInParameter("@fechaEmi", fecha);
                    AddInParameter("@Usuario", usuario);
                    AddOutParameter("@Mensaje", (DbType)SqlDbType.VarChar);
                    ExecuteQuery();
                    sMensaje = GetOutput("@Mensaje").ToString();
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

        public string GenerarComucacionbajaDetalle(int Idcomunicacion, int IdVenta, int TipoVenta, int Tipo, string usuario)
        {
            using (var Connection = GetConnection(BaseDeDatos))
            {
                string sMensaje = "";
                try
                {
                    Connection.Open();
                    SqlTransaction tran = (SqlTransaction)Connection.BeginTransaction();
                    SetQuery("FAC_InsComunicacionDet");
                    CreateHelper(Connection, tran);
                    AddInParameter("@IdComunicacion", Idcomunicacion);
                    AddInParameter("@IdVenta", IdVenta);
                    AddInParameter("@tipoVenta", TipoVenta);
                    AddInParameter("@Tipo", Tipo);
                    AddInParameter("@Usuario", usuario);
                    AddOutParameter("@Mensaje", (DbType)SqlDbType.VarChar);
                    ExecuteQuery();
                    sMensaje = GetOutput("@Mensaje").ToString();
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


        public List<Ecomunicacion> armarBaja(int IdComunicacion)
        {
            List<Ecomunicacion> oDatos = new List<Ecomunicacion>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("FAC_ArmarComunicacion");
                    CreateHelper(Connection);
                    AddInParameter("@Idcomunicacion", IdComunicacion);
                    using (var Reader = ExecuteReader())
                    {

                        while (Reader.Read())
                        {
                            Ecomunicacion obj = new Ecomunicacion();
                            obj.Empresa.RUC = (Reader["sRuc"].ToString());
                            obj.Empresa.RazonSocial = Reader["sRazonSocialE"].ToString();
                            obj.Empresa.Direccion = Reader["sDirecion"].ToString();
                            obj.Empresa.Telefono = Reader["sTelefono"].ToString();
                            obj.Empresa.Ubigeo = Reader["sUbigeo"].ToString();
                            obj.Empresa.Departamento = (Reader["Departamentoemi"].ToString());
                            obj.Empresa.Provincia = (Reader["Provinciaemi"].ToString());
                            obj.Empresa.Distrito = (Reader["Distritoemi"].ToString());

                            obj.Documento = Reader["IdDocumento"].ToString();
                            obj.fecha = Reader["FechaEmision"].ToString();
                            obj.numero = Reader["inumeroDoc"].ToString();
                            obj.Serie = Reader["sSerieDoc"].ToString();
                            obj.codigo = Reader["scodigoTipoDoc"].ToString();
                            obj.Motivo = Reader["sMotivoBaja"].ToString();

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
        public string ActulizarBaja(int Id, string comprobante)
        {
            using (var Connection = GetConnection(BaseDeDatos))
            {
                string sMensaje = "";
                try
                {
                    Connection.Open();
                    SqlTransaction tran = (SqlTransaction)Connection.BeginTransaction();
                    SetQuery("FAC_ActualizarBaja");
                    CreateHelper(Connection, tran);
                    AddInParameter("@idComunicacion", Id);
                    AddInParameter("@Idcom", comprobante);
                    AddOutParameter("@Mensaje", (DbType)SqlDbType.VarChar);
                    ExecuteQuery();
                    sMensaje = GetOutput("@Mensaje").ToString();
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


    }
}
