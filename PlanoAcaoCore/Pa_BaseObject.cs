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

            query = $@"INSERT INTO [dbo].[{ table }] 
                          ([Name], IsActive)                  
                    VALUES                          
                          (@Name, 1)                    
                          SELECT CAST(scope_identity() AS int) ";

            SqlCommand cmd;
            cmd = new SqlCommand(query);
            cmd.Parameters.AddWithValue("@Name", valor);

            return SalvarStatic(cmd);
        }

        public static int GenericUpdate(string valor, string table, bool isActive, int id)
        {
            string query;
            SqlCommand cmd;

            query = $@"UPDATE [dbo].[{ table }]
                    SET [Name] = @Name, [IsActive] = @IsActive
                    WHERE Id = @Id
                    SELECT @Id";

            cmd = new SqlCommand(query);
            cmd.Parameters.AddWithValue("@Name", valor);
            cmd.Parameters.AddWithValue("@IsActive", isActive);
            cmd.Parameters.AddWithValue("@Id", id);

            return SalvarStatic(cmd);
        }

        public static int GetIdGenerico(string valor, string table)
        {
            string query;

            query = $@"SELECT TOP 1 [Id] FROM [dbo].[{ table }] 
                    WHERE [Name] = '{valor}'";

            return GetGenerico<Generico>(query)?.Id ?? 0;
        }

        public static int GenericInsert(string valor, string table, int predecessor, string fk, bool? isPriority = null)
        {
            string query;

            var IsPriority = "";
            var IsPriority2 = "";

            if (isPriority != null)
            {
                IsPriority = "IsPriority,";
                IsPriority2 = "@IsPriority,";
            }

            query = $@"INSERT INTO [dbo].[{ table }] 
                          ([Name],                  
                           { fk },
                           { IsPriority }
                            IsActive)         
                    VALUES                          
                          (@Name,                   
                           @predecessor,
                           { IsPriority2 } 
                            1)             
                          SELECT CAST(scope_identity() AS int) ";

            SqlCommand cmd;
            cmd = new SqlCommand(query);
            cmd.Parameters.AddWithValue("@Name", valor);
            cmd.Parameters.AddWithValue("@predecessor", predecessor);
            cmd.Parameters.AddWithValue("@IsPriority", isPriority);

            return SalvarStatic(cmd);
        }

        public static int GenericUpdate(string valor, string table, bool isActive, int predecessor, string fk, int id, bool? isPriority = null)
        {
            string query;
            SqlCommand cmd;

            var IsPriority = "";

            if (isPriority != null)
                IsPriority = ", [IsPriority] = @IsPriority";


            query = $@"UPDATE [dbo].[{ table }]
                    SET [Name] = @Name, [{fk}] = @predecessor, [IsActive] = @IsActive { IsPriority }
                    WHERE Id = @Id
                    SELECT @Id";

            cmd = new SqlCommand(query);
            cmd.Parameters.AddWithValue("@Name", valor);
            cmd.Parameters.AddWithValue("@predecessor", predecessor);
            cmd.Parameters.AddWithValue("@IsActive", isActive);
            cmd.Parameters.AddWithValue("@Id", id);

            if (isPriority != null)
                cmd.Parameters.AddWithValue("@IsPriority", isPriority);

            return SalvarStatic(cmd);
        }

        public static int GetIdGenerico(string valor, string table, int predecessor, string fk, bool? isPrimary = null)
        {
            string query;

            query = $@"SELECT TOP 1 [Id] FROM [dbo].[{ table }] 
                    WHERE [Name] = '{valor}' AND { fk } = {predecessor}";

            return GetGenerico<Generico>(query)?.Id ?? 0;
        }

        public static int GenericInsertIfNotExists(string valor, string table, bool? isPrimary = null)
        {
            if (!(GetIdGenerico(valor, table) > 0))
            {
                return GenericInsert(valor, table);
            }
            return 0;
        }

        public static int GenericInsertIfNotExists(string valor, string table, int predecessor, string fk, bool? isPrimary = null)
        {
            if (!(GetIdGenerico(valor, table, predecessor, fk) > 0))
            {
                return GenericInsert(valor, table, predecessor, fk, isPrimary);
            }
            return 0;
        }

        public static int GenericUpdateIfUnique(string valor, string table, bool isActive, int id)
        {
            int auxId = GetIdGenerico(valor, table);
            if (auxId == id || auxId == 0)
            {
                return GenericUpdate(valor, table, isActive, id);
            }
            return 0;
        }

        public static int GenericUpdateIfUnique(string valor, string table, bool isActive, int predecessor, string fk, int id, bool? isPrimary = null)
        {
            int auxId = GetIdGenerico(valor, table, predecessor, fk);
            if (auxId == id || auxId == 0)
            {
                return GenericUpdate(valor, table, isActive, predecessor, fk, id, isPrimary);
            }
            return 0;
        }

        public class Generico
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public bool IsActive { get; set; }
            public bool? IsPriority { get; set; }
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