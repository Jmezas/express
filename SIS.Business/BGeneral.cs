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
    public class BGeneral
    {
        private static BGeneral Instancia;
        private DGeneral Data = DGeneral.ObtenerInstancia(DataBase.SqlServer);
        public static BGeneral ObtenerInstancia()
        {
            if (Instancia == null)
            {
                Instancia = new BGeneral();
            }
            return Instancia;
        }

        public List<EGeneral> CBOLista(int Id, int Empresa)
        {
            try
            {
                return Data.CBOLista(Id, Empresa);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public List<EGeneral> CBOListaId(int flag, int Id, int Empresa)
        {
            try
            {
                return Data.CBOListaId(flag, Id, Empresa);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }

        }
        public List<EGeneral> CBOListaIdAlmacen(int sucursal, int Empresa, string usuario)
        {

            try
            {
                return Data.CBOListaIdAlmacen(sucursal, Empresa, usuario);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public List<EGeneral> ListaComboAlmacenes(int Empresa, string usuario)
        {

            try
            {
                return Data.ListaComboAlmacenes(Empresa, usuario);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
      
        public List<EUbigeo> ListarUbigeo(string Acction, string idPais, string IdDep, string IdProv, string IdDis)
        {
            try
            {
                return Data.ListarUbigeo(Acction, idPais, IdDep, IdProv, IdDis);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
       
       
        
        public List<EGeneral> ListaEmpresaLogin()
        {
            try
            {
                return Data.ListaEmpresaLogin();
            }
            catch (Exception Exception)
            {
                throw Exception;
            }

        }

        public List<EMenu> ListaMenu()
        {
            try
            {
                return Data.ListaMenu();
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
             
        }
        public EMenu ListaEditMenu(int ID)
        {
            try
            {
                return Data.ListaEditMenu(ID);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public string Insertar_UpdateMenu(EMenu Menu, string Usuario)
        {
            try
            {
                return Data.Insertar_UpdateMenu(Menu,Usuario);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public List<ETareas> ListaTarea(string stado, string etiqueta, string tarea, string label, string FechaIncio, string FechaFin, int numPag, int allReg, int Cant)
        {
            try
            {
                return Data.ListaTarea(  stado,   etiqueta,   tarea,   label, FechaIncio, FechaFin, numPag, allReg, Cant);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public List<ETareas> ListaTareados(string stado, string etiqueta, string tarea, string label, string FechaIncio, string FechaFin, int numPag, int allReg, int Cant)
        {
            try
            {
                return Data.ListaTareados(stado, etiqueta, tarea, label, FechaIncio, FechaFin, numPag, allReg, Cant);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
    }
}
