﻿using ADOFactory;
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
            using (var db = new Factory(Conn.dataSource, Conn.catalog, Conn.pass, Conn.user))
                return db.InsertUpdateData(cmd);
        }

        protected int Salvar(SqlCommand cmd)
        {
            using (var db = new Factory(Conn.dataSource, Conn.catalog, Conn.pass, Conn.user))
                return db.InsertUpdateData(cmd);

        }

        protected static int SalvarStatic(SqlCommand cmd)
        {
            using (var db = new Factory(Conn.dataSource, Conn.catalog, Conn.pass, Conn.user))
                return db.InsertUpdateData(cmd);

        }

        public static T SalvarGenerico<T>(T obj)
        {
            using (var db = new Factory(Conn.dataSource, Conn.catalog, Conn.pass, Conn.user))
                return db.InsertUpdateData<T>(obj);

        }

        public static List<T> ListarGenerico<T>(string query)
        {
            List<T> listReturn;
            using (var db = new Factory(Conn.dataSource, Conn.catalog, Conn.pass, Conn.user))
                listReturn = db.SearchQuery<T>(query);
            return listReturn;
        }

        public static T GetGenerico<T>(string query)
        {
            T objReturn;
            using (var db = new Factory(Conn.dataSource, Conn.catalog, Conn.pass, Conn.user))
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
            using (var db = new Factory(Conn.dataSource, Conn.catalog, Conn.pass, Conn.user))
                retorno = db.ExecuteSql(sql);
            return retorno;
        } 

        #endregion

    }
}