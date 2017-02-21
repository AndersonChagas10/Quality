﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data.SqlClient;
using System.Web.Helpers;
using SgqSystem.Handlres;
using System.Web.Http.Cors;
using SgqSystem.Services;
using SGQDBContext;
using Dominio.Services;
using DTO.Helpers;
using System.Net.Mail;
using System.Net;
using SgqSystem.ViewModels;
using System.Threading;
using System.Transactions;
using System.Globalization;
using System.Collections;

namespace SgqSystem.Services
{
    /// <summary>
    /// Summary description for SyncServices
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [EnableCors(origins: "*", headers: "*", methods: "*")]

    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class SyncServices : System.Web.Services.WebService
    {
        string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;
        //private SqlConnection connection;

        #region Funções

        /// <summary>
        /// Converter a Data do Tablet
        /// </summary>
        /// <param name="collectionDate">Data Formatada do Tablet</param>
        /// <returns></returns>
        private DateTime DateCollectConvert(string collectionDate)
        {
            if (!collectionDate.Contains("/"))
            {
                collectionDate = collectionDate.Substring(0, 2) + "/" + collectionDate.Substring(2, 2) + "/" + collectionDate.Substring(4, 4) + " 00:00:00";
            }
            string[] data = collectionDate.Split('/');

            //verificar o tipo de data quando for no brasil
            string ano = data[2].Substring(0, 4);
            string mes = data[0];
            string dia = data[1];

            string hora = data[2].Substring(4, (data[2].Length - 4));
            hora = hora.Trim();
            if (hora.Length == 5)
            {
                hora += ":00";
            }
            string[] horaArray = hora.Split(':');


            DateTime newData = new DateTime(
                                            year: Convert.ToInt32(ano),
                                            month: Convert.ToInt32(mes),
                                            day: Convert.ToInt32(dia),
                                            hour: Convert.ToInt32(horaArray[0]),
                                            minute: Convert.ToInt32(horaArray[1]),
                                            second: Convert.ToInt32(horaArray[2]));

            return newData;
        }
        /// <summary>
        /// Retornar um valor Padrão para Campos que chegam como NULL, "", "undefined" ou "null"
        /// </summary>
        /// <param name="valor"></param>
        /// <param name="valorDefault"></param>
        /// <returns></returns>
        public string DefaultValueReturn(string valor, string valorDefault)
        {
            if (string.IsNullOrEmpty(valor) || valor == "undefined" || valor == "null")
            {
                return valorDefault;
            }
            return valor;
        }
        /// <summary>
        /// Converter para Booleano Padrão Sql um valor passado
        /// </summary>
        /// <param name="valor">valor True ou False</param>
        /// <returns></returns>
        public string BoolConverter(string valor)
        {
            valor = valor.ToLower();
            if (valor == "true")
            {
                return "1";
            }
            else
            {
                return "0";
            }
        }
        public string BoolCompletedConverter(string valor)
        {
            valor = valor.ToLower();
            if (valor == "completed")
            {
                return "1";
            }
            else
            {
                return "0";
            }
        }
        public int insertLogJson(string result, string log, string deviceId, string AppVersion, string callback)
        {
            string sql = "INSERT INTO LogJson ([result],[log],[AddDate],[Device_Id],[AppVersion], [callback]) " +
                         "VALUES " +
                         "('" + result.Replace("'", "") + "', '" + log.Replace("'", "") + "', GETDATE(), '" + deviceId + "', '" + AppVersion + "', '" + callback + "')";
            string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        var i = Convert.ToInt32(command.ExecuteNonQuery());
                        if (i > 0)
                        {
                            return i;
                        }
                        else
                        {
                            return 0;
                        }

                    }
                }
            }
            catch (SqlException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion
        #region Json
        /// <summary>
        /// Método Para Inserir Resultado da Coleta
        /// </summary>
        /// <param name="ObjResultJSon">Objeto para salvar no banco de dados</param>
        /// <param name="deviceId">Uuid do dispositivo no caso do APP</param> //para Web vem nulo
        /// <param name="deviceMac">Mac Adress</param> //O App não manda
        /// <returns></returns>
        /// O resultado da coleta é salvo por Level02
        /// Processo no tablet
        /// 1. Usuário Clica no botão Sync
        /// 2. O Tablet processa o Sync através do método sendResults() quando o usuário Clinca no botão Sync.
        /// 3. O metodo sendResults() processa os resultados não sincronizados e envia pacote de 500 resultados (Pode ser configurado) para gravar na tabla CollectionJson.
        /// 4. O método executa o insert
        /// 5. Após o Insert retorna mensagem para APP.
        /// 6. O processo continua 1 a 5 se repete até finalizar os resultados.
        [WebMethod]
        public string InsertJson(string ObjResultJSon, string deviceId, string deviceMac, bool autoSend)
        {
            if (string.IsNullOrEmpty(ObjResultJSon))
            {
                return null;
            }
            //A key não está sendo utilizada
            string key = "111111";
            //Converto o Objeto Json e prepara para extrair os dados do Level02
            ObjResultJSon = ObjResultJSon.Replace("</level02><level02>", "@").Replace("<level02>", "").Replace("</level02>", "");
            //Gera um array
            string[] arrayObj = ObjResultJSon.Split('@');
            //Instanciamos a linha que gera a query
            //Percorre o Objeto
            string versaoApp = null;

            string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {
                     connection.Open();

                    SqlCommand command;
                    for (int i = 0; i < arrayObj.Length; i++)
                    {
                        //Estrai o resultado
                        string[] result = arrayObj[i].Split(';');
                        //Id do Level01
                        string level01Id = result[0];
                        //Data que a coleta começou ser gerada, pelo Id do Level01
                        string level01DataCollect = result[1];
                        //Converte a Data para o padrão correto
                        //Nos EUA a data é mostrado como "11/25/2016 13:05"
                        ////Tem que converter a data do padrão Brasil também
                        DateTime level01CollectData = DateCollectConvert(level01DataCollect);
                        //Converte a Data em String para utilizar no comando sql
                        level01DataCollect = level01CollectData.ToString("yyyy-MM-dd HH:mm:ss");

                        //Pega o Id do Level02
                        string level02Id = result[2];
                        //Pega a Data da Coleta do Level02
                        string level02DataCollect = result[3];

                        //Pega a Coleta do Level02
                        DateTime level02CollectData = DateCollectConvert(level02DataCollect);
                        //Converte a data no padrão igual a data do LEvel01
                        level02DataCollect = level02CollectData.ToString("yyyy-MM-dd HH:mm:ss");

                        //Pega o Id da Unidade
                        string unidadeId = result[4];

                        ////MOCK
                        //unidadeId = DefaultValueReturn(unidadeId, "1");
                        //Pega o Period


                        string period = result[5];
                        period = DefaultValueReturn(period, "1");

                        //Pega o Shit
                        string shift = result[6];
                        shift = DefaultValueReturn(shift, "1");

                        //Pega o Auditor
                        string auditorId = result[7];
                        //Verifica se é reauditoria
                        string reaudit = result[9];
                        //Converte para o padrão Sql
                        ///Criar funções de converter de data, campo nulo automaticos

                        reaudit = BoolConverter(reaudit);

                        //Pega número da Avaliação
                        string evaluate = result[11];
                        //Pega número da Amostra
                        string sample = result[12];
                        //Versão do App
                        versaoApp = result[20];
                        //Ambiente utilizado Ex: Homologação/Produção
                        string ambiente = result[21];
                        //Phase
                        string phase = result[8];
                        //StartPhaseDate
                        string startphasedate = result[10];
                        //Cattle Type (Biased/Unbiased está no Cattle Type também)
                        //Chain Speed
                        string isemptylevel3 = result[14];
                        //Lot Number
                        string hassampletotal = result[15];
                        //Mud Score
                        string mudscore = result[16];
                        //Verifica falhas, descontinuado na JBS EUA por enquanto
                        string consecutivefailurelevel = result[17];
                        string consecutivefailuretotal = result[18];
                        //Verifica se a coleta foi não avaliada
                        ///Sugestão EUA para motivo de não avaliar a coleta
                        string notavaliable = result[19];

                        //Coloca Biased/ Unbiased no Cattle Type
                        string baisedUnbaised = result[27];
                        baisedUnbaised = DefaultValueReturn(baisedUnbaised, "0");
                        //if (baisedUnbaised != "0")
                        //{
                        //    cattletype = baisedUnbaised;
                        //}
                        string AlertLevel = result[27];
                        string completed = result[28];
                        string havePhases = result[29];
                        string CollectionLevel02Id = result[30];
                        string correctiveActionCompleted = result[31];
                        string completeReaudit = result[32];
                        string hashKey = result[33];
                        string weievaluation = result[35];
                        string weidefects = result[36];
                        string defects = result[37];
                        string totallevel3withdefects = result[38];
                        string totalLevel2Evaluation = result[39];
                        string avaliacaoultimoalerta = result[40];
                        string evaluatedresult = result[41];
                        string defectsresult = result[42];
                        string sequential = result[43];
                        string side = result[44];
                        string monitoramentoultimoalerta = result[45];
                        //string alertaAtual = result[40];

                        //Gera o Cabeçalho do Level02
                        string level02HeaderJSon = result[13];
                        level02HeaderJSon += ";" + phase;
                        level02HeaderJSon += ";" + startphasedate;
                        level02HeaderJSon += ";" + consecutivefailurelevel;
                        level02HeaderJSon += ";" + consecutivefailuretotal;
                        level02HeaderJSon += ";" + notavaliable; //[5]
                        level02HeaderJSon += ";" + completed;
                        level02HeaderJSon += ";" + havePhases;
                        level02HeaderJSon += ";" + CollectionLevel02Id;
                        level02HeaderJSon += ";" + correctiveActionCompleted;
                        level02HeaderJSon += ";" + completeReaudit; //[10]
                        level02HeaderJSon += ";" + AlertLevel;
                        level02HeaderJSon += ";" + sequential;
                        level02HeaderJSon += ";" + side;
                        level02HeaderJSon += ";" + weievaluation;
                        level02HeaderJSon += ";" + weidefects;
                        level02HeaderJSon += ";" + defects;
                        level02HeaderJSon += ";" + totallevel3withdefects;
                        level02HeaderJSon += ";" + totalLevel2Evaluation;
                        level02HeaderJSon += ";" + avaliacaoultimoalerta;
                        level02HeaderJSon += ";" + evaluatedresult;
                        level02HeaderJSon += ";" + defectsresult;
                        level02HeaderJSon += ";" + sequential;
                        level02HeaderJSon += ";" + side;
                        level02HeaderJSon += ";" + isemptylevel3;
                        level02HeaderJSon += ";" + hassampletotal;
                        level02HeaderJSon += ";" + hashKey;
                        level02HeaderJSon += ";" + monitoramentoultimoalerta;
                        
                        //level02HeaderJSon += ";" + alertaAtual;

                        //Verifica o Resultado do Level03
                        string level03ResultJson = result[22];
                        //Decodifica a o resultado
                        level03ResultJson = HttpUtility.UrlDecode(level03ResultJson, System.Text.Encoding.Default);
                        //Ação Corretiva
                        string correctiveActionJson = result[23];
                        //Verifica se tem reauditoria pendente
                        string haveReaudit = result[24];
                        //Convert Reauditoria Pendente para valor correto
                        haveReaudit = DefaultValueReturn(haveReaudit, "0");
                        if (haveReaudit != "0")
                        {
                            haveReaudit = "1";
                        }
                        //Se Ação corretiva ficou pendente
                        string haveCorrectiveAction = result[25];
                        //Converte ação corretiva para valor correto
                        haveCorrectiveAction = DefaultValueReturn(haveCorrectiveAction, "0");
                        if (haveCorrectiveAction == "havecorrectiveaction")
                        {
                            haveCorrectiveAction = "1";
                        }
                        //Número da reauditoria
                        string reauditNumber = result[26];
                        reauditNumber = DefaultValueReturn(reauditNumber, "0");
                        //Cria a linah de insert



                        string sql = "INSERT INTO [dbo].[CollectionJson] " +
                               "([Unit_Id],[Shift],[Period],[level01_Id],[Level01CollectionDate],[level02_Id],[Evaluate],[Sample],[AuditorId],[Level02CollectionDate],[Level02HeaderJson],[Level03ResultJSon],[CorrectiveActionJson],[Reaudit],[ReauditNumber],[haveReaudit],[haveCorrectiveAction],[Device_Id],[AppVersion],[Ambient],[IsProcessed],[Device_Mac],[AddDate],[AlterDate],[Key],[TTP]) " +
                               "VALUES " +
                               "('" + unidadeId + "','" + shift + "','" + period + "','" + level01Id + "',CAST(N'" + level01DataCollect + "' AS DateTime),'" + level02Id + "','" + evaluate + "','" + sample + "', '" + auditorId + "',CAST(N'" + level02DataCollect + "' AS DateTime),'" + level02HeaderJSon + "','" + level03ResultJson + "', '" + correctiveActionJson + "', '" + reaudit + "', '" + reauditNumber + "', '" + haveReaudit + "','" + haveCorrectiveAction + "' ,'" + deviceId + "','" + versaoApp + "','" + ambiente + "',0,'" + deviceMac + "',GETDATE(),NULL,'" + key + "',NULL) ";

                        if (autoSend == true)
                        {
                            sql += "SELECT @@IDENTITY AS 'Identity'";
                        }
                        else
                        {
                            sql += "SELECT '1' AS 'Identity'";
                        }

                        command = new SqlCommand(sql, connection);

                        // var i = command.ExecuteNonQuery();
                        var iSql = Convert.ToInt32(command.ExecuteScalar());
                        if (iSql > 0)
                        {
                            if (autoSend == true)
                            {
                                ProcessJson(null, iSql);
                            }
                        }
                        else
                        {
                            //Se não ocorre sem problemas, retorna um erro
                            throw new Exception("erro json");

                        }
                    }
                }
                return null;
            }
            catch (SqlException ex)
            {
                int insertLog = insertLogJson(ObjResultJSon, ex.Message, deviceId, versaoApp, "InsertJson");
                return "error";
                //return "error sql insert";
            }
            catch (Exception ex)
            {
                int insertLog = insertLogJson(ObjResultJSon, ex.Message, deviceId, versaoApp, "InsertJson");
                return "error";
            }
        }

        /// <summary>
        /// Método que grava o Json nas tabelas de resultados
        /// </summary>
        /// <param name="device">Id do dispositivo</param>
        /// <returns></returns>
        /// Para chamar uma consolidação geral digite [web]
        [WebMethod]
        public string ProcessJson(string device, int id)
        {

            try
            {
                if (string.IsNullOrEmpty(device) && id == 0)
                {
                    return "informe o device";
                }
                string query = null;
                //Se for igual web busca de todos os dispositivos
                if (device == "web")
                {
                    query = null;
                }
                else
                {
                    query = "[Device_Id] = '" + device + "' AND";
                }

                if (id > 0)
                {
                    query = "[Id] = '" + id + "' AND";
                }

                string sql = "SELECT [level01_Id], [Level01CollectionDate], [level02_Id], [Level02CollectionDate], [Unit_Id],[Period], [Shift], [AppVersion], [Ambient], [Device_Id], [Device_Mac] , [Key], [Level03ResultJSon], [Id], [Level02HeaderJson], [Evaluate],[Sample],[AuditorId], [Reaudit], [CorrectiveActionJson],[haveReaudit],[haveCorrectiveAction],[ReauditNumber]  FROM CollectionJson WHERE " + query + " [IsProcessed] = 0";


                var CollectionJsonDB = new SGQDBContext.CollectionJson();
                var ConsolidationLevel1DB = new SGQDBContext.ConsolidationLevel1();
                var ConsolidationLevel2DB = new SGQDBContext.ConsolidationLevel2();

                var collectionJson = CollectionJsonDB.getJson(sql);


                //connection = new SqlConnection(conexao);
                //connection.Open();

                foreach (var c in collectionJson)
                {
                    int ConsolidationLevel1_Id = 0;
                    int ConsolidationLevel2_Id = 0;
                    string AlertLevel = "0";
                    string avaliacaoultimoalerta = "0";
                    string monitoramentoultimoalerta = "0";

                    //using (var transacao = new TransactionScope())
                    //{
                    //Cabecalho                   
                    string[] arrayHeader = c.Level02HeaderJson.Split(';');

                        string headersContadores = arrayHeader[0];
                        string Phase = arrayHeader[1];

                        string Reaudit = BoolConverter(c.Reaudit.ToString());

                        string StartPhase = arrayHeader[2];
                        if (string.IsNullOrEmpty(StartPhase) || StartPhase == "null" || StartPhase == "undefined")
                        {
                            StartPhase = "'0001-01-01 00:00:00'";
                        }
                        else
                        {
                            DateTime dataPhase = DateCollectConvert(StartPhase);
                            StartPhase = "CAST(N'" + dataPhase.ToString("yyyy-MM-dd 00:00:00") + "' AS DateTime)";
                        }


                        string ConsecuticeFalireIs = arrayHeader[3];
                        ConsecuticeFalireIs = DefaultValueReturn(arrayHeader[6], "0");
                        if (ConsecuticeFalireIs != "0")
                        {
                            ConsecuticeFalireIs = "1";
                        }

                        string ConsecutiveFailureTotal = arrayHeader[4];
                        ConsecutiveFailureTotal = DefaultValueReturn(ConsecutiveFailureTotal, "0");
                        string NotEvaluateIs = arrayHeader[5];
                        NotEvaluateIs = BoolConverter(NotEvaluateIs);

                        string Duplicated = "0";

                        string completed = arrayHeader[6];
                        completed = BoolCompletedConverter(completed);

                        bool update = false;
                        string idCollectionLevel2 = arrayHeader[8];
                        idCollectionLevel2 = DefaultValueReturn(idCollectionLevel2, "0");
                        if (idCollectionLevel2 != "0")
                        {
                            update = true;
                        }
                        string havePhases = arrayHeader[7];
                        havePhases = BoolConverter(havePhases);

                        AlertLevel = arrayHeader[11];
                        AlertLevel = DefaultValueReturn(AlertLevel, "0");

                        string weievaluation = arrayHeader[14];
                        string weidefects = arrayHeader[15];
                        string defects = arrayHeader[16];
                        string totallevel3withdefects = arrayHeader[17];
                        string totalLevel3evaluation = arrayHeader[18];

                        avaliacaoultimoalerta = arrayHeader[19];
                        avaliacaoultimoalerta = DefaultValueReturn(avaliacaoultimoalerta, "0");

                        

                        string evaluatedresult = arrayHeader[20];
                        string defectsresult = arrayHeader[21];
                        string sequential = arrayHeader[22];
                        sequential = DefaultValueReturn(sequential, "0");
                        string side = arrayHeader[23];
                        side = DefaultValueReturn(side, "0");

                        string isemptylevel3 = arrayHeader[24];
                        isemptylevel3 = DefaultValueReturn(isemptylevel3, "0");
                        isemptylevel3 = BoolConverter(isemptylevel3);

                                             

                        string haveReaudit = BoolConverter(c.haveReaudit.ToString());

                        string haveCorrectiveAction = BoolConverter(c.haveCorrectiveAction.ToString());

                        string reauditNumber = DefaultValueReturn(c.ReauditNumber.ToString(), "0");

                        var consolidationLevel1 = ConsolidationLevel1DB.getConsolidation(c.Unit_Id, c.level01_Id, c.Level01CollectionDate);

                        if (consolidationLevel1 == null)
                        {
                            consolidationLevel1 = InsertConsolidationLevel1(c.Unit_Id, c.level01_Id, c.Level01CollectionDate);
                            if (consolidationLevel1 == null)
                            {
                                throw new Exception();
                            }
                        }

                        var consolidationLevel2 = ConsolidationLevel2DB.getByConsolidationLevel1(c.Unit_Id, consolidationLevel1.Id, c.level02_Id);
                        if (consolidationLevel2 == null)
                        {
                            consolidationLevel2 = InsertConsolidationLevel2(consolidationLevel1.Id, c.level02_Id, c.Unit_Id, c.Level02CollectionDate);
                            if (consolidationLevel2 == null)
                            {
                                throw new Exception();
                            }
                        }
                        ConsolidationLevel1_Id = consolidationLevel1.Id;
                        ConsolidationLevel2_Id = consolidationLevel2.Id;


                    int sampleCollect = c.Sample;
                    int sampleTotal = consolidationLevel2.EvaluatedResult;


                    bool hasSampleTotal = Convert.ToBoolean(DefaultValueReturn(arrayHeader[25], "false"));
                    if (hasSampleTotal == true)
                    {
                        sampleCollect = sampleTotal++;
                    }

                    string hashKey = arrayHeader[26];
                    hashKey = DefaultValueReturn(hashKey, "0");

                    monitoramentoultimoalerta = arrayHeader[27];
                    monitoramentoultimoalerta = DefaultValueReturn(monitoramentoultimoalerta, "0");

                    int CollectionLevel2Id = InsertCollectionLevel2(consolidationLevel1, consolidationLevel2, c.AuditorId, c.Shift, c.Period, Phase, c.Reaudit, c.ReauditNumber, c.Level02CollectionDate,
                                                StartPhase, c.Evaluate, sampleCollect, ConsecuticeFalireIs, ConsecutiveFailureTotal, NotEvaluateIs, Duplicated, haveReaudit,
                                                haveCorrectiveAction, havePhases, completed, idCollectionLevel2, AlertLevel, sequential, side,
                                                weievaluation, weidefects, defects, totallevel3withdefects, totalLevel3evaluation, avaliacaoultimoalerta, monitoramentoultimoalerta, evaluatedresult, defectsresult, isemptylevel3, hashKey);

                    if(CollectionLevel2Id > 0)
                    {

                        int CollectionLevel3Id = InsertCollectionLevel3(CollectionLevel2Id.ToString(), c.level02_Id, c.Level03ResultJSon, c.AuditorId, Duplicated);


                        headersContadores = headersContadores.Replace("</header><header>", ";").Replace("<header>", "").Replace("</header>", "");
                        if (!string.IsNullOrEmpty(headersContadores))
                        {
                            int headerFieldId = InsertCollectionLevel2HeaderField(CollectionLevel2Id, headersContadores);
                        }

                        string correctiveActionCompleted = arrayHeader[9];
                        if (haveCorrectiveAction == "0")
                        {
                            correctiveActionCompleted = DefaultValueReturn(correctiveActionCompleted, "0");
                            if (correctiveActionCompleted == "correctiveActionComplete")
                            {
                                correctiveActionCompleted = "0";
                            }
                        }
                        else
                        {
                            correctiveActionCompleted = haveCorrectiveAction;
                        }

                        string reauditCompleted = arrayHeader[10];
                        if (haveReaudit == "0")
                        {
                            reauditCompleted = DefaultValueReturn(reauditCompleted, "0");
                            if (reauditCompleted == "completereaudit")
                            {
                                reauditCompleted = "0";
                            }
                        }
                        else
                        {
                            reauditCompleted = haveReaudit;
                        }


                        if (update == true && (correctiveActionCompleted == "0" || reauditCompleted == "0"))
                        {
                            int updateCorrectiveAction = updateLevel02CorrectiveActionReaudit(CollectionLevel2Id.ToString(), correctiveActionCompleted, reauditCompleted);
                            if (updateCorrectiveAction == 0)
                            {
                                //return "erro update correctiveaction";
                                return "error";
                            }
                        }

                        if (!string.IsNullOrEmpty(c.CorrectiveActionJson))
                        {
                            c.CorrectiveActionJson = c.CorrectiveActionJson.Replace("<correctiveaction>", "").Replace("</correctiveaction>", "");

                            string[] arrayCorrectiveAction = c.CorrectiveActionJson.Split(',');

                            string slaugthersignature = arrayCorrectiveAction[0];
                            string techinicalsignature = arrayCorrectiveAction[1];
                            string datetimeslaughter = arrayCorrectiveAction[2];
                            string datetimetechinical = arrayCorrectiveAction[3];
                            string auditstarttime = arrayCorrectiveAction[4];
                            string datecorrectiveaction = arrayCorrectiveAction[5];

                            string descriptionFailure = arrayCorrectiveAction[6];
                            descriptionFailure = HttpUtility.UrlDecode(descriptionFailure, System.Text.Encoding.Default);

                            string immediateCorrectiveAction = arrayCorrectiveAction[7];
                            immediateCorrectiveAction = HttpUtility.UrlDecode(immediateCorrectiveAction, System.Text.Encoding.Default);

                            string productDisposition = arrayCorrectiveAction[8];
                            productDisposition = HttpUtility.UrlDecode(productDisposition, System.Text.Encoding.Default);

                            string preventativeMeasure = arrayCorrectiveAction[9];
                            preventativeMeasure = HttpUtility.UrlDecode(preventativeMeasure, System.Text.Encoding.Default);

                            int CorrectiveActionId = correctiveActionInsert(c.AuditorId.ToString(), CollectionLevel2Id.ToString(), slaugthersignature, techinicalsignature, datetimeslaughter,
                                                                           datetimetechinical, datecorrectiveaction, auditstarttime, descriptionFailure, immediateCorrectiveAction,
                                                                           productDisposition, preventativeMeasure);

                            if (CorrectiveActionId == 0)
                            {
                                //return "erro CorrectiveAction";
                                return "error";
                            }

                        }

                        int jsonUpdate = updateJson(c.Id);

                        //    transacao.Complete();

                        //}

                        var CollectionLevel2ConsolidationDB = new SGQDBContext.CollectionLevel2Consolidation();
                        var collectionLevel2Consolidation = CollectionLevel2ConsolidationDB.getConsolidation(ConsolidationLevel2_Id, c.level02_Id);

                        var updateConsolidationLevel2Id = updateConsolidationLevel2(ConsolidationLevel2_Id, AlertLevel, avaliacaoultimoalerta, monitoramentoultimoalerta, collectionLevel2Consolidation);

                        var ConsolidationLevel1XConsolidationLevel2DB = new ConsolidationLevel1XConsolidationLevel2();
                        var consolidationLevel1XConsolidationLevel2 = ConsolidationLevel1XConsolidationLevel2DB.getConsolidation(ConsolidationLevel1_Id);

                        var updateConsolidationLevel1Id = updateConsolidationLevel1(ConsolidationLevel1_Id, AlertLevel, avaliacaoultimoalerta, monitoramentoultimoalerta, consolidationLevel1XConsolidationLevel2);
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int updateJson(int CollectionJson_Id)
        {
            string sql = "UPDATE CollectionJson SET IsProcessed=1 WHERE ID='" + CollectionJson_Id + "'";
            string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        var i = Convert.ToInt32(command.ExecuteNonQuery());
                        if (i > 0)
                        {
                            return i;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                int insertLog = insertLogJson(sql, ex.Message, "N/A", "N/A", "updateJson");
                throw ex;
            }
            catch (Exception ex)
            {
                int insertLog = insertLogJson(sql, ex.Message, "N/A", "N/A", "updateJson");
                throw ex;
            }
        }
        public int updateConsolidationLevel2(int ConsolidationLevel2_Id, string AlertLevel, string LastEvaluationAlert, string LastLevel2Alert, SGQDBContext.CollectionLevel2Consolidation CollectionLevel2Consolidation)
        {
            //verificar se não vai sobreescrever informação com tablet antigo
            int LastEvaluationAlertCheck = Convert.ToInt32(LastEvaluationAlert);
            if (CollectionLevel2Consolidation.LastEvaluationAlert > LastEvaluationAlertCheck)
            {
                LastEvaluationAlert = CollectionLevel2Consolidation.LastEvaluationAlert.ToString();
            }

            string sql = "UPDATE ConsolidationLevel2 SET AlertLevel=" + AlertLevel.ToString().Replace(",", ".") + ", WeiEvaluation=" + CollectionLevel2Consolidation.WeiEvaluationTotal.ToString().Replace(",", ".") + ", EvaluateTotal=" + CollectionLevel2Consolidation.TotalLevel3Evaluation.ToString().Replace(",", ".") + ", DefectsTotal=" + CollectionLevel2Consolidation.DefectsTotal.ToString().Replace(",", ".") + ", WeiDefects=" + CollectionLevel2Consolidation.WeiDefectsTotal.ToString().Replace(",", ".") + ", TotalLevel3Evaluation=" + CollectionLevel2Consolidation.TotalLevel3Evaluation.ToString().Replace(",", ".") + ", TotalLevel3WithDefects=" + CollectionLevel2Consolidation.TotalLevel3WithDefects.ToString().Replace(",", ".") + ", LastEvaluationAlert='" + LastEvaluationAlert.ToString().Replace(",", ".") + "', LastLevel2Alert='" + LastLevel2Alert.ToString().Replace(",", ".") + "', EvaluatedResult='" + CollectionLevel2Consolidation.EvaluatedResult + "', DefectsResult='" + CollectionLevel2Consolidation.DefectsResult + "' WHERE ID='" + ConsolidationLevel2_Id.ToString().Replace(",", ".") + "'";
            string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        var i = Convert.ToInt32(command.ExecuteNonQuery());
                        if (i > 0)
                        {
                            return i;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                int insertLog = insertLogJson(sql, ex.Message, "N/A", "N/A", "updateConsolidationLevel2");
                return 0;
            }
            catch (Exception ex)
            {
                int insertLog = insertLogJson(sql, ex.Message, "N/A", "N/A", "updateConsolidationLevel2");
                return 0;
            }
        }

        public int updateConsolidationLevel1(int ConsolidationLevel1_Id, string AlertLevel, string LastEvaluationAlert, string LastLevel2Alert, SGQDBContext.ConsolidationLevel1XConsolidationLevel2 CL1XCL2)
        {
            int LastEvaluationAlertCheck = Convert.ToInt32(LastEvaluationAlert);
            if (CL1XCL2.LastEvaluationAlert > LastEvaluationAlertCheck)
            {
                LastEvaluationAlert = CL1XCL2.LastEvaluationAlert.ToString();
            }

            string sql = "UPDATE ConsolidationLevel1 SET AtualAlert=" + AlertLevel.ToString().Replace(",", ".") + ", Evaluation=" + CL1XCL2.EvaluateTotal.ToString().Replace(",", ".") + ", WeiEvaluation=" + CL1XCL2.WeiEvaluation.ToString().Replace(",", ".") + ", EvaluateTotal=" + CL1XCL2.EvaluateTotal.ToString().Replace(",", ".") + ", DefectsTotal=" + CL1XCL2.DefectsTotal.ToString().Replace(",", ".") + ", WeiDefects=" + CL1XCL2.WeiDefects.ToString().Replace(",", ".") + ", TotalLevel3Evaluation=" + CL1XCL2.TotalLevel3Evaluation.ToString().Replace(",", ".") + ", TotalLevel3WithDefects=" + CL1XCL2.TotalLevel3WithDefects.ToString().Replace(",", ".") + ", LastEvaluationAlert='" + LastEvaluationAlert.ToString().Replace(",", ".") + "', LastLevel2Alert='" + LastLevel2Alert.ToString().Replace(",", ".") + "', EvaluatedResult='" + CL1XCL2.EvaluatedResult.ToString().Replace(",", ",") + "', DefectsResult='" + CL1XCL2.DefectsResult.ToString().Replace(",", ".") + "' WHERE ID='" + ConsolidationLevel1_Id.ToString().Replace(",", ".") + "'";
            string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        var i = Convert.ToInt32(command.ExecuteNonQuery());
                        if (i > 0)
                        {
                            return i;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                int insertLog = insertLogJson(sql, ex.Message, "N/A", "N/A", "updateConsolidationLevel1");
                throw ex;
            }
            catch (Exception ex)
            {
                int insertLog = insertLogJson(sql, ex.Message, "N/A", "N/A", "updateConsolidationLevel1");
                throw ex;
            }
        }
        public int updateCorrectiveAction_CollectionLevel2_By_ParLevel1(string ParLevel1_Id, string ParCompany_Id, string dataInicio, string dataFim)
        {

            string sql = "UPDATE CollectionLevel2 SET HaveCorrectiveAction=0 WHERE ParLevel1_Id='" + ParLevel1_Id + "' AND UnitId='" + ParCompany_Id + "' AND CollectionDate BETWEEN '" + dataInicio + " 00:00:00' AND '" + dataFim + " 23:59:59' AND HaveCorrectiveAction=1";
            string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        var i = Convert.ToInt32(command.ExecuteNonQuery());
                        if (i > 0)
                        {
                            return i;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                int insertLog = insertLogJson(sql, ex.Message, "N/A", "N/A", "updateCorrectiveAction_CollectionLevel2_By_ParLevel1");
                return 0;
            }
            catch (Exception ex)
            {
                int insertLog = insertLogJson(sql, ex.Message, "N/A", "N/A", "updateCorrectiveAction_CollectionLevel2_By_ParLevel1");
                return 0;
            }
        }

        public int updateLevel02CorrectiveActionReaudit(string id, string correctiveAction, string reaudit)
        {
            string sql = "UPDATE CollectionLevel02 SET HaveCorrectiveAction='" + correctiveAction + "', HaveReaudit='" + reaudit + "' WHERE ID='" + id + "'";
            string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        var i = Convert.ToInt32(command.ExecuteNonQuery());
                        if (i > 0)
                        {
                            return i;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                int insertLog = insertLogJson(correctiveAction, ex.Message, "N/A", "N/A", "updateLevel02CorrectiveActionReaudit");
                return 0;
            }
            catch (Exception ex)
            {
                int insertLog = insertLogJson(correctiveAction, ex.Message, "N/A", "N/A", "updateLevel02CorrectiveActionReaudit");
                return 0;
            }
        }
        #endregion
        #region Consolidation Level01
        /// <summary>
        /// Método que faz a inserção da consolidação
        /// </summary>
        /// <param name="unitId">Id da Unidade</param>
        /// <param name="level01Id">Id do Level01</param>
        /// <param name="collectionDate">Data da Coleta que verifica a consolidação</param>
        /// <param name="departmentId">Id do Departamento</param>
        /// <returns></returns>
        public SGQDBContext.ConsolidationLevel1 InsertConsolidationLevel1(int ParCompany_Id, int ParLevel1_Id, DateTime collectionDate, string departmentId = "1")
        {
            var ConsolidationLevel1DB = new SGQDBContext.ConsolidationLevel1();

            //Script de Insert para consolidação
            string sql = "INSERT ConsolidationLevel1 ([UnitId],[DepartmentId],[ParLevel1_Id],[AddDate],[AlterDate],[ConsolidationDate]) " +
                         "VALUES " +
                         "('" + ParCompany_Id + "','" + departmentId + "','" + ParLevel1_Id + "', GetDate(),null, CONVERT(DATE, '" + collectionDate.ToString("yyyy-MM-dd") + "')) " +
                         "SELECT @@IDENTITY AS 'Identity'";

            string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        var i = Convert.ToInt32(command.ExecuteScalar());
                        //Se o registro for inserido retorno o Id da Consolidação
                        if (i > 0)
                        {
                            return ConsolidationLevel1DB.getConsolidation(ParCompany_Id, ParLevel1_Id, collectionDate);
                        }
                        else
                        {
                            //Caso ocorra algum erro, retorno zero
                            return null;
                        }
                    }
                }
            }
            //Caso ocorra alguma Exception, grava o log e retorna zero
            catch (SqlException ex)
            {
                int insertLog = insertLogJson(sql, ex.Message, "N/A", "N/A", "InsertConsoliDationLevel1");
                throw ex;
            }
            catch (Exception ex)
            {
                int insertLog = insertLogJson(sql, ex.Message, "N/A", "N/A", "InsertConsoliDationLevel1");
                throw ex;
            }
        }

        /// <summary>
        /// Retorna o Id da Consolidação
        /// </summary>
        /// <param name="unitId">Id da Unidade</param>
        /// <param name="level01Id">Id do Level01</param>
        /// <param name="collectionDate">Data da Consolidação</param>
        /// <returns></returns>
        public int GetLevel1Consolidation(string unitId, string level01Id, string collectionDate)
        {
            //Converte a data no padrão de busca do Banco de Dados
            collectionDate = Convert.ToDateTime(collectionDate).ToString("yyyy-MM-dd");

            string sql = "SELECT Id FROM ConsolidationLevel1 WHERE UnitId = '" + unitId + "' AND ParLevel1_Id= '" + level01Id + "' AND CONVERT(date, ConsolidationDate) = '" + collectionDate + "'";

            string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader r = command.ExecuteReader())
                        {
                            //Se encontrar, retorna o Id da Consolidação
                            if (r.Read())
                            {
                                return Convert.ToInt32(r[0]);
                            }
                            //Se não encontrar, retorna zero
                            return 0;
                        }
                    }
                }
            }
            //Em caso de Exception, grava um log no Banco de Dados e Retorna Zero
            catch (SqlException ex)
            {
                int insertLog = insertLogJson(sql, ex.Message, "N/A", "N/A", "GetLevel1Consolidation");
                return 0;
            }
            catch (Exception ex)
            {
                int insertLog = insertLogJson(sql, ex.Message, "N/A", "N/A", "GetLevel1Consolidation");
                return 0;
            }
        }

        #endregion
        #region Consolidation Level02
        /// <summary>
        /// Insere Consolodiação do Level02
        /// </summary>
        /// <param name="Level01ConsolidationId">Id da Consolidação do Level01</param>
        /// <param name="Level02Id">Id do Level02</param>
        /// <param name="unitId">Id da Unidade</param>
        /// <param name="collectionDate">Data da Consolidação</param>
        /// <returns></returns>
        public SGQDBContext.ConsolidationLevel2 InsertConsolidationLevel2(int ConsolidationLevel1_Id, int ParLevel2_Id, int ParCompany_Id, DateTime collectionDate)
        {
            //Verifica se já existe uma consolidação para o level02
            var ConsolidationLevel2DB = new SGQDBContext.ConsolidationLevel2();
            //var ConsolidationLevel2 = ConsolidationLevel2DB.getbYConsolidationLevel1(Convert.ToInt32(Level01ConsolidationId), Convert.ToInt32(Level02Id));

            //if (ConsolidationLevel2 != null)
            //{
            //    return ConsolidationLevel2;
            //}


            //Gera o Script de Insert no Banco
            string sql = "INSERT ConsolidationLevel2 ([ConsolidationLevel1_Id], [ParLevel2_Id], [UnitId], [AddDate], [AlterDate], [ConsolidationDate]) " +
                         "VALUES  " +
                         "('" + ConsolidationLevel1_Id + "', '" + ParLevel2_Id + "', '" + ParCompany_Id + "', GETDATE(), NULL, CAST(N'" + collectionDate.ToString("yyyy-MM-dd") + "' AS DateTime)) " +
                         "SELECT @@IDENTITY AS 'Identity'";

            string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        var i = Convert.ToInt32(command.ExecuteScalar());
                        //Se inserir corretamente, retorno o Id da Consolidação
                        if (i > 0)
                        {
                            return ConsolidationLevel2DB.getByConsolidationLevel1(ParCompany_Id, ConsolidationLevel1_Id, ParLevel2_Id);
                        }
                        else
                        {
                            //Caso não ocorra a inserção, retorno zero
                            return null;
                        }
                    }
                }
            }
            //Caso ocorra qualquer Exception, insere no log e retorna zero
            catch (SqlException ex)
            {
                int insertLog = insertLogJson(sql, ex.Message, "N/A", "N/A", "InsertConsoliDationLevel2");
                throw ex;
            }
            catch (Exception ex)
            {
                int insertLog = insertLogJson(sql, ex.Message, "N/A", "N/A", "InsertConsoliDationLevel2");
                throw ex;
            }
        }

        //public int GetLevel2Consolidation2(string Level01ConsolidationId, string Level02Id)
        //{
        //    string sql = "SELECT Id FROM ConsolidationLevel2 WHERE ConsolidationLevel1_Id = '" + Level01ConsolidationId + "' AND ParLevel2_Id= '" + Level02Id + "'";
        //    string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;
        //    try
        //    {
        //        using (SqlConnection connection = new SqlConnection(conexao))
        //        {
        //            using (SqlCommand command = new SqlCommand(sql, connection))
        //            {
        //                connection.Open();
        //                using (SqlDataReader r = command.ExecuteReader())
        //                {
        //                    if (r.Read())
        //                    {
        //                        return Convert.ToInt32(r[0]);
        //                    }
        //                    return 0;
        //                }
        //            }
        //        }
        //    }
        //    catch (SqlException ex)
        //    {
        //        int insertLog = insertLogJson(sql, ex.Message, "N/A", "N/A", "GetLevel2Consolidation");
        //        return 0;
        //    }
        //    catch (Exception ex)
        //    {
        //        int insertLog = insertLogJson(sql, ex.Message, "N/A", "N/A", "GetLevel2Consolidation");
        //        return 0;
        //    }
        //}

        #endregion
        #region Collection Level02
        /// <summary>
        /// Metodo que grava a coleta
        /// </summary>
        /// <param name="ConsolidationLevel02Id">Id da Consolidação do Level02</param>
        /// <param name="Level01Id">Id do Level01</param>
        /// <param name="Level02Id">Id do Level02</param>
        /// <param name="UnitId">Id da Unidade</param>
        /// <param name="AuditorId">Id do Auditor</param>
        /// <param name="Shift">Shift</param>
        /// <param name="Period">Period</param>
        /// <param name="Phase">Phase</param>
        /// <param name="Reaudit">Se o Level02 é uma reauidtoria ou não</param>
        /// <param name="ReauditNumber">Número da Reauditoria</param>
        /// <param name="CollectionDate">Data da Coleta</param>
        /// <param name="StartPhase">Data que inicio a phase se estiver em uma phase</param>
        /// <param name="Evaluation">Número da Avaliação</param>
        /// <param name="Sample">Número da Amostra</param>
        /// <param name="CatteType"></param>
        /// <param name="ChainSpeed"></param>
        /// <param name="ConsecuticeFalireIs">Se é uma falha consecutiva</param>
        /// <param name="ConsecutiveFailureTotal">Total de falhas consecutivas no level02</param>
        /// <param name="LotNumber"></param>
        /// <param name="MudScore"></param>
        /// <param name="NotEvaluateIs">Não avaliado</param>
        /// <param name="Duplicated">Item duplicado</param>
        /// <param name="haveReaudit">Se tem reauditoria</param>
        /// <param name="haveCorrectiveAction">Se tem corrective action</param>
        /// <param name="HavePhase">Se tem phase</param>
        /// <param name="Completed">Se o level01 está completo(todos os level02 dentro do level01 estão completos)</param>
        /// <param name="id">Id da Coleta</param>
        /// <returns></returns>
        public int InsertCollectionLevel2(SGQDBContext.ConsolidationLevel1 ConsolidationLevel1, SGQDBContext.ConsolidationLevel2 ConsolidationLevel2, int AuditorId, int Shift, 
                                          int Period, string Phase, bool Reaudit, int ReauditNumber, DateTime CollectionDate,
                                           string StartPhase, int Evaluation, int Sample, string ConsecuticeFalireIs, string ConsecutiveFailureTotal, string NotEvaluateIs,
                                           string Duplicated, string haveReaudit, string haveCorrectiveAction, string HavePhase, string Completed, string id, string AlertLevel,
                                           string sequential, string side, string WeiEvaluation, string Defects, string WeiDefects, string TotalLevel3WithDefects, string totalLevel3evaluation,
                                           string avaliacaoultimoalerta, string monitoramentoultimoalerta, string evaluatedresult, string defectsresult, string isemptylevel3, string hashKey=null)
        {
            //Converte a data da coleta
            string sql = null;
            //Se o Id for igual a zero é um insert

            string key = ConsolidationLevel2.UnitId.ToString();

            key += "-" + ConsolidationLevel1.ParLevel1_Id.ToString();
            key += "-" + ConsolidationLevel2.ParLevel2_Id.ToString();
            if(hashKey == "1")
            {
                key += "-" + sequential;
                key += "-" + side;
            }
            else
            {
                key += "-" + Evaluation;
                key += "-" + Sample;
            }
            key += "-" + CollectionDate.ToString("yyyyMMdd");

            if (id == "0")
            {
                sql = "INSERT INTO CollectionLevel2 ([Key],[ConsolidationLevel2_Id],[ParLevel1_Id],[ParLevel2_Id],[UnitId],[AuditorId],[Shift],[Period],[Phase],[ReauditIs],[ReauditNumber],[CollectionDate],[StartPhaseDate],[EvaluationNumber],[Sample],[AddDate],[AlterDate],[ConsecutiveFailureIs],[ConsecutiveFailureTotal],[NotEvaluatedIs],[Duplicated],[HaveReaudit], [HaveCorrectiveAction],[HavePhase],[Completed],[AlertLevel],[Sequential],[Side],[WeiEvaluation],[Defects],[WeiDefects],[TotalLevel3WithDefects], [TotalLevel3Evaluation], [LastEvaluationAlert],[LastLevel2Alert],[EvaluatedResult],[DefectsResult],[IsEmptyLevel3]) " +
                "VALUES " +
                "('" + key + "', '" + ConsolidationLevel2.Id + "','" + ConsolidationLevel1.ParLevel1_Id + "','" + ConsolidationLevel2.ParLevel2_Id + "','" + ConsolidationLevel1.UnitId + "','" + AuditorId + "','" + Shift + "','" + Period + "','" + Phase + "','" + BoolConverter(Reaudit.ToString()) + "','" + ReauditNumber + "', CAST(N'" + CollectionDate.ToString("yyyy-MM-dd HH:mm:ss") + "' AS DateTime), " + StartPhase + ",'" + Evaluation + "','" + Sample + "',GETDATE(),NULL,'" + ConsecuticeFalireIs + "','" + ConsecutiveFailureTotal + "','" + NotEvaluateIs + "','" + Duplicated + "', '" + haveReaudit + "', '" + haveCorrectiveAction + "', '" + HavePhase + "', '" + Completed + "', '" + AlertLevel + "', '" + sequential + "', '" + side + "','" + WeiEvaluation + "','" + Defects + "','" + WeiDefects + "','" + TotalLevel3WithDefects + "', '" + totalLevel3evaluation + "', '" + avaliacaoultimoalerta + "', '" + monitoramentoultimoalerta + "', '" + evaluatedresult + "', '" + defectsresult + "', '" + isemptylevel3 + "') ";

                sql += " SELECT @@IDENTITY AS 'Identity' ";
            }
            else
            {
                ///podemos melhorar a verificação para Id zero, id null e id not null
                //Caso contrário  é u Update
                sql = "UPDATE CollectionLevel2 SET NotEvaluatedIs='" + NotEvaluateIs + "', AlterDate='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', HaveReaudit='" + haveReaudit + "', HaveCorrectiveAction='" + haveCorrectiveAction + "', WeiEvaluation=" + WeiEvaluation + ", Defects=" + defectsresult + ", WeiDefects=" + WeiDefects + ", TotalLevel3WithDefects=" +  TotalLevel3WithDefects + ", TotalLevel3Evaluation=" + totalLevel3evaluation + ", LastEvaluationAlert=" + avaliacaoultimoalerta + ", EvaluatedResult=" + evaluatedresult + ", DefectsResult=" +defectsresult + ", IsEmptyLevel3=" + isemptylevel3 + " WHERE Id='" + id + "'";

                sql += " SELECT '" + id + "' AS 'Identity'";
            }

            string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        var i = Convert.ToInt32(command.ExecuteScalar());
                        //Se o script for executado corretamente retorna o Id
                        if (i > 0)
                        {
                            return i;
                        }
                        else
                        {
                            //Se o script não for executado corretamente, retorna zero
                            return 0;
                        }
                    }
                }
            }
            //Caso ocorra alguma exception, grava no log e retorna zero
            catch (SqlException ex)
            {
                if (ex.Number == 2627)
                {
                   if(hashKey == "1")
                    {
                        var CollectionLevel2DB = new SGQDBContext.CollectionLevel2();
                        var collectionLevel2 = CollectionLevel2DB.GetByKey(key);

                        var updateLevel2Id = InsertCollectionLevel2(ConsolidationLevel1, ConsolidationLevel2, AuditorId, Shift, Period, Phase, Reaudit, ReauditNumber, CollectionDate, StartPhase, Evaluation, Sample, ConsecuticeFalireIs, ConsecutiveFailureTotal, NotEvaluateIs, Duplicated, haveReaudit, haveCorrectiveAction, HavePhase, Completed, collectionLevel2.Id.ToString(), AlertLevel, sequential, side, WeiEvaluation, Defects, WeiDefects, TotalLevel3WithDefects, totalLevel3evaluation, avaliacaoultimoalerta, monitoramentoultimoalerta, evaluatedresult, defectsresult, isemptylevel3, hashKey);
                        if(updateLevel2Id > 0)
                        {
                            int removeLevel3 = ResultLevel3Delete(collectionLevel2.Id);
                            return updateLevel2Id;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                    else
                    {
                        int insertLog = insertLogJson(sql, ex.Message, "N/A", "N/A", "InsertCollectionLevel2");
                        return 0;
                    }
                }
                throw ex;
            }
            catch (Exception ex)
            {
                int insertLog = insertLogJson(sql, ex.Message, "N/A", "N/A", "InsertCollectionLevel2");
                throw ex;
            }
        }
        public int ResultLevel3Delete(int CollectionLevel2_Id)
        {

            string sql = "DELETE FROM Result_Level3 WHERE CollectionLevel2_Id=" + CollectionLevel2_Id;
            string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        var i = Convert.ToInt32(command.ExecuteNonQuery());
                        if (i > 0)
                        {
                            return i;
                        }
                        else
                        {
                            return 0;
                        }

                    }
                }
            }
            catch (SqlException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public int InsertCollectionLevel2HeaderField(int CollectionLevel2Id, string headerList)
        {

            string sql = null;
            string[] arrayHeaderList = headerList.Split(';');
            for (int i = 0; i < arrayHeaderList.Length; i++)
            {
                var header = arrayHeaderList[i].Split(',');

                string ParHeaderField_Id = header[0];
                string ParFieldType_Id = header[1];
                string Value = header[2];

                sql += "INSERT INTO[dbo].[CollectionLevel2XParHeaderField]               " +
                         "      ([CollectionLevel2_Id]                                     " +
                         "      ,[ParHeaderField_Id]                                       " +
                         "      ,[ParHeaderField_Name]                                     " +
                         "      ,[ParFieldType_Id]                                         " +
                         "      ,[Value])                                                  " +
                         "VALUES                                                           " +
                         "      ('" + CollectionLevel2Id + "'                              " +
                         "      ," + ParHeaderField_Id + "                                     " +
                         "      ,(SELECT Name FROM ParHeaderField WHERE Id='" + ParHeaderField_Id + "')   " +
                         "      ,'" + ParFieldType_Id + "'                                  " +
                         "      ,'" + Value + "')                                           ";
            }





            string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        var i = Convert.ToInt32(command.ExecuteNonQuery());
                        //Se o script for executado corretamente retorna o Id
                        if (i > 0)
                        {
                            return i;
                        }
                        else
                        {
                            //Se o script não for executado corretamente, retorna zero
                            return 0;
                        }

                    }
                }
            }
            //Caso ocorra alguma exception, grava no log e retorna zero
            catch (SqlException ex)
            {
                int insertLog = insertLogJson(sql, ex.Message, "N/A", "N/A", "InsertCollectionLevel2HeaderField");
                if (ex.Number == 2627) // <-- but this will
                {
                    return 0;
                }
                throw ex;
            }
            catch (Exception ex)
            {
                int insertLog = insertLogJson(sql, ex.Message, "N/A", "N/A", "InsertCollectionLevel2HeaderField");
                throw ex;
            }
        }

        #endregion
        #region Collection Level03
        /// <summary>
        /// Metodo que insere o CollectionLevel03
        /// </summary>
        /// <param name="CollectionLevel02Id">Id do CollectionLevel02</param>
        /// <param name="level02">Id do Level02</param>
        /// <param name="level03Results">Resultados do Level03</param>
        /// <param name="auditorId">Id do Auditor</param>
        /// <param name="duplicated">Duplicado</param>
        /// <returns></returns>
        public int InsertCollectionLevel3(string CollectionLevel02Id, int level02, string level03Results, int auditorId, string duplicated)
        {
            ///coloquei uma @ para replace, mas podemos utilizar o padrão de ; ou <> desde que todos os campos venha do script com escape()
            //string obj, string collectionDate, string level01id, string unit, string period, string shift, string device, string version

            //Prepara a string para ser convertida em Array
            level03Results = level03Results.Replace("</level03><level03>", "@").Replace("<level03>", "").Replace("</level03>", "");
            //Gera o Array
            string[] arrayResults = level03Results.Split('@');
            //"trocar o virgula do value text";

            string sql = null;

            //Lista de Level3

            var ParLevel3DB = new SGQDBContext.ParLevel3();
            var parLevel3List = ParLevel3DB.getList();

            //Percorre o Array para gerar os inserts
            for (int i = 0; i < arrayResults.Length; i++)
            {
                //Gera o array com o resultado
                var result = arrayResults[i].Split(',');

                //Instancia as variáveis para preencher o script
                string Level03Id = result[0];
                string value = result[2];
                value = DefaultValueReturn(value, "0");

                string conform = result[3];
                conform = BoolConverter(conform);

                string valueText = result[6];
                if (string.IsNullOrEmpty(valueText))
                {
                    valueText = "undefined";
                }
                string id = result[7];

                string weight = result[8];
                weight = DefaultValueReturn(weight, "1");

                string name = result[9];

                string intervalMin = result[10];
                intervalMin = DefaultValueReturn(intervalMin, "0");

                string intervalMax = result[11];
                intervalMax = DefaultValueReturn(intervalMax, "0");

                string isnotEvaluate = result[12];
                isnotEvaluate = DefaultValueReturn(isnotEvaluate, "0");

                isnotEvaluate = BoolConverter(isnotEvaluate);

                string punishimentValue = result[13];
                punishimentValue = DefaultValueReturn(punishimentValue, "0");

                string defects = result[14];

                //aqui tem que mudar no bem estar animal, verificar com o gabriel
                string evaluation = "1";

                string WeiEvaluation = result[15].Replace(",", ".");
                string WeiDefects = result[16].Replace(",", ".");

                //decimal defeitos = Convert.ToDecimal(defects.ToString().Replace(".", ","));
                //decimal punicao = Convert.ToDecimal(punishimentValue.ToString().Replace(".", ","));
                //decimal peso = Convert.ToDecimal(weight.ToString().Replace(".", ","));

                //WeiDefects = (defeitos + punicao) * peso;

                id = DefaultValueReturn(id, "0");

                if (id == "0")
                {
                    sql += "INSERT INTO Result_Level3 ([CollectionLevel2_Id],[ParLevel3_Id],[ParLevel3_Name],[Weight],[IntervalMin],[IntervalMax],[Value],[ValueText],[IsConform],[IsNotEvaluate],[PunishmentValue],[Defects],[Evaluation],[WeiEvaluation],[WeiDefects]) " +
                           "VALUES " +
                           "('" + CollectionLevel02Id + "','" + Level03Id + "', '" + parLevel3List.Where(p => p.Id == Convert.ToInt32(Level03Id)).FirstOrDefault().Name + "'," + weight + "," + intervalMin + "," + intervalMax + ", " + value + ",'" + valueText + "','" + conform + "','" + isnotEvaluate + "', " + punishimentValue + ", " + defects + ", " + evaluation + ", " + WeiEvaluation + ", " + WeiDefects + ") ";

                    sql += " SELECT @@IDENTITY AS 'Identity'";

                }
                else
                {
                    sql += "UPDATE Result_Level3 SET IsConform='" + conform + "', IsNotEvaluate='" + isnotEvaluate + "', Value='" + value + "', ValueText='" + valueText + "' WHERE Id='" + id + "' ";
                    sql += " SELECT '" + id + "' AS 'Identity'";

                }

            }

            string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        var i = Convert.ToInt32(command.ExecuteScalar());
                        //Se o script foi executado, retorna o Id
                        if (i > 0)
                        {
                            return i;
                        }
                        else
                        {
                            //Caso ocorra algum erro, retorna zero
                            return 0;
                        }

                    }
                }
            }
            //Caso ocorra Exception, insere no banco e retorna zero
            catch (SqlException ex)
            {
                int insertLog = insertLogJson(sql, ex.Message, "N/A", "N/A", "InsertCollectionLevel03");
                return 0;
            }
            catch (Exception ex)
            {
                int insertLog = insertLogJson(sql, ex.Message, "N/A", "N/A", "InsertCollectionLevel03");
                return 0;
            }
        }

        #endregion
        #region Corrective Action
        /// <summary>
        /// Metodo para gravar ação corretiva
        /// </summary>
        /// <param name="AuditorId">Id do Auditor</param>
        /// <param name="CollectionLevel02Id">Id da Coleta</param>
        /// <param name="SlaughterId">Id assinatura Slaughter</param>
        /// <param name="TechinicalId">Id assinatura Techinical</param>
        /// <param name="DateTimeSlaughter">Data assinatura Slaughter</param>
        /// <param name="DateTimeTechinical">Data assinatura Techinical</param>
        /// <param name="DateCorrectiveAction">Data da ação corretiva</param>
        /// <param name="AuditStartTime">Date que a auditoria iniciou</param>
        /// <param name="DescriptionFailure"></param>
        /// <param name="ImmediateCorrectiveAction"></param>
        /// <param name="ProductDisposition"></param>
        /// <param name="PreventativeMeasure"></param>
        /// <returns></returns>
        public int correctiveActionInsert(string AuditorId, string CollectionLevel02Id, string SlaughterId, string TechinicalId, string DateTimeSlaughter, string DateTimeTechinical, string DateCorrectiveAction, string AuditStartTime, string DescriptionFailure, string ImmediateCorrectiveAction, string ProductDisposition, string PreventativeMeasure)
        {
            //Conversão das datas
            DateTime SlaughterDateTime = DateCollectConvert(DateTimeSlaughter);
            DateTimeSlaughter = SlaughterDateTime.ToString("yyyy-MM-dd HH:mm:ss");

            DateTime TechinicalDateTime = DateCollectConvert(DateTimeTechinical);
            DateTimeTechinical = TechinicalDateTime.ToString("yyyy-MM-dd HH:mm:ss");

            //DateTime CorrectiveActionDate = DateCollectConvert(DateCorrectiveAction);
            //DateCorrectiveAction = CorrectiveActionDate.ToString("yyyy-MM-dd HH:mm:ss");

            //DateTime StartTimeAudit = DateCollectConvert(AuditStartTime);
            //AuditStartTime = StartTimeAudit.ToString("yyyy-MM-dd HH:mm:ss");

            //Script de Insert
            string sql = "INSERT INTO CorrectiveAction ([AuditorId],[CollectionLevel02Id],[SlaughterId],[TechinicalId],[DateTimeSlaughter],[DateTimeTechinical],[AddDate],[AlterDate],[DateCorrectiveAction],[AuditStartTime],[DescriptionFailure],[ImmediateCorrectiveAction],[ProductDisposition],[PreventativeMeasure]) " +
                         "VALUES " +
                         "('" + AuditorId + "','" + CollectionLevel02Id + "','" + SlaughterId + "','" + TechinicalId + "',CAST(N'" + DateTimeSlaughter + "' AS DateTime),CAST(N'" + DateTimeTechinical + "' AS DateTime),GETDATE(),NULL,CAST(N'" + DateCorrectiveAction + "' AS DateTime),CAST(N'" + AuditStartTime + "' AS DateTime),'" + DescriptionFailure + "','" + ImmediateCorrectiveAction + "','" + ProductDisposition + "','" + PreventativeMeasure + "')";
            string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        var i = Convert.ToInt32(command.ExecuteNonQuery());
                        //Se o script foi executado retona o Id
                        if (i > 0)
                        {
                            return i;
                        }
                        else
                        {
                            //Caso o script não execute, retorna zeros
                            return 0;
                        }
                    }
                }
            }
            //Em caso de Exception, gera um log no banco e retorna zero
            catch (SqlException ex)
            {
                int insertLog = insertLogJson(sql, ex.Message, "N/A", "N/A", "correctiveActionInsert");
                throw ex;
            }
            catch (Exception ex)
            {
                int insertLog = insertLogJson(sql, ex.Message, "N/A", "N/A", "correctiveActionInsert");
                throw ex;
            }
        }
        #endregion
        #region DataBase HTML
        /// <summary>
        /// Metodo que retorna a ultima data de consolidação a anterior a data informada 
        /// </summary>
        /// <returns></returns>
        public string GetMaxDateCollection(DateTime date)
        {
            string sql = "SELECT TOP 1 ConsolidationDate FROM ConsolidationLevel01 WHERE ConsolidationDate < '" + date.ToString("yyyyMMdd") + "' ORDER BY ConsolidationDate DESC";

            string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        using (SqlDataReader r = command.ExecuteReader())
                        {
                            DateTime UltimaDataColeta = date;
                            if (r.Read())
                            {
                                UltimaDataColeta = Convert.ToDateTime(r[0]);
                            }
                            return UltimaDataColeta.ToString("yyyyMMdd");
                        }
                    }
                }
            }
            //Em caso de erro, gera um exception retorna null
            catch (SqlException ex)
            {
                int insertLog = insertLogJson(sql, ex.Message, "N/A", "N/A", "GetMaxDateCollection");
                return null;
            }
            catch (Exception ex)
            {
                int insertLog = insertLogJson(sql, ex.Message, "N/A", "N/A", "GetMaxDateCollection");
                return null;
            }
        }
        /// <summary>
        /// Metodo que para chamar o recebimento de dados
        /// </summary>
        /// <param name="unidadeId"></param>
        /// <returns></returns>
        [WebMethod]
        public string reciveData(string unidadeId, string data)
        {
            DateTime dataConsolidation = DateCollectConvert(data);
            string consolidation = getConsolidation(unidadeId, dataConsolidation);
            return consolidation;
        }
        [WebMethod]
        public string reciveDataByLevel1(string ParCompany_Id, string data, string ParLevel1_Id)
        {
            DateTime dataConsolidation = DateCollectConvert(data);
            string consolidation = getConsolidation(ParCompany_Id, dataConsolidation, Convert.ToInt32(ParLevel1_Id));
            return consolidation;
        }
        /// <summary>
        /// Metodo que verifica as consolidações necessárias
        /// </summary>
        /// <param name="unidadeId">Id da Unidade</param>
        /// <param name="lastDate">Se False, traz o resultado do dia atual somente, se True, traz o ultimo resultado sem o dia atual</param>
        /// <returns></returns>
        //public string GetConsolidationLevel01(string unidadeId, bool lastDate = false)
        //{

        //    string atualCollectionDate = DateTime.Now.ToString("yyyyMMdd");
        //    string collectionDate = atualCollectionDate;

        //    if (lastDate == true)
        //    {
        //        collectionDate = GetMaxDateCollection(DateTime.Now);
        //        atualCollectionDate = collectionDate;
        //    }

        //    string sql = "SELECT Id, ParLevel1_Id, ConsolidationDate FROM ConsolidationLevel1 WHERE ConsolidationDate BETWEEN '" + collectionDate + " 00:00:00' AND '" + atualCollectionDate + " 23:59:59' GROUP BY Id, ParLevel1_Id, ConsolidationDate";

        //    string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;
        //    try
        //    {
        //        using (SqlConnection connection = new SqlConnection(conexao))
        //        {
        //            using (SqlCommand command = new SqlCommand(sql, connection))
        //            {
        //                connection.Open();
        //                using (SqlDataReader r = command.ExecuteReader())
        //                {
        //                    //Lista de Level01
        //                    string Results = null;

        //                    while (r.Read())
        //                    {
        //                        Results += GetColletionlevel2()
        //                    }
        //                    return Results;
        //                }
        //            }
        //        }
        //    }
        //    catch (SqlException ex)
        //    {
        //        int insertLog = insertLogJson(sql, ex.Message, "N/A", "N/A", "GetConsolidationLevel01");
        //        //return null;
        //        return "error";

        //    }
        //    catch (Exception ex)
        //    {
        //        int insertLog = insertLogJson(sql, ex.Message, "N/A", "N/A", "GetConsolidationLevel01");
        //        //return null;
        //        return "error";
        //    }
        //}
        public void getFrequencyDate(int ParFrequency_Id, DateTime data, ref string dataInicio, ref string dataFim)
        {

            DateTime periodoInicio = data;
            DateTime periodoFim = data;

            switch (ParFrequency_Id)
            {
                case 1:
                    break;
                case 2:
                    break;
                case 3: //diario
                    periodoInicio = data;
                    periodoFim = periodoInicio;
                    break;
                case 4: //semanal
                        //Variáveis de controle dos dias.
                    int numeroMenor = 1, numeroMaior = 7;
                    periodoInicio = data.AddDays(numeroMenor - data.DayOfWeek.GetHashCode());
                    periodoFim = data.AddDays(numeroMaior - data.DayOfWeek.GetHashCode());
                    break;
                case 5: //quinzenal
                    periodoInicio = new DateTime(data.Year, data.Month, 1);
                    periodoFim = new DateTime(data.Year, data.Month, 15);
                    if (DateTime.Now.Day > 15)
                    {
                        periodoFim = periodoInicio.AddMonths(1).AddDays(-1);
                        periodoInicio = new DateTime(data.Year, data.Month, 16);
                    }
                    break;
                case 6:
                    periodoInicio = new DateTime(data.Year, data.Month, 1);
                    periodoFim = periodoInicio.AddMonths(1).AddDays(-1);

                    break;
                default:
                    break;
            }

            dataInicio = periodoInicio.ToString("yyyyMMdd");
            dataFim = periodoFim.ToString("yyyyMMdd");

        }
        public string getConsolidation(string ParCompany_Id, DateTime data, int ParLevel1_Id=0)
        {

            //Verificamos os Indicadores que já foram consolidados para a Unidade selecionada
            var ParLevel1ConsolidationXParFrequencyDB = new SGQDBContext.ParLevel1ConsolidationXParFrequency();
            //Instanciamos uma variável que irá 
            var parLevel1ConsolidationXParFrequency = ParLevel1ConsolidationXParFrequencyDB.getList(Convert.ToInt32(ParCompany_Id), data);

            string Results = null;

            if(ParLevel1_Id > 0)
            {
                parLevel1ConsolidationXParFrequency = parLevel1ConsolidationXParFrequency.Where(p => p.ParLevel1_Id == ParLevel1_Id);
            }

            //Percorremos as consolidações de ParLevel1
            foreach (var c in parLevel1ConsolidationXParFrequency)
            {
                //Instanciamos variavel de data
                string dataInicio = null;
                string dataFim = null;

                //Pega a data pela regra da frequencia
                getFrequencyDate(c.ParFrequency_Id, data, ref dataInicio, ref dataFim);

                //Instanciamos a tabela Resultados
                var Level2ResultDB = new SGQDBContext.Level2Result();
                var Level2ResultList = Level2ResultDB.getList(c.ParLevel1_Id, Convert.ToInt32(ParCompany_Id), dataInicio, dataFim);

                //Percorremos os resultados do indicador
                foreach (var Level2Result in Level2ResultList)
                {
                    //Verificamos a consolidação
                    var ConsolidationResultL1L2DB = new SGQDBContext.ConsolidationResultL1L2();
                    var consolidationResultL1L2 = ConsolidationResultL1L2DB.getConsolidation(Level2Result.ParLevel2_Id, Level2Result.Unit_Id, c.Id);

                    var lastSampleConsolidation = Level2ResultDB.getMaxSampe(Level2Result.ConsolidationLevel2_Id, Level2Result.EvaluateLast);

                    Level2Result.SampleLast = lastSampleConsolidation;

                    string partialResults = null;
                    if(c.IsPartialSave ==  true)
                    {

                        var ParLevel1DB = new SGQDBContext.ParLevel1();
                        var parLevel1 = ParLevel1DB.getById(Level2Result.ParLevel1_Id);

                        var ParLevel2DB = new SGQDBContext.ParLevel2();
                        var parLevel2 = ParLevel2DB.getById(Level2Result.ParLevel2_Id);

                        var ParLevel3DB = new SGQDBContext.ParLevel3();
                        var parLevel3InLevel2List = ParLevel3DB.getLevel3InLevel2(parLevel1, parLevel2, Convert.ToInt32(ParCompany_Id), data);
 
                        foreach (var l3 in parLevel3InLevel2List)
                        {
                            partialResults += "<div id=\"" + l3.Id + "\" class=\"r3l2\"></div>";
                        }

                    }

                    // Results += "<div class=\"Resultlevel2\" AlertLevelL1=\"" + consolidationResultL1L2.AlertLevelL1 + "\" WeiEvaluationL1=\"" + consolidationResultL1L2.WeiEvaluationL1 + "\" EvaluateTotalL1=\"" + consolidationResultL1L2.EvaluateTotalL1 + "\" DefectsTotalL1=\"" + consolidationResultL1L2.DefectsTotalL1 + "\" WeiDefectsL1=\"" + consolidationResultL1L2.WeiDefectsL1 + "\" TotalLevel3EvaluationL1=\"" + consolidationResultL1L2.TotalLevel3EvaluationL1 + "\" TotalLevel3WithDefectsL1=\"" + consolidationResultL1L2.TotalLevel3WithDefectsL1 + "\" LastEvaluationAlertL1=\"" + consolidationResultL1L2.LastEvaluationAlertL1 + "\" EvaluatedResultL1=\"" + consolidationResultL1L2.EvaluatedResultL1 + "\" DefectsResultL1=\"" + consolidationResultL1L2.DefectsResultL1 + "\"  EvaluateTotalL2=\"" + consolidationResultL1L2.EvaluateTotalL2 + "\" DefectsTotalL2=\"" + consolidationResultL1L2.DefectsTotalL2 + "\" WeiEvaluationL2=\"" + consolidationResultL1L2.WeiEvaluationL2 + "\"  DefectsL2=\"" + consolidationResultL1L2.DefectsL2 + "\" WeiDefectsL2=\"" + consolidationResultL1L2.WeiDefectsL2 + "\" TotalLevel3WithDefectsL2=\"" + consolidationResultL1L2.TotalLevel3WithDefectsL2 + "\" TotalLevel3EvaluationL2=\"" + consolidationResultL1L2.TotalLevel3EvaluationL2 + "\" EvaluatedResultL2=\"" + consolidationResultL1L2.EvaluateTotalL2 + "\" DefectsResultL2=\"" + consolidationResultL1L2.DefectsResultL2 + "\" Level1Id=\"" + Level2Result.ParLevel1_Id + "\" Level2Id=\"" + Level2Result.ParLevel2_Id + "\" UnitId=\"" + Level2Result.Unit_Id + "\" Shift=\"" + Level2Result.Shift + "\" Period=\"" + Level2Result.Period + "\" CollectionDate=\"" + Level2Result.CollectionDate.ToString("MMddyyyy") + "\" Evaluation=\"" + Level2Result.EvaluateLast + "\" Sample=\"" + Level2Result.SampleLast + "\" havecorrectiveaction=\"false\" CollectionLevel2_ID_CorrectiveAction=\"" + consolidationResultL1L2.CollectionLevel2_ID_CorrectiveAction + "\"></div>";
                    Results += "<div class=\"Resultlevel2\" AlertLevelL1=\"" + consolidationResultL1L2.AlertLevelL1 + "\" WeiEvaluationL1=\"" + consolidationResultL1L2.WeiEvaluationL1 + "\" EvaluateTotalL1=\"" + consolidationResultL1L2.EvaluateTotalL1 + "\" DefectsTotalL1=\"" + consolidationResultL1L2.DefectsTotalL1 + "\" WeiDefectsL1=\"" + consolidationResultL1L2.WeiDefectsL1 + "\" TotalLevel3EvaluationL1=\"" + consolidationResultL1L2.TotalLevel3EvaluationL1 + "\" TotalLevel3WithDefectsL1=\"" + consolidationResultL1L2.TotalLevel3WithDefectsL1 + "\" LastEvaluationAlertL1=\"" + consolidationResultL1L2.LastEvaluationAlertL1 + "\" LastLevel2AlertL1=\"" + consolidationResultL1L2.LastLevel2AlertL1 + "\" EvaluatedResultL1=\"" + consolidationResultL1L2.EvaluatedResultL1 + "\" DefectsResultL1=\"" + consolidationResultL1L2.DefectsResultL1 + "\"  EvaluateTotalL2=\"" + consolidationResultL1L2.EvaluateTotalL2 + "\" DefectsTotalL2=\"" + consolidationResultL1L2.DefectsTotalL2 + "\" WeiEvaluationL2=\"" + consolidationResultL1L2.WeiEvaluationL2 + "\"  DefectsL2=\"" + consolidationResultL1L2.DefectsL2 + "\" WeiDefectsL2=\"" + consolidationResultL1L2.WeiDefectsL2 + "\" TotalLevel3WithDefectsL2=\"" + consolidationResultL1L2.TotalLevel3WithDefectsL2 + "\" TotalLevel3EvaluationL2=\"" + consolidationResultL1L2.TotalLevel3EvaluationL2 + "\" EvaluatedResultL2=\"" + consolidationResultL1L2.EvaluateTotalL2 + "\" DefectsResultL2=\"" + consolidationResultL1L2.DefectsResultL2 + "\" Level1Id=\"" + Level2Result.ParLevel1_Id + "\" Level2Id=\"" + Level2Result.ParLevel2_Id + "\" UnitId=\"" + Level2Result.Unit_Id + "\" Shift=\"" + Level2Result.Shift + "\" Period=\"" + Level2Result.Period + "\" CollectionDate=\"" + Level2Result.CollectionDate.ToString("MMddyyyy") + "\" Evaluation=\"" + Level2Result.EvaluateLast + "\" Sample=\"" + Level2Result.SampleLast + "\" havecorrectiveaction=\"" + consolidationResultL1L2.haveCorrectiveAction.ToString().ToLower() + "\" CollectionLevel2_ID_CorrectiveAction=\"" + consolidationResultL1L2.CollectionLevel2_ID_CorrectiveAction + "\" CollectionLevel2_Period_CorrectiveAction=\"" + consolidationResultL1L2.CollectionLevel2_Period_CorrectiveAction + "\">" +
                                partialResults +
                               "</div>";
                }
            }
            return Results;
        }
        //public string GetConsolidationLevel01(string unidadeId, bool lastDate = false)
        //{

        //    string atualCollectionDate = DateTime.Now.ToString("yyyyMMdd");
        //    string collectionDate = atualCollectionDate;

        //    if (lastDate == true)
        //    {
        //        collectionDate = GetMaxDateCollection(DateTime.Now);
        //        atualCollectionDate = collectionDate;
        //    }

        //    string sql = "SELECT Id, ParLevel1_Id, ConsolidationDate FROM ConsolidationLevel1 WHERE ConsolidationDate BETWEEN '" + collectionDate + " 00:00:00' AND '" + atualCollectionDate + " 23:59:59' GROUP BY Id, ParLevel1_Id, ConsolidationDate";

        //    string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;
        //    try
        //    {
        //        using (SqlConnection connection = new SqlConnection(conexao))
        //        {
        //            using (SqlCommand command = new SqlCommand(sql, connection))
        //            {
        //                connection.Open();
        //                using (SqlDataReader r = command.ExecuteReader())
        //                {
        //                    //Lista de Level01
        //                    string level01Results = null;

        //                    while (r.Read())
        //                    {
        //                        //Instancia variáveis de verificação 
        //                        bool completed = false;
        //                        bool havephases = false;
        //                        bool havereaudit = false;
        //                        bool havecorrectiveaction = false;

        //                        string Id = r[0].ToString();
        //                        string Level01Id = r[1].ToString();
        //                        DateTime ConsolidationDate = Convert.ToDateTime(r[2]);

        //                        string datetime = null;
        //                        string shift = null;
        //                        string period = null;
        //                        string reaudit = null;
        //                        string reauditNumber = null;

        //                        string lastevaluate = null;
        //                        int lastsample = 0;

        //                        int evaluate = 0;
        //                        int sample = 0;
        //                        var Level02ResultList = new List<Level02Result>();

        //                        GetConsolidationLevel02(Id, Level01Id, unidadeId, ref Level02ResultList);

        //                        var periods = (from p in Level02ResultList
        //                                       group p by p.period into g
        //                                       select new { period = g.Key }).ToList();

        //                        foreach (var p in periods)
        //                        {
        //                            string Level01ResultByPeruid = null;
        //                            var Level02ResultByPeriod = Level02ResultList.Where(pe => pe.period == p.period).GroupBy(s => s.reaudit).ToList();

        //                            foreach (var l2p in Level02ResultByPeriod)
        //                            {
        //                                string level02Results = null;
        //                                int More3Errors = 0;
        //                                int SideWithErrors = 0;
        //                                int baisedUnbaised = 0;
        //                                string biasedunbiasedtag = null;

        //                                string haveReauditTag = null;
        //                                string haveCorrectiveActionTag = null;
        //                                string completedSampleTag = null;
        //                                string completedTag = null;
        //                                //Valor Mockado deve ser alterado pela configuração do ParLevel02
        //                                int totalEvaluate = 1;
        //                                int totalSample = 1;

        //                                if (Level01Id == "3")
        //                                {
        //                                    totalEvaluate = 5;
        //                                    totalSample = 10;
        //                                }

        //                                foreach (var rs in l2p)
        //                                {
        //                                    evaluate = rs.evaluate;
        //                                    totalEvaluate = evaluate;

        //                                    sample = rs.sample;
        //                                    lastsample = rs.sample;

        //                                    completed = rs.completed;
        //                                    shift = rs.shift.ToString();

        //                                    havephases = rs.havePhases;
        //                                    havereaudit = rs.haveReaudit;
        //                                    havecorrectiveaction = rs.haveCorrectiveAction;
        //                                    if (completed == true && havereaudit == true)
        //                                    {
        //                                        haveReauditTag = " havereaudit=\"havereaudit\"";
        //                                    }
        //                                    if (completed == true && havecorrectiveaction == true)
        //                                    {
        //                                        haveCorrectiveActionTag = " havecorrectiveaction=\"havecorrectiveaction\"";
        //                                    }
        //                                    if (totalEvaluate > 1 && totalSample == sample)
        //                                    {
        //                                        completedSampleTag = " completedsample=\"completedsample\"";
        //                                    }
        //                                    if (completed == true && evaluate == totalEvaluate && sample == totalSample)
        //                                    {
        //                                        completedTag = " completed=\"completed\"";
        //                                    }
        //                                    datetime = collectionDate;
        //                                    lastevaluate = rs.evaluate.ToString();
        //                                    reaudit = rs.reaudit.ToString().ToLower();
        //                                    reauditNumber = rs.reauditNumber.ToString();
        //                                    level02Results += rs.result;
        //                                    //alterar para verificar por parametro
        //                                    if (rs.defects > 0 && Level01Id == "3")
        //                                    {
        //                                        SideWithErrors++;
        //                                        if (rs.defects >= 3)
        //                                        {
        //                                            More3Errors++;
        //                                        }
        //                                    }
        //                                }
        //                                if (baisedUnbaised > 0)
        //                                {
        //                                    biasedunbiasedtag = " biasedunbiased=\"" + baisedUnbaised + "\"";
        //                                }
        //                                Level01ResultByPeruid += "<div class=\"level01Result\" level01id=\"" + Level01Id + "\" unidadeid=\"" + unidadeId + "\" date=\"" + ConsolidationDate.ToString("MMddyyyy") + "\" datetime=\"" + ConsolidationDate.ToString("MM/dd/yyyy HH:mm:ss") + "\" shift=\"" + shift + "\" period=\"" + p.period + "\" reaudit=\"" + reaudit + "\" reauditnumber=\"" + reauditNumber + "\" totalevaluate=\"" + totalEvaluate + "\" sidewitherros=\"" + SideWithErrors + "\" more3defects=\"" + More3Errors + "\" lastevaluate=\"" + lastevaluate + "\" lastsample=\"" + lastsample + "\" evaluate=\"" + evaluate + "\" sync=\"true\"" + haveCorrectiveActionTag + haveReauditTag + completedSampleTag + biasedunbiasedtag + completedTag + ">" +
        //                                                         level02Results +
        //                                                         "</div>";
        //                            }
        //                            level01Results += Level01ResultByPeruid;
        //                        }
        //                    }
        //                    return level01Results;
        //                }
        //            }
        //        }
        //    }
        //    catch (SqlException ex)
        //    {
        //        int insertLog = insertLogJson(sql, ex.Message, "N/A", "N/A", "GetConsolidationLevel01");
        //        //return null;
        //        return "error";

        //    }
        //    catch (Exception ex)
        //    {
        //        int insertLog = insertLogJson(sql, ex.Message, "N/A", "N/A", "GetConsolidationLevel01");
        //        //return null;
        //        return "error";
        //    }
        //}


        public string getMaxEvaluate(string CollectionLevel02Ids, string Level02Ids)
        {
            string sql = "SELECT MAX([EvaluationNumber]) FROM CollectionLevel02 WHERE ConsolidationLevel02id IN (" + CollectionLevel02Ids + ") AND Level02id IN (" + Level02Ids + ")";

            string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        using (SqlDataReader r = command.ExecuteReader())
                        {
                            string maxEvaluate = "1";
                            if (r.Read())
                            {
                                maxEvaluate = r[0].ToString();
                            }
                            return maxEvaluate;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                int insertLog = insertLogJson(sql, ex.Message, "N/A", "N/A", "getMaxEvaluate");
                return null;
            }
            catch (Exception ex)
            {
                int insertLog = insertLogJson(sql, ex.Message, "N/A", "N/A", "getMaxEvaluate");
                return null;
            }
        }

        //public string GetColletionlevel2(string ConsolidationLevel02Ids, string Level01Id, string Level02Ids, string UnidadeId)
        //{
        //    if (string.IsNullOrEmpty(ConsolidationLevel02Ids) || string.IsNullOrEmpty(Level02Ids))
        //    {
        //        return null;
        //    }

        //    var Level2ResultDB = new SGQDBContext.Level2Result();

        //    var Level2ResultList = Level2ResultDB.getList(UnidadeId);


        //    string Results = null;
        //    foreach (var Level2Result in Level2ResultList)
        //    {
        //        Results += "<div class=\"Resultlevel2\" Level1Id=\"" + Level2Result.ParLevel1_Id + "\" Level2Id=\"" + Level2Result.ParLevel2_Id + "\" UnitId=\"" + Level2Result.Unit_Id + "\" Shift=\"" + Level2Result.Shift + "\" Period=\"" + Level2Result.Period + "\" CollectionDate=\"" + Level2Result.CollectionDate.ToString("MMddyyyy") + "\" Evaluation=\"" + Level2Result.Evaluate + "\" Sample=\"" + Level2Result.Sample + "\"></div>";
        //    }

        //    return Results;
        //    //string maxEvaluate = getMaxEvaluate(ConsolidationLevel02Ids, Level02Ids);
        //    //return null;

        //}


        //public string GetCollectionLevel02(string ConsolidationLevel02Ids, string Level01Id, string Level02Ids, string UnidadeId, ref List<Level02Result> Level02ResultList)
        //{
        //    if (string.IsNullOrEmpty(ConsolidationLevel02Ids) || string.IsNullOrEmpty(Level02Ids))
        //    {
        //        return null;
        //    }
        //    //string maxEvaluate = getMaxEvaluate(ConsolidationLevel02Ids, Level02Ids);
        //    //maxEvaluate = " AND EvaluationNumber = '" + maxEvaluate + "'";
        //    string maxEvaluate = null;                                                                                                                                                  
        //    string sql = "SELECT [Id], [ParLevel2_Id], [AuditorId], [Shift], [Period], [Phase], [ReauditIs], [ReauditNumber], [CollectionDate], [StartPhaseDate], [EvaluationNumber], [Sample], [NotEvaluatedIs], [HaveCorrectiveAction], [HaveReaudit], [HavePhase], [Completed] FROM CollectionLevel2 WHERE ConsolidationLevel2_Id    IN (" + ConsolidationLevel02Ids + ") AND ParLevel1_Id='" + Level01Id + "' AND ParLevel2_Id IN (" + Level02Ids + ") " + maxEvaluate + " AND UnitId='" + UnidadeId + "' AND Duplicated=0";

        //    string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;
        //    try
        //    {
        //        using (SqlConnection connection = new SqlConnection(conexao))
        //        {
        //            using (SqlCommand command = new SqlCommand(sql, connection))
        //            {
        //                connection.Open();
        //                using (SqlDataReader r = command.ExecuteReader())
        //                {
        //                    int i = 0;
        //                    while (r.Read())
        //                    {
        //                        string Id = r[0].ToString();
        //                        string level02id = r[1].ToString();

        //                        string AuditorId = r[2].ToString();
        //                        int shift = Convert.ToInt32(r[3]);
        //                        int period = Convert.ToInt32(r[4]);
        //                        int phase = Convert.ToInt32(r[5]);
        //                        bool reauditIs = Convert.ToBoolean(r[6]);
        //                        int reauditnumber = Convert.ToInt32(r[7]);

        //                        DateTime CollectionDate = Convert.ToDateTime(r[8]);
        //                        string date = CollectionDate.ToString("MMddyyyy");

        //                        string startphasedate = r[9].ToString();
        //                        int evaluate = Convert.ToInt32(r[10]);
        //                        int sample = Convert.ToInt32(r[11]);

        //                        //Estão como int mas tem que abstratir para uma funcao verifica através do banco de dados o tipo que tem que mostrar
        //                        //int cattletype = Convert.ToInt32(r[12]);
        //                        //int baisedUnbaised = 0;
        //                        //if (Level01Id == "1")
        //                        //{
        //                        //    baisedUnbaised = cattletype;
        //                        //}
        //                        //int chainspeed = Convert.ToInt32(r[13]);
        //                        //int lotnumber = Convert.ToInt32(r[14]);
        //                        //int mudscore = Convert.ToInt32(r[15]);


        //                        string notavaliable = r[12].ToString();
        //                        bool haveCorrectiveAction = Convert.ToBoolean(r[13]);
        //                        bool haveReaudit = Convert.ToBoolean(r[14]);
        //                        var havePhases = Convert.ToBoolean(r[15]);
        //                        bool completed = Convert.ToBoolean(r[16]);

        //                        //a date tem que ter o formato brasil e outroa paises que seram utilizado, montar o framewowrk
        //                        //atualizar defects
        //                        int defects = 0;

        //                        string Level03Result = GetCollectionLevel03(Id, date, AuditorId, ref defects);
        //                        string completedReauditTag = null;
        //                        //Procedimento só para HTP, ideal passar para o banco de dados
        //                        if (haveReaudit == false && defects > 0 && Level01Id == "1")
        //                        {
        //                            completedReauditTag = " completereaudit=\"completereaudit\"";
        //                        }

        //                        string haveReauditTag = null;
        //                        if (haveReaudit == true)
        //                        {
        //                            haveReauditTag = " havereaudit=\"havereaudit\"";
        //                        }
        //                        string haveCorrectiveActionTag = null;
        //                        if (haveCorrectiveAction == true)
        //                        {
        //                            haveCorrectiveActionTag = " havecorrectiveaction=\"havecorrectiveaction\"";
        //                        }

        //                        string Level02Result = "<div id=\"" + Id + "\" class=\"level02Result\" level01id=\"" + Level01Id + "\" level02id=\"" + level02id + "\" unidadeid=\"" + UnidadeId + "\" date=\"" + date + "\" datetime=\"" + CollectionDate.ToString("MM/dd/yyyy HH:mm:ss:fff") + "\" auditorid=\"" + AuditorId + "\" shift=\"" + shift + "\" period=\"" + period + "\" defects=\"" + defects + "\" reaudit=\"" + reauditIs.ToString().ToLower() + "\" evaluate=\"" + evaluate + "\" sample=\"" + sample + "\" reauditnumber=\"" + reauditnumber + "\" phase=\"" + phase + "\" startphasedate=\"" + startphasedate + "\"  consecutivefailurelevel=\"undefined\" consecutivefailuretotal=\"undefined\" notavaliable=\"" + notavaliable.ToLower() + "\" sync=\"true\"" + completedReauditTag + haveReauditTag + haveCorrectiveActionTag + ">" +
        //                                               Level03Result +
        //                                               "</div>";

        //                        var l2Result = new Level02Result(unidadeId: UnidadeId,
        //                                                         period: period,
        //                                                         shift: shift,
        //                                                         evaluate: evaluate,
        //                                                         sample: sample,
        //                                                         result: Level02Result,
        //                                                         haveCorrectiveAction: haveCorrectiveAction,
        //                                                         haveReaudit: haveReaudit,
        //                                                         havePhases: havePhases,
        //                                                         reaudit: reauditIs,
        //                                                         reauditNumber: reauditnumber,
        //                                                         completed: completed,
        //                                                         defects: defects);

        //                        Level02ResultList.Add(l2Result);

        //                    }
        //                    return null;
        //                }
        //            }
        //        }
        //    }
        //    catch (SqlException ex)
        //    {
        //        int insertLog = insertLogJson(sql, ex.Message, "N/A", "N/A", "GetCollectionLevel02");
        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        int insertLog = insertLogJson(sql, ex.Message, "N/A", "N/A", "GetCollectionLevel02");
        //        return null;
        //    }
        //}
        public string GetCollectionLevel03(string CollectionLevel02Id, string date, string auditorId, ref int defects)
        {
            string sql = "SELECT [Id], [Level03Id], [ConformedIs], [Value], [ValueText] FROM CollectionLevel03 WHERE CollectionLevel02Id = '" + CollectionLevel02Id + "'";

            string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        using (SqlDataReader r = command.ExecuteReader())
                        {
                            string Level03Results = null;
                            while (r.Read())
                            {
                                string id = r[0].ToString();
                                string level03id = r[1].ToString();
                                bool conformedIs = Convert.ToBoolean(r[2]);
                                int value = Convert.ToInt32(r[3]);
                                if (conformedIs == false && value == 0)
                                {
                                    value = 1;
                                }

                                defects += value;

                                string valueText = r[4].ToString();
                                if (valueText == "undefined")
                                {
                                    valueText = " valuetext";
                                }
                                else
                                {
                                    valueText = " valuetext=\"" + valueText + "\"";
                                    defects += 1;
                                }
                                Level03Results += "<div id=\"" + id + "\" class=\"level03Result\" level03id=\"" + level03id + "\" date=\"" + date + "\" value=\"" + value + "\" conform=\"" + conformedIs.ToString().ToLower() + "\" auditorid=\"" + auditorId + "\" totalerror=\"null\"" + valueText + ">" +
                                                  "</div>";
                            }
                            return Level03Results;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                int insertLog = insertLogJson(sql, ex.Message, "N/A", "N/A", "GetCollectionLevel03");
                return null;
            }
            catch (Exception ex)
            {
                int insertLog = insertLogJson(sql, ex.Message, "N/A", "N/A", "getcollectionlevel02");
                return null;
            }
        }

        #endregion
        #region App
        [WebMethod]
        public string getAPP()
        {

            var html = new Html();

            string login = GetLoginAPP();

            string resource = GetResource();

            return login + resource;
        }
        [WebMethod]
        public string getAPPLevels(int UserSgq_Id, int ParCompany_Id, DateTime Date)
        {
            //colocar autenticação


            string APPMain = getAPPMain(UserSgq_Id, ParCompany_Id, Date); //  /**** COLOQUEI A UNIDADE PRA MONTAR O APP ****/

            string supports = "<div class=\"Results hide\"></div>" +
                              "<div class=\"ResultsConsolidation hide\"></div>" +
                               "<div class=\"ResultsKeys hide\"></div>" +
                              "<div class=\"Deviations hide\"></div>" +
                              "<div class=\"Users hide\"></div>" +
                              "<div class=\"VerificacaoTipificacao hide\"></div>" +
                              "<div class=\"VerificacaoTipificacaoResultados hide\"></div>";

            string resource = GetResource();

            return APPMain +
                   supports +
                   resource;
        }
        public string GetResource()
        {
            System.Reflection.Assembly assembly = this.GetType().Assembly;

            System.Resources.ResourceManager resourceManager = Resources.Resource.ResourceManager;

            var resourceSet = resourceManager.GetResourceSet(
                Thread.CurrentThread.CurrentUICulture, true, false);

            string items = "";

            //foreach (var entry in resourceSet.Cast<DictionaryEntry>())
            //{
            //    items += "<div res='"+entry.Key.ToString() + "'>"+ entry.Value.ToString() + "</div>";
            //} 
            
            return "<div class='Resource hide'>"+ items + "</div>";
        }

        public int getEvaluate(SGQDBContext.ParLevel2 parlevel2, IEnumerable<SGQDBContext.ParLevel2Evaluate> ParEvaluateCompany, IEnumerable<SGQDBContext.ParLevel2Evaluate> ParEvaluatePadrao)
        {
            int evaluate = 1;
            var evaluateConf = ParEvaluateCompany.Where(p => p.Id == parlevel2.Id).FirstOrDefault();
            if (evaluateConf != null)
            {
                evaluate = evaluateConf.Evaluate;
            }
            else
            {
                evaluateConf = ParEvaluatePadrao.Where(p => p.Id == parlevel2.Id).FirstOrDefault();
                if (evaluateConf != null)
                {
                    evaluate = evaluateConf.Evaluate;
                }
            }
            return evaluate;
        }

        public int getSample(SGQDBContext.ParLevel2 parlevel2, IEnumerable<SGQDBContext.ParLevel2Sample> ParSampleCompany, IEnumerable<SGQDBContext.ParLevel2Sample> ParSamplePadrao)
        {
            int sample = 1;
            var sampleConf = ParSampleCompany.Where(p => p.Id == parlevel2.Id).FirstOrDefault();
            if (sampleConf != null)
            {
                sample = sampleConf.Sample;
            }
            else
            {
                sampleConf = ParSamplePadrao.Where(p => p.Id == parlevel2.Id).FirstOrDefault();
                if (sampleConf != null)
                {
                    sample = sampleConf.Sample;
                }
            }
            if (sample == 0)
            {
                sample = 1;
            }
            return sample;
        }

        public string getAPPMain(int UserSgq_Id, int ParCompany_Id, DateTime Date, string culture = "pt-br")
        {
            var html = new Html();

            string breadCrumb = "<ol class=\"breadcrumb\" breadmainlevel=\"Slaughter\"></ol>";

            string container = html.div(

                                         outerhtml: breadCrumb +
                                                    GetLevel01(ParCompany_Id: ParCompany_Id,                     /****** PORQUE ESTA MOKADO ESSA UNIDADE 1? *******/
                                                               dateCollect: Date)

                                        , classe: "container");

            string buttons = " <button id=\"btnSave\" class=\"btn btn-lg btn-warning hide\"><i id=\"saveIcon\" class=\"fa fa-save\"></i><i id=\"loadIcon\" class=\"fa fa-circle-o-notch fa-spin\" style=\"display:none;\"></i></button><!--Save-->" +
                             " <button class=\"btn btn-lg btn-danger btnCA hide\">Ação Corretiva</button><!--Corrective Action-->";

            string message = "<div class=\"message padding20\" style=\"display:none\">                                                                                      " +
                             "   <h1 class=\"head\">Titulo</h1>                                                                                                           " +
                             "   <div class=\"body font16\">Mensagem</div>                                                                                                " +
                             "   <div class=\"foot\"><button id=\"btnMessageOk\" class=\"btn btn-lg marginRight30 btn-primary pull-right btnMessage\"> Ok</button></div>      " +
                             "</div>                                                                                                                                    ";
            // string messageConfirm = null;
            //string viewModal = "<div class=\"viewModal\" style=\"display:none;\">                                                                                                                                                         " +
            //                       "</div>                                                                                                                                                                                                    ";

            string viewModal = "<div class=\"viewModal\" style=\"display:none;\">" +
                               "     <div class=\"head\" style=\"height:35px;line-height:35px;padding-left:10px;padding-right:10px\"><div class=\"title\">View </div><a href=\"#\" class=\"pull-right close\" style=\"color: #000;text-decoration:none\">X</a></div> " +
                               "     <div class=\"body\" style=\"height:565px; overflow-y: auto;padding-left:5px;padding-right:5px;padding-bottom:5px;\"></div>                                                                           " +
                               "</div>                                                                                                                                                                                                    ";

            string modalVF = "<div class=\"modalVF panel panel-primary\" style=\"display:none;\"></div>";

            string modalPCC1B = "<div class=\"modalPCC1B panel panel-primary\" style=\"display:none;\"></div>";

            string messageConfirm = "<div class=\"messageConfirm padding20\" style=\"display:none\">                                                                                                " +
                                        "    <h1 class=\"head\">Titulo</h1>                                                                                                                             " +
                                        "    <div class=\"body font16\"> <div class=\"txtMessage\"></div>                                                                                               " +
                                        "        <input type=\"password\" id=\"passMessageComfirm\" placeholder=\"Password\" class=\"form-control input-sm\" style=\"max-width:160px;\" />        " +
                                        "        <input type=\"text\" masc=\"date\" id=\"inputDate\" placeholder=\"99/99/9999\" class=\"form-control input-sm hide\" style=\"max-width:160px;\" /> </div>       " +
                                        "    <div class=\"foot\"><button id=\"btnMessageYes\" class=\"btn btn-lg marginRight30 btn-primary pull-right btnMessage\"> Sim </button></div>                 " +
                                        "    <div class=\"foot\"><button id=\"btnMessageNo\" class=\"btn btn-lg marginRight30 btn-primary pull-right btnMessage\"> Não </button></div>                   " +
                                        "</div>                                                                                                                                                         ";

            //string viewModal = "<div class=\"viewModal\" style=\"display:none;\">                                                                                                                                                       " +
            //                    "    <div class=\"head\" style=\"height:35px;line-height:35px;padding-left:10px;padding-right:10px\">View <a href=\"#\" class=\"pull-right close\" style=\"color:#000;text-decoration:none\">X</a></div> " +
            //                    "    <div class=\"body\" style=\"height:565px;overflow-y:auto;padding-left:5px;padding-right:5px;padding-bottom:5px;\"></div>                                                                            " +
            //                    "</div>       

            string debug = "<div id = 'ControlaDivDebugAlertas' onclick='showHideDivDebugAlerta();'></div> " +

                           "<div id = 'divDebugAlertas' > " +
                           "     <p class='titDebugAlertas'>Acompanhamento do indicador</p> " +
                           "     Indicador: <div id = 'level1' class='clDebugAlertas'></div> " +
                           "     <br> " +
                           "     Volume total do indicador: <div id = 'volumeTotal'  class='clDebugAlertas'></div> " +
                           "     <br> " +
                           "     Meta %: <div id = 'meta'  class='clDebugAlertas'></div> " +
                           "     <br> " +
                           "     Meta Tolerância: <div id = 'metaTolerancia'  class='clDebugAlertas'></div> " +
                           "     <br> " +
                           "     Meta Dia: <div id = 'metaDia'  class='clDebugAlertas'></div> " +
                           "     <br> " +
                           "     Meta Avaliação: <div id = 'metaAvaliacao'  class='clDebugAlertas'></div> " +
                           "     <br> " +
                           "     Total Avaliado: <div id = 'totalAv'  class='clDebugAlertas'></div> " +
                           "     <br> " +
                           "     Total NC: <div id = 'totalNc'  class='clDebugAlertas'></div> " +
                           "     <br> " +
                           "     Total NC na avaliação: <div id = 'totalNcNaAvalicao'  class='clDebugAlertas'></div> " +

                           "</div> " +

                           "<script> " +

                           "     $('#ControlaDivDebugAlertas').hide(); " +
                           "     $('#divDebugAlertas').hide(); " +

                           " </script> ";

            return html.div(
                            outerhtml: navBar(UserSgq_Id, ParCompany_Id) +
                                       rightMenu() +
                                       html.div(classe: "overlay", style: "display:none") +
                                       container +
                                       buttons +
                                       footer(),
                             classe: "App hide",
                             tags: "breadmainlevel=\"Indicadores\" culture=\"" + culture + "\" turningtime=\"03:00\""
                           ) +
                           correctiveAction() +
                           viewModal +
                           modalVF +
                           modalPCC1B +
                           message +
                           messageConfirm +
                           debug;
        }

       

        public string navBar(int UserSgq_Id, int ParCompany_Id)
        {
            string navBar = "<div class=\"navbar navbar-inverse navbar-fixed-top\">                                                                                                                             " +
                           "    <div class=\"container\">                                                                                                                                                       " +
                           "        <div class=\"navbar-header\" style=\"width: 100%\">                                                                                                                         " +
                           "            <a class=\"navbar-brand\" id=\"SGQName\" href=\"#\"><i class=\"fa fa-chevron-left hide iconReturn\" aria-hidden=\"true\"></i> SGQ - Coleta de dados</a>                 " +
                           "            <div class=\"buttonMenu navbar-brand hide\" id=\"btnShowImage\" level01id=\"2\">Show Image</div>                                                                        " +
                           selectUserCompanys(UserSgq_Id, ParCompany_Id) +
                           "            <span style='color: #ffffff; margin: 14px;' class='period'>Periodo</span><span style='color: #ffffff; margin: 14px;' class='shift'>shift</span> " +
                           "            <div id=\"btnMore\" class=\"iconMoreMenu pull-right\" style=\"padding: 12px;\"><i class=\"fa fa-ellipsis-v iconMoreMenu\" aria-hidden=\"true\"></i></div><span style='color: #ffffff; margin: 14px;' class='atualDate pull-right'></span>" +
                           "        </div>                                                                                                                                                                      " +
                           "    </div>                                                                                                                                                                          " +
                           "</div>                                                                                                                                                                              ";

            return navBar;
        }
        public string rightMenu()
        {
            string menu = "<div class=\"rightMenu\">                                                                                                  " +
                           "     <div class=\"list-group list-group-inverse rightMenuList\">                                                           " +
                           "         <a href=\"#\" id=\"btnSync\" class=\"list-group-item\" style=\"background-color: black; font-weight: bold;\">Sincronizar Resultados</a>                                                  " +
                           "         <a href=\"#\" id=\"btnSyncParam\" class=\"list-group-item\"  style=\"background-color: black; font-weight: bold;\">Sincronizar Parametrizações</a>                                                  " +

                           "         <a href=\"#\" id=\"btnLogout\" class=\"list-group-item\">Logout</a>                                              " +
                           "         <a href=\"#\" id=\"btnLog\" class=\"list-group-item\">Visualizar Log</a>                                               " +
                           "         <a href=\"#\" id=\"btnCollectDB\" class=\"list-group-item\">Visualizar banco de dados</a>                                    " +
                           "         <a href=\"#\" id=\"btnClearDatabase\" class=\"list-group-item\">Limpar banco de dados</a>                                " +
                           "         <a href=\"#\" id=\"btnMostrarContadores\" class=\"list-group-item\">Mostrar contadores</a>                                " +
                           "         <span id=\"version\" class=\"list-group-item\">Versão: <span class=\"number\"></span></span>                     " +
                           "         <span id=\"ambiente\" class=\"list-group-item\"><span class=\"base\"></span></span>                               " +
                           "     </div>                                                                                                                " +
                           " </div>                                                                                                                    ";

            return menu;
        }
        public string correctiveAction()
        {
            string correctiveAction = "<div id=\"correctiveActionModal\" class=\"container panel panel-default modal-padrao\" style=\"display:none\">" +
                                          "<div class=\"panel-body\">" +
                                          "<!--<div class=\"modal-header\">" +
                                          "<button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-hidden=\"true\">×</button>" +
                                          "</div>-->" +
                                          "<div class=\"modal-body\">" +
                                               "<h2>Ação Corretiva</h2>" +
                                          "<div id=\"messageAlert\" class=\"alert alert-info hide\" role=\"alert\">" +
                                          "<span id=\"mensagemAlerta\" class=\"icon-info-sign\"></span>" +
                                          "</div>" +
                                          "<div class=\"row formCorrectiveAction\">" +
                                            "<div class=\"panel panel-default\">" +
                                                "<div class=\"panel-body\">" +
                                                    "<div class=\"row\">" +
                                                        "<div class=\"col-xs-6\" id=\"CorrectiveActionTaken\">" +
                                                            "<b class=\"font16\">Ação Corretiva Tomada:<br /></b>" +
                                                            "<b>Data/Hora:</b> <span id=\"datetime\"></span><br/>" +
                                                            "<b> Auditor: </b><span id=\"auditor\"></span><br/>" +
                                                            //"<b> Shift: </b><span id=\"shift\"></span><br/>" +
                                                            "</div>" +
                                                            "<div class=\"col-xs-6\" id=\"AuditInformation\">" +
                                                            "<b class=\"font16\">Informação da Auditoria:<br/></b>" +
                                                            "<b>Indicador: </b><span id=\"auditText\"></span><br/>" +
                                                            "<b>Data Início:</b><span id=\"starttime\"></span><br/>" +
                                                            //"<b>Period:</b><span id=\"correctivePeriod\"></span>" +
                                                            "</div>" +
                                                            "</div>" +
                                                      "</div>" +
                                                    "</div>" +
                                                      "<div class=\"form-group\">" +
                                                        "<label>Descrição da Falha:</label>" +
                                                        "<textarea id=\"DescriptionFailure\" class=\"form-control custom-control\" rows=\"3\" style=\"resize:none\"></textarea>" +
                                                    "</div>" +
                                                    "<div class=\"form-group\">" +
                                                        "<label>Ação corretiva imediata:</label>" +
                                                        "<textarea id=\"ImmediateCorrectiveAction\" class=\"form-control custom-control\" rows=\"3\" style=\"resize:none\"></textarea>" +
                                                    "</div>" +
                                                    "<div class=\"form-group\">" +
                                                        "<label>Disposição do Produto:</label>" +
                                                        "<textarea id=\"ProductDisposition\" class=\"form-control custom-control\" rows=\"3\" style=\"resize:none\"></textarea>" +
                                                    "</div>" +
                                                    "<div class=\"form-group\">" +
                                                        "<label>Medida Preventiva:</label>" +
                                                        "<textarea id=\"PreventativeMeasure\" class=\"form-control custom-control\" rows=\"3\" style=\"resize:none\"></textarea>" +
                                                    "</div>" +
                                                    "<div class=\"row\">" +
                                                        "<div class=\"col-xs-6\">" +
                                                            "<div class=\"SlaugtherSignature hide\">" +
                                                            "<h4>Slaughter Signature</h4>" +
                                                            "<div class=\"name\">Admin</div>" +
                                                            "<div class=\"date\">08/24/2016 10:31</div>" +
                                                             "<button class=\"btn btn-link btnSlaugtherSignatureRemove\">Remove Signature</button>" +
                                                        "</div>" +
                                                    "</div>" +
                                                    "<div class=\"col-xs-6\">" +
                                                        "<div class=\"TechinicalSignature hide\">" +
                                                        "<h4>Technical Signature</h4>" +
                                                        "<div class=\"name\">Admin2</div>" +
                                                        "<div class=\"date\">08/24/2016</div>" +
                                                        "<button class=\"btn btn-link btnTechinicalSignatureRemove\">Remove Signature</button>" +
                                                                    "</div>" +
                                                                "</div>" +
                                                            "</div>" +
                                                        "</div>" +
                                                    "</div>" +
                                                    "<div class=\"modal-footer\">" +
                                                        "<span class=\"pull-left\">" +
                                                        //"<button class=\"btn btn-default btnSignature btnSlaugtherSignature hide\">" +
                                                        //    "Slaughter Signature" +
                                                        //"</button>" +
                                                        //"<button class=\"btn btn-default btnSignature btnTechinicalSignature hide\">" +
                                                        //    "Technical Signature" +
                                                        //"</button>" +
                                                        "</span>" +

                                                        "<button class=\"btn btn-danger modal-close-ca\">Fechar</button>" +
                                                        "<button class=\"btn btn-primary\" id=\"btnSendCorrectiveAction\">Enviar</button>" +
                                                    "</div>" +
                                                "</div>" +
                                                "</div>";

            return correctiveAction;
        }

        public string footer()
        {
            string foot = "<footer class=\"footer\">                                                                                                                                       " +
                          "   <p style=\"color:white; margin-left:16px; margin-right:16px; margin-top: 12px;\">                                                                      " +
                          "       <span class=\"user\">Admin</span> - <span class=\"unit\">Colorado</span> | <span class=\"urlPrefix\"></span>                                          " +
                          "       <span class=\"status pull-right\"></span>                                                                                                          " +
                          "   </p>                                                                                                                                                   " +
                          "</footer>                                                                                                                                                 ";

            return foot;
        }
        /// <summary>
        /// Recupera Level1 e seus monitoramentos e tarefas relacionados
        /// </summary>
        /// <returns></returns>
        public string GetLevel01(int ParCompany_Id, DateTime dateCollect)
        {
            ///SE NÃO HOUVER NENHUM LEVEL1, LEVEL2, LEVEL3 INFORMAR QUE NÃO ENCONTROU MONITORAMENTOS
            var html = new Html();

            //Instanciamos a Classe ParLevel01 Dapper
            var ParLevel1DB = new SGQDBContext.ParLevel1();
            //Inicaliza ParLevel1VariableProduction
            var ParLevel1VariableProductionDB = new SGQDBContext.ParLevel1VariableProduction();

            //Buscamos os ParLevel11 para a unidade selecionada
            var parLevel1List = ParLevel1DB.getParLevel1ParCriticalLevelList(ParCompany_Id: ParCompany_Id);

            //Agrupamos o ParLevel1 por ParCriticalLevel
            var parLevel1GroupByCriticalLevel = parLevel1List.OrderBy(p => p.ParCriticalLevel_Id).GroupBy(p => p.ParCriticalLevel_Id);

            //Instanciamos uma variável para não gerenciar a utilizar do ParCriticalLevel
            bool ParCriticalLevel = false;

            //Instanciamos uma variável para instanciar a lista de level1, level2 e level3
            //Esses itens podem ser transformados funções menores
            string listlevel1 = null;
            string listLevel2 = null;
            string listLevel3 = null;
            //Percorremos a lista de agrupada
            foreach (var parLevel1Group in parLevel1GroupByCriticalLevel)
            {
                //Instanciamos uma variável level01GroupList
                string level01GroupList = null;
                //Instanciamos uma variável list parLevel1 para adicionar os parLevel1
                string parLevel1 = null;
                //Instanciamos uma variável para verificar o nome do ParCriticalLevel
                string nameParCritialLevel = null;
                //Percorremos a Lista dos Agrupamento
                foreach (var parlevel1 in parLevel1Group)
                {
                    string tipoTela = "";

                    var variableList = ParLevel1VariableProductionDB.getVariable(parlevel1.Id).ToList();

                    if (variableList.Count > 0)
                    {
                        tipoTela = variableList[0].Name;
                    }
                    //Se o ParLevel1 contem um ParCritialLevel_Id
                    var ParLevel1AlertasDB = new SGQDBContext.ParLevel1Alertas();
                    var alertas = ParLevel1AlertasDB.getAlertas(parlevel1.Id, ParCompany_Id, dateCollect);

                    if (parlevel1.ParCriticalLevel_Id > 0)
                    {
                        //O ParLevel1 vai estar dentro de um accordon
                        ParCriticalLevel = true;
                        //Pego o nome do ParCriticalLevel para não precisar fazer outra pesquisa
                        nameParCritialLevel = parlevel1.ParCriticalLevel_Name;
                        //Incremento os itens que estaram no ParLevel1                
                        //Gera linha Level1

                        decimal tipoAlerta = parlevel1.tipoAlerta;
                        decimal valorAlerta = parlevel1.valorAlerta;

                        decimal alertaNivel1 = 0;
                        decimal alertaNivel2 = 0;
                        string alertaNivel3 = "";

                        decimal volumeAlerta = 0;
                        decimal meta = 0;

                        if(tipoAlerta == 1) //JBS por Indicador
                        {
                            if (alertas != null)
                            {
                                alertaNivel1 = alertas.Nivel1;
                                alertaNivel2 = alertas.Nivel2;
                                alertaNivel3 = "a1";
                                volumeAlerta = alertas.VolumeAlerta;
                                meta = alertas.Meta;
                            }
                        }
                        else if (tipoAlerta == 2)  //# de NC
                        {
                            if (alertas != null)
                            {
                                alertaNivel1 = valorAlerta;
                                alertaNivel2 = valorAlerta;
                                alertaNivel3 = "a2";
                                volumeAlerta = alertas.VolumeAlerta;
                                meta = alertas.Meta;
                            }
                        }
                        else if (tipoAlerta == 3)  //% de NC
                        {
                            if (alertas != null)
                            {
                                alertaNivel1 = valorAlerta;
                                alertaNivel2 = valorAlerta;
                                alertaNivel3 = "a3";
                                volumeAlerta = alertas.VolumeAlerta;
                                meta = alertas.Meta;
                            }
                        }
                        else if (tipoAlerta == 4)  //JBS por Monitoramento
                        {
                            if (alertas != null)
                            {
                                alertaNivel1 = alertas.Nivel1;
                                alertaNivel2 = alertas.Nivel2;
                                alertaNivel3 = "a4";
                                volumeAlerta = alertas.VolumeAlerta;
                                meta = alertas.Meta;
                            }
                        }
                        else
                        {
                            if (alertas != null) //Fica como padrão JBS por indicador
                            {
                                alertaNivel1 = alertas.Nivel1;
                                alertaNivel2 = alertas.Nivel2;
                                alertaNivel3 = "a1";
                                volumeAlerta = alertas.VolumeAlerta;
                                meta = alertas.Meta;
                            }
                        }



                        string level01 = html.level1(parlevel1,
                                                     tipoTela: tipoTela,
                                                     totalAvaliado: 0,
                                                     totalDefeitos: 0,                                                     
                                                     alertNivel1: alertaNivel1,
                                                     alertNivel2: alertaNivel2,
                                                     alertaNivel3: alertaNivel3,
                                                     numeroAvaliacoes: 0,
                                                     metaDia: alertaNivel1 * 3,
                                                     metaTolerancia: alertaNivel1,
                                                     metaAvaliacao: 0,
                                                     alertaAtual: 0,
                                                     avaliacaoultimoalerta: 0,
                                                     monitoramentoultimoalerta: 0,
                                                     volumeAlertaIndicador: volumeAlerta,
                                                     metaIndicador: meta);
                        //Incrementa level1
                        parLevel1 += html.listgroupItem(parlevel1.Id.ToString(), classe: "row", outerhtml: level01);
                    }
                    else
                    {
                        //Caso o ParLevel1 não contenha um ParCritialLevel_Id apenas incremento os itens de ParLevel1
                        parLevel1 += html.listgroupItem(parlevel1.Id.ToString(), outerhtml: parlevel1.Name);
                    }
                    //Instancia variável para receber todos os level3
                    string level3Group = null;

                    //Busca os Level2 e reforna no level3Group;
                    listLevel2 += GetLevel02(parlevel1, ParCompany_Id, dateCollect, ref level3Group);

                    //Incrementa Level3Group
                    listLevel3 += level3Group;
                }
                //Quando termina o loop dos itens agrupados por ParCritialLevel 
                //Se contem ParCritialLevel
                
                if (ParCriticalLevel == true)
                {
                    Html.bootstrapcolor? color = null;
                    if (parLevel1Group.Key == 1)
                    {
                        color = Html.bootstrapcolor.danger;
                    }
                    else if (parLevel1Group.Key == 2)
                    {
                        color = Html.bootstrapcolor.warning;
                    }
                    else if (parLevel1Group.Key == 3)
                    {
                        color = Html.bootstrapcolor.info;
                    }
                    //Adicionamos os itens em um acordeon
                    parLevel1 = html.accordeon(
                                                id: parLevel1Group.Key.ToString() + "critivalLevel",
                                                label: nameParCritialLevel,
                                                color: color,
                                                outerhtml: parLevel1);
                }
                else
                {
                    //Adicionamos os itens e um listgroup
                    level01GroupList = html.listgroup(
                                                   outerhtml: parLevel1
                                                );
                }
                //Adicionar a lista de level01 agrupados ou não a lsita geral
                listlevel1 += parLevel1;
            }
            //Retona as lista
            //Podemos gerar uma verificação de atualizações
            return html.div(
                            outerhtml: listlevel1,
                            classe: "level1List"
                            ) +
                   html.div(
                            outerhtml: listLevel2,
                            classe: "level2List col-xs-12 hide"
                           ) +
                   html.div(
                            outerhtml: listLevel3,
                            classe: "level3List  List col-xs-12 hide"
                           );

        }
        /// <summary>
        /// Gera Linhas do level2
        /// </summary>
        /// <param name="ParLevel1"></param>
        /// <param name="ParCompany_Id"></param>
        /// <param name="level3Group"></param>
        /// <returns></returns>
        public string GetLevel02(SGQDBContext.ParLevel1 ParLevel1, int ParCompany_Id, DateTime dateCollect, ref string level3Group)
        {
            //Inicializa ParLevel2
            var ParLevel2DB = new SGQDBContext.ParLevel2();
            //Pega uma lista de ParLevel2
            //Tem que confirmar a company e colocar na query dentro do método, ainda não foi validado
            var parlevel02List = ParLevel2DB.getLevel2ByIdLevel1(ParLevel1.Id, ParCompany_Id);

            //Inicializa Cabecalhos
            var ParLevelHeaderDB = new SGQDBContext.ParLevelHeader();
            //Inicaliza ParFieldType
            var ParFieldTypeDB = new SGQDBContext.ParFieldType();

            var html = new Html();

            //Instancia parLevel2List
            string ParLevel2List = null;
            //Instancia headerlist
            string headerList = null;

            //Inicializa Avaliações e Amostras
            var ParEvaluateDB = new SGQDBContext.ParLevel2Evaluate();
            var ParSampleDB = new SGQDBContext.ParLevel2Sample();

            //Verifica avaliações padrão
            var ParEvaluatePadrao = ParEvaluateDB.getEvaluate(ParLevel1: ParLevel1,
                                                              ParCompany_Id: null);

            //Verifica avaliações pela company informada
            var ParEvaluateCompany = ParEvaluateDB.getEvaluate(ParLevel1: ParLevel1,
                                                               ParCompany_Id: ParCompany_Id);

            //Verifia amostra padrão
            var ParSamplePadrao = ParSampleDB.getSample(ParLevel1: ParLevel1,
                                                        ParCompany_Id: null);

            //Verifica amostra pela company informada
            var ParSampleCompany = ParSampleDB.getSample(ParLevel1: ParLevel1,
                                                        ParCompany_Id: ParCompany_Id);

            //Variaveis para avaliação de grupos
            int evaluateGroup = 0;
            int sampleGroup = 0;

            string groupLevel3Level2 = null;
            string painelLevel3 = null;

            //Enquando houver lista de level2
            foreach (var parlevel2 in parlevel02List)
            {
                //Verifica se pega avaliações e amostras padrão ou da company
                int evaluate = getEvaluate(parlevel2, ParEvaluateCompany, ParEvaluatePadrao);
                int sample = getSample(parlevel2, ParSampleCompany, ParSamplePadrao);

                //Se agrupar level2 com level3 pego o valor da primeira avaliação e amostra
                if(ParLevel1.HasGroupLevel2 == true & evaluateGroup == 0 )
                {
                    evaluateGroup = evaluate;
                    sampleGroup = sample;
                }

                //Colocar função de gerar cabeçalhos por selectbox
                //Monta os cabecalhos
                #region Cabecalhos e Contadores
                string headerCounter =
                                     html.div(
                                               outerhtml: "<b>Av.</b>",
                                               classe: "col-xs-6",
                                               style: "text-align:center"
                                             ) +
                                     html.div(
                                               outerhtml: "<b>Am.</b>",
                                               classe: "col-xs-6",
                                               style: "text-align:center"
                                             ); ;

                headerCounter = html.div(
                                    //aqui vai os botoes
                                    outerhtml: headerCounter,
                                    classe: "counters col-xs-4"
                                    );


                string classXSLevel2 = " col-xs-5";

                int totalSampleXEvaluate = evaluate * sample;

                string counters =
                                      html.div(
                                                outerhtml: html.span(outerhtml: "0", classe: "evaluateCurrent") + " / " + html.span(outerhtml: evaluate.ToString(), classe: "evaluateTotal"),
                                                classe: "col-xs-6",
                                                style: "text-align:center"
                                              ) +
                                      html.div(
                                                outerhtml: html.span(outerhtml: "0", classe: "sampleCurrent hide") + html.span(outerhtml: "0", classe: "sampleCurrentTotal") + " / " + html.span(outerhtml: sample.ToString(), classe: "sampleTotal hide") + html.span(outerhtml: totalSampleXEvaluate.ToString(), classe: "sampleXEvaluateTotal"),
                                                classe: "col-xs-6",
                                                style: "text-align:center"
                                              );

                counters = html.div(
                                    //aqui vai os botoes
                                    outerhtml: counters,
                                    classe: "counters col-xs-4"
                                    );

                #endregion
                string buttons = null;
                string buttonsHeaders = null;
                //Caso tenha funções de não aplicado, coloca os botões nas respectivas linhas
                //Como vai ficar se o item tem varias avaliações?vai ter botão salvar na linha do monitoramento?
                if (ParLevel1.HasNoApplicableLevel2 == true || ParLevel1.HasSaveLevel2 == true)
                {
                    string btnNotAvaliable = null;
                    if (ParLevel1.HasNoApplicableLevel2)
                    {
                        btnNotAvaliable = "<button class=\"btn btn-warning btnNotAvaliableLevel2 na\"> " +
                                           "   <span class=\"cursorPointer iconsArea\">N/A</span> " +
                                           "</button>                                             ";
                    }
                    string btnAreaSave = null;
                    if (ParLevel1.HasSaveLevel2)
                    {
                        btnAreaSave = "<button class=\"btn btn-success hide btnAreaSaveConfirm\">                                                    " +
                                       "   <span class=\"cursorPointer\">Confirm? <i class=\"fa fa-check\" aria-hidden=\"true\"></i></span>     " +
                                       "</button>                                                                                                      " +
                                       "<button class=\"btn btn-primary btnAreaSave\">                                                                 " +
                                       "   <span class=\"cursorPointer iconsArea\"><i class=\"fa fa-floppy-o\" aria-hidden=\"true\"></i></span>        " +
                                       "</button>                                                                                                      ";
                    }
                    buttons = html.div(
                                 //aqui vai os botoes
                                 outerhtml: btnAreaSave +
                                            btnNotAvaliable,
                                 style: "text-align: right",
                                 classe: "userInfo col-xs-3"
                                 );

                    buttonsHeaders = html.div(
                                             outerhtml: null,
                                             classe: "userInfo col-xs-3"
                                             );
                }
                else
                {
                    classXSLevel2 = " col-xs-8";
                }

                string level02Header = html.div(classe: classXSLevel2) +
                                       headerCounter +
                                       buttonsHeaders;

                headerList = html.listgroupItem(
                                                classe: "row",
                                                outerhtml: level02Header
                                               );

                //podemos aplicar os defeitos
                string level2 = html.level2(id: parlevel2.Id.ToString(),
                                            label: parlevel2.Name,
                                            classe: classXSLevel2,
                                            evaluate: evaluate,
                                            sample: sample,
                                            HasSampleTotal: parlevel2.HasSampleTotal,
                                            IsEmptyLevel3: parlevel2.IsEmptyLevel3);

                //Gera linha do Level2
                ParLevel2List += html.listgroupItem(
                                                    id: parlevel2.Id.ToString(),
                                                    classe: "row",
                                                    outerhtml: level2 +
                                                               counters +
                                                               buttons +
                                                               html.div(classe: "level2Debug")
                                                    );


                //Gera monitoramento do level3
                string groupLevel3 = GetLevel03(ParLevel1, parlevel2, ParCompany_Id, dateCollect, ref painelLevel3);

                if(ParLevel1.HasGroupLevel2 == true)
                {
                    groupLevel3 = html.accordeon(
                                                    id: parlevel2.Id.ToString(),
                                                    label: parlevel2.Name,
                                                    classe: "level2 row",
                                                    outerhtml: groupLevel3,
                                                    accordeonId: parlevel2.Id

                                                );

                    groupLevel3Level2 += groupLevel3;
                }
                else
                {
                    level3Group += groupLevel3;
                }
               
            }

            //Se tiver agrupamentos no ParLevel1
            if(ParLevel1.HasGroupLevel2 == true)
            {
                string parLevel3Group = null;


                string accordeonbuttons = null;
                
                    accordeonbuttons = "<button class=\"btn btn-default button-expand marginRight10\"><i class=\"fa fa-expand\" aria-hidden=\"true\"></i> Mostrar Todos</button>" +
                                       "<button class=\"btn btn-default button-collapse\"><i class=\"fa fa-compress\" aria-hidden=\"true\"></i> Fechar Todos</button>";
                

                //painellevel3 = html.listgroupItem(
                //                                            outerhtml: avaliacoes +
                //                                                       amostras +
                //                                                       painelLevel3HeaderListHtml,

                //                               classe: "painel painelLevel03 row");

                string panelButton = html.listgroupItem(
                                                           outerhtml: accordeonbuttons +
                                                                      "<button id='btnAllNA' class='btn btn-warning btn-sm pull-right'> Todos N/A </button>" +

                                                                      "<button id='btnAllNC' class='btn btn-danger btn-sm pull-right' style='margin-right: 10px;'> Clicar em Todos </button>",
                                                           classe: "painel painelLevel02 row"
                                                        );


                if (!string.IsNullOrEmpty(groupLevel3Level2))
                {
                    parLevel3Group = html.div(
                                               classe: "level3Group",
                                               tags: "level1idgroup=\"" + ParLevel1.Id + "\"",

                                               outerhtml: painelLevel3 + panelButton +
                                                          groupLevel3Level2
                                             );

                    level3Group += parLevel3Group;
                }

                headerList = null;
                string level2 = html.level2(id: "0",
                                            label: ParLevel1.Name,
                                            classe: "group col-xs-12",
                                            evaluate: evaluateGroup,
                                            sample: sampleGroup,
                                            HasSampleTotal: false,
                                            IsEmptyLevel3: false,
                                            level1Group_Id: ParLevel1.Id);

                //Gera linha do Level2
                ParLevel2List = html.listgroupItem(
                                                    id: ParLevel1.Id.ToString(),
                                                    classe: "row",
                                                    outerhtml: level2 +
                                                               null +
                                                               null +
                                                               html.div(classe: "level2Debug")
                                                    );
            }
           
            //aqui tem que fazer a pesquisa se tem itens sao do level1 ex: cca,htp
            //quando tiver cabecalhos tem que replicar no level1

            ParLevel2List = headerList +
                            ParLevel2List;

            var painelLevel2HeaderListHtml = GetHeaderHtml(ParLevelHeaderDB.getHeaderByLevel1(ParLevel1.Id), ParFieldTypeDB, html);

            painelLevel2HeaderListHtml = html.listgroupItem(
                                                            outerhtml: painelLevel2HeaderListHtml,
                                                            classe: "row"
                                                            );
            //Se contem  monitoramentos
            if (!string.IsNullOrEmpty(ParLevel2List))
            {
                //Gera agrupamento dw Level2 para o Level1
                ParLevel2List = html.listgroup(
                                                outerhtml: painelLevel2HeaderListHtml +
                                                           ParLevel2List,
                                                tags: "level01Id=\"" + ParLevel1.Id + "\""
                                               , classe: "level2Group hide");
            }

            return ParLevel2List;
        }
        public string GetHeaderHtml(IEnumerable<ParLevelHeader> list, ParFieldType ParFieldTypeDB, Html html, int ParLevel1_Id = 0, int ParLevel2_Id = 0, ParLevelHeader ParLevelHeaderDB = null)
        {
            string retorno = "";

            foreach (var header in list)
            {

                if(ParLevel1_Id > 0 && ParLevel2_Id > 0 && ParLevelHeaderDB != null)
                {
                    if (ParLevelHeaderDB.isHeaderLeve2Exception(ParLevel1_Id, ParLevel2_Id, header.ParHeaderField_Id))
                    {
                        continue;
                    }
                }

                var label = "<label class=\"font-small\">" + header.ParHeaderField_Name + "</label>";

                var form_control = "";

                //ParFieldType 
                switch (header.ParFieldType_Id)
                {
                    //Multipla Escolha
                    case 1:
                        var listMultiple = ParFieldTypeDB.getMultipleValues(header.ParHeaderField_Id);
                        var optionsMultiple = "";
                        bool hasDefault = false;
                        foreach (var value in listMultiple)
                        {
                            if (value.IsDefaultOption == 1)
                            {
                                optionsMultiple += "<option selected=\"selected\" value=\"" + value.Id + "\" PunishmentValue=\"" + value.PunishmentValue + "\">" + value.Name + "</option>";
                                hasDefault = true;
                            }
                            else
                            {
                                optionsMultiple += "<option value=\"" + value.Id + "\" PunishmentValue=\"" + value.PunishmentValue + "\">" + value.Name + "</option>";
                            }
                        }
                        if (!hasDefault)
                            optionsMultiple = "<option selected=\"selected\" value=\"0\">" + Resources.Resource.select + "...</option>" + optionsMultiple;

                        form_control = "<select class=\"form-control input-sm\" ParHeaderField_Id=\"" + header.ParHeaderField_Id + "\" ParFieldType_Id=\"" + header.ParFieldType_Id + "\">" + optionsMultiple + "</select>";
                        break;
                    //Integrações
                    case 2:
                        form_control = "<div class=\"form-control input-sm\" ParHeaderField_Id=\"" + header.ParHeaderField_Id + "\" ParFieldType_Id = \"" + header.ParFieldType_Id + "\">" + header.ParHeaderField_Name + "</div>";
                        break;
                    //Binário
                    case 3:
                        var listBinario = ParFieldTypeDB.getMultipleValues(header.ParHeaderField_Id);
                        var optionsBinario = "";
                        foreach (var value in listBinario)
                        {
                            if (listBinario.ElementAt(0) == value)
                            {
                                optionsBinario += "<option selected value=\"" + value.Id + "\" PunishmentValue=\"" + value.PunishmentValue + "\">" + value.Name + "</option>";
                            }
                            else
                            {
                                optionsBinario += "<option value=\"" + value.Id + "\" PunishmentValue=\"" + value.PunishmentValue + "\">" + value.Name + "</option>";
                            }
                        }
                        form_control = "<select class=\"form-control input-sm\" ParHeaderField_Id='" + header.ParHeaderField_Id + "' ParFieldType_Id = '" + header.ParFieldType_Id + "'>" + optionsBinario + "</select>";
                        break;
                    //Texto
                    case 4:
                        form_control = "<input class=\"form-control input-sm\" type=\"text\" ParHeaderField_Id=\"" + header.ParHeaderField_Id + "\" ParFieldType_Id=\"" + header.ParFieldType_Id + "\">";
                        break;
                    //Numérico
                    case 5:
                        form_control = "<input class=\"form-control input-sm\" type=\"number\" ParHeaderField_Id=\"" + header.ParHeaderField_Id + "\" ParFieldType_Id=\"" + header.ParFieldType_Id + "\">";
                        break;
                    //Data
                    case 6:
                        form_control = "<input class=\"form-control input-sm\" type=\"date\" ParHeaderField_Id=\"" + header.ParHeaderField_Id + "\" ParFieldType_Id=\"" + header.ParFieldType_Id + "\">";
                        break;

                    //Hora
                    case 7:
                        form_control = "<input class=\"form-control input-sm\" type=\"time\" ParHeaderField_Id=\"" + header.ParHeaderField_Id + "\" ParFieldType_Id=\"" + header.ParFieldType_Id + "\">";
                        break;
                }

                var form_group = html.div(
                                            outerhtml: label + form_control,
                                            classe: "form-group header",
                                            tags: header.IsRequired == 1 ? "required" : "",
                                            style: "margin-bottom: 4px;"
                                            );

                retorno += html.div(
                                            outerhtml: form_group,
                                            classe: "col-xs-6 col-sm-4 col-md-3 col-lg-2",
                                            style: "padding-right: 4px !important; padding-left: 4px !important;"
                                            );


            }

            return retorno;
        }
        /// <summary>
        /// Retorna Level3 
        /// </summary>
        /// <param name="ParLevel1"></param>
        /// <param name="ParLevel2"></param>
        /// <returns></returns>
        public string GetLevel03(SGQDBContext.ParLevel1 ParLevel1, SGQDBContext.ParLevel2 ParLevel2, int ParCompany_Id, DateTime dateCollect, ref string painellevel3)
        {
            var html = new Html();

            //Inicializa ParLevel3
            var ParLevel3DB = new SGQDBContext.ParLevel3();

            //Inicializa Cabecalhos
            var ParLevelHeaderDB = new SGQDBContext.ParLevelHeader();
            //Inicaliza ParFieldType
            var ParFieldTypeDB = new SGQDBContext.ParFieldType();
            //Inicaliza ParLevel1VariableProduction
            var ParLevel1VariableProductionDB = new SGQDBContext.ParLevel1VariableProduction();
            //Inicaliza CaracteristicaTipificacao
            var CaracteristicaTipificacaoDB = new SGQDBContext.CaracteristicaTipificacao();
            //Inicaliza VerificacaoTipificacaoTarefaIntegracao
            var VerificacaoTipificacaoTarefaIntegracaoDB = new SGQDBContext.VerificacaoTipificacaoTarefaIntegracao();

            //Pega uma lista de parleve3
            //pode colocar par level3 por unidades, como nos eua
            var parlevel3List = ParLevel3DB.getLevel3ByLevel2(ParLevel1, ParLevel2, ParCompany_Id, dateCollect);

            string tipoTela = "";

            var variableList = ParLevel1VariableProductionDB.getVariable(ParLevel1.Id).ToList();

            if (variableList.Count > 0)
            {
                tipoTela = variableList[0].Name;
            }
            string btnNaoAvaliado = html.button(
                                    label: html.span(
                                                 classe: "cursorPointer iconsArea",
                                                 outerhtml: "N/A"
                                             ),
                                    classe: "btn-warning btnNotAvaliable na font11"
                                );


            bool haveAccordeon = false;

            int Last_Id = 0;
            //Tela de bem estar animal
            if (tipoTela.Equals("BEA"))
            {
                //Instancia uma veriavel para gerar o agrupamento
                string parLevel3Group = null;

                foreach (var parLevel3 in parlevel3List)
                {

                    if (Last_Id != parLevel3.Id)
                    {
                        //Define a qual classe de input pertence o level3
                        string classInput = null;
                        //Labels que mostrar informaçãoes do tipo de input
                        string labelsInputs = null;
                        //tipo de input
                        string input = getTipoInputBEA(parLevel3, ref classInput, ref labelsInputs);

                        string level3List = html.level3(parLevel3, input, classInput, labelsInputs);
                        parLevel3Group += level3List;

                        Last_Id = parLevel3.Id;

                    }
                }

                //Avaliações e amostas para painel
                string avaliacoeshtml = html.div(
                                    outerhtml: "<label class=\"font-small\" style=\"display:inherit\">Avaliações</label><label style=\"display:inline-block; font-size: 20px;\">" + html.span(classe: "evaluateCurrent") + " / " + html.span(classe: "evaluateTotal") + "</label>",
                                    style: "margin-bottom: 4px;",
                                    classe: "form-group");
                string amostrashtml = html.div(
                                    outerhtml: "<label class=\"font-small\" style=\"display:inherit\">Amostras</label><label style=\"display:inline-block; font-size: 20px;\">" + html.span(classe: "sampleCurrent") + " / " + html.span(classe: "sampleTotal") + "</label>",
                                    style: "margin-bottom: 4px;",
                                    classe: "form-group");

                string avaliacoes = html.div(
                                    outerhtml: avaliacoeshtml,
                                    style: "padding-right: 4px !important; padding-left: 4px !important;",
                                    classe: "col-xs-6 col-sm-4 col-md-3 col-lg-2");
                string amostras = html.div(
                                    outerhtml: amostrashtml,
                                    style: "padding-right: 4px !important; padding-left: 4px !important;",
                                    classe: "col-xs-6 col-sm-4 col-md-3 col-lg-2");

                //Painel
                //O interessante é um painel só mas no momento está um painel para cada level3group

                var painelLevel3HeaderListHtml = "";

                var labelPecas = "<label class='font-small'>Animais Avaliados</label>";
                var formControlPecas = "<input class='form-control input-sm pecasAvaliadas' type='number'>";
                var formGroupPecas = html.div(
                                        outerhtml: labelPecas + formControlPecas,
                                        classe: "form-group",
                                        style: "margin-bottom: 4px;"
                                        );

                painelLevel3HeaderListHtml += html.div(
                                                outerhtml: formGroupPecas,
                                                classe: "col-xs-6 col-sm-4 col-md-3 col-lg-2",
                                                style: "padding-right: 4px !important; padding-left: 4px !important;"
                                                );

                //string HeaderLevel02 = null;

                painellevel3 = html.listgroupItem(
                                                     outerhtml: avaliacoes +
                                                                amostras +
                                                                painelLevel3HeaderListHtml,

                                        classe: "painel painelLevel03 row") +
                              html.div(outerhtml: "teste", classe: "painel counters row", style: "background-color: #ff0000");

                //Se tiver level3 gera o agrupamento no padrão
                if (!string.IsNullOrEmpty(parLevel3Group))
                {
                    parLevel3Group = html.div(
                                               classe: "level3Group BEA",
                                               tags: "level1id=\"" + ParLevel1.Id + "\" level2id=\"" + ParLevel2.Id + "\"",

                                               outerhtml: painellevel3 +
                                                          parLevel3Group
                                             );
                }
                return parLevel3Group;
            }
            //Tela da verificação da tipificação
            else if (tipoTela.Equals("VF"))
            {
                //Instancia uma veriavel para gerar o agrupamento
                string parLevel3Group = null;

                foreach (var parLevel3 in parlevel3List)
                {
                    if (Last_Id != parLevel3.Id)
                    {
                        string tags = null;
                        string labels = null;

                        //Gera o level3
                        string level3 = html.link(
                                                    outerhtml: html.span(outerhtml: parLevel3.Name, classe: "levelName"),
                                                    classe: "col-xs-12 col-sm-12 col-md-12"
                                                    );

                        switch (parLevel3.Name)
                        {
                            case "Verificação Tipificação - Falha Operacional":
                                var listOper = CaracteristicaTipificacaoDB.getCaracteristicasTipificacao(206);
                                var listOperHtml = "";
                                foreach (var carac in listOper)
                                {
                                    listOperHtml += "<div class='col-xs-2 hide' cNmCaracteristica='" +
                                                     carac.cNmCaracteristica + "' cIdentificador='" + carac.cIdentificador + "' " +
                                                     " cNrCaracteristica='" + carac.cNrCaracteristica + "' cSgCaracteristica='" + carac.cSgCaracteristica + "'>" +
                                                     carac.cSgCaracteristica + "</div>"; ;
                                }
                                var CtIdOpe = CaracteristicaTipificacaoDB.getCaracteristicasTipificacaoUnico(206).First().nCdCaracteristica;
                                var TIdOpe = VerificacaoTipificacaoTarefaIntegracaoDB.getTarefa(Convert.ToInt32(CtIdOpe)).First().TarefaId;
                                labels += html.div(outerhtml: listOperHtml, classe: "row items", name: "Falha Op.", tags: "listtype = multiple caracteristicatipificacaoid=" + CtIdOpe + " tarefaid=" + TIdOpe);
                                break;
                            case "Verificação Tipificação - Gordura":
                                var listGordura = CaracteristicaTipificacaoDB.getCaracteristicasTipificacao(203);
                                var listGorduraHtml = "";
                                foreach (var carac in listGordura)
                                {
                                    listGorduraHtml += "<div class='col-xs-2 hide' cNmCaracteristica='" +
                                                        carac.cNmCaracteristica + "' cIdentificador='" + carac.cIdentificador + "' " +
                                                        " cNrCaracteristica='" + carac.cNrCaracteristica + "' cSgCaracteristica='" + carac.cSgCaracteristica + "'>" +
                                                        carac.cSgCaracteristica + "</div>"; ;
                                }
                                var CtIdGor = CaracteristicaTipificacaoDB.getCaracteristicasTipificacaoUnico(203).First().nCdCaracteristica;
                                var TIdGor = VerificacaoTipificacaoTarefaIntegracaoDB.getTarefa(Convert.ToInt32(CtIdGor)).First().TarefaId;
                                labels += html.div(outerhtml: listGorduraHtml, classe: "row items", name: "Gordura", tags: "listtype = single caracteristicatipificacaoid=" + CtIdGor + " tarefaid=" + TIdGor);
                                break;
                            case "Verificação Tipificação - Contusão":
                                var listContusao = CaracteristicaTipificacaoDB.getCaracteristicasTipificacao(205);
                                var listContusaoHtml = "";
                                foreach (var carac in listContusao)
                                {
                                    listContusaoHtml += "<div class='col-xs-2 hide' cNmCaracteristica='" +
                                                        carac.cNmCaracteristica + "' cIdentificador='" + carac.cIdentificador + "' " +
                                                        " cNrCaracteristica='" + carac.cNrCaracteristica + "' cSgCaracteristica='" + carac.cSgCaracteristica + "'>" +
                                                        carac.cSgCaracteristica + "</div>"; ;
                                }
                                var CtIdCon = CaracteristicaTipificacaoDB.getCaracteristicasTipificacaoUnico(205).First().nCdCaracteristica;
                                var TIdCon = VerificacaoTipificacaoTarefaIntegracaoDB.getTarefa(Convert.ToInt32(CtIdCon)).First().TarefaId;
                                labels += html.div(outerhtml: listContusaoHtml, classe: "row items", name: "Contusão", tags: "listtype = multiple caracteristicatipificacaoid=" + CtIdCon + " tarefaid=" + TIdCon);
                                break;
                            case "Verificação Tipificação - Idade":
                                var listIdade = CaracteristicaTipificacaoDB.getCaracteristicasTipificacao(201);
                                var listIdadeHtml = "";
                                foreach (var carac in listIdade)
                                {
                                    listIdadeHtml += "<div class='col-xs-2 hide' cNmCaracteristica='" +
                                                        carac.cNmCaracteristica + "' cIdentificador='" + carac.cIdentificador + "' " +
                                                        " cNrCaracteristica='" + carac.cNrCaracteristica + "' cSgCaracteristica='" + carac.cSgCaracteristica + "'>" +
                                                        carac.cSgCaracteristica + "</div>"; ;
                                }
                                var CtIdIdade = CaracteristicaTipificacaoDB.getCaracteristicasTipificacaoUnico(201).First().nCdCaracteristica;
                                var TIdIdade = VerificacaoTipificacaoTarefaIntegracaoDB.getTarefa(Convert.ToInt32(CtIdIdade)).First().TarefaId;
                                labels += html.div(outerhtml: listIdadeHtml, classe: "row items", name: "Maturidade", tags: "listtype = single caracteristicatipificacaoid=" + CtIdIdade + " tarefaid=" + TIdIdade);
                                break;
                            case "Verificação Tipificação - Sexo":
                                var listSexo = CaracteristicaTipificacaoDB.getCaracteristicasTipificacao(207);
                                var listSexoHtml = "";
                                foreach (var carac in listSexo)
                                {
                                    listSexoHtml += "<div class='col-xs-2 hide' cNmCaracteristica='" +
                                                    carac.cNmCaracteristica + "' cIdentificador='" + carac.cIdentificador + "' " +
                                                    " cNrCaracteristica='" + carac.cNrCaracteristica + "' cSgCaracteristica='" + carac.cSgCaracteristica + "'>" +
                                                    carac.cSgCaracteristica + "</div>"; ;
                                }
                                var CtIdSexo = CaracteristicaTipificacaoDB.getCaracteristicasTipificacaoUnico(207).First().nCdCaracteristica;
                                var TIdSexo = VerificacaoTipificacaoTarefaIntegracaoDB.getTarefa(Convert.ToInt32(CtIdSexo)).First().TarefaId;
                                labels += html.div(outerhtml: listSexoHtml, classe: "row items", name: "Sexo", tags: "listtype = single caracteristicatipificacaoid=" + CtIdSexo + " tarefaid=" + TIdSexo);
                                break;
                        }

                        //gera os labels
                        labels = html.div(
                                                outerhtml: labels,
                                                classe: "col-xs-12 col-sm-12 col-md-12"
                                            );

                        //Comandos para intervalos
                        //tags += " weight=\"" + parLevel3.Weight + "\" intervalmin=\"" + parLevel3.IntervalMin + "\" intervalmax=\"" + parLevel3.IntervalMax + "\"";
                        tags += " weight=\"" + parLevel3.Weight + "\" intervalmin=\"" + parLevel3.IntervalMin + "\" intervalmax=\"" + parLevel3.IntervalMax + "\" weievaluation=\"0\" inputtype=\"1\"";
                        //Gera uma linha de level3
                        string level3List = html.listgroupItem(
                                                                id: parLevel3.Id.ToString(),
                                                                classe: "level3 row VF",
                                                                tags: tags,
                                                                outerhtml: level3 +
                                                                            labels
                                                            );

                        parLevel3Group += level3List;
                        Last_Id = parLevel3.Id;

                    }
                }

                var listAreasParticipantes = CaracteristicaTipificacaoDB.getAreasParticipantes();
                var items = "";

                foreach (var area in listAreasParticipantes)
                {
                    items += "<div class='col-xs-3 hide' cNmCaracteristica='" + area.cNmCaracteristica + "' cIdentificador='" + area.cIdentificador + "' " +
                            " cNrCaracteristica='" + area.cNrCaracteristica + "' cSgCaracteristica='" + area.cSgCaracteristica + "'>" +
                            area.cNmCaracteristica + "</div>";
                }

                var CtIdAP = CaracteristicaTipificacaoDB.getAreasParticipantesUnico().First().nCdCaracteristica;
                var TIdAP = VerificacaoTipificacaoTarefaIntegracaoDB.getTarefa(Convert.ToInt32(CtIdAP)).First().TarefaId;
                var areasParticipantes = html.listgroupItem(
                                                id: "400",
                                                classe: "level3 row VF",
                                                tags: "listtype=multiple",
                                                outerhtml: html.link(
                                                                outerhtml: html.span(outerhtml: "Areas Participantes", classe: "levelName"),
                                                                classe: "col-xs-12 col-sm-12 col-md-12"
                                                                ) +
                                                           html.div(
                                                                outerhtml: html.div(outerhtml: items, classe: "items row", name: "Areas Participantes", tags: "listtype = multiple caracteristicatipificacaoid=" + CtIdAP + " tarefaid=" + TIdAP),
                                                                classe: "col-xs-12 col-sm-12 col-md-12"
                                                                )
                                            );

                parLevel3Group = areasParticipantes + parLevel3Group;

                var painelLevel3HeaderListHtml = "";

                var labelSequencial = "<label class='font-small'>Sequencial</label>";
                var formControlSequencial = "<input class='form-control input-sm sequencial' style='font-size:30px; height: 50px; text-align:center;' type='number'>";
                var formGroupSequencial = html.div(
                                        outerhtml: labelSequencial + formControlSequencial,
                                        classe: "form-group",
                                        style: "margin-bottom: 4px;"
                                        );

                var labelBanda = "<label class='font-small'>Banda</label>";

                ////var formControlBanda = "<input class='form-control input-sm banda' style='font-size:30px; height: 50px; text-align:center;' type='number'>";

                //var formControlBanda = "<select class='form-control input-sm banda' style='font-size:30px; height: 50px; text-align:center;'><option value = '1'>1</option><option value='2'>2</option></select>";


                var formControlBanda = "<input class='form-control input-sm banda' min='1' max='2' style='font-size:30px; height: 50px; text-align:center;' type='number'>";
                var formGroupBanda = html.div(
                                        outerhtml: labelBanda + formControlBanda,
                                        classe: "form-group",
                                        style: "margin-bottom: 4px;"
                                        );

                painelLevel3HeaderListHtml += html.div(
                                                outerhtml: formGroupSequencial,
                                                classe: "col-xs-6 col-sm-4 col-md-3 col-lg-2",
                                                style: "padding-right: 4px !important; padding-left: 4px !important;"
                                                );

                painelLevel3HeaderListHtml += html.div(
                                                outerhtml: formGroupBanda,
                                                classe: "col-xs-6 col-sm-4 col-md-3 col-lg-2",
                                                style: "padding-right: 4px !important; padding-left: 4px !important;"
                                                );

                //Avaliações e amostas para painel
                string avaliacoeshtml = html.div(
                                    outerhtml: "<label class=\"font-small\" style=\"display:inherit\">Avaliações</label><label style=\"display:inline-block; font-size: 20px;\">" + html.span(classe: "evaluateCurrent") + " / " + html.span(classe: "evaluateTotal") + "</label>",
                                    style: "margin-bottom: 4px;",
                                    classe: "form-group");
                string amostrashtml = html.div(
                                    outerhtml: "<label class=\"font-small\" style=\"display:inherit\">Amostras</label><label style=\"display:inline-block; font-size: 20px;\">" + html.span(classe: "sampleCurrent") + " / " + html.span(classe: "sampleTotal") + "</label>",
                                    style: "margin-bottom: 4px;",
                                    classe: "form-group");

                string avaliacoes = html.div(
                                    outerhtml: avaliacoeshtml,
                                    style: "padding-right: 4px !important; padding-left: 4px !important;",
                                    classe: "col-xs-6 col-sm-4 col-md-3 col-lg-2");
                string amostras = html.div(
                                    outerhtml: amostrashtml,
                                    style: "padding-right: 4px !important; padding-left: 4px !important;",
                                    classe: "col-xs-6 col-sm-4 col-md-3 col-lg-2");

                //Painel
                //O interessante é um painel só mas no momento está um painel para cada level3group

                painellevel3 = html.listgroupItem(
                                                            outerhtml: avaliacoes +
                                                                       amostras +
                                                                       painelLevel3HeaderListHtml,

                                               classe: "painel painelLevel03 row") +
                                html.div(outerhtml: "teste", classe: "painel counters row", style: "background-color: #ff0000");

                //Se tiver level3 gera o agrupamento no padrão
                if (!string.IsNullOrEmpty(parLevel3Group) && ParLevel1.HasGroupLevel2 != true)
                {
                    parLevel3Group = html.div(
                                               classe: "level3Group VF",
                                               tags: "level1id=\"" + ParLevel1.Id + "\" level2id=\"" + ParLevel2.Id + "\"",

                                                   outerhtml: painellevel3 +
                                                              parLevel3Group
                                                 );
                }
                return parLevel3Group;
            }
            //Tela do PCC1B
            else if (tipoTela.Equals("PCC1B"))
            {
                //Instancia uma veriavel para gerar o agrupamento
                string parLevel3Group = null;

                foreach (var parLevel3 in parlevel3List)
                {
                    if (Last_Id != parLevel3.Id)
                    {
                        //Define a qual classe de input pertence o level3
                        string classInput = null;
                        //Labels que mostrar informaçãoes do tipo de input
                        string labelsInputs = null;
                        //tipo de input
                        string input = getTipoInput(parLevel3, ref classInput, ref labelsInputs);

                        string level3List = html.level3(parLevel3, input, classInput, labelsInputs);
                        parLevel3Group += level3List;
                        Last_Id = parLevel3.Id;
                    }
                }

                //Avaliações e amostas para painel

                var painelLevel3HeaderListHtml = "";

                var labelSequencial = "<label class='font-small'>Sequencial</label>";
                var formControlSequencial = "<input class='form-control input-sm sequencial' style='font-size:100px; height: 150px; text-align:center;' type='number'>";
                var formGroupSequencial = html.div(
                                        outerhtml: labelSequencial + formControlSequencial,
                                        classe: "form-group",
                                        style: "margin-bottom: 4px;"
                                        );

                var labelBanda = "<label class='font-small'>Banda</label>";
                var formControlBanda = "<select class='form-control input-sm banda' style='font-size:100px; height: 150px; text-align:center;'><option value='1'>1</option><option value='2'>2</option></select>";
                var formGroupBanda = html.div(
                                        outerhtml: labelBanda + formControlBanda,
                                        classe: "form-group",
                                        style: "margin-bottom: 4px;"
                                        );

                painelLevel3HeaderListHtml += html.div(
                                                outerhtml: formGroupSequencial,
                                                classe: "col-xs-8 col-sm-6 col-md-6 col-lg-6",
                                                style: "padding-right: 4px !important; padding-left: 4px !important;"
                                                );

                painelLevel3HeaderListHtml += html.div(
                                                outerhtml: formGroupBanda,
                                                classe: "col-xs-2 col-sm-2 col-md-2 col-lg-2",
                                                style: "padding-right: 4px !important; padding-left: 4px !important;"
                                                );

                var button = html.button(classe: "btn btn-lg btn-success pull-right", label: "<i class='fa fa-bookmark' aria-hidden='true'></i>");

                painelLevel3HeaderListHtml += html.div(
                                                outerhtml: button,
                                                classe: "col-xs-2 col-sm-4 col-md-4 col-lg-4",
                                                style: "padding-right: 4px !important; padding-left: 4px !important;"
                                                );

                //Avaliações e amostas para painel
                string totalnchtml = html.div(
                                    outerhtml: "<label class=\"font-small text-center\" style=\"display:inherit\">Total NC</label><label class='text-center' style=\"display:inherit; font-size: 20px;\">" + html.span(classe: "totalnc") + "</label>",
                                    style: "margin-bottom: 4px;",
                                    classe: "form-group");

                string ncdianteirohtml = html.div(
                                    outerhtml: "<label class=\"font-small text-center\" style=\"display:inherit\">NC Dianteiro</label><label class='text-center' style=\"display:inherit; font-size: 20px;\">" + html.span(classe: "ncdianteiro") + "</label>",
                                    style: "margin-bottom: 4px;",
                                    classe: "form-group");

                string nctraseirohtml = html.div(
                                    outerhtml: "<label class=\"font-small text-center\" style=\"display:inherit\">NC Traseiro</label><label class='text-center' style=\"display:inherit; font-size: 20px;\">" + html.span(classe: "nctraseiro") + "</label>",
                                    style: "margin-bottom: 4px;",
                                    classe: "form-group");

                string niveishtml = html.div(
                                    outerhtml: "<label class=\"font-small text-center\" style=\"display:inherit\">Níveis</label><label class='text-center' style=\"display:inherit; font-size: 20px;\">" + html.span(classe: "nivel1") + "  -  " + html.span(classe: "nivel2") + "  -  " + html.span(classe: "nivel3") + "</label>",
                                    style: "margin-bottom: 4px;",
                                    classe: "form-group");

                string totalnc = html.div(
                                    outerhtml: totalnchtml,
                                    style: "padding-right: 4px !important; padding-left: 4px !important;",
                                    classe: "col-xs-2");

                string ncdianteiro = html.div(
                                    outerhtml: ncdianteirohtml,
                                    style: "padding-right: 4px !important; padding-left: 4px !important;",
                                    classe: "col-xs-2");

                string nctraseiro = html.div(
                                    outerhtml: nctraseirohtml,
                                    style: "padding-right: 4px !important; padding-left: 4px !important;",
                                    classe: "col-xs-2");

                string niveis = html.div(
                                    outerhtml: niveishtml,
                                    style: "padding-right: 4px !important; padding-left: 4px !important;",
                                    classe: "col-xs-6");

                string avaliacoeshtml = html.div(
                                    outerhtml: "<label class=\"font-small\" style=\"display:inherit\">Avaliações</label><label style=\"display:inline-block; font-size: 20px;\">" + html.span(classe: "evaluateCurrent") + " / " + html.span(classe: "evaluateTotal") + "</label>",
                                    style: "margin-bottom: 4px;",
                                    classe: "form-group");
                string amostrashtml = html.div(
                                    outerhtml: "<label class=\"font-small\" style=\"display:inherit\">Amostras</label><label style=\"display:inline-block; font-size: 20px;\">" + html.span(classe: "sampleCurrent") + " / " + html.span(classe: "sampleTotal") + "</label>",
                                    style: "margin-bottom: 4px;",
                                    classe: "form-group");

                string avaliacoes = html.div(
                                    outerhtml: avaliacoeshtml,
                                    style: "padding-right: 4px !important; padding-left: 4px !important;",
                                    classe: "col-xs-6 col-sm-4 col-md-3 col-lg-2 hide");
                string amostras = html.div(
                                    outerhtml: amostrashtml,
                                    style: "padding-right: 4px !important; padding-left: 4px !important;",
                                    classe: "col-xs-6 col-sm-4 col-md-3 col-lg-2 hide");

                painellevel3 = html.listgroupItem(
                                                            outerhtml: amostras + avaliacoes + totalnc + ncdianteiro + nctraseiro + niveis + painelLevel3HeaderListHtml,

                                               classe: "painel painelLevel03 row") +
                             html.div(outerhtml: "teste", classe: "painel counters row", style: "background-color: #ff0000");

                //Se tiver level3 gera o agrupamento no padrão
                if (!string.IsNullOrEmpty(parLevel3Group))
                {
                    parLevel3Group = html.div(
                                               classe: "level3Group PCC1B",
                                               tags: "level1id=\"" + ParLevel1.Id + "\" level2id=\"" + ParLevel2.Id + "\"",

                                               outerhtml: painellevel3 +
                                                          parLevel3Group
                                             );
                }
                return parLevel3Group;
            }
            //Tela Genérica
            else
            {
                //Instancia uma veriavel para gerar o agrupamento
                string parLevel3Group = null;

                var parlevel3GroupByLevel2 = parlevel3List.GroupBy(p => p.ParLevel3Group_Id);

                foreach (var parLevel3GroupLevel2 in parlevel3GroupByLevel2)
                {
                    string accordeonName = null;
                    string acoordeonId = null;
                    string level3Group = null;

                    foreach (var parLevel3 in parLevel3GroupLevel2)
                    {

                        if (Last_Id != parLevel3.Id)
                        {

                            if (parLevel3.ParLevel3Group_Id > 0)
                            {
                                accordeonName = parLevel3.ParLevel3Group_Name;
                                acoordeonId = parLevel3.ParLevel3Group_Id.ToString() + ParLevel2.Id.ToString();
                            }

                            //Define a qual classe de input pertence o level3
                            string classInput = null;
                            //Labels que mostrar informaçãoes do tipo de input
                            string labelsInputs = null;
                            //tipo de input
                            string input = getTipoInput(parLevel3, ref classInput, ref labelsInputs);

                            string level3List = html.level3(parLevel3, input, classInput, labelsInputs);
                            level3Group += level3List;

                            Last_Id = parLevel3.Id;
                        }
                    }

                    if (!string.IsNullOrEmpty(acoordeonId))
                    {
                        haveAccordeon = true;
                        level3Group = html.accordeon(
                                                        id: acoordeonId + "Level3", 
                                                        label: accordeonName, 
                                                        outerhtml: level3Group,
                                                        classe: "row"
                                                    );
                    }

                    parLevel3Group += level3Group;

                }

                //< div class="form-group">
                //      <label for="email" style="
                //    display: inherit;
                //">Email:</label>
                //      <label for="email" style="display: inline-block">Email:</label>
                //    </div>

                //Avaliações e amostas para painel
                string avaliacoeshtml = html.div(
                                    outerhtml: "<label class=\"font-small\" style=\"display:inherit\">Avaliações</label><label style=\"display:inline-block; font-size: 20px;\">" + html.span(classe: "evaluateCurrent") + " / " + html.span(classe: "evaluateTotal") + "</label>",
                                    style: "margin-bottom: 4px;",
                                    classe: "form-group");
                string amostrashtml = html.div(
                                    outerhtml: "<label class=\"font-small\" style=\"display:inherit\">Amostras</label><label style=\"display:inline-block; font-size: 20px;\">" + html.span(classe: "sampleCurrent") + " / " + html.span(classe: "sampleTotal") + "</label>",
                                    style: "margin-bottom: 4px;",
                                    classe: "form-group");

                string avaliacoes = html.div(
                                    outerhtml: avaliacoeshtml,
                                    style: "padding-right: 4px !important; padding-left: 4px !important;",
                                    classe: "col-xs-6 col-sm-4 col-md-3 col-lg-2");
                string amostras = html.div(
                                    outerhtml: amostrashtml,
                                    style: "padding-right: 4px !important; padding-left: 4px !important;",
                                    classe: "col-xs-6 col-sm-4 col-md-3 col-lg-2");

                //Painel
                //O interessante é um painel só mas no momento está um painel para cada level3group

                var painelLevel3HeaderListHtml = GetHeaderHtml(
                    ParLevelHeaderDB.getHeaderByLevel1Level2(ParLevel1.Id, ParLevel2.Id), ParFieldTypeDB, html, ParLevel1.Id, ParLevel2.Id, ParLevelHeaderDB);

                //string HeaderLevel02 = null;

                string accordeonbuttons = null;
                if (haveAccordeon == true)
                {
                    accordeonbuttons = "<button class=\"btn btn-default button-expand marginRight10\"><i class=\"fa fa-expand\" aria-hidden=\"true\"></i> Mostrar Todos</button>" +
                                       "<button class=\"btn btn-default button-collapse\"><i class=\"fa fa-compress\" aria-hidden=\"true\"></i> Fechar Todos</button>";
                }

                painellevel3 = html.listgroupItem(
                                                            outerhtml: avaliacoes +
                                                                       amostras +
                                                                       painelLevel3HeaderListHtml,

                                               classe: "painel painelLevel03 row") +
                                                html.div(outerhtml: "teste", classe: "painel counters row", style: "background-color: #ff0000");

                string panelButton = html.listgroupItem(
                                                           outerhtml: accordeonbuttons +
                                                                      "<button id='btnAllNA' class='btn btn-warning btn-sm pull-right btnAllNA'> Todos N/A </button>" +

                                                                      "<button id='btnAllNC' class='btn btn-danger btn-sm pull-right  btnAllNC' style='margin-right: 10px;'> Clicar em Todos </button>",
                                                           classe: "painel painelLevel02 row"
                                                        );

                //Se tiver level3 gera o agrupamento no padrão
                if (!string.IsNullOrEmpty(parLevel3Group) && ParLevel1.HasGroupLevel2 != true)
                {
                    parLevel3Group = html.div(
                                               classe: "level3Group",
                                               tags: "level1id=\"" + ParLevel1.Id + "\" level2id=\"" + ParLevel2.Id + "\"",

                                               outerhtml: painellevel3 + panelButton +
                                                          parLevel3Group
                                             );
                }
                return parLevel3Group;
            }


        }
        /// <summary>
        /// Gera o input para level3
        /// </summary>
        /// <param name="parLevel3">ParLevel3</param>
        /// <param name="classInput">Classe de Input</param>
        /// <param name="labels">Labels do Input</param>
        /// <returns></returns>
        public string getTipoInput(SGQDBContext.ParLevel3 parLevel3, ref string classInput, ref string labels)
        {
            var html = new Html();
            string input = null;
            if (parLevel3.ParLevel3InputType_Id == 1)
            {
                classInput = " boolean";
                input = html.campoBinario(parLevel3.Id.ToString(), parLevel3.ParLevel3BoolTrue_Name, parLevel3.ParLevel3BoolFalse_Name);
            }
            else if (parLevel3.ParLevel3InputType_Id == 2)
            {
                classInput = " defects";
                labels = html.div(
                                           outerhtml: "<b>Max: </b>" + parLevel3.IntervalMax.ToString("G29"),
                                           classe: "font10",
                                           style: "font-size: 11px; margin-top:7px;"
                                       );

                input = html.campoNumeroDeDefeitos(id: parLevel3.Id.ToString(),
                                                intervalMin: parLevel3.IntervalMin,
                                                intervalMax: parLevel3.IntervalMax,
                                                unitName: parLevel3.ParMeasurementUnit_Name);
            }
            else if (parLevel3.ParLevel3InputType_Id == 3)
            {
                classInput = " interval";
                labels = html.div(
                                           outerhtml: "<b>Min: </b>" + parLevel3.IntervalMin.ToString("G29") + " ~ <b>Max: </b>" + parLevel3.IntervalMax.ToString("G29") + " " + parLevel3.ParMeasurementUnit_Name,
                                           classe: "font10",
                                           style: "font-size: 11px; margin-top:7px;"
                                       );

                input = html.campoIntervalo(id: parLevel3.Id.ToString(),
                                                intervalMin: parLevel3.IntervalMin,
                                                intervalMax: parLevel3.IntervalMax,
                                                unitName: parLevel3.ParMeasurementUnit_Name);
            }
            else if (parLevel3.ParLevel3InputType_Id == 4)
            {
                classInput = " calculado";

                var intervalMin = Guard.ConverteValorCalculado(parLevel3.IntervalMin);
                var intervalMax = Guard.ConverteValorCalculado(parLevel3.IntervalMax);

                labels = html.div(
                                           outerhtml: "<b>Min: </b> " + Guard.ConverteValorCalculado(parLevel3.IntervalMin) + " ~ <b>Max: </b>" + Guard.ConverteValorCalculado(parLevel3.IntervalMax) + " " + parLevel3.ParMeasurementUnit_Name,
                                           classe: "font10",
                                           style: "font-size: 11px; margin-top:7px;"
                                       );

                input = html.campoCalculado(id: parLevel3.Id.ToString(),
                                                intervalMin: parLevel3.IntervalMin,
                                                intervalMax: parLevel3.IntervalMax,
                                                unitName: parLevel3.ParMeasurementUnit_Name);
            }
            else
            {
                ///Campo interval está repetindo , falta o campo defeitos
                classInput = " interval";

                labels = html.div(
                                    outerhtml: "<b>Min: </b>" + parLevel3.IntervalMin.ToString("G29") + " ~ <b>Max: </b>" + parLevel3.IntervalMax.ToString("G29") + " " + parLevel3.ParMeasurementUnit_Name,
                                    classe: "font10",
                                    style: "font-size: 11px; margin-top:7px;"
                                );

                input = html.campoIntervalo(id: parLevel3.Id.ToString(),
                                                intervalMin: parLevel3.IntervalMin,
                                                intervalMax: parLevel3.IntervalMax,
                                                unitName: parLevel3.ParMeasurementUnit_Name);
            }
            return input;
        }
        public string getTipoInputBEA(SGQDBContext.ParLevel3 parLevel3, ref string classInput, ref string labels)
        {
            var html = new Html();
            string input = null;
            classInput = " defects";
            labels = html.div(
                                       classe: "font10",
                                       style: "font-size: 11px; margin-top:7px;"
                                   );

            input = html.campoNumeroDeDefeitos(id: parLevel3.Id.ToString(),
                                            intervalMin: parLevel3.IntervalMin,
                                            intervalMax: parLevel3.IntervalMax,
                                            unitName: parLevel3.ParMeasurementUnit_Name);
            return input;
        }

        //public string GetLevel03_novo(SGQDBContext.ParLevel1 ParLevel1, SGQDBContext.ParLevel2 ParLevel2)
        //{
        //    var html = new Html();


        //    var parlevel3List = ParLevel3DB.getLevel3ByLevel2(ParLevel2.Id);

        //    string btnNaoAvaliado = html.button(
        //                               label: html.span(
        //                                                 classe: "cursorPointer iconsArea",
        //                                                 outerhtml: "N/A"
        //                                                ),
        //                               classe: "btn-warning btnNotAvaliable na font11"
        //                           );

        //    string parLevel3Group = null;

        //string panelButton = html.listgroupItem(outerhtml: "<button id='btnAllNA' class='btn btn-warning btn-sm pull-right'> Todos N/A </button>",
        //                                            classe: "painel painelLevel02 row"
        //                                        );



        //    foreach (var parLevel3 in parlevel3List)
        //    {

        //        string classInput = null;
        //        string tags = null;
        //        string labels = null;
        //        string input = null;

        //        if (parLevel3.ParLevel3InputType_Id == 1)
        //        {
        //            classInput = " boolean";
        //            input = html.campoBinario(parLevel3.Id.ToString(), parLevel3.ParLevel3BoolTrue_Name, parLevel3.ParLevel3BoolFalse_Name);

        //        }
        //        else
        //        {
        //            classInput = " interval";
        //            tags = "intervalmin=\"" + parLevel3.IntervalMin + "\" intervalmax=\"" + parLevel3.IntervalMax + "\"";

        //            labels = html.div(
        //                             outerhtml: "<b>Min: </b>" + parLevel3.IntervalMin.ToString() + " ~ <b>Max: </b>" + parLevel3.IntervalMax.ToString() + " " + parLevel3.ParMeasurementUnit_Name,
        //                             classe: "font10",
        //                             style: "font-size: 11px; margin-top:7px;"
        //                           );

        //            input = html.campoIntervalo(id: parLevel3.Id.ToString(),
        //                                           intervalMin: parLevel3.IntervalMin,
        //                                           intervalMax: parLevel3.IntervalMax,
        //                                           unitName: parLevel3.ParMeasurementUnit_Name);

        //        }

        //        string level3 = html.link(
        //                                   outerhtml: html.span(outerhtml: parLevel3.Name, classe: "levelName"),
        //                                   classe: "col-xs-4"
        //                                  );
        //        labels = html.div(
        //                                outerhtml: labels,
        //                                classe: "col-xs-3"
        //                            );
        //        string counters = html.div(
        //                                    outerhtml: input,
        //                                    classe: "col-xs-3 counters cursorPointer"
        //                                  );
        //        string buttons = html.div(
        //                                   outerhtml: btnNaoAvaliado,
        //                                   classe: "col-xs-2",
        //                                   style: "text-align:right"
        //                                 );

        //        tags += " weight=\"" + parLevel3.Weight + "\" intervalmin=\"" + parLevel3.IntervalMin + "\" intervalmax=\"" + parLevel3.IntervalMax + "\"";

        //        string level3List = html.listgroupItem(
        //                                              id: parLevel3.Id.ToString(),
        //                                              classe: "level3 row" + classInput,
        //                                              tags: tags,
        //                                              outerhtml: level3 +
        //                                                         labels +
        //                                                         counters +
        //                                                         buttons
        //                                            );

        //        parLevel3Group += level3List;
        //    }

        //    string avaliacoes = html.div(
        //                      outerhtml: "<b style=\"width:100px;display:inline-block\">Avaliações</b>" + html.span(classe: "evaluateCurrent") + " / " + html.span(classe: "evaluateTotal"),
        //                    style: "font-size: 16px");
        //    string amostrar = html.div(
        //                                  outerhtml: "<b style=\"width:100px;display:inline-block\">Amostras</b>" + html.span(classe: "sampleCurrent") + " / " + html.span(classe: "sampleTotal"),
        //                                style: "font-size: 16px");


        //    string painellevel3 = html.listgroupItem(
        //                                                outerhtml: avaliacoes +
        //                                                           amostrar,

        //                                   classe: "painel painelLevel03 row");



        //    return parLevel3Group;

        //}
        public string GetLoginAPP()
        {
            var html = new Html();
            string head = html.div(classe: "head");

            //Verifica as configurações iniciais da tela
            var ParConfSGQDB = new SGQDBContext.ParConfSGQContext();
            var configuracoes = ParConfSGQDB.get();


            #region form

            #region Unit
            bool inputsDesabilitados = false;

            string selectUnit = null;
            if (configuracoes != null && configuracoes.HaveUnitLogin == true)
            {
                inputsDesabilitados = true;
                //coloca as unidades vindo do banco ou mocado eua, podemos colocar um arquivo para carregar
                selectUnit = html.option("1", "Unit 1", tags: "ip=\"192.168.25.200/SgqMaster\"");
                selectUnit = html.select(selectUnit, "selectUnit");
            }

            #endregion

            #region shift
            string selectShit = null;
            if (configuracoes != null && configuracoes.HaveShitLogin == true)
            {
                inputsDesabilitados = true;
                selectShit = html.option("0", "Select the shift") +
                              html.option("1", "Shift A") +
                              html.option("2", "Shift B");

                selectShit = html.select(selectShit, id: "shift");
            }
            #endregion

            string selectUrlPreffix = html.option("http://mtzsvmqsc/SgqGlobal", "JBS") +
                                      html.option("http://192.168.25.200/SgqMaster", "GRT") +
                                      html.option("http://localhost:8090/SgqSystem", "GCN");

            string formOuterHtml = html.head(Html.h.h2, outerhtml: "Entre com seu Login") +
                                  selectUnit +
                                  selectShit +
                                  html.label(labelfor: "inputUserName", classe: "sr-only", outerhtml: "Username") +
                                  html.input(id: "inputUserName", placeholder: "Username", required: true, disabled: inputsDesabilitados) +
                                  html.label(labelfor: "inputPassword", classe: "sr-only", outerhtml: "Password") +
                                  html.input(type: Html.type.password, id: "inputPassword", placeholder: "Password", required: true, disabled: inputsDesabilitados) +
                                  html.button(label: "Entrar", id: "btnLogin", classe: "btn-lg btn-primary btn-block marginTop10", dataloading: "<i class='fa fa-spinner fa-spin'></i> <span class='wMessage' style='font-size:14px;'>Autenticando</span>") +

                                  html.div(id: "messageError", classe: "alert alert-danger hide", tags: "role=\"alert\"",
                                           outerhtml: html.span(classe: "icon-remove-sign") + "<strong>Erro! </strong>" + html.span(id: "mensagemErro")) +

                                  html.div(classe: "divLoadFiles",
                                           outerhtml: html.span(classe: "messageLoading")) +

                                  html.div(id: "messageAlert",
                                           classe: "alert alert-info hide",
                                           tags: "role=\"alert\"",
                                           outerhtml: html.span(id: "mensagemAlerta", classe: "icon-info-sign")) +

                                  html.div(id: "messageSuccess",
                                           classe: "alert alert-success hide",
                                           tags: "role=\"alert\"",
                                           outerhtml: html.span(id: "mensagemSucesso", classe: "icon-ok-circle")) + 
                                           
                                  html.select(selectUrlPreffix, "cb_UrlPreffix", "\" onChange='abreOApp(this.value);' \"");

            string form = html.form(
                                    outerhtml: formOuterHtml
                                    , classe: "form-signin");

            #endregion

            #region foot
            string footOuterHtml = html.br() +
                                   html.br() +
                                   html.br() +
                                   html.span(
                                              outerhtml: "Versão " +
                                                         html.span(classe: "number")
                                             , id: "versionLogin") +
                                   html.span(
                                               outerhtml: html.span(classe: "base")
                                             , id: "ambienteLogin"

                                            );

            string foot = html.div(
                                    outerhtml: footOuterHtml
                                    , classe: "foot", style: "text-align:center");

            #endregion

            return html.div(
                                outerhtml: head +
                                           form +
                                           foot

                                , classe: "login"
                            );
        }
        #endregion
        #region Users
        [WebMethod]
        public string getCompanyUsers(string ParCompany_Id)
        {
            var ParCompanyXUserSgqDB = new SGQDBContext.ParCompanyXUserSgq();
            var RolesXUserSgqDB = new SGQDBContext.RoleXUserSgq();

            var users = ParCompanyXUserSgqDB.getCompanyUsers(Convert.ToInt32(ParCompany_Id));
            var html = new Html();

            string usersList = null;
            foreach (var user in users)
            {
                string Password = user.UserSGQ_Pass;
                Password = Guard.Descriptografar3DES(Password);
                Password = UserDomain.EncryptStringAES(Password);

                var roles = RolesXUserSgqDB.getRoles(Convert.ToInt32(user.UserSGQ_Id), Convert.ToInt32(ParCompany_Id));

                usersList += html.user(user.UserSGQ_Id, user.UserSGQ_Name, user.UserSGQ_Login, Password, user.Role, user.ParCompany_Id, user.ParCompany_Name, roles);
            }
            return usersList;
        }
        [WebMethod]
        public string getUserCompanys(string UserSgq_Id)
        {
            var ParCompanyXUserSgqDB = new SGQDBContext.ParCompanyXUserSgq();

            var users = ParCompanyXUserSgqDB.getUserCompany(Convert.ToInt32(UserSgq_Id));
            var html = new Html();

            string usersList = null;
            foreach (var user in users)
            {
                string Password = user.UserSGQ_Pass;
                Password = Guard.Descriptografar3DES(Password);
                Password = UserDomain.EncryptStringAES(Password);

                usersList += html.user(user.UserSGQ_Id, user.UserSGQ_Name, user.UserSGQ_Login, Password, user.Role, user.ParCompany_Id, user.ParCompany_Name, null);
            }
            return usersList;
        }
        [WebMethod]
        public string UserSGQLogin(string UserName, string Password)
        {
            var UserSGQDB = new SGQDBContext.UserSGQ();
            var user = UserSGQDB.getUserByLoginOrId(userLogin: UserName.Trim());

            // Password = Guard.Descriptografar3DES("h88Xcom5qf0Ok3LCqZUm1A==");

            var html = new Html();

            Password = UserDomain.DecryptStringAES(Password);
            Password = Guard.Criptografar3DES(Password);

            if (user != null && user.Password == Password)
            {

                Password = Guard.Descriptografar3DES(Password);
                Password = UserDomain.EncryptStringAES(Password);

                if (user.ParCompany_Id == 0)
                {
                    return "A unidade padrão não foi definida";
                }

                //colocar informação que usuario não tem unidade padrão, mas tem que verificar isso
                return html.user(user.Id, user.Name, user.Login, Password, user.Role, user.ParCompany_Id, user.ParCompany_Name, null);
            }
            else
            {
                return "Usuário ou senha inválidos";
            }

        }
        [WebMethod]
        public string UserSGQById(int Id)
        {
            var UserSGQDB = new SGQDBContext.UserSGQ();
            var user = UserSGQDB.getUserByLoginOrId(id: Id);

            var html = new Html();
            if (user != null)
            {
                string Password = Guard.Criptografar3DES(user.Password);
                Password = UserDomain.EncryptStringAES(Password);

                return html.user(user.Id, user.Name, user.Login, Password, user.Role, user.ParCompany_Id, user.ParCompany_Name, null);
            }

            return "Usuário não localizado";
        }
        #endregion
        [WebMethod]
        public string insertDeviation(string deviations)
        {

            if(string.IsNullOrEmpty(deviations))
            {
                return null;
            }
            //var result = deviation.attr('parcompany_id'); // 0
            //result += ";" + deviation.attr('parlevel1_id'); // 1  
            //result += ";" + deviation.attr('parlevel2_id');// 2
            //result += ";" + deviation.attr('evaluation');// 3
            //result += ";" + deviation.attr('sample');// 4
            //result += ";" + deviation.attr('alertnumber');// 5
            //result += ";" + deviation.attr('defects');// 6
            //result += ";" + deviation.attr('deviationdate');// 7

            deviations = deviations.Replace("</deviation><deviation>", "&").Replace("<deviation>", "").Replace("</deviation>", "");
            var arrayDeviations = deviations.Split('&');


            string sql = null;
            for (int i = 0; i < arrayDeviations.Length; i++)
            {
                string[] deviation = arrayDeviations[i].Split(';');

                string ParCompany_Id = deviation[0];
                string ParLevel1_Id = deviation[1];
                string ParLevel2_Id = deviation[2];
                string Evaluation = deviation[3];
                string Sample = deviation[4];
                string alertNumber = deviation[5];
                string defects = deviation[6];
                string deviationDate = deviation[7];
                string deviationMessage = deviation[8];
                if(string.IsNullOrEmpty(deviationMessage))
                {
                    deviationMessage = "null";
                }
                else
                {
                    deviationMessage = "'" + deviationMessage + "'";
                }

                sql += "INSERT INTO Deviation ([ParCompany_Id],[ParLevel1_Id],[ParLevel2_Id],[Evaluation],[Sample],[AlertNumber],[Defects],[DeviationDate],[AddDate],[sendMail], [DeviationMessage]) " +
                        "VALUES " +
                        "('" + ParCompany_Id + "' ,'" + ParLevel1_Id + "','" + ParLevel2_Id + "','" + Evaluation + "','" + Sample + "','" + alertNumber + "','" + defects + "', GetDate() , GetDate(), 0, " + deviationMessage + ")";
            }

            //string sql = null;
            //for (int i = 0; i < arrayDeviations.Length; i++)
            //{
            //    var deviation = arrayDeviations[i].Split(';');


            //}

            string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        var i = Convert.ToInt32(command.ExecuteNonQuery());
                        //Se o registro for inserido retorno o Id da Consolidação
                        if (i > 0)
                        {
                            return null;
                        }
                        else
                        {
                            //Caso ocorra algum erro, retorno zero
                            return null;
                        }
                    }
                }
            }
            //Caso ocorra alguma Exception, grava o log e retorna zero
            catch (SqlException ex)
            {
                int insertLog = insertLogJson(sql, ex.Message, "N/A", "N/A", "insertDeviation");
                return "error";
            }
            catch (Exception ex)
            {
                int insertLog = insertLogJson(sql, ex.Message, "N/A", "N/A", "insertDeviation");
                return "error";
            }
        }
        [WebMethod]
        public string sendEmailAlerta()
        {
            string destinatarios = "antoniobrissolare@hotmail.com";
            string mensagemEstouro = "Estouro de alerta Nivel [X]";

            try
            {
                string termo = "<div style='font-family:Verdana; font-size:10px;color:gray'>Este email é direcionado apenas para a pessoa ou entidade para a qual foi endreçado e pode conter material confidencial ou privilegiado. Qualquer leitura, uso, revelação ou distribuição não autorizados são proibidos. Se você não for o destinatário dessa mensagem, mas não deseja receber mensagens através desse meio, por gentileza, avise o remetente imediatamente</div>";

                string emailRemetente = "services@brzsoftwares.com";
                string nomeRemetente = "SGQ - GRT Soluções";

                MailMessage mailMessage = new MailMessage();
                //Endereço que irá aparecer no e-mail do usuário 
                mailMessage.From = new MailAddress(emailRemetente, nomeRemetente);
                //destinatarios do e-mail, para incluir mais de um basta separar por ponto e virgula  
                mailMessage.To.Add(destinatarios);

                mailMessage.Subject = "Alerta";
                mailMessage.IsBodyHtml = true;
                //conteudo do corpo do e-mail 
                mailMessage.Body = "<div style='font-family:Verdana; font-size:14px'>" + mensagemEstouro + "</div>" +
                                   "<br><br>" +

                                   "<div style='font-family:Verdana; font-size:10px;color:gray'>Esta é uma mensagem automática, por favor não responda. Antes de imprimir pense em seu compromisso com o meio ambiente.</div>" +
                                   "<br>" +
                                   termo +
                                   "<div style='font-family:Verdana; font-size:8px;color:gray'>GRT Soluções Alertas " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "</div>";

                mailMessage.Priority = MailPriority.High;
                string servidorSMTP = "mail.brzsoftwares.com";
                int portaSMS = 587;
                string usuarioSMTP = "services@brzsoftwares.com";
                string senhaSMTP = "#Abn32878732";
                //smtp do e-mail que irá enviar 
                SmtpClient smtpClient = new SmtpClient(servidorSMTP, portaSMS);
                smtpClient.EnableSsl = false;
                //credenciais da conta que utilizará para enviar o e-mail 
                smtpClient.Credentials = new NetworkCredential(usuarioSMTP, senhaSMTP);
                smtpClient.Send(mailMessage);
                return null;
            }
            catch (Exception ex)
            {
                int insertLog = insertLogJson(mensagemEstouro, ex.Message, null, null, "sendEmail");
            }
            return null;
        }
        [WebMethod]
        public string sendEmail(string email, string subject, string body, string email_CopiaOculta = null)
        {
            string destinatarios = email;

            try
            {
                string termo = "<div style='font-family:Verdana; font-size:10px;color:gray'>Este email é direcionado apenas para a pessoa ou entidade para a qual foi endreçado e pode conter material confidencial ou privilegiado. Qualquer leitura, uso, revelação ou distribuição não autorizados são proibidos. Se você não for o destinatário dessa mensagem, mas não deseja receber mensagens através desse meio, por gentileza, avise o remetente imediatamente</div>";

                string emailRemetente = "suporte.grt@grtsolucoes.com.br";
                string nomeRemetente = "GRT Soluções";

                MailMessage mailMessage = new MailMessage();
                //Endereço que irá aparecer no e-mail do usuário 
                mailMessage.From = new MailAddress(emailRemetente, nomeRemetente);
                if (!string.IsNullOrEmpty(email_CopiaOculta))
                {
                    mailMessage.Bcc.Add(new MailAddress(email_CopiaOculta));
                }
                //destinatarios do e-mail, para incluir mais de um basta separar por ponto e virgula  
                mailMessage.To.Add(destinatarios);

                mailMessage.Subject = subject;
                mailMessage.IsBodyHtml = true;
                //conteudo do corpo do e-mail 

                mailMessage.Body = "<div style='font-family:Verdana; font-size:14px'>" + body + "</div><br><br>" +
                                   "<div style='font-family:Verdana; font-size:10px;color:gray'>Esta é uma mensagem automática, por favor não responda. Antes de imprimir pense em seu compromisso com o meio ambiente.</div>" +
                                   "<br>" +
                                   termo +
                                   "<div style='font-family:Verdana; font-size:8px;color:gray'>GRT Soluções " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "</div>";

                mailMessage.Priority = MailPriority.High;

                string servidorSMTP = "mail.brzsoftwares.com";
                int portaSMS = 587;
                string usuarioSMTP = "services@brzsoftwares.com";
                string senhaSMTP = "#Abn32878732";


                ///************LOCAWEB GRT
                //string servidorSMTP = "email-ssl.com.br";

                //int portaSMS = 587;
                //string usuarioSMTP = "suporte.grt@grtsolucoes.com.br";
                //string senhaSMTP = "1qazmko0#";
                ////smtp do e-mail que irá enviar 

                ////*************
                SmtpClient smtpClient = new SmtpClient(servidorSMTP, portaSMS);
                smtpClient.EnableSsl = true;
                //credenciais da conta que utilizará para enviar o e-mail 
                smtpClient.Credentials = new NetworkCredential(usuarioSMTP, senhaSMTP);
                smtpClient.Send(mailMessage);
                return null;
            }
            catch (Exception ex)
            {
                int insertLog = insertLogJson(email, ex.Message, null, null, "sendEmail");
            }
            return null;
        }
        [WebMethod]
        public string sendEmailManutencao()
        {
            //Converte a data no padrão de busca do Banco de Dados

            string sql = "SELECT FullName, Name, email FROM UserSgq where role='somentemanutencao-sgq' and id > 432";

            string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        using (SqlDataReader r = command.ExecuteReader())
                        {
                            while (r.Read())
                            {
                                string nome = r[0].ToString();

                                string email = r[2].ToString();
                                //  email = "antoniobrissolare@hotmail.com";
                                //  email = "antoniobrissolare@hotmail.com; bruno.sousa@grtsolucoes.com.br";

                                string login = r[1].ToString();

                                var nomeArray = nome.Split(' ');
                                string primeiroNome = nomeArray[0];



                                string mensagem = "Olá " + primeiroNome + ", bem vindo ao SGM!<br><br>" +
                                                  "Seus dados de acesso são:<br><br>" +
                                                  "<b>Acesso: </b>http://mtzsvmqsc/SgqGlobal" +
                                                  "<br><b>Usuário: </b>" + login +
                                                  "<br><b>Senha:</b> 123";


                                sendEmail(email, "Dados de acesso SGM", mensagem, email_CopiaOculta: "bruno.sousa@grtsolucoes.com.br");
                                Thread.Sleep(2000);
                            }
                            return null;
                        }
                    }
                }
            }
            //Em caso de Exception, grava um log no Banco de Dados e Retorna Zero
            catch (SqlException ex)
            {
                int insertLog = insertLogJson(sql, ex.Message, "N/A", "N/A", "GetLevel1Consolidation");
                return "erro";
            }
            catch (Exception ex)
            {
                int insertLog = insertLogJson(sql, ex.Message, "N/A", "N/A", "GetLevel1Consolidation");
                return "erro";
            }
        }
        [WebMethod]
        public string updateLevel1Consolidaton(string ParLevel1_Id, string Unit_Id, string DepartmentId, string Evaluation, string Defects)
        {
            //Adicionar o departamento
            string sql = "UPDATE ConsolidationLevel1 SET Defects='" + Defects + "', Evaluation='" + Evaluation + "' WHERE UnitId='" + Unit_Id + "' AND ParLevel1_Id='" + ParLevel1_Id + "'";
            string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        var i = Convert.ToInt32(command.ExecuteScalar());
                        //Se o registro for inserido retorno o Id da Consolidação
                        if (i > 0)
                        {
                            return null;
                        }
                        else
                        {
                            //Caso ocorra algum erro, retorno zero
                            return null;
                        }
                    }
                }
            }
            //Caso ocorra alguma Exception, grava o log e retorna zero
            catch (SqlException ex)
            {
                int insertLog = insertLogJson(sql, ex.Message, "N/A", "N/A", "updateLevel1Consolidaton");
                return "error";
            }
            catch (Exception ex)
            {
                int insertLog = insertLogJson(sql, ex.Message, "N/A", "N/A", "updateLevel1Consolidaton");
                return "error";
            }
        }
        [WebMethod]
        public string updateConsolidacoes(string consolidacoes)
        {

            string sql = "";
            string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        var i = Convert.ToInt32(command.ExecuteNonQuery());
                        if (i > 0)
                        {
                            return null;
                        }
                        else
                        {
                            return null;
                        }


                    }
                }
            }
            catch (SqlException ex)
            {
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// Seleciona Todas as unidades que o usuário pode acessar
        /// </summary>
        /// <param name="UserSgq_Id"></param>
        /// <returns></returns>
        public string selectUserCompanys(int UserSgq_Id, int ParCompany_Id)
        {
            var ParCompanyXUserSgqDB = new SGQDBContext.ParCompanyXUserSgq();
            var parCompanyXUserSgq = ParCompanyXUserSgqDB.getUserCompany(UserSgq_Id);

            string options = null;

            foreach (var p in parCompanyXUserSgq)
            {
                string selected = null;
                if (p.ParCompany_Id == ParCompany_Id)
                {
                    selected = " selected";
                }

                options += "<option" + selected + " value=\"" + p.ParCompany_Id + "\">" + p.ParCompany_Name + "</option>";
            }

            if (!string.IsNullOrEmpty(options))
            {
                options = "<select id=\"selectParCompany\" style=\"margin: 14px;\" ParCompany_Id=\"" + ParCompany_Id + "\">" + options + "</select>";
            }
            return options;
        }
        [WebMethod]
        public string UserCompanyUpdate(string UserSgq_Id, int ParCompany_Id)
        {
            //Adicionar o departamento
            string sql = "UPDATE UserSgq SET ParCompany_Id='" + ParCompany_Id + "' WHERE Id='" + UserSgq_Id + "'";
            string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        var i = Convert.ToInt32(command.ExecuteNonQuery());
                        //Se o registro for inserido retorno o Id da Consolidação
                        if (i > 0)
                        {
                            return null;
                        }
                        else
                        {
                            //Caso ocorra algum erro, retorno zero
                            return "Não foi possivel alterar a unidade";
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                return "Não foi possivel alterar a unidade";
            }
        }
        [WebMethod]
        public string InsertCorrectiveAction(string CollectionLevel2_Id, string ParLevel1_Id, string ParLevel2_Id, string Shift, string Period, string ParCompany_Id, string EvaluationNumber, string ParFrequency_Id, string data, string AuditorId, string SlaughterId, string TechinicalId, string DateTimeSlaughter, string DateTimeTechinical, string DateCorrectiveAction, string AuditStartTime, string DescriptionFailure, string ImmediateCorrectiveAction, string ProductDisposition, string PreventativeMeasure)
        {
            try
            {
                //using (var transacao = new TransactionScope())
                //{
                    SlaughterId = DefaultValueReturn(SlaughterId, "1");
                    TechinicalId = DefaultValueReturn(TechinicalId, "1");
                    DateTimeSlaughter = DefaultValueReturn(DateTimeSlaughter, "03012017 00:00:00");
                    DateTimeTechinical = DateTimeSlaughter;
                    Period = DefaultValueReturn(Period, "1");

                    if (string.IsNullOrEmpty(CollectionLevel2_Id) || CollectionLevel2_Id == "0")
                    {
                        CollectionLevel2_Id = getCollectionLevel2WithCorrectiveAction(ParLevel1_Id, ParLevel2_Id, Shift, Period, ParCompany_Id, EvaluationNumber).ToString();
                        if (CollectionLevel2_Id == "0")
                        {
                            return "error";
                        }
                    }

                    DescriptionFailure = HttpUtility.UrlDecode(DescriptionFailure, System.Text.Encoding.Default);
                    ImmediateCorrectiveAction = HttpUtility.UrlDecode(ImmediateCorrectiveAction, System.Text.Encoding.Default);
                    ProductDisposition = HttpUtility.UrlDecode(ProductDisposition, System.Text.Encoding.Default);
                    PreventativeMeasure = HttpUtility.UrlDecode(PreventativeMeasure, System.Text.Encoding.Default);

                    int id = correctiveActionInsert(AuditorId, CollectionLevel2_Id, SlaughterId, TechinicalId, DateTimeSlaughter, DateTimeTechinical, Convert.ToDateTime(DateCorrectiveAction).ToString("yyyy-MM-dd HH:mm:ss"), Convert.ToDateTime(AuditStartTime).ToString("yyyy-MM-dd HH:mm:ss"), DescriptionFailure, ImmediateCorrectiveAction, ProductDisposition, PreventativeMeasure);
                    if (id > 0)
                    {
                    //01/20/2017

                        
                        string dataInicio = null;
                        string dataFim = null;

                        if(!data.Contains("/"))
                        {
                            string dia = data.Substring(2, 2);
                            string mes = data.Substring(0, 2);
                            string ano = data.Substring(4, 4); 

                            data = ano + "/" + mes + "/" + dia;
                        }
                        DateTime dataAPP = Convert.ToDateTime(data);
                        
                        //Pega a data pela regra da frequencia
                        getFrequencyDate(Convert.ToInt32(ParFrequency_Id), dataAPP, ref dataInicio, ref dataFim);
                        var idUpdate = updateCorrectiveAction_CollectionLevel2_By_ParLevel1(ParLevel1_Id, ParCompany_Id, dataInicio, dataFim);
                        //transacao.complete();
                        return null;
                    }
                    else
                    {
                        throw new Exception();
                    }
               // }
            }
            catch (Exception ex)
            {
                int insertLog = insertLogJson("", ex.Message, "N/A", "N/A", "InsertCorrectiveAction");

                return "erro";
                throw ex;
            }
        }
        public int getCollectionLevel2WithCorrectiveAction(string ParLevel1_Id, string ParLevel2_Id, string Shift, string Period, string ParCompany_Id, string EvaluationNumber)
        {
            //Converte a data no padrão de busca do Banco de Dados

            string sql = "SELECT Id FROM CollectionLevel2 WHERE ParLevel1_Id='" + ParLevel1_Id + "' AND ParLevel2_Id='" + ParLevel2_Id + "' AND UnitId='" + ParCompany_Id + "' AND Shift='" + Shift + "' AND Period='" + Period + "' AND EvaluationNumber='" + EvaluationNumber + "' AND HaveCorrectiveAction=1";

            string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        using (SqlDataReader r = command.ExecuteReader())
                        {
                            //Se encontrar, retorna o Id da Consolidação
                            if (r.Read())
                            {
                                return Convert.ToInt32(r[0]);
                            }
                            //Se não encontrar, retorna zero
                            return 0;
                        }
                    }
                }
            }
            //Em caso de Exception, grava um log no Banco de Dados e Retorna Zero
            catch (SqlException ex)
            {
                int insertLog = insertLogJson(sql, ex.Message, "N/A", "N/A", "GetLevel1Consolidation");
                return 0;
            }
            catch (Exception ex)
            {
                int insertLog = insertLogJson(sql, ex.Message, "N/A", "N/A", "GetLevel1Consolidation");
                return 0;
            }
        }

        [WebMethod]
        public string getCollectionLevel2Keys(string ParCompany_Id, string date, int ParLevel1_Id = 0)
        {

            //Verificamos os Indicadores que já foram consolidados para a Unidade selecionada
            var ParLevel1ConsolidationXParFrequencyDB = new SGQDBContext.ParLevel1ConsolidationXParFrequency();
            //Instanciamos uma variável que irá 

            DateTime data = DateCollectConvert(date);

            var parLevel1ConsolidationXParFrequency = ParLevel1ConsolidationXParFrequencyDB.getList(Convert.ToInt32(ParCompany_Id), data);

           

            if (ParLevel1_Id > 0)
            {
                parLevel1ConsolidationXParFrequency = parLevel1ConsolidationXParFrequency.Where(p => p.ParLevel1_Id == ParLevel1_Id);
            }
            string ResultsKeys = null;
            //Percorremos as consolidações de ParLevel1
            foreach (var c in parLevel1ConsolidationXParFrequency)
            {
                //Instanciamos variavel de data
                string dataInicio = null;
                string dataFim = null;

                //Pega a data pela regra da frequencia
                getFrequencyDate(c.ParFrequency_Id, data, ref dataInicio, ref dataFim);

                //Instanciamos a tabela Resultados
                var Level2ResultDB = new SGQDBContext.Level2Result();
                var Level2ResultList = Level2ResultDB.getKeys(c.ParLevel1_Id, Convert.ToInt32(ParCompany_Id), dataInicio, dataFim);
                string listKeys = null;


                //Percorremos os resultados do indicador
                foreach (var key in Level2ResultList)
                {
                    listKeys += "<div id=\"" + key.Key + "\" class=\"collectionLevel2Key\"></div>";
                }

                if(!string.IsNullOrEmpty(listKeys))
                {
                    ResultsKeys += "<div parlevel1_id=\"" + c.ParLevel1_Id + "\" class=\"ResultLevel2Key\">" +
                                        listKeys +
                                   "</div>";
                }
            }
            return ResultsKeys;
        }

        [WebMethod]
        public string _ConsolidationByCollectionLevel2()
        {
            //Converte a data no padrão de busca do Banco de Dados

            string sql = "select ParLevel1_Id, ParLevel2_Id, UnitId,  CAST(CollectionDate AS DATE) from CollectionLevel2 GROUP BY ParLevel1_Id, ParLevel2_Id, UnitId,  CAST(CollectionDate AS DATE)";

            string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        using (SqlDataReader r = command.ExecuteReader())
                        {

                            var ConsolidationLevel1DB = new SGQDBContext.ConsolidationLevel1();
                            var ConsolidationLevel2DB = new SGQDBContext.ConsolidationLevel2();


                            //Se encontrar, retorna o Id da Consolidação
                            while (r.Read())
                            {
                                int ParLevel1_Id = Convert.ToInt32(r[0]);
                                int ParLevel2_Id = Convert.ToInt32(r[1]);
                                int ParCompany_Id = Convert.ToInt32(r[2]);
                                DateTime CollectionDate = Convert.ToDateTime(r[3]);

                                var consolidationLevel1 = ConsolidationLevel1DB.getConsolidation(ParCompany_Id, ParLevel1_Id, CollectionDate);
                                if (consolidationLevel1 == null)
                                {
                                    consolidationLevel1 = InsertConsolidationLevel1(ParCompany_Id, ParLevel1_Id, CollectionDate);
                                    if (consolidationLevel1 == null)
                                    {
                                        throw new Exception();
                                    }
                                }

                                var consolidationLevel2 = ConsolidationLevel2DB.getByConsolidationLevel1(ParCompany_Id, consolidationLevel1.Id, ParLevel2_Id);

                                if (consolidationLevel2 == null)
                                {
                                    consolidationLevel2 = InsertConsolidationLevel2(consolidationLevel1.Id, ParLevel2_Id, ParCompany_Id, CollectionDate);
                                    if (consolidationLevel2 == null)
                                    {
                                        throw new Exception();
                                    }
                                }

                            }
                            //Se não encontrar, retorna zero
                            return null;
                        }
                    }
                }
            }
            //Em caso de Exception, grava um log no Banco de Dados e Retorna Zero
            catch (SqlException ex)
            {
                int insertLog = insertLogJson(sql, ex.Message, "N/A", "N/A", "GetLevel1Consolidation");
                return ex.Message;
            }
            catch (Exception ex)
            {
                int insertLog = insertLogJson(sql, ex.Message, "N/A", "N/A", "GetLevel1Consolidation");
                return ex.Message;
            }
        }

        [WebMethod]
        public string _CollectionLevel02_ConsolidationLevel2Update(string dataInicio, string dataFim)
        {
            string sql = "SELECT CL2.Id, CL1.ParLevel1_Id, CL2.ParLevel2_Id, CL2.UnitId, CAST(CL2.ConsolidationDate AS DATE) " +
                         "FROM ConsolidationLevel2 CL2 INNER JOIN " +
                         "ConsolidationLevel1 CL1 ON CL2.ConsolidationLevel1_Id = CL1.Id   " +
                         "WHERE CAST(CL2.ConsolidationDate AS DATE) BETWEEN '" + dataInicio + "' AND '" + dataFim + "' ";

            string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        using (SqlDataReader r = command.ExecuteReader())
                        {

                          //Se encontrar, retorna o Id da Consolidação
                            while (r.Read())
                            {
                                int ConsolidationLevel2_Id = Convert.ToInt32(r[0]);
                                int ParLevel1_Id = Convert.ToInt32(r[1]);
                                int ParLevel2_Id = Convert.ToInt32(r[2]);
                                int ParCompany_Id = Convert.ToInt32(r[3]);
                                DateTime ConsolidationDate = Convert.ToDateTime(r[4]);

                                _CollectionLevel02_Update(ConsolidationLevel2_Id, ParLevel1_Id, ParLevel2_Id, ParCompany_Id, ConsolidationDate);

                            }
                            //Se não encontrar, retorna zero
                            return null;
                        }
                    }
                }
            }
            //Em caso de Exception, grava um log no Banco de Dados e Retorna Zero
            catch (SqlException ex)
            {
                int insertLog = insertLogJson(sql, ex.Message, "N/A", "N/A", "GetLevel1Consolidation");
                return ex.Message;
            }
            catch (Exception ex)
            {
                int insertLog = insertLogJson(sql, ex.Message, "N/A", "N/A", "GetLevel1Consolidation");
                return ex.Message;
            }
        }

        [WebMethod]
        public string _ReConsolidation(int ParCompany_Id)
        {

            string sql = "SELECT Id, ParLevel2_Id, ConsolidationLevel1_Id FROM ConsolidationLevel2 WHERE UnitId='" + ParCompany_Id + "'";

            string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        using (SqlDataReader r = command.ExecuteReader())
                        {

                            //Se encontrar, retorna o Id da Consolidação
                            while (r.Read())
                            {

                                int ConsolidationLevel2_Id = Convert.ToInt32(r[0]);
                                int ParLevel2_Id = Convert.ToInt32(r[1]);
                                int ConsolidationLevel1_Id = Convert.ToInt32(r[2]);

                                var CollectionLevel2ConsolidationDB = new SGQDBContext.CollectionLevel2Consolidation();
                                var collectionLevel2Consolidation = CollectionLevel2ConsolidationDB.getConsolidation(ConsolidationLevel2_Id, ParLevel2_Id);

                                var updateConsolidationLevel2Id = updateConsolidationLevel2(ConsolidationLevel2_Id, "0", "0", "0", collectionLevel2Consolidation);

                                var ConsolidationLevel1XConsolidationLevel2DB = new ConsolidationLevel1XConsolidationLevel2();
                                var consolidationLevel1XConsolidationLevel2 = ConsolidationLevel1XConsolidationLevel2DB.getConsolidation(ConsolidationLevel1_Id);

                                var updateConsolidationLevel1Id = updateConsolidationLevel1(ConsolidationLevel1_Id, "0", "0", "0", consolidationLevel1XConsolidationLevel2);


                            }
                            //Se não encontrar, retorna zero
                            return null;
                        }
                    }
                }
            }
            //Em caso de Exception, grava um log no Banco de Dados e Retorna Zero
            catch (SqlException ex)
            {
                int insertLog = insertLogJson(sql, ex.Message, "N/A", "N/A", "GetLevel1Consolidation");
                return ex.Message;
            }
            catch (Exception ex)
            {
                int insertLog = insertLogJson(sql, ex.Message, "N/A", "N/A", "GetLevel1Consolidation");
                return ex.Message;
            }
        }

        [WebMethod]
        public string _ReConsolidationByLevel1(int ParCompany_Id, int ParLevel1_Id, DateTime ConsolidationDate)
        {

            string sql = "SELECT CL2.Id, CL2.ParLevel2_Id, CL2.ConsolidationLevel1_Id FROM ConsolidationLevel2 CL2 " +
                         " INNER JOIN ConsolidationLevel1 CL1 ON CL2.ConsolidationLevel1_Id=CL1.ID " +






               " WHERE CL2.UnitId='" + ParCompany_Id + "' AND CL1.ParLevel1_Id='" + ParLevel1_Id + "' AND CAST(CL1.ConsolidationDate AS DATE) = '" + ConsolidationDate.ToString("yyyyMMdd") + "'" ;

            string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        using (SqlDataReader r = command.ExecuteReader())
                        {

                            //Se encontrar, retorna o Id da Consolidação
                            while (r.Read())
                            {

                                int ConsolidationLevel2_Id = Convert.ToInt32(r[0]);
                                int ParLevel2_Id = Convert.ToInt32(r[1]);
                                int ConsolidationLevel1_Id = Convert.ToInt32(r[2]);

                                var CollectionLevel2ConsolidationDB = new SGQDBContext.CollectionLevel2Consolidation();
                                var collectionLevel2Consolidation = CollectionLevel2ConsolidationDB.getConsolidation(ConsolidationLevel2_Id, ParLevel2_Id);

                                var updateConsolidationLevel2Id = updateConsolidationLevel2(ConsolidationLevel2_Id, "0", "0", "0", collectionLevel2Consolidation);

                                var ConsolidationLevel1XConsolidationLevel2DB = new ConsolidationLevel1XConsolidationLevel2();
                                var consolidationLevel1XConsolidationLevel2 = ConsolidationLevel1XConsolidationLevel2DB.getConsolidation(ConsolidationLevel1_Id);

                                var updateConsolidationLevel1Id = updateConsolidationLevel1(ConsolidationLevel1_Id, "0", "0", "0", consolidationLevel1XConsolidationLevel2);


                            }
                            //Se não encontrar, retorna zero
                            return null;
                        }
                    }
                }
            }
            //Em caso de Exception, grava um log no Banco de Dados e Retorna Zero
            catch (SqlException ex)
            {
                int insertLog = insertLogJson(sql, ex.Message, "N/A", "N/A", "GetLevel1Consolidation");
                return ex.Message;
            }
            catch (Exception ex)
            {
                int insertLog = insertLogJson(sql, ex.Message, "N/A", "N/A", "GetLevel1Consolidation");
                return ex.Message;
            }
        }

        public void _CollectionLevel02_Update(int ConsolidationLevel2_Id, int ParLevel1_Id, int ParLevel2_Id, int ParCompany_Id, DateTime ConsolidationDate)
        {
            //CollectionDate
            string sql = "UPDATE CollectionLevel2  SET ConsolidationLevel2_Id='" + ConsolidationLevel2_Id + "' WHERE ParLevel1_Id='" + ParLevel1_Id + "' AND ParLevel2_Id='" + ParLevel2_Id + "' AND UnitId='" +  ParCompany_Id + "' AND CAST(CollectionDate AS DATE) = '" + ConsolidationDate.ToString("yyyyMMdd") + "'";
            string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        var i = Convert.ToInt32(command.ExecuteNonQuery());
                     
                    }
                }
            }
            catch (SqlException ex)
            {
                int insertLog = insertLogJson(sql, ex.Message, "N/A", "N/A", "_CollectionLevel02_Update");
                throw ex;
            }
            catch (Exception ex)
            {
                int insertLog = insertLogJson(sql, ex.Message, "N/A", "N/A", "_CollectionLevel02_Update");
                throw ex;
            }
        }
    }

}
