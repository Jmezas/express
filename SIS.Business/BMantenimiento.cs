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
    public class BMantenimiento
    {
        private static BMantenimiento Instancia;
        private DMantenimiento Data = DMantenimiento.ObtenerInstancia(DataBase.SqlServer);

        public static BMantenimiento ObtenerInstancia()
        {
            if (Instancia == null)
            {
                Instancia = new BMantenimiento();
            }
            return Instancia;
        }
        
        public string ActualizarDescuentos(List<EMaterial> sLista, string Usuario)
        {
            try
            {
                return Data.ActualizarDescuentos(sLista, Usuario);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public string InstMaterial(EMaterial Material, string Usuario)
        {
            try
            {
                return Data.InstMaterial(Material, Usuario);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public EMaterial ListaEditMaterial(int IdMaterial, int Empresa)
        {
            try
            {
                return Data.ListaEditMaterial(IdMaterial, Empresa);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public List<EMaterial> ListaMaterial(int Empresa/*,DataTableAjaxPostModel datable*/)
        {
            try
            {
                return Data.ListaMaterial(Empresa);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }

        public List<EMaterial> ListadoGiftCard(int Empresa)
        {
            try
            {
                return Data.ListadoGiftCard(Empresa);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public List<EMaterial> ListadodetGiftCard(int Empresa, int idMaterial)
        {
            try
            {
                return Data.ListadodetGiftCard(Empresa, idMaterial);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }

        }



        public string InstUnidad(EUnidad Unidad, string Usuario)
        {
            try
            {
                return Data.InstUnidad(Unidad, Usuario);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public EUnidad ListaEditUnidad(int IdUnidad, int Empresa)
        {
            try
            {
                return Data.ListaEditUnidad(IdUnidad, Empresa);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public List<EUnidad> ListaUnidad(int Empresa)
        {
            try
            {
                return Data.ListaUnidad(Empresa);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public string InstMarca(EMarca Marca, string Usuario)
        {
            try
            {
                return Data.InstMarca(Marca, Usuario);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public EMarca ListaEditMarca(int IdMarca, int Empresa)
        {
            try
            {
                return Data.ListaEditMarca(IdMarca, Empresa);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }

        public List<EMarca> ListaMarca(int Empresa)
        {
            try
            {
                return Data.ListaMarca(Empresa);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }

        }
        public string InstCategoria(ECategoria Categoria, string Usuario)
        {
            try
            {
                return Data.InstCategoria(Categoria, Usuario);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public ECategoria ListaEditCategoria(int IdCategoria, int Empresa)
        {
            try
            {
                return Data.ListaEditCategoria(IdCategoria, Empresa);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }

        }
        public List<ECategoria> ListaCategoria(int Empresa)
        {
            try
            {
                return Data.ListaCategoria(Empresa);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }

        }
        public string InstSubCategoria(ESubCateoria SubCategoria, string Usuario)
        {
            try
            {
                return Data.InstSubCategoria(SubCategoria, Usuario);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }

        }

        public ESubCateoria ListaEditSubCategoria(int IdCategoria, int Empresa)
        {
            try
            {
                return Data.ListaEditSubCategoria(IdCategoria, Empresa);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }

        public List<ESubCateoria> ListaSubCategoria(int Empresa)
        {
            try
            {
                return Data.ListaSubCategoria(Empresa);
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

        public string InstCliente(ECliente Cliente, string Usuario)
        {
            try
            {
                return Data.InstCliente(Cliente, Usuario);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }

        }
        public ECliente ListaEditCliente(int IdCliente, int Empresa)
        {
            try
            {
                return Data.ListaEditCliente(IdCliente, Empresa);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public List<ECliente> ListaCliente(int Empresa)
        {
            try
            {
                return Data.ListaCliente(Empresa);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public string InstProveedor(EProveedor proveedor, string Usuario)
        {
            try
            {
                return Data.InstProveedor(proveedor, Usuario);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public EProveedor ListaEditProveedor(int IdPorveedor, int Empresa)
        {
            try
            {
                return Data.ListaEditProveedor(IdPorveedor, Empresa);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public List<EProveedor> ListaProveedor(int Empresa)
        {
            try
            {
                return Data.ListaProveedor(Empresa);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }

        public string InstSucursal(ESucursal oDatos, string Usuario)
        {
            try
            {
                return Data.InstSucursal(oDatos, Usuario);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public ESucursal ListaEditSucursal(int Id, int Empresa)
        {
            try
            {
                return Data.ListaEditSucursal(Id, Empresa);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public List<ESucursal> ListaSucursal(int Empresa)
        {
            try
            {
                return Data.ListaSucursal(Empresa);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }

        public string InstAlmacen(EAlmacen oDatos, string Usuario)
        {
            try
            {
                return Data.InstAlmacen(oDatos, Usuario);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public EAlmacen ListaEditAlmacen(int Id, int Empresa)
        {
            try
            {
                return Data.ListaEditAlmacen(Id, Empresa);
            }
            catch (Exception Exception)
            {
                throw Exception;

            }
        }
        public List<EAlmacen> ListaAlmacen(int Empresa)
        {
            try
            {
                return Data.ListaAlmacen(Empresa);
            }
            catch (Exception Exception)
            {
                throw Exception;

            }
        }

        public List<EAlmacen> ListaPermisoAlmacen(int Empresa)
        {
            try
            {
                return Data.ListaPermisoAlmacen(Empresa);
            }
            catch (Exception Exception)
            {
                throw Exception;

            }
        }

        public List<EAlmacen> ListaCBO_UsuarioAlmacen(int Flag, int Empresa)
        {

            try
            {
                return Data.ListaCBO_UsuarioAlmacen(Flag, Empresa);
            }
            catch (Exception Exception)
            {
                throw Exception;

            }
        }
        public List<EAlmacen> ListaAlmacenPermisos(int Flag, string login, int Empresa, int sucursal)
        {
            try
            {
                return Data.ListaAlmacenPermisos(Flag, login, Empresa, sucursal);
            }
            catch (Exception Exception)
            {
                throw Exception;

            }
        }
        public string InstUpdPermisoAlmacem(List<EAlmacen> Almacen, int Flag, string Usuario)
        {
            try
            {
                return Data.InstUpdPermisoAlmacem(Almacen, Flag, Usuario);
            }
            catch (Exception Exception)
            {
                throw Exception;

            }
        }
        public string InstEmpresa(EEmpresa oDatos, string Usuario)
        {
            try
            {
                return Data.InstEmpresa(oDatos, Usuario);
            }
            catch (Exception Exception)
            {
                throw Exception;

            }
        }
        public EEmpresa ListaEditEmpresa(int Empresa)
        {
            try
            {
                return Data.ListaEditEmpresa(Empresa);
            }
            catch (Exception Exception)
            {
                throw Exception;

            }
        }
        public List<EEmpresa> ListaEmpresa()
        {
            try
            {
                return Data.ListaEmpresa();
            }
            catch (Exception Exception)
            {
                throw Exception;

            }
        }
        public EPerfil ListaEnviarCorreo(int Empresa)
        {
            try
            {
                return Data.ListaEnviarCorreo(Empresa);
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
                return Data.Insertar_UpdateMenu(Menu, Usuario);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public List<EGeneral> ListaCaja(int empresa, int sucursal)
        {
            try
            {
                return Data.ListaCaja(empresa, sucursal);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }

        }
        public EGeneral ListaEditCaja(int ID)
        {
            try
            {
                return Data.ListaEditCaja(ID);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public string Insertar_UpdateCaja(EGeneral Menu, int empresa, int sucursal, string Usuario)
        {
            try
            {
                return Data.Insertar_UpdateCaja(Menu, empresa, sucursal, Usuario);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public List<EGeneral> ListaTipo(int empresa)
        {
            try
            {
                return Data.ListaTipo(empresa);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }

        }

        public List<EBancos> ListarBancos(int empresa)
        {
            try
            {
                return Data.ListarBancos(empresa);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }

        public List<ETipoEnvio> ListarTipoEnvio(int empresa)
        {
            try
            {
                return Data.ListarTipoEnvio(empresa);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        


        public EGeneral ListaEditTipo(int ID, int empresa)
        {
            try
            {
                return Data.ListaEditTipo(ID, empresa);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }

        public EBancos BuscarBancoId(int ID, int empresa)
        {
            try
            {
                return Data.BuscarBancoId(ID, empresa);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public ETipoEnvio BuscarTipoEnvioId(int ID, int empresa)
        {
            try
            {
                return Data.BuscarTipoEnvioId(ID, empresa);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }


        public string Insertar_UpdateTipo(EGeneral Menu, int empresa, string Usuario)
        {
            try
            {
                return Data.Insertar_UpdateTipo(Menu, empresa, Usuario);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }

        }
        public string RegistrarEditarBanco(EBancos banco, int empresa, string Usuario)
        {
            try
            {
                return Data.RegistrarEditarBanco(banco, empresa, Usuario);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }

        }
        public string RegistrarEditarTipoEnvio(ETipoEnvio Obj, int empresa, string Usuario)
        {
            try
            {
                return Data.RegistrarEditarTipoEnvio(Obj, empresa, Usuario);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }

        }
        


        public List<ETipoSerie> ListaTipoSerie(int empresa, int sucursal)
        {
            try
            {
                return Data.ListaTipoSerie(empresa, sucursal);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public ETipoSerie ListaEditTipoDoc(int ID, int empresa, int sucursal)
        {
            try
            {
                return Data.ListaEditTipoDoc(ID, empresa, sucursal);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public string Insertar_UpdateTipoDoc(ETipoSerie tipo, int empresa, string Usuario, int sucursal)
        {
            try
            {
                return Data.Insertar_UpdateTipoDoc(tipo, empresa, Usuario, sucursal);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }

        }

        public List<EGeneral> ListaColor(int empresa)
        {
            try
            {
                return Data.ListaColor(empresa);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public EGeneral ListaEditColor(int ID)
        {
            try
            {
                return Data.ListaEditColor(ID);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public string Insertar_UpdateColor(EGeneral odatos, int empresa, string Usuario)
        {
            try
            {
                return Data.Insertar_UpdateColor(odatos, empresa, Usuario);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public List<EGeneral> ListaModelo(int empresa)
        {
            try
            {
                return Data.ListaModelo(empresa);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public EGeneral ListaEditModelo(int ID)
        {
            try
            {
                return Data.ListaEditModelo(ID);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public string Insertar_UpdateModelo(EGeneral odatos, int empresa, string Usuario)
        {
            try
            {
                return Data.Insertar_UpdateModelo(odatos, empresa, Usuario);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public string InstCodigo(int IdProducto, int cantidad, string usuario)
        {
            try
            {
                return Data.InstCodigo(IdProducto, cantidad, usuario);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }

        }
    }
}
