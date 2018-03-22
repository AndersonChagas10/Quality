using System.Data.SqlClient;
using System.Linq;
using Dominio.YTOARA;
using Dapper;
using System.Collections.Generic;


/// <summary>
/// Contexto de dados estruturado para Ytoara
/// 
/// Disponibilizar a Estrutura, Elementos com a 
/// suas ligações do pai e seus filhos. 
/// </summary>
namespace SGQDBContextYTOARA
{
    public class SGQDBContext_YTOARA
    {
        string conexao;
        SqlConnection db;
        string sql;

        public string label;

        public Estrutura estrutura;
        public Elemento elemento;

        public SGQDBContext_YTOARA()
        {
            conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            db = new SqlConnection(conexao);
            estrutura = new Estrutura();
            elemento = new Elemento();
        }

        /// <summary>
        /// Obter todos os itens da estrutura 
        /// </summary>
        /// <returns></returns>
        public List<Estrutura> getAllEstrutura()
        {
            sql = "SELECT * FROM ESTRUTURA; ";

            return db.Query<Estrutura>(sql).ToList();
        }

        /// <summary>
        /// Obter a estrutura por parametro
        /// </summary>
        /// <param name="parametro"></param>
        /// <param name="valor"></param>
        /// <returns></returns>
        public List<Estrutura> getEstruturaByParam(string parametro, string valor)
        {
            sql = "SELECT * FROM ESTRUTURA WHERE " + parametro + " " + valor;

            return db.Query<Estrutura>(sql).ToList();
        }

        /// <summary>
        /// Obter todos os elementos
        /// </summary>
        /// <returns></returns>
        public List<Elemento> getALLElemento()
        {
            sql = "SELECT S.* FROM ELEMENTO E " +
                  "INNER JOIN ELEMENTO S ON S.ID_ESTRUTURA = E.ID_ESTRUTURA;";

            return db.Query<Elemento>(sql).ToList();
        }

        /// <summary>
        /// Obter os elementos por parametro
        /// </summary>
        /// <param name="parametro"></param>
        /// <param name="valor"></param>
        /// <returns></returns>
        public List<Elemento> getElementoByParam(string parametro, string valor)
        {
            sql = "SELECT S.* FROM ELEMENTO E " +
                  "INNER JOIN ELEMENTO S ON S.ID_ESTRUTURA = E.ID_ESTRUTURA" +
                  "WHERE " + parametro + " " + valor;

            return db.Query<Elemento>(sql).ToList();
        }
        
        /// <summary>
        /// Obter dados da estrutura Ytoara vinculado em lista unica.
        /// </summary>
        /// <returns></returns>
        public List<EstruturaVinculada> getElementoEstruturado()
        {
            sql = "SELECT * FROM ESTRUTURA S " +
                  "INNER JOIN ELEMENTO E ON E.ID_ESTRUTURA = S.ID_ESTRUTURA;";

            return db.Query<EstruturaVinculada>(sql).ToList();
        }

        /// <summary>
        /// Criar tela com o cabeçalhos Ytoara
        /// </summary>
        /// <param name="Itens"></param>
        /// <returns></returns>
        public string criarHeader(List<EstruturaVinculada>Itens)
        {
            string abredropdowlist = "<select class=\"form-control\">";
            string fechadropdowlist = "</select>";

            string label = "<label class=\"font-small\">OBRA</label><br>";

            string tela = "<div class=\"row\">"+label;

            for (int i=0; i < Itens.Count; i++)
            {
                if(Itens[i].idpai.ToString() == null)
                {
                    tela += "<div class=\"col-md-3\"><label>Local<label>"
                            + abredropdowlist
                            + "<option selected=\"selected\" value=\"" + Itens[i].idestrurua + "\" IdPaiValue=\"" + Itens[i].idpai + "\">" + Itens[i].elemento + "</option>"
                            + fechadropdowlist
                            +"</div>";
                }
                else if (Itens[i].idpai == 1)
                {
                    tela += "<div class=\"col-md-3\"><label>Item 1<label>"
                            + abredropdowlist
                            + "<option selected=\"selected\" value=\"" + Itens[i].idestrurua + "\" IdPaiValue=\"" + Itens[i].idpai + "\">" + Itens[i].elemento + "</option>"
                            + fechadropdowlist
                            +"</div>";
                }
                else if (Itens[i].idpai == 2)
                {
                    tela += "<div class=\"col-md-3\"><label>Item 2<label>"
                            + abredropdowlist
                            + "<option selected=\"selected\" value=\"" + Itens[i].idestrurua + "\" IdPaiValue=\"" + Itens[i].idpai + "\">" + Itens[i].elemento + "</option>"
                            + fechadropdowlist
                            +"</div>";
                }
                else if ((Itens[i].idpai == 4)&& (Itens[i].idpai == 5)&& (Itens[i].idpai == 6))
                {
                    tela += "<div class=\"col-md-3\"><label>Item 3<label>"
                            + abredropdowlist
                            + "<option selected=\"selected\" value=\"" + Itens[i].idestrurua + "\" IdPaiValue=\"" + Itens[i].idpai + "\">" + Itens[i].elemento + "</option>"
                            + fechadropdowlist
                            +"</div>";
                }
                else if ((Itens[i].idpai == 8) && (Itens[i].idpai == 9))
                {
                    tela += "<div class=\"col-md-3\"><label>Item 4<label>"
                            + abredropdowlist
                            + "<option selected=\"selected\" value=\"" + Itens[i].idestrurua + "\" IdPaiValue=\"" + Itens[i].idpai + "\">" + Itens[i].elemento + "</option>"
                            + fechadropdowlist
                            +"</div>";
                }
                else
                {
                    break;
                }
            }

            tela += "</div>";

            return tela;
        }

    }
}