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
    public class DUsuario : DBHelper
    {
        private static DUsuario Instancia;
        private DataBase BaseDeDatos;

        public DUsuario(DataBase BaseDeDatos) : base(BaseDeDatos)
        {
            this.BaseDeDatos = BaseDeDatos;
        }

        public static DUsuario ObtenerInstancia(DataBase BaseDeDatos)
        {
            if (Instancia == null)
            {
                Instancia = new DUsuario(BaseDeDatos);
            }
            return Instancia;
        }


        public EUsuario Login(string sUsuario, string sClave)
        {
            EUsuario oUsuario = null;
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("GEN_LOGIN");
                    CreateHelper(Connection);
                    AddInParameter("@vUsuario", sUsuario);
                    AddInParameter("@vClave", sClave);
                    using (var Reader = ExecuteReader())
                    {
                        if (Reader.Read())
                        {
                            oUsuario = new EUsuario();
                            oUsuario.Respuesta = int.Parse(Reader["Respuesta"].ToString());
                            if (oUsuario.Respuesta == 2)
                            {
                                oUsuario.Id = int.Parse(Reader["IdUsuario"].ToString());
                                oUsuario.Usuario = sUsuario;
                                oUsuario.Nombre = Reader["Nombre"].ToString();
                                oUsuario.ApellidoPaterno = Reader["ApellidoPaterno"].ToString();
                                oUsuario.ApellidoMaterno = Reader["ApellidoMaterno"].ToString();
                                oUsuario.Imagen = Reader["sImgen"].ToString();
                                oUsuario.Perfil = new EPerfil
                                {
                                    Id = int.Parse(Reader["IdPerfil"].ToString()),
                                    Nombre = Reader["Perfil"].ToString(),
                                    EmpresaHolding = new EEmpresaHolding
                                    {
                                        Empresa = new EEmpresa
                                        {
                                            Id = int.Parse(Reader["IdEmpresa"].ToString()),
                                            Nombre = Reader["Empresa"].ToString(),
                                            RazonSocial = Reader["EmpresaRazonSocial"].ToString(),
                                            RUC = Reader["EmpresaRUC"].ToString(),
                                            DireccionTexto = Reader["EmpresaDomicilio"].ToString() 
                                        }
                                    }
                                };
                                //oUsuario.Sucursal.IdSucursal =int.Parse(Reader["IdSucursal"].ToString());
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
            return oUsuario;
        }
        public EUsuario BuscarPorUsuario(string sUsuario, string Ruc)
        {
            EUsuario oUsuario = null;
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("GEN_UsuarioPorUsuario");
                    CreateHelper(Connection);
                    AddInParameter("@vUsuario", sUsuario); 
                    using (var Reader = ExecuteReader())
                    {
                        if (Reader.Read())
                        {
                            oUsuario = new EUsuario();
                            oUsuario.Id = int.Parse(Reader["IdUsuario"].ToString());
                            oUsuario.Nombre = Reader["Nombre"].ToString();
                            oUsuario.ApellidoPaterno = Reader["ApellidoPaterno"].ToString();
                            oUsuario.ApellidoMaterno = Reader["ApellidoMaterno"].ToString();
                            oUsuario.Usuario = Reader["Usuario"].ToString();
                            oUsuario.Perfil = new EPerfil
                            {
                                Id = int.Parse(Reader["IdPerfil"].ToString()),
                                Nombre = Reader["Perfil"].ToString(),

                            };
                            oUsuario.Empresa = new EEmpresa
                            {
                                Id = int.Parse(Reader["IdEmpresa"].ToString()),
                                Nombre = Reader["Empresa"].ToString(),
                                RazonSocial = Reader["EmpresaRazonSocial"].ToString(),
                                RUC = Reader["EmpresaRUC"].ToString(),
                                DireccionTexto = Reader["EmpresaRUC"].ToString()
                            };
 
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
            return oUsuario;
        }

        public List<EMenu> ListarMenuPorUsuario(string sUsuario,string sRuc)
        {
            List<EMenu> lMenu = new List<EMenu>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("GEN_MenuPorUsuario");
                    CreateHelper(Connection);
                    AddInParameter("@vUsuario", sUsuario);
                    //AddInParameter("@Empresa", sRuc);
                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            EMenu oMenu = new EMenu();
                            oMenu.Id = int.Parse(Reader["IdMenu"].ToString());
                            oMenu.Nombre = Reader["Menu"].ToString();
                            oMenu.Orden = int.Parse(Reader["Orden"].ToString());
                            oMenu.Controlador = Reader["Controlador"].ToString();
                            oMenu.Vista = Reader["Vista"].ToString();
                            oMenu.Icono = Reader["sIcono"].ToString();
                            oMenu.Padre = new EMenu
                            {
                                Id = !string.IsNullOrEmpty(Reader["IdPadre"].ToString()) ? int.Parse(Reader["IdPadre"].ToString()) : -1
                            };
                            lMenu.Add(oMenu);
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
            return lMenu;
        }

        

        public string InstUsuario(EUsuario Obj, string Usuario)
        {
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("GEN_InsUsuario");
                    CreateHelper(Connection);
                    AddInParameter("@IdUsuario", Obj.Id);
                    AddInParameter("@IdPerfil", Obj.Perfil.IdPerfil);
                    AddInParameter("@sUsuario", Obj.Usuario);
                    AddInParameter("@sClave", Obj.Password);
                    AddInParameter("@sNombre", Obj.Nombre);
                    AddInParameter("@ApePaterno", Obj.ApellidoPaterno);
                    AddInParameter("@ApeMaterno", Obj.ApellidoMaterno);
                    AddInParameter("@IdDocumento", Obj.TipoDocumento.Id);
                    AddInParameter("@sNumero", Obj.NroDocumento);
                    AddInParameter("@sImagen", Obj.Imagen,AllowNull);
                    AddInParameter("@sDireccion", Obj.Direccion, AllowNull);
                    AddInParameter("@sTelefono", Obj.Telefono, AllowNull);
                    AddInParameter("@Correo", Obj.CorreoElectronico,AllowNull);
                    //AddInParameter("@IdSucursal", Obj.Sucursal.codigo);
                    AddInParameter("@Ruc", Obj.Empresa.RUC);
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


        public EUsuario ListaEditUsuario(int IdUsuario, int Empresa)
        {
            EUsuario obj = null;
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("GEN_ObtenerUsuarioEdit");
                    CreateHelper(Connection);
                    AddInParameter("@IdUsuario", IdUsuario);
                    AddInParameter("@IdEmpresa", Empresa);
                    using (var Reader = ExecuteReader())
                    {
                        if (Reader.Read())
                        {
                            obj = new EUsuario();
                            obj.Id = int.Parse(Reader["iId_Usuario"].ToString());
                            obj.Perfil = new EPerfil()
                            {
                                Id = int.Parse(Reader["iId_perfilusuario"].ToString())
                            };

                            obj.TipoDocumento.Id = int.Parse(Reader["iID_DocumnetoI"].ToString());
                            obj.Usuario = Reader["sUsuario"].ToString();
                            obj.Password = Reader["Pass"].ToString();
                            obj.Nombre = Reader["sNombres"].ToString();
                            obj.ApellidoPaterno = Reader["sApePaterno"].ToString();
                            obj.ApellidoMaterno = Reader["sApeMaterno"].ToString();
                            obj.NroDocumento = Reader["sNumDocumentoI"].ToString();
                            obj.Direccion = Reader["sDireccion"].ToString();
                            obj.Telefono = Reader["sTelefono"].ToString();
                            obj.CorreoElectronico = Reader["sCorreoElectronico"].ToString();
                            obj.Imagen = Reader["sImgen"].ToString();
                            //obj.Sucursal.IdSucursal = int.Parse(Reader["IdSucursal"].ToString());
                            //obj.Sucursal.codigo = (Reader["sIdSucursal"].ToString());
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
        public List<EUsuario> ListaUsuario(int Empresa)
        {
            List<EUsuario> oDatos = new List<EUsuario>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("GEN_ListaUsuarios");
                    CreateHelper(Connection);
                    AddInParameter("@IdEmpresa", Empresa);
                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            EUsuario obj = new EUsuario();
                            obj.Id = int.Parse(Reader["iID_Usuario"].ToString());

                            obj.Perfil = new EPerfil()
                            {
                                Nombre = (Reader["Perfil"].ToString())
                            };

                            obj.NroDocumento = Reader["NumDoc"].ToString();
                            obj.Nombre = Reader["Nombres"].ToString();
                            obj.Usuario = Reader["sUsuario"].ToString();
                            obj.CorreoElectronico = Reader["sCorreoElectronico"].ToString();
                            obj.Direccion = Reader["sDireccion"].ToString();
                            obj.Telefono = Reader["sTelefono"].ToString();
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
        public string CambiarPassword(int IdUsuario, string newpass, string oldpass)
        {
            using (var Connection = GetConnection(this.BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("GEN_Act_CambioPassword");
                    CreateHelper(Connection);
                    AddInParameter("@vOldPass", oldpass);
                    AddInParameter("@vNewPass", newpass);
                    AddInParameter("@iIDUsuario", IdUsuario);
                    AddOutParameter("@Mensaje", (DbType)SqlDbType.VarChar);
                    ExecuteQuery();
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
        public string Eliminar(int Id, int IdFlag, string Usuario)
        {
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("LOG_EliminarGeneral");
                    CreateHelper(Connection);
                    AddInParameter("@Id", Id);
                    AddInParameter("@IdFlag", IdFlag);
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
    }
}
