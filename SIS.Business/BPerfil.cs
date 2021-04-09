using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIS.Factory;
using SIS.Data;
using SIS.Entity;
namespace SIS.Business
{
    public class BPerfil
    {
        private static BPerfil Instancia;
        private DPerfil Data = DPerfil.ObtenerInstancia(DataBase.SqlServer);

        public static BPerfil ObtenerInstancia()
        {
            if (Instancia == null)
            {
                Instancia = new BPerfil();
            }
            return Instancia;
        }

        public List<EPerfil> ListarPerfil(int Empresa)
        {
            try
            {
                return Data.Listar(Empresa);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }


        public string Insertar_Perfil(EPerfil oDato, string Usuario)
        {
            try
            {
                return Data.Insertar_Perfil(oDato, Usuario);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }

        public EPerfil ListaEditPerfil(int ID)
        {
            try
            {
                return Data.ListaEditPerfil(ID);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public List<EMenu> ListarAccesosPorPerfil(int id, int Perfil)
        {
            try
            {
                return Data.ListarAccesosPorPerfil(id, Perfil);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public EPerfil BuscarPerfilPorId(long Id)
        {
            try
            {
                return Data.BuscarPerfilPorId(Id);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public string ActualizarAccesosPorPerfil(int Id, string Menus)
        {
            try
            {
                return Data.ActualizarAccesosPorPerfil(Id, Menus);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
    }
}
