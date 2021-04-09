using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIS.Data;
using SIS.Entity;
using SIS.Factory;

namespace SIS.Business
{
    public class BVenta
    {
        private static BVenta Instancia;
        private DVenta Data = DVenta.ObtenerInstancia(DataBase.SqlServer);
        public static BVenta ObtenerInstancia()
        {
            if (Instancia == null)
            {
                Instancia = new BVenta();
            }
            return Instancia;
        }

        public string RegistrarVenta(EVenta oDatos, List<EVentaDetalle> Detalle, string Usuario)
        {
            try
            {
                return Data.RegistrarVenta(oDatos, Detalle, Usuario);
            }
            catch (Exception ex)
            {
                return ex.Message; ;
            }
        }
        public string RegistrarPost(EVenta oDatos, List<EPago> pago, List<EVentaDetalle> Detalle, string Usuario)
        {
            try
            {
                return Data.RegistrarPost(oDatos, pago, Detalle, Usuario);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public EGeneral SerieNuroDocumento(int IdComprobante, int Empresa, int sucursal)
        {
            try
            {
                return Data.SerieNuroDocumento(IdComprobante, Empresa, sucursal);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public List<EVenta> ListaVenta(string filtro, int Empresa, int sucursal, string FechaIncio, string FechaFin, int numPag, int allReg, int Cant)
        {
            try
            {
                return Data.ListaVenta(filtro, Empresa, sucursal, FechaIncio, FechaFin, numPag, allReg, Cant);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public List<EVentaDetalle> ImprimirFacBolPOST(int Venta, int Empresa)
        {
            try
            {
                return Data.ImprimirFacBolPOST(Venta, Empresa);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<EVenta> ListaVentaPOST(string filtro, int Empresa, int sucursal, string FechaIncio, string FechaFin, int numPag, int allReg, int Cant)
        {
            try
            {
                return Data.ListaVentaPOST(filtro, Empresa, sucursal, FechaIncio, FechaFin, numPag, allReg, Cant);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<EVenta> ListaVentaPOSEnvios(string filtro, int Empresa, int sucursal, string FechaIncio, string FechaFin, int numPag, int allReg, int Cant)
        {
            try
            {
                return Data.ListaVentaPOSEnvios(filtro, Empresa, sucursal, FechaIncio, FechaFin, numPag, allReg, Cant);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        
        public string EstadoEnviadoPOS(int IdVenta, int Empresa, string Motivo, int Flag, string Usuario)
        {
            try
            {
                return Data.EstadoEnviadoPOS(IdVenta, Empresa, Motivo, Flag, Usuario);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }

        }

        public List<EVentaDetalle> ImprimirFacBol(int Venta, int Empresa)
        {
            try
            {
                return Data.ImprimirFacBol(Venta, Empresa);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public string VerificarStock(int empresa, int almacen, int idSucursal, int IdProducto, double Cantidad,int vstock)
        {
            try
            {
                return Data.VerificarStock(empresa, almacen, idSucursal, IdProducto, Cantidad, vstock);
            }
            catch (Exception ex)
            {
                return ex.Message; ;
            }

        }
        public List<EUsuario> ListarUsuarios(int IdEmpresa, int IdSucursal, string usuario)
        {
            return Data.ListarUsuarios(IdEmpresa, IdSucursal, usuario);
        }
        public List<EMoneda> ListarMoneda()
        {
            return Data.ListarMoneda();
        }
        public List<EApertura> ListaApertura(int IdEmpresa, int idSucursal, string usuario)
        {
            return Data.ListaApertura(IdEmpresa, idSucursal, usuario);
        }
        public string Insertar_AperturaCaja(EApertura Apertura, string Usuario, int Idempresa, int idSucursal)
        {
            return Data.Insertar_AperturaCaja(Apertura, Usuario, Idempresa, idSucursal);
        }
        public List<EApertura> ListaAperturaTodo(int IdEmpresa, int idSucursal, string usuario, int numPag, int allReg, int Cant)
        {
            return Data.ListaAperturaTodo(IdEmpresa, idSucursal, usuario, numPag, allReg, Cant);
        }
        public string InstNotaCreditoDebito(ENotaCreditoDebito oDatos, List<EDetalleCretioDebito> Detalle, string Usuario)
        {
            try
            {
                return Data.InstNotaCreditoDebito(oDatos, Detalle, Usuario);
            }
            catch (Exception ex)
            {

                return ex.Message;
            }

        }
        public List<ENotaCreditoDebito> ListaNotaCreditoDebito(string filtro, int IdEmpresa, int idSucursal, string FechaIncio, string FechaFin, int numPag, int allReg, int Cant)
        {
            try
            {
                return Data.ListaNotaCreditoDebito(filtro, IdEmpresa, idSucursal, FechaIncio, FechaFin, numPag, allReg, Cant);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public List<ENotaCreditoDebito> ObtenerCreditoDebito(int IdNota, int IdEmpresa, int idSucursal, int Tipo)
        {
            try
            {
                return Data.ObtenerCreditoDebito(IdNota, IdEmpresa, idSucursal, Tipo);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public string elimiarVenta(EVenta oDatos, List<EVentaDetalle> Detalle, string usuario)
        {
            try
            {
                return Data.elimiarVenta(oDatos, Detalle, usuario);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<EVenta> ListaPrecio(int comienzo, int iMedia, int producto, int cliente)
        {
            try
            {
                return Data.ListaPrecio(comienzo, iMedia, producto, cliente);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
