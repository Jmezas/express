using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIS.Entity;
using SIS.Factory;
using System.Data.SqlClient;
using System.Data;
using System.Xml;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using Ionic.Zip;
namespace SIS.Data
{
    public class DVenta : DBHelper
    {
        private static DVenta Instancia;
        private DataBase BaseDeDatos;

        public DVenta(DataBase BaseDeDatos) : base(BaseDeDatos)
        {
            this.BaseDeDatos = BaseDeDatos;
        }

        public static DVenta ObtenerInstancia(DataBase BaseDeDatos)
        {
            if (Instancia == null)
            {
                Instancia = new DVenta(BaseDeDatos);
            }
            return Instancia;
        }
        public string RegistrarVenta(EVenta oDatos, List<EVentaDetalle> Detalle, string Usuario)
        {
            using (var Connection = GetConnection(BaseDeDatos))
            {
                string sMensaje = "";
                string fileName = "";
                string zipName = "";
                try
                {
                    Connection.Open();
                    SqlTransaction tran = (SqlTransaction)Connection.BeginTransaction();
                    SetQuery("FAC_Inst_CabVenta");
                    CreateHelper(Connection, tran);
                    AddInParameter("@iIdVenta", oDatos.IdVenta);
                    AddInParameter("@iIdCliente", oDatos.cliente.IdCliente);
                    AddInParameter("@CondPago", oDatos.Documento.IdDocumento);
                    AddInParameter("@iIdComprobante", oDatos.Comprobante.Id);
                    AddInParameter("@iIdEmpresa", oDatos.empresa.Id);
                    AddInParameter("@dFechaEmision", DateTime.Parse(oDatos.fechaEmision));
                    AddInParameter("@dfechaPago", DateTime.Parse(oDatos.fechaPago));
                    AddInParameter("@sSerie", oDatos.serie);
                    AddInParameter("@iNumero", oDatos.numero);
                    AddInParameter("@sCodMoneda", oDatos.moneda.IdMoneda);
                    AddInParameter("@nCantTotalCab", oDatos.cantidad);
                    AddInParameter("@nSubtotalCab", oDatos.grabada);
                    AddInParameter("@TotalInafecta", oDatos.inafecta);
                    AddInParameter("@TotalExonerada", oDatos.exonerada);
                    AddInParameter("@TotalGratuita", oDatos.gratuita);
                    AddInParameter("@nIgvCab", (oDatos.igv));
                    AddInParameter("@nTotalCab", (oDatos.total));
                    AddInParameter("@TotalDescuento", oDatos.descuento);
                    AddInParameter("@sCodReg", Usuario);
                    AddInParameter("@Observacion", oDatos.observacion, AllowNull);
                    AddInParameter("@iIdSucursal", oDatos.Sucursal.IdSucursal);
                    AddInParameter("@Cambio", float.Parse(oDatos.cambio));
                    AddInParameter("@fMontoRecibidad", float.Parse(oDatos.montoRecibido));
                    AddInParameter("@fMontoPago", oDatos.total);
                    AddInParameter("@fCambio", float.Parse(oDatos.vuelto));
                    AddInParameter("@iPagodo", oDatos.metodoPago);
                    AddInParameter("@sNotaPago", oDatos.NotaPago, AllowNull);
                    AddInParameter("@CostoEnvio", oDatos.CostoEnvio);
                    AddInParameter("@idTipoVenta", 2);
                    AddInParameter("@IdVendedor", oDatos.IdVendedor);
                    AddOutParameter("@Mensaje", (DbType)SqlDbType.VarChar);
                    ExecuteQuery();
                    sMensaje = GetOutput("@Mensaje").ToString();
                    string[] vMensaje = sMensaje.Split('|');
                    if (vMensaje[0].Equals("success"))
                    {
                        string[] vValues = vMensaje[2].Split('&');
                        int iIVenta = int.Parse(vValues[0]);
                        string[] dMensaje;
                        string alerta;
                        foreach (EVentaDetalle oDetalle in Detalle)
                        {
                            SetQuery("FAC_Inst_detVenta");
                            CreateHelper(Connection, tran);
                            AddInParameter("@iIdVenta", iIVenta);
                            AddInParameter("@IdEmpresa", oDatos.empresa.Id);
                            AddInParameter("@IdSucursal", oDatos.Sucursal.IdSucursal);
                            AddInParameter("@iIdProducto", oDetalle.material.IdMaterial);
                            AddInParameter("@nCantidad", oDetalle.cantidad);
                            AddInParameter("@nPrecio", oDetalle.precio);
                            AddInParameter("@fImporte", oDetalle.Importe);
                            AddInParameter("@nPorcDscto", oDetalle.descuentoPor);
                            AddInParameter("@nDescuento", oDetalle.descuento);
                            AddInParameter("@tipoOpe", oDetalle.operacion);
                            AddInParameter("@IdAlamcen", oDetalle.Almacen.IdAlmacen);
                            AddInParameter("@sCodReg", Usuario);
                            AddInParameter("@vStcok", oDatos.vstock);
                            AddOutParameter("@Mensaje", (DbType)SqlDbType.VarChar);
                            ExecuteQuery();
                            dMensaje = GetOutput("@Mensaje").ToString().Split('|');
                            if (!dMensaje[0].Equals("success"))
                            {
                                //throw new Exception();
                                return alerta = dMensaje[0] + "|" + dMensaje[1];
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


        public string RegistrarPost(EVenta oDatos, List<EPago> pago, List<EVentaDetalle> Detalle, string Usuario)
        {
            using (var Connection = GetConnection(BaseDeDatos))
            {
                string sMensaje = "";
                try
                {
                    Connection.Open();
                    SqlTransaction tran = (SqlTransaction)Connection.BeginTransaction();
                    SetQuery("FAC_Inst_POST");
                    CreateHelper(Connection, tran);
                    AddInParameter("@iIdVenta", oDatos.IdVenta);
                    AddInParameter("@iIdCliente", oDatos.cliente.IdCliente);
                    AddInParameter("@CondPago", oDatos.Documento.IdDocumento);
                    AddInParameter("@iIdComprobante", oDatos.Comprobante.Id);
                    AddInParameter("@iIdEmpresa", oDatos.empresa.Id);
                    AddInParameter("@dFechaEmision", DateTime.Parse(oDatos.fechaEmision));
                    AddInParameter("@dfechaPago", DateTime.Parse(oDatos.fechaPago));
                    AddInParameter("@sSerie", oDatos.serie);
                    AddInParameter("@iNumero", oDatos.numero);
                    AddInParameter("@sCodMoneda", oDatos.moneda.IdMoneda);
                    AddInParameter("@nCantTotalCab", oDatos.cantidad);
                    AddInParameter("@nSubtotalCab", oDatos.grabada);
                    AddInParameter("@TotalInafecta", oDatos.inafecta);
                    AddInParameter("@TotalExonerada", oDatos.exonerada);
                    AddInParameter("@TotalGratuita", oDatos.gratuita);
                    AddInParameter("@nIgvCab", (oDatos.igv));
                    AddInParameter("@nTotalCab", (oDatos.total));
                    AddInParameter("@TotalDescuento", oDatos.descuento);
                    AddInParameter("@sCodReg", Usuario);
                    AddInParameter("@Observacion", oDatos.observacion, AllowNull);
                    AddInParameter("@iIdSucursal", oDatos.Sucursal.IdSucursal);
                    AddInParameter("@Cambio", float.Parse(oDatos.cambio));
                    //AddInParameter("@fMontoRecibidad", float.Parse(oDatos.montoRecibido));
                    //AddInParameter("@fMontoPago", oDatos.total);
                    //AddInParameter("@fCambio", float.Parse(oDatos.vuelto));
                    //AddInParameter("@iPagodo", oDatos.metodoPago);
                    //AddInParameter("@sNotaPago", oDatos.NotaPago, AllowNull);
                    //AddInParameter("@CostoEnvio", oDatos.CostoEnvio, AllowNull);
                    //AddInParameter("@idTipoVenta", 1);
                    //AddInParameter("@CanalesVenta", oDatos.CanalesVenta);
                    //AddInParameter("@IdBanco", oDatos.idBanco);
                    //AddInParameter("@IdEnvio", oDatos.Envio.Id, AllowNull);
                    //AddInParameter("@dFechaEnvio", DateTime.Parse(oDatos.fechaEnvio == null ? "1999-01-01" : oDatos.fechaEnvio));

                    AddOutParameter("@Mensaje", (DbType)SqlDbType.VarChar);
                    ExecuteQuery();
                    sMensaje = GetOutput("@Mensaje").ToString();
                    string[] vMensaje = sMensaje.Split('|');
                    if (vMensaje[0].Equals("success"))
                    {
                        string[] vValues = vMensaje[2].Split('&');
                        int iIVenta = int.Parse(vValues[0]);
                        string[] dMensaje;
                        foreach (EPago oDetalle in pago)
                        {
                            SetQuery("FAC_instCheck_Venta");
                            CreateHelper(Connection, tran);
                            AddInParameter("@iIdVenta", iIVenta);
                            AddInParameter("@fMontoRecibidad", oDetalle.Pago);
                            AddInParameter("@fMontoPago", oDatos.total);
                            AddInParameter("@fCambio", float.Parse(oDatos.vuelto));
                            AddInParameter("@iPagodo", oDetalle.TipoPago);
                            AddInParameter("@sNotaPago", oDatos.NotaPago, AllowNull);
                            AddInParameter("@CostoEnvio", oDatos.CostoEnvio, AllowNull);
                            AddInParameter("@idTipoVenta", 1);
                            AddInParameter("@CanalesVenta", oDatos.CanalesVenta);
                            AddInParameter("@IdBanco", oDetalle.banco);
                            AddInParameter("@IdEnvio", oDatos.Envio.Id, AllowNull);
                            AddInParameter("@sCodReg", Usuario);
                            AddInParameter("@dFechaEnvio", DateTime.Parse(oDatos.fechaEnvio == null ? "1999-01-01" : oDatos.fechaEnvio));
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
                    if (vMensaje[0].Equals("success"))
                    {
                        string[] vValues = vMensaje[2].Split('&');
                        int iIVenta = int.Parse(vValues[0]);
                        string[] dMensaje;
                        string alerta;
                        foreach (EVentaDetalle oDetalle in Detalle)
                        {
                            SetQuery("FAC_Inst_detPost");
                            CreateHelper(Connection, tran);
                            AddInParameter("@iIdVenta", iIVenta);
                            AddInParameter("@IdEmpresa", oDatos.empresa.Id);
                            AddInParameter("@IdSucursal", oDatos.Sucursal.IdSucursal);
                            AddInParameter("@iIdProducto", oDetalle.material.IdMaterial);
                            AddInParameter("@nCantidad", oDetalle.cantidad);
                            AddInParameter("@nPrecio", oDetalle.precio);
                            AddInParameter("@fImporte", oDetalle.Importe);
                            AddInParameter("@nPorcDscto", oDetalle.descuentoPor);
                            AddInParameter("@nDescuento", oDetalle.descuento);
                            AddInParameter("@tipoOpe", oDetalle.operacion);
                            AddInParameter("@IdAlamcen", oDetalle.Almacen.IdAlmacen);
                            AddInParameter("@sCodReg", Usuario);
                            AddInParameter("@vStcok", oDatos.vstock);
                            AddInParameter("@CodigoProducto", oDatos.CodigoProducto);
                            AddOutParameter("@Mensaje", (DbType)SqlDbType.VarChar);
                            ExecuteQuery();
                            dMensaje = GetOutput("@Mensaje").ToString().Split('|');
                            if (!dMensaje[0].Equals("success"))
                            {
                                //throw new Exception();
                                return alerta = dMensaje[0] + "|" + dMensaje[1];
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
        public EGeneral SerieNuroDocumento(int IdComprobante, int Empresa, int sucursal)
        {
            EGeneral oDatos = null;
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("FAC_VerSerieNumeroDoc");
                    CreateHelper(Connection);
                    AddInParameter("@iIdComprobante", IdComprobante);
                    AddInParameter("@Idempresa", Empresa);
                    AddInParameter("@IdSucursal", sucursal);
                    using (var Reader = ExecuteReader())
                    {
                        if (Reader.Read())
                        {
                            oDatos = new EGeneral();
                            oDatos.Text = (Reader["sSerie"].ToString());
                            oDatos.Nombre = (Reader["Numero"].ToString());

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

        public List<EVenta> ListaVenta(string filtro, int Empresa, int sucursal, string FechaIncio, string FechaFin, int numPag, int allReg, int Cant)
        {
            List<EVenta> oDatos = new List<EVenta>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("FAC_ListaFacBol");
                    CreateHelper(Connection);
                    AddInParameter("@Filltro", filtro);
                    AddInParameter("@IdEmpresa", Empresa);
                    AddInParameter("@IdSucursal", sucursal);
                    AddInParameter("@FechaInicio", FechaIncio);
                    AddInParameter("@FechaFin", FechaFin);
                    AddInParameter("@numPagina", numPag);
                    AddInParameter("@allReg", allReg);
                    AddInParameter("@iCantFilas", Cant);

                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            EVenta obj = new EVenta();
                            obj.IdVenta = int.Parse(Reader["iIdVenta"].ToString());
                            obj.serie = Reader["sSerie"].ToString();
                            obj.numero = Reader["numero"].ToString();
                            obj.cliente.Nombre = Reader["sCliente"].ToString();
                            obj.cliente.NroDocumento = Reader["sRucCliente"].ToString();
                            obj.moneda.Nombre = Reader["sCodigoISOMoneda"].ToString();
                            obj.Documento.Nombre = Reader["sNombreDoc"].ToString();
                            obj.fechaEmision = Reader["dFechaEmision"].ToString();
                            obj.cantidad = float.Parse(Reader["fCantidadCab"].ToString());
                            obj.grabada = float.Parse(Reader["opeGrabada"].ToString());
                            obj.inafecta = float.Parse(Reader["OpeInafecta"].ToString());
                            obj.exonerada = float.Parse(Reader["OpeExoneradas"].ToString());
                            obj.igv = float.Parse(Reader["nIgvCab"].ToString());
                            obj.total = float.Parse(Reader["nTotalCab"].ToString());
                            obj.descuento = float.Parse(Reader["TotalDescuento"].ToString());
                            obj.TotalR = int.Parse(Reader["Total"].ToString());
                            obj.item = int.Parse(Reader["item"].ToString());
                            obj.TotalPagina = int.Parse(Reader["totalPaginas"].ToString());
                            obj.Nombre = Reader["sNombreCompletoCobrador"].ToString();
                            obj.CostoEnvio = float.Parse(Reader["fCostoEnvio"].ToString());
                            obj.Text = Reader["TipoPago"].ToString();//tipo de pago -- efecto, credito etc
                            obj.observacion = Reader["Observacion"].ToString();
                            obj.Comprobante.Nombre = Reader["Serie"].ToString();
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

        public List<EVenta> ListaVentaPOST(string filtro, int Empresa, int sucursal, string FechaIncio, string FechaFin, int numPag, int allReg, int Cant)
        {
            List<EVenta> oDatos = new List<EVenta>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("FAC_ListaFacBolPOST");
                    CreateHelper(Connection);
                    AddInParameter("@Filltro", filtro);
                    AddInParameter("@IdEmpresa", Empresa);

                    AddInParameter("@IdSucursal", sucursal);
                    AddInParameter("@FechaInicio", FechaIncio);
                    AddInParameter("@FechaFin", FechaFin);
                    AddInParameter("@numPagina", numPag);
                    AddInParameter("@allReg", allReg);
                    AddInParameter("@iCantFilas", Cant);

                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            EVenta obj = new EVenta();
                            obj.IdVenta = int.Parse(Reader["iIdVenta"].ToString());
                            obj.serie = Reader["sSerie"].ToString();
                            obj.numero = Reader["numero"].ToString();
                            obj.cliente.Nombre = Reader["sCliente"].ToString();
                            obj.cliente.NroDocumento = Reader["sRucCliente"].ToString();
                            obj.cliente.Direccion = Reader["sDireccion"].ToString();
                            obj.cliente.Email = Reader["sEmail"].ToString();
                            obj.cliente.Telefono = Reader["sTelefono"].ToString();
                            obj.moneda.Nombre = Reader["sCodigoISOMoneda"].ToString();
                            obj.Documento.Nombre = Reader["sNombreDoc"].ToString();
                            obj.fechaEmision = Reader["dFechaEmision"].ToString();
                            obj.cantidad = float.Parse(Reader["fCantidadCab"].ToString());
                            obj.grabada = float.Parse(Reader["opeGrabada"].ToString());
                            obj.inafecta = float.Parse(Reader["OpeInafecta"].ToString());
                            obj.exonerada = float.Parse(Reader["OpeExoneradas"].ToString());
                            obj.igv = float.Parse(Reader["nIgvCab"].ToString());
                            obj.total = float.Parse(Reader["nTotalCab"].ToString());
                            obj.descuento = float.Parse(Reader["TotalDescuento"].ToString());
                            obj.TotalR = int.Parse(Reader["Total"].ToString());
                            obj.item = int.Parse(Reader["item"].ToString());
                            obj.TotalPagina = int.Parse(Reader["totalPaginas"].ToString());
                            obj.Nombre = Reader["sNombreCompletoCobrador"].ToString();
                            obj.CostoEnvio = float.Parse(Reader["fCostoEnvio"].ToString());
                            obj.Text = Reader["TipoPago"].ToString();//tipo de pago -- efecto, credito etc
                            obj.observacion = Reader["Observacion"].ToString();
                            obj.Comprobante.Nombre = Reader["Serie"].ToString();
                            obj.sCanalesVenta = Reader["CanalVenta"].ToString();//Canal venta
                            obj.EstadoEnvio = Reader["EstadoEnvio"].ToString();
                            obj.Envio.Nombre = Reader["Nombre"].ToString();
                            obj.fechaEnvio = Reader["dFechaEnvio"].ToString();

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

        public List<EVenta> ListaVentaPOSEnvios(string filtro, int Empresa, int sucursal, string FechaIncio, string FechaFin, int numPag, int allReg, int Cant)
        {
            List<EVenta> oDatos = new List<EVenta>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("FAC_ListaFacBolPOSEnvios");
                    CreateHelper(Connection);
                    AddInParameter("@Filltro", filtro);
                    AddInParameter("@IdEmpresa", Empresa);
                    AddInParameter("@IdSucursal", sucursal);
                    AddInParameter("@FechaInicio", FechaIncio);
                    AddInParameter("@FechaFin", FechaFin);
                    AddInParameter("@numPagina", numPag);
                    AddInParameter("@allReg", allReg);
                    AddInParameter("@iCantFilas", Cant);
                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            EVenta obj = new EVenta();
                            obj.IdVenta = int.Parse(Reader["iIdVenta"].ToString());
                            obj.serie = Reader["sSerie"].ToString();
                            obj.numero = Reader["numero"].ToString();
                            obj.cliente.Nombre = Reader["sCliente"].ToString();
                            obj.cliente.NroDocumento = Reader["sRucCliente"].ToString();
                            obj.moneda.Nombre = Reader["sCodigoISOMoneda"].ToString();
                            obj.Documento.Nombre = Reader["sNombreDoc"].ToString();
                            obj.fechaEmision = Reader["dFechaEmision"].ToString();
                            obj.cantidad = float.Parse(Reader["fCantidadCab"].ToString());
                            obj.grabada = float.Parse(Reader["opeGrabada"].ToString());
                            obj.inafecta = float.Parse(Reader["OpeInafecta"].ToString());
                            obj.exonerada = float.Parse(Reader["OpeExoneradas"].ToString());
                            obj.igv = float.Parse(Reader["nIgvCab"].ToString());
                            obj.total = float.Parse(Reader["nTotalCab"].ToString());
                            obj.descuento = float.Parse(Reader["TotalDescuento"].ToString());
                            obj.TotalR = int.Parse(Reader["Total"].ToString());
                            obj.item = int.Parse(Reader["item"].ToString());
                            obj.TotalPagina = int.Parse(Reader["totalPaginas"].ToString());
                            obj.Nombre = Reader["sNombreCompletoCobrador"].ToString();
                            obj.CostoEnvio = float.Parse(Reader["fCostoEnvio"].ToString());
                            obj.Text = Reader["TipoPago"].ToString();//tipo de pago -- efecto, credito etc
                            obj.observacion = Reader["Observacion"].ToString();
                            obj.Comprobante.Nombre = Reader["Serie"].ToString();
                            obj.sCanalesVenta = Reader["CanalVenta"].ToString();//Canal venta
                            obj.EstadoEnvio = Reader["EstadoEnvio"].ToString();
                            obj.Envio.Nombre = Reader["Nombre"].ToString();
                            obj.fechaEnvio = Reader["dFechaEnvio"].ToString();

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


        public string EstadoEnviadoPOS(int IdVenta, int Empresa, string Motivo, int Flag, string Usuario)
        {
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("LOG_EstadoEnviadoPOS");
                    CreateHelper(Connection);
                    AddInParameter("@IdVenta", IdVenta);
                    AddInParameter("@IdEmpresa", Empresa);
                    AddInParameter("@Motivo", Motivo);
                    AddInParameter("@Flag", Flag);
                    AddInParameter("@usuario", Usuario);
                    AddOutParameter("@Mensaje", (DbType)SqlDbType.VarChar);
                    ExecuteQuery();
                    var smensaje = GetOutput("@Mensaje").ToString();

                    return GetOutput("@Mensaje").ToString();
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

        public List<EVentaDetalle> ImprimirFacBol(int Venta, int Empresa)
        {
            List<EVentaDetalle> oDatos = new List<EVentaDetalle>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("FAC_ImprimirFacBol");
                    CreateHelper(Connection);
                    AddInParameter("@IdVenta", Venta);
                    AddInParameter("@IdEmpresa", Empresa);

                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            EVentaDetalle obj = new EVentaDetalle();
                            obj.Venta.Id = int.Parse(Reader["iidVenta"].ToString());
                            obj.Venta.Comprobante.Codigo = (Reader["sCodigoSunat"].ToString());
                            obj.Empresa.Nombre = Reader["sRazonSocial"].ToString();
                            obj.Empresa.RUC = Reader["sRuc"].ToString();
                            obj.Empresa.Direccion = Reader["sdireccionEmpresa"].ToString();
                            obj.Empresa.EUbigeo.CodigoDepartamento = Reader["sDepartamento"].ToString();
                            obj.Empresa.EUbigeo.CodigoProvincia = Reader["sProvincia"].ToString();
                            obj.Empresa.EUbigeo.CodigoDistrito = Reader["sDistrito"].ToString();
                            obj.Sucursal.Nombre = Reader["sNombreSucursal"].ToString();
                            obj.Sucursal.Direcciones = Reader["sDireccionSucural"].ToString();
                            obj.condicionPago = Reader["sCondicionPago"].ToString();
                            obj.Comprobante.Nombre = Reader["sNombreDoc"].ToString();

                            obj.Transporte = new ETransporte()
                            {
                                IdTrasnporte = int.Parse(Reader["iidTran"].ToString()),
                                Ruc = Reader["RucTrasn"].ToString(),
                                RazonSocial = Reader["RazonTrans"].ToString(),
                                Direccion = Reader["DireccionTrans"].ToString()
                            };
                            obj.Guia.serie = Reader["Guia"].ToString();
                            obj.Venta.resolucion = Reader["Resolucion"].ToString();
                            obj.Venta.DigestValue = Reader["sDigestValue"].ToString();
                            obj.Venta.moneda.Nombre = Reader["Moneda"].ToString();
                            obj.Venta.serie = Reader["sSerie"].ToString();
                            obj.Venta.numero = Reader["numero"].ToString();
                            obj.Venta.cliente.Id = int.Parse(Reader["iIdCliente"].ToString());
                            obj.Venta.cliente.Nombre = Reader["sCliente"].ToString();
                            obj.Venta.cliente.NroDocumento = Reader["sRucCliente"].ToString();
                            obj.Venta.cliente.Direccion = Reader["sDireccionCliente"].ToString();
                            obj.Venta.fechaEmision = Reader["fechaEmision"].ToString();
                            obj.Venta.cantidad = float.Parse(Reader["nCantTotalCab"].ToString());
                            obj.Venta.grabada = float.Parse(Reader["opeGrabada"].ToString());
                            obj.Venta.inafecta = float.Parse(Reader["OpeInafecta"].ToString());
                            obj.Venta.exonerada = float.Parse(Reader["OpeExoneradas"].ToString());
                            obj.Venta.gratuita = float.Parse(Reader["OpeGratuita"].ToString());
                            obj.Venta.igv = float.Parse(Reader["nIgvCab"].ToString());
                            obj.Venta.total = float.Parse(Reader["nTotalCab"].ToString());
                            obj.Venta.descuento = float.Parse(Reader["TotalDescuento"].ToString());
                            obj.material.Nombre = Reader["sNombreProducto"].ToString();
                            obj.material.Unidad.Nombre = Reader["sUnidadMedida"].ToString();
                            obj.cantidad = float.Parse(Reader["nCantidad"].ToString());
                            obj.precio = float.Parse(Reader["nPrecio"].ToString());
                            obj.Importe = float.Parse(Reader["fImporte"].ToString());
                            obj.descuento = float.Parse(Reader["nDescuento"].ToString());
                            obj.Venta.CostoEnvio = float.Parse(Reader["fCostoEnvio"].ToString());
                            obj.IdVentaDetalle = int.Parse(Reader["item"].ToString());
                            obj.material.IdMaterial = int.Parse(Reader["iIdProducto"].ToString());
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


        public List<EVentaDetalle> ImprimirFacBolPOST(int Venta, int Empresa)
        {
            List<EVentaDetalle> oDatos = new List<EVentaDetalle>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("FAC_ImprimirFacBolPOST");
                    CreateHelper(Connection);
                    AddInParameter("@IdVenta", Venta);
                    AddInParameter("@IdEmpresa", Empresa);

                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            EVentaDetalle obj = new EVentaDetalle();
                            obj.Venta.Id = int.Parse(Reader["iidVenta"].ToString());
                            obj.Venta.Comprobante.Codigo = (Reader["sCodigoSunat"].ToString());
                            obj.Empresa.Nombre = Reader["sRazonSocial"].ToString();
                            obj.Empresa.RUC = Reader["sRuc"].ToString();
                            obj.Empresa.Direccion = Reader["sdireccionEmpresa"].ToString();
                            obj.Empresa.EUbigeo.CodigoDepartamento = Reader["sDepartamento"].ToString();
                            obj.Empresa.EUbigeo.CodigoProvincia = Reader["sProvincia"].ToString();
                            obj.Empresa.EUbigeo.CodigoDistrito = Reader["sDistrito"].ToString();
                            obj.Sucursal.Nombre = Reader["sNombreSucursal"].ToString();
                            obj.Sucursal.Direcciones = Reader["sDireccionSucural"].ToString();
                            obj.condicionPago = Reader["sCondicionPago"].ToString();
                            obj.Comprobante.Nombre = Reader["sNombreDoc"].ToString();
                            obj.Venta.resolucion = Reader["Resolucion"].ToString();
                            obj.Venta.DigestValue = Reader["sDigestValue"].ToString();
                            obj.Venta.moneda.Nombre = Reader["Moneda"].ToString();
                            obj.Venta.serie = Reader["sSerie"].ToString();
                            obj.Venta.numero = Reader["numero"].ToString();
                            obj.Venta.cliente.Id = int.Parse(Reader["iIdCliente"].ToString());
                            obj.Venta.cliente.Nombre = Reader["sCliente"].ToString();
                            obj.Venta.cliente.NroDocumento = Reader["sRucCliente"].ToString();
                            obj.Venta.cliente.Direccion = Reader["sDireccionCliente"].ToString();
                            obj.Venta.cliente.Email = Reader["sEmail"].ToString();
                            obj.Venta.fechaEmision = Reader["fechaEmision"].ToString();
                            obj.Venta.cantidad = float.Parse(Reader["nCantTotalCab"].ToString());
                            obj.Venta.grabada = float.Parse(Reader["opeGrabada"].ToString());
                            obj.Venta.inafecta = float.Parse(Reader["OpeInafecta"].ToString());
                            obj.Venta.exonerada = float.Parse(Reader["OpeExoneradas"].ToString());
                            obj.Venta.gratuita = float.Parse(Reader["OpeGratuita"].ToString());
                            obj.Venta.igv = float.Parse(Reader["nIgvCab"].ToString());
                            obj.Venta.total = float.Parse(Reader["nTotalCab"].ToString());
                            obj.Venta.descuento = float.Parse(Reader["TotalDescuento"].ToString());
                            obj.material.Nombre = Reader["sNombreProducto"].ToString();
                            obj.material.Unidad.Nombre = Reader["sUnidadMedida"].ToString();
                            obj.cantidad = float.Parse(Reader["nCantidad"].ToString());
                            obj.precio = float.Parse(Reader["nPrecio"].ToString());
                            obj.Importe = float.Parse(Reader["fImporte"].ToString());
                            obj.descuento = float.Parse(Reader["nDescuento"].ToString());
                            obj.Venta.CostoEnvio = float.Parse(Reader["fCostoEnvio"].ToString());
                            obj.IdVentaDetalle = int.Parse(Reader["item"].ToString());
                            obj.material.IdMaterial = int.Parse(Reader["iIdProducto"].ToString());
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


        public string VerificarStock(int empresa, int almacen, int idSucursal, int IdProducto, double Cantidad, int vstock)
        {
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("LOG_VerificarStockSalida");
                    CreateHelper(Connection);
                    AddInParameter("@IdEmpresa", empresa);
                    AddInParameter("@IdAlmacen", almacen);
                    AddInParameter("@IdSucursal", idSucursal);
                    AddInParameter("@idMaterial", IdProducto);
                    AddInParameter("@iCantidad", Cantidad);
                    AddInParameter("@vstock", vstock);

                    AddOutParameter("@Mensaje", DbType.String);
                    ExecuteQuery();
                    return GetOutput("@Mensaje").ToString();
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


        public byte[] GenerarXml(string xml)
        {
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings
            {
                Indent = true,
                OmitXmlDeclaration = true,
                Encoding = Encoding.GetEncoding("UTF-8")
            };

            MemoryStream ms = new MemoryStream();
            using (XmlWriter writer = XmlWriter.Create(ms, xmlWriterSettings))
            {
                writer.WriteRaw(xml);
            }

            return StreamToByteArray(ms);
        }
        private byte[] StreamToByteArray(Stream inputStream)
        {
            if (!inputStream.CanRead)
            {
                throw new ArgumentException();
            }

            if (inputStream.CanSeek)
            {
                inputStream.Seek(0, SeekOrigin.Begin);
            }

            byte[] output = new byte[inputStream.Length];
            int bytesRead = inputStream.Read(output, 0, output.Length);
            return output;
        }

        private string FirmarXml(string pathxml, String etiquetapadre, string ruta, string claveCertificado)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.PreserveWhitespace = true;
            xmlDoc.Load(pathxml);

            //var ruta = Server.MapPath("~/Certificado/");
            var DigestValue = "";
            var SignatureValue = "";

            //X509Certificate2 myCert = new X509Certificate2(ruta + "Certificado_Sunat_PFX_Comerc.deCombustiblesTriveño_20171018.pfx", "#DESW2ASDZX");

            X509Certificate2 myCert = new X509Certificate2(ruta, claveCertificado);
            RSACryptoServiceProvider rsaKey = (RSACryptoServiceProvider)myCert.PrivateKey;
            SignedXml signedXml = new SignedXml(xmlDoc);

            signedXml.SigningKey = rsaKey;

            Reference reference = new Reference();
            reference.Uri = "";
            XmlDsigEnvelopedSignatureTransform env = new XmlDsigEnvelopedSignatureTransform();
            reference.AddTransform(env);
            signedXml.AddReference(reference);

            KeyInfo KeyInfo = new KeyInfo();
            X509Chain X509Chain = new X509Chain();
            X509Chain.Build(myCert);
            X509ChainElement local_element = X509Chain.ChainElements[0];
            KeyInfoX509Data x509Data = new KeyInfoX509Data(local_element.Certificate);
            String subjectName = local_element.Certificate.Subject;
            x509Data.AddSubjectName(subjectName);
            KeyInfo.AddClause(x509Data);

            signedXml.KeyInfo = KeyInfo;

            signedXml.ComputeSignature();

            XmlElement xmlDigitalSignature = signedXml.GetXml();

            xmlDigitalSignature.Prefix = "ds";
            signedXml.ComputeSignature();
            foreach (XmlNode node in xmlDigitalSignature.SelectNodes("descendant-or-self::*[namespace-uri()='http://www.w3.org/2000/09/xmldsig#']"))
            {
                if (node.LocalName == "Signature")
                {
                    XmlAttribute newAtribute = xmlDoc.CreateAttribute("Id");
                    newAtribute.Value = "SignatureSP";
                    node.Attributes.Append(newAtribute);
                }
            }

            XmlNamespaceManager nsMgr = new XmlNamespaceManager(xmlDoc.NameTable);
            nsMgr.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");

            string l_xpath = "";

            if (pathxml.Contains("-01-")) //factura
            {
                nsMgr.AddNamespace("sac", "urn:sunat:names:specification:ubl:peru:schema:xsd:SunatAggregateComponents-1");
                nsMgr.AddNamespace("ccts", "urn:un:unece:uncefact:documentation:2");
                nsMgr.AddNamespace("tns", "urn:oasis:names:specification:ubl:schema:xsd:Invoice-2");

                nsMgr.AddNamespace("cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
                nsMgr.AddNamespace("udt", "urn:un:unece:uncefact:data:specification:UnqualifiedDataTypesSchemaModule:2");
                nsMgr.AddNamespace("ext", "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2");
                nsMgr.AddNamespace("qdt", "urn:oasis:names:specification:ubl:schema:xsd:QualifiedDatatypes-2");
                nsMgr.AddNamespace("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");
                nsMgr.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");
                //l_xpath = "/tns:Invoice/ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent";
                l_xpath = "/tns:Invoice/ext:UBLExtensions/ext:UBLExtension[2]/ext:ExtensionContent";
            }
            if (pathxml.Contains("-03-")) //BOLETA
            {
                nsMgr.AddNamespace("sac", "urn:sunat:names:specification:ubl:peru:schema:xsd:SunatAggregateComponents-1");
                nsMgr.AddNamespace("ccts", "urn:un:unece:uncefact:documentation:2");
                nsMgr.AddNamespace("tns", "urn:oasis:names:specification:ubl:schema:xsd:Invoice-2");

                nsMgr.AddNamespace("cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
                nsMgr.AddNamespace("udt", "urn:un:unece:uncefact:data:specification:UnqualifiedDataTypesSchemaModule:2");
                nsMgr.AddNamespace("ext", "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2");
                nsMgr.AddNamespace("qdt", "urn:oasis:names:specification:ubl:schema:xsd:QualifiedDatatypes-2");
                nsMgr.AddNamespace("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");
                nsMgr.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");

                l_xpath = "/tns:Invoice/ext:UBLExtensions/ext:UBLExtension[2]/ext:ExtensionContent";
            }
            if (pathxml.Contains("-07-")) //nota de credito
            {
                nsMgr.AddNamespace("sac", "urn:sunat:names:specification:ubl:peru:schema:xsd:SunatAggregateComponents-1");
                nsMgr.AddNamespace("ccts", "urn:un:unece:uncefact:documentation:2");
                nsMgr.AddNamespace("tns", "urn:oasis:names:specification:ubl:schema:xsd:CreditNote-2");

                nsMgr.AddNamespace("cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
                nsMgr.AddNamespace("udt", "urn:un:unece:uncefact:data:specification:UnqualifiedDataTypesSchemaModule:2");
                nsMgr.AddNamespace("ext", "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2");
                nsMgr.AddNamespace("qdt", "urn:oasis:names:specification:ubl:schema:xsd:QualifiedDatatypes-2");
                nsMgr.AddNamespace("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");
                nsMgr.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");

                l_xpath = "/tns:CreditNote/ext:UBLExtensions/ext:UBLExtension[2]/ext:ExtensionContent";
            }
            if (pathxml.Contains("-08-"))//nota de d�bito
            {
                nsMgr.AddNamespace("sac", "urn:sunat:names:specification:ubl:peru:schema:xsd:SunatAggregateComponents-1");
                nsMgr.AddNamespace("ccts", "urn:un:unece:uncefact:documentation:2");
                nsMgr.AddNamespace("tns", "urn:oasis:names:specification:ubl:schema:xsd:DebitNote-2");

                nsMgr.AddNamespace("cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
                nsMgr.AddNamespace("udt", "urn:un:unece:uncefact:data:specification:UnqualifiedDataTypesSchemaModule:2");
                nsMgr.AddNamespace("ext", "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2");
                nsMgr.AddNamespace("qdt", "urn:oasis:names:specification:ubl:schema:xsd:QualifiedDatatypes-2");
                nsMgr.AddNamespace("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");
                nsMgr.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");

                l_xpath = "/tns:DebitNote/ext:UBLExtensions/ext:UBLExtension[2]/ext:ExtensionContent";
            }
            if (pathxml.Contains("RA")) // documento de baja
            {
                nsMgr.AddNamespace("tns", "urn:sunat:names:specification:ubl:peru:schema:xsd:VoidedDocuments-1");
                nsMgr.AddNamespace("sac", "urn:sunat:names:specification:ubl:peru:schema:xsd:SunatAggregateComponents-1");
                nsMgr.AddNamespace("cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
                nsMgr.AddNamespace("ext", "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2");
                nsMgr.AddNamespace("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");
                nsMgr.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");

                l_xpath = "/tns:VoidedDocuments/ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent";
            }
            if (pathxml.Contains("RC"))// documento de revision de boleta
            {
                nsMgr.AddNamespace("tns", "urn:sunat:names:specification:ubl:peru:schema:xsd:SummaryDocuments-1");
                nsMgr.AddNamespace("sac", "urn:sunat:names:specification:ubl:peru:schema:xsd:SunatAggregateComponents-1");
                nsMgr.AddNamespace("cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
                nsMgr.AddNamespace("ext", "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2");
                nsMgr.AddNamespace("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");
                nsMgr.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");

                l_xpath = "/tns:SummaryDocuments/ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent";
            }

            XmlNode counterSignature = xmlDoc.SelectSingleNode(l_xpath, nsMgr);
            counterSignature.AppendChild(xmlDoc.ImportNode(xmlDigitalSignature, true));

            XmlNodeList elemList = xmlDoc.GetElementsByTagName("DigestValue");
            for (int i = 0; i < elemList.Count; i++)
            {
                DigestValue = elemList[i].InnerXml;
            }

            XmlNodeList elemList2 = xmlDoc.GetElementsByTagName("SignatureValue");
            for (int i = 0; i < elemList2.Count; i++)
            {
                SignatureValue = elemList2[i].InnerXml;
            }
            xmlDoc.Save(pathxml);
            return DigestValue + "|" + SignatureValue;
        }
        public List<EUsuario> ListarUsuarios(int IdEmpresa, int IdSucursal, string usuario)
        {
            List<EUsuario> oDatos = new List<EUsuario>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("FAC_ListarUsuario");
                    CreateHelper(Connection);
                    AddInParameter("@iID_Empresa", IdEmpresa);
                    AddInParameter("@IdSucursal", IdSucursal);
                    AddInParameter("@sUsuario", usuario);
                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            EUsuario obj = new EUsuario();
                            obj.Usuario = Reader["sUsuario"].ToString();
                            obj.Nombres = Reader["nomUsuario"].ToString();
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
        public List<EMoneda> ListarMoneda()
        {
            List<EMoneda> oDatos = new List<EMoneda>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("GEN_ListarMoneda");
                    CreateHelper(Connection);
                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            EMoneda obj = new EMoneda();
                            obj.IdMoneda = Convert.ToInt32(Reader["codigo"].ToString());
                            obj.Nombre = Reader["descripcion"].ToString();
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
        public List<EApertura> ListaApertura(int IdEmpresa, int idSucursal, string usuario)
        {
            List<EApertura> oDatos = new List<EApertura>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("FAC_ListarCajaAperturada");
                    CreateHelper(Connection);
                    AddInParameter("@IdEmpresa", IdEmpresa);
                    AddInParameter("@IdSucursal", idSucursal);
                    AddInParameter("@sUsuario", usuario);
                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            EApertura obj = new EApertura();
                            obj.caja.Nombre = Reader["Caja"].ToString();
                            obj.usuario.Nombres = Reader["usuario"].ToString();
                            obj.moneda.Nombre = Reader["Moneda"].ToString();
                            obj.fechaApertura = Reader["dFechaApertura"].ToString();
                            obj.montoApertura = Convert.ToDouble(Reader["fmontoApertura"].ToString());
                            obj.fechaCierre = Reader["dFechaCierre"].ToString();
                            obj.montoCierre = Convert.ToDouble(Reader["fmontoCierre"].ToString());
                            obj.estadoApertura = Reader["sestadoApertura"].ToString();
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
        public string Insertar_AperturaCaja(EApertura Apertura, string Usuario, int Idempresa, int idSucursal)
        {
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("FAC_RegistroAperturaCierre");
                    CreateHelper(Connection);
                    AddInParameter("@idcaja", Apertura.caja.Id);
                    AddInParameter("@sUsuario", Apertura.usuario.Usuario);
                    AddInParameter("@idMoneda", Apertura.moneda.IdMoneda);
                    AddInParameter("@fmonto", Apertura.montoApertura);
                    AddInParameter("@sestadoApertura", Apertura.estadoApertura);
                    AddInParameter("@sCodReg", Usuario);
                    AddInParameter("@idEmpresa", Idempresa);
                    AddInParameter("@idSucursal", idSucursal);
                    AddOutParameter("@Mensaje", (DbType)SqlDbType.VarChar);
                    ExecuteQuery();
                    var smensaje = GetOutput("@Mensaje").ToString();
                    return GetOutput("@Mensaje").ToString();
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
        public List<EApertura> ListaAperturaTodo(int IdEmpresa, int idSucursal, string usuario, int numPag, int allReg, int Cant)
        {
            List<EApertura> oDatos = new List<EApertura>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("FAC_ListarAperturas");
                    CreateHelper(Connection);
                    AddInParameter("@IdEmpresa", IdEmpresa);
                    AddInParameter("@IdSucursal", idSucursal);
                    AddInParameter("@sUsuario", usuario);
                    //AddInParameter("@numPagina", numPag);
                    //AddInParameter("@allReg", allReg);
                    //AddInParameter("@iCantFilas", Cant);

                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            EApertura obj = new EApertura();
                            obj.caja.Nombre = Reader["Caja"].ToString();
                            obj.usuario.Nombres = Reader["usuario"].ToString();
                            obj.moneda.Nombre = Reader["Moneda"].ToString();
                            obj.fechaApertura = Reader["dFechaApertura"].ToString();
                            obj.montoApertura = Convert.ToDouble(Reader["fmontoApertura"].ToString());
                            obj.fechaCierre = Reader["dFechaCierre"].ToString();
                            obj.montoCierre = Convert.ToDouble(Reader["MontoCierre"].ToString());
                            obj.estadoApertura = Reader["estadoApertura"].ToString();
                            //obj.TotalR = Reader["TotalR"].ToString();
                            //obj.TotalPagina = Reader["totalPaginas"].ToString();
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
        //nota de credito debito
        public string InstNotaCreditoDebito(ENotaCreditoDebito oDatos, List<EDetalleCretioDebito> Detalle, string Usuario)
        {
            using (var Connection = GetConnection(BaseDeDatos))
            {
                string sMensaje = "";
                try
                {
                    Connection.Open();
                    SqlTransaction tran = (SqlTransaction)Connection.BeginTransaction();
                    SetQuery("FAC_Inst_NotaCreditoDebito");
                    CreateHelper(Connection, tran);

                    AddInParameter("@iCodigo", oDatos.IdNota);
                    AddInParameter("@IdEmpresa", oDatos.Empresa.Id);
                    AddInParameter("@iID_TipoDocumento", oDatos.TipoDocumento.IdDocumento);
                    AddInParameter("@IdSucursal", oDatos.Sucursal.IdSucursal);
                    AddInParameter("@IdMoneda", oDatos.Moneda.IdMoneda);
                    AddInParameter("@dFechaOperacion", oDatos.FechaEmision);
                    AddInParameter("@Idcliente", oDatos.cliente.Id);
                    AddInParameter("@fGravadas", oDatos.grabada);
                    AddInParameter("@fInafectadas", oDatos.inafecta);
                    AddInParameter("@fExoneradas", oDatos.exonerada);
                    AddInParameter("@fTotalDescuento", oDatos.descuento);
                    AddInParameter("@fIGV", oDatos.igv);
                    AddInParameter("@fCantidad", oDatos.cantidad);
                    AddInParameter("@fTotalVenta", oDatos.totalVenta);
                    AddInParameter("@IdAlmacen", oDatos.Almacen.IdAlmacen);
                    AddInParameter("@sAfectaStcok", oDatos.AfectaStock);
                    AddInParameter("@iID_TipoNotaCredito", oDatos.NotaCreditoDebito.Credito, AllowNull);
                    AddInParameter("@iID_TipoNotaDebito", oDatos.NotaCreditoDebito.Debito, AllowNull);
                    AddInParameter("@sSerieNotaCreditoDebito", oDatos.Serie);
                    AddInParameter("@sNumeracionNotaCreditoDebito", oDatos.Numero);
                    AddInParameter("@sMotivoNotaCreditoDebito", oDatos.Motivo);
                    AddInParameter("@sObservacion", oDatos.Observacion, AllowNull);
                    AddInParameter("@IdVenta", oDatos.Venta.Id);
                    AddInParameter("@Usuario", Usuario);
                    AddInParameter("@Tipoventa", oDatos.TipoVenta);
                    AddOutParameter("@Mensaje", (DbType)SqlDbType.VarChar);
                    ExecuteQuery();
                    sMensaje = GetOutput("@Mensaje").ToString();
                    string[] vMensaje = sMensaje.Split('|');
                    if (vMensaje[0].Equals("success"))
                    {
                        string[] vValues = vMensaje[2].Split('&');
                        int iIVenta = int.Parse(vValues[0]);
                        string[] dMensaje;
                        foreach (EDetalleCretioDebito oDetalle in Detalle)
                        {
                            SetQuery("FAC_Inst_NotaCreditoDebitoDetalle");
                            CreateHelper(Connection, tran);
                            AddInParameter("@IdNotCreditoDebito", iIVenta);
                            AddInParameter("@IdEmpresa", oDatos.Empresa.Id);
                            AddInParameter("@IdSucursal", oDatos.Sucursal.IdSucursal);
                            AddInParameter("@iID_Producto", oDetalle.Material.Id);
                            AddInParameter("@sNombreProducto", oDetalle.Material.Nombre);
                            AddInParameter("@iCantidad", oDetalle.cantidad);
                            AddInParameter("@sUnidadMedida", oDetalle.Material.Unidad.Nombre);
                            AddInParameter("@iPrecioUnitarioIncIGV", oDetalle.precio);
                            AddInParameter("@fDescuento", oDetalle.descuento);
                            AddInParameter("@ImportTotal", oDetalle.Importe);
                            AddInParameter("@IdAlmacen", oDatos.Almacen.IdAlmacen);
                            AddInParameter("@sAfectaStcok", oDatos.AfectaStock);
                            AddInParameter("@sCodReg", Usuario);
                            AddInParameter("@Tipoventa", oDatos.TipoVenta);
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

        public List<ENotaCreditoDebito> ListaNotaCreditoDebito(string filtro, int IdEmpresa, int idSucursal, string FechaIncio, string FechaFin, int numPag, int allReg, int Cant)
        {
            List<ENotaCreditoDebito> oDatos = new List<ENotaCreditoDebito>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("FAC_ListaNotaCreditoDebito");
                    CreateHelper(Connection);
                    AddInParameter("@Filtro", filtro);
                    AddInParameter("@IdEmpresa", IdEmpresa);
                    AddInParameter("@IdSucursal", idSucursal);
                    AddInParameter("@FechaInicio", FechaIncio);
                    AddInParameter("@FechaFin", FechaFin);
                    AddInParameter("@numPagina", numPag);
                    AddInParameter("@allReg", allReg);
                    AddInParameter("@iCantFilas", Cant);
                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            ENotaCreditoDebito obj = new ENotaCreditoDebito();
                            obj.IdNota = int.Parse(Reader["iID_NotaCreditoDebito"].ToString());
                            obj.TotalPagina = int.Parse(Reader["totalPaginas"].ToString());
                            obj.TotalReg = int.Parse(Reader["Total"].ToString());
                            obj.item = int.Parse(Reader["item"].ToString());
                            obj.sFechaEmision = (Reader["FechaEmison"].ToString());
                            obj.TipoDocumento.Nombre = (Reader["Documento2"].ToString());
                            obj.Moneda.Nombre = (Reader["sMoneda"].ToString());
                            obj.cliente.Nombre = (Reader["sNombreCliente"].ToString());
                            obj.cliente.NroDocumento = (Reader["sRucDNICliente"].ToString());
                            obj.grabada = float.Parse(Reader["fGravadas"].ToString());
                            obj.exonerada = float.Parse(Reader["fExoneradas"].ToString());
                            obj.inafecta = float.Parse(Reader["fInafectadas"].ToString());
                            obj.igv = float.Parse(Reader["fIGV"].ToString());
                            obj.totalVenta = float.Parse(Reader["fTotalVenta"].ToString());
                            obj.descuento = float.Parse(Reader["fTotalDescuento"].ToString());
                            obj.Serie = (Reader["sSerieNotaCreditoDebito"].ToString());
                            obj.Numero = (Reader["Numero"].ToString());
                            obj.Motivo = (Reader["sMotivoNotaCreditoDebito"].ToString());
                            obj.Venta.Documento.Nombre = (Reader["Documento"].ToString());
                            obj.Venta.serie = (Reader["sSerie"].ToString());
                            obj.Venta.numero = (Reader["iNumero"].ToString());
                            obj.sTipoVenta = (Reader["TipoVenta"].ToString());
                            obj.TipoVenta = int.Parse(Reader["iTipoVenta"].ToString());
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


        public List<ENotaCreditoDebito> ObtenerCreditoDebito(int IdNota, int IdEmpresa, int idSucursal, int Tipo)
        {
            List<ENotaCreditoDebito> oDatos = new List<ENotaCreditoDebito>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("FAC_ObtenerCredtioDebito");
                    CreateHelper(Connection);
                    AddInParameter("@IdNota", IdNota);
                    AddInParameter("@IdEmpresa", IdEmpresa);
                    AddInParameter("@IdSucursal", idSucursal);
                    AddInParameter("@iTipoVenta", Tipo);
                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            ENotaCreditoDebito obj = new ENotaCreditoDebito();
                            obj.TipoDocumento.Nombre = (Reader["Documento2"].ToString());
                            obj.Sucursal.IdSucursal = int.Parse(Reader["iID_Sucursal"].ToString());
                            obj.Sucursal.Nombre = (Reader["sNombreSucursal"].ToString());
                            obj.Sucursal.Direcciones = (Reader["sDireccionSucural"].ToString());
                            obj.sFechaEmision = (Reader["FechaEmison"].ToString());
                            obj.Moneda.Nombre = (Reader["sMoneda"].ToString());
                            obj.cliente.Nombre = (Reader["sNombreCliente"].ToString());
                            obj.cliente.NroDocumento = (Reader["sRucDNICliente"].ToString());
                            obj.cliente.Direccion = (Reader["sDireccionCliente"].ToString());
                            obj.cliente.Email = (Reader["sEmail"].ToString());
                            obj.TipoDocumento.IdDocumento = int.Parse(Reader["iID_TipoDoc"].ToString());
                            obj.cantidad = float.Parse(Reader["fCantidad"].ToString());
                            obj.grabada = float.Parse(Reader["fGravadas"].ToString());
                            obj.inafecta = float.Parse(Reader["fInafectadas"].ToString());
                            obj.exonerada = float.Parse(Reader["fExoneradas"].ToString());
                            obj.descuento = float.Parse(Reader["fTotalDescuento"].ToString());
                            obj.igv = float.Parse(Reader["fIGV"].ToString());
                            obj.totalVenta = float.Parse(Reader["fTotalVenta"].ToString());
                            obj.Serie = (Reader["sSerieNotaCreditoDebito"].ToString());
                            obj.Numero = (Reader["Numero"].ToString());
                            obj.Motivo = (Reader["sMotivoNotaCreditoDebito"].ToString());
                            obj.Venta.serie = (Reader["sSerie"].ToString());
                            obj.Venta.numero = (Reader["NumeroFac"].ToString());
                            obj.Venta.Documento.Nombre = (Reader["Documento"].ToString());
                            /*detalle*/

                            obj.detalle.cantidad = float.Parse((Reader["iCantidad"].ToString()));
                            obj.detalle.Material.Nombre = (Reader["sNombreProducto"].ToString());
                            obj.detalle.Material.Unidad.Nombre = (Reader["sUnidadMedida"].ToString());
                            obj.detalle.precio = float.Parse(Reader["iPrecioUnitarioIncIGV"].ToString());
                            obj.detalle.descuento = float.Parse(Reader["fDescuento"].ToString());
                            obj.detalle.Importe = float.Parse(Reader["ImportTotal"].ToString());

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
        public string elimiarVenta(EVenta oDatos, List<EVentaDetalle> Detalle, string usuario)
        {
            using (var Connection = GetConnection(BaseDeDatos))
            {
                string sMensaje = "";

                try
                {
                    Connection.Open();
                    SqlTransaction tran = (SqlTransaction)Connection.BeginTransaction();
                    SetQuery("FAC_ElimiarVenta");
                    CreateHelper(Connection, tran);
                    AddInParameter("@Idventa", oDatos.IdVenta);
                    AddInParameter("@usuario", usuario);
                    AddInParameter("@motivo", oDatos.motivo);
                    AddInParameter("@IdEmpresa", oDatos.empresa.Id);
                    AddInParameter("@Idsucursal", oDatos.Sucursal.IdSucursal);
                    AddInParameter("@Almacen", oDatos.Almacen.IdAlmacen);
                    AddInParameter("@tipoVenta", oDatos.Id);
                    AddInParameter("@Estado", oDatos.EstadoEnvio);
                    AddOutParameter("@Mensaje", (DbType)SqlDbType.VarChar);
                    ExecuteQuery();
                    sMensaje = GetOutput("@Mensaje").ToString();
                    string[] vMensaje = sMensaje.Split('|');
                    if (vMensaje[0].Equals("success"))
                    {
                        string[] dMensaje;

                        foreach (EVentaDetalle oDetalle in Detalle)
                        {
                            SetQuery("FAC_ElimiarVentaDet");
                            CreateHelper(Connection, tran);
                            AddInParameter("@Idventa", oDatos.IdVenta);
                            AddInParameter("@IdEmpresa", oDatos.empresa.Id);
                            AddInParameter("@IdSucursal", oDatos.Sucursal.IdSucursal);
                            AddInParameter("@idMat", oDetalle.material.Id);
                            AddInParameter("@fcantidad", oDetalle.cantidad);
                            AddInParameter("@Almacen", oDatos.Almacen.IdAlmacen);
                            AddInParameter("@sCodReg", usuario);
                            AddInParameter("@TipoVenta", oDatos.Id);
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

        public List<EVenta> ListaPrecio(int comienzo, int iMedia, int producto, int cliente)
        {
            List<EVenta> oDatos = new List<EVenta>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("FAC_ListaPrecio");
                    CreateHelper(Connection);
                    AddInParameter("@iComienzo", comienzo);
                    AddInParameter("@iMedida", iMedia);
                    AddInParameter("@iidProducto", producto);
                    AddInParameter("@Cliente", cliente);

                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            EVenta obj = new EVenta();
                            obj.item = int.Parse(Reader["item"].ToString());
                            obj.TotalR = int.Parse(Reader["Total"].ToString());
                            obj.DetalleVenta = new EVentaDetalle()
                            {
                                precio = float.Parse(Reader["nPrecio"].ToString()),
                                cantidad = float.Parse(Reader["nCantidad"].ToString()),
                                material = new EMaterial()
                                {
                                    IdMaterial = int.Parse(Reader["iIdProducto"].ToString()),
                                }
                            };
                            obj.fechaEmision = Reader["dFecReg"].ToString();

                            obj.moneda.Nombre = Reader["Moneda"].ToString();
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
