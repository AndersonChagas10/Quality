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

        private static string catalog { get { return "PlanoDeAcao"; } }
        private static string dataSource { get { return @"SERVERGRT\MSSQLSERVER2014"; } }
        private static string user { get { return "sa"; } }
        private static string pass { get { return "1qazmko0"; } }

        public int Update(SqlCommand cmd)
        {
            using (var db = new Factory(dataSource, catalog, pass, user))
                return db.InsertUpdateData(cmd);
        }

        public int Salvar(SqlCommand cmd)
        {
            using (var db = new Factory(dataSource, catalog, pass, user))
                return db.InsertUpdateData(cmd);
            
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

    }
}