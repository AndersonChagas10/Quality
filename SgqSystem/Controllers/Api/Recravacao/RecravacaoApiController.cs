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
using System.Threading.Tasks;
using System.Web.Http;
using static SgqSystem.Controllers.Api.SyncServiceApiController;

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

        // GET: api/RecravacaoApi
        public HttpResponseMessage Get(int id, int x)
        {
            using (Factory factory = new Factory("DefaultConnection"))
            {
                var requestResults = Request.Content.ReadAsStringAsync().Result;
                var paramsFromRequest = ToDynamic(Request.Content.ReadAsStringAsync().Result);
                var query = string.Format("SELECT * FROM RecravacaoJson WHERE Id = {0}", id);
                var results = QueryNinja(db, query);
                var latasId = results.Count() > 0 ? QueryNinja(db, string.Format("SELECT Id from RecravacaoLataJson where RecravacaoJson_Id = {0}", id)) : null;
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

            Task.Delay(new Random().Next(1, 500)).Wait(); 
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

                int RecravacaoJsonId = 0;

                int? userFinished_Id = Convert.ToInt32(linha["UserFinished_Id"]?.ToString());
                if (userFinished_Id == 0) userFinished_Id = null;
                int? userValidated_Id = Convert.ToInt32(linha["UserValidated_Id"]?.ToString());
                if (userValidated_Id == 0) userValidated_Id = null;

                if (IsPropertyExist(linha, "isValidated"))
                    isValidated = linha["isValidated"];

                if (IsPropertyExist(linha, "SalvoParaInserirColeta"))
                    salvoParaInserirNovaColeta = linha["SalvoParaInserirColeta"];

                //SE TIVER ID DE RECRAVAÇÃO QUER DIZER QUE É RETROATIVO E DEVE SER CONSIDERADO PARA QUALQUER AÇÃO
                int? existente = 0;
                if (data["recravacaoJsonId"] != null)
                {
                    existente = (int)data["recravacaoJsonId"];
                }
                else
                {
                    existente = db.RecravacaoJson.Where(r => r.ParCompany_Id == idCompany
                    && r.Linha_Id == idLinha && (r.isValidated != true || isValidated && r.isValidated == true) && r.SalvoParaInserirNovaColeta == null)
                    .OrderByDescending(x => x.Id).FirstOrDefault()?.Id;
                }

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
                            new SyncServiceApiController().InsertJson(new InsertJsonClass() {
                                ObjResultJSon = collectionJson,
                                deviceId = "1",
                                deviceMac = "1",
                                autoSend = false
                            });
                            //após realizado a inserção efetuar a reconsolidação por level3
                            //1004
                        }
                    }
                }
                //Post Save
                mensagemSucesso = "Registro atualizado";

                if (!salvoParaInserirNovaColeta)
                    SaveLatas(RecravacaoJsonId, latas);

            }
            catch (Exception e)
            {
                errors.Add("Não foi possível Salvar os Dados: " + e.Message);
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

        private int SaveLatas(int? RecravacaoJson_Id, dynamic latas)
        {
            var retorno = 0;
            using (Factory factory = new Factory("DefaultConnection"))
            {
                factory.ExecuteSql(string.Format("DELETE FROM RecravacaoLataJson WHERE RecravacaoJson_Id = {0};", RecravacaoJson_Id));
                foreach (dynamic lata in latas)
                {
                    var lataStringFormatada = ToJson(lata);
                    retorno = factory.ExecuteSql(
                        string.Format("INSERT INTO RecravacaoLataJson (RecravacaoJson_Id, AddDate, ObjectRecravacaoJson) VALUES ({0}, GETDATE(), '{1}')", RecravacaoJson_Id, lataStringFormatada));
                }
            }
            return retorno;
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
            int parLevel1 = Convert.ToInt32(linha["ParLevel1_Id"]);
            var dataAtual = DateTime.Now.Date;
            int avaliacaoAtual = Convert.ToInt32(db.CollectionJson
                .Where(x => x.Level01CollectionDate > dataAtual && x.level01_Id == parLevel1)
                .OrderByDescending(x => x.Evaluate)
                .Select(x => x.Evaluate)
                .FirstOrDefault()
                + 1);
            int amostraAtual = 0;
            List<string> listaCollectionJson = new List<string>();
            foreach (dynamic lata in latas)
            {
                int contadorPontos = Convert.ToInt32(linha["TipoDeLata"]["NumberOfPoints"]);

                for (int i = 0; i < contadorPontos; i++)
                {
                    DateTime dataColetaFormatada = DateTime.MinValue;
                    //DateTime.TryParseExact(Convert.ToString(linha["AddDate"]), "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dataColetaFormatada);
                    DateTime.TryParseExact(DateTime.Now.ToString(), "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dataColetaFormatada);

                    int processo = 0; //talvez inserir no web.config

                    string collectionJson = "<level02>";
                    collectionJson += processo + "|" + parLevel1; //[0]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += dataColetaFormatada.ToString("MM/dd/yyyy HH:mm:ss"); //[1]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += processo + "|" + linha["ParLevel2_Id"]; //[2]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += dataColetaFormatada.ToString("MM/dd/yyyy HH:mm:ss"); //[3]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += linha["ParCompany_Id"]; //[4]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "1"; //[5]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "1"; //[6]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += linha["UserValidated_Id"]; //[7] -----------------usuario que assinou a lata
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "0"; //[8]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "false"; //[9]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += dataColetaFormatada.ToString("MM/dd/yyyy HH:mm:ss"); //[10]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += avaliacaoAtual; //[11] --------------------identificador da lata
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += i; //[12]
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
                    if (lata["ResultValue"] != null)
                    {
                        foreach (dynamic Tarefa in lata["ResultValue"])
                        {
                            int parCompany = Convert.ToInt32(linha["ParCompany_Id"]);
                            var parLeve3Level2 = db.ParLevel3Level2.Where(t => t.ParCompany_Id == null
                            || t.ParCompany_Id == parCompany)
                                .OrderByDescending(t => t.ParCompany_Id).FirstOrDefault();
                            decimal peso = parLeve3Level2.Weight;

                            string tarefa_id = Convert.ToString(Tarefa);
                            int primeiraAspas = tarefa_id.IndexOf("\"");
                            int segundaAspas = tarefa_id.Substring(primeiraAspas + 1).IndexOf("\"");
                            tarefa_id = tarefa_id.Substring(primeiraAspas + 1, segundaAspas - primeiraAspas);
                            int contador = 0;
                            string valorDaTarefa = "";
                            string tipo = "";
                            string minimo = "", maximo = "";
                            string nome = "";

                            foreach (dynamic Parametrizacao in lata["ListParlevel3"])
                            {
                                nome = Parametrizacao["Name"];

                                if (tarefa_id == Convert.ToString(Parametrizacao["Id"]))
                                {
                                    foreach (dynamic listParlevel3 in Parametrizacao["ParLevel3Value"])
                                    {
                                        tipo = listParlevel3["ParLevel3InputType_Id"];
                                        minimo = listParlevel3["IntervalMin"];
                                        maximo = listParlevel3["IntervalMax"];
                                        break;
                                    }
                                    foreach (dynamic parLevel3Value_OuterList in Parametrizacao["ParLevel3Value_OuterList"])
                                    {
                                        tipo = parLevel3Value_OuterList["ParLevel3InputType_Id"];
                                        minimo = parLevel3Value_OuterList["LimInferior"];
                                        maximo = parLevel3Value_OuterList["LimSuperior"];
                                        break;
                                    }
                                    break;
                                }
                            }

                            foreach (dynamic valorTarefa in Tarefa)
                            {
                                valorDaTarefa = valorTarefa[(amostraAtual + 1).ToString()];
                                if (contador == amostraAtual)
                                {
                                    break;
                                }
                                ++contador;
                            }

                            bool isConform = false;
                            if (Convert.ToInt32(tipo) == 99 || Convert.ToInt32(tipo) == 3)
                            {
                                isConform = Convert.ToDecimal(minimo.Replace(".", ",")) <= Convert.ToDecimal(valorDaTarefa.Replace(".", ","))
                                    && Convert.ToDecimal(valorDaTarefa.Replace(".", ",")) <= Convert.ToDecimal(maximo.Replace(".", ","));
                            }
                            else
                            {
                                if (valorDaTarefa == "1" || valorDaTarefa == "true")
                                {
                                    isConform = true;
                                }
                            }

                            collectionJson += "<level03>";
                            collectionJson += tarefa_id; //level3 0
                            collectionJson += ","; //SEPARADOR LEVEL3
                            collectionJson += dataColetaFormatada.ToString("MM/dd/yyyy HH:mm:ss");//collectionDate; //level3 1
                            collectionJson += ","; //SEPARADOR LEVEL3
                            collectionJson += valorDaTarefa; //level3 2
                            collectionJson += ","; //SEPARADOR LEVEL3
                            collectionJson += isConform; //level3 3 //conforme ou não
                            collectionJson += ","; //SEPARADOR LEVEL3
                            collectionJson += "1"; //level3 4
                            collectionJson += ","; //SEPARADOR LEVEL3
                            collectionJson += "null"; //level3 5
                            collectionJson += ","; //SEPARADOR LEVEL3
                            collectionJson += valorDaTarefa; //level3 6
                            collectionJson += ","; //SEPARADOR LEVEL3
                            collectionJson += "undefined"; //level3 7
                            collectionJson += ","; //SEPARADOR LEVEL3
                            collectionJson += peso.ToString().Replace(",", "."); //level3 8 -------------------------- peso da tarefa
                            collectionJson += ","; //SEPARADOR LEVEL3
                            collectionJson += nome; //level3 9 --------------------------- nome da tarefa
                            collectionJson += ","; //SEPARADOR LEVEL3
                            collectionJson += minimo; //level3 10
                            collectionJson += ","; //SEPARADOR LEVEL3
                            collectionJson += maximo; //level3 11
                            collectionJson += ","; //SEPARADOR LEVEL3
                            collectionJson += "0"; //level3 12 --------------------- verificar caso que possa existir Não Avaliar
                            collectionJson += ","; //SEPARADOR LEVEL3
                            collectionJson += "0";//av; //level3 13
                            collectionJson += ","; //SEPARADOR LEVEL3
                            collectionJson += (isConform ? 0 : 1);//NC; //level3 14
                            collectionJson += ","; //SEPARADOR LEVEL3
                            collectionJson += "1"; //level3 15 
                            collectionJson += ","; //SEPARADOR LEVEL3
                            collectionJson += (isConform ? 0 : 1) * peso; //level3 16 --------------------- peso X defeito
                            collectionJson += ","; //SEPARADOR LEVEL3
                            collectionJson += "0"; //level3 17
                            collectionJson += "</level03>";

                        }
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
                    collectionJson += "0"; //[35]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "0"; //[36]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "0"; //[37]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "0"; //[38]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "0"; //[39]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "0"; //[40]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "0"; //[41]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "0"; //[42]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "0"; //[43]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "0"; //[44]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "0"; //[45]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "0"; //[46]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "0"; //[47]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "0"; //[48]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += processo; //[49]
                    collectionJson += "</level02>";
                    listaCollectionJson.Add(collectionJson);
                }

                amostraAtual++;
                avaliacaoAtual++;
            }

            return listaCollectionJson;
        }

    }


}
