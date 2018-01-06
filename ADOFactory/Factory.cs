using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Security;

namespace ADOFactory
{
    public class Factory : IDisposable
    {
        public SqlConnection connection;
        private SqlConnectionStringBuilder connectionString;
        private SqlConnectionStringBuilder connectionString1;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataSource">Ip do DB ou caminho físico.</param>
        /// <param name="catalog">Nome do catalogo do DB.</param>
        /// <param name="password">Senha.</param>
        /// <param name="user">User.</param>
        public Factory(string dataSource, string catalog, string password, string user)
        {
            //var teste = System.Configuration.ConfigurationManager.AppSettings["SgqDbDev"];
            connectionString = new SqlConnectionStringBuilder();
            connectionString.DataSource = dataSource;//@"SERVERGRT\MSSQLSERVER2014";
            connectionString.InitialCatalog = catalog;//"SgqDbDev";

            //if (!string.IsNullOrEmpty(password))
            //{
            connectionString.Password = password;//"1qazmko0";
            connectionString.UserID = user;// "sa";
            //}
            //else
            //{
            //    connectionString.IntegratedSecurity = true;
            //}

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
        public Factory(string connectionStringDoWebConfig)
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

        public Factory(SqlConnectionStringBuilder connectionString1)
        {
            try
            {
                var connectionString = connectionString1.ConnectionString;
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
                            try
                            {
                                if (!reader.IsDBNull(i))
                                    if (instance.GetType().GetProperty(reader.GetName(i)) != null)
                                        instance.GetType().GetProperty(reader.GetName(i)).SetValue(instance, reader[i]);
                            }
                            catch (Exception e)
                            {
                                throw e;
                            }
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

        public List<JObject> QueryNinjaADO(string query)
        {
            List<JObject> items = new List<JObject>();
            SqlCommand command = new SqlCommand(query, connection);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    JObject row = new JObject();
                    for (int i = 0; i < reader.FieldCount; i++)
                        row[reader.GetName(i)] = reader[i].ToString();

                    items.Add(row);
                }
            }
            return items;
        }

        public int InsertUpdateData(SqlCommand cmd)
        {

            cmd.CommandType = CommandType.Text;
            cmd.Connection = connection;
            try
            {
                int newID;
                newID = (int)cmd.ExecuteScalar();
                return newID;
            }
            catch (Exception ex)
            {
                //Response.Write(ex.Message);
                throw ex;
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
        }

        public T InsertUpdateData<T>(T obj)
        {
            SqlCommand cmd = GetQuery(obj);


            cmd.CommandType = CommandType.Text;
            cmd.Connection = connection;
            try
            {
                if (obj.GetType().GetProperty("Id") != null)
                {
                    var id = (int)obj.GetType().GetProperty("Id").GetValue(obj, null);
                    if (id > 0)
                        cmd.ExecuteNonQuery();
                    else
                    {
                        int newID;
                        newID = (int)cmd.ExecuteScalar();
                        obj.GetType().GetProperty("Id").SetValue(obj, newID);
                    }
                }
            }
            catch (Exception ex)
            {
                //Response.Write(ex.Message);
                throw ex;
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }

            return obj;
        }

        private static SqlCommand GetQuery<T>(T obj)
        {
            SqlCommand cmd = new SqlCommand();

            if (obj.GetType().GetProperty("Id") != null)
            {
                var id = (int)obj.GetType().GetProperty("Id").GetValue(obj, null);
                if (id > 0)
                    cmd = UpdateQuery(obj, id);
                else
                    cmd = InsertQuery(obj);
            }

            return cmd;
        }

        private static SqlCommand UpdateQuery<T>(T obj, int id)
        {

            var query = "UPDATE " + obj.GetType().Name;
            var queryProps = " SET ";

            foreach (var item in obj.GetType().GetProperties())
            {
                if (/*item != null &&*/ item.Name != "Id" && item.Name != "AddDate" && item.Name != "AlterDate" && !item.Name.StartsWith("_"))
                {
                    //if (obj.GetType().GetProperty(item.Name).GetValue(obj) == null)
                    //    continue;

                    //if (item.PropertyType.Name == "String")
                    //    if (string.IsNullOrEmpty(obj.GetType().GetProperty(item.Name).GetValue(obj) as string))
                    //        continue;

                    if (item.PropertyType.IsClass && item.PropertyType.Name != "String")
                        continue;

                    queryProps += "\n " + item.Name + " = @" + item.Name + ", ";
                }
            }

            var queryDebug = "UPDATE " + obj.GetType().Name;
            var queryPropsDebug = " SET ";

            foreach (var item in obj.GetType().GetProperties())
            {
                if (/*item != null &&*/ item.Name != "Id" && item.Name != "AddDate" && item.Name != "AlterDate" && !item.Name.StartsWith("_"))
                {
                    //if (obj.GetType().GetProperty(item.Name).GetValue(obj) == null)
                    //    continue;

                    //if (item.PropertyType.Name == "String")
                    //    if (string.IsNullOrEmpty(obj.GetType().GetProperty(item.Name).GetValue(obj) as string))
                    //        continue;

                    if (item.PropertyType.IsClass && item.PropertyType.Name != "String")
                        continue;

                    queryPropsDebug += "\n " + item.Name + " = " + (obj.GetType().GetProperty(item.Name).GetValue(obj) == null ? "null" : item.GetValue(obj)) + ", ";
                }
            }

            var resultDebug = queryDebug + queryPropsDebug;

            queryProps = queryProps.Substring(0, queryProps.LastIndexOf(", ")) + "\n ";

            query += queryProps;
            query += " WHERE Id = " + id;
            query += " \n SELECT CAST(scope_identity() AS int)";

            SqlCommand cmd;
            cmd = new SqlCommand(query);
            var pqp = string.Empty;
            foreach (var item in obj.GetType().GetProperties())
            {
                if (/*item != null &&*/ item.Name != "Id" && item.Name != "AddDate" && item.Name != "AlterDate")
                {
                    if (item.PropertyType.Name == "String")
                        if (string.IsNullOrEmpty(obj.GetType().GetProperty(item.Name).GetValue(obj) as string))
                        {
                            cmd.Parameters.AddWithValue("@" + item.Name, "");
                            continue;
                        }

                    if (obj.GetType().GetProperty(item.Name).GetValue(obj) == null)
                    {
                        //if(item.PropertyType.Name ==)
                        cmd.Parameters.AddWithValue("@" + item.Name, obj.GetType().GetProperty(item.Name).GetValue(obj));
                        pqp += "@" + item.Name + " = " + obj.GetType().GetProperty(item.Name).GetValue(obj);
                        continue;
                    }


                    if (item.PropertyType.IsClass && item.PropertyType.Name != "String")
                        continue;

                    cmd.Parameters.AddWithValue("@" + item.Name, obj.GetType().GetProperty(item.Name).GetValue(obj));
                    pqp += "@" + item.Name + " = " + obj.GetType().GetProperty(item.Name).GetValue(obj);

                }
            }

            return cmd;

        }

        private static SqlCommand InsertQuery<T>(T obj)
        {
            var query = "INSERT INTO " + obj.GetType().Name;
            var queryProps = "(";
            var queryValues = " VALUES (";

            foreach (var item in obj.GetType().GetProperties())
            {
                if (item != null && item.Name != "Id" && item.Name != "AddDate" && item.Name != "AlterDate" && !item.Name.StartsWith("_"))
                {
                    if (obj.GetType().GetProperty(item.Name).GetValue(obj) == null)
                        continue;

                    if (item.PropertyType.Name == "String")
                        if (string.IsNullOrEmpty(obj.GetType().GetProperty(item.Name).GetValue(obj) as string))
                            continue;

                    if (item.PropertyType.IsClass && item.PropertyType.Name != "String")
                        continue;

                    queryProps += "\n " + item.Name + ", ";
                    queryValues += "\n @" + item.Name + ", ";
                }
            }

            queryProps = queryProps.Substring(0, queryProps.LastIndexOf(", ")) + "\n )";
            queryValues = queryValues.Substring(0, queryValues.LastIndexOf(", ")) + "\n )";

            query += queryProps;
            query += queryValues;
            query += " \n SELECT CAST(scope_identity() AS int)";

            SqlCommand cmd;
            cmd = new SqlCommand(query);

            foreach (var item in obj.GetType().GetProperties())
            {
                if (item != null && item.Name != "Id" && item.Name != "AddDate" && item.Name != "AlterDate")
                {
                    if (obj.GetType().GetProperty(item.Name).GetValue(obj) == null)
                        continue;

                    if (item.PropertyType.Name == "String")
                        if (string.IsNullOrEmpty(obj.GetType().GetProperty(item.Name).GetValue(obj) as string))
                            continue;

                    if (item.PropertyType.IsClass && item.PropertyType.Name != "String")
                        continue;

                    cmd.Parameters.AddWithValue("@" + item.Name, obj.GetType().GetProperty(item.Name).GetValue(obj));
                }
            }

            return cmd;

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


        //private static void Salvar<T>(string query, T obj)
        //{
        //    int id;
        //    using (var db = new FactoryPA(""))
        //        id = db.ExecuteSql(query);

        //    obj.GetType().GetProperty("Id").SetValue(obj, id);
        //}

        //private static void Update<T>(string query, T obj)
        //{
        //    var id = (int)obj.GetType().GetProperty("Id").GetValue(obj, null);
        //    using (var db = new FactoryPA(""))
        //        db.ExecuteSql(query);
        //}
    }
}
