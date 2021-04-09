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
    public class BCotizacion
    {
        private static BCotizacion Instancia;
        private DCotizacion Data = DCotizacion.ObtenerInstancia(DataBase.SqlServer);
        public static BCotizacion ObtenerInstancia()
        {
            if (Instancia == null)
            {
                Instancia = new BCotizacion();
            }
            return Instancia;
        }
        public string RegistrarCotizacion(ECotizacion oDatos, List<EDetCotizacion> Detalle, string Usuario)
        {
            try
            {
                return Data.RegistrarCotizacion(oDatos, Detalle, Usuario);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public List<ECotizacion> ListaCotizacion(string cliente, int moneda, int Empresa, string FechaIncio, string FechaFin, int numPag, int allReg, int Cant)
        {
            try
            {
                return Data.ListaCotizacion(cliente, moneda, Empresa, FechaIncio, FechaFin, numPag, allReg, Cant);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }

        public List<EDetCotizacion> ImpirmirCotizacion(int Idoc)
        {
            try
            {
                return Data.ImpirmirCotizacion(Idoc);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }

        }
    }
}
