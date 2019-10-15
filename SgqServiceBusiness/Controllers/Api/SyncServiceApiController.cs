using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dominio;
//using SgqSystem.Handlres;
using System.Net;
using System.Data.SqlClient;
using DTO;
using ADOFactory;
using System.Threading;
using System.Globalization;
using DTO.Helpers;
using SGQDBContext;
using SgqServiceBusiness.Helpers;
using System.Net.Mail;
using System.Text;
using System.Collections;
using System.Data;
using ServiceModel;
using SgqServiceBusiness.Api.App;
using SgqServiceBusiness.Services;
using SgqSystem.Helpers;

namespace SgqServiceBusiness.Api
{
    public class SyncServiceApiController
    {

        string conexao;
        string conexaoSGQ_GlobalADO;

        protected SqlConnection db;
        protected SqlConnection SGQ_GlobalADO;

        Dominio.SgqDbDevEntities dbEf;

        public SyncServiceApiController(string conexao,
        string conexaoSGQ_GlobalADO)
        {

            this.conexao = conexao;
            this.conexaoSGQ_GlobalADO = conexaoSGQ_GlobalADO;

            db = new SqlConnection(conexao);
            SGQ_GlobalADO = new SqlConnection(conexaoSGQ_GlobalADO);

            dbEf = new Dominio.SgqDbDevEntities();

        }

        #region Funções

        public static string quebraProcesso = "98789";

        /// <summary>
        /// Converter a Data do Tablet
        /// </summary>
        /// <param name="collectionDate">Data Formatada do Tablet</param>
        /// <returns></returns>
        /// 
        /**
         * TODOS QUE CHAMEREM ESTE MÉTODO DEVEM ENVIAR A DATA MM/dd/yyyy
         * OU YYYY-MM-DD (2017-05-03)
         * COMENTÁRIO: GABRIEL 2017-04-24
         * 
         */
        private DateTime DateCollectConvert(string collectionDate)
        {
            //acerto para data yyyy-mm-dd
            if (collectionDate.Contains("-"))
            {
                collectionDate = collectionDate.Substring(5, 2) + "/" + collectionDate.Substring(8, 2) + "/" + collectionDate.Substring(0, 4) + " 00:00:00";
            }
            else
            //fim acerto data yyyy-mm-dd
            if (!collectionDate.Contains("/"))
            {
                collectionDate = collectionDate.Substring(0, 2) + "/" + collectionDate.Substring(2, 2) + "/" + collectionDate.Substring(4, 4) + " 00:00:00";
            }
            string[] data = collectionDate.Split('/');

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
        protected string DefaultValueReturn(string valor, string valorDefault)
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
        protected string BoolConverter(string valor)
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

        protected string BoolCompletedConverter(string valor)
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

        protected int insertLogJson(string result, string log, string deviceId, string AppVersion, string callback)
        {

            string sql = $@"INSERT INTO LogJson ([result],[log],[AddDate],[Device_Id],[AppVersion], [callback]) 
                         VALUES (@Result, @Log, GETDATE(), @DeviceId, @AppVersion, @Callback)";

            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {

                        command.CommandType = CommandType.Text;
                        command.Parameters.Add(new SqlParameter("@Result", result.Replace("'", "")));
                        command.Parameters.Add(new SqlParameter("@Log", log.Replace("'", "")));
                        command.Parameters.Add(new SqlParameter("@DeviceId", deviceId));
                        command.Parameters.Add(new SqlParameter("@AppVersion", AppVersion == null ? "" : AppVersion));
                        command.Parameters.Add(new SqlParameter("@Callback", callback));

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
        public string InsertJson(InsertJsonClass insertJsonClass)
        {
            try
            {
                string ObjResultJSon = insertJsonClass.ObjResultJSon.Replace("NaN", "0");
                string deviceId = insertJsonClass.deviceId;
                string deviceMac = insertJsonClass.deviceMac;
                bool autoSend = insertJsonClass.autoSend;


                SqlConnection.ClearAllPools();

                //ObjResultJSon = "<level02>3987891;03/30/2018 08:41:032:033;1;03/30/2018 08:41:032:072;5;1;1;1;0;false;03302018;1;1;<header>17,1,3,0,0,0,0,0,0</header>;false;false;;undefined;undefined;false; 2.0.46;JBS ;<level03>16,03/30/2018 08:41:032:075,,true,1,null,null,undefined,1.00000,,0.0000000000,0.0000000000,false,0,0,1,0</level03><level03>27,03/30/2018 08:41:032:076,,true,1,null,null,undefined,1.00000,,0.0000000000,0.0000000000,false,0,0,1,0</level03><level03>29,03/30/2018 08:41:032:077,,true,1,null,null,undefined,1.00000,,0.0000000000,0.0000000000,false,0,0,1,0</level03>;;undefined;undefined;0;undefined;undefined;undefined;undefined;undefined;undefined;0;0;3;0;0;0;3;0;1;0;0;0;0;undefined;0;0</level02>";

                ObjResultJSon = ObjResultJSon.Replace("%2C", "").Replace("NaN", "0");

                var objObjResultJSonPuro = ObjResultJSon;

                string versaoApp = null;

                try
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

                    using (SqlConnection connection = new SqlConnection(conexao))
                    {
                        connection.Open();

                        for (int i = 0; i < arrayObj.Length; i++)
                        {
                            //Estrai o resultado
                            string[] result = arrayObj[i].Split(';');

                            //4 98789 1190 //98789 é a chave que separa processo de produto
                            string parCluster_Id_parLevel1_id = result[0].Replace(quebraProcesso, "|"); //"Cluster|Indicador"
                            string parCluster_Id = parCluster_Id_parLevel1_id.Split('|').Length > 1 ? parCluster_Id_parLevel1_id.Split('|')[0] : "0";
                            string parLevel1_Id = parCluster_Id_parLevel1_id.Split('|').Length > 1 ? parCluster_Id_parLevel1_id.Split('|')[1] : parCluster_Id_parLevel1_id.Split('|')[0];

                            string parCluster_Id_parLevel2_id = result[2].Replace(quebraProcesso, "|");
                            string parLevel2_Id = parCluster_Id_parLevel2_id.Split('|').Length > 1 ? parCluster_Id_parLevel2_id.Split('|')[1] : parCluster_Id_parLevel2_id.Split('|')[0];

                            string parCluster_Id_parLevel2_id_UltimoAlerta = result[45].Replace(quebraProcesso, "|");
                            string parLevel2_Id_UltimoAlerta = parCluster_Id_parLevel2_id_UltimoAlerta.Split('|').Length > 1 ? parCluster_Id_parLevel2_id_UltimoAlerta.Split('|')[1] : parCluster_Id_parLevel2_id_UltimoAlerta.Split('|')[0];


                            result[0] = parLevel1_Id;

                            result[2] = parLevel2_Id;

                            result[45] = parLevel2_Id_UltimoAlerta;

                            //List<string> r1 = result.ToList<string>();

                            //r1.Add(parCluster_Id);

                            int insertLog = 0;

                            if (parCluster_Id == "0")
                                insertLog = insertLogJson(objObjResultJSonPuro, "Gravou cluster 0", deviceId, versaoApp, "Cluster 0");

                            //result = r1.ToArray();


                            string[] resultCopy = result;
                            while (!resultCopy[22].Contains("<level03>") && resultCopy.Count() > 23)
                            {
                                resultCopy = RemoveFrom(resultCopy, 22);
                            }

                            if (resultCopy.Count() > 22)
                            {
                                if (resultCopy[22].Contains("<level03>"))
                                {
                                    result = resultCopy;
                                }
                            }

                            /**
                             * autor: Gabriel Nunes
                             * date: 2017-06-05
                             * title: indicador pai
                             */

                            //verifico se este indicador é pai de algum outro. Trago uma lista com os leveis 3 do indicador filho, se for o caso

                            var ParLevel1Origin_Id = DefaultValueReturn(result[0], "0");

                            //string indicadorPai = "    SELECT distinct(cast(p32.ParLevel3_Id as varchar)) retorno FROM ParLevel1 p1  WITH (NOLOCK)" +
                            //                      "\n  inner join ParLevel3Level2Level1 p321  WITH (NOLOCK)" +
                            //                      "\n  on p321.ParLevel1_Id = p1.id " +
                            //                      "\n  inner join ParLevel3Level2 p32  WITH (NOLOCK)" +
                            //                      "\n  on p32.id = p321.ParLevel3Level2_Id " +
                            //                      "\n  WHERE ParLevel1Origin_Id = " + ParLevel1Origin_Id +
                            //                      "\n  and p1.isActive = 1 " +
                            //                      "\n  and p321.Active = 1 " +
                            //                      "\n  and p32.IsActive = 1" +
                            //                      "\n  and p32.Parlevel2_Id = " + parLevel2_Id;

                            string indicadorFilho_ = $@"SELECT distinct(cast(p32.ParLevel3_Id as varchar)) retorno FROM ParLevel1 p1  WITH (NOLOCK)
                                              inner join ParLevel3Level2Level1 p321  WITH (NOLOCK)
                                              on p321.ParLevel1_Id = p1.id 
                                              inner join ParLevel3Level2 p32  WITH (NOLOCK)
                                              on p32.id = p321.ParLevel3Level2_Id 
                                              WHERE ParLevel1Origin_Id = @ParLevel1Origin_Id
                                              and p1.isActive = 1 
                                              and p321.Active = 1 
                                              and p32.IsActive = 1
                                              and p32.Parlevel2_Id = @ParLevel2_Id";

                            List<ResultadoUmaColuna> list;

                            using (Factory factory = new Factory("DefaultConnection"))
                            {
                                using (SqlCommand cmd = new SqlCommand(indicadorFilho_, factory.connection))
                                {
                                    cmd.CommandType = CommandType.Text;
                                    cmd.Parameters.Add(new SqlParameter("@ParLevel1Origin_Id", ParLevel1Origin_Id));
                                    cmd.Parameters.Add(new SqlParameter("@ParLevel2_Id", parLevel2_Id));

                                    list = factory.SearchQuery<ResultadoUmaColuna>(cmd).ToList();
                                }
                            }

                            string level3split = result[22].Replace("</level03><level03>", "@").Replace("<level03>", "").Replace("</level03>", ""); //tiro as tags de <level3></level3>, deixando o simbolo @ para separar os elementos.
                            string[] leveis3 = level3split.Split('@'); //faço um array contendo cada elemento level3 vindo do sistema

                            string indicadorPai = @"SELECT distinct(cast(p32.ParLevel3_Id as varchar)) retorno FROM ParLevel1 p1  WITH (NOLOCK)
                                              inner join ParLevel3Level2Level1 p321  WITH (NOLOCK)
                                              on p321.ParLevel1_Id = p1.id 
                                              inner join ParLevel3Level2 p32  WITH (NOLOCK)
                                              on p32.id = p321.ParLevel3Level2_Id 
                                              WHERE p1.id = @ParLevel1Origin_Id
                                              and p1.isActive = 1 
                                              and p321.Active = 1 
                                              and p32.IsActive = 1
                                              and p32.Parlevel2_Id = @ParLevel2_Id";

                            List<ResultadoUmaColuna> listPai;

                            using (Factory factory = new Factory("DefaultConnection"))
                            {
                                using (SqlCommand cmd = new SqlCommand(indicadorPai, factory.connection))
                                {
                                    cmd.CommandType = CommandType.Text;
                                    cmd.Parameters.Add(new SqlParameter("@ParLevel1Origin_Id", ParLevel1Origin_Id));
                                    cmd.Parameters.Add(new SqlParameter("@ParLevel2_Id", parLevel2_Id));

                                    listPai = factory.SearchQuery<ResultadoUmaColuna>(cmd).ToList();
                                }
                            }


                            //string[][] matrizLevel3 = new string[leveis3.Length][];

                            string retorno = "";

                            string retornoFilho = "";

                            bool apagarLevel3 = true;

                            //tiro todos os level3 que não são do indicador
                            for (int j = 0; j < leveis3.Length; j++) //Percorro cada elemento do array
                            {
                                string[] esteLevel3 = leveis3[j].Split(',');

                                for (var k = 0; k < list.Count(); k++)
                                {
                                    if (list[k].retorno.ToString() == esteLevel3[0])
                                    {
                                        retornoFilho += "<level03>";
                                        retornoFilho += leveis3[j];
                                        retornoFilho += "</level03>";

                                        for (var l = 0; l < listPai.Count(); l++)
                                        {
                                            if (listPai[l].retorno.ToString() == esteLevel3[0])
                                            {
                                                apagarLevel3 = false;
                                            }
                                        }

                                        if (apagarLevel3)
                                            leveis3[j] = "";

                                        apagarLevel3 = true;
                                    }
                                }
                            }


                            //coloco as tag de level3 e tiro quando não houver elementos
                            for (int j = 0; j < leveis3.Length; j++) //Percorro cada elemento do array
                            {
                                if (leveis3[j] != "")
                                {
                                    retorno += "<level03>";
                                    retorno += leveis3[j];
                                    retorno += "</level03>";
                                }

                            }

                            result[22] = retorno;

                            //-----------------------------

                            //Id do Level01
                            string level01Id = DefaultValueReturn(result[0], "0");

                            if (level01Id == "0")
                            {
                                //string p1Undefined = "    SELECT distinct(cast(p321.ParLevel1_Id as varchar)) retorno FROM ParLevel1 p1  WITH (NOLOCK)" +
                                //                     "\n  inner join ParLevel3Level2Level1 p321  WITH (NOLOCK)" +
                                //                     "\n  on p321.ParLevel1_Id = p1.id " +
                                //                     "\n  inner join ParLevel3Level2 p32  WITH (NOLOCK)" +
                                //                     "\n  on p32.id = p321.ParLevel3Level2_Id " +
                                //                     "\n  WHERE p32.ParLevel2_Id = " + result[2] +
                                //                     "\n  and p1.isActive = 1 " +
                                //                     "\n  and p321.Active = 1 " +
                                //                     "\n  and p32.IsActive = 1";

                                string p1Undefined = $@"SELECT distinct(cast(p321.ParLevel1_Id as varchar)) retorno FROM ParLevel1 p1  WITH (NOLOCK)
                                                inner join ParLevel3Level2Level1 p321  WITH (NOLOCK)
                                                on p321.ParLevel1_Id = p1.id 
                                                inner join ParLevel3Level2 p32  WITH (NOLOCK)
                                                on p32.id = p321.ParLevel3Level2_Id 
                                                WHERE p32.ParLevel2_Id = @Result
                                                and p1.isActive = 1 
                                                and p321.Active = 1 
                                                and p32.IsActive = 1";

                                using (Factory factory = new Factory("DefaultConnection"))
                                {
                                    using (SqlCommand cmd = new SqlCommand(p1Undefined, factory.connection))
                                    {
                                        cmd.CommandType = CommandType.Text;
                                        cmd.Parameters.Add(new SqlParameter("@Result", result[2]));

                                        level01Id = factory.SearchQuery<ResultadoUmaColuna>(cmd).FirstOrDefault().retorno;

                                    }
                                }
                            }

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

                            //DEBUGAR PARA O AUDITOR
                            //auditorId = "1";

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
                            string startphaseevaluation = "0";
                            string endphaseevaluation = "0";
                            string reprocesso = null;
                            string cluster = parCluster_Id;
                            string ParReason_Id = null;
                            string ParReasonType_Id = null;
                            string parDepartment_Id = null;

                            if (result.Length > 47)
                            {
                                startphaseevaluation = result[47];
                            }
                            if (result.Length > 48)
                            {
                                endphaseevaluation = result[48];
                            }

                            if (result.Length > 49)
                            {
                                reprocesso = result[49];
                            }

                            if (result.Length > 50)
                            {
                                ParReason_Id = result[50];
                                ParReasonType_Id = result[51];
                            }

                            if (result.Length > 51)
                            {
                                parDepartment_Id = result[52];
                            }

                            //Gera o Cabeçalho do Level02
                            string level02HeaderJSon = result[13]; //[0]
                            level02HeaderJSon += ";" + phase; //[1]
                            level02HeaderJSon += ";" + startphasedate; //[2]
                            level02HeaderJSon += ";" + consecutivefailurelevel; //[3]
                            level02HeaderJSon += ";" + consecutivefailuretotal; //[4]
                            level02HeaderJSon += ";" + notavaliable; //[5]
                            level02HeaderJSon += ";" + completed; //[6]
                            level02HeaderJSon += ";" + havePhases; //[7]
                            level02HeaderJSon += ";" + CollectionLevel02Id; //[8]
                            level02HeaderJSon += ";" + correctiveActionCompleted; //[9]
                            level02HeaderJSon += ";" + completeReaudit; //[10]
                            level02HeaderJSon += ";" + AlertLevel; //[11]
                            level02HeaderJSon += ";" + sequential; //[12]
                            level02HeaderJSon += ";" + side; //[13]
                            level02HeaderJSon += ";" + weievaluation; //[14]
                            level02HeaderJSon += ";" + weidefects; //[15]
                            level02HeaderJSon += ";" + defects; //[16]
                            level02HeaderJSon += ";" + totallevel3withdefects; //[17]
                            level02HeaderJSon += ";" + totalLevel2Evaluation; //[18]
                            level02HeaderJSon += ";" + avaliacaoultimoalerta; //[19]
                            level02HeaderJSon += ";" + evaluatedresult; //[20]
                            level02HeaderJSon += ";" + defectsresult; //[21]
                            level02HeaderJSon += ";" + sequential; //[22]
                            level02HeaderJSon += ";" + side; //[23]
                            level02HeaderJSon += ";" + isemptylevel3; //[24]
                            level02HeaderJSon += ";" + hassampletotal; //[25]
                            level02HeaderJSon += ";" + hashKey; //[26]
                            level02HeaderJSon += ";" + monitoramentoultimoalerta; //[27]
                            level02HeaderJSon += ";" + startphaseevaluation; //[28]
                            level02HeaderJSon += ";" + endphaseevaluation; //[29]
                            level02HeaderJSon += ";" + reprocesso; //[30]
                            level02HeaderJSon += ";" + cluster; //[31]
                            level02HeaderJSon += ";" + ParReason_Id; //[32]
                            level02HeaderJSon += ";" + ParReasonType_Id; //[33]
                            level02HeaderJSon += ";" + parDepartment_Id; //[34]

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
                            string reauditlevel = result[46];
                            //Convert Reauditoria Pendente para valor correto
                            reauditlevel = DefaultValueReturn(reauditlevel, "0");

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
                                "([Unit_Id]," +
                                "[Shift]," +
                                "[Period]," +
                                "[level01_Id]," +
                                "[Level01CollectionDate]," +
                                "[level02_Id]," +
                                "[Evaluate]," +
                                "[Sample]," +
                                "[AuditorId]," +
                                "[Level02CollectionDate]," +
                                "[Level02HeaderJson]," +
                                "[Level03ResultJSon]," +
                                "[CorrectiveActionJson]," +
                                "[Reaudit]," +
                                "[ReauditNumber]," +
                                "[haveReaudit]," +
                                "[ReauditLevel]," +
                                "[haveCorrectiveAction]," +
                                "[Device_Id]," +
                                "[AppVersion]," +
                                "[Ambient]," +
                                "[IsProcessed]," +
                                "[Device_Mac]," +
                                "[AddDate]," +
                                "[AlterDate]," +
                                "[Key]," +
                                "[TTP]) " +
                                "VALUES " +
                                "(@UnidadeId," +
                                "@Shift," +
                                "@Period," +
                                "@Level1Id," +
                                "CAST(@Level1DataCollect AS DateTime)," +
                                "@Level2Id," +
                                "@Evaluate," +
                                "@Sample, " +
                                "@AuditorId," +
                                "CAST(@Level2DataCollect AS DateTime)," +
                                "@Level2HeaderJson," +
                                "@Level3ResultJson, " +
                                "@CorrectiveActionJson, " +
                                "@Reaudit, " +
                                "@ReauditNumber, " +
                                "@HaveReaudit, " +
                                "@ReauditLevel," +
                                "@HaveCorrectiveAction," +
                                "@DeviceId," +
                                "@VersaoApp," +
                                "@Ambiente," +
                                "0," +
                                "@DeviceMac," +
                                "GETDATE()," +
                                "NULL," +
                                "@Key" +
                                ",NULL) ";

                            sql += "SELECT @@IDENTITY AS 'Identity'";

                            var iSql = 0;

                            using (SqlCommand cmd = new SqlCommand(sql, connection))
                            {
                                cmd.CommandType = CommandType.Text;
                                cmd.Parameters.Add(new SqlParameter("@UnidadeId", unidadeId));
                                cmd.Parameters.Add(new SqlParameter("@Shift", shift));
                                cmd.Parameters.Add(new SqlParameter("@Period", period));
                                cmd.Parameters.Add(new SqlParameter("@Level1Id", level01Id));
                                cmd.Parameters.Add(new SqlParameter("@Level1DataCollect", level01DataCollect));
                                cmd.Parameters.Add(new SqlParameter("@Level2Id", level02Id));
                                cmd.Parameters.Add(new SqlParameter("@Evaluate", evaluate));
                                cmd.Parameters.Add(new SqlParameter("@Sample", sample));
                                cmd.Parameters.Add(new SqlParameter("@AuditorId", auditorId));
                                cmd.Parameters.Add(new SqlParameter("@Level2DataCollect", level02DataCollect));
                                cmd.Parameters.Add(new SqlParameter("@Level2HeaderJson", level02HeaderJSon));
                                cmd.Parameters.Add(new SqlParameter("@Level3ResultJson", level03ResultJson));
                                cmd.Parameters.Add(new SqlParameter("@CorrectiveActionJson", correctiveActionJson));
                                cmd.Parameters.Add(new SqlParameter("@Reaudit", reaudit));
                                cmd.Parameters.Add(new SqlParameter("@ReauditNumber", reauditNumber));
                                cmd.Parameters.Add(new SqlParameter("@HaveReaudit", haveReaudit));
                                cmd.Parameters.Add(new SqlParameter("@ReauditLevel", reauditlevel));
                                cmd.Parameters.Add(new SqlParameter("@HaveCorrectiveAction", haveCorrectiveAction));
                                cmd.Parameters.Add(new SqlParameter("@DeviceId", deviceId));
                                cmd.Parameters.Add(new SqlParameter("@VersaoApp", versaoApp));
                                cmd.Parameters.Add(new SqlParameter("@Ambiente", ambiente));
                                cmd.Parameters.Add(new SqlParameter("@DeviceMac", deviceMac));
                                cmd.Parameters.Add(new SqlParameter("@Key", key));

                                iSql = Convert.ToInt32(cmd.ExecuteScalar());
                            }

                            if (iSql > 0)
                            {
                                //if (autoSend == true)
                                //{
                                ProcessJson(null, iSql, false);

                                //}
                            }

                            else
                            {
                                //Se não ocorre sem problemas, retorna um erro
                                throw new Exception("erro json");

                            }

                            if (retornoFilho != "")
                            {


                                List<ResultadoUmaColuna> list2;

                                var indicadorFilho_id = "";
                                var monitoramentoFilho_id = "";

                                using (Factory factory = new Factory("DefaultConnection"))
                                {

                                    string sqlIndicadorFilho = $@" SELECT distinct(cast(p1.Id as varchar)) retorno FROM ParLevel1 p1  WITH (NOLOCK) 
                                                    inner join ParLevel3Level2Level1 p321  WITH (NOLOCK) 
                                                    on p321.ParLevel1_Id = p1.id 
                                                    inner join ParLevel3Level2 p32  WITH (NOLOCK) 
                                                    on p32.id = p321.ParLevel3Level2_Id 
                                                    WHERE ParLevel1Origin_Id = @Result
                                                    and p1.isActive = 1 
                                                    and p321.Active = 1 
                                                    and p32.IsActive = 1";

                                    using (SqlCommand cmd = new SqlCommand(sqlIndicadorFilho, factory.connection))
                                    {
                                        cmd.CommandType = CommandType.Text;
                                        cmd.Parameters.Add(new SqlParameter("@Result", result[0]));
                                        list2 = factory.SearchQuery<ResultadoUmaColuna>(cmd).ToList();
                                    }

                                    for (var l = 0; l < list2.Count(); l++)
                                    {
                                        indicadorFilho_id = list2[l].retorno.ToString();
                                    }

                                    string sqlMonitoramentoFilho = $@"select top 1 cast(p32.ParLevel2_Id as varchar) retorno
                                            from parlevel3level2level1 p321 WITH (NOLOCK)
                                            inner join parlevel3level2 p32 WITH (NOLOCK)
                                            on p321.parlevel3level2_id = p32.id 
                                            where p321.Active = 1 and p321.parlevel1_id = @IndicadorFilho_Id";

                                    using (SqlCommand cmd = new SqlCommand(sqlMonitoramentoFilho, factory.connection))
                                    {
                                        cmd.CommandType = CommandType.Text;
                                        cmd.Parameters.Add(new SqlParameter("@IndicadorFilho_Id", indicadorFilho_id));
                                        list2 = factory.SearchQuery<ResultadoUmaColuna>(cmd).ToList();
                                    }

                                    for (var l = 0; l < list2.Count(); l++)
                                    {
                                        monitoramentoFilho_id = list2[l].retorno.ToString();
                                    }

                                }

                                string sqlInsertCollectionJsonIndicadorFilho = $@"INSERT INTO [dbo].[CollectionJson] 
                               ([Unit_Id],
                               [Shift],
                               [Period],
                               [level01_Id],
                               [Level01CollectionDate],
                               [level02_Id],[Evaluate],
                               [Sample],
                               [AuditorId],
                               [Level02CollectionDate],
                               [Level02HeaderJson],
                               [Level03ResultJSon],
                               [CorrectiveActionJson],
                               [Reaudit],
                               [ReauditNumber],
                               [haveReaudit],
                               [ReauditLevel],
                               [haveCorrectiveAction],
                               [Device_Id],
                               [AppVersion],
                               [Ambient],
                               [IsProcessed],
                               [Device_Mac],
                               [AddDate],
                               [AlterDate],
                               [Key],
                               [TTP]) 
                               VALUES
                               (@UnidadeId,
                               @Shift,
                               @Period,
                               @IndicadorFilhoId,
                               CAST(@Level1DataCollect AS DateTime),
                               @Level2Id,
                               @Evaluate,
                               @Sample, 
                               @AuditorId,
                               CAST(@Level2DataColletion AS DateTime),
                               @Level2HeaderJson,
                               @RetornoFilho,
                               @CorrectiveActionJson,
                               @Reaudit,
                               @ReauditNumber, 
                               @HaveReaudit, 
                               @Reauditlevel,
                               @HaveCorrectiveAction,
                               @DeviceId,
                               @VersaoApp,
                               @Ambiente,
                               0,
                               @DeviceMac,
                               GETDATE(),
                               NULL,
                               @Key,
                               NULL); ";

                                sqlInsertCollectionJsonIndicadorFilho += "SELECT @@IDENTITY AS 'Identity'";

                                var iSql2 = 0;

                                using (SqlCommand cmd = new SqlCommand(sqlInsertCollectionJsonIndicadorFilho, connection))
                                {
                                    cmd.CommandType = CommandType.Text;
                                    cmd.Parameters.Add(new SqlParameter("@UnidadeId", unidadeId));
                                    cmd.Parameters.Add(new SqlParameter("@Shift", shift));
                                    cmd.Parameters.Add(new SqlParameter("@Period", period));
                                    cmd.Parameters.Add(new SqlParameter("@IndicadorFilhoId", indicadorFilho_id));
                                    cmd.Parameters.Add(new SqlParameter("@Level1DataCollect", level01DataCollect));
                                    cmd.Parameters.Add(new SqlParameter("@Level2Id", level02Id));
                                    cmd.Parameters.Add(new SqlParameter("@Evaluate", evaluate));
                                    cmd.Parameters.Add(new SqlParameter("@Sample", sample));
                                    cmd.Parameters.Add(new SqlParameter("@AuditorId", auditorId));
                                    cmd.Parameters.Add(new SqlParameter("@Level2DataColletion", level02DataCollect));
                                    cmd.Parameters.Add(new SqlParameter("@Level2HeaderJson", level02HeaderJSon));
                                    cmd.Parameters.Add(new SqlParameter("@RetornoFilho", retornoFilho));
                                    cmd.Parameters.Add(new SqlParameter("@CorrectiveActionJson", correctiveActionJson));
                                    cmd.Parameters.Add(new SqlParameter("@Reaudit", reaudit));
                                    cmd.Parameters.Add(new SqlParameter("@ReauditNumber", reauditNumber));
                                    cmd.Parameters.Add(new SqlParameter("@HaveReaudit", haveReaudit));
                                    cmd.Parameters.Add(new SqlParameter("@Reauditlevel", reauditlevel));
                                    cmd.Parameters.Add(new SqlParameter("@HaveCorrectiveAction", haveCorrectiveAction));
                                    cmd.Parameters.Add(new SqlParameter("@DeviceId", deviceId));
                                    cmd.Parameters.Add(new SqlParameter("@VersaoApp", versaoApp));
                                    cmd.Parameters.Add(new SqlParameter("@Ambiente", ambiente));
                                    cmd.Parameters.Add(new SqlParameter("@DeviceMac", deviceMac));
                                    cmd.Parameters.Add(new SqlParameter("@Key", key));

                                    iSql2 = Convert.ToInt32(cmd.ExecuteScalar());
                                }


                                if (iSql2 > 0)
                                {
                                    //if (autoSend == true)
                                    //{
                                    ProcessJson(null, iSql2, true);

                                    //}
                                }

                                else
                                {
                                    //Se não ocorre sem problemas, retorna um erro
                                    throw new Exception("erro json");

                                }

                            }

                        }

                        if (connection.State == System.Data.ConnectionState.Open) connection.Close();
                    }
                    return null;
                }
                catch (SqlException ex)
                {
                    int insertLog = insertLogJson(objObjResultJSonPuro, ex.Message, deviceId, versaoApp, "InsertJson");
                    return ex.ToClient();
                    //return "error sql insert";
                }
                catch (Exception ex)
                {
                    int insertLog = insertLogJson(objObjResultJSonPuro, ex.Message, deviceId, versaoApp, "InsertJson");
                    return ex.ToClient();
                }
            }
            catch (Exception ex)
            {
                return ex.ToClient();
            }
        }

        private T[] RemoveFrom<T>(T[] source, int index)
        {
            T[] dest = new T[source.Length - 1];
            if (index > 0)
                Array.Copy(source, 0, dest, 0, index);

            if (index < source.Length - 1)
                Array.Copy(source, index + 1, dest, index, source.Length - index - 1);

            return dest;
        }

        /// <summary>
        /// Método que grava o Json nas tabelas de resultados
        /// </summary>
        /// <param name="device">Id do dispositivo</param>
        /// <returns></returns>
        /// Para chamar uma consolidação geral digite [web]
        public string ProcessJson(string device, int id, bool filho)
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
                    //query = "[Device_Id] = '" + device + "' AND";
                    query = "[Device_Id] = @Device AND";
                }

                if (id > 0)
                {
                    //query = "[Id] = '" + id + "' AND";
                    query = "[Id] = @Id AND";
                }

                string sql = $@"SELECT [level01_Id], 
                    [Level01CollectionDate], 
                    [level02_Id], 
                    [Level02CollectionDate], 
                    [Unit_Id],
                    [Period], 
                    [Shift], 
                    [AppVersion], 
                    [Ambient], 
                    [Device_Id], 
                    [Device_Mac] , 
                    [Key], 
                    [Level03ResultJSon], 
                    [Id], 
                    [Level02HeaderJson], 
                    [Evaluate],
                    [Sample],
                    [AuditorId], 
                    [Reaudit],
                    [CorrectiveActionJson],
                    [haveReaudit],
                    [ReauditLevel],
                    [haveCorrectiveAction],
                    [ReauditNumber]  FROM CollectionJson (nolock)
                    WHERE { query } [IsProcessed] = 0";




                var CollectionJsonDB = new SGQDBContext.CollectionJson(db);
                var ConsolidationLevel1DB = new SGQDBContext.ConsolidationLevel1(db);
                var ConsolidationLevel2DB = new SGQDBContext.ConsolidationLevel2(db);

                var collectionJson = new List<SGQDBContext.CollectionJson>();

                using (Factory factory = new Factory("DefaultConnection"))
                {

                    using (SqlCommand cmd = new SqlCommand(sql, new SqlConnection(conexao)))
                    {
                        cmd.CommandType = CommandType.Text;

                        if (device == "web")
                        {

                        }
                        else if (id > 0)
                        {
                            cmd.Parameters.Add(new SqlParameter("@Id", id));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@Device", device));
                        }

                        cmd.Connection.Open();

                        collectionJson = factory.SearchQuery<SGQDBContext.CollectionJson>(cmd).ToList();
                    }
                }

                var collectionLevel2XCollectionJsonList = new List<KeyValuePair<int, int>>();


                //connection = new SqlConnection(conexao);
                //connection.Open();

                foreach (var c in collectionJson)
                {

                    /*
                     * MOCK INDICADOR FILHO GABRIEL
                     * 30/03/2017
                     */
                    //if (c.level01_Id == 22)
                    //{
                    //    ParLevel1_Id = 1042;
                    //}
                    //else
                    //{
                    //    ParLevel1_Id = null;
                    //}

                    var l3Temp = c.Level03ResultJSon.Replace("</level03><level03>", "@").Replace("<level03>", "").Replace("</level03>", "");

                    if (l3Temp.Count() < 16)
                    {
                        c.Level03ResultJSon = null;
                    }


                    int ConsolidationLevel1_Id = 0;
                    int ConsolidationLevel2_Id = 0;
                    string AlertLevel = "0";
                    string avaliacaoultimoalerta = "0";
                    string monitoramentoultimoalerta = "0";

                    string ParReason_Id = null;
                    string ParReasonType_Id = null;

                    //using (var transacao = new TransactionScope())
                    //{
                    //Cabecalho                   
                    string[] arrayHeader = c.Level02HeaderJson.Split(';');

                    string headersContadores = arrayHeader[0];
                    string Phase = arrayHeader[1];

                    string Reaudit = BoolConverter(c.Reaudit.ToString());

                    string StartPhase = arrayHeader[2];
                    if (VerificaStringNulaUndefinedNaN(StartPhase))
                    {
                        StartPhase = "'0001-01-01 00:00:00'";
                    }
                    else
                    {
                        try
                        {
                            DateTime dataPhase = DateCollectConvert(StartPhase);
                            StartPhase = "CAST(N'" + dataPhase.ToString("yyyy-MM-dd 00:00:00") + "' AS DateTime)";
                        }
                        catch (Exception)
                        {
                            StartPhase = "CAST(N'" + DateTime.Now.ToString("yyyy-MM-dd 00:00:00") + "' AS DateTime)";
                        }

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
                    AlertLevel = AlertLevel == "NaN" ? "0" : AlertLevel; //pog
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

                    string cluster = DefaultValueReturn(arrayHeader[31], null);
                    string parDepartment_Id = arrayHeader.Length > 34 ? DefaultValueReturn(arrayHeader[34], null) : null;

                    string haveReaudit = BoolConverter(c.haveReaudit.ToString());

                    int reauditLevel = c.ReauditLevel;

                    string haveCorrectiveAction = BoolConverter(c.haveCorrectiveAction.ToString());

                    string reauditNumber = DefaultValueReturn(c.ReauditNumber.ToString(), "0");

                    var consolidationLevel1 = ConsolidationLevel1DB.getConsolidation(c.Unit_Id, c.level01_Id, c.Level01CollectionDate, c.Shift, c.Period, cluster, Convert.ToInt32(parDepartment_Id));

                    if (c.Reaudit)
                        consolidationLevel1 = ConsolidationLevel1DB.getConsolidation(c.Unit_Id, c.level01_Id, c.Level01CollectionDate, c.Shift, c.Period, cluster, Convert.ToInt32(parDepartment_Id));
                    if (consolidationLevel1 == null)
                    {
                        consolidationLevel1 = InsertConsolidationLevel1(c.Unit_Id, c.level01_Id, c.Level01CollectionDate, c.Shift, c.Period, cluster, Convert.ToInt32(parDepartment_Id));
                        if (consolidationLevel1 == null)
                        {
                            throw new Exception();
                        }
                    }

                    var consolidationLevel2 = ConsolidationLevel2DB.getByConsolidationLevel1(c.Unit_Id, consolidationLevel1.Id, c.level02_Id, cluster, Convert.ToInt32(parDepartment_Id));

                    if (c.Reaudit)
                        consolidationLevel2 = ConsolidationLevel2DB.getByConsolidationLevel1(c.Unit_Id, consolidationLevel1.Id, c.level02_Id, 1, reauditNumber, cluster, Convert.ToInt32(parDepartment_Id));

                    if (consolidationLevel2 == null)
                    {
                        consolidationLevel2 = InsertConsolidationLevel2(consolidationLevel1.Id, c.level02_Id, c.Unit_Id, c.Level02CollectionDate, c.Reaudit, c.ReauditNumber, cluster, Convert.ToInt32(parDepartment_Id));
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

                    string startphaseevaluation = DefaultValueReturn(arrayHeader[28], "0");
                    string endphaseevaluation = "0";
                    if (arrayHeader.Length > 29)
                    {
                        endphaseevaluation = DefaultValueReturn(arrayHeader[29], "0");
                    }

                    if (arrayHeader.Length > 32)
                    {
                        ParReason_Id = arrayHeader[32];
                        ParReasonType_Id = arrayHeader[33];
                    }

                    if (arrayHeader.Length > 33)
                    {
                        parDepartment_Id = arrayHeader[34];
                    }

                    int parDepartmentId = 0;
                    int.TryParse(parDepartment_Id, out parDepartmentId);
                    int CollectionLevel2Id = InsertCollectionLevel2(consolidationLevel1, consolidationLevel2, c.AuditorId, c.Shift, c.Period, Phase, c.Reaudit, c.ReauditNumber, c.Level02CollectionDate,
                                                StartPhase, c.Evaluate, sampleCollect, ConsecuticeFalireIs, ConsecutiveFailureTotal, NotEvaluateIs, Duplicated, haveReaudit, reauditLevel,
                                                haveCorrectiveAction, havePhases, completed, idCollectionLevel2, AlertLevel, sequential, side,
                                                weievaluation, weidefects, defects, totallevel3withdefects, totalLevel3evaluation,
                                                avaliacaoultimoalerta, monitoramentoultimoalerta, evaluatedresult,
                                                defectsresult, isemptylevel3, startphaseevaluation, endphaseevaluation,
                                                hashKey, cluster, ParReason_Id, ParReasonType_Id, parDepartmentId);

                    if (arrayHeader.Length > 30)
                    {
                        string reprocesso = DefaultValueReturn(arrayHeader[30], null);

                        if (reprocesso != null)
                            InsertCollectionLevel2Object(CollectionLevel2Id, reprocesso);
                    }

                    if (CollectionLevel2Id == 2627)
                    {
                        int jsonUpdate = updateJsonDuplicated(c.Id);
                    }
                    else if (CollectionLevel2Id > 0 && !string.IsNullOrEmpty(c.Level03ResultJSon))
                    {

                        InsertCollectionLevel3(CollectionLevel2Id.ToString(), c.level02_Id, c.Level03ResultJSon, c.AuditorId, Duplicated, filho);

                        int IsBEA = 0;

                        using (var db = new SgqDbDevEntities())
                        {

                            var BEA = db.ParLevel1VariableProductionXLevel1.FirstOrDefault(r => r.ParLevel1_Id == c.level01_Id);
                            if (BEA != null)
                                IsBEA = BEA.ParLevel1VariableProduction_Id;

                        }

                        var isRecravacao = new SGQDBContext.ParLevel1(db, quebraProcesso).getById(c.level01_Id).IsRecravacao == true;

                        if (IsBEA == 3 || IsBEA == 2 || c.level01_Id == 43 || c.level01_Id == 42 || isRecravacao || (c.Unit_Id == 4 && c.level01_Id == 22) || (c.Unit_Id == 4 && c.level01_Id == 47)) //se fora a unidade de CPG reconsolida o Vácuo GRD
                            ReconsolidationToLevel3(CollectionLevel2Id.ToString());

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

                        if (!string.IsNullOrEmpty(c.CorrectiveActionJson) && c.CorrectiveActionJson != "null")
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

                        var CollectionLevel2ConsolidationDB = new SGQDBContext.CollectionLevel2Consolidation(db);
                        var collectionLevel2Consolidation = CollectionLevel2ConsolidationDB.getConsolidation(ConsolidationLevel2_Id, c.level02_Id);

                        var updateConsolidationLevel2Id = updateConsolidationLevel2(ConsolidationLevel2_Id, AlertLevel, avaliacaoultimoalerta, monitoramentoultimoalerta, collectionLevel2Consolidation);

                        var ConsolidationLevel1XConsolidationLevel2DB = new ConsolidationLevel1XConsolidationLevel2(db);
                        var consolidationLevel1XConsolidationLevel2 = ConsolidationLevel1XConsolidationLevel2DB.getConsolidation(ConsolidationLevel1_Id);

                        var updateConsolidationLevel1Id = updateConsolidationLevel1(ConsolidationLevel1_Id, AlertLevel, avaliacaoultimoalerta, monitoramentoultimoalerta, consolidationLevel1XConsolidationLevel2);

                        if (filho)
                        {
                            ReconsolidationToLevel3(CollectionLevel2Id.ToString());
                        }

                    }
                    else if (string.IsNullOrEmpty(c.Level03ResultJSon))
                    {
                        var ids = "Level2ID:" + CollectionLevel2Id + " CollectionJsonID: " + c.Id;
                        int insertLog = insertLogJson("", "Level 3 VAZIO " + ids, "N/A", "N/A", "Erro no [ProcessJson].");
                    }

                    collectionLevel2XCollectionJsonList.Add(new KeyValuePair<int, int>(CollectionLevel2Id, c.Id));

                }

                InsertCollectionLevel2XCollectionJson(collectionLevel2XCollectionJsonList);

                return null;
            }
            catch (Exception ex)
            {
                int insertLog = insertLogJson("", ex.Message + " | " + ex.StackTrace, "N/A", "N/A", "Erro no [ProcessJson]" + ex.ToString());
                throw ex;
            }
        }

        private static bool VerificaStringNulaUndefinedNaN(string value)
        {
            return (string.IsNullOrEmpty(value) || value.ToLowerInvariant() == "null".ToLowerInvariant() || value.ToLowerInvariant() == "undefined".ToLowerInvariant() || value.ToLowerInvariant() == "nan".ToLowerInvariant());
        }

        protected int updateJson(int CollectionJson_Id)
        {
            string sql = "UPDATE CollectionJson SET IsProcessed=1 WHERE ID=@CollectionJsonId";
            string conexao = this.conexao;
            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {

                        command.CommandType = CommandType.Text;
                        command.Parameters.Add(new SqlParameter("@CollectionJsonId", CollectionJson_Id));

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

        protected int updateJsonDuplicated(int CollectionJson_Id)
        {
            string sql = "UPDATE CollectionJson SET IsProcessed=1, TTP = '2627' WHERE ID=@CollectionJsonId";
            string conexao = this.conexao;

            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.Add(new SqlParameter("@CollectionJsonId", CollectionJson_Id));

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

        protected int updateConsolidationLevel2(int ConsolidationLevel2_Id, string AlertLevel, string LastEvaluationAlert, string LastLevel2Alert, SGQDBContext.CollectionLevel2Consolidation CollectionLevel2Consolidation)
        {
            //verificar se não vai sobreescrever informação com tablet antigo

            if (LastEvaluationAlert == "NULL")
            {
                LastEvaluationAlert = "0";
            }

            if (LastLevel2Alert == "NULL")
            {
                LastLevel2Alert = "null";
            }

            int LastEvaluationAlertCheck = 0;

            if (LastEvaluationAlert == "NULL")
            {
                LastEvaluationAlertCheck = Convert.ToInt32(0);
            }
            else
            {
                LastEvaluationAlertCheck = Convert.ToInt32(LastEvaluationAlert);
            }


            if (CollectionLevel2Consolidation.LastEvaluationAlert > LastEvaluationAlertCheck)
            {
                LastEvaluationAlert = CollectionLevel2Consolidation.LastEvaluationAlert.ToString();
            }

            string sql = $@"UPDATE ConsolidationLevel2 SET AlertLevel=@AlertLevel, 
                WeiEvaluation=@WeiEvaluation, 
                EvaluateTotal=@EvaluateTotal, 
                DefectsTotal=@DefectsTotal, 
                WeiDefects=@WeiDefects, 
                TotalLevel3Evaluation=@TotalLevel3Evaluation, 
                TotalLevel3WithDefects=@TotalLevel3WithDefects, 
                LastEvaluationAlert=@LastEvaluationAlert, 
                LastLevel2Alert=@LastLevel2Alert,
                EvaluatedResult=@EvaluatedResult, 
                DefectsResult=@DefectsResult WHERE ID=@ConsolidationLevel2_Id";

            string conexao = this.conexao;
            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {

                        command.CommandType = CommandType.Text;
                        command.Parameters.Add(new SqlParameter("@AlertLevel", AlertLevel.ToString().Replace(",", ".")));
                        command.Parameters.Add(new SqlParameter("@WeiEvaluation", CollectionLevel2Consolidation.WeiEvaluationTotal.ToString().Replace(",", ".")));
                        command.Parameters.Add(new SqlParameter("@EvaluateTotal", CollectionLevel2Consolidation.TotalLevel3Evaluation.ToString().Replace(",", ".")));
                        command.Parameters.Add(new SqlParameter("@DefectsTotal", CollectionLevel2Consolidation.DefectsTotal.ToString().Replace(",", ".")));
                        command.Parameters.Add(new SqlParameter("@WeiDefects", CollectionLevel2Consolidation.WeiDefectsTotal.ToString().Replace(",", ".")));
                        command.Parameters.Add(new SqlParameter("@TotalLevel3Evaluation", CollectionLevel2Consolidation.TotalLevel3Evaluation.ToString().Replace(",", ".")));
                        command.Parameters.Add(new SqlParameter("@TotalLevel3WithDefects", CollectionLevel2Consolidation.TotalLevel3WithDefects.ToString().Replace(",", ".")));
                        command.Parameters.Add(new SqlParameter("@LastEvaluationAlert", LastEvaluationAlert.ToString().Replace(",", ".")));
                        command.Parameters.Add(new SqlParameter("@LastLevel2Alert", LastLevel2Alert.ToString().Replace(",", ".")));
                        command.Parameters.Add(new SqlParameter("@EvaluatedResult", CollectionLevel2Consolidation.EvaluatedResult));
                        command.Parameters.Add(new SqlParameter("@DefectsResult", CollectionLevel2Consolidation.DefectsResult));
                        command.Parameters.Add(new SqlParameter("@ConsolidationLevel2_Id", ConsolidationLevel2_Id.ToString().Replace(",", ".")));

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

        protected int updateConsolidationLevel1(int ConsolidationLevel1_Id, string AlertLevel, string LastEvaluationAlert, string LastLevel2Alert, SGQDBContext.ConsolidationLevel1XConsolidationLevel2 CL1XCL2)
        {

            if (LastEvaluationAlert == "NULL")
            {
                LastEvaluationAlert = "0";
            }

            if (LastLevel2Alert == "NULL")
            {
                LastLevel2Alert = "null";
            }

            int LastEvaluationAlertCheck = Convert.ToInt32(LastEvaluationAlert);
            if (CL1XCL2.LastEvaluationAlert > LastEvaluationAlertCheck)
            {
                LastEvaluationAlert = CL1XCL2.LastEvaluationAlert.ToString();
            }

            string sql = $@"UPDATE ConsolidationLevel1 SET AtualAlert=@AtualAlert,
                    Evaluation=@Evaluation,
                    WeiEvaluation=@WeiEvaluation,
                    EvaluateTotal=@EvaluateTotal, 
                    DefectsTotal=@DefectsTotal, 
                    WeiDefects=@WeiDefects, 
                    TotalLevel3Evaluation=@TotalLevel3Evaluation, 
                    TotalLevel3WithDefects=@TotalLevel3WithDefects, 
                    LastEvaluationAlert=@LastEvaluationAlert,
                    LastLevel2Alert=@LastLevel2Alert,
                    EvaluatedResult=@EvaluatedResult, 
                    DefectsResult=@DefectsResult 
                    WHERE ID=@ConsolidationLevel1_Id";

            string conexao = this.conexao;
            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {

                        command.CommandType = CommandType.Text;
                        command.Parameters.Add(new SqlParameter("@AtualAlert", AlertLevel.ToString().Replace(",", ".")));
                        command.Parameters.Add(new SqlParameter("@Evaluation", CL1XCL2.EvaluateTotal.ToString().Replace(",", ".")));
                        command.Parameters.Add(new SqlParameter("@WeiEvaluation", CL1XCL2.WeiEvaluation.ToString().Replace(",", ".")));
                        command.Parameters.Add(new SqlParameter("@EvaluateTotal", CL1XCL2.EvaluateTotal.ToString().Replace(",", ".")));
                        command.Parameters.Add(new SqlParameter("@DefectsTotal", CL1XCL2.DefectsTotal.ToString().Replace(",", ".")));
                        command.Parameters.Add(new SqlParameter("@WeiDefects", CL1XCL2.WeiDefects.ToString().Replace(",", ".")));
                        command.Parameters.Add(new SqlParameter("@TotalLevel3Evaluation", CL1XCL2.TotalLevel3Evaluation.ToString().Replace(",", ".")));
                        command.Parameters.Add(new SqlParameter("@TotalLevel3WithDefects", CL1XCL2.TotalLevel3WithDefects.ToString().Replace(",", ".")));
                        command.Parameters.Add(new SqlParameter("@LastEvaluationAlert", LastEvaluationAlert.ToString().Replace(",", ".")));
                        command.Parameters.Add(new SqlParameter("@LastLevel2Alert", LastLevel2Alert.ToString().Replace(",", ".")));
                        command.Parameters.Add(new SqlParameter("@EvaluatedResult", CL1XCL2.EvaluatedResult.ToString().Replace(",", ",")));
                        command.Parameters.Add(new SqlParameter("@DefectsResult", CL1XCL2.DefectsResult.ToString().Replace(",", ".")));
                        command.Parameters.Add(new SqlParameter("@ConsolidationLevel1_Id", ConsolidationLevel1_Id.ToString().Replace(",", ".")));

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

        protected int updateCorrectiveAction_CollectionLevel2_By_ParLevel1(string ParLevel1_Id, string ParCompany_Id, string dataInicio, string dataFim, string reauditnumber)
        {

            //string sql = "UPDATE CollectionLevel2 SET HaveCorrectiveAction=0 WHERE " +
            //    "ParLevel1_Id='" + ParLevel1_Id + "' AND " +
            //    "UnitId='" + ParCompany_Id +"' AND " +
            //    "CollectionDate BETWEEN '" + dataInicio + " 00:00:00' AND " +
            //    "'" + dataFim + " 23:59:59' AND HaveCorrectiveAction= 1 and reauditnumber='" + reauditnumber + "'";

            string sql = $@"UPDATE CollectionLevel2 SET HaveCorrectiveAction = 0 WHERE 
                ParLevel1_Id=@ParLevel1_Id AND 
                UnitId=@ParCompany_Id AND 
                CollectionDate BETWEEN @DataInicio AND 
                @DataFim AND 
                HaveCorrectiveAction= 1 and 
                reauditnumber=@ReauditNumber";

            string conexao = this.conexao;

            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {

                        command.CommandType = CommandType.Text;
                        command.Parameters.Add(new SqlParameter("@ParLevel1_Id", ParLevel1_Id));
                        command.Parameters.Add(new SqlParameter("@ParCompany_Id", ParCompany_Id));
                        command.Parameters.Add(new SqlParameter("@DataInicio", dataInicio + " 00:00:00"));
                        command.Parameters.Add(new SqlParameter("@DataFim", dataFim + " 23:59:59"));
                        command.Parameters.Add(new SqlParameter("@ReauditNumber", reauditnumber));

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

        protected int updateLevel02CorrectiveActionReaudit(string id, string correctiveAction, string reaudit)
        {
            //string sql = "UPDATE CollectionLevel02 SET HaveCorrectiveAction='" + correctiveAction + "', HaveReaudit='" + reaudit + "' WHERE ID='" + id + "'";

            string sql = "UPDATE CollectionLevel02 SET HaveCorrectiveAction=@HaveCorrectiveAction, HaveReaudit=@HaveReaudit WHERE ID=@Id";

            string conexao = this.conexao;

            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {

                        command.CommandType = CommandType.Text;
                        command.Parameters.Add(new SqlParameter("@HaveCorrectiveAction", correctiveAction));
                        command.Parameters.Add(new SqlParameter("@HaveReaudit", reaudit));
                        command.Parameters.Add(new SqlParameter("@Id", id));

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
        /// <param name="Shift">Turno</param>
        /// <param name="Period">Periodo</param>
        /// <returns></returns>
        protected SGQDBContext.ConsolidationLevel1 InsertConsolidationLevel1(int ParCompany_Id, int ParLevel1_Id, DateTime collectionDate, int Shift, int Period, string cluster, int parDepartment_Id)
        {
            var ConsolidationLevel1DB = new SGQDBContext.ConsolidationLevel1(db);

            //Script de Insert para consolidação
            //string sql = "INSERT ConsolidationLevel1 ([UnitId],[DepartmentId],[ParLevel1_Id],[AddDate],[AlterDate],[ConsolidationDate],[shift],[period]) " +
            //             "VALUES " +
            //             "('" + ParCompany_Id + "','" + departmentId + "','" + ParLevel1_Id + "', GetDate(),null, CONVERT(DATE, '" + collectionDate.ToString("yyyy-MM-dd") + "'),"+
            //             Shift+","+Period+")"+
            //             "SELECT @@IDENTITY AS 'Identity'";

            string sql = $@"INSERT ConsolidationLevel1 ([UnitId],
                [DepartmentId],
                [ParLevel1_Id],
                [AddDate],
                [AlterDate],
                [ConsolidationDate],
                [shift],
                [period]) 
                VALUES 
                (@ParCompany_Id, 
                1,
                @ParLevel1_Id, 
                GetDate(),
                null, 
                CONVERT(DATE, @CollectionDate), 
                @Shift,
                @Period)

                SELECT @@IDENTITY AS 'Identity'";

            string conexao = this.conexao;
            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {

                        command.CommandType = CommandType.Text;
                        command.Parameters.Add(new SqlParameter("@ParCompany_Id", ParCompany_Id));
                        command.Parameters.Add(new SqlParameter("@ParLevel1_Id", ParLevel1_Id));
                        command.Parameters.Add(new SqlParameter("@CollectionDate", collectionDate.ToString("yyyy-MM-dd")));
                        command.Parameters.Add(new SqlParameter("@Shift", Shift));
                        command.Parameters.Add(new SqlParameter("@Period", Period));

                        var i = Convert.ToInt32(command.ExecuteScalar());
                        //Se o registro for inserido retorno o Id da Consolidação
                        if (i > 0)
                        {

                            InsertConsolidationLevel1XCluster(i, cluster);

                            if (parDepartment_Id > 0)
                                InsertConsolidationLevel1XParDepartment(i, parDepartment_Id);

                            return ConsolidationLevel1DB.getConsolidation(ParCompany_Id, ParLevel1_Id, collectionDate, Shift, Period, cluster, parDepartment_Id);
                        }
                        else
                        {
                            //Caso ocorra algum erro, retorno zero
                            int insertLog = insertLogJson(i.ToString(), "Erro", "Erro na InsertConsolidationLevel1XCluster", "Erro na InsertConsolidationLevel1XCluster", "InsertConsolidationLevel1XCluster");
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

        protected int InsertConsolidationLevel1XCluster(int consolidationLevel1_Id, string cluster)
        {
            var ConsolidationLevel1DB = new SGQDBContext.ConsolidationLevel1(db);

            //string sql = "INSERT ConsolidationLevel1XCluster ([consolidationLevel1_Id],[ParCluster_Id]) " +
            //             "VALUES " +
            //             "('" + consolidationLevel1_Id + "','" + cluster + "')" +
            //             "SELECT @@IDENTITY AS 'Identity'";

            string sql = $@"INSERT ConsolidationLevel1XCluster ([consolidationLevel1_Id],[ParCluster_Id]) 
             VALUES 
             (@ConsolidationLevel1_Id, @Cluster)
             SELECT @@IDENTITY AS 'Identity'";

            string conexao = this.conexao;
            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.Add(new SqlParameter("@ConsolidationLevel1_Id", consolidationLevel1_Id));
                        command.Parameters.Add(new SqlParameter("@Cluster", cluster));

                        var i = Convert.ToInt32(command.ExecuteScalar());
                        //Se o registro for inserido retorno o Id da Consolidação
                        if (i > 0)
                        {
                            return i;
                        }
                        else
                        {
                            //Caso ocorra algum erro, retorno zero
                            return 0;
                        }
                    }

                    if (connection.State == System.Data.ConnectionState.Open) connection.Close();
                }
            }
            //Caso ocorra alguma Exception, grava o log e retorna zero
            catch (SqlException ex)
            {
                int insertLog = insertLogJson(sql, ex.Message, "N/A", "N/A", "InsertConsolidationLevel1XCluster");
                throw ex;
            }
            catch (Exception ex)
            {
                int insertLog = insertLogJson(sql, ex.Message, "N/A", "N/A", "InsertConsolidationLevel1XCluster");
                throw ex;
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
        protected SGQDBContext.ConsolidationLevel2 InsertConsolidationLevel2(int ConsolidationLevel1_Id, int ParLevel2_Id, int ParCompany_Id, DateTime collectionDate, bool reaudit, int reauditNumber, string cluster, int parDepartment_Id)
        {
            //Verifica se já existe uma consolidação para o level02
            var ConsolidationLevel2DB = new SGQDBContext.ConsolidationLevel2(db);
            //var ConsolidationLevel2 = ConsolidationLevel2DB.getbYConsolidationLevel1(Convert.ToInt32(Level01ConsolidationId), Convert.ToInt32(Level02Id));

            //if (ConsolidationLevel2 != null)
            //{
            //    return ConsolidationLevel2;
            //}

            int reaud = 0;
            if (reaudit)
                reaud = 1;

            //Gera o Script de Insert no Banco
            //string sql = "INSERT ConsolidationLevel2 ([ConsolidationLevel1_Id], [ParLevel2_Id], [UnitId], [AddDate], [AlterDate], [ConsolidationDate],[ReauditIs],[ReauditNumber]) " +
            //             "VALUES  " +
            //             "('" + ConsolidationLevel1_Id + "', '" + ParLevel2_Id + "', '" + ParCompany_Id + "', GETDATE(), NULL, CAST(N'" + collectionDate.ToString("yyyy-MM-dd") + "' AS DateTime)"+
            //             reaud + reauditNumber+") " +
            //             "SELECT @@IDENTITY AS 'Identity'";

            //string sql = "INSERT ConsolidationLevel2 ([ConsolidationLevel1_Id], [ParLevel2_Id], [UnitId], [AddDate], [AlterDate], [ConsolidationDate], [ReauditIs]) " +
            //             "VALUES  " +
            //             "('" + ConsolidationLevel1_Id + "', '" + ParLevel2_Id + "', '" + ParCompany_Id + "', GETDATE(), NULL, CAST(N'" + collectionDate.ToString("yyyy-MM-dd") + "' AS DateTime)"+","+reaud + ") " +
            //             "SELECT @@IDENTITY AS 'Identity'";

            //string sql = "INSERT ConsolidationLevel2 ([ConsolidationLevel1_Id], [ParLevel2_Id], [UnitId], [AddDate], [AlterDate], [ConsolidationDate], [ReauditIs],[ReauditNumber]) " +
            //            "VALUES  " +
            //            "('" + ConsolidationLevel1_Id + "', '" + ParLevel2_Id + "', '" + ParCompany_Id + "', GETDATE(), NULL, CAST(N'" + collectionDate.ToString("yyyy-MM-dd") + "' AS DateTime)," +
            //            reaud + "," + reauditNumber + " ) " +
            //            "SELECT @@IDENTITY AS 'Identity'";

            string sql = $@"INSERT ConsolidationLevel2 ([ConsolidationLevel1_Id], 
                        [ParLevel2_Id], 
                        [UnitId], 
                        [AddDate], 
                        [AlterDate], 
                        [ConsolidationDate], 
                        [ReauditIs],
                        [ReauditNumber]) 
                        VALUES  
                        (@ConsolidationLevel1_Id,
                        @ParLevel2_Id, 
                        @ParCompany_Id, 
                        GETDATE(), 
                        NULL, 
                        CAST(@CollectionDate AS DateTime),
                        @Reaud,
                        @ReauditNumber)
                        SELECT @@IDENTITY AS 'Identity'";

            string conexao = this.conexao;
            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {

                        command.CommandType = CommandType.Text;
                        command.Parameters.Add(new SqlParameter("@ConsolidationLevel1_Id", ConsolidationLevel1_Id));
                        command.Parameters.Add(new SqlParameter("@ParLevel2_Id", ParLevel2_Id));
                        command.Parameters.Add(new SqlParameter("@ParCompany_Id", ParCompany_Id));
                        command.Parameters.Add(new SqlParameter("@CollectionDate", collectionDate.ToString("yyyy-MM-dd")));
                        command.Parameters.Add(new SqlParameter("@Reaud", reaudit));
                        command.Parameters.Add(new SqlParameter("@ReauditNumber", cluster));

                        connection.Open();

                        var i = Convert.ToInt32(command.ExecuteScalar());
                        //Se inserir corretamente, retorno o Id da Consolidação
                        if (i > 0)
                        {

                            InsertConsolidationLevel2XCluster(i, cluster);

                            if (parDepartment_Id > 0)
                                InsertConsolidationLevel2XParDepartment(i, parDepartment_Id);

                            if (reaudit)
                                return ConsolidationLevel2DB.getByConsolidationLevel1(ParCompany_Id, ConsolidationLevel1_Id, ParLevel2_Id, 1, reauditNumber.ToString(), cluster, parDepartment_Id);
                            else
                                return ConsolidationLevel2DB.getByConsolidationLevel1(ParCompany_Id, ConsolidationLevel1_Id, ParLevel2_Id, cluster, parDepartment_Id);
                        }
                        else
                        {
                            //Caso não ocorra a inserção, retorno zero
                            int insertLog = insertLogJson(i.ToString(), "Erro", "Erro na InsertConsolidationLevel2XCluster", "Erro na InsertConsolidationLevel2XCluster", "InsertConsolidationLevel2XCluster");
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
        protected int InsertConsolidationLevel2XCluster(int consolidationLevel2_Id, string cluster)
        {
            var ConsolidationLevel1DB = new SGQDBContext.ConsolidationLevel1(db);

            //string sql = "INSERT ConsolidationLevel2XCluster ([consolidationLevel2_Id]," +
            //             "[ParCluster_Id]) " +
            //             "VALUES " +
            //             "('" + consolidationLevel2_Id + "'," +
            //             "'" + cluster + "')" +
            //             "SELECT @@IDENTITY AS 'Identity'";

            string sql = @"INSERT ConsolidationLevel2XCluster ([consolidationLevel2_Id],
                         [ParCluster_Id]) 
                         VALUES 
                         (@ConsolidationLevel2_Id,
                         @Cluster)
                         SELECT @@IDENTITY AS 'Identity'";

            string conexao = this.conexao;
            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {

                        command.CommandType = CommandType.Text;
                        command.Parameters.Add(new SqlParameter("@ConsolidationLevel2_Id", consolidationLevel2_Id));
                        command.Parameters.Add(new SqlParameter("@Cluster", cluster));


                        var i = Convert.ToInt32(command.ExecuteScalar());

                        //Se o registro for inserido retorno o Id da Consolidação
                        if (i > 0)
                        {
                            return i;
                        }
                        else
                        {
                            //Caso ocorra algum erro, retorno zero
                            return 0;
                        }
                    }
                    if (connection.State == System.Data.ConnectionState.Open)
                        connection.Close();
                }
            }
            //Caso ocorra alguma Exception, grava o log e retorna zero
            catch (SqlException ex)
            {
                int insertLog = insertLogJson(sql, ex.Message, "N/A", "N/A", "InsertConsolidationLevel2XCluster");
                throw ex;
            }
            catch (Exception ex)
            {
                int insertLog = insertLogJson(sql, ex.Message, "N/A", "N/A", "InsertConsolidationLevel2XCluster");
                throw ex;
            }
        }
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
        protected int InsertCollectionLevel2(SGQDBContext.ConsolidationLevel1 ConsolidationLevel1, SGQDBContext.ConsolidationLevel2 ConsolidationLevel2, int AuditorId, int Shift,
                                                  int Period, string Phase, bool Reaudit, int ReauditNumber, DateTime CollectionDate,
                                                   string StartPhase, int Evaluation, int Sample, string ConsecuticeFalireIs, string ConsecutiveFailureTotal, string NotEvaluateIs,
                                                   string Duplicated, string haveReaudit, int reauditLevel, string haveCorrectiveAction, string HavePhase, string Completed, string id, string AlertLevel,
                                                   string sequential, string side, string WeiEvaluation, string Defects, string WeiDefects, string TotalLevel3WithDefects, string totalLevel3evaluation,
                                                   string avaliacaoultimoalerta, string monitoramentoultimoalerta, string evaluatedresult, string defectsresult, string isemptylevel3,
                                                   string startphaseevaluation, string endphaseevaluation, string hashKey = null, string cluster = null,
                                                   string ParReason_Id = null, string ParReasonType_Id = null, int? parDepartment_Id = null)
        {

            //var buscaParLevel1HashKey = "SELECT TOP 1 Hashkey FROM ParLevel1 WHERE id = " + ConsolidationLevel1.ParLevel1_Id.ToString();
            var buscaParLevel1HashKey = "SELECT TOP 1 Hashkey FROM ParLevel1 WHERE id = @ParLevel1_Id";

            string con = this.conexao;
            try
            {
                using (SqlConnection connection = new SqlConnection(con))
                {
                    using (SqlCommand command = new SqlCommand(buscaParLevel1HashKey, connection))
                    {

                        command.CommandType = CommandType.Text;
                        command.Parameters.Add(new SqlParameter("@ParLevel1_Id", ConsolidationLevel1.ParLevel1_Id.ToString()));

                        connection.Open();

                        var reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            hashKey = reader[0].ToString();
                        }

                    }
                    if (connection.State == System.Data.ConnectionState.Open) connection.Close();
                }
            }
            catch
            {

            }

            //Converte a data da coleta
            string sql = null;
            //Se o Id for igual a zero é um insert

            string key = ConsolidationLevel2.UnitId.ToString();

            key += "-" + Shift;
            key += "-" + Period;

            key += "-" + ConsolidationLevel1.ParLevel1_Id.ToString();
            key += "-" + ConsolidationLevel2.ParLevel2_Id.ToString();
            if (hashKey == "1" || hashKey == "5")
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

            if (Reaudit == true && ConsolidationLevel1.ParLevel1_Id != 3)
            {
                key += "-r" + ReauditNumber;
            }

            key += "-" + cluster;

            var keySolid = key;

            //NotEvaluateIs = (naoAvaliado) ? "1" : "0";

            var CollectionLevel2DB = new SGQDBContext.CollectionLevel2(db);
            var colLevel2 = CollectionLevel2DB.GetByKey(key);
            if (colLevel2 != null)
                id = Convert.ToString(colLevel2.Id);

            if (evaluatedresult == null || evaluatedresult == "undefined" || evaluatedresult == "NaN")
                evaluatedresult = "0";
            if (defectsresult == null || defectsresult == "undefined" || defectsresult == "NaN")
                defectsresult = "0";
            if (TotalLevel3WithDefects == null || TotalLevel3WithDefects == "undefined" || TotalLevel3WithDefects == "NaN")
                TotalLevel3WithDefects = "0";
            if (WeiDefects == null || WeiDefects == "undefined" || WeiDefects == "NaN")
                WeiDefects = "0";
            if (Defects == null || Defects == "undefined" || Defects == "NaN")
                Defects = "0";
            if (WeiEvaluation == null || WeiEvaluation == "undefined" || WeiEvaluation == "NaN")
                WeiEvaluation = "0";


            if (id == "0")
            {
                /*PQP ESSE StartPhase startphaseevaluation*/
                //sql = "INSERT INTO CollectionLevel2 ([Key],[ConsolidationLevel2_Id],[ParLevel1_Id],[ParLevel2_Id],[UnitId],[AuditorId],[Shift],[Period],[Phase],[ReauditIs],[ReauditNumber],[CollectionDate],[StartPhaseDate],[EvaluationNumber],[Sample],[AddDate],[AlterDate],[ConsecutiveFailureIs],[ConsecutiveFailureTotal],[NotEvaluatedIs],[Duplicated],[HaveReaudit],[ReauditLevel], [HaveCorrectiveAction],[HavePhase],[Completed],[AlertLevel],[Sequential],[Side],[WeiEvaluation],[Defects],[WeiDefects],[TotalLevel3WithDefects], [TotalLevel3Evaluation], [LastEvaluationAlert],[LastLevel2Alert],[EvaluatedResult],[DefectsResult],[IsEmptyLevel3], [StartPhaseEvaluation]) " +
                //"VALUES " +
                //"('" + key + "', '" + ConsolidationLevel2.Id + "','" + ConsolidationLevel1.ParLevel1_Id + "','" + ConsolidationLevel2.ParLevel2_Id + "','" + ConsolidationLevel1.UnitId + "','" + AuditorId + "','" + Shift + "','" + Period + "','" + Phase + "','" + BoolConverter(Reaudit.ToString()) + "','" + ReauditNumber + "', CAST(N'" + CollectionDate.ToString("yyyy-MM-dd HH:mm:ss") + "' AS DateTime), " + StartPhase + ",'" + Evaluation + "','" + Sample + "',GETDATE(),NULL,'" + ConsecuticeFalireIs + "','" + ConsecutiveFailureTotal + "','" + NotEvaluateIs + "','" + Duplicated + "', '" + haveReaudit + "', " + reauditLevel + ", '" + haveCorrectiveAction + "', '" + HavePhase + "', '" + Completed + "', '" + AlertLevel + "', '" + sequential + "', '" + side + "','" + WeiEvaluation + "','" + Defects + "','" + WeiDefects + "','" + TotalLevel3WithDefects + "', '" + totalLevel3evaluation + "', '" + avaliacaoultimoalerta + "', '" + monitoramentoultimoalerta + "', '" + evaluatedresult + "', '" + defectsresult + "', '" + isemptylevel3 + "', '" + startphaseevaluation + "') ";

                //   sql = "INSERT INTO CollectionLevel2 ([Key],[ConsolidationLevel2_Id],[ParLevel1_Id],[ParLevel2_Id],[UnitId],[AuditorId],[Shift],[Period],[Phase],[ReauditIs],[ReauditNumber],[CollectionDate],[StartPhaseDate],[EvaluationNumber],[Sample],[AddDate],[AlterDate],[ConsecutiveFailureIs],[ConsecutiveFailureTotal],[NotEvaluatedIs],[Duplicated],[HaveReaudit],[ReauditLevel], [HaveCorrectiveAction],[HavePhase],[Completed],[AlertLevel],[Sequential],[Side],[WeiEvaluation],[Defects],[WeiDefects],[TotalLevel3WithDefects], [TotalLevel3Evaluation], [LastEvaluationAlert],[LastLevel2Alert],[EvaluatedResult],[DefectsResult],[IsEmptyLevel3], [StartPhaseEvaluation], [EndPhaseEvaluation]) " +
                //"VALUES " +
                //"('" + key + "', '" + ConsolidationLevel2.Id + "','" + ConsolidationLevel1.ParLevel1_Id + "','" + ConsolidationLevel2.ParLevel2_Id + "','" + ConsolidationLevel1.UnitId + "','" + AuditorId + "','" + Shift + "','" + Period + "','" + Phase + "','" + BoolConverter(Reaudit.ToString()) + "','" + ReauditNumber + "', CAST(N'" + CollectionDate.ToString("yyyy-MM-dd HH:mm:ss") + "' AS DateTime), GETDATE(),'" + Evaluation + "','" + Sample + "',GETDATE(),NULL,'" + ConsecuticeFalireIs + "','" + ConsecutiveFailureTotal + "','" + NotEvaluateIs + "','" + Duplicated + "', '" + haveReaudit + "', " + reauditLevel + ", '" + haveCorrectiveAction + "', '" + HavePhase + "', '" + Completed + "', '" + AlertLevel + "', '" + sequential + "', '" + side + "','" + WeiEvaluation + "','" + Defects + "','" + WeiDefects + "','" + TotalLevel3WithDefects + "', '" + totalLevel3evaluation + "', '" + avaliacaoultimoalerta + "', '" + monitoramentoultimoalerta + "', '" + evaluatedresult + "', '" + defectsresult + "', '" + isemptylevel3 + "', '" + startphaseevaluation + "', '" + endphaseevaluation + "') ";

                //   sql += " SELECT @@IDENTITY AS 'Identity' ";

                //   sql = sql.Replace("'NULL'", "NULL");

                sql = $@"INSERT INTO CollectionLevel2 ([Key],
                    [ConsolidationLevel2_Id],
                    [ParLevel1_Id],
                    [ParLevel2_Id],
                    [UnitId],
                    [AuditorId],
                    [Shift],
                    [Period],
                    [Phase],
                    [ReauditIs],
                    [ReauditNumber],
                    [CollectionDate],
                    [StartPhaseDate],
                    [EvaluationNumber],
                    [Sample],
                    [AddDate],
                    [AlterDate],
                    [ConsecutiveFailureIs],
                    [ConsecutiveFailureTotal],
                    [NotEvaluatedIs],
                    [Duplicated],
                    [HaveReaudit],
                    [ReauditLevel],
                    [HaveCorrectiveAction],
                    [HavePhase],
                    [Completed],
                    [AlertLevel],
                    [Sequential],
                    [Side],
                    [WeiEvaluation],
                    [Defects],
                    [WeiDefects],
                    [TotalLevel3WithDefects],
                    [TotalLevel3Evaluation],
                    [LastEvaluationAlert],
                    [LastLevel2Alert],
                    [EvaluatedResult],
                    [DefectsResult],
                    [IsEmptyLevel3],
                    [StartPhaseEvaluation],
                    [EndPhaseEvaluation]) 
                      VALUES 
                      (@Key,
                      @ConsolidationLevel2_Id,
                      @ConsolidationLevel1_ParLevel1_Id,
                      @ConsolidationLevel2_ParLevel2_Id,
                      @ConsolidationLevel1_UnitId,
                      @AuditorId,
                      @Shift,
                      @Period,
                      @Phase,
                      @HaveReaudit,
                      @ReauditNumber,
                      CAST(@CollectionDate AS DateTime), 
                      GETDATE(),
                      @Evaluation,
                      @Sample,
                      GETDATE(),
                      NULL,
                      @ConsecuticeFalireIs,
                      @ConsecutiveFailureTotal,
                      @NotEvaluateIs,
                      @Duplicated, 
                      @HaveReaudi, 
                      @ReauditLevel, 
                      @HaveCorrectiveAction,
                      @HavePhase,
                      @Completed,
                      @AlertLevel,
                      @sequential,
                      @side,
                      @WeiEvaluation,
                      @Defects,
                      @WeiDefects,
                      @TotalLevel3WithDefects,
                      @TotalLevel3evaluation,
                      @Avaliacaoultimoalerta,
                      @Monitoramentoultimoalerta,
                      @Evaluatedresult,
                      @Defectsresult,
                      @Isemptylevel3,
                      @Startphaseevaluation,
                      @Endphaseevaluation)";

                sql += " SELECT @@IDENTITY AS 'Identity' ";

                sql = sql.Replace("'NULL'", "NULL");
            }
            else
            {
                ///podemos melhorar a verificação para Id zero, id null e id not null
                //Caso contrário  é u Update
                //sql = $@"UPDATE CollectionLevel2 SET NotEvaluatedIs='" + NotEvaluateIs + "', 
                //    AlterDate='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', 
                //    HaveReaudit='" + haveReaudit + "', 
                //    ReauditLevel=" + reauditLevel + ", 
                //    HaveCorrectiveAction='" + haveCorrectiveAction + "', 
                //    WeiEvaluation=" + WeiEvaluation + ", 
                //    Defects=" + defectsresult + ", 
                //    WeiDefects=" + WeiDefects + ", 
                //    TotalLevel3WithDefects=" + TotalLevel3WithDefects + ", 
                //    TotalLevel3Evaluation=" + totalLevel3evaluation + ", 
                //    LastEvaluationAlert=" + avaliacaoultimoalerta + ", 
                //    EvaluatedResult=" + evaluatedresult + ",
                //    DefectsResult=" + defectsresult + ", 
                //    IsEmptyLevel3=" + isemptylevel3 + ",
                //    StartPhaseEvaluation=" + startphaseevaluation + ", 
                //    EndPhaseEvaluation=" + endphaseevaluation + " WHERE Id='" + id + "'";

                sql = $@"UPDATE CollectionLevel2 SET NotEvaluatedIs=@NotEvaluateIs, 
                    AlterDate = @AlterDate, 
                    HaveReaudit = @HaveReaudit , 
                    ReauditLevel = @ReauditLevel, 
                    HaveCorrectiveAction = @HaveCorrectiveAction, 
                    WeiEvaluation = @WeiEvaluation, 
                    Defects = @Defects, 
                    WeiDefects = @WeiDefects, 
                    TotalLevel3WithDefects = @TotalLevel3WithDefects, 
                    TotalLevel3Evaluation = @TotalLevel3Evaluation, 
                    LastEvaluationAlert = @LastEvaluationAlert, 
                    EvaluatedResult = @EvaluatedResult,
                    DefectsResult = @DefectsResult, 
                    IsEmptyLevel3 = @IsEmptyLevel3,
                    StartPhaseEvaluation = @StartPhaseEvaluation, 
                    EndPhaseEvaluation = @EndPhaseEvaluation WHERE Id = @CollectionLevel2_Id";

                sql += " SELECT '" + id + "' AS 'Identity'";
            }

            string conexao = this.conexao;
            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        if (id == "0")
                        {
                            command.CommandType = CommandType.Text;
                            command.Parameters.Add(new SqlParameter("@Key", key));
                            command.Parameters.Add(new SqlParameter("@ConsolidationLevel2_Id", ConsolidationLevel2.Id));
                            command.Parameters.Add(new SqlParameter("@ConsolidationLevel1_ParLevel1_Id", ConsolidationLevel1.ParLevel1_Id));
                            command.Parameters.Add(new SqlParameter("@ConsolidationLevel2_ParLevel2_Id", ConsolidationLevel2.ParLevel2_Id));
                            command.Parameters.Add(new SqlParameter("@ConsolidationLevel1_UnitId", ConsolidationLevel1.UnitId));
                            command.Parameters.Add(new SqlParameter("@AuditorId", AuditorId));
                            command.Parameters.Add(new SqlParameter("@Shift", Shift));
                            command.Parameters.Add(new SqlParameter("@Period", Period));
                            command.Parameters.Add(new SqlParameter("@Phase", Phase));
                            command.Parameters.Add(new SqlParameter("@HaveReaudit", BoolConverter(Reaudit.ToString())));
                            command.Parameters.Add(new SqlParameter("@ReauditNumber", ReauditNumber));
                            command.Parameters.Add(new SqlParameter("@CollectionDate", CollectionDate.ToString("yyyy-MM-dd HH:mm:ss")));
                            command.Parameters.Add(new SqlParameter("@Evaluation", Evaluation));
                            command.Parameters.Add(new SqlParameter("@Sample", Sample));
                            command.Parameters.Add(new SqlParameter("@ConsecuticeFalireIs", ConsecuticeFalireIs));
                            command.Parameters.Add(new SqlParameter("@ConsecutiveFailureTotal", ConsecutiveFailureTotal));
                            command.Parameters.Add(new SqlParameter("@NotEvaluateIs", NotEvaluateIs));
                            command.Parameters.Add(new SqlParameter("@Duplicated", Duplicated));
                            command.Parameters.Add(new SqlParameter("@HaveReaudi", haveReaudit));
                            command.Parameters.Add(new SqlParameter("@ReauditLevel", reauditLevel));
                            command.Parameters.Add(new SqlParameter("@HaveCorrectiveAction", haveCorrectiveAction));
                            command.Parameters.Add(new SqlParameter("@HavePhase", HavePhase));
                            command.Parameters.Add(new SqlParameter("@Completed", Completed));
                            command.Parameters.Add(new SqlParameter("@AlertLevel", AlertLevel));
                            command.Parameters.Add(new SqlParameter("@Sequential", sequential));
                            command.Parameters.Add(new SqlParameter("@Side", side));
                            command.Parameters.Add(new SqlParameter("@WeiEvaluation", WeiEvaluation));
                            command.Parameters.Add(new SqlParameter("@Defects", Defects));
                            command.Parameters.Add(new SqlParameter("@WeiDefects", WeiDefects));
                            command.Parameters.Add(new SqlParameter("@TotalLevel3WithDefects", TotalLevel3WithDefects));
                            command.Parameters.Add(new SqlParameter("@TotalLevel3evaluation", totalLevel3evaluation));
                            command.Parameters.Add(new SqlParameter("@Avaliacaoultimoalerta", avaliacaoultimoalerta));
                            command.Parameters.Add(new SqlParameter("@Monitoramentoultimoalerta", monitoramentoultimoalerta));
                            command.Parameters.Add(new SqlParameter("@Evaluatedresult", evaluatedresult));
                            command.Parameters.Add(new SqlParameter("@Defectsresult", defectsresult));
                            command.Parameters.Add(new SqlParameter("@Isemptylevel3", isemptylevel3));
                            command.Parameters.Add(new SqlParameter("@Startphaseevaluation", startphaseevaluation));
                            command.Parameters.Add(new SqlParameter("@Endphaseevaluation", endphaseevaluation));
                        }
                        else
                        {

                            command.Parameters.Add(new SqlParameter("@NotEvaluateIs", NotEvaluateIs));
                            command.Parameters.Add(new SqlParameter("@AlterDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                            command.Parameters.Add(new SqlParameter("@HaveReaudit", haveReaudit));
                            command.Parameters.Add(new SqlParameter("@ReauditLevel", reauditLevel));
                            command.Parameters.Add(new SqlParameter("@HaveCorrectiveAction", haveCorrectiveAction));
                            command.Parameters.Add(new SqlParameter("@WeiEvaluation", WeiEvaluation));
                            command.Parameters.Add(new SqlParameter("@Defects", defectsresult));
                            command.Parameters.Add(new SqlParameter("@WeiDefects", WeiDefects));
                            command.Parameters.Add(new SqlParameter("@TotalLevel3WithDefects", TotalLevel3WithDefects));
                            command.Parameters.Add(new SqlParameter("@TotalLevel3Evaluation", totalLevel3evaluation));
                            command.Parameters.Add(new SqlParameter("@LastEvaluationAlert", avaliacaoultimoalerta));
                            command.Parameters.Add(new SqlParameter("@EvaluatedResult", evaluatedresult));
                            command.Parameters.Add(new SqlParameter("@DefectsResult", defectsresult));
                            command.Parameters.Add(new SqlParameter("@IsEmptyLevel3", isemptylevel3));
                            command.Parameters.Add(new SqlParameter("@StartPhaseEvaluation", startphaseevaluation));
                            command.Parameters.Add(new SqlParameter("@EndPhaseEvaluation", endphaseevaluation));
                            command.Parameters.Add(new SqlParameter("@CollectionLevel2_Id", id));
                        }


                        connection.Open();

                        //var teste = command.ExecuteScalar();

                        var i = Convert.ToInt32(command.ExecuteScalar());
                        //Se o script for executado corretamente retorna o Id

                        //Atualiza a situação de reauditoria
                        if (Reaudit)
                        {
                            var UpdateCollectionLevel2DB = new SGQDBContext.UpdateCollectionLevel2(db);
                            UpdateCollectionLevel2DB.UpdateIsReauditByKey(keySolid, Reaudit, Int16.Parse(haveReaudit), ReauditNumber, reauditLevel);
                        }

                        if (i > 0)
                        {
                            if (id == "0")
                            {
                                InsertCollectionLevel2XCluster(i, cluster);
                            }

                            int ParReasonId = 0;

                            Int32.TryParse(ParReason_Id, out ParReasonId);

                            if (ParReasonId > 0)
                            {
                                InsertCollectionLevel2XParReason(i, ParReason_Id, ParReasonType_Id);
                            }

                            if (parDepartment_Id > 0)
                            {
                                InsertCollectionLevel2XParDepartment(i, parDepartment_Id);
                            }

                            return i;
                        }
                        else
                        {
                            //Se o script não for executado corretamente, retorna zero

                            int insertLog = insertLogJson(i.ToString(), "Não entrou na InsertCollectionLevel2XCluster", "Não entrou na InsertCollectionLevel2XCluster", "Não entrou na InsertCollectionLevel2XCluster", "InsertCollectionLevel2XCluster");
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
                    if (hashKey == "1")
                    {
                        var collectionLevel2 = CollectionLevel2DB.GetByKey(key);

                        var updateLevel2Id = InsertCollectionLevel2(ConsolidationLevel1,
                            ConsolidationLevel2, AuditorId, Shift, Period, Phase, Reaudit,
                            ReauditNumber, CollectionDate, StartPhase, Evaluation, Sample,
                            ConsecuticeFalireIs, ConsecutiveFailureTotal, NotEvaluateIs,
                            Duplicated, haveReaudit, reauditLevel, haveCorrectiveAction,
                            HavePhase, Completed, collectionLevel2.Id.ToString(), AlertLevel,
                            sequential, side, WeiEvaluation, Defects, WeiDefects,
                            TotalLevel3WithDefects, totalLevel3evaluation, avaliacaoultimoalerta,
                            monitoramentoultimoalerta, evaluatedresult, defectsresult,
                            isemptylevel3, startphaseevaluation, endphaseevaluation, hashKey,
                            cluster);
                        if (updateLevel2Id > 0)
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

                        if (ex.Number == 2627)
                        {
                            return ex.Number;
                        }

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

        protected int ResultLevel3Delete(int CollectionLevel2_Id)
        {

            string sql = "DELETE FROM Result_Level3 whith (nolock) WHERE CollectionLevel2_Id=@CollectionLevel2_Id";
            string conexao = this.conexao;
            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {

                        command.CommandType = CommandType.Text;
                        command.Parameters.Add(new SqlParameter("@CollectionLevel2_Id", CollectionLevel2_Id));

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
            catch (SqlException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected int InsertCollectionLevel2HeaderField(int CollectionLevel2Id, string headerList)
        {

            //string sql = null;
            List<SqlCommand> sql = new List<SqlCommand>();
            string[] arrayHeaderList = headerList.Split(';');

            //Como fazer tramento de SQLInjection nisso?
            for (int i = 0; i < arrayHeaderList.Length; i++)
            {
                var header = arrayHeaderList[i].Split(',');

                string ParHeaderField_Id = header[0];
                string ParFieldType_Id = header[1];
                string Value = header[2];


                string Evaluation = header.Length > 3 ? header[3] : "0";
                string Sample = header.Length > 3 ? header[4] : "0";

                //Tratamento de erros Gabriel 2017-05-27
                if (ParHeaderField_Id != "undefined" && ParFieldType_Id != "undefined")
                {

                    var query = $@"INSERT INTO[dbo].[CollectionLevel2XParHeaderField]
                               ([CollectionLevel2_Id]
                               ,[ParHeaderField_Id]
                               ,[ParHeaderField_Name]
                               ,[ParFieldType_Id]
                               ,[Evaluation]
                               ,[Sample]
                               ,[Value])
                         VALUES
                               (@CollectionLevel2Id
                               ,@ParHeaderField_Id
                               ,(SELECT Name FROM ParHeaderField (nolock)  WHERE Id=@ParHeaderField_Id)
                               ,@ParFieldType_Id
                               ,@Evaluation
                               ,@Sample
                               ,@Value)";

                    using (SqlConnection connection = new SqlConnection(conexao))
                    {
                        using (SqlCommand cmd = new SqlCommand(query, connection))
                        {

                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.Add(new SqlParameter("@CollectionLevel2Id", CollectionLevel2Id));
                            cmd.Parameters.Add(new SqlParameter("@ParHeaderField_Id", ParHeaderField_Id));
                            cmd.Parameters.Add(new SqlParameter("@ParFieldType_Id", ParFieldType_Id));
                            cmd.Parameters.Add(new SqlParameter("@Evaluation", Evaluation));
                            cmd.Parameters.Add(new SqlParameter("@Sample", Sample));
                            cmd.Parameters.Add(new SqlParameter("@Value", Value));

                            sql.Add(cmd);
                        }
                    }
                }

            }

            //Tratamento de erros Gabriel 2017-05-27
            if (sql.Count > 0)
            {
                var retornoSql = 0;

                string conexao = this.conexao;
                try
                {

                    using (SqlConnection connection = new SqlConnection(conexao))
                    {
                        connection.Open();

                        foreach (var command in sql)
                        {
                            command.Connection = connection;
                            //connection.Open();
                            retornoSql = Convert.ToInt32(command.ExecuteNonQuery());
                        }
                    }

                    return retornoSql;
                }
                //Caso ocorra alguma exception, grava no log e retorna zero
                catch (SqlException ex)
                {
                    int insertLog = insertLogJson(sql.ToString(), ex.Message, "N/A", "N/A", "InsertCollectionLevel2HeaderField");
                    if (ex.Number == 2627) // <-- but this will
                    {
                        return 0;
                    }
                    throw ex;
                }
                catch (Exception ex)
                {
                    int insertLog = insertLogJson(sql.ToString(), ex.Message, "N/A", "N/A", "InsertCollectionLevel2HeaderField");
                    throw ex;
                }
            }
            else //Tratamento de erros Gabriel 2017-05-27
            {
                return 1;
            }

            return 1;
        }

        protected int InsertCollectionLevel2XCluster(int CollectionLevel2Id, string cluster)
        {
            string sql = "INSERT INTO CollectionLevel2XCluster ([CollectionLevel2_Id], [ParCluster_Id]) VALUES (@CollectionLevel2Id, @Cluster)";

            sql += " SELECT @@IDENTITY AS 'Identity' ";

            string conexao = this.conexao;
            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.Add(new SqlParameter("@CollectionLevel2Id", CollectionLevel2Id));
                        command.Parameters.Add(new SqlParameter("@Cluster", cluster));

                        connection.Open();
                        var i = Convert.ToInt32(command.ExecuteScalar());
                        return i;
                    }
                }
            }
            catch (Exception ex)
            {
                int insertLog = insertLogJson(sql, ex.Message, "Erro na InsertCollectionLevel2XCluster", "Erro na InsertCollectionLevel2XCluster", "InsertCollectionLevel2XCluster");
                throw ex;
            }
        }

        protected int InsertCollectionLevel2Object(int CollectionLevel2Id, string Reprocesso)
        {
            string sql = "INSERT INTO CollectionLevel2Object ([CollectionLevel2_Id], [Objeto], [AddDate]) " +
             "VALUES (@CollectionLevel2Id, @Reprocesso, GETDATE()) ";

            sql += " SELECT @@IDENTITY AS 'Identity' ";

            string conexao = this.conexao;
            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.Add(new SqlParameter("@CollectionLevel2Id", CollectionLevel2Id));
                        command.Parameters.Add(new SqlParameter("@Reprocesso", Reprocesso));

                        connection.Open();
                        var i = Convert.ToInt32(command.ExecuteScalar());
                        return i;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void InsertCollectionLevel2XParReason(int CollectionLevel2_Id, string ParReason_Id, string ParReasonType_Id)
        {
            var IsUpdate = false;
            var sql = "";

            using (var db = new SgqDbDevEntities())
            {
                IsUpdate = db.CollectionLevel2XParReason.Any(r => r.CollectionLevel2_Id == CollectionLevel2_Id);
            }

            if (IsUpdate)
            {
                sql = $@"UPDATE CollectionLevel2XParReason set [ParReason_Id] = @ParReason_Id, 
                    [ParReasonType_Id] = @ParReasonType_Id, [AlterDate] = GETDATE() 
                    WHERE [CollectionLevel2_Id] = @CollectionLevel2_Id";
            }
            else
            {
                sql = $@"INSERT INTO CollectionLevel2XParReason ([CollectionLevel2_Id], [ParReason_Id], [ParReasonType_Id], [AddDate]) 
                VALUES (@CollectionLevel2_Id, @ParReason_Id, @ParReasonType_Id, GETDATE())";
            }

            string conexao = this.conexao;

            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {

                        command.CommandType = CommandType.Text;
                        command.Parameters.Add(new SqlParameter("@ParReason_Id", ParReason_Id));
                        command.Parameters.Add(new SqlParameter("@ParReasonType_Id", ParReasonType_Id));
                        command.Parameters.Add(new SqlParameter("@CollectionLevel2_Id", CollectionLevel2_Id));

                        connection.Open();
                        Convert.ToInt32(command.ExecuteScalar());
                    }
                }
            }
            catch (Exception)
            {
                throw;
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
        protected void InsertCollectionLevel3(string CollectionLevel02Id, int level02, string level03Results, int auditorId, string duplicated, bool filho)
        {
            ///coloquei uma @ para replace, mas podemos utilizar o padrão de ; ou <> desde que todos os campos venha do script com escape()
            //string obj, string collectionDate, string level01id, string unit, string period, string shift, string device, string version

            //if (ParLevel1_Id != null)
            //{
            //    /*
            //     * MOCK Gabriel para indicador filho
            //     * 30/03/2017
            //     */

            //    var ParLevel3DB_IndicadorFilho = new SGQDBContext.ParLevel3(db);
            //    parLevel3List_IndicadorFilho = ParLevel3DB_IndicadorFilho.getListPerLevel1Id(ParLevel1_Id.GetValueOrDefault());

            //}

            //Prepara a string para ser convertida em Array
            level03Results = level03Results.Replace("sebo%20%20%2C%20suporte", "sebo%20%20e%20suporte");
            level03Results = level03Results.Replace("</level03><level03>", "@").Replace("<level03>", "").Replace("</level03>", "");
            //Gera o Array
            string[] arrayResults = level03Results.Split('@');
            //"trocar o virgula do value text";

            StringBuilder sql = new StringBuilder();

            //Lista de Level3

            var ParLevel3DB = new SGQDBContext.ParLevel3(db, quebraProcesso);
            var Result_Level3DB = new SGQDBContext.Result_Level3(db);
            var parLevel3List = ParLevel3DB.getList();

            //Percorre o Array para gerar os inserts
            for (int i = 0; i < arrayResults.Length; i++)
            {
                /*ACERTO PARA JUARA, BIANCA 2017-06-30 QUE MERDA*/
                arrayResults[i] = arrayResults[i].Replace("sebo  , suporte", "sebo e suporte");
                arrayResults[i] = arrayResults[i].Replace("sebo%20%20%2C%20suporte", "sebo%20%20e%20suporte");

                //Gera o array com o resultado
                var result = arrayResults[i].Split(',');

                //Instancia as variáveis para preencher o script
                string Level03Id = result[0];

                Dominio.ParLevel3Level2 tarefaFilha = new Dominio.ParLevel3Level2();
                Dominio.ParLevel3Level2Level1 indicadorFilha = new Dominio.ParLevel3Level2Level1();
                Dominio.CollectionLevel2 collectionLevel2Filha = new Dominio.CollectionLevel2();

                int collectionLevel2_id = Int32.Parse(CollectionLevel02Id);

                if (filho)
                {
                    using (var db = new Dominio.SgqDbDevEntities())
                    {
                        int idl3 = Int32.Parse(Level03Id);

                        collectionLevel2Filha = db.CollectionLevel2.FirstOrDefault(r => r.Id == collectionLevel2_id);


                        var ListaindicadorFilha = db.ParLevel3Level2Level1.Where(r => r.ParLevel1_Id == collectionLevel2Filha.ParLevel1_Id);


                        tarefaFilha = db.ParLevel3Level2.FirstOrDefault(r => r.ParLevel3_Id == idl3 && r.IsActive && ListaindicadorFilha.Any(z => z.ParLevel3Level2_Id == r.Id)); //&& r.ParLevel2_Id == level02
                    }
                }


                bool skip = false;

                //if (TarefasIndicadorFilho.GetValueOrDefault())
                //{
                //    skip = true;

                //    foreach (var l3_filho in parLevel3List_IndicadorFilho)
                //    {
                //        if (l3_filho.Id.ToString() == Level03Id)
                //        {
                //            skip = false;
                //        }
                //    }
                //}
                //else
                //{
                //foreach (var l3_filho in parLevel3List_IndicadorFilho)
                //{
                //    if (l3_filho.Id.ToString() == Level03Id)
                //    {
                //        skip = true;
                //    }
                //}
                //}

                if (skip)
                {
                    continue;
                }

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
                if (filho)
                    weight = tarefaFilha.Weight.ToString().Replace(",", ".");

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

                string defects = result[14] == "NaN" ? "0" : result[14];

                //aqui tem que mudar no bem estar animal, verificar com o gabriel
                string evaluation = "1";

                string WeiEvaluation = "1";
                if (result.Count() > 15)
                    WeiEvaluation = result[15].Replace(",", ".");

                if (filho)
                    WeiEvaluation = tarefaFilha.Weight.ToString().Replace(",", ".");

                string WeiDefects = "0";
                if (result.Count() > 16)
                    WeiDefects = result[16].Replace(",", ".");

                string hasPhoto = BoolConverter(result.Count() > 17 ? result[17] : "false");

                if (filho)
                    WeiDefects = (tarefaFilha.Weight * Decimal.Parse(defects)).ToString().Replace(",", ".");

                //decimal defeitos = Convert.ToDecimal(defects.ToString().Replace(".", ","));
                //decimal punicao = Convert.ToDecimal(punishimentValue.ToString().Replace(".", ","));
                //decimal peso = Convert.ToDecimal(weight.ToString().Replace(".", ","));

                //WeiDefects = (defeitos + punicao) * peso;

                id = DefaultValueReturn(id, "0");

                if (Int64.Parse(id) == 0)
                {
                    var r = Result_Level3DB.get(Int32.Parse(CollectionLevel02Id), Int32.Parse(Level03Id));
                    if (r != null)
                    {
                        id = r.Id.ToString();
                    }
                }

                //Verifica se é BEA e faz a conta do WeiEvaluation
                var _WeiEvaluation = GetWeiEvaluation(WeiEvaluation, CollectionLevel02Id);
                var query = "";

                if (id == "0")
                {
                    var parLevel3_Name = parLevel3List.FirstOrDefault(p => p.Id == Convert.ToInt32(Level03Id)) != null ?
                        parLevel3List.FirstOrDefault(p => p.Id == Convert.ToInt32(Level03Id)).Name.Replace("'", "''") : "";

                    //sql = "INSERT INTO Result_Level3 ([CollectionLevel2_Id]," +
                    //    "[ParLevel3_Id]," +
                    //    "[ParLevel3_Name]," +
                    //    "[Weight]," +
                    //    "[IntervalMin]," +
                    //    "[IntervalMax]," +
                    //    "[Value]," +
                    //    "[ValueText]," +
                    //    "[IsConform]," +
                    //    "[IsNotEvaluate]," +
                    //    "[PunishmentValue]," +
                    //    "[Defects]," +
                    //    "[Evaluation]," +
                    //    "[WeiEvaluation]," +
                    //    "[WeiDefects]) " +
                    //       "VALUES " +
                    //       "('" + CollectionLevel02Id + "'," +
                    //       "'" + Level03Id + "', " +
                    //       "'" + parLevel3_Name + "'," +
                    //       "" + weight + "," +
                    //       "" + intervalMin + "," +
                    //       "" + intervalMax + "," +
                    //       "'" + value + "'," +
                    //       "'" + valueText + "'," +
                    //       "'" + conform + "'," +
                    //       "'" + isnotEvaluate + "'," +
                    //       "" + punishimentValue + "," +
                    //       "" + defects + "," +
                    //       "" + evaluation + "," +
                    //       "" + _WeiEvaluation + "," +
                    //       "" + WeiDefects + ") " +
                    //       " SELECT @@IDENTITY AS 'Identity'";

                    query = $@"

                        DECLARE @GRUPO INT
                        DECLARE @L1 INT 
                        DECLARE @L2 INT 
                        DECLARE @UNIT INT

                        select top 1 @L1 = parlevel1_id, @L2 = parlevel2_id, @UNIT = unitid from collectionlevel2 where id = @CollectionLevel02Id

                        select top 1 @GRUPO = ParLevel3Group_Id from parlevel3level2 p32
                        inner join parlevel3level2level1 p321
                        on p32.id = p321.parlevel3level2_id
                        where parlevel3_id = @Level03Id 
                          and parlevel1_id = @L1
                          and parlevel2_id = @L2
                          and ( p32.ParCompany_Id = @UNIT or  p32.ParCompany_Id is null)
                          and (p321.ParCompany_Id = @UNIT or p321.ParCompany_Id is null)
                        order by p32.ParCompany_Id, p321.ParCompany_Id
                        
                        INSERT INTO Result_Level3 ([CollectionLevel2_Id],
                         [ParLevel3_Id],
                         [ParLevel3_Name],
                         [Weight],
                         [IntervalMin],
                         [IntervalMax],
                         [Value],
                         [ValueText],
                         [IsConform],
                         [IsNotEvaluate],
                         [PunishmentValue],
                         [Defects],
                         [Evaluation],
                         [WeiEvaluation],
                         [WeiDefects]) 
                           VALUES 
                           (@CollectionLevel02Id,
                           @Level03Id,
                           @ParLevel3_Name,
                           @Weight,
                           @IntervalMin,
                           @IntervalMax,
                           @Value,
                           @ValueText,
                           @Conform,
                           @IsnotEvaluate,
                           @PunishimentValue,
                           @Defects,
                           @Evaluation,
                           @_WeiEvaluation,
                           @WeiDefects)

                           SELECT @@IDENTITY AS 'Identity'
                           
                           if @GRUPO > 0 begin INSERT INTO Result_Level3XGroup VALUES (@@IDENTITY, @GRUPO, GETDATE(), NULL, 1) end
                           ";

                }
                else
                {
                    query = $@"UPDATE Result_Level3 SET
                            IsConform=@Conform,
                            IsNotEvaluate=@IsnotEvaluate,
                            Value=@Value,
                            Weight=@Weight,
                            Defects=@Defects,
                            WeiEvaluation=@_WeiEvaluation,
                            WeiDefects=@WeiDefects,
                            ValueText=@ValueText
                            WHERE Id=@Id
                            SELECT @Id AS 'Identity' ";
                }


                string conexao = this.conexao;

                try
                {
                    using (SqlConnection connection = new SqlConnection(conexao))
                    {
                        using (SqlCommand cmd = new SqlCommand(query, connection))
                        {

                            if (id == "0")
                            {
                                var parLevel3_Name = parLevel3List.FirstOrDefault(p => p.Id == Convert.ToInt32(Level03Id)) != null ?
                                    parLevel3List.FirstOrDefault(p => p.Id == Convert.ToInt32(Level03Id)).Name.Replace("'", "''") : "";

                                cmd.CommandType = CommandType.Text;
                                cmd.Parameters.Add(new SqlParameter("@CollectionLevel02Id", CollectionLevel02Id));
                                cmd.Parameters.Add(new SqlParameter("@Level03Id", Level03Id));
                                cmd.Parameters.Add(new SqlParameter("@ParLevel3_Name", parLevel3_Name));
                                cmd.Parameters.Add(new SqlParameter("@Weight", weight));
                                cmd.Parameters.Add(new SqlParameter("@IntervalMin", intervalMin));
                                cmd.Parameters.Add(new SqlParameter("@IntervalMax", intervalMax));
                                cmd.Parameters.Add(new SqlParameter("@Value", value));
                                cmd.Parameters.Add(new SqlParameter("@ValueText", valueText));
                                cmd.Parameters.Add(new SqlParameter("@Conform", conform));
                                cmd.Parameters.Add(new SqlParameter("@IsnotEvaluate", isnotEvaluate));
                                cmd.Parameters.Add(new SqlParameter("@PunishimentValue", punishimentValue));
                                cmd.Parameters.Add(new SqlParameter("@Defects", defects));
                                cmd.Parameters.Add(new SqlParameter("@Evaluation", evaluation));
                                cmd.Parameters.Add(new SqlParameter("@_WeiEvaluation", _WeiEvaluation));
                                cmd.Parameters.Add(new SqlParameter("@WeiDefects", WeiDefects));

                            }
                            else
                            {

                                cmd.CommandType = CommandType.Text;
                                cmd.Parameters.Add(new SqlParameter("@Conform", conform));
                                cmd.Parameters.Add(new SqlParameter("@IsnotEvaluate", isnotEvaluate));
                                cmd.Parameters.Add(new SqlParameter("@Value", value));
                                cmd.Parameters.Add(new SqlParameter("@Weight", weight));
                                cmd.Parameters.Add(new SqlParameter("@Defects", defects));
                                cmd.Parameters.Add(new SqlParameter("@_WeiEvaluation", _WeiEvaluation));
                                cmd.Parameters.Add(new SqlParameter("@WeiDefects", WeiDefects));
                                cmd.Parameters.Add(new SqlParameter("@ValueText", valueText));
                                cmd.Parameters.Add(new SqlParameter("@Id", id));
                            }

                            connection.Open();

                            cmd.ExecuteScalar();

                        }
                    }
                }
                //Caso ocorra Exception, insere no banco e retorna zero
                catch (SqlException ex)
                {
                    int insertLog = insertLogJson(sql.ToString(), ex.Message, "N/A", "N/A", "InsertCollectionLevel03");
                }
                catch (Exception ex)
                {
                    int insertLog = insertLogJson(sql.ToString(), ex.Message, "N/A", "N/A", "InsertCollectionLevel03");
                }

            }

        }
        #endregion

        #region WeiEvaluation
        private string GetWeiEvaluation(string WeiEvaluation, string CollectionLevel2Id)
        {
            string _WeiEvaluation2 = WeiEvaluation;

            int CollectionLevel2Id_int = Int32.Parse(CollectionLevel2Id);

            decimal WeiEvaluation_decimal = decimal.Parse(WeiEvaluation);

            using (var databaseSgq = new Dominio.SgqDbDevEntities())
            {

                var collectionLevel2_obj = databaseSgq.CollectionLevel2.FirstOrDefault(r => r.Id == CollectionLevel2Id_int);

                var parLeve1BEA = databaseSgq.ParLevel1VariableProductionXLevel1.FirstOrDefault(r => r.ParLevel1_Id == collectionLevel2_obj.ParLevel1_Id);

                //Se for BEA
                if (parLeve1BEA != null)
                    if (parLeve1BEA.ParLevel1VariableProduction_Id == 3)
                    {
                        var collectionLevel2_obj2 = databaseSgq.CollectionLevel2.Where(
                        r => System.Data.Entity.DbFunctions.TruncateTime(r.CollectionDate) == System.Data.Entity.DbFunctions.TruncateTime(collectionLevel2_obj.CollectionDate) &&
                        r.ParLevel1_Id == collectionLevel2_obj.ParLevel1_Id &&
                        r.Shift == collectionLevel2_obj.Shift &&
                        r.Period == collectionLevel2_obj.Period &&
                        r.UnitId == collectionLevel2_obj.UnitId &&
                        r.Sample < collectionLevel2_obj.Sample
                        ).OrderByDescending(r => r.Sample).FirstOrDefault();


                        if (collectionLevel2_obj2 != null)

                            _WeiEvaluation2 = (collectionLevel2_obj.Sample - collectionLevel2_obj2.Sample).ToString();

                        else
                            _WeiEvaluation2 = (collectionLevel2_obj.Sample).ToString();

                    }

                return _WeiEvaluation2;
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
        protected int correctiveActionInsert(string AuditorId, string CollectionLevel02Id, string SlaughterId, string TechinicalId, string DateTimeSlaughter, string DateTimeTechinical, string DateCorrectiveAction, string AuditStartTime, string DescriptionFailure, string ImmediateCorrectiveAction, string ProductDisposition, string PreventativeMeasure)
        {
            //Conversão das datas
            DateTime SlaughterDateTime = DateCollectConvert(DateTimeSlaughter);
            DateTimeSlaughter = SlaughterDateTime.ToString("yyyy-MM-dd HH:mm:ss");

            DateTime TechinicalDateTime = DateCollectConvert(DateTimeTechinical);
            DateTimeTechinical = TechinicalDateTime.ToString("yyyy-MM-dd HH:mm:ss");

            DateTime CorrectiveActionDate = DateCollectConvert(DateCorrectiveAction);
            DateCorrectiveAction = CorrectiveActionDate.ToString("yyyy-MM-dd HH:mm:ss");

            DateTime StartTimeAudit = DateCollectConvert(AuditStartTime);
            AuditStartTime = StartTimeAudit.ToString("yyyy-MM-dd HH:mm:ss");

            //Script de Insert
            //string sql = "INSERT INTO CorrectiveAction ([AuditorId]," +
            //    "[CollectionLevel02Id]," +
            //    "[SlaughterId]," +
            //    "[TechinicalId]," +
            //    "[DateTimeSlaughter]," +
            //    "[DateTimeTechinical]," +
            //    "[AddDate]," +
            //    "[AlterDate]," +
            //    "[DateCorrectiveAction]," +
            //    "[AuditStartTime]," +
            //    "[DescriptionFailure]," +
            //    "[ImmediateCorrectiveAction]," +
            //    "[ProductDisposition]," +
            //    "[PreventativeMeasure]) " +
            //    "VALUES " +
            //    "('" + AuditorId + "'," +
            //    "'" + CollectionLevel02Id + "'," +
            //    "'" + SlaughterId + "'," +
            //    "'" + TechinicalId + "'," +
            //    "CAST(N'" + DateTimeSlaughter + "' AS DateTime)," +
            //    "CAST(N'" + DateTimeTechinical + "' AS DateTime)," +
            //    "GETDATE()," +
            //    "NULL," +
            //    "CAST(N'" + DateCorrectiveAction + "' AS DateTime)," +
            //    "CAST(N'" + AuditStartTime + "' AS DateTime)," +
            //    "'" + DescriptionFailure.Replace("'", "''") + "'," +
            //    "'" + ImmediateCorrectiveAction.Replace("'", "''") + "'," +
            //    "'" + ProductDisposition.Replace("'", "''") + "'," +
            //    "'" + PreventativeMeasure.Replace("'", "''") + "')";

            string sql = $@"INSERT INTO CorrectiveAction ([AuditorId],
                [CollectionLevel02Id],
                [SlaughterId],
                [TechinicalId],
                [DateTimeSlaughter],
                [DateTimeTechinical],
                [AddDate],
                [AlterDate],
                [DateCorrectiveAction],
                [AuditStartTime],
                [DescriptionFailure],
                [ImmediateCorrectiveAction],
                [ProductDisposition],
                [PreventativeMeasure])
                VALUES 
                (@AuditorId,
                @CollectionLevel02Id,
                @SlaughterId,
                @TechinicalId ,
                CAST(@DateTimeSlaughter AS DateTime),
                CAST(@DateTimeTechinical AS DateTime),
                GETDATE(),
                NULL,
                CAST(@DateCorrectiveAction AS DateTime),
                CAST(@AuditStartTime AS DateTime),
                @DescriptionFailure,
                @ImmediateCorrectiveAction,
                @ProductDisposition,
                @PreventativeMeasure)";

            string conexao = this.conexao;

            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        DescriptionFailure = DescriptionFailure == null ? DescriptionFailure = "" : DescriptionFailure;
                        ImmediateCorrectiveAction = ImmediateCorrectiveAction == null ? ImmediateCorrectiveAction = "" : ImmediateCorrectiveAction;
                        ProductDisposition = ProductDisposition == null ? ProductDisposition = "" : ProductDisposition;
                        PreventativeMeasure = PreventativeMeasure == null ? PreventativeMeasure = "" : PreventativeMeasure;

                        command.CommandType = CommandType.Text;
                        command.Parameters.Add(new SqlParameter("@AuditorId", AuditorId));
                        command.Parameters.Add(new SqlParameter("@CollectionLevel02Id", CollectionLevel02Id));
                        command.Parameters.Add(new SqlParameter("@SlaughterId", SlaughterId));
                        command.Parameters.Add(new SqlParameter("@TechinicalId", TechinicalId));
                        command.Parameters.Add(new SqlParameter("@DateTimeSlaughter", DateTimeSlaughter));
                        command.Parameters.Add(new SqlParameter("@DateTimeTechinical", DateTimeTechinical));
                        command.Parameters.Add(new SqlParameter("@DateCorrectiveAction", DateCorrectiveAction));
                        command.Parameters.Add(new SqlParameter("@AuditStartTime", AuditStartTime));
                        command.Parameters.Add(new SqlParameter("@DescriptionFailure", DescriptionFailure.Replace("'", "''")));
                        command.Parameters.Add(new SqlParameter("@ImmediateCorrectiveAction", ImmediateCorrectiveAction.Replace("'", "''")));
                        command.Parameters.Add(new SqlParameter("@ProductDisposition", ProductDisposition.Replace("'", "''")));
                        command.Parameters.Add(new SqlParameter("@PreventativeMeasure", PreventativeMeasure.Replace("'", "''")));

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
        /// Metodo que para chamar o recebimento de dados
        /// </summary>
        /// <param name="unidadeId"></param>
        /// <returns></returns>
        /// PORQUE QUE ESSA PORRA DESTA DATA É MESDIAANO?????????????????? (Comentário Gabriel)

        public string reciveData(string unidadeId, string data)
        {
            DateTime dataConsolidation = DateCollectConvert(data);
            string consolidation = getConsolidation(unidadeId, dataConsolidation, 0);
            return consolidation;
        }

        public string reciveDataByLevel1(string ParCompany_Id, string data, string ParLevel1_Id)
        {
            DateTime dataConsolidation = DateCollectConvert(data);
            string consolidation = getConsolidation(ParCompany_Id, dataConsolidation, Convert.ToInt32(ParLevel1_Id));
            return consolidation;
        }

        public static void getFrequencyDate(int ParFrequency_Id, DateTime data, ref string dataInicio, ref string dataFim)
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

                    /*
                     * DateTime.Now????? PUTA QUE PARIU!!!! TEM A PORRA DA DATA E O CARA COMPARA COM NOW????????????
                     * Estava assim: DateTime.Now.Day > 15
                     * Corrigido dia 30/03/2017
                     */
                    if (data.Day > 15)
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

        protected string getConsolidation(string ParCompany_Id, DateTime data, int ParLevel1_Id)
        {

            string dataIni = data.ToString("yyyyMMdd");

            StringBuilder retorno = new StringBuilder();

            using (Factory factory = new Factory("DefaultConnection"))
            {

                string sql = $@"

                declare @data date = @DataIni
                declare @unidade int = @ParCompany_Id
                declare @datainicio date
                declare @datafim date
                declare @datadiario date
                declare @datasemanal date
                declare @dataquinzenal date
                declare @datamensal date
                
                SET @datainicio = @data
                SET @datafim = @data
                SET @datadiario = @data  --1,2,3
                SET @datasemanal = DATEADD(DAY, -(DATEPART(WEEKDAY, @data) - 1), @data)
                SET @dataquinzenal =
                CASE
                	WHEN DAY(@data) < 16 THEN DATEADD(MONTH, 1, DATEADD(mm, DATEDIFF(mm, 0, @data) - 1, 0))
                	ELSE DATEADD(DAY, 15, DATEADD(MONTH, 1, DATEADD(mm, DATEDIFF(mm, 0, @data) - 1, 0)))
                END
                SET @datamensal = DATEADD(MONTH, 1, DATEADD(mm, DATEDIFF(mm, 0, @data) - 1, 0))
                SET @datainicio = @data
                SET @datafim = @data
                
				
				SELECT 
					row_number() over(partition by ParLevel1_id,ParLevel2_id,ParCluster_id order by ParCompany_id desc) as [rank]
					,ParCompany_Id
					,ParLevel2_Id
					,Number
					,ParLevel1_Id
					,ParCluster_Id
					,Sample
					,ParFrequency_Id
					INTO #PAREVALUATION
					FROM PAREVALUATION  
				WHERE 1=1
				AND Isactive = 1
				AND (ParCompany_id is null or ParCompany_id = @unidade)




                CREATE TABLE #COLETASLEVEL3 (																																											  
                	ROW INT NULL,																																															  
                	COLUNA VARCHAR(8000) NULL																																												  
                )
                
                INSERT INTO #COLETASLEVEL3
                	SELECT
                		ROW_NUMBER() OVER (ORDER BY R3.ParLevel3_Id) AS ROW
                	   ,'<div id=""' + CAST(R3.ParLevel3_Id AS VARCHAR) + '"" class=""r3l2""></div>' COLUNA
                	FROM CollectionLevel2 C2 (NOLOCK)
					LEFT JOIN CollectionLevel2XCluster C2XCL (NOLOCK)
						ON C2.ID = C2XCL.CollectionLevel2_Id
                	INNER JOIN ParLevel1 L1 (NOLOCK)
                		ON C2.ParLevel1_Id = L1.Id
                			AND L1.IsPartialSave = 1
                	INNER JOIN ParLevel2 L2 (NOLOCK)
                		ON C2.ParLevel2_Id = L2.Id

					LEFT JOIN ( 
						SELECT * FROM #PAREVALUATION 
					WHERE 1=1
					AND [rank] = 1
					) FREQ_AV 
					ON FREQ_AV.PARLEVEL2_ID = C2.PARLEVEL2_ID AND FREQ_AV.PARLEVEL1_ID = C2.PARLEVEL1_ID AND FREQ_AV.ParCluster_Id = C2XCL.ParCluster_Id
                	INNER JOIN Result_Level3 R3 (NOLOCK)
                		ON R3.CollectionLevel2_Id = C2.Id
                	WHERE 1=1
					AND C2.UnitId = @unidade
                	AND CAST(C2.CollectionDate AS DATE) BETWEEN
                	CASE
                		WHEN (FREQ_AV.ParFrequency_Id) IN (1, 2, 3, 10) THEN @datadiario
                		WHEN (FREQ_AV.ParFrequency_Id) IN (4) THEN @datasemanal
                		WHEN (FREQ_AV.ParFrequency_Id) IN (5) THEN @dataquinzenal
                		WHEN (FREQ_AV.ParFrequency_Id) IN (6) THEN @datamensal
                		ELSE @datadiario
                	END AND @datafim

					--AND C2.ParLevel1_Id =112 
					--AND C2.ParLevel2_Id = 633
	
                DECLARE @HOMENSFORBRUNO INT = ( SELECT
                		COUNT(1)
                	FROM #COLETASLEVEL3);
                DECLARE @I INT = 1;
                DECLARE @RESPOSTA VARCHAR(8000) = '';
                WHILE @I<@HOMENSFORBRUNO
                BEGIN
                SELECT
                	@RESPOSTA = @RESPOSTA + COLUNA
                FROM (SELECT
                		*
                	FROM #COLETASLEVEL3) consulta
                WHERE ROW = @I
                SET @I = @I + 1;
                END

                SELECT 
					 ConsolidationLevel2_id,
					 CollectionDate,
					 EvaluationNumber,
					 [Sample]
				INTO #CollectionLevel2_HPA -- Hora Primeira Avaliação
				FROM CollectionLevel2 WITH (NOLOCK)
				WHERE 1=1
				AND Unitid = @Unidade
				AND CollectionDate BETWEEN @data AND concat(@data,' ','23:59:59') 
				AND ParLevel2_id != 0

				CREATE INDEX IDX_CollectionLevel2_HPA_ConsolidationLevel2_id ON #CollectionLevel2_HPA(ConsolidationLevel2_id,EvaluationNumber,[Sample])
                
                CREATE TABLE #COLETA(																																													  
                ParLevel1_Id varchar(255) null,																																												  
                ParLevel2_Id varchar(255) null,																																												  
                UnitId int null,																																													  
                Shift int null,																																														  
                Period int null,																																													  
                CollectionDate Date null,																																											  
                EvaluateLast int null,																																												  
                ConsolidationLevel2_Id int null,																																									  
                SampleLast int null,
                Phase int null,																																														  
                StartPhaseEvaluation int null,																																										  
                haveCorrectiveAction int null,																																										  
                haveReaudit int null,																																												  
                ReauditLevel int null,																																												  
                Sequential int null,																																												  
                Side int null,				  																																								  
                Id int null,
                HoraPrimeiraAvaliacao varchar(20)
                )
                
                /*coletas diárias */
                INSERT INTO #COLETA
                	SELECT
                		CAST(ISNULL(MAX(CL2C.ParCluster_Id), 0) AS VARCHAR) + '98789' + CAST(CL2.ParLevel1_Id AS VARCHAR) AS ParLevel1_Id, --indicador
                		CAST(ISNULL(MAX(CL2C.ParCluster_Id), 0) AS VARCHAR) + '98789' + CAST(CL2.ParLevel2_Id AS VARCHAR) AS ParLevel2_Id, --monitoramento
                		UnitId AS Unit_Id,--unidade
                		Shift, --shift
                		Period,--periodo
                		CAST(CollectionDate AS DATE) CollectionDate, --data da coleta
                		MAX(EvaluationNumber) AS EvaluateLast,--maior avaliacao
                		ConsolidationLevel2_Id,  --id da consolidaçao level2
                		(SELECT
                				MAX(sample)
                			FROM CollectionLevel2 WITH (NOLOCK)
                			WHERE ConsolidationLevel2_id = cl2.ConsolidationLevel2_Id
                			AND EvaluationNumber = MAX(cl2.EvaluationNumber))
                		AS SampleLast
                	   ,MAX(Phase) AS Phase
                	   ,MAX(StartPhaseEvaluation) AS StartPhaseEvaluation
                	   ,MAX(CAST(haveCorrectiveAction AS INT)) haveCorrectiveAction
                	   ,MAX(CAST(haveReaudit AS INT)) haveReaudit
                	   ,MAX(ReauditLevel) ReauditLevel
                	   ,MAX(Sequential) Sequential
                	   ,MAX(Side) Side
                	   ,MIN(CL2.Id) AS ID
                	   ,(SELECT
                				MIN(CAST(CollectionDate AS TIME))
                			FROM #CollectionLevel2_HPA WITH (NOLOCK)
                			WHERE ConsolidationLevel2_id = cl2.ConsolidationLevel2_Id
                			AND EvaluationNumber = 1
                			AND [Sample] = 1)
                		AS HoraPrimeiraAvaliacao
                	FROM CollectionLevel2 CL2 WITH (NOLOCK)
                	LEFT JOIN CollectionLevel2XCluster CL2C
                		ON CL2C.CollectionLevel2_Id = CL2.Id
                	INNER JOIN parlevel2 p2 WITH (NOLOCK)
                		ON p2.id = CL2.ParLevel2_Id
					LEFT JOIN ( 
						SELECT * FROM #PAREVALUATION 
					WHERE 1=1
					AND [rank] = 1
					) FREQ_AV 
					ON FREQ_AV.PARLEVEL2_ID = CL2.PARLEVEL2_ID AND FREQ_AV.PARLEVEL1_ID = CL2.PARLEVEL1_ID AND FREQ_AV.ParCluster_Id = CL2C.ParCluster_Id
                	WHERE 1=1
					AND unitid = @unidade
                	AND FREQ_AV.ParFrequency_Id IN (1, 2, 3, 10)
                	AND CAST(CollectionDate AS DATE) BETWEEN @datadiario AND @data
                	GROUP BY CL2.ParLevel1_Id
                			,CL2.ParLevel2_Id
                			,UnitId
                			,Shift
                			,Period
                			,CAST(CollectionDate AS DATE)
                			,ConsolidationLevel2_Id
                
                /*coletas semanal */
                INSERT INTO #COLETA
                	SELECT
                		CAST(ISNULL(MAX(CL2C.ParCluster_Id), 0) AS VARCHAR) + '98789' + CAST(CL2.ParLevel1_Id AS VARCHAR) AS ParLevel1_Id, --indicador
                		CAST(ISNULL(MAX(CL2C.ParCluster_Id), 0) AS VARCHAR) + '98789' + CAST(CL2.ParLevel2_Id AS VARCHAR) AS ParLevel2_Id, --monitoramento
                		UnitId AS Unit_Id,--unidade
                		Shift, --shift
                		Period,--periodo
                		CAST(CollectionDate AS DATE) CollectionDate, --data da coleta
                		MAX(EvaluationNumber) AS EvaluateLast,--maior avaliacao
                		ConsolidationLevel2_Id,  --id da consolidaçao level2
                		(SELECT
                				MAX(sample)
                			FROM CollectionLevel2 WITH (NOLOCK)
                			WHERE ConsolidationLevel2_id = cl2.ConsolidationLevel2_Id
                			AND EvaluationNumber = MAX(cl2.EvaluationNumber))
                		AS SampleLast
                	   ,MAX(Phase) AS Phase
                	   ,MAX(StartPhaseEvaluation) AS StartPhaseEvaluation
                	   ,MAX(CAST(haveCorrectiveAction AS INT)) haveCorrectiveAction
                	   ,MAX(CAST(haveReaudit AS INT)) haveReaudit
                	   ,MAX(ReauditLevel) ReauditLevel
                	   ,MAX(Sequential) Sequential
                	   ,MAX(Side) Side
                	   ,MIN(CL2.Id) AS ID
                	   ,(SELECT
                				MIN(CAST(CollectionDate AS TIME))
                			FROM #CollectionLevel2_HPA WITH (NOLOCK)
                			WHERE ConsolidationLevel2_id = cl2.ConsolidationLevel2_Id
                			AND EvaluationNumber = 1
                			AND [Sample] = 1)
                		AS HoraPrimeiraAvaliacao
                	FROM CollectionLevel2 CL2 WITH (NOLOCK)
                	LEFT JOIN CollectionLevel2XCluster CL2C
                		ON CL2C.CollectionLevel2_Id = CL2.Id
                	INNER JOIN parlevel2 p2 WITH (NOLOCK)
                		ON p2.id = CL2.ParLevel2_Id
					LEFT JOIN ( 
						SELECT * FROM #PAREVALUATION 
					WHERE 1=1
					AND [rank] = 1
					) FREQ_AV 
					ON FREQ_AV.PARLEVEL2_ID = CL2.PARLEVEL2_ID AND FREQ_AV.PARLEVEL1_ID = CL2.PARLEVEL1_ID AND FREQ_AV.ParCluster_Id = CL2C.ParCluster_Id
                	WHERE unitid = @unidade
                	AND FREQ_AV.ParFrequency_Id IN (4)
                	AND CAST(CollectionDate AS DATE) BETWEEN @datasemanal AND @data
                	GROUP BY CL2.ParLevel1_Id
                			,CL2.ParLevel2_Id
                			,UnitId
                			,Shift
                			,Period
                			,CAST(CollectionDate AS DATE)
                			,ConsolidationLevel2_Id
                
                /*coletas quinzenal */
                INSERT INTO #COLETA
                	SELECT
                		CAST(ISNULL(MAX(CL2C.ParCluster_Id), 0) AS VARCHAR) + '98789' + CAST(CL2.ParLevel1_Id AS VARCHAR) AS ParLevel1_Id, --indicador
                		CAST(ISNULL(MAX(CL2C.ParCluster_Id), 0) AS VARCHAR) + '98789' + CAST(CL2.ParLevel2_Id AS VARCHAR) AS ParLevel2_Id, --monitoramento
                		UnitId AS Unit_Id,--unidade
                		Shift, --shift
                		Period,--periodo
                		CAST(CollectionDate AS DATE) CollectionDate, --data da coleta
                		MAX(EvaluationNumber) AS EvaluateLast,--maior avaliacao
                		ConsolidationLevel2_Id,  --id da consolidaçao level2
                		(SELECT
                				MAX(sample)
                			FROM CollectionLevel2 WITH (NOLOCK)
                			WHERE ConsolidationLevel2_id = cl2.ConsolidationLevel2_Id
                			AND EvaluationNumber = MAX(cl2.EvaluationNumber))
                		AS SampleLast
                	   ,MAX(Phase) AS Phase
                	   ,MAX(StartPhaseEvaluation) AS StartPhaseEvaluation
                	   ,MAX(CAST(haveCorrectiveAction AS INT)) haveCorrectiveAction
                	   ,MAX(CAST(haveReaudit AS INT)) haveReaudit
                	   ,MAX(ReauditLevel) ReauditLevel
                	   ,MAX(Sequential) Sequential
                	   ,MAX(Side) Side
                	   ,MIN(CL2.Id) AS ID
                	   ,(SELECT
                				MIN(CAST(CollectionDate AS TIME))
                			FROM #CollectionLevel2_HPA WITH (NOLOCK)
                			WHERE ConsolidationLevel2_id = cl2.ConsolidationLevel2_Id
                			AND EvaluationNumber = 1
                			AND [Sample] = 1)
                		AS HoraPrimeiraAvaliacao
                	FROM CollectionLevel2 CL2 WITH (NOLOCK)
                	LEFT JOIN CollectionLevel2XCluster CL2C
                		ON CL2C.CollectionLevel2_Id = CL2.Id
                	INNER JOIN parlevel2 p2 WITH (NOLOCK)
                		ON p2.id = CL2.ParLevel2_Id
					LEFT JOIN ( 
						SELECT * FROM #PAREVALUATION 
					WHERE 1=1
					AND [rank] = 1
					) FREQ_AV 
					ON FREQ_AV.PARLEVEL2_ID = CL2.PARLEVEL2_ID AND FREQ_AV.PARLEVEL1_ID = CL2.PARLEVEL1_ID AND FREQ_AV.ParCluster_Id = CL2C.ParCluster_Id
                	WHERE unitid = @unidade
                	AND FREQ_AV.ParFrequency_Id IN (5)
                	AND CAST(CollectionDate AS DATE) BETWEEN @dataquinzenal AND @data
                	GROUP BY CL2.ParLevel1_Id
                			,CL2.ParLevel2_Id
                			,UnitId
                			,Shift
                			,Period
                			,CAST(CollectionDate AS DATE)
                			,ConsolidationLevel2_Id
                
                /*coletas mensal */
                INSERT INTO #COLETA
                	SELECT
                		CAST(ISNULL(MAX(CL2C.ParCluster_Id), 0) AS VARCHAR) + '98789' + CAST(CL2.ParLevel1_Id AS VARCHAR) AS ParLevel1_Id, --indicador
                		CAST(ISNULL(MAX(CL2C.ParCluster_Id), 0) AS VARCHAR) + '98789' + CAST(CL2.ParLevel2_Id AS VARCHAR) AS ParLevel2_Id, --monitoramento
                		UnitId AS Unit_Id,--unidade
                		Shift, --shift
                		Period,--periodo
                		CAST(CollectionDate AS DATE) CollectionDate, --data da coleta
                		MAX(EvaluationNumber) AS EvaluateLast,--maior avaliacao
                		ConsolidationLevel2_Id,  --id da consolidaçao level2
                		(SELECT
                				MAX(sample)
                			FROM CollectionLevel2 WITH (NOLOCK)
                			WHERE ConsolidationLevel2_id = cl2.ConsolidationLevel2_Id
                			AND EvaluationNumber = MAX(cl2.EvaluationNumber))
                		AS SampleLast
                	   ,MAX(Phase) AS Phase
                	   ,MAX(StartPhaseEvaluation) AS StartPhaseEvaluation
                	   ,MAX(CAST(haveCorrectiveAction AS INT)) haveCorrectiveAction
                	   ,MAX(CAST(haveReaudit AS INT)) haveReaudit
                	   ,MAX(ReauditLevel) ReauditLevel
                	   ,MAX(Sequential) Sequential
                	   ,MAX(Side) Side
                	   ,MIN(CL2.Id) AS ID
                	   ,(SELECT
                				MIN(CAST(CollectionDate AS TIME))
                			FROM #CollectionLevel2_HPA WITH (NOLOCK)
                			WHERE ConsolidationLevel2_id = cl2.ConsolidationLevel2_Id
                			AND EvaluationNumber = 1
                			AND [Sample] = 1)
                		AS HoraPrimeiraAvaliacao
                	FROM CollectionLevel2 CL2 WITH (NOLOCK)
                	LEFT JOIN CollectionLevel2XCluster CL2C
                		ON CL2C.CollectionLevel2_Id = CL2.Id
                	INNER JOIN parlevel2 p2 WITH (NOLOCK)
                		ON p2.id = CL2.ParLevel2_Id
					LEFT JOIN ( 
						SELECT * FROM #PAREVALUATION 
					WHERE 1=1
					AND [rank] = 1
					) FREQ_AV 
					ON FREQ_AV.PARLEVEL2_ID = CL2.PARLEVEL2_ID AND FREQ_AV.PARLEVEL1_ID = CL2.PARLEVEL1_ID AND FREQ_AV.ParCluster_Id = CL2C.ParCluster_Id
                	WHERE unitid = @unidade
                	AND FREQ_AV.ParFrequency_Id IN (6)
                	AND CAST(CollectionDate AS DATE) BETWEEN @datamensal AND @data
                	GROUP BY CL2.ParLevel1_Id
                			,CL2.ParLevel2_Id
                			,UnitId
                			,Shift
                			,Period
                			,CAST(CollectionDate AS DATE)
                			,ConsolidationLevel2_Id
                
                
                SELECT
                	'<div class=""Resultlevel2""																																												  
                	AlertLevelL1 = ""' + ISNULL(REPLACE(CAST(CDL1.AtualAlert AS VARCHAR), '.', ','), 'NULL') + '""
                	WeiEvaluationL1 = ""' + ISNULL(REPLACE(CAST(CDL1.WeiEvaluation AS VARCHAR), '.', ','), 'NULL') + '""
                	EvaluateTotalL1 = ""' + ISNULL(REPLACE(CAST(CDL1.EvaluateTotal AS VARCHAR), '.', ','), 'NULL') + '""
                	DefectsTotalL1 = ""' + ISNULL(REPLACE(CAST(CDL1.WeiDefects AS VARCHAR), '.', ','), 'NULL') + '""
                	WeiDefectsL1 = ""' + ISNULL(REPLACE(CAST(CDL1.WeiDefects AS VARCHAR), '.', ','), 'NULL') + '""
                	TotalLevel3EvaluationL1 = ""' + ISNULL(REPLACE(CAST(CDL1.TotalLevel3Evaluation AS VARCHAR), '.', ','), 'NULL') + '""
                	TotalLevel3WithDefectsL1 = ""' + ISNULL(REPLACE(CAST(CDL1.TotalLevel3WithDefects AS VARCHAR), '.', ','), 'NULL') + '""
                	LastEvaluationAlertL1 = ""' + ISNULL(REPLACE(CAST(CDL1.LastEvaluationAlert AS VARCHAR), '.', ','), 'NULL') + '""
                	LastLevel2AlertL1 = ""' + ISNULL(REPLACE(CAST(CDL1.LastLevel2Alert AS VARCHAR), '.', ','), 'NULL') + '""
                	EvaluatedResultL1 = ""' + ISNULL(REPLACE(CAST(CDL1.EvaluatedResult AS VARCHAR), '.', ','), 'NULL') + '""
                	DefectsResultL1 = ""' + ISNULL(REPLACE(CAST(CDL1.DefectsResult AS VARCHAR), '.', ','), 'NULL') + '""
                	EvaluateTotalL2 = ""' + ISNULL(REPLACE(CAST(CDL2.EvaluateTotal AS VARCHAR), '.', ','), 'NULL') + '""
                	DefectsTotalL2 = ""' + ISNULL(REPLACE(CAST(CDL2.DefectsTotal AS VARCHAR), '.', ','), 'NULL') + '""
                	WeiEvaluationL2 = ""' + ISNULL(REPLACE(CAST(CDL2.WeiEvaluation AS VARCHAR), '.', ','), 'NULL') + '""
                	DefectsL2 = ""' + ISNULL(REPLACE(CAST(CDL2.DefectsTotal AS VARCHAR), '.', ','), 'NULL') + '""
                	WeiDefectsL2 = ""' + ISNULL(REPLACE(CAST(CDL2.WeiDefects AS VARCHAR), '.', ','), 'NULL') + '""
                	TotalLevel3WithDefectsL2 = ""' + ISNULL(REPLACE(CAST(CDL2.TotalLevel3WithDefects AS VARCHAR), '.', ','), 'NULL') + '""
                	TotalLevel3EvaluationL2 = ""' + ISNULL(REPLACE(CAST(CDL2.TotalLevel3Evaluation AS VARCHAR), '.', ','), 'NULL') + '""
                	EvaluatedResultL2 = ""' + ISNULL(REPLACE(CAST(CDL2.EvaluateTotal AS VARCHAR), '.', ','), 'NULL') + '""
                	DefectsResultL2 = ""' + ISNULL(REPLACE(CAST(CDL2.DefectsResult AS VARCHAR), '.', ','), 'NULL') + '""
                	Level1Id = ""' + ISNULL(REPLACE(CAST(Level2Result.ParLevel1_Id AS VARCHAR), '.', ','), 'NULL') + '""
                	Level2Id = ""' + ISNULL(REPLACE(CAST(Level2Result.ParLevel2_Id AS VARCHAR), '.', ','), 'NULL') + '""
                	UnitId = ""' + ISNULL(REPLACE(CAST(Level2Result.UnitId AS VARCHAR), '.', ','), 'NULL') + '""
                	Shift = ""' + ISNULL(REPLACE(CAST(Level2Result.Shift AS VARCHAR), '.', ','), 'NULL') + '""
                	Period = ""' + ISNULL(REPLACE(CAST(Level2Result.Period AS VARCHAR), '.', ','), 'NULL') + '""
                	CollectionDate = ""' + ISNULL(FORMAT(Level2Result.CollectionDate, 'MMddyyyy'), 'NULL') + '""
                	Evaluation = ""' + ISNULL(REPLACE(CAST(Level2Result.EvaluateLast AS VARCHAR), '.', ','), 'NULL') + '""
                	Sample = ""' + ISNULL(REPLACE(CAST(Level2Result.SampleLast AS VARCHAR), '.', ','), 'NULL') + '""
                	Phase = ""' + ISNULL(REPLACE(CAST(MAX(Level2Result.Phase) AS VARCHAR), '.', ','), 'NULL') + '""
                	StartPhaseDate = ""' + ISNULL(REPLACE(CAST(MAX(Level2Result.StartPhaseEvaluation) AS VARCHAR), '.', ','), 'NULL') + '""
                	StartPhaseEvaluation = ""' + ISNULL(REPLACE(CAST(MAX(Level2Result.StartPhaseEvaluation) AS VARCHAR), '.', ','), 'NULL') + '""
                	havecorrectiveaction = ""' + ISNULL(REPLACE(CAST(MAX(CAST(Level2Result.haveCorrectiveAction AS INT)) AS VARCHAR), '1', 'true'), 'NULL') + '""
                	Sequential = ""' + ISNULL(REPLACE(CAST((Level2Result.Sequential) AS VARCHAR), '.', ','), 'NULL') + '""
                	Side = ""' + ISNULL(REPLACE(CAST((Level2Result.Side) AS VARCHAR), '.', ','), 'NULL') + '""
                	havereaudit = ""' + ISNULL(REPLACE(CAST(MAX(CAST(Level2Result.haveReaudit AS INT)) AS VARCHAR), '1', 'true'), 'NULL') + '""
                	reauditlevel = ""' + ISNULL(REPLACE(CAST(MAX(Level2Result.ReauditLevel) AS VARCHAR), '.', ','), 'NULL') + '""
                	reauditnumber = ""' + ISNULL(REPLACE(CAST(CDL2.ReauditNumber AS VARCHAR), '.', ','), 'NULL') + '""
                	isreaudit = ""' + ISNULL(REPLACE(CAST(CDL2.ReauditIs AS VARCHAR), '1', 'true'), 'NULL') + '""
                	more3defectsEvaluate = ""0""
                	CollectionLevel2_ID_CorrectiveAction = ""' + ISNULL(REPLACE(CAST(MIN(Level2Result.Id) AS VARCHAR), '.', ','), 'NULL') + '""
                	CollectionLevel2_Period_CorrectiveAction = ""' + ISNULL(REPLACE(CAST(MIN(Level2Result.Period) AS VARCHAR), '.', ','), 'NULL') + '"" 
                	HoraPrimeiraAvaliacao = ""' + ISNULL(Level2Result.HoraPrimeiraAvaliacao, 'NULL') + '"" >' as retornoLevel2,
                    @RESPOSTA as retornoLevel3,
                    '</div> ' AS retornoFechaDiv
                FROM #COLETA Level2Result
                INNER JOIN ConsolidationLevel2 CDL2 WITH (NOLOCK)
                	ON Level2Result.ConsolidationLevel2_Id = CDL2.Id
                INNER JOIN ConsolidationLevel1 CDL1 WITH (NOLOCK)
                	ON CDL2.ConsolidationLevel1_Id = CDL1.Id
                GROUP BY CDL1.WeiEvaluation
                		,CDL1.EvaluateTotal
                		,CDL1.WeiDefects
                		,CDL1.WeiDefects
                		,CDL1.TotalLevel3Evaluation
                		,CDL1.TotalLevel3WithDefects
                		,CDL1.LastEvaluationAlert
                		,CDL1.LastLevel2Alert
                		,CDL1.EvaluatedResult
                		,CDL1.DefectsResult
                		,CDL2.EvaluateTotal
                		,CDL2.DefectsTotal
                		,CDL2.WeiEvaluation
                		,CDL2.DefectsTotal
                		,CDL2.WeiDefects
                		,CDL2.TotalLevel3WithDefects
                		,CDL2.TotalLevel3Evaluation
                		,CDL2.EvaluateTotal
                		,CDL2.DefectsResult
                		,Level2Result.ParLevel1_Id
                		,Level2Result.ParLevel2_Id
                		,Level2Result.UnitId
                		,Level2Result.Shift
                		,Level2Result.Period
                		,Level2Result.CollectionDate
                		,Level2Result.EvaluateLast
                		,Level2Result.SampleLast
                		,Level2Result.Sequential
                		,Level2Result.Side
                		,Level2Result.HoraPrimeiraAvaliacao
                		,CDL2.ReauditNumber
                		,CDL2.ReauditIs
                		,CDL1.AtualAlert
                ORDER BY Level2Result.CollectionDate ASC, Level2Result.ParLevel1_Id ASC, CDL2.ReauditNumber ASC
                
                DROP TABLE #COLETASLEVEL3
                DROP TABLE #COLETA
				DROP TABLE #PAREVALUATION
				DROP TABLE #CollectionLevel2_HPA";


                using (SqlCommand cmd = new SqlCommand(sql, factory.connection))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add(new SqlParameter("@DataIni", dataIni));
                    cmd.Parameters.Add(new SqlParameter("@ParCompany_Id", ParCompany_Id));

                    var list = factory.SearchQuery<ResultadoH4B>(cmd).ToList();

                    for (var i = 0; i < list.Count(); i++)
                    {
                        retorno.Append(list[i].retornoLevel2.ToString());
                        retorno.Append(list[i].retornoLevel3.ToString());
                        retorno.Append(list[i].retornoFechaDiv.ToString());
                    }
                }
            }

            return retorno.ToString();
        }
        #endregion

        #region App
        public string getAPP(/*string version*/)
        {
            return getAPP2("");
        }

        public string getAPP2(string version)
        {
            //var version = "2.0.47";
            string forcaAtualizacao = "";

            string appVersion = DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.appVersion;

            if (!version.Contains(appVersion))
                forcaAtualizacao = @"<script>
                                    setTimeout(function(){
                                        navigator.notification.alert('Nova atualização disponivel. A aplicação será atualizada!', 
                                        Reload, 
                                        'Atualização', 
                                        'OK');
                                    },500);
                                    </script>";

            string login = GetLoginAPP();

            string resource = GetResource();

            return login + resource + forcaAtualizacao;
        }

        public string getAPPLevels(int UserSgq_Id, int ParCompany_Id, DateTime Date, int Shift_Id)
        {
            string APPMain = string.Empty;

            //colocar autenticação
            APPMain = getAPPMain(UserSgq_Id, ParCompany_Id, Date, null, Shift_Id); //  /**** COLOQUEI A UNIDADE PRA MONTAR O APP ****/


            string supports = "<div class=\"Results hide\"></div>" +
                              "<div class=\"ResultsConsolidation hide\"></div>" +
                               "<div class=\"ResultsKeys hide\"></div>" +
                               "<div class=\"ResultsPhase hide\"></div>" +
                               "<div class=\"ResultsDefectsEvaluation hide\"></div>" +
                              "<div class=\"Deviations hide\"></div>" +
                              "<div class=\"Users hide\"></div>" +
                              "<div class=\"VerificacaoTipificacao hide\"></div>" +
                              "<div class=\"VerificacaoTipificacaoResultados hide\"></div>";

            try
            {
                SGQDBContext.Generico listaProdutos = new Generico(db, conexao);
                var listaProdutosJSON = listaProdutos.getProdutos();

                supports += @" <script>
                                var listaProdutosJson = " + Newtonsoft.Json.JsonConvert.SerializeObject(listaProdutosJSON) + @";
                                           
                                function buscarProduto(a,valor){

                                    for (var j=0; j < listaProdutosJson.length;j++) {
                                        if (listaProdutosJson[j].id == valor) {

		                                    $(a).next().html(listaProdutosJson[j].nome);
                    
                                            return;
                                        }		                                               
                                    }
                                    //$(a).val('');
                                    $(a).next().html('');
                                }

                                function validaProduto(a,valor){
                                    for (var j=0; j < listaProdutosJson.length; j++) {
                                        if (listaProdutosJson[j].id == valor) {

		                                    //alert(listaProdutosJson[j].nome);
                    
                                            return;
                                        }
                                                                                                       
                                    }
                                    $(a).val('');
                                }
                                </script> ";
            }
            catch (Exception ex)
            {

            }


            try
            {

                using (var db = new SgqDbDevEntities())
                {

                    var listaDicionario = db.DicionarioEstatico.ToList();

                    supports += $@"<script>
                                var listaDicionarios = " + Newtonsoft.Json.JsonConvert.SerializeObject(listaDicionario) + @";
                                           
                                function getDicionario(key){
                                    var valor = $.grep(listaDicionarios, function(obj){ return obj.Key == key });
                                    return (valor && valor.length > 0) ? valor[0].Value : '';
                                }

                                </script> ";
                }
            }
            catch (Exception)
            {

            }



            //string resource = GetResource();


            return APPMain + supports;// + resource;
        }

        public string getAPPLevelsVolume(GetAPPLevelsVolumeClass getAPPLevelsVolumeClass)
        {
            string APPMain = string.Empty;

            APPMain = getAPPMain(getAPPLevelsVolumeClass.UserSgq_Id, getAPPLevelsVolumeClass.ParCompany_Id,
                getAPPLevelsVolumeClass.Date, getAPPLevelsVolumeClass.Level1ListId, getAPPLevelsVolumeClass.Shift_Id, true);

            return APPMain;// + resource;
        }

        public string GetResource()
        {
            if (GlobalConfig.LanguageBrasil)
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(Guard.LANGUAGE_PT_BR);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(Guard.LANGUAGE_PT_BR);
            }
            else
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("");
            }
            //setup temporário

            System.Reflection.Assembly assembly = this.GetType().Assembly;

            var resourceManager = (IDictionary<string, object>)Resources.Resource;

            string items = "";

            foreach (var entry in resourceManager)
            {
                items += "<div res='" + entry.Key.ToString() + "'>" + entry.Value.ToString() + "</div>";
            }

            return "<div class='Resource hide'>" + items + "</div>";
        }

        protected ParLevel2Evaluate getEvaluate(SGQDBContext.ParLevel2 parlevel2, IEnumerable<SGQDBContext.ParLevel2Evaluate> ParEvaluateCompany, IEnumerable<SGQDBContext.ParLevel2Evaluate> ParEvaluatePadrao)
        {

            var lista = parlevel2.Id.ToString().Replace(quebraProcesso, "|").Split('|');

            int parCluster_Id = lista.Length > 1 ? Int32.Parse(lista[0]) : 0;

            int parlevel2_id = lista.Length > 1 ? Int32.Parse(lista[1]) : Int32.Parse(lista[0]);

            ParLevel2Evaluate evaluate = new ParLevel2Evaluate() { Evaluate = 0 };

            var evaluateConf = ParEvaluateCompany.Where(p => p.Id == parlevel2.Id).FirstOrDefault();

            if (evaluateConf != null)
            {
                evaluate = evaluateConf;
            }
            else
            {
                evaluateConf = ParEvaluatePadrao.Where(p => p.Id == parlevel2.Id).FirstOrDefault();

                if (evaluateConf != null)
                {
                    evaluate = evaluateConf;
                }
            }

            return evaluate;
        }

        protected int getParFrequency_Id(SGQDBContext.ParLevel1 parlevel1, SGQDBContext.ParLevel2 parlevel2, int ParCompany_Id)
        {
            int parfrenquency_Id = 0;

            string sql = $@"            
            SELECT
            	CASE
            		WHEN FPE.ParFrequency_Id IS NULL THEN FPL2.ParFrequency_Id
            		ELSE FPE.ParFrequency_Id
            	END as ParFrequency_Id,
				FPE.ParCompany_Id
            FROM (SELECT
            			 ParFrequency_Id,
						 ParCompany_Id
            		 FROM ParEvaluation
            		 WHERE 1 = 1
            		 AND ParLevel1_Id = @ParLevel1_Id
            		 AND ParLevel2_Id = @ParLevel2_Id
            		 AND ParCluster_Id = @ParCluster_Id
            		 AND IsActive = 1
					 AND (ParCompany_Id = @ParCompany_Id OR ParCompany_Id IS NULL)) AS FPE
            	,(SELECT
            			 ParFrequency_Id
            		 FROM ParLevel2
            		 WHERE 1 = 1
            		 AND Id = @ParLevel2_Id
            		 AND IsActive = 1) AS FPL2
					 ORDER BY ParCompany_Id DESC";

            string conexao = this.conexao;
            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {

                        command.CommandType = CommandType.Text;
                        command.Parameters.Add(new SqlParameter("@ParCompany_Id", ParCompany_Id));
                        command.Parameters.Add(new SqlParameter("@ParLevel1_Id", parlevel1.ParLevel1_Id));
                        command.Parameters.Add(new SqlParameter("@ParLevel2_Id", parlevel2.ParLevel2_id));
                        command.Parameters.Add(new SqlParameter("@ParCluster_Id", parlevel1.ParCluster_Id));


                        connection.Open();
                        using (SqlDataReader r = command.ExecuteReader())
                        {
                            if (r.Read())
                            {
                                parfrenquency_Id = Convert.ToInt32(r[0]);
                            }
                        }
                    }

                    if (connection.State == System.Data.ConnectionState.Open)
                        connection.Close();
                }
            }

            catch (Exception)
            {

                return parfrenquency_Id;
            }

            return parfrenquency_Id;
        }

        protected int getMaxEvaluateLevel1(SGQDBContext.ParLevel1 parlevel1, IEnumerable<SGQDBContext.ParLevel2Evaluate> ParEvaluateCompany)
        {
            int evaluate = 0;

            string sql = $@"
                DECLARE @ParLevel1_id int =  @ParLevel1_Id
                DECLARE @ParCluster_id int = @ParCluster_Id 
                SELECT max(Number) as av FROM ParEvaluation EV (nolock) 
                WHERE ParLevel2_id in ( 
                SELECT p32.ParLevel2_Id FROM ParLevel3Level2Level1 P321 (nolock) 
                inner join ParLevel3Level2 P32 (nolock) 
                on p32.id = p321.ParLevel3Level2_Id 
                where p321.ParLevel1_Id = @ParLevel1_id and (p32.ParCompany_Id is null) and P321.Active = 1 and p32.IsActive = 1 
                and Ev.ParCluster_Id = @ParCluster_Id
                group by p32.ParLevel2_Id
                )
                and ev.IsActive = 1 
                and(ev.ParCompany_Id is null)";

            string conexao = this.conexao;
            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {


                        command.CommandType = CommandType.Text;
                        command.Parameters.Add(new SqlParameter("@ParLevel1_Id", parlevel1.ParLevel1_Id));
                        command.Parameters.Add(new SqlParameter("@ParCluster_Id", parlevel1.ParCluster_Id));


                        connection.Open();
                        using (SqlDataReader r = command.ExecuteReader())
                        {
                            if (r.Read())
                            {
                                evaluate = Convert.ToInt32(r[0]);
                            }
                        }
                    }
                    if (connection.State == System.Data.ConnectionState.Open) connection.Close();
                }
            }

            catch (Exception)
            {

                return evaluate;
            }

            return evaluate;
        }

        protected int getSample(SGQDBContext.ParLevel2 parlevel2, IEnumerable<SGQDBContext.ParLevel2Sample> ParSampleCompany, IEnumerable<SGQDBContext.ParLevel2Sample> ParSamplePadrao)
        {
            int sample = 0;

            var lista = parlevel2.Id.ToString().Replace(quebraProcesso, "|").Split('|');

            int parCluster_Id = lista.Length > 1 ? Int32.Parse(lista[0]) : 0;

            int parlevel2_id = lista.Length > 1 ? Int32.Parse(lista[1]) : Int32.Parse(lista[0]);

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
                sample = 0;
            }
            return sample;
        }

        protected string GetEvaluationSchedule(int parLevel1_Id, int parLevel2_Id, int company_Id, int shift_Id, int cluster_id)
        {
            try
            {
                List<string> frequencia = new List<string>();
                using (var conexaoEF = new SgqDbDevEntities())
                {

                    var parEvaluation = conexaoEF.ParEvaluation.Where(x => (x.ParLevel1_Id == parLevel1_Id || x.ParLevel1_Id == null)
                        && (x.ParCompany_Id == company_Id || x.ParCompany_Id == null)
                        && x.ParLevel2_Id == parLevel2_Id
                        && x.ParCluster_Id == cluster_id
                        && x.IsActive)
                        .OrderByDescending(x => new { x.ParCompany_Id, x.ParLevel1_Id })
                        .FirstOrDefault();

                    var list = conexaoEF.ParEvaluationSchedule.Where(x => x.ParEvaluation_Id == parEvaluation.Id && x.IsActive).ToList();

                    foreach (var item in list)
                    {
                        if (item.ParEvaluation.ParCompany_Id == list[0].ParEvaluation.ParCompany_Id)
                        {
                            if (parEvaluation.ParFrequency_Id != 10)
                            {
                                frequencia.Add($"{item.Av}-{item.Inicio}-{item.Fim}");
                            }
                            else
                            {
                                frequencia.Add($"{item.Intervalo}");
                            }
                        }
                    }

                    return string.Join("|", frequencia);
                }
            }
            catch (Exception Ex)
            {
                return "";
            }
        }

        protected string GetEvaluationScheduleVolume(SGQDBContext.ParLevel1 parLevel1, SGQDBContext.ParLevel2 parLevel2, int company_Id, int shift_Id, DateTime DateCollection)
        {
            try
            {

                using (var conexaoEF = new SgqDbDevEntities())
                {

                    //Se tiver - considerar como Intervalo
                    var agendamento = "";
                    var sql = "";
                    var date = DateCollection.ToString("yyyy-MM-dd");

                    switch (parLevel1.hashKey)
                    {
                        case 1: //VolumePcc1b

                            //sql = $@"SELECT TOP 1
                            //        	Agendamento
                            //        FROM VolumePcc1b(nolock)
                            //        WHERE Data = (SELECT
                            //        		MAX(DATA)
                            //        	FROM VolumePcc1b(nolock)
                            //        	WHERE ParCompany_id = { company_Id }
                            //        	AND (Shift_Id = { shift_Id }
                            //        	OR Shift_Id IS NULL)
                            //        	AND CAST(DATA AS DATE) <= '{ date }')
                            //        AND ParCompany_id = { company_Id }
                            //        AND (Shift_Id = { shift_Id }
                            //        OR Shift_Id IS NULL)
                            //        ORDER BY Shift_Id DESC";


                            sql = $@"SELECT TOP 1
                                    	Agendamento
                                    FROM VolumePcc1b(nolock)
                                    WHERE Data = (SELECT
                                    		MAX(DATA)
                                    	FROM VolumePcc1b(nolock)
                                    	WHERE ParCompany_id = @Company_Id
                                    	AND (Shift_Id = @Shift_Id
                                    	OR Shift_Id IS NULL)
                                    	AND CAST(DATA AS DATE) <= @Date)
                                    AND ParCompany_id = @Company_Id
                                    AND (Shift_Id = @Shift_Id)
                                    OR Shift_Id IS NULL)
                                    ORDER BY Shift_Id DESC";

                            using (Factory factory = new Factory("DefaultConnection"))
                            {
                                using (SqlCommand cmd = new SqlCommand(sql, factory.connection))
                                {
                                    cmd.CommandType = CommandType.Text;
                                    cmd.Parameters.Add(new SqlParameter("@Company_Id", company_Id));
                                    cmd.Parameters.Add(new SqlParameter("@Shift_Id", shift_Id));
                                    cmd.Parameters.Add(new SqlParameter("@Date", date));

                                    //agendamento = factory.SearchQuery<Dominio.VolumePcc1b>(cmd).FirstOrDefault()?.Agendamento;
                                    agendamento = null;

                                    //agendamento = conexaoEF.Database.SqlQuery<string>(cmd).FirstOrDefault();
                                }

                            }


                            break;

                        case 2: //VolumeCepDesossa

                            //sql = $@"SELECT TOP 1
                            //        	Agendamento
                            //        FROM VolumeCepDesossa(nolock)
                            //        WHERE Data = (SELECT
                            //        		MAX(DATA)
                            //        	FROM VolumeCepDesossa(nolock)
                            //        	WHERE ParCompany_id = { company_Id }
                            //        	AND (Shift_Id = { shift_Id }
                            //        	OR Shift_Id IS NULL)
                            //        	AND CAST(DATA AS DATE) <= '{ date }')
                            //        AND ParCompany_id = { company_Id }
                            //        AND (Shift_Id = { shift_Id }
                            //        OR Shift_Id IS NULL)
                            //        ORDER BY Shift_Id DESC";

                            sql = $@"SELECT TOP 1
                                    	Agendamento
                                    FROM VolumeCepDesossa(nolock)
                                    WHERE Data = (SELECT
                                    		MAX(DATA)
                                    	FROM VolumeCepDesossa(nolock)
                                    	WHERE ParCompany_id = @Company_Id
                                    	AND (Shift_Id = @Shift_Id
                                    	OR Shift_Id IS NULL)
                                    	AND CAST(DATA AS DATE) <= @Date)
                                    AND ParCompany_id = @Company_Id
                                    AND (Shift_Id = @Shift_Id
                                    OR Shift_Id IS NULL)
                                    ORDER BY Shift_Id DESC";
                            using (Factory factory = new Factory("DefaultConnection"))
                            {
                                using (SqlCommand cmd = new SqlCommand(sql, factory.connection))
                                {
                                    cmd.CommandType = CommandType.Text;
                                    cmd.Parameters.Add(new SqlParameter("@Company_Id", company_Id));
                                    cmd.Parameters.Add(new SqlParameter("@Shift_Id", shift_Id));
                                    cmd.Parameters.Add(new SqlParameter("@Date", date));

                                    agendamento = factory.SearchQuery<VolumeCepDesossa>(cmd).FirstOrDefault()?.Agendamento;
                                    //agendamento = conexaoEF.Database.SqlQuery<string>(cmd).FirstOrDefault();
                                }
                            }

                            break;
                        case 3: //VolumeVacuoGRD

                            //sql = $@"SELECT TOP 1
                            //        	Agendamento
                            //        FROM VolumeVacuoGRD(nolock)
                            //        WHERE Data = (SELECT
                            //        		MAX(DATA)
                            //        	FROM VolumeVacuoGRD(nolock)
                            //        	WHERE ParCompany_id = { company_Id }
                            //        	AND (Shift_Id = { shift_Id }
                            //        	OR Shift_Id IS NULL)
                            //        	AND CAST(DATA AS DATE) <= '{ date }')
                            //        AND ParCompany_id = { company_Id }
                            //        AND (Shift_Id = { shift_Id }
                            //        OR Shift_Id IS NULL)
                            //        ORDER BY Shift_Id DESC";

                            sql = $@"SELECT TOP 1
                                    	Agendamento
                                    FROM VolumeVacuoGRD(nolock)
                                    WHERE Data = (SELECT
                                    		MAX(DATA)
                                    	FROM VolumeVacuoGRD(nolock)
                                    	WHERE ParCompany_id = @Company_Id
                                    	AND (Shift_Id = @Shift_Id
                                    	OR Shift_Id IS NULL)
                                    	AND CAST(DATA AS DATE) <= @Date)
                                    AND ParCompany_id = @Company_Id
                                    AND (Shift_Id = @Shift_Id
                                    OR Shift_Id IS NULL)
                                    ORDER BY Shift_Id DESC";

                            using (Factory factory = new Factory("DefaultConnection"))
                            {
                                using (SqlCommand cmd = new SqlCommand(sql, factory.connection))
                                {
                                    cmd.CommandType = CommandType.Text;
                                    cmd.Parameters.Add(new SqlParameter("@Company_Id", company_Id));
                                    cmd.Parameters.Add(new SqlParameter("@Shift_Id", shift_Id));
                                    cmd.Parameters.Add(new SqlParameter("@Date", date));

                                    agendamento = factory.SearchQuery<VolumeVacuoGRD>(cmd).FirstOrDefault()?.Agendamento;
                                    //agendamento = conexaoEF.Database.SqlQuery<string>(cmd).FirstOrDefault();
                                }
                            }

                            break;
                        case 4: //VolumeCepRecortes

                            //sql = $@"SELECT TOP 1
                            //        	Agendamento
                            //        FROM VolumeCepRecortes(nolock)
                            //        WHERE Data = (SELECT
                            //        		MAX(DATA)
                            //        	FROM VolumeCepRecortes(nolock)
                            //        	WHERE ParCompany_id = { company_Id }
                            //        	AND (Shift_Id = { shift_Id }
                            //        	OR Shift_Id IS NULL)
                            //        	AND CAST(DATA AS DATE) <= '{ date }')
                            //        AND ParCompany_id = { company_Id }
                            //        AND (Shift_Id = { shift_Id }
                            //        OR Shift_Id IS NULL)
                            //        ORDER BY Shift_Id DESC";

                            sql = $@"SELECT TOP 1
                                    	Agendamento
                                    FROM VolumeCepRecortes(nolock)
                                    WHERE Data = (SELECT
                                    		MAX(DATA)
                                    	FROM VolumeCepRecortes(nolock)
                                    	WHERE ParCompany_id = @Company_Id
                                    	AND (Shift_Id = @Shift_Id
                                    	OR Shift_Id IS NULL)
                                    	AND CAST(DATA AS DATE) <= @Date)
                                    AND ParCompany_id = @Company_Id
                                    AND (Shift_Id = @Shift_Id
                                    OR Shift_Id IS NULL)
                                    ORDER BY Shift_Id DESC";

                            using (Factory factory = new Factory("DefaultConnection"))
                            {
                                using (SqlCommand cmd = new SqlCommand(sql, factory.connection))
                                {
                                    cmd.CommandType = CommandType.Text;
                                    cmd.Parameters.Add(new SqlParameter("@Company_Id", company_Id));
                                    cmd.Parameters.Add(new SqlParameter("@Shift_Id", shift_Id));
                                    cmd.Parameters.Add(new SqlParameter("@Date", date));

                                    agendamento = factory.SearchQuery<VolumeCepRecortes>(cmd).FirstOrDefault()?.Agendamento;
                                    //agendamento = conexaoEF.Database.SqlQuery<string>(cmd).FirstOrDefault();
                                }
                            }

                            break;

                        default:
                            return "";
                    }

                    return agendamento;

                }
            }
            catch (Exception Ex)
            {
                return "";
            }
        }

        protected int getMaxSampleLevel1(SGQDBContext.ParLevel1 parlevel1, IEnumerable<SGQDBContext.ParLevel2Evaluate> ParEvaluateCompany)
        {
            int evaluate = 0;

            string sql = $@"
                DECLARE @ParCompany_id int = 16
                DECLARE @ParLevel1_id int =  @ParLevel1_Id
                DECLARE @ParCluster_id int = @ParCluster_Id 
                SELECT max(Number) as av FROM ParSample EV (nolock)  
                WHERE ParLevel2_id in ( 
                SELECT p32.ParLevel2_Id FROM ParLevel3Level2Level1 P321 (nolock)  
                inner join ParLevel3Level2 P32  (nolock) 
                on p32.id = p321.ParLevel3Level2_Id 
                where p321.ParLevel1_Id = @ParLevel1_id and (p32.ParCompany_Id is null) and P321.Active = 1 and p32.IsActive = 1 
                and Ev.ParCluster_Id = @ParCluster_Id 
                group by p32.ParLevel2_Id 
                )
                and ev.IsActive = 1
                and(ev.ParCompany_Id is null)";

            string conexao = this.conexao;
            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {

                        command.CommandType = CommandType.Text;
                        command.Parameters.Add(new SqlParameter("@ParLevel1_Id", parlevel1.ParLevel1_Id));
                        command.Parameters.Add(new SqlParameter("@ParCluster_Id", parlevel1.ParCluster_Id));

                        connection.Open();
                        using (SqlDataReader r = command.ExecuteReader())
                        {
                            if (r.Read())
                            {
                                evaluate = Convert.ToInt32(r[0]);
                            }
                        }
                    }
                    if (connection.State == System.Data.ConnectionState.Open) connection.Close();
                }
            }

            catch (Exception)
            {

                return evaluate;
            }

            return evaluate;
        }

        protected string getAPPMain(int UserSgq_Id, int ParCompany_Id, DateTime Date, string Level1ListId, int Shift_Id, bool isVolume = false)
        {
            #region Antes do loop1

            var html = new Html();
            string culture;

            if (GlobalConfig.LanguageBrasil)
            {
                culture = "pt-br";
            }
            else
            {
                culture = "en-us";
            }

            string breadCrumb = "<ol class=\"breadcrumb\" breadmainlevel=\"Audit\"></ol>";

            string selectPeriod = html.option("1", CommonData.getResource("period").Value.ToString() + " 1") +
                              html.option("2", CommonData.getResource("period").Value.ToString() + " 2") +
                              html.option("3", CommonData.getResource("period").Value.ToString() + " 3") +
                              html.option("4", CommonData.getResource("period").Value.ToString() + " 4");

            string hide = string.Empty;
            if (GlobalConfig.Brasil || GlobalConfig.Ytoara)
            {
                hide = "hide";
            }

            selectPeriod = html.select(selectPeriod, id: "period", disabled: true, style: "width: 160px");

            selectPeriod = "<li class='painel list-group-item " + hide + " '>" + selectPeriod + " </li>";

            #endregion

            var seiLaLevel1 = GetLevel01(ParCompany_Id: ParCompany_Id, dateCollect: Date, Level1ListId: Level1ListId, isVolume: isVolume, Shift_Id: Shift_Id); /****** PORQUE ESTA MOKADO ESSA UNIDADE 1? *******/

            var seiLaCluster = GetClustersCompany(ParCompany_Id);

            string container = html.div(outerhtml: breadCrumb + selectPeriod + seiLaLevel1 + seiLaCluster, classe: "container");

            string buttons = " <button id=\"btnSave\" class=\"btn btn-lg btn-warning hide\"><i id=\"saveIcon\" class=\"fa fa-save\"></i><i id=\"loadIcon\" class=\"fa fa-circle-o-notch fa-spin\" style=\"display:none;\"></i></button>";

            buttons += " <button id=\"btnSaveTemp\" class=\"btn btn-lg btn-warning hide\"><i id=\"saveIcon\" class=\"fa fa-chevron-right\"></i><i id=\"loadIcon\" class=\"fa fa-circle-o-notch fa-spin\" style=\"display:none;\"></i></button>";

            buttons += " <button id=\"btnSaveAllTemp\" class=\"btn btn-lg btn-warning hide\"><i id=\"saveIcon\" class=\"fa fa-save\"></i><i id=\"loadIcon\" class=\"fa fa-circle-o-notch fa-spin\" style=\"display:none;\"></i></button>";

            buttons += " <button id=\"btnCA\" class=\"btn btn-lg btn-danger hide\">" + Resources.Resource.corrective_action + "</button>";

            string message = "<div class=\"message padding20\" style=\"display:none\">                                                                                      " +
                             "   <h1 class=\"head\">Titulo</h1>                                                                                                           " +
                             "   <div class=\"body font16\">Mensagem</div>                                                                                                " +
                             "   <div class=\"foot\"><button id=\"btnMessageOk\" class=\"btn btn-lg marginRight30 btn-primary pull-right btnMessage\"> Ok</button></div>      " +
                             "</div>";
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
                                        "    <div class=\"foot\"><button id=\"btnMessageYes\" class=\"btn btn-lg marginRight30 btn-primary pull-right btnMessage\"> " + CommonData.getResource("yes").Value.ToString() + " </button></div>                 " +
                                        "    <div class=\"foot\"><button id=\"btnMessageNo\" class=\"btn btn-lg marginRight30 btn-primary pull-right btnMessage\"> " + CommonData.getResource("no").Value.ToString() + " </button></div>                   " +
                                        "</div>         " +
                                        "";

            #region ParReason

            var listMotivo = dbEf.ParReason.Where(r => r.IsActive).ToList();

            StringBuilder selectMotivo = new StringBuilder();

            selectMotivo.Append(@"<select id=""slcMotivo"" class=""form-control"" style=""width: 600px;"">");

            foreach (var item in listMotivo.Where(x => x.ParReasonType_Id == 1))
                selectMotivo.Append($@"<option value=""{ item.Id }"">{item.Motivo}</option>");

            selectMotivo.Append(@"</select>");

            string messageParReasonType1 =
                $@"<div class=""messageParReason messageParReasonType1 padding20"" style=""display:none;"">
                        <h1 class=""head"">{ CommonData.getResource("select_reason_out_of_date").Value.ToString() }</h1>
                        <div class=""body font16""> <div class=""txtMessage""></div>
                            { selectMotivo }
                        <div class=""foot""><button id=""btnAtrasoOk"" class=""btn btn-lg marginRight30 btn-primary pull-right btnMessage""> OK </button></div>
                    </div></div>";


            selectMotivo = new StringBuilder();
            selectMotivo.Append(@"<select id=""slcMotivo"" class=""form-control"" style=""width: 600px;"">");

            foreach (var item in listMotivo.Where(x => x.ParReasonType_Id == 2))
                selectMotivo.Append($@"<option value=""{ item.Id }"">{item.Motivo}</option>");

            selectMotivo.Append(@"</select>");

            string messageParReasonType2 =
                $@"<div class=""messageParReason messageParReasonType2 padding20"" style=""display:none;"">
                        <h1 class=""head"">{ CommonData.getResource("select_reason_out_of_date").Value.ToString() }</h1>
                        <div class=""body font16""> <div class=""txtMessage""></div>
                            { selectMotivo }
                        <div class=""foot""><button id=""btnAtrasoOk"" class=""btn btn-lg marginRight30 btn-primary pull-right btnMessage""> OK </button></div>
                    </div></div>";

            #endregion

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

            debug = "<div id = 'ControlaDivDebugAlertas' style='display:none'></div> " +

                           "<div id = 'divDebugAlertas'  style='display:none'> " +
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

                           "</div> ";

            SGQDBContext.ParLevel3Vinculado listaProdutos = new ParLevel3Vinculado(db);
            var listaParLevel3VinculadoJSON = listaProdutos.getParLevel3Vinculado(ParCompany_Id);

            var listaParLevel3Vinculado =
                "<script> var listaParLevel3Vinculado = " + Newtonsoft.Json.JsonConvert.SerializeObject(listaParLevel3VinculadoJSON) + ";</script>";

            string local = "";

            if (GlobalConfig.Brasil)
            {
                local = "brasil";
            }
            if (GlobalConfig.Eua)
            {
                local = "eua";
            }
            if (GlobalConfig.Canada)
            {
                local = "canada";
            }


            return html.div(
                            outerhtml: navBar(UserSgq_Id, ParCompany_Id) +
                                       rightMenu() +
                                       html.div(classe: "overlay", style: "display:none") +
                                       container +
                                       buttons +
                                       footer(),
                             classe: "App hide",
                             tags: "breadmainlevel=\"" + CommonData.getResource("slaughter").Value.ToString()
                             + "\" culture=\"" + culture + "\" turningtime=\"03:00\" serverdate =\"" + DateTime.Now.AddDays(1).ToString("dd/MM/yyyy HH:mm") + "\""
                             + "\" local=\"" + local
                             + "\" "
                           ) +
                           correctiveAction() +
                           viewModal +
                           modalVF +
                           modalPCC1B +
                           message +
                           messageConfirm +
                           messageParReasonType1 +
                           messageParReasonType2 +
                           debug +
                           listaParLevel3Vinculado;
        }

        protected string navBar(int UserSgq_Id, int ParCompany_Id)
        {
            string navBar = "<div class=\"navbar navbar-inverse navbar-fixed-top\">                                                                                                                             " +
                           "    <div class=\"container\" style=\"padding: 0px !important;\">                                                                                                                                                       " +
                           "        <div class=\"navbar-header\" style=\"width: 100%\">                                                                                                                         " +
                           "            <a class=\"navbar-brand\" id=\"SGQName\" href=\"#\"><i class=\"fa fa-chevron-left hide iconReturn\" style=\"margin-left: 8px; font-size: 24px;\" aria-hidden=\"true\"></i> SGQ </a>                 " +
                           "            <div class=\"buttonMenu navbar-brand hide\" id=\"btnShowImage\" level01id=\"2\">Show Image</div>                                                                        " +
                           selectUserCompanys(UserSgq_Id, ParCompany_Id) +
                           //"            <span style='color: #ffffff; margin: 14px 0px;' class='period'></span> / <span style='color: #ffffff; margin: 14px 0px;' class='shift'>shift</span> " +
                           "            <span style='color: #ffffff; margin: 14px 0px;' class='shift'>shift</span> " +
                           "            <div id=\"btnMore\" class=\"iconMoreMenu pull-right\" style=\"padding: 12px; margin-right: 10px\"><i class=\"fa fa-ellipsis-v iconMoreMenu\" aria-hidden=\"true\"></i></div><span id='btnDate' style='color: #ffffff; margin: 14px 0px;' class='atualDate pull-right'></span>" +
                           "        </div>                                                                                                                                                                      " +
                           "    </div>                                                                                                                                                                          " +
                           "</div>                                                                                                                                                                              ";

            return navBar;
        }

        protected string rightMenu()
        {
            string menu = "<div class=\"rightMenu\">                                                                                                  " +
                           "     <div class=\"list-group list-group-inverse rightMenuList\">                                                           " +
                           "         <a href=\"#\" id=\"btnSync\" class=\"list-group-item\" style=\"background-color: black; font-weight: bold;\">" + CommonData.getResource("sync_results").Value.ToString() + "</a>                                                  " +
                           "         <a href=\"#\" id=\"btnSyncParam\" class=\"list-group-item\"  style=\"background-color: black; font-weight: bold;\">" + CommonData.getResource("sync_parameretrization").Value.ToString() + "</a>                                                  ";
            if (GlobalConfig.Brasil == true)
            {
                menu += "         <a href=\"#\" id=\"btnSyncVolume\" class=\"list-group-item\"  style=\"background-color: black; font-weight: bold;\">Sincronizar Volume</a> ";
                //"         <a href=\"#\" id=\"btnChangeModule\" class=\"list-group-item\"  style=\"background-color: black; font-weight: bold;\">" + CommonData.getResource("change_module").Value.ToString() + "</a>                                                  ";
            }
            menu += "         <a href=\"#\" id=\"btnLogout\" class=\"list-group-item\">" + CommonData.getResource("logout").Value.ToString() + "</a>                                                     " +
                           "         <a href=\"#\" id=\"btnLog\" class=\"list-group-item\">" + CommonData.getResource("view_log").Value.ToString() + "</a>                                                      " +
                           "         <a href=\"#\" id=\"btnCollectDB\" class=\"list-group-item\">" + CommonData.getResource("view_db").Value.ToString() + "</a>                                                 " +
                           "         <a href=\"#\" id=\"btnClearDatabase\" class=\"list-group-item\">" + CommonData.getResource("clean_db").Value.ToString() + "</a>                                            " +
                           "         <a href=\"#\" id=\"btnMostrarContadores\" class=\"list-group-item\">" + CommonData.getResource("show_counters").Value.ToString() + "</a>                                   " +
                           "         <a href=\"#\" id=\"btnAutoSend\" class=\"list-group-item\">" + CommonData.getResource("auto_send_on").Value.ToString() + "</a>                                   " +
                           "         <span id=\"version\" style=\"font-size: 11px\" class=\"list-group-item\">" + CommonData.getResource("version").Value.ToString() + ": <span class=\"number\"></span></span>                           " +
                           "         <span id=\"ambiente\" style=\"font-size: 11px\" class=\"list-group-item\"><span class=\"base\"></span></span>                                                                                        " +
                           "     </div>                                                                                                                                                                         " +
                           " </div>                                                                                                                                                                             ";

            return menu;
        }

        protected string correctiveAction()
        {
            string correctiveAction =
                "<div id=\"correctiveActionModal\" class=\"container panel panel-default modal-padrao\" style=\"display:none\">" +
                    "<div class=\"panel-body\">" +
                        "<div class=\"modal-body\">" +
                            "<h2>" + CommonData.getResource("corrective_action").Value.ToString() + " </h2>" +
                            "<div id=\"messageAlert\" class=\"alert alert-info hide\" role=\"alert\">" +
                                "<span id=\"mensagemAlerta\" class=\"icon-info-sign\"></span>" +
                            "</div>" +

                            "<div class=\"row formCorrectiveAction\">" +
                                "<div class=\"panel panel-default\">" +
                                    "<div class=\"panel-body\" >" +
                                        "<div class=\"row\" style=\"padding:8px;\">" +
                                            "<div class=\"col-xs-6\" id=\"CorrectiveActionTaken\">" +
                                                "<b class=\"font16\">" + CommonData.getResource("corrective_action_taken").Value.ToString() + ":<br/></b>" +
                                                "<b>" + CommonData.getResource("date_time").Value.ToString() + ":</b> <span id=\"datetime\"></span><br/>" +
                                                "<b>" + CommonData.getResource("auditor").Value.ToString() + ": </b><span id=\"auditor\"></span><br/>" +
                                                "<b>" + CommonData.getResource("shift").Value.ToString() + ": </b><span id=\"shift\"></span><br/>" +
                                            "</div>" +
                                            "<div class=\"col-xs-6\" id=\"AuditInformation\">" +
                                                "<b class=\"font16\">" + CommonData.getResource("audit_information").Value.ToString() + ":<br/></b>" +
                                                "<b>" + CommonData.getResource("level1").Value.ToString() + ": </b><span id=\"auditText\"></span><br/>" +
                                                "<b>" + CommonData.getResource("initial_date").Value.ToString() + ":</b><span id=\"starttime\"></span><br/>" +
                                                "<b>" + CommonData.getResource("period").Value.ToString() + ":</b><span id=\"correctivePeriod\"></span>" +
                                            "</div>" +
                                        "</div>" +
                                    "</div>" +
                                "</div>" +


                                "<div class=\"form-group\">" +
                                    "<label>" + CommonData.getResource("failure_description").Value.ToString() + ":</label>" +
                                    "<textarea id=\"DescriptionFailure\" class=\"form-control custom-control\" rows=\"3\" style=\"resize:none\"></textarea>" +
                                "</div>" +
                                "<div class=\"form-group\">" +
                                    "<label>" + CommonData.getResource("immediate_corrective_action").Value.ToString() + ":</label>" +
                                    "<textarea id=\"ImmediateCorrectiveAction\" class=\"form-control custom-control\" rows=\"3\" style=\"resize:none\"></textarea>" +
                                "</div>" +
                                "<div class=\"form-group\">" +
                                    "<label>" + CommonData.getResource("product_disposition").Value.ToString() + ":</label>" +
                                    "<textarea id=\"ProductDisposition\" class=\"form-control custom-control\" rows=\"3\" style=\"resize:none\"></textarea>" +
                                "</div>" +
                                "<div class=\"form-group\">" +
                                    "<label>" + CommonData.getResource("preventive_measure").Value.ToString() + ":</label>" +
                                    "<textarea id=\"PreventativeMeasure\" class=\"form-control custom-control\" rows=\"3\" style=\"resize:none\"></textarea>" +
                                "</div>";

            if (GlobalConfig.Eua)
            {
                correctiveAction +=
                                "<div class=\"row\">" +
                                    "<div class=\"col-xs-6\">" +
                                        "<div class=\"SlaugtherSignature hide\">" +
                                            "<h4>Slaughter Signature</h4>" +
                                            "<div class=\"name\">Admin</div>" +
                                            "<div class=\"date\">08/24/2016 10:31</div>" +
                                            "<button class=\"btn btn-link btnSlaugtherSignatureRemove\">" + CommonData.getResource("remove_signature").Value.ToString() + "</button>" +
                                        "</div>" +
                                    "</div>" +
                                    "<div class=\"col-xs-6\">" +
                                        "<div class=\"TechinicalSignature hide\">" +
                                            "<h4>Technical Signature</h4>" +
                                            "<div class=\"name\">Admin2</div>" +
                                            "<div class=\"date\">08/24/2016</div>" +
                                            "<button class=\"btn btn-link btnTechinicalSignatureRemove\">" + CommonData.getResource("remove_signature").Value.ToString() + "</button>" +
                                        "</div>" +
                                    "</div>" +
                                "</div>";
            }


            correctiveAction +=
                                "<div class=\"modal-footer\">";

            if (GlobalConfig.Eua)
            {
                correctiveAction +=
                                    "<span class=\"pull-left\">" +
                                        "<button class=\"btn btn-default btnSignature btnSlaugtherSignature\">" +
                                            CommonData.getResource("slaughter_signature").Value.ToString() +
                                        "</button>" +
                                        "<button class=\"btn btn-default btnSignature btnTechinicalSignature\">" +
                                            CommonData.getResource("technical_signature").Value.ToString() +
                                        "</button>" +
                                    "</span>";
            }

            correctiveAction +=
                                    "<button class=\"btn btn-danger modal-close-ca\">" + CommonData.getResource("close").Value.ToString() + "</button>" +
                                    "<button class=\"btn btn-primary\" id=\"btnSendCorrectiveAction\">" + CommonData.getResource("send").Value.ToString() + " </button>" +
                                "</div>" +
                        "</div>" +
                        "</div>" +
                        "</div>" +
                    "</div>";

            if (GlobalConfig.Eua)
            {
                correctiveAction +=
                    "<div id=\"modalSignatureCorrectiveAction\" class=\"panel panel-default modal-padrao signature\" style=\"display:none\">" +
                        "<div class=\"panel-body\">" +
                            "<div class=\"modal-header\">" +
                                "<h3 class=\"slaughtersig head hide\">" + CommonData.getResource("slaughter_signature").Value.ToString() + " </h3>" +
                                "<h3 class=\"techinicalsig head hide\">" + CommonData.getResource("technical_signature").Value.ToString() + " </h3>" +
                                "<div id=\"messageAlert\" class=\"alert alert-info hide\">" +
                                    "<span id=\"mensagemAlerta\" class=\"icon-info-sign\"></span>" +
                                "</div>" +
                            "</div>" +
                            "<div class=\"modal-body\">" +
                                "<div class=\"row\">" +
                                    "<div class=\"col-xs-12\">" +
                                        "<div class=\"form-group\">" +
                                            "<label>" + CommonData.getResource("login").Value.ToString() + ":</label>" +
                                            "<input type=\"text\" class=\"form-control\" id=\"signatureLogin\">" +
                                        "</div>" +
                                        "<div class=\"form-group\">" +
                                            "<label>" + CommonData.getResource("password").Value.ToString() + ":</label>" +
                                            "<input type=\"password\" class=\"form-control\" id=\"signaturePassword\">" +
                                        "</div>" +
                                        "<div id=\"messageError\" class=\"alert alert-danger hide\">" +
                                            "<span class=\"icon-remove-sign\"></span><strong>" + CommonData.getResource("error").Value.ToString() + "! </strong><span id=\"mensagemErro\"> </span>" +
                                        "</div>" +
                                        "<div id=\"messageAlert\" class=\"alert alert-info hide\">" +
                                            "<span id=\"mensagemAlerta\" class=\"icon-info-sign\"></span>" +
                                        "</div>" +
                                        "<div id=\"messageSuccess\" class=\"alert alert-success hide\">" +
                                            "<span id=\"mensagemSucesso\" class=\"icon-ok-circle\"></span>" +
                                        "</div>" +
                                    "</div>" +
                                "</div>" +
                            "</div>" +
                            "<div class=\"modal-footer\">" +
                                "<button class=\"btn btn-danger modal-close-signature\">" + CommonData.getResource("close").Value.ToString() + " </button>" +
                                "<button type=\"button\" class=\"btn btn-primary\" id=\"btnSignatureLogin\">" + CommonData.getResource("sign").Value.ToString() + " </button>" +
                            "</div>" +
                        "</div>" +
                    "</div>" +
                "</div>";
            }

            return correctiveAction;
        }

        protected string footer()
        {
            string foot = "<footer class=\"footer\" style=\"font-size: 11px;\">                                                                                                                                       " +
                          "   <p style=\"color:white; margin-left:16px; margin-right:16px; margin-top: 12px;\">                                                                      " +
                          "       <span class=\"user\">Admin</span> - <span class=\"unit\">Colorado</span> | <span class=\"urlPrefix\"></span>                                          " +
                          "        <span class=\"status pull-right\"></span> <span class=\"database pull-right\"></span>                                                                                                         " +
                          "   </p>                                                                                                                                                   " +
                          "</footer>                                                                                                                                                 ";

            return foot;
        }
        /// <summary>
        /// Recupera Level1 e seus monitoramentos e tarefas relacionados
        /// </summary>
        /// <returns></returns>
        protected string GetLevel01(int ParCompany_Id, DateTime dateCollect, string Level1ListId, bool isVolume, int Shift_Id)
        {

            #region Parametros do level 1 e "instancias"

            ///SE NÃO HOUVER NENHUM LEVEL1, LEVEL2, LEVEL3 INFORMAR QUE NÃO ENCONTROU MONITORAMENTOS
            var html = new Html();

            //Instanciamos a Classe ParLevel01 Dapper
            var ParLevel1DB = new SGQDBContext.ParLevel1(db, quebraProcesso);
            var ParCounterDB = new SGQDBContext.ParCounter(db, quebraProcesso);
            //Inicaliza ParLevel1VariableProduction
            var ParLevel1VariableProductionDB = new SGQDBContext.ParLevel1VariableProduction(db);
            var ParRelapseDB = new SGQDBContext.ParRelapse(db);

            //Buscamos os ParLevel11 para a unidade selecionada
            var parLevel1List = ParLevel1DB.getParLevel1ParCriticalLevelList(ParCompany_Id: ParCompany_Id, Level1ListId: Level1ListId, dateCollection: dateCollect);

            if (isVolume)
            {
                var parLevel1Familia = ParLevel1DB.getByFamilia(dateCollection: dateCollect);

                parLevel1List = parLevel1List.Where(r =>
                                            r.Name.Equals("(%) NC CEP Vácuo GRD") ||
                                            r.Name.Equals("(%) NC PCC 1B") ||
                                            r.Name.Equals("(%) NC CEP Desossa") ||
                                            r.Name.Equals("(%) NC CEP Matéria Prima") ||
                                            r.IsFixedEvaluetionNumber == true);
            }

            //Agrupamos o ParLevel1 por ParCriticalLevel
            var parLevel1GroupByCriticalLevel = parLevel1List.OrderBy(p => p.ParCriticalLevel_Id).GroupBy(p => p.ParCriticalLevel_Id);

            //Instanciamos uma variável para não gerenciar a utilizar do ParCriticalLevel
            bool ParCriticalLevel = false;

            //Instanciamos uma variável para instanciar a lista de level1, level2 e level3
            //Esses itens podem ser transformados funções menores
            System.Text.StringBuilder listlevel1 = new System.Text.StringBuilder();
            System.Text.StringBuilder listLevel2 = new System.Text.StringBuilder();
            System.Text.StringBuilder listLevel3 = new System.Text.StringBuilder();

            string excecao = null;
            #endregion

            //Percorremos a lista de agrupada
            foreach (var parLevel1Group in parLevel1GroupByCriticalLevel) //LOOP1
            {

                #region instancia

                //Instanciamos uma variável level01GroupList
                System.Text.StringBuilder level01GroupList = new System.Text.StringBuilder();
                //Instanciamos uma variável list parLevel1 para adicionar os parLevel1
                System.Text.StringBuilder parLevel1 = new System.Text.StringBuilder();
                //Instanciamos uma variável para verificar o nome do ParCriticalLevel
                string nameParCritialLevel = null;
                //Percorremos a Lista dos Agrupamento 

                #endregion

                //var counter = 0;
                foreach (var parlevel1 in parLevel1Group) //LOOP2
                {

                    #region 1 monte de coisa que aparentemente roda rapido....

                    string tipoTela = "";

                    var variableList = ParLevel1VariableProductionDB.getVariable(parlevel1.ParLevel1_Id).ToList();

                    if (variableList.Count > 0)
                    {
                        tipoTela = variableList[0].Name;
                    }

                    var ParLevel2DB = new SGQDBContext.ParLevel2(db, quebraProcesso);
                    var parlevel02List = ParLevel2DB.getLevel2ByIdLevel1(parlevel1, dateCollect, ParCompany_Id);

                    //Se o ParLevel1 contem um ParCritialLevel_Id
                    var ParLevel1AlertasDB = new SGQDBContext.ParLevel1Alertas(db);
                    var alertas = ParLevel1AlertasDB.getAlertas(parlevel1, ParCompany_Id, dateCollect, Shift_Id);

                    if (alertas != null)
                    {
                        if (parlevel1.ParCriticalLevel_Id > 0)
                        {
                            //O ParLevel1 vai estar dentro de um accordon
                            ParCriticalLevel = true;
                            //Pego o nome do ParCriticalLevel para não precisar fazer outra pesquisa
                            nameParCritialLevel = parlevel1.ParCriticalLevel_Name;
                            //Incremento os itens que estaram no ParLevel1                
                            //Gera linha Level1

                            int tipoAlerta = parlevel1.tipoAlerta;
                            decimal valorAlerta = parlevel1.valorAlerta;

                            decimal alertaNivel1 = 0;
                            decimal alertaNivel2 = 0;
                            string alertaNivel3 = "";

                            decimal volumeAlerta = 0;
                            decimal meta = 0;

                            if (tipoAlerta == 1) //JBS por Indicador
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
                            else if (tipoAlerta == 5)  //# de NC
                            {
                                if (alertas != null)
                                {
                                    alertaNivel1 = valorAlerta;
                                    alertaNivel2 = valorAlerta;
                                    alertaNivel3 = "a5";
                                    volumeAlerta = alertas.VolumeAlerta;
                                    meta = alertas.Meta;
                                }
                            }
                            else if (tipoAlerta == 6)  //# de NC
                            {
                                if (alertas != null)
                                {
                                    alertaNivel1 = valorAlerta;
                                    alertaNivel2 = valorAlerta;
                                    alertaNivel3 = "a6";
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
                                    alertaNivel3 = "a0";
                                    volumeAlerta = alertas.VolumeAlerta;
                                    meta = alertas.Meta;
                                }
                            }

                            var listCounter = ParCounterDB.GetParLevelXParCounterList(parlevel1, null, 1);

                            string painelCounters = "";

                            //Identidicar se possui contador para o indicador
                            if (listCounter != null)
                            {
                                painelCounters = html.painelCounters(listCounter.Where(r => r.Local == "level1_line"), "margin-top: 40px;font-size: 12px;");
                            }

                            if (GlobalConfig.Eua && parlevel1.Name.Contains("CFF"))
                            {
                                tipoTela = "CFF";
                            }

                            var listParRelapse = ParRelapseDB.getRelapses(parlevel1.ParLevel1_Id);

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
                                                         metaIndicador: meta,
                                                         IsLimitedEvaluetionNumber: parlevel1.IsLimitedEvaluetionNumber,
                                                         listParRelapse: listParRelapse,
                                                         ParCluster_Id: parlevel1.ParCluster_Id.ToString());
                            //Incrementa level1
                            parLevel1.Append(html.listgroupItem(parlevel1.Id.ToString(), classe: "row " + excecao, outerhtml: level01 + painelCounters));
                        }
                        else
                        {
                            //Caso o ParLevel1 não contenha um ParCritialLevel_Id apenas incremento os itens de ParLevel1
                            parLevel1.Append(html.listgroupItem(parlevel1.Id.ToString(), outerhtml: parlevel1.Name, classe: excecao));
                        }
                    }
                    //Instancia variável para receber todos os level3
                    StringBuilder level3Group = new StringBuilder();

                    #endregion

                    //Busca os Level2 e reforna no level3Group;
                    listLevel2.Append(GetLevel02(parlevel1, ParCompany_Id, dateCollect, level3Group, Shift_Id, isVolume));

                    //Incrementa Level3Group
                    listLevel3.Append(level3Group);
                    //counter++;
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
                    parLevel1 = new System.Text.StringBuilder(html.accordeon(
                                                id: parLevel1Group.Key.ToString() + "critivalLevel",
                                                label: nameParCritialLevel,
                                                color: color,
                                                outerhtml: parLevel1.ToString(),
                                                aberto: true));
                }
                else
                {
                    //Adicionamos os itens e um listgroup
                    level01GroupList = new System.Text.StringBuilder(html.listgroup(
                                                   outerhtml: parLevel1.ToString()
                                                ));
                }
                //Adicionar a lista de level01 agrupados ou não a lsita geral
                listlevel1.Append(parLevel1);
            }
            //Retona as lista
            //Podemos gerar uma verificação de atualizações
            html.div(true,
                    outerhtml: listlevel1,
                    classe: "level1List"
                    );
            html.div(true,
                     outerhtml: listLevel2,
                     classe: "level2List col-xs-12 hide"
                    );
            html.div(true,
                     outerhtml: listLevel3,
                     classe: "level3List  List col-xs-12 hide"
                    );
            listlevel1.Append(listLevel2);
            listlevel1.Append(listLevel3);
            return listlevel1.ToString();

        }

        /// <summary>
        /// Recupera Level1 e seus monitoramentos e tarefas relacionados
        /// </summary>
        /// <returns></returns>
        protected string GetClustersCompany(int ParCompany_Id)
        {

            #region Buscar CLusters da Unidade

            ///SE NÃO HOUVER NENHUM LEVEL1, LEVEL2, LEVEL3 INFORMAR QUE NÃO ENCONTROU MONITORAMENTOS
            var html = new Html();

            //Buscar a lista de Módulos
            SGQDBContext.Generico listaClusterGroup = new Generico(db, conexao);
            var parClusterGroupSQL = listaClusterGroup.getClusterGroupCompany(ParCompany_Id: ParCompany_Id);

            SGQDBContext.Generico listaCluster = new Generico(db, conexao);

            var parClusterSQL = new List<Generico>();

            //Instanciamos uma variável para instanciar a lista de level1, level2 e level3
            //Esses itens podem ser transformados funções menores
            System.Text.StringBuilder listCluster = new System.Text.StringBuilder();

            var i = 0;

            foreach (var parClusterGroup in parClusterGroupSQL)
            {
                i++;

                //Buscamos os Clusters para a unidade selecionada
                parClusterSQL = listaCluster.getClusterCompany(ParCompany_Id: ParCompany_Id, ParClusterGroup_Id: parClusterGroup.id);



                string excecao = null;
                #endregion

                #region instancia

                //Instanciamos uma variável level01GroupList
                System.Text.StringBuilder parClusterList = new System.Text.StringBuilder();
                //Instanciamos uma variável list parLevel1 para adicionar os parLevel1
                System.Text.StringBuilder parClusters = new System.Text.StringBuilder();

                //Percorremos a Lista dos Agrupamento 

                #endregion

                var meuHtml = "<div class=\"panel-group\"><div class=\"panel panel-info\"><div class=\"panel-heading\" role=\"tab\" id=\"heading" + i + "ParClusterGroup\">";
                meuHtml += "<h4 class=\"panel-title\"><a role=\"button\" data-toggle=\"collapse\" href=\"#collapse1critivalLevel\" class=\"\" aria-expanded=\"true\" aria-controls=\"collapse" + i + "ParClusterGroup\">" + parClusterGroup.nome + "</a></h4></div>";
                meuHtml += "<div id=\"collapse" + i + "ParClusterGroup\" class=\"panel-collapse collapse in\" role=\"tabpanel\" aria-labelledby=\"heading" + i + "ParClusterGroup\">";

                parClusters.Append(meuHtml);

                //var counter = 0;
                foreach (var parCluster in parClusterSQL) //LOOP2
                {

                    string clusterObj = html.cluster(parCluster.id,
                                                    parCluster.nome);

                    //Incrementa level1
                    parClusters.Append(html.listgroupItem(parCluster.id.ToString(), classe: "row " + excecao, outerhtml: clusterObj));

                }

                parClusters.Append("</div></div></div>");

                //Quando termina o loop dos itens agrupados por ParCritialLevel 
                //Se contem ParCritialLevel


                //Adicionamos os itens e um listgroup
                parClusterList = new System.Text.StringBuilder(html.listgroup(
                                                outerhtml: parClusters.ToString()
                                            ));

                //Adicionar a lista de level01 agrupados ou não a lsita geral
                listCluster.Append(parClusters);

            }




            //Retona as lista
            //Podemos gerar uma verificação de atualizações
            html.div(true,
                    outerhtml: listCluster,
                    classe: "ClusterList"
                    );

            return listCluster.ToString();

        }

        /// <summary>
        /// Gera Linhas do level2
        /// </summary>
        /// <param name="ParLevel1"></param>
        /// <param name="ParCompany_Id"></param>
        /// <param name="level3Group"></param>
        /// <returns></returns>
        protected string GetLevel02(SGQDBContext.ParLevel1 ParLevel1, int ParCompany_Id, DateTime dateCollect, StringBuilder level3Group, int Shift_Id, bool isVolume)
        {

            #region Parametros e "Instancias"

            //Inicializa ParLevel2
            var ParLevel1DB = new SGQDBContext.ParLevel1(db, quebraProcesso);
            var ParLevel2DB = new SGQDBContext.ParLevel2(db, quebraProcesso);
            var ParCounterDB = new SGQDBContext.ParCounter(db, quebraProcesso);

            //Pega uma lista de ParLevel2
            //Tem que confirmar a company e colocar na query dentro do método, ainda não foi validado
            var parlevel02List = ParLevel2DB.getLevel2ByIdLevel1(ParLevel1, dateCollect, ParCompany_Id);

            //Inicializa Cabecalhos
            var ParLevelHeaderDB = new SGQDBContext.ParLevelHeader(db);
            //Inicaliza ParFieldType
            var ParFieldTypeDB = new SGQDBContext.ParFieldType(db, conexao);
            var ParNCRuleDB = new SGQDBContext.NotConformityRule(db);

            var reauditFlag = "<li class='painel row list-group-item hide active reauditFlag'> Reaudit <span class='reauditnumber'></span></li>";

            var html = new Html();

            //Instancia parLevel2List
            string ParLevel2List = null;
            //Instancia headerlist
            string headerList = null;

            //Inicializa Avaliações e Amostras
            var ParEvaluateDB = new SGQDBContext.ParLevel2Evaluate(db, quebraProcesso);
            var ParSampleDB = new SGQDBContext.ParLevel2Sample(db, quebraProcesso);


            //Verifica avaliações padrão
            var ParEvaluatePadrao = ParEvaluateDB.getEvaluate(ParLevel1: ParLevel1,
                                                              ParCompany_Id: null, DateCollection: dateCollect, Shift_Id: Shift_Id);

            //Verifica avaliações pela company informada
            var ParEvaluateCompany = ParEvaluateDB.getEvaluate(ParLevel1: ParLevel1,
                                                               ParCompany_Id: ParCompany_Id, DateCollection: dateCollect, Shift_Id: Shift_Id);

            //Verifia amostra padrão
            var ParSamplePadrao = ParSampleDB.getSample(ParLevel1: ParLevel1,
                                                        ParCompany_Id: null, DateCollection: dateCollect, Shift_Id: Shift_Id);

            //Verifica amostra pela company informada
            var ParSampleCompany = ParSampleDB.getSample(ParLevel1: ParLevel1,
                                                        ParCompany_Id: ParCompany_Id, DateCollection: dateCollect, Shift_Id: Shift_Id);

            //Variaveis para avaliação de grupos
            int evaluateGroup = 0;
            int sampleGroup = 0;

            StringBuilder groupLevel3Level2 = new StringBuilder();
            StringBuilder painelLevel3 = new StringBuilder();

            #endregion

            int evaluate = 0;
            int sample = 0;
            int defect = 0;

            if (ParLevel1.HasGroupLevel2 == true)
            {
                evaluate = getMaxEvaluateLevel1(ParLevel1, ParEvaluateCompany);
                sample = getMaxSampleLevel1(ParLevel1, ParEvaluateCompany);
                evaluateGroup = evaluate;
                sampleGroup = sample;
            }

            var departamento = "";

            var index = 0;
            var count = parlevel02List.Count();

            var auxDepto = "";
            var countDepto = 0;

            foreach (var parlevel2count in parlevel02List) //LOOP3
            {
                if (parlevel2count.Departamento != auxDepto)
                {
                    countDepto++;
                }
                auxDepto = parlevel2count.Departamento;
            }


            //Enquando houver lista de level2
            foreach (var parlevel2 in parlevel02List) //LOOP3
            {
                string frequencia = "";
                //Verifica se pega avaliações e amostras padrão ou da company

                var parlevel2ParFrequency = getParFrequency_Id(ParLevel1, parlevel2, ParCompany_Id);

                if (ParLevel1.HasGroupLevel2 != true)
                {
                    var parlevel2Evaluate = getEvaluate(parlevel2, ParEvaluateCompany, ParEvaluatePadrao);


                    if (isVolume)
                    {
                        frequencia = GetEvaluationScheduleVolume(ParLevel1, parlevel2, ParCompany_Id, Shift_Id, dateCollect);

                        if (frequencia != null)
                        {
                            parlevel2.ParFrequency_Id = frequencia.Contains("-") ? 3 : 10;
                        }

                    }
                    else
                    {
                        frequencia = GetEvaluationSchedule(ParLevel1.ParLevel1_Id, parlevel2.ParLevel2_id, ParCompany_Id, Shift_Id, ParLevel1.ParCluster_Id);
                    }


                    evaluate = parlevel2Evaluate.Evaluate;
                    sample = getSample(parlevel2, ParSampleCompany, ParSamplePadrao);
                    //defect = getCollectionLevel2Keys(ParCompany_Id,data, ParLevel1);

                }


                ////Se agrupar level2 com level3 pego o valor da primeira avaliação e amostra
                //if (ParLevel1.HasGroupLevel2 == true & evaluateGroup == 0)
                //{
                //evaluateGroup = evaluate;
                //sampleGroup = sample;
                //}

                //Colocar função de gerar cabeçalhos por selectbox
                //Monta os cabecalhos
                //Incluisão de coluna de defeito.
                #region Cabecalhos e Contadores
                string headerCounter =
                                     html.div(
                                               outerhtml: "<b>" + CommonData.getResource("ev").Value.ToString() + " </b>",
                                               classe: "col-xs-4",
                                               style: "text-align:center"
                                             ) +
                                     html.div(
                                               outerhtml: "<b>" + CommonData.getResource("sd").Value.ToString() + " </b>",
                                               classe: "col-xs-4",
                                               style: "text-align:center"
                                              ) +
                                      html.div(
                                               outerhtml: "<b>" + CommonData.getResource("df").Value.ToString() + " </b>",
                                               classe: "col-xs-4",
                                               style: "text-align:center"
                                             );

                headerCounter = html.div(
                                    //aqui vai os botoes
                                    outerhtml: headerCounter,
                                    classe: "counters col-xs-4 headerCounter"
                                    );


                string classXSLevel2 = " col-xs-5";

                int totalSampleXEvaluate = evaluate * sample;

                string counters =
                                      html.div(
                                                outerhtml: html.span(outerhtml: "0", classe: "evaluateCurrent") + html.span(outerhtml: "/", classe: "separator") + html.span(outerhtml: evaluate.ToString(), classe: "evaluateTotal"),
                                                classe: "col-xs-4",
                                                style: "text-align:center; font-size:10px;"
                                              ) +
                                      html.div(
                                                outerhtml: html.span(outerhtml: "0", classe: "sampleCurrent hide") + html.span(outerhtml: "0", classe: "sampleCurrentTotal") + html.span(outerhtml: "/", classe: "separator") + html.span(outerhtml: sample.ToString(), classe: "sampleTotal hide") + html.span(outerhtml: totalSampleXEvaluate.ToString(), classe: "sampleXEvaluateTotal"),
                                                classe: "col-xs-4",
                                                style: "text-align:center; font-size:10px;"
                                              ) +
                                       html.div(
                                                outerhtml: html.span(outerhtml: defect.ToString(), classe: "defectstotal"),
                                                classe: "col-xs-4",
                                                style: "text-align:center; font-size:10px;"
                                              );



                //                        html.div(
                //                                    outerhtml: html.span(outerhtml: "0", classe: "defectsLevel2"),
                //                                    classe: "col-xs-3",
                //                                    style: "text-align:center"
                //                                 ) +
                //                          html.div(
                //                                    outerhtml: html.span(outerhtml: "", classe: "newcoutner"),
                //                                    classe: "col-xs-3",
                //                                    style: "text-align:center"
                //                                 );

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
                                       "   <span class=\"cursorPointer\"><i class=\"fa fa-check\" aria-hidden=\"true\"></i></span>     " +
                                       "</button>                                                                                                      " +
                                       "<button class=\"btn btn-primary btnAreaSave\">                                                                 " +
                                       "   <span class=\"cursorPointer iconsArea\"><i class=\"fa fa-floppy-o\" aria-hidden=\"true\"></i></span>        " +
                                       "</button>                                                                                                      ";
                    }
                    string btnReaudit = null;
                    if (parlevel2.IsReaudit || ParLevel1.IsReaudit)
                    {
                        btnReaudit = "<button class=\"btn btn-primary hide btnReaudit\"> " +
                                      "<span>R</span></button>";
                    }
                    buttons = html.div(
                                 //aqui vai os botoes
                                 outerhtml: btnReaudit +
                                            btnAreaSave +
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
                    classXSLevel2 = " col-xs-5";
                    string btnReaudit = null;
                    if (parlevel2.IsReaudit || ParLevel1.IsReaudit)
                    {
                        btnReaudit = "<button class=\"btn btn-primary hide btnReaudit\"> " +
                                      "<span>R</span></button>";

                        buttons = html.div(
                                     //aqui vai os botoes
                                     outerhtml: btnReaudit,
                                     style: "text-align: right",
                                     classe: "userInfo col-xs-1"
                                     );
                    }

                }

                string level02Header = html.div(classe: classXSLevel2) +
                                       headerCounter +
                                       buttonsHeaders;

                headerList = html.listgroupItem(
                                                classe: "row",
                                                outerhtml: level02Header
                                               );

                var parNCRuleDB = ParNCRuleDB.getParNCRule(parlevel2.ParNotConformityRule_id, parlevel2.ParLevel2_id, ParLevel1.Id);
                decimal ruleValue = 0;

                if (parNCRuleDB != null)
                {
                    ruleValue = parNCRuleDB.Value;
                }



                //podemos aplicar os defeitos
                string level2 = html.level2(id: parlevel2.Id.ToString(),
                                            label: parlevel2.Name,
                                            classe: classXSLevel2,
                                            evaluate: evaluate,
                                            sample: sample,
                                            HasSampleTotal: parlevel2.HasSampleTotal,
                                            ParFrequency_Id: parlevel2ParFrequency, // parlevel2.ParFrequency_Id,
                                            IsEmptyLevel3: parlevel2.IsEmptyLevel3,
                                            RuleId: parlevel2.ParNotConformityRule_id,
                                            RuleValue: ruleValue.ToString(),
                                            reaudit: parlevel2.IsReaudit,
                                            HasTakePhoto: parlevel2.HasTakePhoto,
                                            FrequenciaValor: frequencia,
                                            FrequenciaMensagemInativo: "");

                var listLineCounter = ParCounterDB.GetParLevelXParCounterList(null, parlevel2, 2);

                string lineCounters = "";

                if (listLineCounter != null)
                {
                    lineCounters = html.painelCounters(listLineCounter.Where(r => r.Local == "level2_line"), "margin-top: 45px;font-size: 12px;");
                }

                //Gera monitoramento do level3
                string groupLevel3 = GetLevel03(ParLevel1, parlevel2, ParCompany_Id, dateCollect, out painelLevel3);

                if (string.IsNullOrEmpty(groupLevel3))
                    continue;

                //Gera linha do Level2

                var inicioGrupo = false;
                var fimGrupo = false;
                var fimFinalGrupo = false;
                var Grupo = "";

                if (countDepto > 1)
                {

                    if (departamento == "")
                    {
                        Grupo = parlevel2.Departamento;
                        inicioGrupo = true;
                    }
                    else if (departamento == parlevel2.Departamento)
                    {
                        inicioGrupo = false;
                    }
                    else if (departamento != parlevel2.Departamento)
                    {
                        Grupo = parlevel2.Departamento;
                        inicioGrupo = true;
                        fimGrupo = true;
                    }

                    if (++index == count)
                    {
                        fimFinalGrupo = true;
                    }

                    departamento = parlevel2.Departamento;

                }

                ParLevel2List += html.listgroupItem(
                                                    id: parlevel2.Id.ToString(),
                                                    classe: "row gabriel " + parlevel2.Departamento,
                                                    tags: "departamento='" + parlevel2.Departamento + "'",
                                                    outerhtml: level2 +
                                                                counters +
                                                                buttons +
                                                                html.div(classe: "level2Debug") +
                                                                lineCounters,
                                                    inicioGrupo: inicioGrupo,
                                                    fimGrupo: fimGrupo,
                                                    fimFinalGrupo: fimFinalGrupo,
                                                    Grupo: Grupo
                                                    );

                //ParLevel2List += html.listgroupItem(
                //                                    id: parlevel2.Id.ToString(),
                //                                    classe: "row",
                //                                    outerhtml: level2 +
                //                                               counters +
                //                                               buttons +
                //                                               html.div(classe: "level2Debug") +
                //                                               lineCounters
                //                                    );

                if (ParLevel1.HasGroupLevel2 == true)
                {
                    var othersTags = "defects=\"" + 1 +
                           "\" evaluate=\"" + evaluate +
                           "\" sample=\"" + sample +
                           "\" weievaluation=\"0" +
                           "\" evaluatetotal=\"0" +
                           "\" defectstotal=\"0\" weidefects=\"0\"" +
                           " totallevel3evaluation=\"0\"" +
                           " totallevel3withdefects=\"0\"" +
                           " hassampletotal=\"" + parlevel2.HasSampleTotal.ToString().ToLower() + "\"" +
                           " isemptylevel3=\"" + parlevel2.IsEmptyLevel3.ToString().ToLower()
                           + "\" ParNotConformityRule_id=\"" + parlevel2.ParNotConformityRule_id
                           + "\" ParNotConformityRule_value=\"" + ruleValue.ToString()
                           + "\" AlertValue=\"" + 0
                           + "\" reaudit=\"" + parlevel2.IsReaudit.ToString().ToLower() + "\"";

                    groupLevel3 = html.accordeon(
                                                    id: parlevel2.Id.ToString(),
                                                    label: parlevel2.Name,
                                                    classe: "level2 row",
                                                    outerhtml: groupLevel3,
                                                    accordeonId: parlevel2.Id,
                                                    othersTags: othersTags
                                                );

                    groupLevel3Level2.Append(groupLevel3);
                }
                else
                {
                    level3Group.Append(groupLevel3);
                }

            }

            //Se tiver agrupamentos no ParLevel1
            if (ParLevel1.HasGroupLevel2 == true)
            {

                string headerCounter =
                                     html.div(
                                               outerhtml: "<b>" + CommonData.getResource("ev").Value.ToString() + " </b>",
                                               classe: "col-xs-4",
                                               style: "text-align:center"
                                             ) +
                                     html.div(
                                               outerhtml: "<b>" + CommonData.getResource("sd").Value.ToString() + " </b>",
                                               classe: "col-xs-4",
                                               style: "text-align:center"
                                              ) +
                                      html.div(
                                               outerhtml: "<b>" + CommonData.getResource("df").Value.ToString() + " </b>",
                                               classe: "col-xs-4",
                                               style: "text-align:center"
                                             );

                headerCounter = html.div(
                                    //aqui vai os botoes
                                    outerhtml: headerCounter,
                                    classe: "counters col-xs-4 headerCounter"
                                    );


                // string classXSLevel2 = " col-xs-5";

                int totalSampleXEvaluate = evaluate * sample;

                string counters =
                                      html.div(
                                                outerhtml: html.span(outerhtml: "0", classe: "evaluateCurrent") + html.span(outerhtml: "/", classe: "separator") + html.span(outerhtml: evaluate.ToString(), classe: "evaluateTotal"),
                                                classe: "col-xs-4",
                                                style: "text-align:center; font-size:10px;"
                                              ) +
                                      html.div(
                                                outerhtml: html.span(outerhtml: "0", classe: "sampleCurrent hide") + html.span(outerhtml: "0", classe: "sampleCurrentTotal") + html.span(outerhtml: "/", classe: "separator") + html.span(outerhtml: sample.ToString(), classe: "sampleTotal hide") + html.span(outerhtml: totalSampleXEvaluate.ToString(), classe: "sampleXEvaluateTotal"),
                                                classe: "col-xs-4",
                                                style: "text-align:center; font-size:10px;"
                                              ) +
                                       html.div(
                                                outerhtml: html.span(outerhtml: defect.ToString(), classe: "SmpDefects"),
                                                classe: "col-xs-4",
                                                style: "text-align:center; font-size:10px;"
                                              ); //+
                                                 //html.div(
                                                 //      outerhtml: html.span(outerhtml: defect.ToString(), classe: "3moreDefects"),
                                                 //      classe: "col-xs-4",
                                                 //      style: "text-align:center; font-size:10px;"
                                                 // );
                counters = html.div(
                                   //aqui vai os botoes
                                   outerhtml: counters,
                                   classe: "counters col-xs-4"
                                   );
                string parLevel3Group = null;


                string accordeonbuttons = null;

                accordeonbuttons = "<button class=\"btn btn-default button-expand marginRight10\"><i class=\"fa fa-expand\" aria-hidden=\"true\"></i> " + @Resources.Resource.show_all + "</button>" +
                                   "<button class=\"btn btn-default button-collapse\"><i class=\"fa fa-compress\" aria-hidden=\"true\"></i> " + @Resources.Resource.hide_all + "</button>";

                string panelAccordeon = html.listgroupItem(
                                                           outerhtml: accordeonbuttons,
                                                           classe: "painel painelLevel02 row"
                                                        );


                if (!string.IsNullOrEmpty(groupLevel3Level2.ToString()))
                {
                    parLevel3Group = html.div(
                                               classe: "level3Group",
                                               tags: "level1idgroup=\"" + ParLevel1.Id + "\"",

                                               outerhtml: reauditFlag +
                                                          painelLevel3.ToString() +
                                                          panelAccordeon +
                                                          groupLevel3Level2.ToString()
                                             );

                    level3Group.Append(parLevel3Group);
                }

                //headerList = null;
                var listLineCounter = ParCounterDB.GetParLevelXParCounterList(ParLevel1, null, 1);

                string lineCounters = "";

                if (listLineCounter != null)
                {
                    lineCounters = html.painelCounters(listLineCounter, "margin-top: 45px;font-size: 12px;");
                }
                string level2 = html.level2(id: "0",
                                            label: ParLevel1.Name,
                                            classe: "group col-xs-5",
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
                                                               counters +
                                                               lineCounters +
                                                               html.div(classe: "level2Debug")
                                                    );
            }

            //aqui tem que fazer a pesquisa se tem itens sao do level1 ex: cca,htp
            //quando tiver cabecalhos tem que replicar no level1

            ParLevel2List = headerList +
                            ParLevel2List;

            var painelLevel2HeaderListHtml = GetHeaderHtml(ParLevelHeaderDB.getHeaderByLevel1(ParLevel1.ParLevel1_Id), ParFieldTypeDB, html, ParLevel1.ParLevel1_Id, ParCompany_id: ParCompany_Id);


            //if (!string.IsNullOrEmpty(painelLevel2HeaderListHtml))
            //{
            //    painelLevel2HeaderListHtml = html.listgroupItem(
            //                                                    outerhtml: painelLevel2HeaderListHtml,
            //                                                    classe: "row painelLevel02"
            //                                                    );
            //}

            var listCounter = ParCounterDB.GetParLevelXParCounterList(ParLevel1, null, 1);

            string painelCounters = "";

            if (listCounter != null)
            {
                //var listAux = listCounter.Where(r => r.Local == "level2_header" && Convert.ToInt32(r.indicador) == ParLevel1.Id);
                painelCounters = html.painelCounters(listCounter.Where(r => r.Local == "level2_header"), "margin-top: 45px;font-size: 12px;");
                var form_dentro = html.div(
                                            outerhtml: painelCounters,
                                            classe: "form-group header",
                                            style: "margin-bottom: 4px;"
                                           );
                painelCounters += html.div(
                                            outerhtml: form_dentro,
                                            classe: "col-xs-6 col-sm-4 col-md-3 col-lg-2",
                                            style: "padding-right: 4px !important; padding-left: 4px !important;"
                                           );
            }

            if (!string.IsNullOrEmpty(painelLevel2HeaderListHtml))
            {
                painelLevel2HeaderListHtml = html.listgroupItem(
                                                                outerhtml: painelLevel2HeaderListHtml + painelCounters,
                                                                classe: "row painelLevel02"
                                                                );
            }

            //Se contem  monitoramentos
            if (!string.IsNullOrEmpty(ParLevel2List))
            {
                //Gera agrupamento dw Level2 para o Level1
                ParLevel2List = html.listgroup(
                                                outerhtml: reauditFlag +
                                                           painelLevel2HeaderListHtml +
                                                           null +
                                                           ParLevel2List,
                                                tags: "level01Id=\"" + ParLevel1.Id + "\""
                                               , classe: "level2Group hide");
            }

            return ParLevel2List;
        }

        protected string GetHeaderHtml(IEnumerable<ParLevelHeader> list, SGQDBContext.ParFieldType ParFieldTypeDB, Html html, int ParLevel1_Id = 0, int ParLevel2_Id = 0, ParLevelHeader ParLevelHeaderDB = null, int ParCompany_id = 0, int parLevelDefinition_Id = 1)
        {
            string retorno = "";
            int id = 0;

            #region BotoesDeBusca
            var rotinasIntegracaoXLevel1 = dbEf.ParLevel1XRotinaIntegracao
                .Where(x => x.ParLevel1_Id == ParLevel1_Id
                && x.IsActive
                && x.ParLevelDefinition_Id == parLevelDefinition_Id)
                .Select(x => x.RotinaIntegracao_Id);
            var rotinasIntegracao = dbEf.RotinaIntegracao.Where(x => rotinasIntegracaoXLevel1.Contains(x.Id) && x.IsActive).ToList();

            foreach (var botao in rotinasIntegracao)
            {
                var botoes = $@"<button type=""button"" class=""btn btn-primary"" data-id-rotina=""{ botao.Id }"" 
                                data-headerFields=""{ botao.Parametro }"" onclick=""getRotina(this);"" 
                                data-headerFieldsClean=""{ botao.Retornos }""
                                data-loading-text=""<i class='fa fa-spinner fa-spin'></i> { Resources.Resource.loading }..."">{ botao.Name }</button>";

                retorno += html.div(
                        outerhtml: botoes,
                        classe: "col-xs-6 col-sm-4 col-md-3 col-lg-2",
                        style: "padding-right: 4px !important; padding-left: 4px !important;"
                        );
            }

            if (rotinasIntegracao.Count > 0)
                retorno += "<br><br>";

            #endregion

            foreach (var header in list) //LOOP7
            {

                #region MyRegion

                if (ParLevel1_Id > 0 && ParLevel2_Id > 0 && ParLevelHeaderDB != null)
                {
                    if (ParLevelHeaderDB.isHeaderLeve2Exception(ParLevel1_Id, ParLevel2_Id, header.ParHeaderField_Id))
                    {
                        continue;
                    }
                }

                var duplicar = header.duplicate;

                var duplicaHeader = duplicar ? "  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;  <div style='display: inline-table' hfg=\"" + header.HeaderFieldGroup + "\" onclick='clonarHF(this);'><i class='fa fa-plus' aria-hidden='true'></i></div>     &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;     <div style='display: inline-table' onclick='removerHF(this);'><i class='fa fa-minus' aria-hidden='true'></i></div>" : "";



                var label = "<label class=\"font-small\">" + header.ParHeaderField_Name + "</label>"
                    + duplicaHeader;

                var form_control = "";

                #endregion

                #region Switch com Loop
                //ParFieldType 

                switch (header.ParFieldType_Id)
                {
                    //Multipla Escolha
                    case 1:
                        var listMultiple = ParFieldTypeDB.getMultipleValues(header.ParHeaderField_Id);
                        var optionsMultiple = "";
                        bool hasDefault = false;

                        foreach (var value in listMultiple) //LOOP8
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
                            optionsMultiple = "<option selected=\"selected\" value=\"0\">" + CommonData.getResource("select").Value.ToString() + "...</option>" + optionsMultiple;

                        //form_control = "<select class=\"form-control input-sm\" Id=\"cb" + header.ParHeaderField_Id + "\"  ParHeaderField_Id=\"" + header.ParHeaderField_Id + "\" ParFieldType_Id=\"" + header.ParFieldType_Id + "\" IdPai=\"" + id + "\">" + optionsMultiple + "</select>";

                        form_control = "<select id=\"\" class=\"form-control input-sm ddl\" Id=\"cb" + header.ParHeaderField_Id + "\" name=cb   ParHeaderField_Id=\"" + header.ParHeaderField_Id + "\" ParFieldType_Id=\"" + header.ParFieldType_Id + "\" IdPai=\"" + id + "\" LinkNumberEvaluetion=\"" + header.LinkNumberEvaluetion.ToString().ToLower() + "\"   >" + optionsMultiple + "</select>";
                        form_control += " <label class=\"\"></label>";

                        break;
                    //Integrações
                    case 2:

                        /* Se for produto que digito o código e busco em uma lista*/
                        if (header.ParHeaderField_Description == "Produto")
                        {
                            form_control += " <input id=\"\" class=\"form-control input-sm \" type=\"number\" Id=\"cb" + header.ParHeaderField_Id + "\" ParHeaderField_Id=\"" + header.ParHeaderField_Id + "\" ParFieldType_Id=\"" + header.ParFieldType_Id + "\" oninput=\"buscarProduto(this, $(this).val()); \" onchange=\"validaProduto(this, $(this).val()); \"  >";
                            form_control += " <label class=\"productNamelabel\"></label>";
                            //form_control += "<script>$(\"#cb" + header.ParHeaderField_Id + "\").inputmask('number');</script>";
                        }
                        /* se for um combobox integrado*/
                        else
                        {
                            var listIntegration = ParFieldTypeDB.getIntegrationValues(header.ParHeaderField_Id, header.ParHeaderField_Description, ParCompany_id);
                            var optionsIntegration = "";
                            bool hasDefaultIntegration = false;

                            foreach (var value in listIntegration) //LOOP8
                            {
                                if (value.IsDefaultOption == true)
                                {
                                    optionsIntegration += "<option selected=\"selected\" value=\"" + value.Id + "\" PunishmentValue=\"0\">" + value.Name + "</option>";
                                    hasDefaultIntegration = true;
                                }
                                else
                                {
                                    optionsIntegration += "<option value=\"" + value.Id + "\" PunishmentValue=\"0\">" + value.Name + "</option>";
                                }
                            }
                            if (!hasDefaultIntegration)
                                optionsIntegration = "<option selected=\"selected\" value=\"0\">" + CommonData.getResource("select").Value.ToString() + "...</option>" + optionsIntegration;

                            form_control = "<select id=\"\" class=\"form-control input-sm \" Id=\"cb" + header.ParHeaderField_Id + "\" ParHeaderField_Id=\"" + header.ParHeaderField_Id + "\" ParFieldType_Id=\"" + header.ParFieldType_Id + "\"LinkNumberEvaluetion=\"" + header.LinkNumberEvaluetion.ToString().ToLower() + "\"  >" + optionsIntegration + "</select>";
                        }
                        break;
                    //Binário
                    case 3:
                        var listBinario = ParFieldTypeDB.getMultipleValues(header.ParHeaderField_Id);
                        var optionsBinario = "";
                        foreach (var value in listBinario) //LOOP8
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
                        form_control = "<select id=\"\" class=\"form-control input-sm \" Id=\"cb" + header.ParHeaderField_Id + "\" ParHeaderField_Id='" + header.ParHeaderField_Id + "' ParFieldType_Id = '" + header.ParFieldType_Id + "'LinkNumberEvaluetion=\"" + header.LinkNumberEvaluetion.ToString().ToLower() + "\"  >" + optionsBinario + "</select>";
                        form_control += " <label class=\"\"></label>";
                        break;
                    //Texto
                    case 4:
                        form_control = "<input id=\"\" class=\"form-control input-sm\" type=\"text\" Id=\"cb" + header.ParHeaderField_Id + "\" ParHeaderField_Id=\"" + header.ParHeaderField_Id + "\" ParFieldType_Id=\"" + header.ParFieldType_Id + "\"  >";
                        form_control += " <label class=\"\"></label>";
                        break;
                    //Numérico
                    case 5:
                        form_control = "<input id=\"\" class=\"form-control input-sm numeric\" type=\"text\" Id=\"cb" + header.ParHeaderField_Id + "\" ParHeaderField_Id=\"" + header.ParHeaderField_Id + "\" ParFieldType_Id=\"" + header.ParFieldType_Id + "\"  >";
                        form_control += " <label class=\"\"></label>";
                        break;
                    //Data
                    case 6:
                        form_control = "<input id=\"\" class=\"form-control input-sm \" type=\"date\" Id=\"cb" + header.ParHeaderField_Id + "\" ParHeaderField_Id=\"" + header.ParHeaderField_Id + "\" ParFieldType_Id=\"" + header.ParFieldType_Id + "\"  >";
                        form_control += " <label class=\"\"></label>";
                        break;

                    //Hora
                    case 7:
                        form_control = "<input id=\"\" class=\"form-control input-sm \" type=\"time\" Id=\"cb" + header.ParHeaderField_Id + "\" ParHeaderField_Id=\"" + header.ParHeaderField_Id + "\" ParFieldType_Id=\"" + header.ParFieldType_Id + "\"  >";
                        form_control += " <label class=\"\"></label>";
                        break;
                    //Infomações
                    case 8:
                        form_control = "<br><div id=\"info" + header.ParHeaderField_Id + "\" style=\"display: none;background: RGBA(0,0,0,0.35);position: fixed;z-index: 999999;width: 100%;height: 100%;top: 0;left: 0;\"><div style=\"color: white; font-size: 16px; background: #5353c6;position: fixed; width: 100% ;height: 200px ; margin: 80px 0 0 0; padding: 10px 20px 20px 20px;\"><div style=\"float:right; cursor: pointer;\" class=\"btn btn-default\" onclick='document.getElementById(\"info" + header.ParHeaderField_Id + "\").style.display = \"none\";'>X</div><br><br>" + header.ParHeaderField_Description + "</div></div><button style=\"padding-left: 5px;padding-right: 5px; padding-bottom: 0px; padding-top: 0px;\" onclick='document.getElementById(\"info" + header.ParHeaderField_Id + "\").style.display = \"block\"' class='btn btn-default headerInformacao' ParHeaderField_Id=\"" + header.ParHeaderField_Id + "\"><i class=\"fa fa-info-circle \" aria-hidden=\"true\" style=\"float:right; color:#17175c;font-size: 28px;\" title=\"" + header.ParHeaderField_Description + "\" ></i></button>";
                        form_control += " <label class=\"\"></label>";
                        break;
                    case 9:
                        form_control = $@"<input id="""" class=""form-control input-sm"" type=""text"" Id=""cb{ header.ParHeaderField_Id }"" ParHeaderField_Id=""{ header.ParHeaderField_Id }"" ParFieldType_Id=""{ header.ParFieldType_Id }"" data-param=""{ header.ParHeaderField_Description }"">";
                        form_control += $@"<label class=""""></label>";
                        break;
                    //Dinâmico
                    case 10:
                        form_control = $@"<input id="""" class=""form-control input-sm"" type=""text"" Id=""cb{ header.ParHeaderField_Id }"" ParHeaderField_Id=""{ header.ParHeaderField_Id }"" ParFieldType_Id=""{ header.ParFieldType_Id }"" data-din=""{ header.ParHeaderField_Description }"" readonly>";
                        form_control += $@"<label class=""""></label>";
                        break;
                }

                //Incrementar valor para o pai do elemento para Ytoara.
                id = id + 1;

                var form_group = html.div(
                                            outerhtml: label + form_control,
                                            classe: "form-group header",
                                            tags: header.IsRequired == 1 ? "required" : "",
                                            style: "margin-bottom: 4px;"
                                            );

                retorno += html.div(
                                            outerhtml: form_group,
                                            classe: "col-xs-6 col-sm-4 col-md-3 col-lg-2",
                                            style: "padding-right: 4px !important; padding-left: 4px !important;height:90px !important"
                                            );


                #endregion

            }

            return retorno;
        }

        /// <summary>
        /// Retorna Level3 
        /// </summary>
        /// <param name="ParLevel1"></param>
        /// <param name="ParLevel2"></param>
        /// <returns></returns>
        protected string GetLevel03(SGQDBContext.ParLevel1 ParLevel1, SGQDBContext.ParLevel2 ParLevel2, int ParCompany_Id, DateTime dateCollect, out StringBuilder painellevel3)
        {
            var html = new Html();

            var reauditFlag = "<li class='painel row list-group-item active hide reauditFlag'> Reaudit <span class='reauditnumber'></span></li>";

            //Inicializa ParLevel3
            var ParLevel3DB = new SGQDBContext.ParLevel3(db, quebraProcesso);
            var ParCounterDB = new SGQDBContext.ParCounter(db, quebraProcesso);

            //Inicializa Cabecalhos
            var ParLevelHeaderDB = new SGQDBContext.ParLevelHeader(db);
            //Inicaliza ParFieldType
            var ParFieldTypeDB = new SGQDBContext.ParFieldType(db, conexao);
            //Inicaliza ParLevel1VariableProduction
            var ParLevel1VariableProductionDB = new SGQDBContext.ParLevel1VariableProduction(db);

            //Pega uma lista de parleve3
            //pode colocar par level3 por unidades, como nos eua
            var parlevel3List = ParLevel3DB.getLevel3ByLevel2(ParLevel1, ParLevel2, ParCompany_Id, dateCollect);

            string tipoTela = "";

            var variableList = ParLevel1VariableProductionDB.getVariable(ParLevel1.ParLevel1_Id).ToList();

            var listCounter = ParCounterDB.GetParLevelXParCounterList(null, ParLevel2, 2).ToList();
            listCounter.AddRange(ParCounterDB.GetParLevelXParCounterList(ParLevel1, null, 1).ToList());

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
                #region MyRegion

                //Instancia uma veriavel para gerar o agrupamento
                string parLevel3Group = null;

                foreach (var parLevel3 in parlevel3List) //LOOP4
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
                                    outerhtml: "<label class=\"font-small\" style=\"display:inherit\">" + CommonData.getResource("evaluation").Value.ToString() + " </label><label style=\"display:inline-block; font-size: 20px;\">" + html.span(classe: "evaluateCurrent") + html.span(outerhtml: " / ", classe: "separator") + html.span(classe: "evaluateTotal") + "</label>",
                                    style: "margin-bottom: 4px;",
                                    classe: "form-group");
                string amostrashtml = html.div(
                                    outerhtml: "<label class=\"font-small\" style=\"display:inherit\">" + CommonData.getResource("samples").Value.ToString() + " </label><label style=\"display:inline-block; font-size: 20px;\">" + html.span(classe: "sampleCurrent") + html.span(outerhtml: " / ", classe: "separator") + html.span(classe: "sampleTotal") + "</label>",
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

                //var painelLevel3HeaderListHtml = "";



                var tituloLabel = "Animais Avaliados";

                if (ParLevel1.ParLevel1_Id == 42)
                {
                    tituloLabel = "Total Bloqueado (Kg)";
                }

                var labelPecas = "<label class='font-small'>" + tituloLabel + "</label>";
                var formControlPecas = "<input class='form-control input-sm pecasAvaliadas' type='number'>";
                var formGroupPecas = html.div(
                                        outerhtml: labelPecas + formControlPecas,
                                        classe: "form-group",
                                        style: "margin-bottom: 4px;"
                                        );

                var painelLevel3HeaderListHtml = new StringBuilder(GetHeaderHtml(
                   ParLevelHeaderDB.getHeaderByLevel1Level2(ParLevel1.ParLevel1_Id, ParLevel2.ParLevel2_id), ParFieldTypeDB, html, ParLevel1.ParLevel1_Id, ParLevel2.ParLevel2_id, ParLevelHeaderDB, ParCompany_Id, 2));

                var painelLevel3HeaderListHtml2 = "";
                painelLevel3HeaderListHtml2 += html.div(
                                                outerhtml: formGroupPecas,
                                                classe: "col-xs-6 col-sm-4 col-md-3 col-lg-2",
                                                style: "padding-right: 4px !important; padding-left: 4px !important;"
                                                );

                //string HeaderLevel02 = null;
                painellevel3 = new StringBuilder(html.listgroupItem(
                                                     outerhtml: avaliacoes +
                                                                amostras +
                                                                painelLevel3HeaderListHtml.ToString() +
                                                                painelLevel3HeaderListHtml2,

                                        classe: "painel painelLevel03 row"));
                painellevel3.Append(html.painelCounters(listCounter));
                //          +
                //html.div(outerhtml: "teste", classe: "painel counters row", style: "background-color: #ff0000");

                //Se tiver level3 gera o agrupamento no padrão
                if (!string.IsNullOrEmpty(parLevel3Group))
                {
                    parLevel3Group = html.div(
                                               classe: "level3Group BEA",
                                               tags: "level1id=\"" + ParLevel1.Id + "\" level2id=\"" + ParLevel2.Id + "\"",

                                               outerhtml: painellevel3.ToString() +
                                                          parLevel3Group
                                             );
                }
                return parLevel3Group;

                #endregion
            }
            //Tela da verificação da tipificação
            else if (tipoTela.Equals("VF"))
            {
                #region MyRegion
                //Inicaliza CaracteristicaTipificacao
                var CaracteristicaTipificacaoDB = new SGQDBContext.CaracteristicaTipificacao(SGQ_GlobalADO);
                //Inicaliza VerificacaoTipificacaoTarefaIntegracao
                var VerificacaoTipificacaoTarefaIntegracaoDB = new SGQDBContext.VerificacaoTipificacaoTarefaIntegracao(SGQ_GlobalADO);

                //Instancia uma veriavel para gerar o agrupamento
                string parLevel3Group = null;

                foreach (var parLevel3 in parlevel3List) // //LOOP4
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

                        #region Switch parLevel3.Name

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

                        #endregion

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

                foreach (var area in listAreasParticipantes) //LOOP5
                {
                    items += "<div class='col-xs-3 hide' cNmCaracteristica='" + area.cNmCaracteristica + "' cIdentificador='" + area.cIdentificador + "' " +
                            " cNrCaracteristica='" + area.cNrCaracteristica + "' cSgCaracteristica='" + area.cSgCaracteristica + "'>" +
                            area.cNmCaracteristica + "</div>";
                }

                var CtIdAP = "";
                if (CaracteristicaTipificacaoDB.getAreasParticipantesUnico().First().nCdCaracteristica != null)
                    CtIdAP = CaracteristicaTipificacaoDB.getAreasParticipantesUnico().First().nCdCaracteristica;
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


                //var formControlBanda = "<input class='form-control input-sm banda' min='1' max='2' style='font-size:30px; height: 50px; text-align:center;' type='number'>";
                var formControlBanda = "<select class='form-control input-sm banda' style='font-size:30px; height: 50px; text-align:center;'><option value='1'>1</option><option value='2'>2</option></select>";

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
                                    outerhtml: "<label class=\"font-small\" style=\"display:inherit\">" + CommonData.getResource("evaluation").Value.ToString() + " </label><label style=\"display:inline-block; font-size: 20px;\">" + html.span(classe: "evaluateCurrent") + html.span(outerhtml: " / ", classe: "separator") + html.span(classe: "evaluateTotal") + "</label>",
                                    style: "margin-bottom: 4px;",
                                    classe: "form-group");
                string amostrashtml = html.div(
                                    outerhtml: "<label class=\"font-small\" style=\"display:inherit\">" + CommonData.getResource("samples").Value.ToString() + " </label><label style=\"display:inline-block; font-size: 20px;\">" + html.span(classe: "sampleCurrent") + html.span(outerhtml: " / ", classe: "separator") + html.span(classe: "sampleTotal") + "</label>",
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

                painellevel3 = new StringBuilder(html.listgroupItem(
                                                            outerhtml: avaliacoes +
                                                                       amostras +
                                                                       painelLevel3HeaderListHtml,

                                               classe: "painel painelLevel03 row"));

                painellevel3.Append(html.painelCounters(listCounter));
                //+
                //                html.div(outerhtml: "teste", classe: "painel counters row", style: "background-color: #ff0000");

                //Se tiver level3 gera o agrupamento no padrão
                if (!string.IsNullOrEmpty(parLevel3Group) && ParLevel1.HasGroupLevel2 != true)
                {
                    parLevel3Group = html.div(
                                               classe: "level3Group VF",
                                               tags: "level1id=\"" + ParLevel1.Id + "\" level2id=\"" + ParLevel2.Id + "\"",

                                                   outerhtml: painellevel3.ToString() +
                                                              parLevel3Group
                                                 );
                }
                return parLevel3Group;
                #endregion
            }
            //Tela do PCC1B
            else if (tipoTela.Equals("PCC1B"))
            {
                #region MyRegion
                //Instancia uma veriavel para gerar o agrupamento
                string parLevel3Group = null;

                foreach (var parLevel3 in parlevel3List) //LOOP4
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
                                                classe: "col-xs-6 col-sm-6 col-md-6 col-lg-6",
                                                style: "padding-right: 4px !important; padding-left: 4px !important;"
                                                );

                painelLevel3HeaderListHtml += html.div(
                                                outerhtml: formGroupBanda,
                                                classe: "col-xs-4 col-sm-2 col-md-2 col-lg-2",
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
                                    outerhtml: "<label class=\"font-small\" style=\"display:inherit\">" + CommonData.getResource("evaluation").Value.ToString() + " </label><label style=\"display:inline-block; font-size: 20px;\">" + html.span(classe: "evaluateCurrent") + html.span(outerhtml: " / ", classe: "separator") + html.span(classe: "evaluateTotal") + "</label>",
                                    style: "margin-bottom: 4px;",
                                    classe: "form-group");
                string amostrashtml = html.div(
                                    outerhtml: "<label class=\"font-small\" style=\"display:inherit\">" + CommonData.getResource("samples").Value.ToString() + " </label><label style=\"display:inline-block; font-size: 20px;\">" + html.span(classe: "sampleCurrent") + html.span(outerhtml: " / ", classe: "separator") + html.span(classe: "sampleTotal") + "</label>",
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

                painellevel3 = new StringBuilder(html.listgroupItem(
                                                            outerhtml: amostras + avaliacoes + totalnc + ncdianteiro + nctraseiro + niveis + painelLevel3HeaderListHtml,
                                               classe: "painel painelLevel03 row"));

                painellevel3.Append(html.painelCounters(listCounter));
                //+
                //                  +
                //html.div(outerhtml: "teste", classe: "painel counters row", style: "background-color: #ff0000");

                //Se tiver level3 gera o agrupamento no padrão
                if (!string.IsNullOrEmpty(parLevel3Group))
                {
                    parLevel3Group = html.div(
                                               classe: "level3Group PCC1B",
                                               tags: "level1id=\"" + ParLevel1.Id + "\" level2id=\"" + ParLevel2.Id + "\"",

                                               outerhtml: painellevel3.ToString() +
                                                          parLevel3Group
                                             );
                }
                return parLevel3Group;
                #endregion
            }
            //Tela Genérica
            else
            {
                //Instancia uma veriavel para gerar o agrupamento
                StringBuilder parLevel3Group = new StringBuilder();

                //retornar apena a primeira tarefa de cada id

                var parlevel3GroupByLevel2 = parlevel3List.GroupBy(p => p.ParLevel3Group_Id);
                var listaIdParLevel3 = new List<int>();

                foreach (var parLevel3GroupLevel2 in parlevel3GroupByLevel2)//LOOP4
                {
                    string accordeonName = null;
                    string acoordeonId = null;
                    StringBuilder level3Group = new StringBuilder();

                    foreach (var parLevel3 in parLevel3GroupLevel2)//LOOP5
                    {

                        if (!listaIdParLevel3.Contains(parLevel3.Id))
                        {
                            listaIdParLevel3.Add(parLevel3.Id);

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
                            level3Group.Append(level3List);
                        }
                    }

                    if (!string.IsNullOrEmpty(acoordeonId))
                    {
                        haveAccordeon = true;
                        level3Group = new StringBuilder(html.accordeon(
                                                        id: acoordeonId + "Level3",
                                                        label: accordeonName,
                                                        outerhtml: level3Group.ToString(),
                                                        classe: "row"
                                                    ));
                    }

                    //*inserir contador
                    string painelCounters = "";
                    if (listCounter != null)
                    {
                        painelCounters = html.painelCounters(listCounter.Where(r => r.Local == "level3_header"));
                    }

                    parLevel3Group.Append(level3Group);

                }

                //< div class="form-group">
                //      <label for="email" style="
                //    display: inherit;
                //">Email:</label>
                //      <label for="email" style="display: inline-block">Email:</label>
                //    </div>

                //Avaliações e amostas para painel
                string avaliacoeshtml = html.div(
                                    outerhtml: "<label class=\"font-small\" style=\"display:inherit\">" + CommonData.getResource("evaluation").Value.ToString() + " </label><label style=\"display:inline-block; font-size: 20px;\">" + html.span(classe: "evaluateCurrent") + html.span(outerhtml: " / ", classe: "separator") + html.span(classe: "evaluateTotal") + "</label>",
                                    style: "margin-bottom: 4px;",
                                    classe: "form-group");
                string amostrashtml = html.div(
                                    outerhtml: "<label class=\"font-small\" style=\"display:inherit\">" + CommonData.getResource("samples").Value.ToString() + " </label><label style=\"display:inline-block; font-size: 20px;\">" + html.span(classe: "sampleCurrent") + html.span(outerhtml: " / ", classe: "separator") + html.span(classe: "sampleTotal") + "</label>",
                                    style: "margin-bottom: 4px;",
                                    classe: "form-group");
                string defeitoshtml = html.div(
                                    outerhtml: "<label class=\"font-small\" style=\"display:inherit\">" + CommonData.getResource("defects").Value.ToString() +
                                    "</label><label style=\"display:inline-block; font-size: 20px;\">" + html.span(outerhtml: "0", classe: "defects") + "</label>",
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
                string defeitos = html.div(
                                    outerhtml: defeitoshtml,
                                    style: "padding-right: 4px !important; padding-left: 4px !important;",
                                    classe: "col-xs-6 col-sm-4 col-md-3 col-lg-2");

                //Painel
                //O interessante é um painel só mas no momento está um painel para cada level3group

                var painelLevel3HeaderListHtml = new StringBuilder(GetHeaderHtml(
                    ParLevelHeaderDB.getHeaderByLevel1Level2(ParLevel1.ParLevel1_Id, ParLevel2.ParLevel2_id), ParFieldTypeDB, html, ParLevel1.ParLevel1_Id, ParLevel2.ParLevel2_id, ParLevelHeaderDB, ParCompany_Id, 2));

                //string HeaderLevel02 = null;

                string accordeonbuttons = null;
                if (haveAccordeon == true)
                {
                    accordeonbuttons = "<button class=\"btn btn-default button-expand marginRight10\"><i class=\"fa fa-expand\" aria-hidden=\"true\"></i> " + Resources.Resource.show_all + " </button>" +
                                       "<button class=\"btn btn-default button-collapse\"><i class=\"fa fa-compress\" aria-hidden=\"true\"></i> " + Resources.Resource.hide_all + " </button>";
                }

                // incluir coluna e obter o total de amostras com defeito agrupado.
                var level2 = dbEf.ParCounterXLocal.FirstOrDefault(r => r.ParLevel1_Id == ParLevel1.ParLevel1_Id && r.ParCounter.Name == "defects" && r.IsActive);
                if (level2 != null)
                {
                    var teste = new ContadoresXX().GetContadoresXX(dbEf, ParLevel1.ParLevel1_Id, ParCompany_Id);

                    //MOCK
                    var listaShift = new List<int>();
                    listaShift.Add(1);
                    listaShift.Add(2);
                    var listaPeriod = new List<int>();
                    listaPeriod.Add(1);
                    listaPeriod.Add(2);
                    listaPeriod.Add(3);
                    listaPeriod.Add(4);

                    if (teste.IsNotNull() && teste.Count > 0)
                    {
                        if ((GlobalConfig.Eua || GlobalConfig.Canada) && ParLevel1.ParLevel1_Id == 55)
                        {

                            foreach (var s in listaShift)
                            {
                                foreach (var p in listaPeriod)
                                {
                                    painelLevel3HeaderListHtml.Append("<div style='display: none;' level1TdefId=" + ParLevel1.Id + " id='tdefPeriod" + p + "Shif" + s + "level1TdefId" + ParLevel1.Id + "'>" + CommonData.getResource("total_defects").Value.ToString() + ": <span>0</span></div>");
                                    //painelLevel3HeaderListHtml += "<div style='display: none;' level1TdefId=" + ParLevel1.Id + " id='tdefPeriod" + p + "Shif" + s + "level1TdefId" + ParLevel1.Id + "'>" + CommonData.getResource("total_defects_sample").Value.ToString() + ": <span>" + teste.LastOrDefault(r=>r.Period == p && r.Shift == s)?.WeiDefects.ToString("G29") + "</span></div>";
                                    painelLevel3HeaderListHtml.Append("<div style='display: none;' level1TdefId=" + ParLevel1.Id + " id='tdefPeriod" + p + "Shif" + s + "level1TdefId" + ParLevel1.Id + "'>" + CommonData.getResource("total_defects_sample").Value.ToString() + ": <span>" + teste.Where(r => r.Period == p && r.Shift == s).Sum(r => r.WeiDefects).ToString("G29") + "</span></div>");

                                    if ((GlobalConfig.Eua || GlobalConfig.Canada) && ParLevel1.ParLevel1_Id == 55)
                                        painelLevel3HeaderListHtml.Append("<div style='display: none;' level1TdefId=" + ParLevel1.Id + " id='tdefPeriod" + p + "Shif" + s + "level1TdefId" + ParLevel1.Id + "'>" + CommonData.getResource("three_more_defects").Value.ToString() + ": <span>0</span></div>");

                                }

                            }
                        }
                    }
                    else
                    {
                        if ((GlobalConfig.Eua || GlobalConfig.Canada) && ParLevel1.ParLevel1_Id == 55)
                        {
                            foreach (var s in listaShift)
                            {
                                foreach (var p in listaPeriod)
                                {
                                    painelLevel3HeaderListHtml.Append("<div style='display: none;' level1TdefId=" + ParLevel1.Id + " id='tdefPeriod" + p + "Shif" + s + "level1TdefId" + ParLevel1.Id + "'>" + CommonData.getResource("total_defects").Value.ToString() + ": <span>0</span></div>");
                                    painelLevel3HeaderListHtml.Append("<div style='display: none;' level1TdefId=" + ParLevel1.Id + " id='tdefPeriod" + p + "Shif" + s + "level1TdefId" + ParLevel1.Id + "'>" + CommonData.getResource("total_defects_sample").Value.ToString() + ": <span>0</span></div>");

                                    if ((GlobalConfig.Eua || GlobalConfig.Canada) && ParLevel1.ParLevel1_Id == 55)
                                        painelLevel3HeaderListHtml.Append("<div style='display: none;' level1TdefId=" + ParLevel1.Id + " id='tdefPeriod" + p + "Shif" + s + "level1TdefId" + ParLevel1.Id + "'>" + CommonData.getResource("three_more_defects").Value.ToString() + ": <span>0</span></div>");
                                }

                            }
                        }
                    }
                }

                painellevel3 = new StringBuilder(html.listgroupItem(outerhtml: avaliacoes +
                                                             amostras +
                                                             defeitos +
                                                             painelLevel3HeaderListHtml.ToString(),
                                                  classe: "painel painelLevel03 row"));
                painellevel3.Append(html.painelCounters(listCounter));

                //html.div(outerhtml: "teste", classe: "painel counters row", style: "background-color: #ff0000");

                var botoesTodos = "";
                if (GlobalConfig.Brasil)
                {
                    botoesTodos = "<button id='btnAllNA' class='btn btn-warning btn-sm pull-right'> Todos N/A </button>" +
                                    "<button id='btnAllNC' class='btn btn-danger btn-sm pull-right' style='margin-right: 10px;'> Clicar em Todos </button>";
                }

                string panelButton = html.listgroupItem(
                                                        outerhtml: botoesTodos + accordeonbuttons,
                                                        classe: "painel row"
                                                    );

                //Se tiver level3 gera o agrupamento no padrão
                if (!string.IsNullOrEmpty(parLevel3Group.ToString()) && ParLevel1.HasGroupLevel2 != true)
                {
                    parLevel3Group = new StringBuilder(html.div(
                                               classe: "level3Group",
                                               tags: "level1id=\"" + ParLevel1.Id + "\" level2id=\"" + ParLevel2.Id + "\"",
                                               outerhtml: reauditFlag +
                                                          painellevel3.ToString() + panelButton +
                                                          parLevel3Group.ToString()
                                             ));
                }
                return parLevel3Group.ToString();
            }


        }

        /// <summary>
        /// Gera o input para level3
        /// </summary>
        /// <param name="parLevel3">ParLevel3</param>
        /// <param name="classInput">Classe de Input</param>
        /// <param name="labels">Labels do Input</param>
        /// <returns></returns>
        protected string getTipoInput(SGQDBContext.ParLevel3 parLevel3, ref string classInput, ref string labels)
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
                           classe: "levelName",
                           //style: "margin-top:7px;"
                           style: "visibility: hidden;"
                         );

                input = html.campoNumeroDeDefeitos(id: parLevel3.Id.ToString(),
                                                intervalMin: parLevel3.IntervalMin,
                                                intervalMax: parLevel3.IntervalMax,
                                                unitName: parLevel3.ParMeasurementUnit_Name);
            }
            else if (parLevel3.ParLevel3InputType_Id == 3 || parLevel3.ParLevel3InputType_Id == 9)
            {
                classInput = " interval";

                string valorMinimo = parLevel3.IntervalMin.ToString("G29") == "-9999999999999,9" ? "" : "<b>Min: </b>" + parLevel3.IntervalMin.ToString("G29");
                string valorMaximo = parLevel3.IntervalMax.ToString("G29") == "9999999999999,9" ? "" : " <b>Max: </b>" + parLevel3.IntervalMax.ToString("G29");

                string valorCompleto = "";

                if (valorMinimo == "")
                {
                    valorCompleto = valorMaximo;
                }
                else if (valorMaximo == "")
                {
                    valorCompleto = valorMinimo;
                }
                else
                {
                    valorCompleto = valorMinimo + " ~ " + valorMaximo;
                }


                labels = html.div(



                                            outerhtml: valorCompleto + " " + parLevel3.ParMeasurementUnit_Name,
                                           classe: "levelName"
                                       //style: "margin-top:7px;"
                                       );

                if (parLevel3.ParLevel3InputType_Id == 3)
                {

                    input = html.campoIntervalo(id: parLevel3.Id.ToString(),
                                                intervalMin: parLevel3.IntervalMin,
                                                intervalMax: parLevel3.IntervalMax,
                                                unitName: parLevel3.ParMeasurementUnit_Name);

                }
                else if (parLevel3.ParLevel3InputType_Id == 9)
                {

                    input = html.campoIntervaloTexto(id: parLevel3.Id.ToString(),
                                                    intervalMin: parLevel3.IntervalMin,
                                                    intervalMax: parLevel3.IntervalMax,
                                                    unitName: parLevel3.ParMeasurementUnit_Name);

                }
            }

            else if (parLevel3.ParLevel3InputType_Id == 4)
            {
                classInput = " calculado";

                string valorMinimo = parLevel3.IntervalMin.ToString("G29") == "-9999999999999,9" ? "" : "<b>Min: </b>" + Guard.ConverteValorCalculado(parLevel3.IntervalMin);
                string valorMaximo = parLevel3.IntervalMax.ToString("G29") == "9999999999999,9" ? "" : " <b>Max: </b>" + Guard.ConverteValorCalculado(parLevel3.IntervalMax);

                string valorCompleto = "";

                if (valorMinimo == "")
                {
                    valorCompleto = valorMaximo;
                }
                else if (valorMaximo == "")
                {
                    valorCompleto = valorMinimo;
                }
                else
                {
                    valorCompleto = valorMinimo + " ~ " + valorMaximo;
                }

                //var intervalMin = Guard.ConverteValorCalculado(parLevel3.IntervalMin);
                //var intervalMax = Guard.ConverteValorCalculado(parLevel3.IntervalMax);

                labels = html.div(



                                            outerhtml: valorCompleto + " " + parLevel3.ParMeasurementUnit_Name,
                                           classe: "levelName"
                                       //style: "margin-top:7px;"
                                       );

                //labels = html.div(
                //                           outerhtml: "<b>Min: </b> " + Guard.ConverteValorCalculado(parLevel3.IntervalMin) + " ~ <b>Max: </b>" + Guard.ConverteValorCalculado(parLevel3.IntervalMax) + " " + parLevel3.ParMeasurementUnit_Name,
                //                           classe: "levelName"
                //                       //style: "margin-top:7px;"
                //                       );

                input = html.campoCalculado(id: parLevel3.Id.ToString(),
                                                intervalMin: parLevel3.IntervalMin,
                                                intervalMax: parLevel3.IntervalMax,
                                                unitName: parLevel3.ParMeasurementUnit_Name);
            }
            else if (parLevel3.ParLevel3InputType_Id == 5)
            {
                classInput = " texto naoValidarInput";
                labels = html.div(
                                           outerhtml: "",
                                           classe: "levelName"
                                       //style: "margin-top:7px;"
                                       );

                input = html.campoTexto(id: parLevel3.Id.ToString());
            }//Binário com texto
            else if (parLevel3.ParLevel3InputType_Id == 6)
            {
                classInput = " boolean";
                labels = html.campoTextoBinario(id: parLevel3.Id.ToString());
                input = html.campoBinario(parLevel3.Id.ToString(), parLevel3.ParLevel3BoolTrue_Name, parLevel3.ParLevel3BoolFalse_Name);
            }//Intervalo em minutos
            else if (parLevel3.ParLevel3InputType_Id == 7)
            {
                //input = html.campoTextoMinutos(id: parLevel3.Id.ToString());

                classInput = " interval";

                string valorMinimo = parLevel3.IntervalMin.ToString("G29") == "-9999999999999,9" ? "" : "<b>Min: </b>" + parLevel3.IntervalMin.ToString("G29");
                string valorMaximo = parLevel3.IntervalMax.ToString("G29") == "9999999999999,9" ? "" : " <b>Max: </b>" + parLevel3.IntervalMax.ToString("G29");

                string valorCompleto = "";

                if (valorMinimo == "")
                {
                    valorCompleto = valorMaximo;
                }
                else if (valorMaximo == "")
                {
                    valorCompleto = valorMinimo;
                }
                else
                {
                    valorCompleto = valorMinimo + " ~ " + valorMaximo;
                }


                labels = html.div(



                                            outerhtml: valorCompleto + " " + Resources.Resource.minutes_initials,
                                           classe: "levelName"
                                       //style: "margin-top:7px;"
                                       );

                //input = html.campoIntervalo(id: parLevel3.Id.ToString(),
                //                                intervalMin: parLevel3.IntervalMin,
                //                                intervalMax: parLevel3.IntervalMax,
                //                                unitName: parLevel3.ParMeasurementUnit_Name);

                input = html.campoTextoMinutos(id: parLevel3.Id.ToString(),
                                                intervalMin: parLevel3.IntervalMin,
                                                intervalMax: parLevel3.IntervalMax,
                                                unitName: parLevel3.ParMeasurementUnit_Name);

            }//Escala Likert
            else if (parLevel3.ParLevel3InputType_Id == 8)
            {
                var ranges = dbEf.ParInputTypeValues.Where(r => r.ParLevel3Value_Id == parLevel3.ParLevel3Value_Id && r.IsActive && (r.Intervalo <= parLevel3.IntervalMax && r.Intervalo >= parLevel3.IntervalMin)).ToList();

                var paramns = new List<string>();

                foreach (var item in ranges)
                {
                    paramns.Add(item.Intervalo + ":" + item.Cor + ":" + item.Valor);
                }
                input = html.campoRangeSlider(parLevel3.Id.ToString(), parLevel3.IntervalMin, parLevel3.IntervalMax, null, "valor_range_" + parLevel3.Id.ToString(), string.Join("|", paramns));

                //INSERE O MIN MAX
                string valorMinimo = parLevel3.IntervalMin.ToString("G29") == "-9999999999999,9" ? "" : parLevel3.IntervalMin.ToString("G29");
                string valorMaximo = parLevel3.IntervalMax.ToString("G29") == "9999999999999,9" ? "" : parLevel3.IntervalMax.ToString("G29");

                string valorCompleto = "";

                valorCompleto = "<strong>Escalas: </strong>" + valorMinimo + " a " + valorMaximo;

                labels = html.div(outerhtml: valorCompleto, classe: "levelName");
            }//Resultado
            else if (parLevel3.ParLevel3InputType_Id == 10)
            {
                classInput = " interval";

                string valorMinimo = parLevel3.IntervalMin.ToString("G29") == "-9999999999999,9" ? "" : "<b>Min: </b>" + parLevel3.IntervalMin.ToString("G29");
                string valorMaximo = parLevel3.IntervalMax.ToString("G29") == "9999999999999,9" ? "" : " <b>Max: </b>" + parLevel3.IntervalMax.ToString("G29");

                string valorCompleto = "";

                if (valorMinimo == "")
                {
                    valorCompleto = valorMaximo;
                }
                else if (valorMaximo == "")
                {
                    valorCompleto = valorMinimo;
                }
                else
                {
                    valorCompleto = valorMinimo + " ~ " + valorMaximo;
                }


                labels = html.div(



                                            outerhtml: valorCompleto + " " + parLevel3.ParMeasurementUnit_Name,
                                           classe: "levelName"
                                       //style: "margin-top:7px;"
                                       );

                input = html.campoResultado(parLevel3.Id.ToString(), parLevel3.DynamicValue);
            }
            else if (parLevel3.ParLevel3InputType_Id == 11)
            {
                classInput = " texto naoValidarInput";
                labels = html.div(
                                           outerhtml: "",
                                           classe: "levelName"
                                       //style: "margin-top:7px;"
                                       );

                input = html.campoTexto(id: parLevel3.Id.ToString(), classe: classInput);
            }
            else
            {
                ///Campo interval está repetindo , falta o campo defeitos
                classInput = " interval";

                string valorMinimo = parLevel3.IntervalMin.ToString("G29") == "-9999999999999,9" ? "" : "<b>Min: </b>" + parLevel3.IntervalMin.ToString("G29");
                string valorMaximo = parLevel3.IntervalMax.ToString("G29") == "9999999999999,9" ? "" : "<b>Max: </b>" + parLevel3.IntervalMax.ToString("G29");

                string valorCompleto = "";

                if (valorMinimo == "")
                {
                    valorCompleto = valorMaximo;
                }
                else if (valorMaximo == "")
                {
                    valorCompleto = valorMinimo;
                }
                else
                {
                    valorCompleto = valorMinimo + " ~ " + valorMaximo;
                }

                labels = html.div(
                                    outerhtml: valorCompleto + " " + parLevel3.ParMeasurementUnit_Name,  //"<b>Min: </b>" + parLevel3.IntervalMin.ToString("G29") + " ~ <b>Max: </b>" + parLevel3.IntervalMax.ToString("G29") + " " + parLevel3.ParMeasurementUnit_Name,
                                    classe: "levelName"
                                //style: "margin-top:7px;"
                                );

                input = html.campoIntervalo(id: parLevel3.Id.ToString(),
                                                intervalMin: parLevel3.IntervalMin,
                                                intervalMax: parLevel3.IntervalMax,
                                                unitName: parLevel3.ParMeasurementUnit_Name);
            }
            return input;
        }

        protected string getTipoInputBEA(SGQDBContext.ParLevel3 parLevel3, ref string classInput, ref string labels)
        {
            var html = new Html();
            string input = null;
            classInput = " defects";
            labels = html.div(
                                   //classe: "font10"
                                   //style: "margin-top:7px;"
                                   );

            input = html.campoNumeroDeDefeitos(id: parLevel3.Id.ToString(),
                                            intervalMin: parLevel3.IntervalMin,
                                            intervalMax: parLevel3.IntervalMax,
                                            unitName: parLevel3.ParMeasurementUnit_Name);
            return input;
        }

        public string GetLoginAPP()
        {
            var html = new Html();
            string head = html.div(classe: "head", style: "background-image: url(" + DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.systemLogo + ");");

            //Verifica as configurações iniciais da tela
            var ParConfSGQDB = new SGQDBContext.ParConfSGQContext(db);
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
                selectShit = html.option("0", CommonData.getResource("select_the_shift").Value.ToString()) +
                              html.option("1", CommonData.getResource("shift_a").Value.ToString()) +
                              html.option("2", CommonData.getResource("shift_b").Value.ToString());

                selectShit = html.select(selectShit, id: "shift");
            }
            #endregion
            //                          html.option("http://mtzsvmqsc/SgqGlobal", "JBS") +
            //                          html.option("http://192.168.25.200/SgqMaster", "GRT") +
            //                          html.option("http://localhost:8090/SgqSystem", "GCN");

            string formOuterHtml = html.head(Html.h.h2, outerhtml: CommonData.getResource("login").Value.ToString()) +
                                  selectUnit +
                                  selectShit +
                                  html.label(labelfor: "inputUserName", classe: "sr-only", outerhtml: CommonData.getResource("username").Value.ToString()) +
                                  html.input(id: "inputUserName", placeholder: CommonData.getResource("username").Value.ToString(), required: true, disabled: inputsDesabilitados) +
                                  html.label(labelfor: "inputPassword", classe: "sr-only", outerhtml: CommonData.getResource("password").Value.ToString()) +
                                  html.input(type: Html.type.password, id: "inputPassword", placeholder: CommonData.getResource("password").Value.ToString(), required: true, disabled: inputsDesabilitados);
            if (GlobalConfig.Brasil == true && GlobalConfig.Ytoara == false && GlobalConfig.Guarani == false)
            {
                formOuterHtml +=
                    html.button(label: CommonData.getResource("enter_offline").Value.ToString(), id: "btnLoginOffline", classe: "btn-lg btn-primary btn-block marginTop10", dataloading: "<i class='fa fa-spinner fa-spin'></i> <span class='wMessage' style='font-size:14px;'>" + CommonData.getResource("authenticating").Value.ToString() + "</span>") +
                    html.button(label: CommonData.getResource("enter_online").Value.ToString(), id: "btnLoginOnline", classe: "btn-lg btn-default btn-sm btn-block marginTop10", dataloading: "<i class='fa fa-spinner fa-spin'></i> <span class='wMessage' style='font-size:14px;'>" + CommonData.getResource("authenticating").Value.ToString() + "</span>");

            }
            else
            {
                formOuterHtml +=
                    html.button(label: CommonData.getResource("enter").Value.ToString(), id: "btnLoginOnline", classe: "btn-lg btn-primary btn-block marginTop10", dataloading: "<i class='fa fa-spinner fa-spin'></i> <span class='wMessage' style='font-size:14px;'>" + CommonData.getResource("authenticating").Value.ToString() + "</span>") +
                    html.button(label: CommonData.getResource("enter_offline").Value.ToString(), id: "btnLoginOffline", classe: "btn-lg btn-primary btn-block hide marginTop10", dataloading: "<i class='fa fa-spinner fa-spin'></i> <span class='wMessage' style='font-size:14px;'>" + CommonData.getResource("authenticating").Value.ToString() + "</span>");
            }

            formOuterHtml +=
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
                                           outerhtml: html.span(id: "mensagemSucesso", classe: "icon-ok-circle"));

            //html.select(selectUrlPreffix, "cb_UrlPreffix", "\" onChange='abreOApp(this.value);' \"");

            string form = html.form(
                                    outerhtml: formOuterHtml
                                    , classe: "form-signin");

            #endregion

            string divChangeServer =
                html.div(style: "max-width:320px; margin: 0 auto; padding-right:15px; padding-left:15px",
                outerhtml: html.button(label: "Atualizar o APP", id: "btnChangeHost", classe: "btn-lg btn-default btn-sm btn-block"));

            #region foot

            string local = "";
            string empresa = "";

            if (GlobalConfig.Brasil)
            {
                local = "brasil";
            }
            if (GlobalConfig.Eua)
            {
                local = "eua";
            }
            if (GlobalConfig.Canada)
            {
                local = "canada";
            }
            if (GlobalConfig.Guarani)
            {
                empresa = "guarani";
            }
            if (GlobalConfig.Brasil)
            {
                empresa = "jbs";
            }
            if (GlobalConfig.Ytoara)
            {
                empresa = "ytoara";
            }
            if (GlobalConfig.Santander)
            {
                empresa = "santander";
            }

            string footOuterHtml = html.br() +
                                   html.br() +
                                   html.br() +
                                   html.span(classe: "hide", id: "local", attr: " empresa='" + empresa + "' local='" + local + "'") +
                                   html.span(
                                              outerhtml: CommonData.getResource("version").Value.ToString() +
                                                         html.span(classe: "number")
                                             , id: "versionLogin") +
                                   html.span(
                                               outerhtml: html.span(classe: "base")
                                             , id: "ambienteLogin"

                                            );

            string foot = html.div(
                                    outerhtml: footOuterHtml
                                    , classe: "foot", style: "text-align:center;background-image: url(" + DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.systemLogoFooter + ");");

            #endregion

            return html.div(
                outerhtml: head +
                form +
                divChangeServer +
                foot,
                classe: "login"
                );
        }

        #region Users

        public string getCompanyUsers(string ParCompany_Id)
        {
            var ParCompanyXUserSgqDB = new SGQDBContext.ParCompanyXUserSgq(db);
            var RolesXUserSgqDB = new SGQDBContext.RoleXUserSgq(db);

            var users = ParCompanyXUserSgqDB.getCompanyUsers(Convert.ToInt32(ParCompany_Id));
            var html = new Html();

            string usersList = null;
            foreach (var user in users)
            {
                string Password = user.UserSGQ_Pass;
                //Password = Guard.Descriptografar3DES(Password);
                //Password = Guard.EncryptStringAES(Password);

                var roles = RolesXUserSgqDB.getRoles(Convert.ToInt32(user.UserSGQ_Id), Convert.ToInt32(ParCompany_Id));

                usersList += html.user(user.UserSGQ_Id, user.UserSGQ_Name, user.UserSGQ_Login, Password, user.Role, user.ParCompany_Id, user.ParCompany_Name, roles);

            }
            return usersList;
        }

        public string UserSGQById(int Id)
        {
            var UserSGQDB = new SGQDBContext.UserSGQ(db);
            var user = UserSGQDB.getUserByLoginOrId(id: Id);

            var html = new Html();
            if (user != null)
            {
                string Password = user.Password;//Guard.Criptografar3DES(user.Password);
                                                //Password = Guard.EncryptStringAES(Password);

                return html.user(user.Id, user.Name, user.Login, Password, user.Role, user.ParCompany_Id, user.ParCompany_Name, null);
            }

            return "Usuário não localizado";
        }

        #endregion

        public string insertDeviation(InsertDeviationClass insertDeviationClass)
        {
            string deviations = insertDeviationClass.Deviations;

            if (string.IsNullOrEmpty(deviations))
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


            List<SqlCommand> sql = new List<SqlCommand>();
            for (int i = 0; i < arrayDeviations.Length; i++)
            {
                string[] deviation = arrayDeviations[i].Split(';');

                string ParCompany_Id = deviation[0];
                string ParLevel1_Id = deviation[1].Contains(quebraProcesso) ? deviation[1].Replace(quebraProcesso, "|").Split('|')[1] : deviation[1];
                string ParLevel2_Id = deviation[2].Contains(quebraProcesso) ? deviation[2].Replace(quebraProcesso, "|").Split('|')[1] : deviation[2];
                string Evaluation = deviation[3] == "" ? "0" : deviation[3];

                if (Evaluation == "undefined")
                {
                    Evaluation = "0";
                }

                string Sample = deviation[4] == "" ? "0" : deviation[4];

                if (Sample == "undefined")
                {
                    Sample = "0";
                }
                string alertNumber = deviation[5];
                string defects = deviation[6];
                string deviationDate = deviation[7];
                string deviationMessage = deviation[8];
                if (string.IsNullOrEmpty(deviationMessage))
                {
                    deviationMessage = "null";
                }
                else
                {
                    deviationMessage = "'" + deviationMessage + "'";
                }

                var dt = DateCollectConvert(deviationDate);

                if (VerificaStringNulaUndefinedNaN(defects))
                    defects = "0";

                var query = $@"INSERT INTO Deviation ([ParCompany_Id],
                    [ParLevel1_Id],
                    [ParLevel2_Id],
                    [Evaluation],
                    [Sample],
                    [AlertNumber],
                    [Defects],
                    [DeviationDate],
                    [AddDate],
                    [sendMail],
                    [DeviationMessage]) 
                    VALUES 
                    (@ParCompany_Id,
                    @ParLevel1_Id,
                    @ParLevel2_Id,
                    @Evaluation,
                    @Sample,
                    @AlertNumber ,
                    @Defects,
                    @Dt,
                    GetDate(), 
                    0,
                    @DeviationMessage)";

                string conexao = this.conexao;

                using (SqlCommand cmd = new SqlCommand(query, new SqlConnection(conexao)))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add(new SqlParameter("@ParCompany_Id", ParCompany_Id));
                    cmd.Parameters.Add(new SqlParameter("@ParLevel1_Id", ParLevel1_Id));
                    cmd.Parameters.Add(new SqlParameter("@ParLevel2_Id", ParLevel2_Id));
                    cmd.Parameters.Add(new SqlParameter("@Evaluation", Evaluation));
                    cmd.Parameters.Add(new SqlParameter("@Sample", Sample));
                    cmd.Parameters.Add(new SqlParameter("@AlertNumber", alertNumber));
                    cmd.Parameters.Add(new SqlParameter("@Defects", defects));
                    cmd.Parameters.Add(new SqlParameter("@Dt", dt.ToString("yyyyMMdd HH:mm:ss")));
                    cmd.Parameters.Add(new SqlParameter("@DeviationMessage", HttpUtility.UrlDecode(deviationMessage)));

                    sql.Add(cmd);
                }

            }

            try
            {

                foreach (var command in sql)
                {

                    command.Connection.Open();

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

                return null;
            }
            //Caso ocorra alguma Exception, grava o log e retorna zero
            catch (SqlException ex)
            {
                /**
                 * GABRIEL NUNES VOLTOU TIROU O LOG PARA MELHRAR PERFORMANCE
                 * DATE 2017-07-27
                 */
                int insertLog = insertLogJson(sql.ToString(), ex.Message, "N/A", "N/A", "insertDeviation");
                return "error";
            }
            catch (Exception ex)
            {
                /**
                * GABRIEL NUNES VOLTOU TIROU O LOG PARA MELHRAR PERFORMANCE
                * DATE 2017-07-27
                */
                int insertLog = insertLogJson(sql.ToString(), ex.Message, "N/A", "N/A", "insertDeviation");
                return "error";
            }
        }

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
            catch (Exception)
            {
                /**
                 * GABRIEL NUNES TIROU A GRAVAÇÃO DO LOG DE SENDEMAIL PARA MELHORAR PERFORMANCE DO SISTEMA
                 * DATE: 2017-06-23
                 */
                //int insertLog = insertLogJson(mensagemEstouro, ex.Message, null, null, "sendEmail");
            }
            return null;
        }

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
            catch (Exception)
            {
                //int insertLog = insertLogJson(email, ex.Message, null, null, "sendEmail");
            }
            return null;
        }

        /// <summary>
        /// Seleciona Todas as unidades que o usuário pode acessar
        /// </summary>
        /// <param name="UserSgq_Id"></param>
        /// <returns></returns>
        protected string selectUserCompanys(int UserSgq_Id, int ParCompany_Id)
        {
            var parCompanyXUserSgq = dbEf.ParCompany.Where(r => r.IsActive).ToList();

            string options = null;

            foreach (var p in parCompanyXUserSgq)
            {
                string selected = null;
                if (p.ParCompany_Id == ParCompany_Id)
                {
                    selected = " selected";
                }

                options += "<option" + selected + " value=\"" + p.Id + "\">" + p.Name + "</option>";
            }

            if (!string.IsNullOrEmpty(options))
            {
                options = "<select id=\"selectParCompany\" style=\"margin: 14px;\" ParCompany_Id=\"" + ParCompany_Id + "\">" + options + "</select>";
            }

            return options;
        }

        public string UserCompanyUpdate(string UserSgq_Id, int ParCompany_Id)
        {
            //Adicionar o departamento
            string sql = "UPDATE UserSgq SET ParCompany_Id=@ParCompany_Id WHERE Id=@UserSgq_Id";

            string conexao = this.conexao;

            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {

                        command.CommandType = CommandType.Text;
                        command.Parameters.Add(new SqlParameter("@ParCompany_Id", ParCompany_Id));
                        command.Parameters.Add(new SqlParameter("@UserSgq_Id", UserSgq_Id));

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
            catch (Exception)
            {
                return "Não foi possivel alterar a unidade";
            }
        }

        public string InsertCorrectiveAction(InsertCorrectiveActionClass insertCorrectiveActionClass)
        {
            string CollectionLevel2_Id = insertCorrectiveActionClass.CollectionLevel2_Id;
            string ParLevel1_Id = insertCorrectiveActionClass.ParLevel1_Id;
            string ParLevel2_Id = insertCorrectiveActionClass.ParLevel2_Id;
            string Shift = insertCorrectiveActionClass.Shift;
            string Period = insertCorrectiveActionClass.Period;
            string ParCompany_Id = insertCorrectiveActionClass.ParCompany_Id;
            string EvaluationNumber = insertCorrectiveActionClass.EvaluationNumber;
            string ParFrequency_Id = insertCorrectiveActionClass.ParFrequency_Id;
            string data = insertCorrectiveActionClass.data;
            string AuditorId = insertCorrectiveActionClass.AuditorId;
            string SlaughterId = insertCorrectiveActionClass.SlaughterId;
            string TechinicalId = insertCorrectiveActionClass.TechinicalId;
            string DateTimeSlaughter = insertCorrectiveActionClass.DateTimeSlaughter;
            string DateTimeTechinical = insertCorrectiveActionClass.DateTimeTechinical;
            string DateCorrectiveAction = insertCorrectiveActionClass.DateCorrectiveAction;
            string AuditStartTime = insertCorrectiveActionClass.AuditStartTime;
            string DescriptionFailure = insertCorrectiveActionClass.DescriptionFailure;
            string ImmediateCorrectiveAction = insertCorrectiveActionClass.ImmediateCorrectiveAction;
            string ProductDisposition = insertCorrectiveActionClass.ProductDisposition;
            string PreventativeMeasure = insertCorrectiveActionClass.PreventativeMeasure;
            string reauditnumber = insertCorrectiveActionClass.reauditnumber;

            try
            {

                //inserir a acção corretiva com processo

                string parCluster_Id_parLevel1_id = ParLevel1_Id.Replace(quebraProcesso, "|");
                string parCluster_Id = parCluster_Id_parLevel1_id.Split('|').Length > 1 ? parCluster_Id_parLevel1_id.Split('|')[0] : "0";
                ParLevel1_Id = parCluster_Id_parLevel1_id.Split('|').Length > 1 ? parCluster_Id_parLevel1_id.Split('|')[1] : parCluster_Id_parLevel1_id.Split('|')[0];

                string parCluster_Id_parLevel2_id = ParLevel2_Id.Replace(quebraProcesso, "|");
                ParLevel2_Id = parCluster_Id_parLevel2_id.Split('|').Length > 1 ? parCluster_Id_parLevel2_id.Split('|')[1] : parCluster_Id_parLevel2_id.Split('|')[0];



                //using (var transacao = new TransactionScope())
                //{
                SlaughterId = DefaultValueReturn(SlaughterId, "1");
                TechinicalId = DefaultValueReturn(TechinicalId, "1");
                DateTimeSlaughter = DefaultValueReturn(DateTimeSlaughter, "03012017 00:00:00");
                DateTimeTechinical = DateTimeSlaughter;
                Period = DefaultValueReturn(Period, "1");

                if (string.IsNullOrEmpty(CollectionLevel2_Id) || CollectionLevel2_Id == "0")
                {
                    CollectionLevel2_Id = getCollectionLevel2WithCorrectiveAction(ParLevel1_Id, ParLevel2_Id, Shift, Period, ParCompany_Id, EvaluationNumber, reauditnumber, data, parCluster_Id).ToString();
                    if (CollectionLevel2_Id == "0")
                    {
                        return "erro na InsertCorrectiveAction!";
                    }
                }

                DescriptionFailure = HttpUtility.UrlDecode(DescriptionFailure, System.Text.Encoding.Default);
                ImmediateCorrectiveAction = HttpUtility.UrlDecode(ImmediateCorrectiveAction, System.Text.Encoding.Default);
                ProductDisposition = HttpUtility.UrlDecode(ProductDisposition, System.Text.Encoding.Default);
                PreventativeMeasure = HttpUtility.UrlDecode(PreventativeMeasure, System.Text.Encoding.Default);

                int id = correctiveActionInsert(AuditorId, CollectionLevel2_Id, SlaughterId, TechinicalId, DateTimeSlaughter, DateTimeTechinical, DateCorrectiveAction, AuditStartTime, DescriptionFailure,
                    ImmediateCorrectiveAction, ProductDisposition, PreventativeMeasure);

                if (id > 0)
                {
                    //01/20/2017


                    string dataInicio = null;
                    string dataFim = null;

                    data = data.Trim();

                    if (!data.Contains("/"))
                    {
                        string dia = data.Substring(2, 2);
                        string mes = data.Substring(0, 2);
                        string ano = data.Substring(4, 4);

                        data = ano + "/" + mes + "/" + dia;
                    }

                    DateTime dataAPP = Convert.ToDateTime(data);

                    //Pega a data pela regra da frequencia
                    getFrequencyDate(Convert.ToInt32(ParFrequency_Id), dataAPP, ref dataInicio, ref dataFim);
                    var idUpdate = updateCorrectiveAction_CollectionLevel2_By_ParLevel1(ParLevel1_Id, ParCompany_Id, dataInicio, dataFim, reauditnumber);
                    //transacao.complete();
                    return null;
                }
                else
                {
                    int insertLog = insertLogJson("", "", "N/A", "N/A", "Na InsertCorrectiveAction não achou uma referencia");
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

        protected int getCollectionLevel2WithCorrectiveAction(string ParLevel1_Id, string ParLevel2_Id, string Shift, string Period, string ParCompany_Id, string EvaluationNumber, string reauditnumber, string data, string parCluster_Id)
        {
            //Converte a data no padrão de busca do Banco de Dados

            if (!data.Contains("-"))
            {
                string dia = data.Substring(2, 2);
                string mes = data.Substring(0, 2);
                string ano = data.Substring(4, 4);

                data = ano + "-" + mes + "-" + dia;
            }

            //string sql = "SELECT c2.Id FROM CollectionLevel2 c2 WITH (NOLOCK) " +
            //    " left join CollectionLevel2XCluster C2C on C2C.CollectionLevel2_Id = C2.id " +
            //    " WHERE ParLevel1_Id ='" + ParLevel1_Id + "' AND " +
            //    "ParLevel2_Id='" + ParLevel2_Id + "' AND " +
            //    "UnitId='" + ParCompany_Id + "' AND " +
            //    "Shift='" + Shift + "' AND " +
            //    "Period='" + Period + "' AND " +
            //    "EvaluationNumber='" + EvaluationNumber + "' AND " +
            //    "CAST(CollectionDate as date)=CAST('" + data + "' as date) AND " +
            //    "c2c.parCluster_Id = '" + parCluster_Id + "' AND " +
            //    "reauditNumber= " + reauditnumber;

            string sql = $@"SELECT c2.Id FROM CollectionLevel2 c2 WITH (NOLOCK) 
                left join CollectionLevel2XCluster C2C on C2C.CollectionLevel2_Id = C2.id
                WHERE ParLevel1_Id=@ParLevel1_Id AND
                ParLevel2_Id=@ParLevel2_Id AND 
                UnitId=@ParCompany_Id AND 
                Shift=@Shift AND 
                Period=@Period AND 
                EvaluationNumber=@EvaluationNumber AND 
                CAST(CollectionDate as date)=CAST(@Data as date) AND
                c2c.parCluster_Id=@ParCluster_Id AND
                reauditNumber= @Reauditnumber";

            //string sql = "SELECT Id FROM CollectionLevel2 WHERE ParLevel1_Id='" + ParLevel1_Id + "' AND UnitId='" + ParCompany_Id + "' AND Shift='" + Shift + "' AND Period='" + Period + "' AND EvaluationNumber='" + EvaluationNumber + "'AND ReauditNumber='" + reauditnumber +
            //"' AND HaveCorrectiveAction=1";

            string conexao = this.conexao;
            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.Add(new SqlParameter("@ParLevel1_Id", ParLevel1_Id));
                        command.Parameters.Add(new SqlParameter("@ParLevel2_Id", ParLevel2_Id));
                        command.Parameters.Add(new SqlParameter("@ParCompany_Id", ParCompany_Id));
                        command.Parameters.Add(new SqlParameter("@Shift", Shift));
                        command.Parameters.Add(new SqlParameter("@Period", Period));
                        command.Parameters.Add(new SqlParameter("@EvaluationNumber", EvaluationNumber));
                        command.Parameters.Add(new SqlParameter("@Data", data));
                        command.Parameters.Add(new SqlParameter("@ParCluster_Id", parCluster_Id));
                        command.Parameters.Add(new SqlParameter("@Reauditnumber", reauditnumber));

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

        public string getPhaseLevel2(int ParCompany_Id, string date)
        {
            var ResultPhaseDB = new SGQDBContext.ResultPhase(db);
            var ResultPhaseFrequencyDB = new SGQDBContext.ResultPhaseFrequency(db);
            var ResultLevel2PeriodDB = new SGQDBContext.ResultLevel2Period(db);

            //Instanciamos uma variável que irá 

            DateTime startDate = DateCollectConvert(date);
            DateTime endDate = DateCollectConvert(date);

            startDate = startDate.AddDays(-30);

            var ResultPhaseList = ResultPhaseDB.GetByMonth(ParCompany_Id, startDate, endDate);

            string PhaseResult = null;
            //Percorremos as consolidações de ParLevel1
            foreach (var c in ResultPhaseList)
            {
                var frequency = ResultPhaseFrequencyDB.GetPhaseFrequency(c.ParLevel1_Id, c.Phase);

                c.CountPeriod = 0;
                c.CountShift = 0;

                var divPeriod = "";
                if (frequency != null && frequency.ParFrequency_Id == 1)
                {
                    var listResultLevel2 = ResultLevel2PeriodDB.GetResultLevel2Period(c.Id, ParCompany_Id, c.ParLevel1_Id, c.ParLevel2_Id, startDate, endDate, c.Shift);

                    foreach (var obj in listResultLevel2)
                    {
                        divPeriod += "<div class='countPeriod' period='" + obj.Period + "' date='" + obj.CollectionDate.ToString("MMddyyyy") + "'></div>";
                    }

                }

                PhaseResult += "<div " +
                    "parlevel1_id=\"" + c.ParLevel1_Id + "\" " +
                    "parlevel2_id=\"" + c.ParLevel2_Id + "\" " +
                    "collectiondate=\"" + c.CollectionDate + "\" " +
                    "evaluationnumber=\"" + c.EvaluationNumber + "\" " +
                    "period=\"" + c.Period + "\" " +
                    "shift=\"" + c.Shift + "\" " +
                    "phase=\"" + c.Phase + "\" " +
                    "class=\"PhaseResultlevel2\">" +
                    divPeriod
                    + "</div>";
            }
            return PhaseResult;
        }

        public string getResultEvaluationDefects(int parCompany_Id, string date, int parLevel1_Id)
        {
            var ResultPhaseDB = new SGQDBContext.ResultEvaluationDefects(db);
            //Instanciamos uma variável que irá 

            DateTime dateAtual = DateCollectConvert(date);

            var ResultEvaluationDefectsList = ResultPhaseDB.GetByDay(parCompany_Id, dateAtual, parLevel1_Id);

            string PhaseResult = null;
            //Percorremos as consolidações de ParLevel1
            foreach (var c in ResultEvaluationDefectsList)
            {
                PhaseResult += "<div                                                                            " +
                    "date=\"" + date + "\"                                                                      " +
                    "Defects=\"" + c.Defects + "\"                                                              " +
                    "EvaluationNumber=\"" + c.EvaluationNumber + "\"                                            " +
                    "Sample=\"" + c.Sample + "\"                                            " +
                    "Period=\"" + c.Period + "\"                                                                " +
                    "Shift=\"" + c.Shift + "\"                                                                  " +
                    "ParLevel1_id=\"" + parLevel1_Id + "\"                                                      " +
                    "class=\"EvaluationDefects\"></div>";
            }
            return PhaseResult;
        }

        public string getCollectionLevel2Keys(GetCollectionLevel2KeysClass getCollectionLevel2KeysClass)
        {
            string ParCompany_Id = getCollectionLevel2KeysClass.ParCompany_Id ?? "";
            string date = getCollectionLevel2KeysClass.date;
            int ParLevel1_Id = getCollectionLevel2KeysClass.ParLevel1_Id;

            DateTime data = DateCollectConvert(date);

            string dataS = data.ToString("yyyyMMdd");

            string ResultsKeys = "";

            string sql = $@"
                SELECT
                ROW_NUMBER() OVER(ORDER BY CL2.ParLevel1_Id) AS ROW,
                CL2.ParLevel1_Id,
                '<div id=""' + CL2.[Key] + '"" class=""collectionLevel2Key""></div>' COLUNA
                INTO #COLETASLEVEL3
                FROM CollectionLevel2 CL2   (nolock)
                WHERE CL2.UnitId=@ParCompany_Id AND CAST(CL2.CollectionDate AS DATE) BETWEEN @DataS AND @DataS
                ----------------------------------------------------------
                -- LISTA DE INDICADORES--
                ----------------------------------------------------------
                DECLARE @Indicadores Table(ParLevel1_ID int)

                insert into @Indicadores
                select distinct ParLevel1_ID from #COLETASLEVEL3

                ----------------------------------------------------------
                -- PRIMEIRO INDICADOR --
                ----------------------------------------------------------
                DECLARE @TBL_RESPOSTA TABLE (ROW INT, ParLevel1_id INT, Coluna VARCHAR(MAX))

                declare @I int = 1;
                            declare @Indicador int;
                            SELECT TOP 1 @Indicador = ParLevel1_ID FROM @Indicadores

                DECLARE @CONCAT VARCHAR(MAX) = ''

                WHILE @Indicador IS NOT NULL
                BEGIN
                    WHILE @I <= (SELECT Count(*) FROM #COLETASLEVEL3 WHERE @Indicador = ParLevel1_ID)
                    BEGIN                                                                                                                                 

                        INSERT INTO @TBL_RESPOSTA
                        SELECT ROW,ParLevel1_id,Coluna + '' + @CONCAT FROM(

                                SELECT ROW_NUMBER() OVER(ORDER BY ParLevel1_Id) AS ROW, ParLevel1_id, Coluna FROM #COLETASLEVEL3  WHERE @Indicador = ParLevel1_ID   
                        ) consulta
                        WHERE ROW = @I
                        SET @CONCAT = (SELECT TOP 1 Coluna FROM @TBL_RESPOSTA WHERE ROW = @I AND @Indicador = ParLevel1_ID   )
                        DELETE FROM @TBL_RESPOSTA WHERE ROW = (@I - 1) AND @Indicador = ParLevel1_ID
                        SET @I = @I + 1
                    END                                                                                                                                            

                SET @I = 1
                SET @CONCAT = ''
                DELETE FROM @Indicadores WHERE ParLevel1_ID = @Indicador
                SET @Indicador = (SELECT TOP 1 ParLevel1_ID FROM @Indicadores)

                END

                    --ENTREGA DA RESPOSTA

                    SELECT '<div parlevel1_id=""' + CAST(ParLevel1_Id AS VARCHAR) + '"" class=""ResultLevel2Key"">' + Coluna + '</div>' as retorno  
                    INTO #TBL_RESPOSTA
                    FROM @TBL_RESPOSTA
                    SELECT* FROM #TBL_RESPOSTA                                                                                                                     
                DROP TABLE #TBL_RESPOSTA DROP TABLE #COLETASLEVEL3 ";

            List<ResultadoUmaColuna> Lista1 = new List<ResultadoUmaColuna>();

            try
            {
                using (Factory factory = new Factory("DefaultConnection"))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, factory.connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add(new SqlParameter("@ParCompany_Id", ParCompany_Id));
                        cmd.Parameters.Add(new SqlParameter("@DataS", dataS));

                        Lista1 = factory.SearchQuery<ResultadoUmaColuna>(cmd).ToList();
                    }
                }

                foreach (var i in Lista1)
                {
                    ResultsKeys += i.retorno;
                }
            }
            catch (Exception ex)
            {


            }

            return ResultsKeys;
        }

        public string _ReConsolidationByLevel1(int ParCompany_Id, int ParLevel1_Id, DateTime ConsolidationDate)
        {

            //string sql = "SELECT CL2.Id, CL2.ParLevel2_Id, CL2.ConsolidationLevel1_Id FROM ConsolidationLevel2 CL2 WITH (NOLOCK) " +
            //             "\n INNER JOIN ConsolidationLevel1 CL1 WITH (NOLOCK)  ON CL2.ConsolidationLevel1_Id=CL1.ID " +
            //   "WHERE CL2.UnitId=" + ParCompany_Id + " AND CL1.ParLevel1_Id=" + ParLevel1_Id + " AND CAST(CL1.ConsolidationDate AS DATE) = '" + ConsolidationDate.ToString("yyyyMMdd") + "'";

            string sql = $@"SELECT CL2.Id, CL2.ParLevel2_Id, CL2.ConsolidationLevel1_Id FROM ConsolidationLevel2 CL2 WITH (NOLOCK) 
                INNER JOIN ConsolidationLevel1 CL1 WITH (NOLOCK)  ON CL2.ConsolidationLevel1_Id=CL1.ID
                WHERE CL2.UnitId=@ParCompany_Id AND CL1.ParLevel1_Id=@ParLevel1_Id AND CAST(CL1.ConsolidationDate AS DATE)=@ConsolidationDate";


            string conexao = this.conexao;
            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {

                        command.CommandType = CommandType.Text;
                        command.Parameters.Add(new SqlParameter("@ParCompany_Id", ParCompany_Id));
                        command.Parameters.Add(new SqlParameter("@ParLevel1_Id", ParLevel1_Id));
                        command.Parameters.Add(new SqlParameter("@ConsolidationDate", ConsolidationDate.ToString("yyyyMMdd")));

                        connection.Open();
                        using (SqlDataReader r = command.ExecuteReader())
                        {

                            //Se encontrar, retorna o Id da Consolidação
                            while (r.Read())
                            {

                                int ConsolidationLevel2_Id = Convert.ToInt32(r[0]);
                                int ParLevel2_Id = Convert.ToInt32(r[1]);
                                int ConsolidationLevel1_Id = Convert.ToInt32(r[2]);

                                var CollectionLevel2ConsolidationDB = new SGQDBContext.CollectionLevel2Consolidation(db);
                                var collectionLevel2Consolidation = CollectionLevel2ConsolidationDB.getConsolidation(ConsolidationLevel2_Id, ParLevel2_Id);

                                var updateConsolidationLevel2Id = updateConsolidationLevel2(ConsolidationLevel2_Id, "0", "0", "0", collectionLevel2Consolidation);

                                var ConsolidationLevel1XConsolidationLevel2DB = new ConsolidationLevel1XConsolidationLevel2(db);
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
                int insertLog = insertLogJson(sql, ex.Message, "N/A", "N/A", "_ReConsolidationByLevel1");
                return ex.Message;
            }
            catch (Exception ex)
            {
                int insertLog = insertLogJson(sql, ex.Message, "N/A", "N/A", "_ReConsolidationByLevel1");
                return ex.Message;
            }
        }

        public string ReconsolidationToLevel3(string collectionLevel2_Id)
        {

            try
            {
                ReconsolidationLevel3ByCollectionLevel2Id(collectionLevel2_Id);

                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    connection.Open();

                    SqlCommand command;

                    string query = $@"
                       DECLARE @ID BIGINT=@CollectionLevel2_Id
                    DECLARE @CL2 INT
                    DECLARE @CL1 INT
                    DECLARE @Defects DECIMAL(10,3)                                                                                           
                    DECLARE @DefectsResult DECIMAL(10, 3)                                                                                    
                    DECLARE @EvatuationResult DECIMAL(10, 3)                                                                                 
                    DECLARE @WeiEvaluation DECIMAL(10, 3)                                                                                    
                    DECLARE @WeiDefects DECIMAL(10, 3)                                                                                       
                    DECLARE @TotalLevel3Evaluation  DECIMAL(10, 3)                                                                           
                    DECLARE @TotalLevel3WithDefects DECIMAL(10, 3)                                                                           

                    select
                                                                                                                         
                    @Defects = isnull(sum(r3.Defects),0),                                                                                    
                    @DefectsResult = case when sum(r3.WeiDefects) > 0 then 1 else 0 end,                                                     
                    @EvatuationResult = case when sum(r3.Evaluation) > 0 then 1 else 0 end,                                                  
                    @WeiEvaluation = isnull(sum(r3.WeiEvaluation),0),                                                                        
                    @WeiDefects = isnull(sum(r3.WeiDefects),0),                                                                              
                    @TotalLevel3Evaluation = count(1),                                                                                       
                    @TotalLevel3WithDefects = SUM(IIF(r3.Defects > 0 AND r3.IsNotEvaluate = 0,1,0 ))
                    from result_level3 r3 WITH (NOLOCK)                                                                                      
                    where collectionlevel2_id = @ID                                                                                          
                    and r3.IsNotEvaluate = 0                                                                                                 
                                                                                                                         
                    UPDATE CollectionLevel2                                                                                                  
                    SET Defects = @Defects                                                                                                   
                    , DefectsResult = @DefectsResult                                                                                         
                    , EvaluatedResult = @EvatuationResult                                                                                    
                    , WeiEvaluation = @WeiEvaluation                                                                                         
                    , WeiDefects = @WeiDefects                                                                                               
                    , TotalLevel3Evaluation = @TotalLevel3Evaluation                                                                         
                    , TotalLevel3WithDefects = @TotalLevel3WithDefects                                                                       
                    , AlterDate = GETDATE()                                                                                                  
                    WHERE Id = @ID         --SELECT 1     

                    DECLARE @MAXEVALERT INT
                    DECLARE @LAST2ALERT INT


                    SELECT @CL2 = ConsolidationLevel2_Id FROM CollectionLevel2 WITH (NOLOCK) WHERE ID = @ID


                    SELECT 
	                      @WeiEvaluation = SUM(WeiEvaluation) 
	                    , @Defects = SUM(Defects) 
	                    , @WeiDefects = SUM(WeiDefects) 
	                    , @TotalLevel3WithDefects = SUM(TotalLevel3WithDefects) 
	                    , @TotalLevel3Evaluation = SUM(TotalLevel3Evaluation) 
	                    , @MAXEVALERT = MAX(LastEvaluationAlert) 
	                    , @LAST2ALERT = (SELECT top 1 LastLevel2Alert FROM CollectionLevel2 WHERE Id = max(c2.id)) 
	                    , @EvatuationResult =  SUM(EvaluatedResult) 
	                    , @DefectsResult  = SUM(DefectsResult)

                    FROM CollectionLevel2 C2  (nolock) WHERE  ConsolidationLevel2_Id = @CL2 


                    group by ConsolidationLevel2_Id

                    UPDATE ConsolidationLevel2 
                    SET AlertLevel=ISNULL(@LAST2ALERT,0)
                       , WeiEvaluation=@WeiEvaluation
                       , EvaluateTotal=@EvatuationResult
                       , DefectsTotal=@DefectsResult
                       , WeiDefects=@WeiDefects
                       , TotalLevel3Evaluation=@TotalLevel3Evaluation
                       , TotalLevel3WithDefects=@TotalLevel3WithDefects
                       , LastEvaluationAlert = ISNULL(@MAXEVALERT,0)
                       , LastLevel2Alert = ISNULL(@LAST2ALERT,0)
                       , EvaluatedResult=@EvatuationResult
                       , DefectsResult=@DefectsResult
                    WHERE ID = @CL2   

                    SELECT @CL1 = ConsolidationLevel1_Id FROM ConsolidationLevel2 WHERE ID = @CL2


                    select 
	                    @WeiEvaluation = SUM(WeiEvaluation) 
	                    , @EvatuationResult =  SUM(EvaluateTotal)
	                    , @Defects = SUM(DefectsTotal)
	                    , @WeiDefects = SUM(WeiDefects) 
	                    , @TotalLevel3Evaluation = SUM(TotalLevel3Evaluation) 
	                    , @TotalLevel3WithDefects = SUM(TotalLevel3WithDefects) 
	                    , @MAXEVALERT = MAX(LastEvaluationAlert) 
	                    , @LAST2ALERT = (SELECT top 1 LastLevel2Alert FROM CollectionLevel2 (nolock)  WHERE ConsolidationLevel2_Id = max(c2.id))
	                    , @EvatuationResult = SUM(EvaluatedResult) 
	                    , @DefectsResult = SUM(DefectsResult) 
                    FROM ConsolidationLevel2 C2 (nolock)  
                    where ConsolidationLevel1_Id=@CL1   

                    UPDATE ConsolidationLevel1 
	                    SET AtualAlert=@LAST2ALERT
	                    , Evaluation= @TotalLevel3Evaluation
	                    , WeiEvaluation=@WeiEvaluation
	                    , EvaluateTotal=@EvatuationResult
	                    , DefectsTotal=@Defects
	                    , WeiDefects=@WeiDefects
	                    , TotalLevel3Evaluation=@TotalLevel3Evaluation
	                    , TotalLevel3WithDefects=@TotalLevel3WithDefects
	                    , LastEvaluationAlert=@MAXEVALERT
	                    , LastLevel2Alert=@LAST2ALERT
	                    , EvaluatedResult= @EvatuationResult
	                    , DefectsResult=@DefectsResult
                    WHERE ID=@CL1";


                    command = new SqlCommand(query, connection);

                    command.CommandType = CommandType.Text;
                    command.Parameters.Add(new SqlParameter("@CollectionLevel2_Id", collectionLevel2_Id));

                    var iSql = command.ExecuteScalar();

                    if (connection.State == System.Data.ConnectionState.Open) connection.Close();
                }

                //using (var db = new Dominio.SgqDbDevEntities())
                //{
                //    int idl2 = Int32.Parse(collectionLevel2_Id);
                //    Dominio.CollectionLevel2 collectionLevel2 = db.CollectionLevel2.FirstOrDefault(r => r.Id == idl2);


                //    int company_Id = collectionLevel2.UnitId;
                //    int level1_Id = collectionLevel2.ParLevel1_Id;
                //    DateTime data = collectionLevel2.CollectionDate.Date;

                //    var retorno = _ReConsolidationByLevel1(company_Id, level1_Id, data);

                //    return "OK";
                //}
                return "OK";
            }
            catch (Exception e)
            {
                return e.ToString();
            }

        }

        protected void InsertCollectionLevel2XCollectionJson(List<KeyValuePair<int, int>> list)
        {
            string sql = $@"INSERT INTO CollectionLevel2XCollectionJson
                        (CollectionLevel2_Id,CollectionJson_Id)
                        VALUES";

            StringBuilder query = new StringBuilder();
            if (list.Count > 0)
            {

                for (int i = 0; i < list.Count; i++)
                {
                    if (i % 1000 == 0)
                    {
                        if (query.Length > 0)
                            query.Append(" GO ");
                        query.Append(sql);
                    }
                    query.Append($"({list[i].Key},{list[i].Value})");
                    if (i + 1 % 1000 != 0 && i + 1 < list.Count)
                    {
                        query.Append(",");
                    }
                }

                try
                {
                    using (SqlConnection connection = new SqlConnection(conexao))
                    {
                        using (SqlCommand command = new SqlCommand(query.ToString(), connection))
                        {
                            connection.Open();
                            Convert.ToInt32(command.ExecuteNonQuery());
                        }
                        if (connection.State == System.Data.ConnectionState.Open) connection.Close();
                    }
                }
                catch (SqlException ex)
                {
                    int insertLog = insertLogJson(sql, ex.Message, "N/A", "N/A", "InsertCollectionLevel2XCollectionJson");
                    //throw ex;
                }
                catch (Exception ex)
                {
                    int insertLog = insertLogJson(sql, ex.Message, "N/A", "N/A", "InsertCollectionLevel2XCollectionJson");
                    //throw ex;
                }

            }

        }

        public void ReconsolidationLevel3ByCollectionLevel2Id(string collectionLevel2_Id)
        {
            try
            {

                var sql = $@"SELECT
                    	RL3.Id
                       ,SUM(PMV.PunishmentValue) AS PunishmentValue
                       ,RL3.IsConform
                    FROM CollectionLevel2XParHeaderField CL2XHF
                    INNER JOIN Result_Level3 RL3
                    	ON RL3.CollectionLevel2_Id = CL2XHF.CollectionLevel2_Id
                    INNER JOIN ParHeaderField PHF
                    	ON PHF.Id = CL2XHF.ParHeaderField_Id
                    INNER JOIN ParMultipleValues PMV
                    	ON PMV.Id = CAST(CL2XHF.Value as int)
                    WHERE CL2XHF.CollectionLevel2_Id = @CollectionLevel2_Id
                    and CL2XHF.ParFieldType_Id in (1,2,3)
                    GROUP BY RL3.Id, RL3.IsConform";


                using (Factory factory = new Factory("DefaultConnection"))
                {
                    var resultsLevel3 = new List<Dominio.Result_Level3>();
                    using (SqlCommand cmd = new SqlCommand(sql, factory.connection))
                    {

                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add(new SqlParameter("@CollectionLevel2_Id", collectionLevel2_Id));
                        resultsLevel3 = factory.SearchQuery<Dominio.Result_Level3>(cmd).ToList();
                    }

                    if (resultsLevel3.Count > 0)
                    {
                        foreach (var resultLevel3 in resultsLevel3)
                        {
                            if (!resultLevel3.IsConform.Value)
                            {
                                var sql2 = $@"SELECT
                                             	RL3.Id
                                             	,((RL3.Defects * RL3.Weight) + ( @PunishmentValue * RL3.Weight)) as WeiDefects
                                             FROM Result_Level3 RL3
                                             WHERE Id=@ResultLevel3_Id";

                                var resultLevel3WeiDefects = new Dominio.Result_Level3();

                                using (SqlCommand cmd = new SqlCommand(sql2, factory.connection))
                                {

                                    cmd.CommandType = CommandType.Text;
                                    cmd.Parameters.Add(new SqlParameter("@PunishmentValue", resultLevel3.PunishmentValue.ToString().Replace(',', '.')));
                                    cmd.Parameters.Add(new SqlParameter("@ResultLevel3_Id", resultLevel3.Id));

                                    resultLevel3WeiDefects = factory.SearchQuery<Dominio.Result_Level3>(cmd).FirstOrDefault();
                                }

                                if (resultLevel3WeiDefects != null)
                                {
                                    var sqlUpdateWeiDefects = $@"
                                            update Result_Level3 set WeiDefects = @WeiDefects, 
                                            PunishmentValue = @PunishmentValue
                                            where id = @ResultLevel3WeiDefects_Id";

                                    using (SqlCommand cmd = new SqlCommand(sqlUpdateWeiDefects, factory.connection))
                                    {

                                        cmd.CommandType = CommandType.Text;
                                        cmd.Parameters.Add(new SqlParameter("@WeiDefects", resultLevel3WeiDefects.WeiDefects.ToString().Replace(',', '.')));
                                        cmd.Parameters.Add(new SqlParameter("@PunishmentValue", resultLevel3.PunishmentValue.ToString().Replace(',', '.')));
                                        cmd.Parameters.Add(new SqlParameter("@ResultLevel3WeiDefects_Id", resultLevel3.PunishmentValue.ToString().Replace(',', '.')));

                                        cmd.ExecuteNonQuery();
                                        //factory.ExecuteSql(cmd.CommandText);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        protected int InsertConsolidationLevel1XParDepartment(int consolidationLevel1_Id, int? parDepartment_Id)
        {
            var ConsolidationLevel1DB = new SGQDBContext.ConsolidationLevel1(db);

            string sql = $@"INSERT ConsolidationLevel1XParDepartment ([consolidationLevel1_Id],[ParDepartment_Id],[AddDate]) 
                         VALUES
                         (@ConsolidationLevel1_Id, @ParDepartment_Id, getdate())
                         SELECT @@IDENTITY AS 'Identity'";

            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {

                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {

                        command.CommandType = CommandType.Text;
                        command.Parameters.Add(new SqlParameter("@ConsolidationLevel1_Id", consolidationLevel1_Id));
                        command.Parameters.Add(new SqlParameter("@ParDepartment_Id", parDepartment_Id));

                        var i = Convert.ToInt32(command.ExecuteScalar());
                        //Se o registro for inserido retorno o Id da Consolidação
                        if (i > 0)
                        {
                            return i;
                        }
                        else
                        {
                            //Caso ocorra algum erro, retorno zero
                            return 0;
                        }
                    }
                    if (connection.State == System.Data.ConnectionState.Open) connection.Close();
                }
            }
            //Caso ocorra alguma Exception, grava o log e retorna zero
            catch (SqlException ex)
            {
                int insertLog = insertLogJson(sql, ex.Message, "N/A", "N/A", "InsertConsolidationLevel1XCluster");
                throw ex;
            }
            catch (Exception ex)
            {
                int insertLog = insertLogJson(sql, ex.Message, "N/A", "N/A", "InsertConsolidationLevel1XCluster");
                throw ex;
            }
        }

        protected void InsertCollectionLevel2XParDepartment(int CollectionLevel2_Id, int? parDepartment_Id)
        {
            var IsUpdate = false;
            var sql = "";

            using (var db = new SgqDbDevEntities())
            {
                IsUpdate = db.CollectionLevel2XParDepartment.Any(r => r.CollectionLevel2_Id == CollectionLevel2_Id);
            }

            if (IsUpdate)
            {
                sql = $@"UPDATE CollectionLevel2XParDepartment set [ParDepartment_Id] = @ParDepartment_Id, [AlterDate] = GETDATE() 
                WHERE [CollectionLevel2_Id] = @CollectionLevel2_Id";
            }
            else
            {
                sql = $@"INSERT INTO CollectionLevel2XParDepartment ([CollectionLevel2_Id], [ParDepartment_Id], [AddDate]) 
                VALUES (@CollectionLevel2_Id, @ParDepartment_Id , GETDATE())";
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {

                        command.CommandType = CommandType.Text;
                        command.Parameters.Add(new SqlParameter("@CollectionLevel2_Id", CollectionLevel2_Id));
                        command.Parameters.Add(new SqlParameter("@ParDepartment_Id", parDepartment_Id));

                        connection.Open();
                        Convert.ToInt32(command.ExecuteScalar());
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected int InsertConsolidationLevel2XParDepartment(int consolidationLevel2_Id, int parDepartment_Id)
        {
            var ConsolidationLevel1DB = new SGQDBContext.ConsolidationLevel1(db);

            string sql = $@"INSERT ConsolidationLevel2XParDepartment ([consolidationLevel2_Id],[ParDepartment_Id],[AddDate])
                         VALUES 
                         (@ConsolidationLevel2_Id,@ParDepartment_Id, getdate())
                         SELECT @@IDENTITY AS 'Identity'";

            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {

                        command.CommandType = CommandType.Text;
                        command.Parameters.Add(new SqlParameter("@ConsolidationLevel2_Id", consolidationLevel2_Id));
                        command.Parameters.Add(new SqlParameter("@ParDepartment_Id", parDepartment_Id));

                        var i = Convert.ToInt32(command.ExecuteScalar());
                        //Se o registro for inserido retorno o Id da Consolidação
                        if (i > 0)
                        {
                            return i;
                        }
                        else
                        {
                            //Caso ocorra algum erro, retorno zero
                            return 0;
                        }
                    }
                    if (connection.State == System.Data.ConnectionState.Open)
                        connection.Close();
                }
            }
            //Caso ocorra alguma Exception, grava o log e retorna zero
            catch (SqlException ex)
            {
                int insertLog = insertLogJson(sql, ex.Message, "N/A", "N/A", "InsertConsolidationLevel2XCluster");
                throw ex;
            }
            catch (Exception ex)
            {
                int insertLog = insertLogJson(sql, ex.Message, "N/A", "N/A", "InsertConsolidationLevel2XCluster");
                throw ex;
            }
        }

        public int GetLastSampleByCollectionLevel2(GetLastSampleByCollectionLevel2Class getLastSampleByCollectionLevel2Class)
        {
            string ParLevel1_Id = getLastSampleByCollectionLevel2Class.ParLevel1_Id;
            string ParLevel2_Id = getLastSampleByCollectionLevel2Class.ParLevel2_Id;
            string UnitId = getLastSampleByCollectionLevel2Class.UnitId;
            string EvaluationNumber = getLastSampleByCollectionLevel2Class.EvaluationNumber;
            string Shift = getLastSampleByCollectionLevel2Class.Shift;
            DateTime CollectionDate = getLastSampleByCollectionLevel2Class.CollectionDate;

            if (string.IsNullOrEmpty(ParLevel1_Id) ||
                string.IsNullOrEmpty(ParLevel2_Id) ||
                string.IsNullOrEmpty(UnitId) ||
                string.IsNullOrEmpty(EvaluationNumber) ||
                string.IsNullOrEmpty(Shift) ||
                string.IsNullOrEmpty(CollectionDate.ToString()))
            {
                return 0;
            }


            var lista1 = ParLevel1_Id.Replace(quebraProcesso, "|").Split('|');
            var lista2 = ParLevel2_Id.Replace(quebraProcesso, "|").Split('|');

            int parCluster_Id = lista1.Length > 1 ? Int32.Parse(lista1[0]) : 0;

            int parlevel1_id = lista1.Length > 1 ? Int32.Parse(lista1[1]) : Int32.Parse(lista1[0]);
            int parlevel2_id = lista2.Length > 1 ? Int32.Parse(lista2[1]) : Int32.Parse(lista2[0]);


            var sql = $@"
                    SELECT
                    	IIF(MAX(cl.Sample) IS NULL, 0, MAX(cl.Sample)) AS Sample
                    FROM CollectionLevel2 cl with (nolock)
                    INNER JOIN ParLevel1XCluster plx with (nolock)
                    	ON plx.ParLevel1_Id = cl.ParLevel1_Id
                    		AND plx.IsActive = 1
                    WHERE 1 = 1
                    AND cl.ParLevel1_Id = @Parlevel1_id 
                    AND cl.ParLevel2_Id = @Parlevel2_id 
                    AND cl.UnitId = @UnitId 
                    AND cl.EvaluationNumber = @EvaluationNumber
                    AND plx.ParCluster_Id = @ParCluster_Id
                    AND cl.Shift = @Shift
                    AND CAST(cl.CollectionDate AS DATE) = @CollectionDate";

            using (var factory = new Factory("DefaultConnection"))
            {
                using (SqlCommand cmd = new SqlCommand(sql, factory.connection))
                {

                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add(new SqlParameter("@Parlevel1_id", parlevel1_id));
                    cmd.Parameters.Add(new SqlParameter("@Parlevel2_id", parlevel2_id));
                    cmd.Parameters.Add(new SqlParameter("@UnitId", UnitId));
                    cmd.Parameters.Add(new SqlParameter("@EvaluationNumber", EvaluationNumber));
                    cmd.Parameters.Add(new SqlParameter("@ParCluster_Id", parCluster_Id));
                    cmd.Parameters.Add(new SqlParameter("@Shift", Shift));
                    cmd.Parameters.Add(new SqlParameter("@CollectionDate", CollectionDate.ToString("yyyy-MM-dd")));

                    var retorno = Convert.ToInt32(cmd.ExecuteScalar());

                    return retorno;
                }
            }
        }

        public IEnumerable<DictionaryEntry> GetResource(string language)
        {
            return Resources.Resource;
        }
    }
}

#endregion