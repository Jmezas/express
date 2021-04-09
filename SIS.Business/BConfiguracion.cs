using SIS.Data;
using SIS.Factory;
using SIS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIS.Business
{
    public class BConfiguracion
    {
        private static BConfiguracion Instancia;
        private DConfiguracion Data = DConfiguracion.ObtenerInstancia(DataBase.SqlServer);
        public static BConfiguracion ObtenerInstancia()
        {
            if (Instancia == null)
            {
                Instancia = new BConfiguracion();
            }
            return Instancia;
        }

        public List<EConfiguracion> ListarConfiguraciones(int Flag, int empresa, int sucursal)
        {
            try
            {
                return Data.ListarConfiguraciones(Flag, empresa,sucursal);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }

        //actualizamos Permitir Stock
        public string ActualizarConfiguracion(int Flag,EConfiguracion oDatos, string Usuario)
        {
            try
            {
                return Data.ActualizarConfiguracion(Flag, oDatos, Usuario);
            }
            catch (Exception ex)
            {

                return ex.Message;
            }

        }
    }
}
