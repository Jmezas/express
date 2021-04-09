using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient; 

namespace SIS.Factory
{
   public class DBHelper: DBConnection, IDisposable
    {


        private DataBase Database;
        private DbCommand Command;
        private string Query;

        protected bool AllowNull = true;

        public DBHelper(DataBase Database)
        {
            this.Database = Database;
        }

        protected void SetQuery(string Query)
        {
            this.Query = Query;
        }

        protected void CreateHelper(DbConnection Connection)
        {
            Command = GetCommand();
            Command.Connection = Connection;
            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandText = Query;
        }

        protected void CreateHelper(DbConnection Connection, DbTransaction Transaction)
        {
            Command = GetCommand();
            Command.Connection = Connection;
            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandText = Query;
            Command.Transaction = Transaction;
        }

        protected void CreateHelperText(DbConnection Connection)
        {
            Command = GetCommand();
            Command.Connection = Connection;
            Command.CommandType = CommandType.Text;
            Command.CommandText = Query;
        }

        protected void CreateHelperText(DbConnection Connection, DbTransaction Transaction)
        {
            Command = GetCommand();
            Command.Connection = Connection;
            Command.CommandType = CommandType.Text;
            Command.CommandText = Query;
            Command.Transaction = Transaction;
        }

        private DbCommand GetCommand()
        {
            switch (Database)
            {
                case DataBase.SqlServer:
                    Command = new SqlCommand();
                    break;
                case DataBase.Oracle:
                    break;
         
            }
            return Command;
        }

        private DbParameter GetParam()
        {
            DbParameter Parameter = null;
            switch (Database)
            {
                case DataBase.SqlServer:
                    Parameter = new SqlParameter();
                    break;
                case DataBase.Oracle:
                    break;
               
            }
            return Parameter;
        }

        protected DbDataReader ExecuteReader()
        {
            if (Query != null)
            {
                if (Command != null)
                {
                    try
                    {
                        return Command.ExecuteReader();
                    }
                    catch (Exception Exception)
                    {
                        throw Exception;
                    }
                }
            }
            throw new Exception("No existe una consulta que enviar a la base de datos.");
        }

        protected DbDataReader ExecuteReader(int TimeOut)
        {
            if (Query != null)
            {
                if (Command != null)
                {
                    try
                    {
                        Command.CommandTimeout = TimeOut;
                        return Command.ExecuteReader();
                    }
                    catch (Exception Exception)
                    {
                        throw Exception;
                    }
                }
            }
            throw new Exception("No existe una consulta que enviar a la base de datos.");
        }

        protected int ExecuteQuery()
        {
            if (Query != null)
            {
                if (Command != null)
                {
                    try
                    {
                        return Command.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            throw new Exception("No existe una consulta que enviar a la base de datos.");
        }

        protected int ExecuteQuery(int TimeOut)
        {
            if (Query != null)
            {
                if (Command != null)
                {
                    try
                    {
                        Command.CommandTimeout = TimeOut;
                        return Command.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            throw new Exception("No existe una consulta que enviar a la base de datos.");
        }

        protected object ExecuteScalar()
        {
            if (Query != null)
            {
                if (Command != null)
                {
                    try
                    {
                        return Command.ExecuteScalar();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            throw new Exception("No existe una consulta que enviar a la base de datos.");
        }

        protected object ExecuteScalar(int TimeOut)
        {
            if (Query != null)
            {
                if (Command != null)
                {
                    try
                    {
                        Command.CommandTimeout = TimeOut;
                        return Command.ExecuteScalar();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            throw new Exception("No existe una consulta que enviar a la base de datos.");
        }

        protected void AddInParameter(string Name, object Value)
        {
            if (Query != null)
            {
                if (Command != null)
                {
                    DbParameter Parameter = GetParam();
                    Parameter.Direction = ParameterDirection.Input;
                    Parameter.ParameterName = Name;
                    Parameter.Value = Value;
                    Command.Parameters.Add(Parameter);
                    return;
                }
                throw new Exception("No se ha creado el comando a ejecutar en la base de datos [CreateHelper]");
            }
            throw new Exception("No existe una consulta que enviar a la base de datos.");
        }

        protected void AddInParameter(string Name, object Value, bool Nullable)
        {
            if (Query != null)
            {
                if (Command != null)
                {
                    DbParameter Parameter = GetParam();
                    Parameter.Direction = ParameterDirection.Input;
                    Parameter.ParameterName = Name;
                    Parameter.Value = Nullable ? (Value == null ? DBNull.Value : Value) : Value;
                    Command.Parameters.Add(Parameter);
                    return;
                }
                throw new Exception("No se ha creado el comando a ejecutar en la base de datos [CreateHelper]");
            }
            throw new Exception("No existe una consulta que enviar a la base de datos.");
        }

        protected void AddOutParameter(string Name, DbType Type)
        {
            if (Query != null)
            {
                if (Command != null)
                {
                    DbParameter Parameter = GetParam();
                    Parameter.Direction = ParameterDirection.Output;
                    Parameter.ParameterName = Name;
                    Parameter.Size = int.MaxValue;
                    Parameter.DbType = Type;
                    Command.Parameters.Add(Parameter);
                    return;
                }
                throw new Exception("No se ha creado el comando a ejecutar en la base de datos [CreateHelper]");
            }
            throw new Exception("No existe una consulta que enviar a la base de datos.");
        }

        protected object GetOutput(string Name)
        {
            foreach (DbParameter Parameter in Command.Parameters)
            {
                if (Parameter.ParameterName.Equals(Name))
                {
                    return Parameter.Value;
                }
            }
            throw new Exception(string.Format("No se ha encontrado el parámetro de salida {0}", Name));
        }

        public void Dispose()
        {
            Command.Dispose();
        }
    }
}
