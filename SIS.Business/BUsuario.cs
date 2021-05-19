using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIS.Business;
using SIS.Data;
using SIS.Entity;
using SIS.Factory;

namespace SIS.Business
{
    public class BUsuario
    {
        private static BUsuario Instancia;
        private DUsuario Data = DUsuario.ObtenerInstancia(DataBase.SqlServer);

        public static BUsuario ObtenerInstancia()
        {
            if (Instancia == null)
            {
                Instancia = new BUsuario();
            }
            return Instancia;
        }
        public EUsuario Login(string sUsuario, string sClave)
        {
            try
            {
                return Data.Login(sUsuario, sClave);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public EUsuario BuscarPorUsuario(string sUsuario,string Ruc)
        {
            try
            {
                return Data.BuscarPorUsuario(sUsuario, Ruc);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public List<EMenu> ListarMenuPorUsuario(string sUsuario,string ruc)
        {
            try
            {
                return Data.ListarMenuPorUsuario(sUsuario, ruc);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public string InstUsuario(EUsuario Obj, string Usuario)
        {
            try
            {
                return Data.InstUsuario(Obj, Usuario);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public EUsuario ListaEditUsuario(int IdUnidad, int Empresa)
        {

            try
            {
                return Data.ListaEditUsuario(IdUnidad, Empresa);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public List<EUsuario> ListaUsuario(int Empresa)
        {
            try
            {
                return Data.ListaUsuario(Empresa);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public string CambiarPassword(int IdUsuario, string newpass, string oldpass)
        {
            try
            {
                return Data.CambiarPassword(IdUsuario, newpass, oldpass);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public string Eliminar(int Id, int IdFlag, string Usuario)
        {
            try
            {
                return Data.Eliminar(Id, IdFlag, Usuario);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
    }
}
