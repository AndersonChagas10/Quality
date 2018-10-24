using Dominio;
using Newtonsoft.Json.Linq;
using SgqSystem.Controllers.Api.Public;
using SgqSystem.Handlres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;

namespace SgqSystem.Controllers.Api
{
    public class ExcelItens
    {
        public string Indicador { get; set; }
        public string Monitoramento { get; set; }
        public string Tarefa { get; set; }
        public string Processo { get; set; }
        public string VersaoApp { get; set; }
        public string Empresa { get; set; }
        public int Ano { get; set; }
        public int Mes { get; set; }
        public int Dia { get; set; }
        public string Iniciais { get; set; }
        public string Avaliacao { get; set; }
        public string NaoConformidade { get; set; }
    }

    [RoutePrefix("api/ImportacaoExcel")]
    [HandleApi()]
    public class ImportacaoExcelApiController : ApiController
    {

        public SgqDbDevEntities db = new SgqDbDevEntities();

        [HttpPost]
        [Route("SalvarExcel")]
        public IHttpActionResult SalvarExcel([FromBody] List<JObject> dados, int formatoId)
        {
            //criar um passo a passo de como criar o formato do excel e upa-lo
            try
            {
                //busca os  itens do formato - ok
                var formatos = db.ImportFormatItem.Where(x => x.ImportFormat_Id == formatoId).ToList();

                //cria o dicionario para formatar o arquivo 
                Dictionary<string, string> dicionarioItem = new Dictionary<string, string>();
                foreach (var item in formatos)
                {
                    dicionarioItem.Add(item.Key, item.Value);
                }

                var divjeto = "";
                //metodo para montar o divjeto
                divjeto = MontaDivJeto(dados);

                //var dicionario = new Dictionary<string, string>();

                //Metodo para criar um dicionario padrão
                var dicionarioPadrao = CriaDicionarioPadrao();             

                //metodo para formatar o divjeto, e preencher com os dados padroes 
                var divjetoPreenchido = GetUrlDivjeto(divjeto, dicionarioItem, dicionarioPadrao);

                //metodo para atribuir os valores do arquivo conforme o formato escolhido
                List<string> error = new List<string>();
                var divjetoFinal = GetUrlDivjetoFinal(divjetoPreenchido, dicionarioItem, dados, ref error);
                if (error.Count > 0)
                {
                    return BadRequest(string.Join("",error.ToArray()));
                }
                // CriarCollectJson(string parlevel1_id, string parlevel2_id, string parlevel3_id, string cluster_id, string versionApp, string empresa, int ano, int mes, int dia, string initials, string av, string NC)
                ExcelItens itensExcel = new ExcelItens();
                PublicApiController publicController = new PublicApiController();
                foreach (var item in divjetoFinal)
                {
                    var collectionJson = item.Split('/');                   

                    itensExcel.Indicador = collectionJson[0].ToString();
                    itensExcel.Monitoramento = collectionJson[1].ToString();
                    itensExcel.Tarefa = collectionJson[2].ToString();
                    itensExcel.Processo = collectionJson[3].ToString();
                    itensExcel.VersaoApp = collectionJson[4].ToString();
                    itensExcel.Empresa = collectionJson[5].ToString();
                    itensExcel.Ano = Convert.ToInt16(collectionJson[6]);
                    itensExcel.Mes = Convert.ToInt16(collectionJson[7]);
                    itensExcel.Dia = Convert.ToInt16(collectionJson[8]);
                    itensExcel.Iniciais = collectionJson[9].ToString();
                    itensExcel.Avaliacao = Math.Round(Convert.ToDecimal(collectionJson[10])).ToString();
                    itensExcel.NaoConformidade = Math.Round(Convert.ToDecimal(collectionJson[11])).ToString();
                   

                    //public void CriarCollectJson   (string parlevel1_id,  string parlevel2_id,      string parlevel3_id, string cluster_id, string versionApp,     string empresa,      int ano,         int mes,         int dia,        string initials,       string av,             string NC)
                    publicController.CriarCollectJson(itensExcel.Indicador, itensExcel.Monitoramento, itensExcel.Tarefa, itensExcel.Processo, itensExcel.VersaoApp, itensExcel.Empresa, itensExcel.Ano, itensExcel.Mes, itensExcel.Dia, itensExcel.Iniciais, itensExcel.Avaliacao, itensExcel.NaoConformidade);
                }              
                return Ok("Processado com sucesso");
            }
            catch (Exception e)
            {
                return BadRequest(String.Join("-", e));
            }
        }

        public static Dictionary<string, string> CriaDicionarioPadrao()
        {
            var data = DateTime.Now;
            Dictionary<string, string> dicionarioPadrao = new Dictionary<string, string>();
            
            dicionarioPadrao.Add("Indicador", "42");
            dicionarioPadrao.Add("Monitoramento", "306");
            dicionarioPadrao.Add("Tarefa", "1142");
            dicionarioPadrao.Add("Processo", "3");
            dicionarioPadrao.Add("Empresa", "JBS");        
            dicionarioPadrao.Add("Ano", data.Year.ToString());
            dicionarioPadrao.Add("Mes", data.Month.ToString());
            dicionarioPadrao.Add("Dia", data.Day.ToString());
            dicionarioPadrao.Add("Sigla da Unidade", "11");
            dicionarioPadrao.Add("Avaliacao", "17");
            dicionarioPadrao.Add("Nao Conformidade", "258");

            return dicionarioPadrao;
        }

        private string MontaDivJeto(List<JObject> dados)
        {          
            string json = "{Indicador}" + "/";
            json += "{Monitoramento}" + "/";
            json += "{Tarefa}" + "/";
            json += "{Processo}" + "/";
            json += "Excel" + "/";
            json += "JBS" + "/";
            json += "{Ano}" + "/";
            json += "{Mes}" + "/";
            json += "{Dia}" + "/";
            json += "{Sigla da Unidade}" + "/";
            json += "{Avaliacao}" + "/";
            json += "{Nao Conformidade}";
           
            return json;
        }

        private List<string> GetUrlDivjetoFinal(string divjetoPreenchido, Dictionary<string, string> dicionarioItem, List<JObject> dados, ref List<string> error)
        {
            var listaDivJeto = new List<string>();

            for (int i = 0; i < dados.Count; i++)
            {
                var item = dados[i];
                string expRegex = "{.*?}";
                Match m = Regex.Match(divjetoPreenchido, expRegex);
                string divjeto = divjetoPreenchido;
                string camposErrados = "";
                while (m.Success)
                {
                    var value = m.Value.Replace("{", "").Replace("}", "");

                    if (item[value] != null)
                    {
                        var valor = item[value].ToString();

                        if (dicionarioItem.FirstOrDefault(x => x.Key.Equals("Data da Coleta") || x.Key.Equals("Horario de Inicio")).Value == m.Value)
                        {
                            DateTime dataFormatada;
                            DateTime.TryParseExact(valor, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dataFormatada);
                            if (dataFormatada > DateTime.MinValue)
                                valor = dataFormatada.ToString("MM/dd/yyyy HH:mm:ss");
                        }

                        divjeto = divjeto.Replace(m.Value, valor);
                    }
                    else 
                    {
                        var mensagemErro = "O campo obrigatorio " + m.Value + " precisa ser preenchido <br />";
                        if(!error.Contains(mensagemErro))
                            error.Add(mensagemErro);
                    }
                    m = m.NextMatch();
                }
                listaDivJeto.Add(divjeto);
            }
            return listaDivJeto;
        }

        public string GetUrlDivjeto(string divjeto, Dictionary<string, string> dicionarioItem, Dictionary<string, string> dicionarioPadrao)
        {
            string expRegex = "{.*?}";

            Match m = Regex.Match(divjeto, expRegex);
            while (m.Success)
            {
                var value = m.Value.Replace("{", "").Replace("}", "");

                if (dicionarioItem.Any(i => i.Key == value))
                {
                    divjeto = divjeto.Replace(m.Value, dicionarioItem[value]);
                }
                else if (dicionarioPadrao.Any(i => i.Key == value))
                {
                    divjeto = divjeto.Replace(m.Value, dicionarioPadrao[value]);
                }
                m = m.NextMatch();
            }
            return divjeto;
        }

        public DateTime? convertStringToDatetimeNull(string date, string format)
        {
            if (string.IsNullOrEmpty(date))
            {
                return null;
            }

            if (date.Length == 10)
            {
                date += " 00:00:00";
            }
            DateTime datetime = DateTime.MinValue;

            if (DateTime.TryParseExact(
                    date,
                    format,
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None,
                    out datetime) || DateTime.TryParseExact(
                    date,
                    format.Replace("/", "."),
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None,
                    out datetime))
            {
                if (datetime > DateTime.MinValue)
                {
                    return datetime;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
