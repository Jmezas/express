using SIS.Data;
using SIS.Entity;
using SIS.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIS.Business
{
    public class BReportes
    {
        private static BReportes Instancia;
        private DReportes Data = DReportes.ObtenerInstancia(DataBase.SqlServer);
        public static BReportes ObtenerInstancia()
        {
            if (Instancia == null)
            {
                Instancia = new BReportes();
            }
            return Instancia;
        }

        public List<EVentaDetalle> ResumenVentasXProductoPOS(string filtro, int Empresa, string FechaIncio, string FechaFin, int numPag, int allReg, int Cant, string vendedor, int sucursal)
        {
            try
            {
                return Data.ResumenVentasXProductoPOS(filtro, Empresa,FechaIncio, FechaFin, numPag, allReg, Cant, vendedor, sucursal);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<EVentaDetalle> ResumenVentasXProductoRegular(string filtro, int Empresa, string FechaIncio, string FechaFin, int numPag, int allReg, int Cant, string vendedor, int sucursal)
        {
            try
            {
                return Data.ResumenVentasXProductoRegular(filtro, Empresa, FechaIncio, FechaFin, numPag, allReg, Cant, vendedor, sucursal);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<EVenta> VentasGeneralesPos(string filtro, int Empresa, int sucursal, string FechaIncio, string FechaFin, int numPag, int allReg, int Cant)
        {
            try
            {
                return Data.VentasGeneralesPos(filtro, Empresa, sucursal, FechaIncio, FechaFin, numPag, allReg, Cant);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<EVenta> VentasGeneralesRegular(string filtro, int Empresa, int sucursal, string FechaIncio, string FechaFin, int numPag, int allReg, int Cant)
        {
            try
            {
                return Data.VentasGeneralesRegular(filtro, Empresa, sucursal, FechaIncio, FechaFin, numPag, allReg, Cant);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<EVentaDetalle> VentasPorTipoPagoPos(string filtro, int Empresa, int sucursal, string FechaIncio, string FechaFin, int numPag, int allReg, int Cant)
        {
            try
            {
                return Data.VentasPorTipoPagoPos(filtro, Empresa, sucursal, FechaIncio, FechaFin, numPag, allReg, Cant);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<EVentaDetalle> VentasPorTipoPagoRegular(string filtro, int Empresa, int sucursal, string FechaIncio, string FechaFin, int numPag, int allReg, int Cant)
        {
            try
            {
                return Data.VentasPorTipoPagoRegular(filtro, Empresa, sucursal, FechaIncio, FechaFin, numPag, allReg, Cant);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<EProductoMasVendido> ListaProductoMasVendido(int empresa)
        {
            try
            {
                return Data.ListaProductoMasVendido(empresa);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }

        }

        
    }
}
