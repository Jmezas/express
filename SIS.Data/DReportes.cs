using SIS.Entity;
using SIS.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SIS.Data
{
    public class DReportes : DBHelper
    {
        private static DReportes Instancia;
        private DataBase BaseDeDatos;

        public DReportes(DataBase BaseDeDatos) : base(BaseDeDatos)
        {
            this.BaseDeDatos = BaseDeDatos;
        }

        public static DReportes ObtenerInstancia(DataBase BaseDeDatos)
        {
            if (Instancia == null)
            {
                Instancia = new DReportes(BaseDeDatos);
            }
            return Instancia;
        }
        
        public List<EVentaDetalle> ResumenVentasXProductoPOS(string filtro, int Empresa, string FechaIncio, string FechaFin, int numPag, int allReg, int Cant, string vendedor, int sucursal)
        {
            List<EVentaDetalle> oDatos = new List<EVentaDetalle>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("REP_ResumenVentasPos");
                    CreateHelper(Connection);
                    AddInParameter("@Filtro", filtro);
                    AddInParameter("@IdEmpresa", Empresa);
                    AddInParameter("@FechaInicio", FechaIncio);
                    AddInParameter("@FechaFin", FechaFin);
                    AddInParameter("@numPagina", numPag);
                    AddInParameter("@allReg", allReg);
                    AddInParameter("@iCantFilas", Cant);
                    AddInParameter("@IdSucursal", sucursal);
                    AddInParameter("@vendedor", vendedor);

                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            EVentaDetalle obj = new EVentaDetalle();

                            obj.Venta.Id = int.Parse(Reader["iIdVenta"].ToString());
                            obj.Venta.vendedor = Reader["sNombreCompletoCobrador"].ToString();
                            obj.Sucursal.Nombre = Reader["sNombreSucursal"].ToString();
                            obj.Venta.fechaEmision = Reader["dFecEmision"].ToString();
                            obj.Venta.serie = Reader["sSerie"].ToString();
                            obj.Venta.numero = Reader["numero"].ToString();
                            obj.Venta.moneda.Nombre = Reader["sCodigoISOMoneda"].ToString();
                            obj.Venta.cliente.NroDocumento = Reader["sRucCliente"].ToString();
                            obj.Venta.cliente.Nombre = Reader["sCliente"].ToString();
                            //obj.Venta.CostoEnvio = float.Parse(Reader["fCostoEnvio"].ToString());
                            //obj.Venta.fechaEnvio = float.Parse(Reader["fCostoEnvio"].ToString()) ==0 ?"-":Reader["dFechaEnvio"].ToString();
                            //obj.Venta.Envio.Nombre = Reader["Nombre"].ToString();
                            //obj.Venta.sCanalesVenta = Reader["CanalVenta"].ToString();
                            obj.material.Codigo = Reader["sCodMaterial"].ToString();
                            obj.material.Nombre = Reader["sNomMaterial"].ToString();
                            obj.material.Unidad.Nombre = Reader["sUnidadMedida"].ToString();
                            obj.material.Marca.Nombre = Reader["marca"].ToString();
                            obj.material.Modelo.Nombre = Reader["modelo"].ToString();
                            obj.material.Etipo.Nombre = Reader["tipo"].ToString();
                            obj.material.EColor.Nombre = Reader["color"].ToString();
                            obj.material.Talla.Nombre = Reader["talla"].ToString();
                            obj.material.Temporada.Nombre = Reader["temporada"].ToString();
                            obj.material.Categoria.Nombre = Reader["categoria"].ToString();
                            obj.cantidad = float.Parse(Reader["nCantidad"].ToString());
                            obj.precio = float.Parse(Reader["nPrecio"].ToString());
                            obj.descuento = float.Parse(Reader["nDescuento"].ToString());
                            obj.Importe = float.Parse(Reader["fImporte"].ToString());
                            obj.TotalPagina = int.Parse(Reader["totalPaginas"].ToString());
                            obj.TotalR = int.Parse(Reader["Total"].ToString());
                            obj.item = int.Parse(Reader["item"].ToString());

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

        public List<EVentaDetalle> ResumenVentasXProductoRegular(string filtro, int Empresa, string FechaIncio, string FechaFin, int numPag, int allReg, int Cant, string vendedor, int sucursal)
        {
            List<EVentaDetalle> oDatos = new List<EVentaDetalle>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("REP_ResumenVentasRegular");
                    CreateHelper(Connection);
                    AddInParameter("@Filtro", filtro);
                    AddInParameter("@IdEmpresa", Empresa);
                    AddInParameter("@FechaInicio", FechaIncio);
                    AddInParameter("@FechaFin", FechaFin);
                    AddInParameter("@numPagina", numPag);
                    AddInParameter("@allReg", allReg);
                    AddInParameter("@iCantFilas", Cant);
                    AddInParameter("@IdSucursal", sucursal);
                    AddInParameter("@vendedor", vendedor);

                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            EVentaDetalle obj = new EVentaDetalle();

                            obj.Venta.Id = int.Parse(Reader["iIdVenta"].ToString());
                            obj.Venta.vendedor = Reader["sNombreCompletoCobrador"].ToString();
                            obj.Sucursal.Nombre = Reader["sNombreSucursal"].ToString();
                            obj.Venta.fechaEmision = Reader["dFecEmision"].ToString();
                            obj.Venta.serie = Reader["sSerie"].ToString();
                            obj.Venta.numero = Reader["numero"].ToString();
                            obj.Venta.moneda.Nombre = Reader["sCodigoISOMoneda"].ToString();
                            obj.Venta.cliente.NroDocumento = Reader["sRucCliente"].ToString();
                            obj.Venta.cliente.Nombre = Reader["sCliente"].ToString();
                            //obj.Venta.CostoEnvio = float.Parse(Reader["fCostoEnvio"].ToString());
                            //obj.Venta.fechaEnvio = float.Parse(Reader["fCostoEnvio"].ToString()) == 0 ? "-" : Reader["dFechaEnvio"].ToString();
                            //obj.Venta.Envio.Nombre = Reader["Nombre"].ToString();
                            //obj.Venta.sCanalesVenta = Reader["CanalVenta"].ToString();
                            obj.material.Codigo = Reader["sCodMaterial"].ToString();
                            obj.material.Nombre = Reader["sNomMaterial"].ToString();
                            obj.material.Unidad.Nombre = Reader["sUnidadMedida"].ToString();
                            obj.material.Marca.Nombre = Reader["marca"].ToString();
                            obj.material.Modelo.Nombre = Reader["modelo"].ToString();
                            obj.material.Etipo.Nombre = Reader["tipo"].ToString();
                            obj.material.EColor.Nombre = Reader["color"].ToString();
                            obj.material.Talla.Nombre = Reader["talla"].ToString();
                            obj.material.Temporada.Nombre = Reader["temporada"].ToString();
                            obj.material.Categoria.Nombre = Reader["categoria"].ToString();
                            obj.cantidad = float.Parse(Reader["nCantidad"].ToString());
                            obj.precio = float.Parse(Reader["nPrecio"].ToString());
                            obj.descuento = float.Parse(Reader["nDescuento"].ToString());
                            obj.Importe = float.Parse(Reader["fImporte"].ToString());
                            obj.TotalPagina = int.Parse(Reader["totalPaginas"].ToString());
                            obj.TotalR = int.Parse(Reader["Total"].ToString());
                            obj.item = int.Parse(Reader["item"].ToString());

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

        public List<EVenta> VentasGeneralesPos(string filtro, int Empresa, int sucursal, string FechaIncio, string FechaFin, int numPag, int allReg, int Cant)
        {
            List<EVenta> oDatos = new List<EVenta>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("REP_ventasPos");
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

        public List<EVentaDetalle> VentasPorTipoPagoPos(string filtro, int Empresa, int sucursal, string FechaIncio, string FechaFin, int numPag, int allReg, int Cant)
        {
            List<EVentaDetalle> oDatos = new List<EVentaDetalle>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("REP_ventasTipoPagoPos");
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
                            EVentaDetalle obj = new EVentaDetalle();
                            obj.Venta.Id = int.Parse(Reader["IdVenta"].ToString());
                            obj.Sucursal.Nombre = Reader["sNombreSucursal"].ToString();
                            obj.Venta.fechaEmision = Reader["dFecEmision"].ToString();
                            obj.Venta.serie = Reader["sSerie"].ToString();
                            obj.Venta.numero = Reader["numero"].ToString();
                            obj.Venta.moneda.Nombre = Reader["moneda"].ToString();
                            obj.precio = float.Parse(Reader["monto"].ToString());
                            obj.Venta.Text = Reader["TipoPago"].ToString();
                            obj.Venta.TextBanco = Reader["banco"].ToString();
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

        public List<EVentaDetalle> VentasPorTipoPagoRegular(string filtro, int Empresa, int sucursal, string FechaIncio, string FechaFin, int numPag, int allReg, int Cant)
        {
            List<EVentaDetalle> oDatos = new List<EVentaDetalle>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("REP_ventasTipoPagoRegular");
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
                            EVentaDetalle obj = new EVentaDetalle();
                            obj.Venta.Id = int.Parse(Reader["IdVenta"].ToString());
                            obj.Sucursal.Nombre = Reader["sNombreSucursal"].ToString();
                            obj.Venta.fechaEmision = Reader["dFecEmision"].ToString();
                            obj.Venta.serie = Reader["sSerie"].ToString();
                            obj.Venta.numero = Reader["numero"].ToString();
                            obj.Venta.moneda.Nombre = Reader["moneda"].ToString();
                            obj.precio = float.Parse(Reader["monto"].ToString());
                            obj.Venta.Text = Reader["TipoPago"].ToString();
                            obj.Venta.TextBanco = Reader["banco"].ToString();
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

        public List<EVenta> VentasGeneralesRegular(string filtro, int Empresa, int sucursal, string FechaIncio, string FechaFin, int numPag, int allReg, int Cant)
        {
            List<EVenta> oDatos = new List<EVenta>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("REP_ventasRegulares");
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

        public List<EProductoMasVendido> ListaProductoMasVendido(int empresa)
        {
            List<EProductoMasVendido> oDatos = new List<EProductoMasVendido>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("LOG_productoMasVendido");
                    CreateHelper(Connection);
                    AddInParameter("@IdEmpresa", empresa);
                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            EProductoMasVendido obj = new EProductoMasVendido();
                            obj.id = int.Parse(Reader["iIdProducto"].ToString());
                            obj.codigo = (Reader["sCodMaterial"].ToString());
                            obj.producto = (Reader["sNomMaterial"].ToString());
                            obj.cantidad = float.Parse(Reader["cantidad"].ToString());
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
