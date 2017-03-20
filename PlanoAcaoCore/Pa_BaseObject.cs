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


        //protected static string catalog { get { return "PlanoDeAcao"; } }
        //protected static string dataSource { get { return @"SERVERGRT\MSSQLSERVER2014"; } }
        //protected static string user { get { return "sa"; } }
        //protected static string pass { get { return "1qazmko0"; } }

        //protected static string catalog { get { return "dbGQualidade_"; } }
        //protected static string dataSource { get { return @"dellgabriel\mssql2014"; } }
        //protected static string user { get { return "sa"; } }
        //protected static string pass { get { return "betsy1"; } }

        protected static string catalog { get { return "SgqDbDev2"; } }
        protected static string dataSource { get { return @"SgqDbDev2.mssql.somee.com"; } }
        protected static string user { get { return "gcnunes7_SQLLogin_1"; } }
        protected static string pass { get { return "12y3srpfp5"; } }

        

        //protected static string catalog { get { return "dbGQualidadeTeste"; } }
        //protected static string dataSource { get { return @"10.255.0.41"; } }
        //protected static string user { get { return "UserGQualidade"; } }
        //protected static string pass { get { return "grJsoluco3s"; } }

        #region Validação de campos Front end

        protected string message { get; set; }
        protected static void VerificaMensagemCamposObrigatorios(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                message = "\n Campos necessários para o cadastro não foram preenchidos: " + message;

                throw new Exception(message.TrimEnd(',') + ".");
            }
        }

        #endregion

        #region DataBase

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

        public static List<T> ListarGenerico<T>(string query)
        {
            List<T> listReturn;
            using (var db = new Factory(dataSource, catalog, pass, user))
                listReturn = db.SearchQuery<T>(query);
            return listReturn;
        }

        public static T GetGenerico<T>(string query)
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

        public static int GenericInsert(string valor, string table, int predecessor, string fk)
        {
            string query;

            query = " INSERT INTO [dbo].[" + table + "] " +
                    "\n       ([Name],                  " +
                    "\n        " + fk + "     )          " +
                    "\n VALUES                          " +
                    "\n       (@Name,                   " +
                    "\n        @predecessor)            " +
                    "\n       SELECT CAST(scope_identity() AS int) ";

            SqlCommand cmd;
            cmd = new SqlCommand(query);
            cmd.Parameters.AddWithValue("@Name", valor);
            cmd.Parameters.AddWithValue("@predecessor", predecessor);

            return SalvarStatic(cmd);
        }

        public static int ExecutarSql(string sql)
        {
            var retorno = 0;
            using (var db = new Factory(dataSource, catalog, pass, user))
                retorno = db.ExecuteSql(sql);
            return retorno;
        } 

        #endregion

    }
}