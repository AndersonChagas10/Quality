using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security;

namespace Dominio.ADO
{
    public class FactoryADO : IDisposable
    {

        public SqlConnection connection;
        private SqlConnectionStringBuilder connectionString;
        public SqlCommand command;

        public FactoryADO(string dataSource, string catalog, string password, string user)
        {
            connectionString = new SqlConnectionStringBuilder();
            connectionString.DataSource = dataSource;//@"SERVERGRT\MSSQLSERVER2014";
            connectionString.InitialCatalog = catalog;//"SgqDbDev";
            connectionString.Password = password;//"1qazmko0";
            connectionString.UserID = user;// "sa";

            try
            {
                connection = new SqlConnection();
                {
                    connection.ConnectionString = connectionString.ConnectionString;
                }
                connection.Open();
            }
            catch (SqlException ex)
            {
                closeConnection();
                throw ex;
            }
            catch (Exception ex)
            {
                closeConnection();
                throw ex;
            }
        }

        public List<T> ExecuteQuery<T>(string query)
        {
            try
            {
                var listReturn = new List<T>();
                
                SqlCommand command = new SqlCommand(query, connection);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //var columns = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToList();
                        object instance = Activator.CreateInstance(typeof(T));
                       
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                           if(!reader.IsDBNull(i))
                                instance.GetType().GetProperty(reader.GetName(i)).SetValue(instance, reader[i]);
                        }
                        listReturn.Add((T)instance);
                    }
                }
                return listReturn;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        [SecuritySafeCritical]
        protected void closeConnection()
        {
            if (((connection != null)))
            {
                connection.Close();
                connection.Dispose();
            }
        }
     
        public void Dispose()
        {
            closeConnection();
            Dispose();
        }
    }
}
