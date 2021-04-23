using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIS.Entity;
using SIS.Factory;
using System.Data;

namespace SIS.Data
{
    public class DGeneral : DBHelper
    {
        private static DGeneral Instancia;
        private DataBase BaseDeDatos;

        public DGeneral(DataBase BaseDeDatos) : base(BaseDeDatos)
        {
            this.BaseDeDatos = BaseDeDatos;
        }

        public static DGeneral ObtenerInstancia(DataBase BaseDeDatos)
        {
            if (Instancia == null)
            {
                Instancia = new DGeneral(BaseDeDatos);
            }
            return Instancia;
        }

        public List<EGeneral> CBOLista(int Id, int Empresa)
        {
            List<EGeneral> oDatos = new List<EGeneral>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("LOG_CBOLista");
                    CreateHelper(Connection);
                    AddInParameter("@IdFlag", Id);
                    AddInParameter("@IdEmpresa", Empresa);
                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            EGeneral obj = new EGeneral();
                            obj.Id = int.Parse(Reader["Id"].ToString());
                            obj.Nombre = (Reader["Nombre"].ToString());
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
        public List<EGeneral> CBOListaId(int flag, int Id, int Empresa)
        {
            List<EGeneral> oDatos = new List<EGeneral>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("LOG_CBOListaId");
                    CreateHelper(Connection);
                    AddInParameter("@IdFlag", flag);
                    AddInParameter("@Id", Id);
                    AddInParameter("@IdEmpresa", Empresa);
                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            EGeneral obj = new EGeneral();
                            obj.Id = int.Parse(Reader["Id"].ToString());
                            obj.Nombre = (Reader["Nombre"].ToString());
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

        public List<EGeneral> CBOListaIdAlmacen(int sucursal, int Empresa, string usuario)
        {
            List<EGeneral> oDatos = new List<EGeneral>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("LOG_ListaCBOAlmacen");
                    CreateHelper(Connection);
                    AddInParameter("@IdSucursal", sucursal);
                    AddInParameter("@IdEmpresa", Empresa);
                    AddInParameter("@sLogin", usuario);
                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            EGeneral obj = new EGeneral();
                            obj.Id = int.Parse(Reader["Id"].ToString());
                            obj.Nombre = (Reader["Nombre"].ToString());
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

        public List<EGeneral> ListaComboAlmacenes(int Empresa, string usuario)
        {
            List<EGeneral> oDatos = new List<EGeneral>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("LOG_ListaCBOAlmacenAll");
                    CreateHelper(Connection);
                    AddInParameter("@IdEmpresa", Empresa);
                    AddInParameter("@sLogin", usuario);
                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            EGeneral obj = new EGeneral();
                            obj.Id = int.Parse(Reader["Id"].ToString());
                            obj.Nombre = (Reader["Nombre"].ToString());
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

       
        public List<EUbigeo> ListarUbigeo(string Acction, string idPais, string IdDep, string IdProv, string IdDis)
        {
            List<EUbigeo> lUbigeo = new List<EUbigeo>();
            using (var Connection = GetConnection(this.BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("LOG_Cbo_Lista_de_Ubicaciones");
                    CreateHelper(Connection);
                    AddInParameter("@saccion", Acction);
                    AddInParameter("@sIdPais", idPais);
                    AddInParameter("@sIdDep", IdDep);
                    AddInParameter("@sIdProv", IdProv);
                    AddInParameter("@sIdDis", IdDis);
                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            EUbigeo oUbigeo = new EUbigeo();
                            oUbigeo.CodigoInei = Reader["CODIGO"].ToString();
                            oUbigeo.Nombre = Reader["DESCRIPCION"].ToString();
                            lUbigeo.Add(oUbigeo);
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
            }
            return lUbigeo;
        }
      
 

     
        public List<EGeneral> ListaEmpresaLogin()
        {
            List<EGeneral> oDatos = new List<EGeneral>();
            using (var Connection = GetConnection(this.BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("GEN_ListaEmpresaLogin");
                    CreateHelper(Connection);
                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            EGeneral obj = new EGeneral();
                            obj.Text = (Reader["sRUC"].ToString());
                            obj.Nombre = Reader["sRazonSocialE"].ToString();
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
            }
            return oDatos;
        }
        public List<EMenu> ListaMenu()
        {
            List<EMenu> oDatos = new List<EMenu>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("GEN_ListaMenuCab");
                    CreateHelper(Connection);
                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            EMenu Menu = new EMenu();
                            Menu.Id = int.Parse(Reader["iID_Menu"].ToString());
                            Menu.Text = Reader["sDescripcionMenu"].ToString();
                            Menu.Orden = int.Parse(Reader["iOrden"].ToString());
                            Menu.Nombre = Reader["NomMenu"].ToString();
                            Menu.Vista = Reader["Vista"].ToString();
                            Menu.Controlador = Reader["Controlador"].ToString();
                            Menu.sEstado = Reader["sEstado"].ToString();
                            Menu.Icono = Reader["sIcono"].ToString();
                            oDatos.Add(Menu);
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
        public EMenu ListaEditMenu(int ID)
        {
            EMenu obj = null;
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("GEN_ObtenerMenu");
                    CreateHelper(Connection);
                    AddInParameter("@idMenu", ID);
                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            obj = new EMenu();
                            obj.Id = int.Parse(Reader["iID_Menu"].ToString());
                            obj.Orden = int.Parse(Reader["iOrden"].ToString());

                            obj.IdPadre = int.Parse(Reader["iID_MenuPadre"].ToString());

                            obj.Nombre = Reader["NomMenu"].ToString();
                            obj.Vista = Reader["Vista"].ToString();
                            obj.Controlador = Reader["sControladorMenu"].ToString();
                            obj.sEstado = Reader["sEstado"].ToString();
                            obj.Icono = Reader["sIcono"].ToString();
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
                return obj;

            }
        }

        public string Insertar_UpdateMenu(EMenu Menu, string Usuario)
        {
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("GEM_InstMenu");
                    CreateHelper(Connection);
                    AddInParameter("@iID_Menu", Menu.Id);
                    AddInParameter("@iID_MenuPadre", Menu.IdPadre);
                    AddInParameter("@iOrden", Menu.Orden);
                    AddInParameter("@sDescripcionMenu", Menu.Nombre);
                    AddInParameter("@sVistaMenu", Menu.Vista, AllowNull);
                    AddInParameter("@sControladorMenu", Menu.Controlador, AllowNull);
                    AddInParameter("@sEstado", Menu.sEstado, AllowNull);
                    AddInParameter("@User", Usuario);
                    AddInParameter("@icono", Menu.Icono, AllowNull);
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

        public List<ETareas> ListaTarea(string stado, string etiqueta, string tarea, string label, string FechaIncio, string FechaFin, int numPag, int allReg, int Cant)
        {
            List<ETareas> oDatos = new List<ETareas>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("ListaTarea");
                    CreateHelper(Connection);
                    AddInParameter("@stado", stado);
                    AddInParameter("@etiqueta", etiqueta);
                    AddInParameter("@tarea", tarea);
                    AddInParameter("@label", label); 
                    AddInParameter("@FechaInicio", FechaIncio);
                    AddInParameter("@FechaFin", FechaFin);
                    AddInParameter("@numPagina", numPag);
                    AddInParameter("@allReg", allReg);
                    AddInParameter("@iCantFilas", Cant);
                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            ETareas obj = new ETareas();
                            obj.intcheck = int.Parse(Reader["intcheck"].ToString());
                            obj.item = int.Parse(Reader["item"].ToString());
                            obj.stado = Reader["status"].ToString();
                            obj.etiqueta = Reader["etiqueta"].ToString();
                            obj.tarea = Reader["tarea"].ToString();
                            obj.label = Reader["label"].ToString();
                            obj.descripcion = Reader["description"].ToString();
                            obj.id = (Reader["id"].ToString()); 
                          
                            obj.direcion = Reader["direccion"].ToString();
                            obj.inicio = Reader["inicio"].ToString();
                            obj.fin = Reader["fin"].ToString();
                            obj.llegada = Reader["llegada"].ToString();
                            obj.duracion = Reader["duracion"].ToString();
                           
                            obj.total = int.Parse(Reader["Total"].ToString());
                            obj.totalPagina = int.Parse(Reader["totalPaginas"].ToString()); 
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
