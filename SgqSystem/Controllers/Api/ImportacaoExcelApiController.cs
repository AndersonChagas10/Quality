﻿using Dominio;
using Newtonsoft.Json.Linq;
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

                //montar um divjeto com os seguintes campos e enviar ao metodo CriarCollectJson
                //{parlevel1_id}/{parlevel2_id}/{parlevel3_id}/{cluster_id}/{versionApp}/{empresa}/{ano}/{mes}/{dia}/{initials}/{av}/{NC}"
                //var novoDivjetoPReenchido = NovoGetUrlDivjeto(divjeto, dicionarioItem, dicionarioPadrao);


                //metodo para formatar o divjeto, e preencher com os dados padroes 
                var divjetoPreenchido = GetUrlDivjeto(divjeto, dicionarioItem, dicionarioPadrao);

                //metodo para atribuir os valores do arquivo conforme o formato escolhido
                List<string> error = new List<string>();
                var divjetoFinal = GetUrlDivjetoFinal(divjetoPreenchido, dicionarioItem, dados, ref error);
                if (error.Count > 0)
                {
                    return BadRequest(string.Join("-", error.ToArray()));
                }

                foreach (var item in divjetoFinal)
                {

                    var collectionJson = item.Split(';');
                    var monitoramentoId = Convert.ToInt32(collectionJson[2].Contains("|") ? collectionJson[2].Split('|')[1] : collectionJson[2]);
                    int companyId = 0;
                    try
                    {
                        companyId = Convert.ToInt32(collectionJson[4]);
                    }
                    catch (Exception e)
                    {
                        var company = collectionJson[4];
                        companyId = db.ParCompany.Where(x => x.Initials.Equals(company)).FirstOrDefault().Id;
                    }

                    var weight = db.ParLevel3Level2
                        .Where(x => x.ParLevel2_Id == monitoramentoId && (x.ParCompany_Id == null || x.ParCompany_Id == companyId))
                        .OrderByDescending(x => x.ParCompany_Id)
                        .Select(x => x.Weight).FirstOrDefault();

                    var level3 = collectionJson[22].Split(',');

                    level3[8] = weight.ToString().Replace(',', '.');

                    collectionJson[22] = string.Join(",", level3);

                    collectionJson[4] = companyId.ToString();

                    string collectionJsonFinal = string.Join(";", collectionJson);

                    new SgqSystem.Services.SyncServices().InsertJson(collectionJsonFinal, "1", "1", false);
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
            dicionarioPadrao.Add("Processo", "3");
            dicionarioPadrao.Add("Indicador", "42");
            dicionarioPadrao.Add("Data da Coleta", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"));
            dicionarioPadrao.Add("Monitoramento", "306");
            dicionarioPadrao.Add("Unidade", "11");
            dicionarioPadrao.Add("Horario de Inicio", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"));
            dicionarioPadrao.Add("Versão App", "2.0.50");
            dicionarioPadrao.Add("Empresa", "JBS");
            dicionarioPadrao.Add("Tarefa", "1142");
            dicionarioPadrao.Add("Avaliacao", "17");
            dicionarioPadrao.Add("Nao Conformidade", "258");
            dicionarioPadrao.Add("Ano", data.Year.ToString());
            dicionarioPadrao.Add("Mes", data.Month.ToString());
            dicionarioPadrao.Add("Dia", data.Day.ToString());

            return dicionarioPadrao;
        }

        private string MontaDivJeto(List<JObject> dados)
        {
            //{parlevel1_id}/{parlevel2_id}/{parlevel3_id}/{cluster_id}/{versionApp}/{empresa}/{ano}/{mes}/{dia}/{initials}/{av}/{NC}"

            string json = "{Indicador}" + "/";
            json += "{Monitoramento}" + "/";
            json += "{Tarefa}" + "/";
            json += "{Processo}" + "/";
            json += "{Versão App}" + "/";
            json += "{Empresa}" + "/";
            json += "{Ano}" + "/";
            json += "{Mes}" + "/";
            json += "{Dia}" + "/";
            json += "{Unidade}" + "/";
            json += "{Avaliacao}" + "/";
            json += "{Nao Conformidade}";

            //string collectionJson = "<level02>";
            //collectionJson += "{Processo}" + "|" + "{Indicador}"; //[0]
            //collectionJson += ";"; //SEPARADOR LEVEL2
            //collectionJson += "{Data da Coleta}"; //[1]
            //collectionJson += ";"; //SEPARADOR LEVEL2
            //collectionJson += "{Processo}" + "|" + "{Monitoramento}"; //[2]
            //collectionJson += ";"; //SEPARADOR LEVEL2
            //collectionJson += "{Data da Coleta}"; //[3]
            //collectionJson += ";"; //SEPARADOR LEVEL2
            //collectionJson += "{Unidade}"; //[4]
            //collectionJson += ";"; //SEPARADOR LEVEL2
            //collectionJson += "1"; //[5]
            //collectionJson += ";"; //SEPARADOR LEVEL2
            //collectionJson += "1"; //[6]
            //collectionJson += ";"; //SEPARADOR LEVEL2
            //collectionJson += "1"; //[7]
            //collectionJson += ";"; //SEPARADOR LEVEL2
            //collectionJson += "0"; //[8]
            //collectionJson += ";"; //SEPARADOR LEVEL2
            //collectionJson += "false"; //[9]
            //collectionJson += ";"; //SEPARADOR LEVEL2
            //collectionJson += "{Horario de Inicio}"; //[10]
            //collectionJson += ";"; //SEPARADOR LEVEL2
            //collectionJson += "1"; //[11]
            //collectionJson += ";"; //SEPARADOR LEVEL2
            //collectionJson += "1"; //[12]
            //collectionJson += ";"; //SEPARADOR LEVEL2
            //collectionJson += ""; //[13]
            //collectionJson += ";"; //SEPARADOR LEVEL2
            //collectionJson += "false"; //[14]
            //collectionJson += ";"; //SEPARADOR LEVEL2
            //collectionJson += "false"; //[15]
            //collectionJson += ";"; //SEPARADOR LEVEL2
            //collectionJson += ""; //[16]
            //collectionJson += ";"; //SEPARADOR LEVEL2
            //collectionJson += "undefined"; //[17]
            //collectionJson += ";"; //SEPARADOR LEVEL2
            //collectionJson += "undefined"; //[18]
            //collectionJson += ";"; //SEPARADOR LEVEL2
            //collectionJson += "false"; //[19]
            //collectionJson += ";"; //SEPARADOR LEVEL2
            //collectionJson += "{Versão App}"; //[20]
            //collectionJson += ";"; //SEPARADOR LEVEL2
            //collectionJson += "{Empresa}"; //[21]
            //collectionJson += ";"; //SEPARADOR LEVEL2

            //// Início do Level03

            //collectionJson += "<level03>";
            //collectionJson += "{Tarefa}"; //level3 1
            //collectionJson += ","; //SEPARADOR LEVEL3
            //collectionJson += "{Data da Coleta}";//collectionDate; //level3 2
            //collectionJson += ","; //SEPARADOR LEVEL3
            //collectionJson += "0"; //level3 2???
            //collectionJson += ","; //SEPARADOR LEVEL3
            //collectionJson += "true"; //level3 3
            //collectionJson += ","; //SEPARADOR LEVEL3
            //collectionJson += "1"; //level3 6
            //collectionJson += ","; //SEPARADOR LEVEL3
            //collectionJson += "null"; //level3 7
            //collectionJson += ","; //SEPARADOR LEVEL3
            //collectionJson += "undefined"; //level3 8
            //collectionJson += ","; //SEPARADOR LEVEL3
            //collectionJson += "undefined"; //level3 9
            //collectionJson += ","; //SEPARADOR LEVEL3
            //collectionJson += "0"; //level3 10
            //collectionJson += ","; //SEPARADOR LEVEL3
            //collectionJson += ""; //level3 11
            //collectionJson += ","; //SEPARADOR LEVEL3
            //collectionJson += "0"; //level3 12
            //collectionJson += ","; //SEPARADOR LEVEL3
            //collectionJson += "0"; //level3 13
            //collectionJson += ","; //SEPARADOR LEVEL3
            //collectionJson += "0"; //level3 14
            //collectionJson += ","; //SEPARADOR LEVEL3
            //collectionJson += "{Avaliacao}";//av; //level3 15
            //collectionJson += ","; //SEPARADOR LEVEL3
            //collectionJson += "{Nao Conformidade}";//NC; //level3 16
            //collectionJson += ","; //SEPARADOR LEVEL3
            //collectionJson += "0"; //level3 17
            //collectionJson += ","; //SEPARADOR LEVEL3
            //collectionJson += "0"; //level3 17??
            //collectionJson += ","; //SEPARADOR LEVEL3
            //collectionJson += "0"; //level3 17??
            //collectionJson += "</level03>";

            //// Final do Level03

            //collectionJson += ";"; //SEPARADOR LEVEL2
            //collectionJson += ""; //[23]
            //collectionJson += ";"; //SEPARADOR LEVEL2
            //collectionJson += "false"; //[24]
            //collectionJson += ";"; //SEPARADOR LEVEL2
            //collectionJson += "false"; //[25]
            //collectionJson += ";"; //SEPARADOR LEVEL2
            //collectionJson += "0"; //[26]
            //collectionJson += ";"; //SEPARADOR LEVEL2
            //collectionJson += "null"; //[27]
            //collectionJson += ";"; //SEPARADOR LEVEL2
            //collectionJson += "null"; //[28]
            //collectionJson += ";"; //SEPARADOR LEVEL2
            //collectionJson += "false"; //[29]
            //collectionJson += ";"; //SEPARADOR LEVEL2
            //collectionJson += "undefined"; //[30]
            //collectionJson += ";"; //SEPARADOR LEVEL2
            //collectionJson += "false"; //[31]
            //collectionJson += ";"; //SEPARADOR LEVEL2
            //collectionJson += "false"; //[32]
            //collectionJson += ";"; //SEPARADOR LEVEL2
            //collectionJson += "undefined"; //[33]
            //collectionJson += ";"; //SEPARADOR LEVEL2
            //collectionJson += "1"; //[35]
            //collectionJson += ";"; //SEPARADOR LEVEL2
            //collectionJson += "0"; //[36]
            //collectionJson += ";"; //SEPARADOR LEVEL2
            //collectionJson += "0"; //[37]
            //collectionJson += ";"; //SEPARADOR LEVEL2
            //collectionJson += "1"; //[38]
            //collectionJson += ";"; //SEPARADOR LEVEL2
            //collectionJson += "0"; //[39]
            //collectionJson += ";"; //SEPARADOR LEVEL2
            //collectionJson += "0"; //[40]
            //collectionJson += ";"; //SEPARADOR LEVEL2
            //collectionJson += "1"; //[41]
            //collectionJson += ";"; //SEPARADOR LEVEL2
            //collectionJson += "0"; //[42]
            //collectionJson += ";"; //SEPARADOR LEVEL2
            //collectionJson += "1"; //[43]
            //collectionJson += ";"; //SEPARADOR LEVEL2
            //collectionJson += "1"; //[44]
            //collectionJson += ";"; //SEPARADOR LEVEL2
            //collectionJson += "0"; //[45]
            //collectionJson += ";"; //SEPARADOR LEVEL2
            //collectionJson += "0"; //[46]
            //collectionJson += ";"; //SEPARADOR LEVEL2
            //collectionJson += "0"; //[47]
            //collectionJson += ";"; //SEPARADOR LEVEL2
            //collectionJson += "0"; //[48]
            //collectionJson += ";"; //SEPARADOR LEVEL2
            //collectionJson += "{Processo}"; //[49]
            //collectionJson += "</level02>";

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
                while (m.Success)
                {
                    var value = m.Value.Replace("{", "").Replace("}", "");

                    if (item[value] != null)
                    {
                        var valor = item[value].ToString();

                        if(value == "Data da Coleta")
                        {
                            string dataString = valor.ToString();
                            DateTime data = Convert.ToDateTime(dataString);
                            string ano = data.Year.ToString();
                            string mes = data.Month.ToString();
                            string dia = data.Day.ToString();
                        }

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
                        error.Add("O campo obrigatorio " + m.Value + " precisa ser preenchido");
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

                if (value == "Ano" || value == "Mes" || value == "Dia")
                {
                    string dataString = value.ToString();
                    DateTime data = Convert.ToDateTime(dataString);
                    var ano = Convert.ToInt16(data.Year);
                    var mes = Convert.ToInt16(data.Month);
                    var dia = Convert.ToInt16(data.Day);
                }

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
