using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIS.Factory;
using SIS.Entity;
using System.Data;
namespace SIS.Data
{
    public class DPerfil : DBHelper
    {
        private static DPerfil Instancia;
        private DataBase BaseDeDatos;

        public DPerfil(DataBase BaseDeDatos) : base(BaseDeDatos)
        {
            this.BaseDeDatos = BaseDeDatos;
        }

        public static DPerfil ObtenerInstancia(DataBase BaseDeDatos)
        {
            if (Instancia == null)
            {
                Instancia = new DPerfil(BaseDeDatos);
            }
            return Instancia;
        }
        public List<EPerfil> Listar(int Empresa)
        {
            List<EPerfil> oDatos = new List<EPerfil>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("GEN_ListaPerfil");
                    CreateHelper(Connection);
                    //AddInParameter("@IdEmpresa", Empresa);
      
                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            EPerfil oPerfil = new EPerfil();
                            oPerfil.Id = int.Parse(Reader["IdPerfil"].ToString());
                            oPerfil.Nombre = Reader["Perfil"].ToString();
                            oDatos.Add(oPerfil);
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


        public string Insertar_Perfil(EPerfil oDato, string Usuario)
        {
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("GEN_InsPerfil");
                    CreateHelper(Connection);
                    AddInParameter("@Idperfil", oDato.Id);
                    AddInParameter("@IdEmpresa", oDato.Empresa.Id);
                    AddInParameter("@Nombre", oDato.NombrePerfil);
                    AddInParameter("@Usuario", Usuario);
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

        public EPerfil ListaEditPerfil(int ID)
        {
            EPerfil obj = null;
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("GEN_ObtenerPerfil");
                    CreateHelper(Connection);
                    AddInParameter("@IdPerfil", ID);
                    using (var Reader = ExecuteReader())
                    {
                        if (Reader.Read())
                        {
                            obj = new EPerfil();
                            obj.Id = int.Parse(Reader["IdPerfil"].ToString());
                            obj.Nombre = Reader["Perfil"].ToString();
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


        public List<EMenu> ListarAccesosPorPerfil(int id, int Perfil)
        {
            List<EMenu> lPerfiles = new List<EMenu>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("GEN_AccesosPorPerfilId");
                    CreateHelper(Connection);
                    AddInParameter("@iIdPerfilUsuario", id);
                    AddInParameter("@Perfil", Perfil);
                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            EMenu oPerfil = new EMenu();
                            oPerfil.Id = int.Parse(Reader["Menu"].ToString());
                            oPerfil.Padre = new EMenu
                            {
                                Id = int.Parse(Reader["MenuPadre"].ToString())
                            };
                            oPerfil.Nombre = Reader["DescripcionMenu"].ToString();
                            oPerfil.TieneAcceso = bool.Parse(Reader["TieneAcceso"].ToString());
                            lPerfiles.Add(oPerfil);
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
            return lPerfiles;
        }

      
        public EPerfil BuscarPerfilPorId(long Id)
        {
            EPerfil oPerfil = null;
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("GEM_PerfilPorID");
                    CreateHelper(Connection);
                    AddInParameter("@iIdPerfilUsuario", Id);
                    using (var Reader = ExecuteReader())
                    {

                        if (Reader.Read())
                        {

                            oPerfil = new EPerfil();
                            oPerfil.Respuesta = int.Parse(Reader["Respuesta"].ToString());
                            oPerfil.NombrePerfil = Reader["NombrePerfil"].ToString();
                            oPerfil.Id = int.Parse(Reader["Id"].ToString());
                            if (oPerfil.Respuesta == 2)
                            {

                                oPerfil.IdEmpresaHolding = int.Parse(Reader["IdEmpresaHolding"].ToString());

                                oPerfil.Usuario = (Reader["IdUsuarioReg"].ToString());
                                oPerfil.FechaHoraReg = Reader["FechaHoraReg"].ToString();
                                oPerfil.EstadoB = (Reader["Estado"].ToString());
                                oPerfil.Menu = int.Parse(Reader["Menu"].ToString());
                            }


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
            return oPerfil;
        }
       
        public string ActualizarAccesosPorPerfil(int Id, string Menus)
        {
            using (var Connection = GetConnection(BaseDeDatos))
            {
                var Mensaje = "";
                try
                {
                    Connection.Open();
                    SetQuery("GEN_Act_AccesosPorPerfilId");
                    CreateHelper(Connection);
                    AddInParameter("@iIdPerfilUsuario", Id);
                    AddInParameter("@vMenu", Menus);
                    AddOutParameter("@Mensaje", System.Data.DbType.String);
                    ExecuteQuery();
                    Mensaje = GetOutput("@Mensaje").ToString();
                    return Mensaje;
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
    }
}
