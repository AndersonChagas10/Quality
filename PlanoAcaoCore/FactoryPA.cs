
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Security;

namespace PlanoAcaoCore
{
    public class FactoryPA : IDisposable
    {
        public SqlConnection connection;
        private SqlConnectionStringBuilder connectionString;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataSource">Ip do DB ou caminho físico.</param>
        /// <param name="catalog">Nome do catalogo do DB.</param>
        /// <param name="password">Senha.</param>
        /// <param name="user">User.</param>
        public static SqlConnection GetConnection(string dataSource, string catalog, string password, string user)
        {
            var connectionString = new SqlConnectionStringBuilder();
            connectionString.DataSource = dataSource;//@"SERVERGRT\MSSQLSERVER2014";
            connectionString.InitialCatalog = catalog;//"SgqDbDev";
            connectionString.Password = password;//"1qazmko0";
            connectionString.UserID = user;// "sa";

            try
            {
                var connection = new SqlConnection();
                {
                    connection.ConnectionString = connectionString.ConnectionString;
                }
                connection.Open();
                return connection;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataSource">Ip do DB ou caminho físico.</param>
        /// <param name="catalog">Nome do catalogo do DB.</param>
        /// <param name="password">Senha.</param>
        /// <param name="user">User.</param>
        public FactoryPA(string dataSource, string catalog, string password, string user)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionStringDoWebConfig"></param>
        public FactoryPA(string connectionStringDoWebConfig)
        {
            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings[connectionStringDoWebConfig].ConnectionString;
                connection = new SqlConnection();
                connection.ConnectionString = connectionString;
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

        public List<T> SearchQuery<T>(string query)
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
                            if (!reader.IsDBNull(i))
                                if (instance.GetType().GetProperty(reader.GetName(i)) != null)
                                    instance.GetType().GetProperty(reader.GetName(i)).SetValue(instance, reader[i]);
                        }
                        listReturn.Add((T)instance);
                    }
                }
                return listReturn;
            }
            catch (Exception e)
            {
                closeConnection();
                throw e;
            }

        }

        public int ExecuteSql(string query)
        {
            try
            {
                SqlCommand command = new SqlCommand(query, connection);
                return command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                closeConnection();
                throw e;
            }
        }

        public List<T> ExecuteStoredProcedure<T>(string query)
        {
            var listReturn = new List<T>();
            try
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    // iterate through results, printing each to console
                    while (reader.Read())
                    {
                        //var columns = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToList();
                        object instance = Activator.CreateInstance(typeof(T));

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            if (!reader.IsDBNull(i))
                                instance.GetType().GetProperty(reader.GetName(i)).SetValue(instance, reader[i]);
                        }
                        listReturn.Add((T)instance);
                    }
                }
                return listReturn;
            }
            catch (Exception e)
            {
                closeConnection();
                throw e;
            }
        }

        [SecuritySafeCritical]
        protected void closeConnection(bool disposing = false)
        {
            if (((connection != null)))
            {
                connection.Close();
                if (!disposing)
                    connection.Dispose();
            }
        }

        public void Dispose()
        {
            closeConnection(true);
            //Dispose();
        }
    }
}
