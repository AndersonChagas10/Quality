using ADOFactory;
using Dominio;
using Newtonsoft.Json.Linq;
using SgqSystem.Controllers.Api.Recravacao;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace SgqSystem.Controllers.Api
{

    public class RecravacaoApiController : BaseApiController
    {
        private SgqDbDevEntities db;
        private RepoRecravacao<RecravacaoJson> repo;
        private List<string> errors;
        private string mensagemSucesso;

        public RecravacaoApiController()
        {
            db = new SgqDbDevEntities();
            errors = new List<string>();
            repo = new RepoRecravacao<RecravacaoJson>();
            mensagemSucesso = string.Empty;
        }

        // GET: api/RecravacaoApi
        public HttpResponseMessage Get(int companyId, int level1Id, int linhaId)
        {
            using (Factory factory = new Factory("DefaultConnection"))
            {
                var requestResults = Request.Content.ReadAsStringAsync().Result;
                var paramsFromRequest = ToDynamic(Request.Content.ReadAsStringAsync().Result);
                var query = string.Format("SELECT TOP 1* FROM RecravacaoJson WHERE ParCompany_Id = {0} AND ParLevel1_Id = {1} AND SalvoParaInserirNovaColeta IS NULL AND Linha_Id = {2} AND ISACTIVE = 1 ORDER BY Id DESC", companyId, level1Id, linhaId);
                var results = QueryNinja(db, query);
                var latasId = results.Count() > 0 ? QueryNinja(db, string.Format("SELECT Id from RecravacaoLataJson where RecravacaoJson_Id = {0}", results[0].GetValue("Id").ToString())) : null;
                var produtos = factory.SearchQuery<ReprocessoApiController.Produto>("SELECT * FROM Produto").ToList();
                var sugestoes = factory.SearchQuery<DTO.DTO.RecravacaoSugestaoDTO>("SELECT * FROM RecravacaoSugestao").ToList();

                if (results.Count() > 0)
                {
                    var queryImagensTipoLataPorparRecravacao_TypeLata_Id = "select * from ParLataImagens where ParRecravacao_TipoLata_Id = (select top 1 ParRecravacao_TypeLata_Id from ParRecravacao_Linhas where id = (select top 1 Linha_Id from RecravacaoJson where id = {0}))";
                    var listImages = QueryNinja(db, string.Format(queryImagensTipoLataPorparRecravacao_TypeLata_Id, results[0].GetValue("Id").ToString())).ToList();
                    results[0]["TipoDeLataImagens"] = JToken.FromObject(listImages, new Newtonsoft.Json.JsonSerializer { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore });
                }

                return Request.CreateResponse(HttpStatusCode.OK,
                    new { resposta = "Dados Recuperados", model = results, produtos = produtos, sugestoes = sugestoes, latasId = latasId });
            }
        }

        public HttpResponseMessage Get(int recravacaoLataJsonId)
        {
            var query = string.Format(@"SELECT * FROM RecravacaoLataJson WHERE Id = {0}", recravacaoLataJsonId);
            var listRecravacaoJson = QueryNinja(db, query).FirstOrDefault();

            return Request.CreateResponse(HttpStatusCode.OK, new { resposta = "Busca", model = listRecravacaoJson });
        }

        // POST: api/RecravacaoApi
        public HttpResponseMessage Post(dynamic data)
        {
            ////Teste de erros não controlados
            //throw new Exception("teste", new Exception("INNER", new Exception("Inner 2")));
            var model = string.Empty;

            try
            {
                ////Teste de erros controlados
                //throw new Exception("teste", new Exception("INNER", new Exception("Inner 2")));
                //model = Request.Content.ReadAsStringAsync().Result;
                //System.Web.Script.Serialization.JavaScriptSerializer json_serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                //dynamic dados = json_serializer.DeserializeObject(model);
                var linha = data["linha"];
                var latas = data["latas"];

                var linhaStringFormatada = ToJson(linha);
                int idLinha = int.Parse(linha["Id"].ToString());
                int idCompany = int.Parse(linha["ParCompany_Id"].ToString());
                int parLevel1_Id = int.Parse(linha["ParLevel1_Id"].ToString());
                bool salvoParaInserirNovaColeta = false;
                bool isValidated = false;
                var existente = db.RecravacaoJson.FirstOrDefault(r => r.ParCompany_Id == idCompany && r.Linha_Id == idLinha && !isValidated && r.SalvoParaInserirNovaColeta == null)?.Id;

                int RecravacaoJsonId = 0;

                int? userFinished_Id = Convert.ToInt32(linha["UserFinished_Id"]?.ToString());
                if (userFinished_Id == 0) userFinished_Id = null;
                int? userValidated_Id = Convert.ToInt32(linha["UserValidated_Id"]?.ToString());
                if (userValidated_Id == 0) userValidated_Id = null;

                if (IsPropertyExist(linha, "isValidated"))
                    isValidated = linha["isValidated"];

                if (IsPropertyExist(linha, "SalvoParaInserirColeta"))
                    salvoParaInserirNovaColeta = linha["SalvoParaInserirColeta"];

                if (existente.GetValueOrDefault() > 0 && salvoParaInserirNovaColeta == false)
                    RecravacaoJsonId = Update(linhaStringFormatada, existente, userFinished_Id, userValidated_Id);
                else
                {
                    if (existente.GetValueOrDefault() > 0 && salvoParaInserirNovaColeta == true)
                        RecravacaoJsonId = UpdateRecravacaoJsonParaNovaColeta(linhaStringFormatada, existente.GetValueOrDefault());
                    else
                        RecravacaoJsonId = Save(linhaStringFormatada, idLinha, idCompany, parLevel1_Id, userFinished_Id, userValidated_Id);
                }

                if (isValidated)
                {
                    RecravacaoJson recravacaoJsonValidada;
                    RecravacaoJsonId = UpdateRecravacaoJsonFinalizaColetaValidada(linhaStringFormatada, existente.GetValueOrDefault(), userFinished_Id, userValidated_Id, out recravacaoJsonValidada);

                    var listaCollectionJson = MapeiaRecravacaoJsonParaCollectionJson(linha, latas);
                    using (Factory factory = new Factory("DefaultConnection"))
                    {
                        foreach (string collectionJson in listaCollectionJson)
                        {
                            new SgqSystem.Services.SyncServices().InsertJson(collectionJson, "1", "1", false);
                        }
                    }
                }
                //Post Save
                mensagemSucesso = "Registro atualizado";

                SaveLatas(RecravacaoJsonId, latas);

            }
            catch (Exception e)
            {
                errors.Add("Não foi possível Salvas os Dados: " + e.Message);
            }

            if (errors.Count() > 0)
                return Request.CreateResponse(HttpStatusCode.OK, new { errors });

            //return Request.CreateResponse(HttpStatusCode.OK, new { resposta = mensagemSucesso, model = Request.Content.ReadAsStringAsync().Result });
            return Request.CreateResponse(HttpStatusCode.OK, new { resposta = mensagemSucesso });

        }

        private int UpdateRecravacaoJsonFinalizaColetaValidada(string linhaStringFormatada, int? existente, int? userFinished_Id, int? userValidated_Id, out RecravacaoJson recravacaoJsonValidada)
        {
            recravacaoJsonValidada = db.RecravacaoJson.FirstOrDefault(r => r.Id == existente);
            recravacaoJsonValidada.ObjectRecravacaoJson = linhaStringFormatada;
            recravacaoJsonValidada.AlterDate = DateTime.Now;
            recravacaoJsonValidada.UserFinished_Id = userFinished_Id;
            recravacaoJsonValidada.UserValidated_Id = userValidated_Id;
            recravacaoJsonValidada.isValidated = true;
            return repo.Save(recravacaoJsonValidada).Id;
        }

        private int UpdateRecravacaoJsonParaNovaColeta(string linhaStringFormatada, int? existente)
        {
            var updateRecravacaoJsonNovaColeta = db.RecravacaoJson.FirstOrDefault(r => r.Id == existente);
            updateRecravacaoJsonNovaColeta.ObjectRecravacaoJson = linhaStringFormatada;
            updateRecravacaoJsonNovaColeta.UserFinished_Id = null;
            updateRecravacaoJsonNovaColeta.UserValidated_Id = null;
            updateRecravacaoJsonNovaColeta.AlterDate = DateTime.Now;
            updateRecravacaoJsonNovaColeta.SalvoParaInserirNovaColeta = existente;
            return repo.Save(updateRecravacaoJsonNovaColeta).Id;
        }

        private int Save(string linhaStringFormatada, int idLinha, int idCompany, int parLevel1_Id, int? userFinished_Id, int? userValidated_Id)
        {
            var newRecravacaoColeta = new RecravacaoJson()
            {
                AddDate = DateTime.Now,
                IsActive = true,
                UserSgqId = 1,
                UserFinished_Id = userFinished_Id,
                UserValidated_Id = userValidated_Id,
                ParCompany_Id = idCompany,
                Linha_Id = idLinha,
                ParLevel1_Id = parLevel1_Id,
                ObjectRecravacaoJson = linhaStringFormatada
            };
            return repo.Save(newRecravacaoColeta).Id;
        }

        private int Update(string linhaStringFormatada, int? existente, int? userFinished_Id, int? userValidated_Id)
        {
            var updateRecravacaoJson = db.RecravacaoJson.FirstOrDefault(r => r.Id == existente);
            updateRecravacaoJson.ObjectRecravacaoJson = linhaStringFormatada;
            updateRecravacaoJson.UserFinished_Id = userFinished_Id;
            updateRecravacaoJson.UserValidated_Id = userValidated_Id;
            updateRecravacaoJson.AlterDate = DateTime.Now;
            return repo.Save(updateRecravacaoJson).Id;
        }

        private void SaveLatas(int? RecravacaoJson_Id, dynamic latas)
        {
            using (Factory factory = new Factory("DefaultConnection"))
            {
                factory.ExecuteSql(string.Format("DELETE FROM RecravacaoLataJson WHERE RecravacaoJson_Id = {0};", RecravacaoJson_Id));
                foreach (dynamic lata in latas)
                {
                    var lataStringFormatada = ToJson(lata);
                    factory.ExecuteSql(
                        string.Format("INSERT INTO RecravacaoLataJson (RecravacaoJson_Id, AddDate, ObjectRecravacaoJson) VALUES ({0}, GETDATE(), '{1}')", RecravacaoJson_Id, lataStringFormatada));
                }
            }
        }

        private void UpdateRecravacaoLataJson(int RecravacaoJson_Id, string LataJson)
        {
            using (Factory factory = new Factory("DefaultConnection"))
            {
                factory.ExecuteSql(
                string.Format("UPDATE RecravacaoLataJson SET AlterDate = GETDATE(), ObjectRecravacaoJson = '{1}' WHERE RecravacaoJson_Id = {0}", RecravacaoJson_Id, LataJson));
            }
        }

        private bool IsPropertyExist(dynamic obj, string name)
        {
            try
            {
                //return ((IDictionary<string, object>)obj).ContainsKey(name);
                return !(obj[name] == null);
            }
            catch (Exception)
            {
                return false;
            }
            //if (obj is ExpandoObject)
            //    return ((IDictionary<string, object>)obj).ContainsKey(name);

            //return obj.GetType().GetProperty(name) != null;
        }

        //[Angular5]
        private List<string> MapeiaRecravacaoJsonParaCollectionJson(dynamic linha, dynamic latas)
        {
            int amostraAtual = 0;
            List<string> listaCollectionJson = new List<string>();
            foreach (dynamic lata in latas)
            {
                foreach (dynamic ContadorDeAmostras in lata["ResultValue"])
                {
                    DateTime dataColetaFormatada = DateTime.MinValue;
                    DateTime.TryParseExact(Convert.ToString(linha["AddDate"]), "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dataColetaFormatada);

                    string collectionJson = "<level02>";
                    collectionJson += "{Processo}" + "|" + linha["ParLevel1_Id"]; //[0]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += dataColetaFormatada.ToString("MM/dd/yyyy HH:mm:ss"); //[1]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "{Processo}" + "|" + linha["ParLevel2_Id"]; //[2]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += dataColetaFormatada.ToString("MM/dd/yyyy HH:mm:ss"); //[3]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += linha["ParCompany_Id"]; //[4]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "1"; //[5]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "1"; //[6]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "1"; //[7]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "0"; //[8]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "false"; //[9]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += dataColetaFormatada.ToString("MM/dd/yyyy HH:mm:ss"); //[10]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "1"; //[11]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "1"; //[12]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += ""; //[13]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "false"; //[14]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "false"; //[15]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += ""; //[16]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "undefined"; //[17]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "undefined"; //[18]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "false"; //[19]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "recravacao"; //[20]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "jbs"; //[21]
                    collectionJson += ";"; //SEPARADOR LEVEL2

                    // Início do Level03
                    foreach (dynamic Tarefa in lata["ResultValue"])
                    {
                        string tarefa_id = Convert.ToString(Tarefa);
                        int primeiraAspas = tarefa_id.IndexOf("\"");
                        int segundaAspas = tarefa_id.Substring(primeiraAspas + 1).IndexOf("\"");
                        tarefa_id = tarefa_id.Substring(primeiraAspas + 1, segundaAspas - primeiraAspas);
                        int contador = 0;
                        string valor = "";
                        foreach (dynamic valorTarefa in Tarefa)
                        {
                            valor = valorTarefa[(amostraAtual + 1).ToString()];
                            if (contador == amostraAtual)
                            {
                                continue;
                            }
                            ++contador;
                        }
                        collectionJson += "<level03>";
                        collectionJson += tarefa_id; //level3 1
                        collectionJson += ","; //SEPARADOR LEVEL3
                        collectionJson += dataColetaFormatada.ToString("MM/dd/yyyy HH:mm:ss");//collectionDate; //level3 2
                        collectionJson += ","; //SEPARADOR LEVEL3
                        collectionJson += "0"; //level3 2???
                        collectionJson += ","; //SEPARADOR LEVEL3
                        collectionJson += "true"; //level3 3
                        collectionJson += ","; //SEPARADOR LEVEL3
                        collectionJson += "1"; //level3 6
                        collectionJson += ","; //SEPARADOR LEVEL3
                        collectionJson += "null"; //level3 7
                        collectionJson += ","; //SEPARADOR LEVEL3
                        collectionJson += "undefined"; //level3 8
                        collectionJson += ","; //SEPARADOR LEVEL3
                        collectionJson += "undefined"; //level3 9
                        collectionJson += ","; //SEPARADOR LEVEL3
                        collectionJson += "0"; //level3 10
                        collectionJson += ","; //SEPARADOR LEVEL3
                        collectionJson += ""; //level3 11
                        collectionJson += ","; //SEPARADOR LEVEL3
                        collectionJson += "0"; //level3 12
                        collectionJson += ","; //SEPARADOR LEVEL3
                        collectionJson += "0"; //level3 13
                        collectionJson += ","; //SEPARADOR LEVEL3
                        collectionJson += "0"; //level3 14
                        collectionJson += ","; //SEPARADOR LEVEL3
                        collectionJson += "{Avaliacao}";//av; //level3 15
                        collectionJson += ","; //SEPARADOR LEVEL3
                        collectionJson += "{Nao Conformidade}";//NC; //level3 16
                        collectionJson += ","; //SEPARADOR LEVEL3
                        collectionJson += "0"; //level3 17
                        collectionJson += ","; //SEPARADOR LEVEL3
                        collectionJson += "0"; //level3 17??
                        collectionJson += ","; //SEPARADOR LEVEL3
                        collectionJson += "0"; //level3 17??
                        collectionJson += "</level03>";

                    }

                    // Final do Level03

                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += ""; //[23]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "false"; //[24]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "false"; //[25]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "0"; //[26]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "null"; //[27]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "null"; //[28]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "false"; //[29]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "undefined"; //[30]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "false"; //[31]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "false"; //[32]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "undefined"; //[33]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "1"; //[35]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "0"; //[36]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "0"; //[37]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "1"; //[38]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "0"; //[39]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "0"; //[40]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "1"; //[41]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "0"; //[42]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "1"; //[43]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "1"; //[44]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "0"; //[45]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "0"; //[46]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "0"; //[47]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "0"; //[48]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "{Processo}"; //[49]
                    collectionJson += "</level02>";
                    listaCollectionJson.Add(collectionJson);
                }

                amostraAtual++;
            }

            return listaCollectionJson;
        }

    }


}
