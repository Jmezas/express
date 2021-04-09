using SIS.Factory;
using SIS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace SIS.Data
{
    public class DConfiguracion : DBHelper
    {
        private static DConfiguracion Instancia;
        private DataBase BaseDeDatos;

        public DConfiguracion(DataBase BaseDeDatos) : base(BaseDeDatos)
        {
            this.BaseDeDatos = BaseDeDatos;
        }

        public static DConfiguracion ObtenerInstancia(DataBase BaseDeDatos)
        {
            if (Instancia == null)
            {
                Instancia = new DConfiguracion(BaseDeDatos);
            }
            return Instancia;
        }

        public List<EConfiguracion> ListarConfiguraciones(int Flag, int empresa, int sucursal)
        {
            List<EConfiguracion> oDatos = new List<EConfiguracion>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("LOG_DatosConfiguracion");
                    CreateHelper(Connection);
                    AddInParameter("@IdFlag", Flag);
                    AddInParameter("@IdEmpresa", empresa);
                    AddInParameter("@IdSucursal", sucursal);
                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            EConfiguracion obj = new EConfiguracion();
                            obj.Id = int.Parse(Reader["Id"].ToString());
                            obj.Valor = Reader["Valor"].ToString();
                            obj.Descripcion = (Reader["Descripcion"].ToString());
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

        public string ActualizarConfiguracion(int Flag, EConfiguracion oDatos, string Usuario)
        {
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("LOG_InsConfiguracion");
                    CreateHelper(Connection);
                    AddInParameter("@Flag", Flag);
                    AddInParameter("@Id", oDatos.Id);
                    AddInParameter("@Valor", oDatos.Valor);
                    AddInParameter("@IdEmpresa", oDatos.Empresa.Id);
                    AddInParameter("@IdSucursal", oDatos.Sucursal.IdSucursal);
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

    }
}
