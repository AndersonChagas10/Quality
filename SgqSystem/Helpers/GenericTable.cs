using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Dynamic;


namespace SgqSystem.Helpers
{
    public static class GenericTable
    {
        public static dynamic QueryNinja(DbContext db, string query)
        {
            db.Database.Connection.Open();
            var cmd = db.Database.Connection.CreateCommand();
            cmd.CommandText = query;
            var reader = cmd.ExecuteReader();
            List<JObject> datas = new List<JObject>();
            List<JObject> columns = new List<JObject>();
            dynamic retorno = new ExpandoObject();

            while (reader.Read())
            {
                var row = new JObject();
                for (int i = 0; i < reader.FieldCount; i++)
                    row[reader.GetName(i)] = reader[i].ToString();

                datas.Add(row);
            }

            for (int i = 0; i < reader.FieldCount; i++)
            {
                var col = new JObject();
                col["title"] = col["data"] = reader.GetName(i);
                columns.Add(col);
            }

            retorno.datas = datas;
            retorno.columns = columns;

            return retorno;
        }

    }
}