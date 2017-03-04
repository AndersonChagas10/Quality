using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace PlanoAcaoCore
{
    public class Pa_ContramedidaEspecifica : Pa_BaseObject, ICrudPa<Pa_ContramedidaEspecifica>
    {
        public string Text { get; set; }

        public static List<Pa_ContramedidaEspecifica> Listar()
        {
            var query = "SELECT * FROM Pa_ContramedidaEspecifica";
            return ListarGenerico<Pa_ContramedidaEspecifica>(query);
        }

        public static Pa_ContramedidaEspecifica Get(int Id)
        {
            var query = "SELECT * FROM Pa_ContramedidaEspecifica WHERE Id = " + Id;
            return GetGenerico<Pa_ContramedidaEspecifica>(query);
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
                query = "UPDATE [Pa_ContramedidaEspecifica]  " +
                     "\n     SET [Text] = @Text WHERE Id = @Id" +
                     "\n SELECT CAST(@Id AS int)";

                SqlCommand cmd;
                cmd = new SqlCommand(query);
                cmd.Parameters.AddWithValue("@Text", Text.TrimEnd().TrimStart());
                cmd.Parameters.AddWithValue("@Id", Id);

                Salvar(cmd);
            }
            else
            {

                query = "INSERT INTO [dbo].[Pa_ContramedidaEspecifica]" +
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
