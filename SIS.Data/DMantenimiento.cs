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
    public class DMantenimiento : DBHelper
    {
        private static DMantenimiento Instancia;
        private DataBase BaseDeDatos;

        public DMantenimiento(DataBase BaseDeDatos) : base(BaseDeDatos)
        {
            this.BaseDeDatos = BaseDeDatos;
        }

        public static DMantenimiento ObtenerInstancia(DataBase BaseDeDatos)
        {
            if (Instancia == null)
            {
                Instancia = new DMantenimiento(BaseDeDatos);
            }
            return Instancia;
        }

        public string ActualizarDescuentos(List<EMaterial> sLista, string Usuario)
        {
            using (var Connection = GetConnection(BaseDeDatos))
            {
                Connection.Open();
                string sMensaje = "";
                string[] dMensaje;
                string alerta;

                try
                {
                    foreach (EMaterial Material in sLista)
                    {
                        SetQuery("LOG_ActualizarDescuentos");
                        CreateHelper(Connection);
                        AddInParameter("@iIdMat", Material.IdMaterial);
                        AddInParameter("@IdEmpresa", Material.Empresa.Id);
                        AddInParameter("@Descuento", Material.Descuento);
                        AddInParameter("@sCodReg", Usuario);
                        AddOutParameter("@Mensaje", (DbType)SqlDbType.VarChar);
                        ExecuteQuery();
                        dMensaje = GetOutput("@Mensaje").ToString().Split('|');
                        if (!dMensaje[0].Equals("success"))
                        {
                            //throw new Exception();
                            return alerta = dMensaje[0] + "|" + dMensaje[1];
                        }

                        sMensaje = dMensaje[0] + "|" + dMensaje[1];
                    }
                    return sMensaje;
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

        #region Material 
        public string InstMaterial(EMaterial Material, string Usuario)
        {
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("LOG_InstMaterial");
                    CreateHelper(Connection);
                    AddInParameter("@iIdMat", Material.IdMaterial);
                    AddInParameter("@IdEmpresa", Material.Empresa.Id);
                    AddInParameter("@iIdTipo", Material.Id);
                    AddInParameter("@sCodMaterial", Material.Codigo);
                    AddInParameter("@sNomMaterial", Material.Nombre);
                    AddInParameter("@sDescripcion", Material.Descripcion, AllowNull);
                    AddInParameter("@iIdUM", Material.Unidad.IdUnidad, AllowNull);
                    AddInParameter("@iIdMar", Material.Marca.IdMarca, AllowNull);
                    AddInParameter("@iModelo", Material.Modelo.IdModelo, AllowNull);
                    AddInParameter("@fPrecioVenta", Material.PrecioVenta);
                    AddInParameter("@fPrecioCompra", Material.PrecioCompra);
                    AddInParameter("@iIdCategoria", Material.Categoria.IdCateogira);
                    AddInParameter("@iIdSubCateogria", Material.SubCateoria.IdSubCategoira);
                    AddInParameter("@idGenero", Material.genero.IdGenero, AllowNull);
                    AddInParameter("@idTipoDes", Material.Etipo.IdTipo, AllowNull);
                    AddInParameter("@idColor", Material.EColor.IdColor, AllowNull);
                    AddInParameter("@iIdTemporada", Material.Temporada.IdTemporada, AllowNull);
                    AddInParameter("@iIdTalla", Material.Talla.IdTalla, AllowNull);
                    AddInParameter("@PorcenDscto", Material.Descuento);
                    AddInParameter("@sCodReg", Usuario);
                    AddInParameter("@iTipoOperacion", Material.TipoOperacion.IdTipoOperacion, AllowNull);

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
        public EMaterial ListaEditMaterial(int IdMaterial, int Empresa)
        {
            EMaterial obj = null;
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("LOG_ObtenerMaterial");
                    CreateHelper(Connection);
                    AddInParameter("@IdMaterial", IdMaterial);
                    AddInParameter("@IdEmpresa", Empresa);
                    using (var Reader = ExecuteReader())
                    {
                        if (Reader.Read())
                        {
                            obj = new EMaterial();
                            obj.IdMaterial = int.Parse(Reader["iIdMat"].ToString());
                            obj.Id = int.Parse(Reader["iIdTipo"].ToString());
                            obj.Codigo = Reader["sCodMaterial"].ToString();
                            obj.Nombre = Reader["sNomMaterial"].ToString();
                            obj.Descripcion = Reader["sDescripcion"].ToString();
                            obj.Unidad.IdUnidad = int.Parse(Reader["iIdUM"].ToString());
                            obj.Unidad.Nombre = Reader["sNombreUMD"].ToString();
                            obj.Marca.IdMarca = int.Parse(Reader["iIdMar"].ToString());
                            obj.Modelo.IdModelo = int.Parse(Reader["sModelo"].ToString());
                            obj.PrecioCompra = float.Parse(Reader["fPrecioCompra"].ToString());
                            obj.PrecioVenta = float.Parse(Reader["fPrecioVenta"].ToString());
                            obj.Categoria.IdCateogira = int.Parse(Reader["iIdCategoria"].ToString());
                            obj.SubCateoria.IdSubCategoira = int.Parse(Reader["iIdSubCateogria"].ToString());
                            obj.genero.IdGenero = int.Parse(Reader["idGenero"].ToString());
                            obj.Etipo.IdTipo = int.Parse(Reader["idTipoDes"].ToString());
                            obj.EColor.IdColor = int.Parse(Reader["idColor"].ToString());
                            obj.Temporada.IdTemporada = int.Parse(Reader["iIdTemporada"].ToString());
                            obj.Talla.IdTalla = int.Parse(Reader["iIdTalla"].ToString());
                            obj.Descuento = double.Parse(Reader["PorcenDscto"].ToString());
                            obj.TipoOperacion.IdTipoOperacion = int.Parse(Reader["iTipoOperacion"].ToString());
                            obj.TipoOperacion.Nombre = Reader["DesOperacion"].ToString();

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
        public List<EMaterial> ListaMaterial(int Empresa /*, DataTableAjaxPostModel datable*/)
        {
            List<EMaterial> oDatos = new List<EMaterial>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("LOG_ListaMaterial");
                    CreateHelper(Connection);
                    AddInParameter("@IdEmpresa", Empresa);
                    //AddInParameter("@PageNumber", datable.draw);
                    //AddInParameter("@RowspPage", datable.length);
                    //AddInParameter("@Search", (datable.search.value?null:'));
                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            EMaterial Material = new EMaterial();
                            Material.IdMaterial = int.Parse(Reader["iIdMat"].ToString());
                            Material.sCodigo = (Reader["Codigo"].ToString());
                            Material.Text = (Reader["Tipo"].ToString());
                            Material.Codigo = Reader["sCodMaterial"].ToString();
                            Material.Nombre = Reader["sNomMaterial"].ToString();
                            Material.Descripcion = Reader["sDescripcion"].ToString();
                            Material.Unidad.Nombre = Reader["sNombreUMD"].ToString();
                            Material.Marca.Nombre = Reader["Marca"].ToString();
                            Material.Modelo.Nombre = (Reader["sModelo"].ToString());
                            Material.PrecioCompra = float.Parse(Reader["fPrecioCompra"].ToString());
                            Material.PrecioVenta = float.Parse(Reader["fPrecioVenta"].ToString());
                            Material.Categoria.Nombre = Reader["Categoria"].ToString();
                            Material.SubCateoria.Nombre = Reader["Subcategoria"].ToString();
                            Material.Descuento = double.Parse(Reader["PorcenDscto"].ToString());
                            Material.TotalPagina = int.Parse(Reader["Total"].ToString());
                            oDatos.Add(Material);
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

        public List<EMaterial> ListadoGiftCard(int Empresa)
        {
            List<EMaterial> oDatos = new List<EMaterial>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("LOG_ListaGiftCard");
                    CreateHelper(Connection);
                    AddInParameter("@IdEmpresa", Empresa);
                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            EMaterial Material = new EMaterial();
                            Material.IdMaterial = int.Parse(Reader["iIdMat"].ToString());
                            Material.sCodigo = (Reader["Codigo"].ToString());
                            Material.Text = (Reader["Tipo"].ToString());
                            Material.Codigo = Reader["sCodMaterial"].ToString();
                            Material.Nombre = Reader["sNomMaterial"].ToString();
                            Material.Descripcion = Reader["sDescripcion"].ToString();
                            Material.Unidad.Nombre = Reader["sNombreUMD"].ToString();
                            Material.Marca.Nombre = Reader["Marca"].ToString();
                            Material.Modelo.Nombre = (Reader["sModelo"].ToString());
                            Material.PrecioCompra = float.Parse(Reader["fPrecioCompra"].ToString());
                            Material.PrecioVenta = float.Parse(Reader["fPrecioVenta"].ToString());
                            Material.Categoria.Nombre = Reader["Categoria"].ToString();
                            Material.SubCateoria.Nombre = Reader["Subcategoria"].ToString();
                            Material.Descuento = double.Parse(Reader["PorcenDscto"].ToString());
                            Material.TotalPagina = int.Parse(Reader["Total"].ToString());
                            oDatos.Add(Material);
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
        public List<EMaterial> ListadodetGiftCard(int Empresa, int idMaterial)
        {
            List<EMaterial> oDatos = new List<EMaterial>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("LOG_ListaDetGiftCard");
                    CreateHelper(Connection);
                    AddInParameter("@IdEmpresa", Empresa);
                    AddInParameter("@IdProducto", idMaterial);
                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            EMaterial Material = new EMaterial();
                            Material.IdMaterial = int.Parse(Reader["iIdMat"].ToString());
                            Material.sCodigo = (Reader["Codigo"].ToString());
                            Material.Codigo = Reader["sCodMaterial"].ToString();
                            Material.Nombre = Reader["sNomMaterial"].ToString();
                            Material.EstadoGift = Reader["EstadoGift"].ToString();
                            Material.PrecioVenta = float.Parse(Reader["fPrecioVenta"].ToString());
                            Material.TotalPagina = int.Parse(Reader["Total"].ToString());
                            oDatos.Add(Material);
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
        #endregion

        #region Unidada

        //unidad
        public string InstUnidad(EUnidad Unidad, string Usuario)
        {
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("LOG_InsUnidadMedida");
                    CreateHelper(Connection);
                    AddInParameter("@iIdUnidad", Unidad.IdUnidad);
                    AddInParameter("@IdEmpresa", Unidad.Empresa.Id);
                    AddInParameter("@sCodigoSunat", Unidad.CodigSunat);
                    AddInParameter("@sNombreUMD", Unidad.Nombre);
                    AddInParameter("@sSiglaUMD", Unidad.Sigla);
                    AddInParameter("@sUserReg", Usuario);
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


        public EUnidad ListaEditUnidad(int IdUnidad, int Empresa)
        {
            EUnidad obj = null;
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("LOG_ObtenerUnidadMedida");
                    CreateHelper(Connection);
                    AddInParameter("@IdUndiad", IdUnidad);
                    AddInParameter("@IdEmpresa", Empresa);
                    using (var Reader = ExecuteReader())
                    {
                        if (Reader.Read())
                        {
                            obj = new EUnidad();
                            obj.IdUnidad = int.Parse(Reader["iIdUnidad"].ToString());
                            obj.CodigSunat = (Reader["sCodigoSunat"].ToString());
                            obj.Nombre = Reader["sNombreUMD"].ToString();
                            obj.Sigla = Reader["sSiglaUMD"].ToString();
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
        public List<EUnidad> ListaUnidad(int Empresa)
        {
            List<EUnidad> oDatos = new List<EUnidad>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("LOG_ListaUnidad");
                    CreateHelper(Connection);
                    AddInParameter("@IdEmpresa", Empresa);
                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            EUnidad Material = new EUnidad();
                            Material.IdUnidad = int.Parse(Reader["iIdUnidad"].ToString());
                            Material.CodigSunat = (Reader["sCodigoSunat"].ToString());
                            Material.Nombre = Reader["sNombreUMD"].ToString();
                            Material.Sigla = Reader["sSiglaUMD"].ToString();

                            oDatos.Add(Material);
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
        #endregion 
        #region Marca

        //marca 
        public string InstMarca(EMarca Marca, string Usuario)
        {
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("LOG_InsMarca");
                    CreateHelper(Connection);
                    AddInParameter("@iIdMar", Marca.IdMarca);
                    AddInParameter("@IdEmpresa", Marca.Empresa.Id);
                    AddInParameter("@sDescripcion", Marca.Nombre);
                    AddInParameter("@sCodigo", Marca.Codigo);
                    AddInParameter("@sCodReg", Usuario);
                    AddInParameter("@IdLinea", Marca.IdLinea);
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


        public EMarca ListaEditMarca(int IdMarca, int Empresa)
        {
            EMarca obj = null;
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("LOG_ObtenerMarca");
                    CreateHelper(Connection);
                    AddInParameter("@IdMarca", IdMarca);
                    AddInParameter("@IdEmpresa", Empresa);
                    using (var Reader = ExecuteReader())
                    {
                        if (Reader.Read())
                        {
                            obj = new EMarca();
                            obj.IdMarca = int.Parse(Reader["iIdMar"].ToString());
                            obj.Nombre = (Reader["sDescripcion"].ToString());
                            obj.Codigo = (Reader["sCodigo"].ToString());
                            obj.IdLinea = int.Parse(Reader["IdLinea"].ToString());
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
        public List<EMarca> ListaMarca(int Empresa)
        {
            List<EMarca> oDatos = new List<EMarca>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("LOG_ListaMarca");
                    CreateHelper(Connection);
                    AddInParameter("@IdEmpresa", Empresa);
                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            EMarca Material = new EMarca();
                            Material.IdMarca = int.Parse(Reader["iIdMar"].ToString());
                            Material.Nombre = (Reader["sDescripcion"].ToString());
                            Material.Codigo = (Reader["sCodigo"].ToString());
                            oDatos.Add(Material);
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
        #endregion

        #region Categoria 
        //Categoria
        public string InstCategoria(ECategoria Categoria, string Usuario)
        {
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("LOG_InsCategoria");
                    CreateHelper(Connection);
                    AddInParameter("@iIdCat", Categoria.IdCateogira);
                    AddInParameter("@IdEmpresa", Categoria.Empresa.Id);
                    AddInParameter("@sDescripcion", Categoria.Nombre);
                    AddInParameter("@sSiglaFamilia", Categoria.Sigla);
                    AddInParameter("@sCodReg", Usuario);
                    AddInParameter("@IdLinea", Categoria.IdLinea);
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


        public ECategoria ListaEditCategoria(int IdCategoria, int Empresa)
        {
            ECategoria obj = null;
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("LOG_ObtenerCategoria");
                    CreateHelper(Connection);
                    AddInParameter("@IdCategoria", IdCategoria);
                    AddInParameter("@IdEmpresa", Empresa);
                    using (var Reader = ExecuteReader())
                    {
                        if (Reader.Read())
                        {
                            obj = new ECategoria();
                            obj.IdCateogira = int.Parse(Reader["iIdCat"].ToString());
                            obj.Nombre = (Reader["sDescripcion"].ToString());
                            obj.Sigla = (Reader["sSiglaFamilia"].ToString());
                            obj.IdLinea = int.Parse(Reader["IdLinea"].ToString());

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
        public List<ECategoria> ListaCategoria(int Empresa)
        {
            List<ECategoria> oDatos = new List<ECategoria>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("LOG_ListaCategoria");
                    CreateHelper(Connection);
                    AddInParameter("@IdEmpresa", Empresa);
                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            ECategoria categoria = new ECategoria();
                            categoria.IdCateogira = int.Parse(Reader["iIdCat"].ToString());
                            categoria.Nombre = (Reader["sDescripcion"].ToString());
                            categoria.Sigla = (Reader["sSiglaFamilia"].ToString());
                            oDatos.Add(categoria);
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
        #endregion

        #region suCategoria 

        //SubCategoria
        public string InstSubCategoria(ESubCateoria SubCategoria, string Usuario)
        {
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("LOG_InsSubCategoria");
                    CreateHelper(Connection);
                    AddInParameter("@iIdSubCat", SubCategoria.IdSubCategoira);
                    AddInParameter("@IdEmpresa", SubCategoria.Empresa.Id);
                    AddInParameter("@sDescripcion", SubCategoria.Nombre);
                    AddInParameter("@sSiglaSubFamilia", SubCategoria.Sigla);
                    //AddInParameter("@iIdCat", SubCategoria.Categoria.IdCateogira);
                    AddInParameter("@sCodReg", Usuario);
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


        public ESubCateoria ListaEditSubCategoria(int IdSubCategoria, int Empresa)
        {
            ESubCateoria obj = null;
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("LOG_ObtenerSubCategoria");
                    CreateHelper(Connection);
                    AddInParameter("@IdSubCategoria", IdSubCategoria);
                    AddInParameter("@IdEmpresa", Empresa);
                    using (var Reader = ExecuteReader())
                    {
                        if (Reader.Read())
                        {
                            obj = new ESubCateoria();
                            obj.IdSubCategoira = int.Parse(Reader["iIdSubCat"].ToString());
                            obj.Nombre = (Reader["sDescripcion"].ToString());
                            obj.Sigla = (Reader["sSiglaSubFamilia"].ToString());
                            obj.Categoria.IdCateogira = int.Parse(Reader["iIdCat"].ToString());
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
        public List<ESubCateoria> ListaSubCategoria(int Empresa)
        {
            List<ESubCateoria> oDatos = new List<ESubCateoria>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("LOG_ListaSubCategoria");
                    CreateHelper(Connection);
                    AddInParameter("@IdEmpresa", Empresa);
                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            ESubCateoria categoria = new ESubCateoria();
                            categoria.IdSubCategoira = int.Parse(Reader["iIdSubCat"].ToString());
                            categoria.Nombre = (Reader["sDescripcion"].ToString());
                            categoria.Sigla = (Reader["sSiglaSubFamilia"].ToString());
                            //categoria.Categoria.Nombre = (Reader["Categoria"].ToString());
                            oDatos.Add(categoria);
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
        #endregion

        //cliente
        #region clientes



        public string InstCliente(ECliente Cliente, string Usuario)
        {
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("LOG_InsCliente");
                    CreateHelper(Connection);
                    AddInParameter("@iIdCliente", Cliente.IdCliente);
                    AddInParameter("@iIdTipoDoc", Cliente.Id);
                    AddInParameter("@IdEmpresa", Cliente.Empresa.Id);

                    AddInParameter("@sNroDoc", Cliente.NroDocumento);
                    AddInParameter("@sRazonSocial", Cliente.Razonsocial);
                    AddInParameter("@sTelefono", Cliente.Telefono, AllowNull);
                    AddInParameter("@sCelular", Cliente.Celular, AllowNull);
                    AddInParameter("@sEmail", Cliente.Email, AllowNull);
                    AddInParameter("@sIdDep", Cliente.Ubigeo.CodigoDepartamento == "0" ? "15" : Cliente.Ubigeo.CodigoDepartamento);
                    AddInParameter("@sIdProv", Cliente.Ubigeo.CodigoProvincia == "0" ? "01" : Cliente.Ubigeo.CodigoProvincia);
                    AddInParameter("@sIdDistrito", Cliente.Ubigeo.CodigoDistrito == "0" ? "01" : Cliente.Ubigeo.CodigoDistrito);
                    AddInParameter("@sDireccion", Cliente.Direccion);
                    AddInParameter("@sCodReg", Usuario);
                    AddInParameter("@IdVendedor", Cliente.IdVendedor);
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


        public ECliente ListaEditCliente(int IdCliente, int Empresa)
        {
            ECliente obj = null;
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("LOG_ObtenerCliente");
                    CreateHelper(Connection);
                    AddInParameter("@IdCliente", IdCliente);
                    AddInParameter("@IdEmpresa", Empresa);
                    using (var Reader = ExecuteReader())
                    {
                        if (Reader.Read())
                        {
                            obj = new ECliente();
                            obj.IdCliente = int.Parse(Reader["iIdCliente"].ToString());
                            obj.Id = int.Parse(Reader["iIdTipoDoc"].ToString());
                            obj.NroDocumento = (Reader["sNroDoc"].ToString());
                            obj.Razonsocial = (Reader["sRazonSocial"].ToString());
                            obj.Telefono = (Reader["sTelefono"].ToString());
                            obj.Celular = (Reader["sCelular"].ToString());
                            obj.Email = (Reader["sEmail"].ToString());
                            obj.IdVendedor = int.Parse(Reader["iIdVendedor"].ToString());
                            obj.Ubigeo.CodigoDepartamento = (Reader["sIdDep"].ToString());
                            obj.Ubigeo.CodigoProvincia = (Reader["sIdProv"].ToString());
                            obj.Ubigeo.CodigoDistrito = (Reader["sIdDistrito"].ToString());
                            obj.Direccion = (Reader["sDireccion"].ToString());
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
        public List<ECliente> ListaCliente(int Empresa)
        {
            List<ECliente> oDatos = new List<ECliente>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("LOG_ListaCliente");
                    CreateHelper(Connection);
                    AddInParameter("@IdEmpresa", Empresa);
                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            ECliente obj = new ECliente();
                            obj.IdCliente = int.Parse(Reader["iIdCliente"].ToString());
                            obj.Text = (Reader["Documento"].ToString());
                            obj.NroDocumento = (Reader["sNroDoc"].ToString());
                            obj.Razonsocial = (Reader["sRazonSocial"].ToString());
                            obj.Telefono = (Reader["sTelefono"].ToString());
                            obj.Celular = (Reader["sCelular"].ToString());
                            obj.Email = (Reader["sEmail"].ToString());
                            obj.Direccion = (Reader["sDireccion"].ToString());
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
        #endregion

        #region Proveedor 
        // proveedor

        public string InstProveedor(EProveedor proveedor, string Usuario)
        {
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("LOG_InsProveedor");
                    CreateHelper(Connection);
                    AddInParameter("@iIdProveedor", proveedor.IdProveedor);
                    AddInParameter("@iIdTipoDoc", proveedor.Id);
                    AddInParameter("@IdEmpresa", proveedor.Empresa.Id);
                    AddInParameter("@sNroDoc", proveedor.NroDocumento);
                    AddInParameter("@sRazonSocial", proveedor.Razonsocial);
                    AddInParameter("@sTelefono", proveedor.Telefono, AllowNull);
                    AddInParameter("@sCelular", proveedor.Celular, AllowNull);
                    AddInParameter("@sEmail", proveedor.Email, AllowNull);
                    AddInParameter("@sIdDep", proveedor.Ubigeo.CodigoDepartamento);
                    AddInParameter("@sIdProv", proveedor.Ubigeo.CodigoProvincia);
                    AddInParameter("@sIdDistrito", proveedor.Ubigeo.CodigoDistrito);
                    AddInParameter("@sDireccion", proveedor.Direccion);
                    AddInParameter("@sCodReg", Usuario);
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


        public EProveedor ListaEditProveedor(int IdPorveedor, int Empresa)
        {
            EProveedor obj = null;
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("LOG_ObtenerProveedor");
                    CreateHelper(Connection);
                    AddInParameter("@IdProveedor", IdPorveedor);
                    AddInParameter("@IdEmpresa", Empresa);
                    using (var Reader = ExecuteReader())
                    {
                        if (Reader.Read())
                        {
                            obj = new EProveedor();
                            obj.IdProveedor = int.Parse(Reader["iIdProveedor"].ToString());
                            obj.Id = int.Parse(Reader["iIdTipoDoc"].ToString());
                            obj.NroDocumento = (Reader["sNroDoc"].ToString());
                            obj.Razonsocial = (Reader["sRazonSocial"].ToString());
                            obj.Telefono = (Reader["sTelefono"].ToString());
                            obj.Celular = (Reader["sCelular"].ToString());
                            obj.Email = (Reader["sEmail"].ToString());
                            obj.Ubigeo.CodigoDepartamento = (Reader["sIdDep"].ToString());
                            obj.Ubigeo.CodigoProvincia = (Reader["sIdProv"].ToString());
                            obj.Ubigeo.CodigoDistrito = (Reader["sIdDistrito"].ToString());
                            obj.Direccion = (Reader["sDireccion"].ToString());
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
        public List<EProveedor> ListaProveedor(int Empresa)
        {
            List<EProveedor> oDatos = new List<EProveedor>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("LOG_ListaProveedor");
                    CreateHelper(Connection);
                    AddInParameter("@IdEmpresa", Empresa);
                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            EProveedor categoria = new EProveedor();
                            categoria.IdProveedor = int.Parse(Reader["iIdProveedor"].ToString());
                            categoria.Text = (Reader["Documento"].ToString());
                            categoria.NroDocumento = (Reader["sNroDoc"].ToString());
                            categoria.Razonsocial = (Reader["sRazonSocial"].ToString());
                            categoria.Telefono = (Reader["sTelefono"].ToString());
                            categoria.Celular = (Reader["sCelular"].ToString());
                            categoria.Email = (Reader["sEmail"].ToString());
                            categoria.Direccion = (Reader["sDireccion"].ToString());
                            oDatos.Add(categoria);
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
        #endregion

        //Sucursal

        public string InstSucursal(ESucursal oDatos, string Usuario)
        {
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("LOG_InsSucursal");
                    CreateHelper(Connection);
                    AddInParameter("@iIdSucursal", oDatos.IdSucursal);
                    AddInParameter("@iIdEmpresa", oDatos.Empresa.Id);
                    AddInParameter("@sNombreSucursal", oDatos.Nombre);
                    AddInParameter("@sTelefono", oDatos.Telefono, AllowNull);
                    AddInParameter("@NroAutorizacionSUNAT", oDatos.AutorizacionSUNAT, AllowNull);
                    AddInParameter("@sDireccion", oDatos.Direcciones);
                    AddInParameter("@sReferencia", oDatos.Referencia, AllowNull);
                    AddInParameter("@sIdDep", oDatos.Ubigeo.CodigoDepartamento);
                    AddInParameter("@sIdProv", oDatos.Ubigeo.CodigoProvincia);
                    AddInParameter("@sIdDistrito", oDatos.Ubigeo.CodigoDistrito);
                    AddInParameter("@sCodReg", Usuario);
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


        public ESucursal ListaEditSucursal(int Id, int Empresa)
        {
            ESucursal obj = null;
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("LOG_ObtenerSucursal");
                    CreateHelper(Connection);
                    AddInParameter("@IdSucursal", Id);
                    AddInParameter("@IdEmpresa", Empresa);
                    using (var Reader = ExecuteReader())
                    {
                        if (Reader.Read())
                        {
                            obj = new ESucursal();
                            obj.IdSucursal = int.Parse(Reader["iIdSucursal"].ToString());
                            obj.Nombre = (Reader["sNombreSucursal"].ToString());
                            obj.Telefono = (Reader["sTelefono"].ToString());
                            obj.Direcciones = (Reader["sDireccion"].ToString());
                            obj.Referencia = (Reader["sReferencia"].ToString());
                            obj.AutorizacionSUNAT = (Reader["NroAutorizacionSUNAT"].ToString());
                            obj.Ubigeo.CodigoDepartamento = (Reader["sIdDep"].ToString());
                            obj.Ubigeo.CodigoProvincia = (Reader["sIdProv"].ToString());
                            obj.Ubigeo.CodigoDistrito = (Reader["sIdDistrito"].ToString());

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
        public List<ESucursal> ListaSucursal(int Empresa)
        {
            List<ESucursal> oDatos = new List<ESucursal>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("LOG_ListaSucursal");
                    CreateHelper(Connection);
                    AddInParameter("@IdEmpresa", Empresa);
                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            ESucursal obj = new ESucursal();
                            obj.IdSucursal = int.Parse(Reader["iIdSucursal"].ToString());
                            obj.Nombre = (Reader["sNombreSucursal"].ToString());
                            obj.Telefono = (Reader["sTelefono"].ToString());
                            obj.Referencia = (Reader["sReferencia"].ToString());
                            obj.Direcciones = (Reader["sDireccion"].ToString());
                            obj.Ubigeo.CodigoDepartamento = (Reader["Departamento"].ToString());
                            obj.Ubigeo.CodigoProvincia = (Reader["Provincia"].ToString());
                            obj.Ubigeo.CodigoDistrito = (Reader["Distrito"].ToString());
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



        //Almacen
        public string InstAlmacen(EAlmacen oDatos, string Usuario)
        {
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("LOG_InsAlmacen");
                    CreateHelper(Connection);
                    AddInParameter("@iIdAlm", oDatos.IdAlmacen);
                    AddInParameter("@IdEmpresa", oDatos.Empresa.Id);
                    AddInParameter("@IdSucursal", oDatos.Sucursal.IdSucursal);
                    AddInParameter("@sAlmacen", oDatos.Nombre);
                    AddInParameter("@sDireccion", oDatos.Direccion);
                    AddInParameter("@sTelefono", oDatos.Telefono, AllowNull);
                    AddInParameter("@sIdDep", oDatos.Ubigeo.CodigoDepartamento);
                    AddInParameter("@sIdProv", oDatos.Ubigeo.CodigoProvincia);
                    AddInParameter("@sIdDistrito", oDatos.Ubigeo.CodigoDistrito);
                    AddInParameter("@sCodReg", Usuario);
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


        public EAlmacen ListaEditAlmacen(int Id, int Empresa)
        {
            EAlmacen obj = null;
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("LOG_ObtenerAlmacen");
                    CreateHelper(Connection);
                    AddInParameter("@IdAlmacen", Id);
                    AddInParameter("@IdEmpresa", Empresa);
                    using (var Reader = ExecuteReader())
                    {
                        if (Reader.Read())
                        {
                            obj = new EAlmacen();
                            obj.IdAlmacen = int.Parse(Reader["iIdAlm"].ToString());
                            obj.Nombre = (Reader["sAlmacen"].ToString());
                            obj.Sucursal.IdSucursal = int.Parse(Reader["IdSucursal"].ToString());
                            obj.Direccion = (Reader["sDireccion"].ToString());
                            obj.Telefono = (Reader["sTelefono"].ToString());
                            obj.Ubigeo.CodigoDepartamento = (Reader["sIdDep"].ToString());
                            obj.Ubigeo.CodigoProvincia = (Reader["sIdProv"].ToString());
                            obj.Ubigeo.CodigoDistrito = (Reader["sIdDistrito"].ToString());

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
        public List<EAlmacen> ListaAlmacen(int Empresa)
        {
            List<EAlmacen> oDatos = new List<EAlmacen>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("LOG_ListaAlmacen");
                    CreateHelper(Connection);
                    AddInParameter("@IdEmpresa", Empresa);
                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            EAlmacen obj = new EAlmacen();
                            obj.IdAlmacen = int.Parse(Reader["iIdAlm"].ToString());
                            obj.Nombre = (Reader["sAlmacen"].ToString());
                            obj.Direccion = (Reader["sDireccion"].ToString());
                            obj.Telefono = (Reader["sTelefono"].ToString());
                            obj.Sucursal.Nombre = (Reader["sNombreSucursal"].ToString());
                            obj.Ubigeo.CodigoDepartamento = (Reader["Departamento"].ToString());
                            obj.Ubigeo.CodigoProvincia = (Reader["Provincia"].ToString());
                            obj.Ubigeo.CodigoDistrito = (Reader["Distrito"].ToString());
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

        public List<EAlmacen> ListaPermisoAlmacen(int Empresa)
        {
            List<EAlmacen> oDatos = new List<EAlmacen>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("LOG_List_Permisos_Usuario_Almacen");
                    CreateHelper(Connection);
                    AddInParameter("@IdEmpresa", Empresa);
                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            EAlmacen obj = new EAlmacen();
                            obj.Usuario.Nombres = (Reader["usuario"].ToString());
                            obj.Usuario.NroDocumento = (Reader["sNumDocumentoI"].ToString());
                            obj.Usuario.Usuario = (Reader["sUsuario"].ToString());
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


        public List<EAlmacen> ListaCBO_UsuarioAlmacen(int Flag, int Empresa)
        {
            List<EAlmacen> oDatos = new List<EAlmacen>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("LOG_Cbo_Lista_Usuarios");
                    CreateHelper(Connection);
                    AddInParameter("@flag", Flag);
                    AddInParameter("@IdEmpresa", Empresa);
                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            EAlmacen obj = new EAlmacen();
                            obj.Usuario.Usuario = (Reader["sUsuario"].ToString());//login
                            obj.Usuario.Nombre = (Reader["Nombre"].ToString());// 
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



        public List<EAlmacen> ListaAlmacenPermisos(int Flag, string login, int Empresa, int sucursal)
        {
            List<EAlmacen> oDatos = new List<EAlmacen>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("LOG_List_Almacen_Permisos");
                    CreateHelper(Connection);
                    AddInParameter("@flag", Flag);
                    AddInParameter("@sLogin", login);
                    AddInParameter("@IdEmpresa", Empresa);
                    AddInParameter("@IdSucural", sucursal);
                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            EAlmacen obj = new EAlmacen();
                            obj.IdAlmacen = int.Parse(Reader["iIdAlm"].ToString());
                            obj.Nombre = (Reader["sAlmacen"].ToString());
                            obj.Ubigeo.UbicacionGeografica = (Reader["Ubicacion"].ToString());
                            obj.Estado = (Reader["Estado"].ToString());
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

        public string InstUpdPermisoAlmacem(List<EAlmacen> Almacen, int Flag, string Usuario)
        {
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();

                    foreach (EAlmacen item in Almacen)
                    {

                        SetQuery("LOG_InsUpd_Permisos_Usuario_Almacen");
                        CreateHelper(Connection);
                        AddInParameter("@sLogin", item.Usuario.Usuario);
                        AddInParameter("@IdEmpresa", item.Empresa.Id);
                        AddInParameter("@Idalmacen", item.IdAlmacen);
                        AddInParameter("@sEstado", item.Estado);
                        AddInParameter("@Flag", Flag);
                        AddInParameter("@UserReg", Usuario);
                        AddOutParameter("@Mensaje", (DbType)SqlDbType.VarChar);
                        ExecuteQuery();
                        var smensaje = GetOutput("@Mensaje").ToString();

                    }
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
        //Empresa
        public string InstEmpresa(EEmpresa oDatos, string Usuario)
        {
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("GEN_InstEmpresa");
                    CreateHelper(Connection);
                    AddInParameter("@ID_Empresa", oDatos.Id);
                    AddInParameter("@sRUC", oDatos.RUC);
                    AddInParameter("@sRazonSocialE", oDatos.RazonSocial);
                    AddInParameter("@sUbigeo", oDatos.Ubigeo);
                    AddInParameter("@sDirecion", oDatos.Direccion, AllowNull);
                    AddInParameter("@sCorreoEmp", oDatos.Correo);
                    AddInParameter("@sContrasenia", oDatos.Contrasenia);
                    AddInParameter("@sTelefono", oDatos.Telefono, AllowNull);
                    AddInParameter("@sCelular", oDatos.Celular, AllowNull);
                    AddInParameter("@sUserReg", Usuario);
                    AddInParameter("@sImagen", oDatos.Logo, AllowNull);
                    AddInParameter("@sIdDep", oDatos.EUbigeo.CodigoDepartamento);
                    AddInParameter("@sIdProv", oDatos.EUbigeo.CodigoProvincia);
                    AddInParameter("@sIdDistrito", oDatos.EUbigeo.CodigoDistrito);
                    AddInParameter("@sPaginaWeb", oDatos.PaginaWeb, AllowNull);
                    AddInParameter("@sUsuarioSol", oDatos.UsuarioSol);
                    AddInParameter("@ClaveSol", oDatos.ClaveSol);
                    AddInParameter("@sCertificado", oDatos.Certificado, AllowNull);
                    AddInParameter("@sPaswordCertificado", oDatos.ClaveCertificado);
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
        public EEmpresa ListaEditEmpresa(int Empresa)
        {
            EEmpresa obj = null;
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("GEN_ObtenerEmpresa");
                    CreateHelper(Connection);
                    AddInParameter("@IdEmpresa", Empresa);
                    using (var Reader = ExecuteReader())
                    {
                        if (Reader.Read())
                        {
                            obj = new EEmpresa();
                            obj.Id = int.Parse(Reader["iID_Empresa"].ToString());
                            obj.RUC = (Reader["sRUC"].ToString());
                            obj.Nombre = (Reader["sRazonSocialE"].ToString());
                            obj.Correo = (Reader["sCorreoEmp"].ToString());
                            obj.Contrasenia = (Reader["sContrasenia"].ToString());
                            obj.Direccion = (Reader["sDirecion"].ToString());
                            obj.Telefono = (Reader["sTelefono"].ToString());
                            obj.Celular = (Reader["sCelular"].ToString());
                            obj.Logo = (Reader["sImagen"].ToString());
                            obj.EUbigeo.CodigoDepartamento = (Reader["sIdDep"].ToString());
                            obj.EUbigeo.CodigoProvincia = (Reader["sIdProv"].ToString());
                            obj.EUbigeo.CodigoDistrito = (Reader["sIdDistrito"].ToString());
                            obj.PaginaWeb = (Reader["sPaginaWeb"].ToString());
                            obj.UsuarioSol = (Reader["sUsuarioSol"].ToString());
                            obj.ClaveSol = (Reader["ClaveSol"].ToString());
                            obj.Certificado = (Reader["sCertificado"].ToString());
                            obj.EndPointUrl = (Reader["sunat"].ToString());
                            obj.ClaveCertificado = (Reader["sPaswordCertificado"].ToString());

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
        public List<EEmpresa> ListaEmpresa()
        {
            List<EEmpresa> oDatos = new List<EEmpresa>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("GEN_ListaEmpresa");
                    CreateHelper(Connection);

                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            EEmpresa obj = new EEmpresa();
                            obj.Id = int.Parse(Reader["iID_Empresa"].ToString());
                            obj.RUC = (Reader["sRUC"].ToString());
                            obj.RazonSocial = (Reader["sRazonSocialE"].ToString());
                            obj.Direccion = (Reader["sDirecion"].ToString());
                            obj.Correo = (Reader["sCorreoEmp"].ToString());
                            obj.Telefono = (Reader["sTelefono"].ToString());
                            obj.Celular = (Reader["sCelular"].ToString());
                            obj.Logo = (Reader["sImagen"].ToString());
                            obj.EUbigeo.CodigoDepartamento = (Reader["Departamento"].ToString());
                            obj.EUbigeo.CodigoProvincia = (Reader["Provincia"].ToString());
                            obj.EUbigeo.CodigoDistrito = (Reader["Distrito"].ToString());
                            obj.PaginaWeb = (Reader["sPaginaWeb"].ToString());

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
        public EPerfil ListaEnviarCorreo(int Empresa)
        {
            EPerfil obj = null;
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("GEN_ListaEnvioCorreo");
                    CreateHelper(Connection);
                    AddInParameter("@IdEmpresa", Empresa);
                    using (var Reader = ExecuteReader())
                    {
                        if (Reader.Read())
                        {
                            obj = new EPerfil();
                            obj.Empresa.Id = int.Parse(Reader["iID_Empresa"].ToString());
                            obj.Usuario = (Reader["sUsuario"].ToString());
                            obj.UsuarioCreador.Password = (Reader["sPassword"].ToString());
                            obj.Nombre = (Reader["sNomPerUsuario"].ToString());
                            obj.UsuarioCreador.Nombre = (Reader["sNombres"].ToString());
                            obj.Empresa.RUC = (Reader["sRUC"].ToString());
                            obj.Empresa.Nombre = (Reader["sRazonSocialE"].ToString());
                            obj.Empresa.Correo = (Reader["sCorreoEmp"].ToString());
                            obj.Empresa.Direccion = (Reader["sDirecion"].ToString());
                            obj.Empresa.Telefono = (Reader["sTelefono"].ToString());
                            obj.Empresa.Celular = (Reader["sCelular"].ToString());

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

        public List<EGeneral> ListaCaja(int empresa, int sucursal)
        {
            List<EGeneral> oDatos = new List<EGeneral>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("FAC_ListaCaja");
                    CreateHelper(Connection);
                    AddInParameter("@idEmpresa", empresa);
                    AddInParameter("@idSucursal", sucursal);
                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            EGeneral Menu = new EGeneral();
                            Menu.Id = int.Parse(Reader["idcaja"].ToString());
                            Menu.Text = Reader["sNombre"].ToString();
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
        public EGeneral ListaEditCaja(int ID)
        {
            EGeneral obj = null;
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("FAC_ListaCajaId");
                    CreateHelper(Connection);
                    AddInParameter("@id", ID);
                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            obj = new EGeneral();
                            obj.Id = int.Parse(Reader["idcaja"].ToString());
                            obj.Text = (Reader["sNombre"].ToString());

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

        public string Insertar_UpdateCaja(EGeneral Menu, int empresa, int sucursal, string Usuario)
        {
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("FAC_InsUp");
                    CreateHelper(Connection);
                    AddInParameter("@id", Menu.Id);
                    AddInParameter("@snombre", Menu.Text);
                    AddInParameter("@idEmpresa", empresa);
                    AddInParameter("@idsucursal", sucursal);
                    AddInParameter("@sCodReg", Usuario);
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

        #region Tipo Tipo 
        public List<EGeneral> ListaTipo(int empresa)
        {
            List<EGeneral> oDatos = new List<EGeneral>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("LOG_ListaTipo");
                    CreateHelper(Connection);
                    AddInParameter("@IdEmpresa", empresa);
                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            EGeneral Menu = new EGeneral();
                            Menu.Id = int.Parse(Reader["iId_Tipo"].ToString());
                            Menu.Nombre = Reader["Linea"].ToString();
                            Menu.Text = Reader["sDescripcion"].ToString();
                            Menu.sCodigo = Reader["sCodigo"].ToString();
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

        public List<EBancos> ListarBancos(int empresa)
        {
            List<EBancos> oDatos = new List<EBancos>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("LOG_ListarBancos");
                    CreateHelper(Connection);
                    AddInParameter("@IdEmpresa", empresa);
                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            EBancos Banco = new EBancos();
                            Banco.Id = int.Parse(Reader["Id"].ToString());
                            Banco.nombreBanco = Reader["nombreBanco"].ToString();
                            Banco.numeroCuenta = Reader["numeroCuenta"].ToString();
                            Banco.Moneda.Nombre = Reader["moneda"].ToString();
                            oDatos.Add(Banco);
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

        public List<ETipoEnvio> ListarTipoEnvio(int empresa)
        {
            List<ETipoEnvio> oDatos = new List<ETipoEnvio>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("LOG_ListarTipoEnvio");
                    CreateHelper(Connection);
                    AddInParameter("@IdEmpresa", empresa);
                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            ETipoEnvio Data = new ETipoEnvio();
                            Data.Id = int.Parse(Reader["Id"].ToString());
                            Data.Nombre = Reader["Nombre"].ToString();
                            Data.Costo = float.Parse(Reader["Costo"].ToString()); 
                            oDatos.Add(Data);
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


        public EGeneral ListaEditTipo(int ID, int empresa)
        {
            EGeneral obj = null;
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("LOG_ObtenerTipo");
                    CreateHelper(Connection);
                    AddInParameter("@IdTipo", ID);
                    AddInParameter("@IdEmpresa", empresa);
                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            obj = new EGeneral();
                            obj.Id = int.Parse(Reader["iId_Tipo"].ToString());
                            obj.Linea = int.Parse(Reader["IdLinea"].ToString());
                            obj.Text = (Reader["sDescripcion"].ToString());
                            obj.sCodigo = (Reader["sCodigo"].ToString());
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

        public EBancos BuscarBancoId(int ID, int empresa)
        {
            EBancos obj = null;
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("LOG_ObtenerBanco");
                    CreateHelper(Connection);
                    AddInParameter("@Id", ID);
                    AddInParameter("@IdEmpresa", empresa);
                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            obj = new EBancos();
                            obj.Id = int.Parse(Reader["Id"].ToString());
                            obj.nombreBanco = (Reader["nombreBanco"].ToString());
                            obj.Moneda.IdMoneda = int.Parse(Reader["idMoneda"].ToString());
                            obj.numeroCuenta = (Reader["numeroCuenta"].ToString());
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

        public ETipoEnvio BuscarTipoEnvioId(int ID, int empresa)
        {
            ETipoEnvio obj = null;
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("LOG_ObtenerTipoEnvio");
                    CreateHelper(Connection);
                    AddInParameter("@Id", ID);
                    AddInParameter("@IdEmpresa", empresa);
                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            obj = new ETipoEnvio();
                            obj.Id = int.Parse(Reader["Id"].ToString());
                            obj.Nombre = (Reader["Nombre"].ToString());
                            obj.Costo = float.Parse(Reader["Costo"].ToString());
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



        public string Insertar_UpdateTipo(EGeneral Menu, int empresa, string Usuario)
        {
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("LOG_InsTipo");
                    CreateHelper(Connection);
                    AddInParameter("@iIdTipo", Menu.Id);
                    AddInParameter("@IdLinea", Menu.Linea);
                    AddInParameter("@IdEmpresa", empresa);
                    AddInParameter("@sDescripcion", Menu.Text);
                    AddInParameter("@sCodigo", Menu.sCodigo);
                    AddInParameter("@sCodReg", Usuario);
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

        public string RegistrarEditarBanco(EBancos banco, int empresa, string Usuario)
        {
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("LOG_InsBanco");
                    CreateHelper(Connection);
                    AddInParameter("@Id", banco.Id);
                    AddInParameter("@nombreBanco", banco.nombreBanco);
                    AddInParameter("@idMoneda", banco.Moneda.IdMoneda);
                    AddInParameter("@numeroCuenta", banco.numeroCuenta);
                    AddInParameter("@usuario", Usuario);
                    AddInParameter("@IdEmpresa", empresa);
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

        public string RegistrarEditarTipoEnvio(ETipoEnvio Obj, int empresa, string Usuario)
        {
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("LOG_InsTipoEnvio");
                    CreateHelper(Connection);
                    AddInParameter("@Id", Obj.Id);
                    AddInParameter("@Nombre", Obj.Nombre);
                    AddInParameter("@Costo", Obj.Costo);
                    AddInParameter("@usuario", Usuario);
                    AddInParameter("@IdEmpresa", empresa);
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

        #endregion

        #region tipo de serio 
        /*tipo de serie*/
        public List<ETipoSerie> ListaTipoSerie(int empresa, int sucursal)
        {
            List<ETipoSerie> oDatos = new List<ETipoSerie>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("GEN_ListaTipoDocumento");
                    CreateHelper(Connection);
                    AddInParameter("@IdEmpresa", empresa);
                    AddInParameter("@IdSucursal", sucursal);
                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            ETipoSerie obj = new ETipoSerie();
                            obj.Id = int.Parse(Reader["IdTipoDoc"].ToString());
                            obj.codigo = Reader["sCodigoSunat"].ToString();
                            obj.nombre = Reader["sNombreDoc"].ToString();
                            obj.serie = Reader["sSerie"].ToString();
                            obj.numero = int.Parse(Reader["iNumero"].ToString());
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
        public ETipoSerie ListaEditTipoDoc(int ID, int empresa, int sucursal)
        {
            ETipoSerie obj = null;
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("GEN_ObtenerDocumento");
                    CreateHelper(Connection);
                    AddInParameter("@IdTipo", ID);
                    AddInParameter("@IdEmpresa", empresa);
                    AddInParameter("@IdSucursal", sucursal);
                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            obj = new ETipoSerie();
                            obj.Id = int.Parse(Reader["IdTipoDoc"].ToString());
                            obj.codigo = Reader["sCodigoSunat"].ToString();
                            obj.nombre = Reader["sNombreDoc"].ToString();
                            obj.serie = Reader["sSerie"].ToString();
                            obj.numero = int.Parse(Reader["iNumero"].ToString());
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

        public string Insertar_UpdateTipoDoc(ETipoSerie tipo, int empresa, string Usuario, int sucursal)
        {
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("GEN_InstTipoDocumento");
                    CreateHelper(Connection);
                    AddInParameter("@iIdDocumento", tipo.Id);
                    AddInParameter("@codigoSunat", "0" + tipo.codigo);
                    AddInParameter("@NombreDocumento", tipo.nombre);
                    AddInParameter("@sSerie", tipo.serie);
                    AddInParameter("@Numero", tipo.numero);
                    AddInParameter("@usuario", Usuario);
                    AddInParameter("@IdEmpresa", empresa);
                    AddInParameter("@IdSucursal", sucursal);
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
        #endregion

        #region Tipo Color 
        public List<EGeneral> ListaColor(int empresa)
        {
            List<EGeneral> oDatos = new List<EGeneral>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("LOG_ListaColor");
                    CreateHelper(Connection);
                    AddInParameter("@IdEmpresa", empresa);
                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            EGeneral Menu = new EGeneral();
                            Menu.Id = int.Parse(Reader["iId_Color"].ToString());
                            Menu.Nombre = Reader["linea"].ToString();
                            Menu.Text = Reader["sDescripcion"].ToString();
                            Menu.sCodigo = Reader["sCodigo"].ToString();
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
        public EGeneral ListaEditColor(int ID)
        {
            EGeneral obj = null;
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("LOG_ObtenerColor");
                    CreateHelper(Connection);
                    AddInParameter("@IdColor", ID);
                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            obj = new EGeneral();
                            obj.Id = int.Parse(Reader["iId_Color"].ToString());
                            obj.Linea = int.Parse(Reader["IdLinea"].ToString());
                            obj.Text = (Reader["sDescripcion"].ToString());
                            obj.sCodigo = (Reader["sCodigo"].ToString());
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

        public string Insertar_UpdateColor(EGeneral odatos, int empresa, string Usuario)
        {
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("LOG_InstColor");
                    CreateHelper(Connection);
                    AddInParameter("@iId_Color", odatos.Id);
                    AddInParameter("@IdLinea", odatos.Linea);
                    AddInParameter("@IdEmpresa", empresa);
                    AddInParameter("@sDescripcion", odatos.Text);
                    AddInParameter("@sCodigo", odatos.sCodigo);
                    AddInParameter("@sCodReg", Usuario);
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
        #endregion

        #region Tipo Color 
        public List<EGeneral> ListaModelo(int empresa)
        {
            List<EGeneral> oDatos = new List<EGeneral>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("LOG_ListaModelo");
                    CreateHelper(Connection);
                    AddInParameter("@IdEmpresa", empresa);
                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            EGeneral Menu = new EGeneral();
                            Menu.Id = int.Parse(Reader["iId_modelo"].ToString());
                            Menu.Nombre = Reader["sCodigo"].ToString();
                            Menu.Text = Reader["sDescripcion"].ToString();
                            Menu.sCodigo = Reader["Linea"].ToString();
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
        public EGeneral ListaEditModelo(int ID)
        {
            EGeneral obj = null;
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("LOG_ObtenerModelo");
                    CreateHelper(Connection);
                    AddInParameter("@IdModelo", ID);
                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            obj = new EGeneral();
                            obj.Id = int.Parse(Reader["iId_Modelo"].ToString());
                            obj.Linea = int.Parse(Reader["IdLinea"].ToString());
                            obj.Text = (Reader["sDescripcion"].ToString());
                            obj.sCodigo = (Reader["sCodigo"].ToString());
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

        public string Insertar_UpdateModelo(EGeneral odatos, int empresa, string Usuario)
        {
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("LOG_InstModelo");
                    CreateHelper(Connection);
                    AddInParameter("@iId_Modelo", odatos.Id);
                    AddInParameter("@IdLinea", odatos.Linea);
                    AddInParameter("@IdEmpresa", empresa);
                    AddInParameter("@sDescripcion", odatos.Text);
                    AddInParameter("@sCodigo", odatos.sCodigo);
                    AddInParameter("@sCodReg", Usuario);
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
        #endregion

        public string InstCodigo(int IdProducto, int cantidad, string usuario)
        {
            var smensaje = "";
            using (var Connection = GetConnection(BaseDeDatos))
            {
                for (int i = 0; i < cantidad; i++)
                {
                    try
                    {
                        Connection.Open();
                        SetQuery("LOG_InstCodigo");
                        CreateHelper(Connection);
                        AddInParameter("@IdMaterial", IdProducto);
                        AddInParameter("@Usuario", usuario);
                        AddOutParameter("@Mensaje", (DbType)SqlDbType.VarChar);
                        ExecuteQuery();
                        smensaje = GetOutput("@Mensaje").ToString();
                        string[] vMensaje = smensaje.Split('|');
                        if (!vMensaje[0].Equals("success"))
                        {
                            //throw new Exception();
                            return smensaje;
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
                return smensaje;
            }
        }
    }
}
