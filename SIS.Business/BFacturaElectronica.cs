using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIS.Data;
using SIS.Factory;
using SIS.Entity;
using System.Data;
using System.Xml;
using System.Xml.Linq;
using System.IO;

namespace SIS.Business
{
    public class BFacturaElectronica
    {

        private static BFacturaElectronica Instancia;
        private DFacturaElectronica Data = DFacturaElectronica.ObtenerInstancia(DataBase.SqlServer);

        public static BFacturaElectronica ObtenerInstancia()
        {
            if (Instancia == null)
            {
                Instancia = new BFacturaElectronica();
            }
            return Instancia;
        }
        public List<EVenta> ListarComprobante(int Comienzo, int Medida, int empresa, int Sucursal, string FechaEmi, int tipo)
        {
            try
            {
                return Data.ListarComprobante(Comienzo, Medida, empresa, Sucursal, FechaEmi, tipo);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public List<EVenta> ListarBoletaResumen(int Comienzo, int Medida, int empresa, int Sucursal, string FechaEmi, int tipo)
        {
            try
            {
                return Data.ListarBoletaResumen(Comienzo, Medida, empresa, Sucursal, FechaEmi, tipo);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public string GeneraResumen(int empresa, string fecha, string usuario)
        {
            try
            {
                return Data.GeneraResumen(empresa, fecha, usuario);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public string GeneraResumenDetalle(int idResumen, int idVenta, int tipoVenta, string usuario, int tipo)
        {
            try
            {
                return Data.GeneraResumenDetalle(idResumen, idVenta, tipoVenta, usuario, tipo);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }

        public List<EVenta> FAC_ArmarXMLResumen(int idResumen)
        {
            try
            {
                return Data.FAC_ArmarXMLResumen(idResumen);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public string ActualizarResumen(int IdResumen, string codigo)
        {
            try
            {
                return Data.ActualizarResumen(IdResumen, codigo);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }

        public List<EVentaDetalle> oListaEnvioSunat(int IdFactura, int tipo)
        {
            try
            {
                return Data.oListaEnvioSunat(IdFactura, tipo);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public string RegistrarPost(int IdVenta, string comprobante, int tipo)
        {
            try
            {
                return Data.RegistrarPost(IdVenta, comprobante, tipo);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public List<EGuia> ListarGuia(int Comienzo, int Medida, int empresa, int Sucursal, string FechaEmi)
        {
            try
            {
                return Data.ListarGuia(Comienzo, Medida, empresa, Sucursal, FechaEmi);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }

        }
        public string ActulizarGuia(int IdGuia, string comprobante)
        {
            try
            {
                return Data.ActulizarGuia(IdGuia, comprobante);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }

        }
        public List<EVenta> ListarComprobanteAnulados(int Comienzo, int Medida, int empresa, int Sucursal, string FechaEmi, int tipo)
        {
            try
            {
                return Data.ListarComprobanteAnulados(Comienzo, Medida, empresa, Sucursal, FechaEmi, tipo);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public string GenerarComucacionbaja(int IdEmpresa, string fecha, string usuario)
        {
            try
            {
                return Data.GenerarComucacionbaja(IdEmpresa, fecha, usuario);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public string GenerarComucacionbajaDetalle(int Idcomunicacion, int IdVenta, int TipoVenta, int Tipo, string usuario)
        {
            try
            {
                return Data.GenerarComucacionbajaDetalle(Idcomunicacion, IdVenta, TipoVenta, Tipo, usuario);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public List<Ecomunicacion> armarBaja(int IdComunicacion)
        {
            try
            {
                return Data.armarBaja(IdComunicacion);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }

        }
        public string ActulizarBaja(int Id, string comprobante)
        {
            try
            {
                return Data.ActulizarBaja(Id, comprobante);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }

        }
    }
}
