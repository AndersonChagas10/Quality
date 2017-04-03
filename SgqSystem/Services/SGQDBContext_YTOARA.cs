using System.Data.SqlClient;
using System.Linq;
using Dominio.YTOARA;
using Dapper;
using System.Collections.Generic;

namespace SgqSystem.Services
{
    public class SGQDBContext_YTOARA
    {
        string conexao;
        SqlConnection db;
        string sql;

        Estrutura estrutura;
        Elemento elemento;

        public SGQDBContext_YTOARA()
        {
            conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;

            db = new SqlConnection(conexao);
            estrutura = new Estrutura();
            elemento = new Elemento();
        }

        public List<Estrutura> getAllEstrutura()
        {
            sql = "SELECT * FROM ESTRUTURA; ";

            return db.Query<Estrutura>(sql).ToList();
        }

        public List<Estrutura> getEstruturaByParam(string parametro, string value)
        {
            sql = "SELECT * FROM ESTRUTURA WHERE " + parametro + " " + value;

            return db.Query<Estrutura>(sql).ToList();
        }

        public List<Elemento> getALLElemento()
        {
            sql = "SELECT S.* FROM ELEMENTO E " +
                  "INNER JOIN ELEMENTO S ON S.ID_ESTRUTURA = E.ID_ESTRUTURA;";

            return db.Query<Elemento>(sql).ToList();
        }

        public List<Elemento> getElementoByParam(string parametro, string value)
        {
            sql = "SELECT S.* FROM ELEMENTO E " +
                  "INNER JOIN ELEMENTO S ON S.ID_ESTRUTURA = E.ID_ESTRUTURA" +
                  "WHERE " + parametro + " " + value;

            return db.Query<Elemento>(sql).ToList();
        }

    }
}