using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace PlanoAcaoCore
{
    public class Pa_CausaEspecifica : Pa_BaseObject, ICrudPa<Pa_CausaEspecifica>
    {
        public string Text { get; set; }

        public static List<Pa_CausaEspecifica> Listar()
        {
            var query = "SELECT * FROM Pa_CausaEspecifica";
            return ListarGenerico<Pa_CausaEspecifica>(query);
        }

        public static Pa_CausaEspecifica Get(int Id)
        {
            var query = "SELECT * FROM Pa_CausaGenerica WHERE Id = " + Id;
            return GetGenerico<Pa_CausaEspecifica>(query);
        }

        public void IsValid()
        {
            //throw new NotImplementedException();
        }

        public void AddOrUpdate()
        {
            IsValid();
            string query;
            if (Id > 0)
            {


            }
            else
            {

                query = "INSERT INTO [dbo].[Pa_CausaEspecifica]" +
              "\n       ([Text])                       " +
              "\n  VALUES                                         " +
              "\n        (@Text)SELECT CAST(scope_identity() AS int)";

                SqlCommand cmd;
                cmd = new SqlCommand(query);

                cmd.Parameters.AddWithValue("@Text", Text);

                Id = Salvar(cmd);

            }
        }
    }
}
