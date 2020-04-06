using ADOFactory;
using Dapper;
using Dominio;
using DTO;
using DTO.Helpers;
using Newtonsoft.Json;
using SGQDBContext;
using SgqServiceBusiness.Services;
using SgqService.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Newtonsoft.Json.Linq;
using System.Dynamic;

namespace SgqServiceBusiness.Api
{
    public class VTVerificacaoTipificacaoApiController

    {
        public string mensagemErro { get; set; }
        public string conexao { get; set; }

        //string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        //SGQ_GlobalEntities dbGlobal;
        SgqDbDevEntities dbSgq;
        public VTVerificacaoTipificacaoApiController(string conexao)
        {
            this.conexao = conexao;
            dbSgq = new SgqDbDevEntities();
            dbSgq.Configuration.LazyLoadingEnabled = false;
            dbSgq.Configuration.ValidateOnSaveEnabled = false;
        }

        public void SaveVTVerificacaoTipificacao(TipificacaoViewModel model)
        {

            var bkp = JsonConvert.SerializeObject(model);

            var VT = model.VerificacaoTipificacao;

            //throw new Exception("teste");
            var verificacaoListJBS = new List<VerificacaoTipificacaoV2>();
            var verificacaoListGRT = new List<VerificacaoTipificacaoV2>();
            var verificacaoSalvar = new List<VerificacaoTipificacaoV2>();
            //GlobalConfig.MockOn = true;

            //if (GlobalConfig.MockOn)
            //    conexaoUndiade = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            SqlConnectionStringBuilder connectionString = new SqlConnectionStringBuilder();

            //connectionString.Password = "betsy1";
            //connectionString.UserID = "sa";
            //connectionString.InitialCatalog = "dbGQualidade_JBS";
            //connectionString.DataSource = @"DELLGABRIEL\MSSQL2014";

            connectionString.Password = "grJsoluco3s";
            connectionString.UserID = "UserGQualidade";



            foreach (var verificacao in model.VerificacaoTipificacao)
            {

                var controleErro = false;

                try
                {
                    #region Primeira Etapa
                    string codigoUnidade;

                    using (var db = new Dominio.SgqDbDevEntities())
                    {
                        int a = Int32.Parse(verificacao.UnidadeId.ToString());
                        Dominio.ParCompany company = db.ParCompany.FirstOrDefault(r => r.Id == a);
                        codigoUnidade = company.CompanyNumber.ToString();
                        connectionString.InitialCatalog = company.DBServer.ToString();
                        connectionString.DataSource = company.IPServer.ToString();
                    }

                    using (var dbUnit = new Factory(connectionString))
                    {

                        string sql = "exec FBED_GRTTipificacaoCaracteristica " + codigoUnidade + ", '" + verificacao.DataHora.ToString("yyyyMMdd") + "', " + verificacao.Sequencial.ToString();

                        dynamic resultFbed = dbUnit.QueryNinjaADO(sql);

                        //Adiciona todas que vem da JBS
                        foreach (var fbed in resultFbed)
                        {
                            if (fbed.iBanda == verificacao.Banda)
                            {
                                var verificacaoJBS = new VerificacaoTipificacaoV2();
                                fbed.existeComparacao = true;
                                verificacaoJBS.JBS_nCdCaracteristicaTipificacao = fbed.nCdCaracteristicaTipificacao;//JBS_nCdCaracteristicaTipificacao EX: 83
                                verificacaoJBS.cIdentificadorTipificacao = fbed.cIdentificadorTipificacao; //JBS_nCdCaracteristicaTipificacao EX: <CONTUSAO>
                                verificacaoListJBS.Add(verificacaoJBS);
                            }
                        }

                        //Adiciona as que vieram do tablet
                        foreach (var ii in model.VerificacaoTipificacaoResultados.Where(r => r.Chave == verificacao.Chave))
                        {
                            var verificacaoGRT = new VerificacaoTipificacaoV2();
                            if (ii.AreasParticipantesId == "null")
                            {
                                dynamic caracteristicaTipificacao = QueryNinja(dbSgq, "SELECT * FROM CaracteristicaTipificacao WHERE cNrCaracteristica = " + ii.CaracteristicaTipificacaoId).FirstOrDefault();

                                if (caracteristicaTipificacao == null)
                                    continue;

                                verificacaoGRT.GRT_nCdCaracteristicaTipificacao = caracteristicaTipificacao.nCdCaracteristica; //GRT_nCdCaracteristicaTipificacao EX: 83
                                verificacaoGRT.cIdentificadorTipificacao = caracteristicaTipificacao.cIdentificador;// cIdentificadorTipificacao EX: <CONTUSAO>
                                verificacaoGRT.cNmCaracteristica = caracteristicaTipificacao.cNmCaracteristica;//cNmCaracteristica EX: CONTUSÃO - 2 CONTRA FILÉ
                                verificacaoGRT.cSgCaracteristica = caracteristicaTipificacao.cSgCaracteristica;//cNmCaracteristica EX: CONTUSÃO - 2 CONTRA FILÉ

                                verificacaoListGRT.Add(verificacaoGRT);
                            }
                            else
                            {
                                dynamic aresParticipante = QueryNinja(dbSgq, "SELECT * FROM AreasParticipantes WHERE cNrCaracteristica = " + ii.AreasParticipantesId).FirstOrDefault();

                                verificacaoGRT.GRT_nCdCaracteristicaTipificacao = aresParticipante.nCdCaracteristica;
                                verificacaoGRT.cIdentificadorTipificacao = aresParticipante.cIdentificador;
                                verificacaoGRT.cNmCaracteristica = aresParticipante.cNmCaracteristica;
                                verificacaoGRT.cSgCaracteristica = aresParticipante.cSgCaracteristica;

                                verificacaoListGRT.Add(verificacaoGRT);

                            }

                        }

                        var listRemoverGRT = new List<int>();
                        var listRemoverJBS = new List<int>();
                        foreach (var grt in verificacaoListGRT)
                        {
                            var verificacaoSalvarObj = new VerificacaoTipificacaoV2();
                            var comparacaoExistenteJBS = verificacaoListJBS.FirstOrDefault(r => r.cIdentificadorTipificacao == grt.cIdentificadorTipificacao && r.JBS_nCdCaracteristicaTipificacao == grt.GRT_nCdCaracteristicaTipificacao);
                            if (comparacaoExistenteJBS != null)
                            {

                                verificacaoSalvarObj = new VerificacaoTipificacaoV2()
                                {
                                    GRT_nCdCaracteristicaTipificacao = grt.GRT_nCdCaracteristicaTipificacao,
                                    JBS_nCdCaracteristicaTipificacao = comparacaoExistenteJBS.JBS_nCdCaracteristicaTipificacao,
                                    cNmCaracteristica = grt.cNmCaracteristica,
                                    cIdentificadorTipificacao = grt.cIdentificadorTipificacao,
                                    cSgCaracteristica = grt.cSgCaracteristica
                                    // ResultadoComparacaoGRT_JBS = resultado
                                };

                                verificacaoSalvar.Add(verificacaoSalvarObj);
                                listRemoverGRT.Add(verificacaoListGRT.IndexOf(grt));
                                listRemoverJBS.Add(verificacaoListJBS.IndexOf(comparacaoExistenteJBS));
                            }
                        }

                        listRemoverJBS = listRemoverJBS.Distinct().ToList();

                        foreach (var i in listRemoverGRT.OrderByDescending(r => r))
                            verificacaoListGRT.RemoveAt(i);

                        foreach (var i in listRemoverJBS.OrderByDescending(r => r))
                            verificacaoListJBS.RemoveAt(i);

                        if (verificacaoListGRT.Count > 0)
                            verificacaoSalvar.AddRange(verificacaoListGRT);

                        if (verificacaoListJBS.Count > 0)
                            verificacaoSalvar.AddRange(verificacaoListJBS);

                        var key = model.VerificacaoTipificacao[0].Chave;

                        List<VerificacaoTipificacaoV2> jaExiste = new List<VerificacaoTipificacaoV2>();
                        using (Factory factory = new Factory("DefaultConnection"))
                        {
                            string consultaVerificacaoTipificacaoV2 = $"select * from VerificacaoTipificacaoV2 where [key] = '{key}'";
                            jaExiste = factory.SearchQuery<VerificacaoTipificacaoV2>(consultaVerificacaoTipificacaoV2);

                            if (jaExiste.IsNotNull())
                            {
                                string deleteVerificacaoTipificacaoV2 = $"delete VerificacaoTipificacaoV2 where [key] = '{key}'";
                                 factory.ExecuteSql(deleteVerificacaoTipificacaoV2);
                            }
                        }

                        foreach (var ver in verificacaoSalvar)
                        {
                            ver.AddDate = DateTime.Now;
                            ver.ParCompany_Id = model.UnidadeId; //ParCompany_Id
                            ver.UserSgq_Id = model.AuditorId; //UserSgq_Id
                            ver.Sequencial = model.VerificacaoTipificacao[0].Sequencial; //Sequencial
                            ver.Banda = model.VerificacaoTipificacao[0].Banda; //Banda
                            ver.CollectionDate = model.VerificacaoTipificacao[0].DataHora;
                            ver.Key = model.VerificacaoTipificacao[0].Chave;
                            ver.ResultadoComparacaoGRT_JBS = ver.GRT_nCdCaracteristicaTipificacao.GetValueOrDefault() == ver.JBS_nCdCaracteristicaTipificacao.GetValueOrDefault();
                            if (jaExiste.IsNotNull()) { ver.AlterDate = DateTime.Now; }
                            dbSgq.VerificacaoTipificacaoV2.Add(ver);// Resultado
                        }

                        dbSgq.SaveChanges();

                    }
                    #endregion
                }
                catch (Exception e)
                {
                    LogSystem.LogErrorBusiness.Register(new Exception("Erro ao Savar verificação da Tipificação, Objeto bkp salvo.", e), model);
                    controleErro = true;
                    //throw ;
                }

                if (!controleErro)
                {
                    try
                    {
                        string[] nome = new string[] { "<CONTUSAO>", "<GORDURA>" };

                        foreach (var variavel in nome)
                        {
                            #region Query


                            var sql = "" +
                                "\n DECLARE @TIPO VARCHAR(20) = '" + variavel + "'                                                                                                            " +
                                "\n DECLARE @UNIDADE INT = " + verificacao.UnidadeId.ToString() + "                                                                                                 " +
                                "\n DECLARE @SEQ INT = " + verificacao.Sequencial.ToString() + "                                                                                                    " +
                                "\n DECLARE @BANDA INT = " + verificacao.Banda.ToString() + "                                                                                                       " +
                                "\n DECLARE @DATA DATE = '" + verificacao.DataHora.ToString("yyyyMMdd") + "'                                                                                        " +

                                "\n INSERT INTO CollectionJson                                                                                                                                " +
                                "\n                                                                                                                                                           " +
                                "\n select                                                                                                                                                    " +
                                "\n ParCompany_Id as unit_Id,                                                                                                                                 " +
                                "\n 1 as Shift,                                                                                                                                               " +
                                "\n 1 as Period,                                                                                                                                              " +
                                "\n 24 as level01_Id,                                                                                                                                         " +
                                "\n CollectionDate as Level01CollectionDate,                                                                                                                  " +
                                "\n 100 as level02_Id,                                                                                                                                        " +
                                "\n 0 as Evaluate,                                                                                                                                            " +
                                "\n 0 as Sample,                                                                                                                                              " +
                                "\n UserSgq_Id as Auditor_Id,                                                                                                                                 " +
                                "\n CollectionDate as Level02CollectionDate,                                                                                                                  " +
                                "\n (                                                                                                                                                         " +
                                "\n     SELECT                                                                                                                                                " +
                                "\n                                                                                                                                                           " +
                                "\n     ';' + --CABECALHOS                                                                                                                                    " +
                                "\n                                                                                                                                                           " +
                                "\n     '0;' + --phase                                                                                                                                        " +
                                "\n                                                                                                                                                           " +
                                "\n     format(CollectionDate, 'MMddyyyy') + ';' + --DATA                                                                                                     " +
                                "\n                                                                                                                                                           " +
                                "\n     'undefined;' + --consecultivefailurelevel                                                                                                             " +
                                "\n                                                                                                                                                           " +
                                "\n     'undefined;' + --consecultivefailuretotal                                                                                                             " +
                                "\n                                                                                                                                                           " +
                                "\n     'false;' + --NA                                                                                                                                       " +
                                "\n                                                                                                                                                           " +
                                "\n     'undefined;' + --completed                                                                                                                            " +
                                "\n                                                                                                                                                           " +
                                "\n     'undefined;' + --havePhases                                                                                                                           " +
                                "\n                                                                                                                                                           " +
                                "\n     'undefined;' + --collectiolevel2_id                                                                                                                   " +
                                "\n                                                                                                                                                           " +
                                "\n     'undefined;' + --correctiveActionCompleted                                                                                                            " +
                                "\n                                                                                                                                                           " +
                                "\n     'undefined;' + --Completed Reaudit                                                                                                                    " +
                                "\n                                                                                                                                                           " +
                                "\n     '0;' + --alertLevel                                                                                                                                   " +
                                "\n                                                                                                                                                           " +
                                "\n     cast(sequencial as varchar) + ';' + --sequencial                                                                                                      " +
                                "\n                                                                                                                                                           " +
                                "\n     cast(banda as varchar) + ';' + --side                                                                                                                 " +
                                "\n                                                                                                                                                           " +
                                "\n     '1;' + --weiEva                                                                                                                                       " +
                                "\n     case when SUM(CAST(ResultadoComparacaoGRT_JBS AS INT)) <> COUNT(ResultadoComparacaoGRT_JBS) then '1;' else '0;' end + --weiDefects                    " +
                                "\n     case when SUM(CAST(ResultadoComparacaoGRT_JBS AS INT)) <> COUNT(ResultadoComparacaoGRT_JBS) then '1;' else '0;' end + --Defects                       " +
                                "\n     case when SUM(CAST(ResultadoComparacaoGRT_JBS AS INT)) <> COUNT(ResultadoComparacaoGRT_JBS) then '1;' else '0;' end + --totalLevel3com defeitos       " +
                                "\n     case when SUM(CAST(ResultadoComparacaoGRT_JBS AS INT)) <> COUNT(ResultadoComparacaoGRT_JBS) then '1;' else '0;' end + --totalLevel2 evaluate          " +
                                "\n                                                                                                                                                           " +
                                "\n     '0;' + --avaliação ultimo alerta                                                                                                                      " +
                                "\n                                                                                                                                                           " +
                                "\n     '1;' + --Eva result                                                                                                                                   " +
                                "\n     case when SUM(CAST(ResultadoComparacaoGRT_JBS AS INT)) <> COUNT(ResultadoComparacaoGRT_JBS) then '1;' else '0;' end + --DefectsResult                 " +
                                "\n                                                                                                                                                           " +
                                "\n     cast(sequencial as varchar) + ';' + --sequencial                                                                                                      " +
                                "\n                                                                                                                                                           " +
                                "\n     cast(banda as varchar) + ';' + --side                                                                                                                 " +
                                "\n                                                                                                                                                           " +
                                "\n     'false;' + --isEmptyLevel3                                                                                                                            " +
                                "\n                                                                                                                                                           " +
                                "\n     'false;' + --hasSampleTotal                                                                                                                           " +
                                "\n                                                                                                                                                           " +
                                "\n     '5;' + --hashkey                                                                                                                                      " +
                                "\n                                                                                                                                                           " +
                                "\n     '0;' + --monitoraemntoUltimo Alerta                                                                                                                   " +
                                "\n                                                                                                                                                           " +
                                "\n     'undefined;0;;3'                                                                                                                                           " +
                                "\n                                                                                                                                                           " +
                                "\n                                                                                                                                                           " +
                                "\n     from VerificacaoTipificacaoV2                                                                                                                         " +
                                "\n                                                                                                                                                           " +
                                "\n     where cIdentificadorTipificacao = @TIPO                                                                                                               " +
                                "\n                                                                                                                                                           " +
                                "\n     AND ParCompany_Id = @UNIDADE                                                                                                                          " +
                                "\n                                                                                                                                                           " +
                                "\n     AND Sequencial = @SEQ                                                                                                                                 " +
                                "\n                                                                                                                                                           " +
                                "\n     AND Banda = @BANDA                                                                                                                                    " +
                                "\n                                                                                                                                                           " +
                                "\n     AND CAST(CollectionDate as DATE) = @DATA                                                                                                              " +
                                "\n                                                                                                                                                           " +
                                "\n     GROUP BY userSgq_Id, cIdentificadorTipificacao, CollectionDate, Sequencial, Banda                                                                     " +
                                "\n ) as Level02HeaderJson,                                                                                                                                   " +
                                "\n (                                                                                                                                                         " +
                                "\n     SELECT                                                                                                                                                " +
                                "\n                                                                                                                                                           " +
                                "\n     '<level03>' +                                                                                                                                         " +
                                "\n     CASE                                                                                                                                                  " +
                                "\n                                                                                                                                                           " +
                                "\n     WHEN cIdentificadorTipificacao = '<GORDURA>' THEN '1058,'-- level3 id GORDURA                                                                         " +
                                "\n   WHEN cIdentificadorTipificacao = '<CONTUSAO>' THEN '1059,'-- level3 id CONTUSAO                                                                         " +
                                "\n                                                                                                                                                           " +
                                "\n     END +                                                                                                                                                 " +
                                "\n     format(CollectionDate, 'MM/dd/yyyy hh:mm:ss') + ',' + --DATA                                                                                          " +
                                "\n                                                                                                                                                           " +
                                "\n     ',' + --VALOR                                                                                                                                         " +
                                "\n     case when SUM(CAST(ResultadoComparacaoGRT_JBS AS INT)) <> COUNT(ResultadoComparacaoGRT_JBS) then 'false,' else 'true,' end + --Conforme               " +
                                "\n                                                                                                                                                           " +
                                "\n     CAST(userSgq_Id AS VARCHAR) + ',' + --auditorId                                                                                                       " +
                                "\n     case when SUM(CAST(ResultadoComparacaoGRT_JBS AS INT)) <> COUNT(ResultadoComparacaoGRT_JBS) then '1,' else '0,' end + --total Erros                   " +
                                "\n                                                                                                                                                           " +
                                "\n     'null,' + --valueText                                                                                                                                 " +
                                "\n                                                                                                                                                           " +
                                "\n     'undefined,' + --result_level3_id                                                                                                                     " +
                                "\n                                                                                                                                                           " +
                                "\n     '1,' + --peso                                                                                                                                         " +
                                "\n                                                                                                                                                           " +
                                "\n     ',' + --nome level3                                                                                                                                   " +
                                "\n                                                                                                                                                           " +
                                "\n     '0,' + --intervalMin                                                                                                                                  " +
                                "\n                                                                                                                                                           " +
                                "\n     '0,' + --intervalMax                                                                                                                                  " +
                                "\n                                                                                                                                                           " +
                                "\n     'false,' + --NA                                                                                                                                       " +
                                "\n                                                                                                                                                           " +
                                "\n     '0,' + --punição                                                                                                                                      " +
                                "\n     case when SUM(CAST(ResultadoComparacaoGRT_JBS AS INT)) <> COUNT(ResultadoComparacaoGRT_JBS) then '1,' else '0,' end + +--defeitos                     " +
                                "\n                                                                                                                                                           " +
                                "\n     '1,' + --avaliacao com peso                                                                                                                           " +
                                "\n     case when SUM(CAST(ResultadoComparacaoGRT_JBS AS INT)) <> COUNT(ResultadoComparacaoGRT_JBS) then '1,' else '0,' end + +--defeitos com peso            " +
                                "\n                                                                                                                                                           " +
                                "\n     '</level03>'                                                                                                                                          " +
                                "\n                                                                                                                                                           " +
                                "\n     from VerificacaoTipificacaoV2                                                                                                                         " +
                                "\n                                                                                                                                                           " +
                                "\n     where cIdentificadorTipificacao = @TIPO                                                                                                               " +
                                "\n                                                                                                                                                           " +
                                "\n     AND ParCompany_Id = @UNIDADE                                                                                                                          " +
                                "\n                                                                                                                                                           " +
                                "\n     AND Sequencial = @SEQ                                                                                                                                 " +
                                "\n                                                                                                                                                           " +
                                "\n     AND Banda = @BANDA                                                                                                                                    " +
                                "\n                                                                                                                                                           " +
                                "\n     AND CAST(CollectionDate as DATE) = @DATA                                                                                                              " +
                                "\n                                                                                                                                                           " +
                                "\n     GROUP BY userSgq_Id, cIdentificadorTipificacao, CollectionDate                                                                                        " +
                                "\n ) as Level03ResultJSon,                                                                                                                                   " +
                                "\n '' as CorrectiveActionJson,                                                                                                                               " +
                                "\n 0 as Reaudit,                                                                                                                                             " +
                                "\n 0 as ReauditNumber,                                                                                                                                       " +
                                "\n 0 as haveReaudit,                                                                                                                                         " +
                                "\n 0 as haveCorrectiveAction,                                                                                                                                " +
                                "\n 'null' as Device_Id,                                                                                                                                      " +
                                "\n 'verificacao' as AppVersion,                                                                                                                              " +
                                "\n 'JBS' as Ambient,                                                                                                                                         " +
                                "\n 0 as IsProcessed,                                                                                                                                         " +
                                "\n 1 as Device_Mac,                                                                                                                                          " +
                                "\n getdate() as AddDate,                                                                                                                                     " +
                                "\n null as AlterDate,                                                                                                                                        " +
                                "\n '111111' as [Key],                                                                                                                                        " +
                                "\n null as TTP,                                                                                                                                              " +
                                "\n 0 as ReauditLevel                                                                                                                                         " +
                                "\n from VerificacaoTipificacaoV2                                                                                                                             " +
                                "\n where cIdentificadorTipificacao = @TIPO                                                                                                                   " +
                                "\n                                                                                                                                                           " +
                                "\n     AND ParCompany_Id = @UNIDADE                                                                                                                          " +
                                "\n                                                                                                                                                           " +
                                "\n     AND Sequencial = @SEQ                                                                                                                                 " +
                                "\n                                                                                                                                                           " +
                                "\n     AND Banda = @BANDA                                                                                                                                    " +
                                "\n                                                                                                                                                           " +
                                "\n     AND CAST(CollectionDate as DATE) = @DATA                                                                                                              " +
                                "\n SELECT @@IDENTITY AS 'Identity'                                                                                                                           ";
                            #endregion

                            #region Salvando Consolidação
                            using (SqlConnection connection = new SqlConnection(conexao))
                            {
                                connection.Open();

                                SqlCommand command;

                                command = new SqlCommand(sql, connection);
                                var iSql = Convert.ToInt32(command.ExecuteScalar());

                                if (iSql > 0)
                                {
                                    //if (autoSend == true)
                                    //{
                                    SyncServiceApiController sync = new SyncServiceApiController(conexao,conexao);
                                    sync.ProcessJson(null, iSql, false);

                                    //}
                                }

                                else
                                {
                                    //Se não ocorre sem problemas, retorna um erro
                                    throw new Exception("erro json");

                                }

                            }
                            #endregion
                        }
                    }
                    catch (Exception e)
                    {
                        LogSystem.LogErrorBusiness.Register(new Exception("Erro ao Salvar GORDURA E CONTUSAO verificação da Tipificação, Objeto bkp salvo.", e), model);
                        //throw ;
                    }
                }
            }

        }

        //QueryGenerica para implementar
        protected List<JObject> QueryNinja(DbContext db, string query)
        {
            db.Database.Connection.Open();
            var cmd = db.Database.Connection.CreateCommand();
            cmd.CommandText = query;
            cmd.CommandTimeout = 300;
            var reader = cmd.ExecuteReader();
            List<JObject> items = new List<JObject>();
            while (reader.Read())
            {
                JObject row = new JObject();
                for (int i = 0; i < reader.FieldCount; i++)
                    row[reader.GetName(i)] = reader[i].ToString();

                items.Add(row);
            }
            db.Database.Connection.Close();
            return items;
        }

        public string GetVTVerificacaoTipificacao(String Date, int UnidadeId)
        {
            string retorno = "";
            try
            {

                using (Factory factory = new Factory("DefaultConnection"))
                {
                    var sql = "select distinct '<div class=\"Key\" date=\"" + Date + "\" unidadeid=\"" + UnidadeId + "\" key=\"'+ [Key] +'\"></div>' as retorno from VerificacaoTipificacaoV2 (nolock)  " +
                          " where FORMAT(CollectionDate, 'MMddyyyy') = '" + Date + "' and        " +
                          " ParCompany_Id = " + UnidadeId + ";                              ";

                    var list = factory.SearchQuery<ResultadoUmaColuna>(sql).ToList();

                    for (var i = 0; i < list.Count(); i++)
                    {
                        retorno += list[i].retorno.ToString();
                    }
                }


            }
            catch (Exception e)
            {
                LogSystem.LogErrorBusiness.Register(new Exception("Erro ao listar VTVerificacaoTipificacao.", e));
            }

            return retorno;

        }
    }



}