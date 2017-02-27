using ADOFactory;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace PlanoAcaoCore
{
    public class Pa_BaseObject
    {
        public int Id { get; set; }
        public DateTime AddDate { get; set; }
        public DateTime AlterDate { get; set; }

        protected static string catalog { get { return "PlanoDeAcao"; } }
        protected static string dataSource { get { return @"SERVERGRT\MSSQLSERVER2014"; } }
        protected static string user { get { return "sa"; } }
        protected static string pass { get { return "1qazmko0"; } }

        protected int Update(SqlCommand cmd)
        {
            using (var db = new Factory(dataSource, catalog, pass, user))
                return db.InsertUpdateData(cmd);
        }

        protected int Salvar(SqlCommand cmd)
        {
            using (var db = new Factory(dataSource, catalog, pass, user))
                return db.InsertUpdateData(cmd);
            
        }

        protected static int SalvarStatic(SqlCommand cmd)
        {
            using (var db = new Factory(dataSource, catalog, pass, user))
                return db.InsertUpdateData(cmd);

        }

        public static T SalvarGenerico<T>(T obj)
        {
            using (var db = new Factory(dataSource, catalog, pass, user))
                return db.InsertUpdateData<T>(obj);

        }

        protected static List<T> ListarGenerico<T>(string query)
        {
            List<T> listReturn;
            using (var db = new Factory(dataSource, catalog, pass, user))
                listReturn = db.SearchQuery<T>(query);
            return listReturn;
        }

        protected static T GetGenerico<T>(string query)
        {
            T objReturn;
            using (var db = new Factory(dataSource, catalog, pass, user))
                objReturn = db.SearchQuery<T>(query).FirstOrDefault();

            return objReturn;

        }

        public static int GenericInsert(string valor, string table)
        {
            string query;

            query = " INSERT INTO [dbo].[" + table + "] " +
                    "\n       ([Name])                  " +
                    "\n VALUES                          " +
                    "\n       (@Name)                   " +
                    "\n       SELECT CAST(scope_identity() AS int) ";

            SqlCommand cmd;
            cmd = new SqlCommand(query);
            cmd.Parameters.AddWithValue("@Name", valor);

            return SalvarStatic(cmd);
        }

    }
}