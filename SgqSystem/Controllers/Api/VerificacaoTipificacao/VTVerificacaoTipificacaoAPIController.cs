using Dapper;
using Dominio;
using DTO;
using DTO.Helpers;
using SgqSystem.Handlres;
using SgqSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace SgqSystem.Controllers.Api
{
    [HandleApi()]
    [RoutePrefix("api/VTVerificacaoTipificacao")]
    public class VTVerificacaoTipificacaoApiController : ApiController

    {
        public string mensagemErro { get; set; }

        string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;

        [Route("Save")]
        [HttpPost]
        public void SaveVTVerificacaoTipificacao(TipificacaoViewModel model)
        {


            var _verificacao = model.VerificacaoTipificacao;

            if (model.VerificacaoTipificacaoResultados.Count == 0)
                return;

            using (var db = new SGQ_GlobalEntities())
            {
                var verificacaoTipificacao = db.VTVerificacaoTipificacao.FirstOrDefault(r => r.Chave == _verificacao.Chave);

                DateTime dataHoraTipificacao = _verificacao.DataHora;

                if (verificacaoTipificacao != null)
                {
                    //Atribui a data da verificação que for encontrada
                    dataHoraTipificacao = verificacaoTipificacao.DataHora;

                    string queryValidacaoVDelete = "DELETE FROM VTVerificacaoTipificacaoResultados WHERE chave='" + verificacaoTipificacao.Chave + "'";
                    int noOfRowDeleted = db.Database.ExecuteSqlCommand(queryValidacaoVDelete);


                    string queryValidacaoVDelete1 = "DELETE FROM VTVerificacaoTipificacao WHERE id='" + verificacaoTipificacao.Id + "'";
                    int noOfRowDeleted1 = db.Database.ExecuteSqlCommand(queryValidacaoVDelete1);
                    

                }
                verificacaoTipificacao = new VTVerificacaoTipificacao();

                verificacaoTipificacao.Sequencial = _verificacao.Sequencial;
                verificacaoTipificacao.Banda = _verificacao.Banda;
                verificacaoTipificacao.DataHora = dataHoraTipificacao;
                verificacaoTipificacao.UnidadeId = _verificacao.UnidadeId;
                verificacaoTipificacao.Chave = _verificacao.Chave;
                verificacaoTipificacao.Status = false;
                verificacaoTipificacao.EvaluationNumber = _verificacao.EvaluationNumber;
                verificacaoTipificacao.Sample = _verificacao.Sample;
                
                db.VTVerificacaoTipificacao.Add(verificacaoTipificacao);

                for (var i = 0; i < model.VerificacaoTipificacaoResultados.Count; i++)
                {
                    if (model.VerificacaoTipificacaoResultados[i].CaracteristicaTipificacaoId == "null")
                    {
                        string aId = model.VerificacaoTipificacaoResultados[i].AreasParticipantesId;

                        var AreaParticObj = (from x in db.AreasParticipantes.AsNoTracking()
                                             where x.cNrCaracteristica == aId
                                             select x).FirstOrDefault();


                        var numeroCaracteristica = AreaParticObj.cNrCaracteristica;

                        numeroCaracteristica = numeroCaracteristica.Substring(0, 4);

                        var codigoCaracteristica = (from x in db.AreasParticipantes.AsNoTracking()
                                                    where x.cNrCaracteristica == numeroCaracteristica
                                                    select x.nCdCaracteristica).FirstOrDefault();

                        var codigoTarefa = (from x in db.VerificacaoTipificacaoTarefaIntegracao.AsNoTracking()
                                            where x.CaracteristicaTipificacaoId == codigoCaracteristica
                                            select x.TarefaId).FirstOrDefault();

                        VTVerificacaoTipificacaoResultados verificacaoTipificacaoResultados = new VTVerificacaoTipificacaoResultados();
                        verificacaoTipificacaoResultados.AreasParticipantesId = Convert.ToInt32(model.VerificacaoTipificacaoResultados[i].AreasParticipantesId);
                        verificacaoTipificacaoResultados.TarefaId = codigoTarefa;
                        verificacaoTipificacaoResultados.Chave = _verificacao.Chave;


                        db.VTVerificacaoTipificacaoResultados.Add(verificacaoTipificacaoResultados);

                    }
                    else
                    {
                        var idCaracteristicaTipificacaoTemp = model.VerificacaoTipificacaoResultados[i].CaracteristicaTipificacaoId;

                        var obj = (from x in db.CaracteristicaTipificacao.AsNoTracking()
                                   where x.cNrCaracteristica == idCaracteristicaTipificacaoTemp
                                   select x).FirstOrDefault();

                        var numeroCaracteristica = obj.cNrCaracteristica;
                        numeroCaracteristica = numeroCaracteristica.Substring(0, 3);

                        var codigoCaracteristica = (from x in db.CaracteristicaTipificacao.AsNoTracking()
                                                    where x.cNrCaracteristica == numeroCaracteristica
                                                    select x.nCdCaracteristica).FirstOrDefault();

                        var codigoTarefa = (from x in db.VerificacaoTipificacaoTarefaIntegracao.AsNoTracking()
                                            where x.CaracteristicaTipificacaoId == codigoCaracteristica
                                            select x.TarefaId).FirstOrDefault();

                        VTVerificacaoTipificacaoResultados verificacaoTipificacaoResultados = new VTVerificacaoTipificacaoResultados();
                        verificacaoTipificacaoResultados.CaracteristicaTipificacaoId = Convert.ToInt32(model.VerificacaoTipificacaoResultados[i].CaracteristicaTipificacaoId);
                        verificacaoTipificacaoResultados.TarefaId = codigoTarefa;
                        verificacaoTipificacaoResultados.Chave = _verificacao.Chave;
                        
                        db.VTVerificacaoTipificacaoResultados.Add(verificacaoTipificacaoResultados);

                        
                    }

                }
                
                db.SaveChanges();
                
                try
                {
                    GetDadosGet(_verificacao.Chave);
                }
                catch (Exception e)
                {
                    //throw new Exception("Exception GetDadosGet primeira chamada", e);
                    new DTO.CreateLog(new Exception("Exception GetDadosGet primeira chamada"));
                }

                var inicioSemana = verificacaoTipificacao.DataHora.AddDays(-(int)verificacaoTipificacao.DataHora.DayOfWeek);

                var listVT = db.VTVerificacaoTipificacao.Where(x => x.Status == false && x.DataHora >= inicioSemana).ToList();

                foreach (VTVerificacaoTipificacao vt in listVT)
                {
                    try
                    {
                        GetDadosGet(vt.Chave);
                    }
                    catch (SqlException ex)
                    {
                        //throw new Exception("SqlException GetDadosGet reconsolidação", ex);
                        new DTO.CreateLog(new Exception("SqlException GetDadosGet reconsolidação"));
                    }
                    catch (Exception ex)
                    {
                        //throw new Exception("Exception GetDadosGet reconsolidação", ex);
                        new DTO.CreateLog(new Exception("Exception GetDadosGet reconsolidação"));
                    }
                }

            }

            
        }
        public void connectionString(int parCompany_Id, ref string conexao, ref ParCompany company)
        {
            try
            {

                using (var db = new SgqDbDevEntities())
                {
                    string _user = "UserGQualidade";
                    string _password = "grJsoluco3s";

                    var parCompany = (from p in db.ParCompany
                                      where p.Id == parCompany_Id
                                      select p).FirstOrDefault();

                    if (parCompany.DBServer.ToLower() == "sgqdbdev")
                    {
                        //unidades.EnderecoIP = "mssql1.gear.host";
                        //unidades.NomeDatabase = "GRJQualidadeDev";
                        _user = "sa";
                        _password = "1qazmko0";
                    }


                   
                    if(parCompany != null)
                    {
                        string porta = null;

                         conexao = "data source=" + parCompany.IPServer + porta + ";initial catalog=" + parCompany.DBServer + ";persist security info=True;user id=" + _user + ";password=" + _password + ";";
                        company = parCompany;
                    }
                }                   
            }
            catch (Exception ex)
            {
                string mensagem = ex.Message;
            }
        }

        [Route("Consolidation")]
        [HttpPost]
        public System.Web.Mvc.JsonResult GetDadosGet(string verificacaoTipificacaoChave)//codigo só pra teste
        {
            var SgqSystem = new SgqSystem.Services.SyncServices();
            var db = new SGQ_GlobalEntities();
            using (db)
            {
                var verificacaoTipificacao = (from p in db.VTVerificacaoTipificacao
                                              where p.Chave == verificacaoTipificacaoChave
                                              select p).FirstOrDefault();
                
                if (verificacaoTipificacao == null)
                {
                    return null;
                }
                verificacaoTipificacao.Status = false;

                bool existeComparacao = false;

                bool excluirVerificacaoAntiga = false;

                string conexao = null;
                var company = new ParCompany();

                connectionString(verificacaoTipificacao.UnidadeId, ref conexao, ref company);

                string queryString = "exec FBED_GRTTipificacaoCaracteristica " + company.CompanyNumber + ", '" + verificacaoTipificacao.DataHora.ToString("yyyyMMdd") + "', " + verificacaoTipificacao.Sequencial;

                if (GlobalConfig.MockOn)
                {
                    //queryString = "select * from verificacaoteste where nCdEmpresa=" + company.CompanyNumber + " and iSequencial=" + verificacaoTipificacao.Sequencial;
                    conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;
                }


                //queryString = "SELECT 1";
                int iSequencial = 0;
                int iBanda = 0;
                DateTime dataHoraMonitor = DateTime.Now;

                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    try
                    {
                        SqlCommand command = new SqlCommand(queryString, connection);
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            string queryZeroToNull = "UPDATE VTVerificacaoTipificacaoResultados SET AreasParticipantesId = NULL WHERE AreasParticipantesId='0'";
                            int noOfRowDeleted0 = db.Database.ExecuteSqlCommand(queryZeroToNull);

                            queryZeroToNull = "UPDATE VTVerificacaoTipificacaoResultados SET CaracteristicaTipificacaoId = NULL WHERE CaracteristicaTipificacaoId='0'";
                            noOfRowDeleted0 = db.Database.ExecuteSqlCommand(queryZeroToNull);
                            
                            string[] comparacao = { "<IDADE>", "<SEXO>", "<GORDURA>", "<CONTUSAO>", "<FALHAOP>" };
                            
                            while (reader.Read())
                            {

                                if (Convert.ToInt32(reader[4].ToString()) == verificacaoTipificacao.Banda)
                                {
                                    int nCdEmpresa = Convert.ToInt32(reader[0].ToString());
                                    DateTime dMovimento = Convert.ToDateTime(reader[1].ToString());
                                    dataHoraMonitor = dMovimento;
                                    iSequencial = Convert.ToInt32(reader[2].ToString());
                                    int iSequencialTipificacao = Convert.ToInt32(reader[3].ToString());
                                    iBanda = Convert.ToInt32(reader[4].ToString());
                                    string cIdentificadorTipificacao = reader[5].ToString();
                                    int nCdCaracteristicaTipificacao = Convert.ToInt32(reader[6].ToString());

                                    if (excluirVerificacaoAntiga == false)
                                    {
                                        string queryValidacaoVDelete = "DELETE FROM VTVerificacaoTipificacaoValidacao WHERE nCdEmpresa='" + nCdEmpresa + "' AND CAST(dMovimento AS DATE) ='" + dMovimento.ToString("yyyy-MM-dd 00:00:00") + "' AND iSequencial='" + iSequencial + "' AND iBanda='" + iBanda + "'";
                                        int noOfRowDeleted = db.Database.ExecuteSqlCommand(queryValidacaoVDelete);
                                        
                                        string queryValidacaoCDelete = "DELETE FROM VTVerificacaoTipificacaoComparacao WHERE nCdEmpresa='" + nCdEmpresa + "' AND CAST(DataHora AS DATE) = '" + dMovimento.ToString("yyyy-MM-dd 00:00:00") + "' AND Sequencial='" + iSequencial + "' AND Banda='" + iBanda + "'";
                                        noOfRowDeleted = db.Database.ExecuteSqlCommand(queryValidacaoCDelete);
                                        
                                        excluirVerificacaoAntiga = true;
                                        
                                    }

                                    if (MatrizStrinComparacao(comparacao, cIdentificadorTipificacao) == true)
                                    {
                                        existeComparacao = true;

                                        verificacaoTipificacao.Status = true;
                                        //Instanciamos um novo objeto da tabela VerificacaoTipificacaoValidacao
                                        var VerificacaoTipificacaoValidacao = new VTVerificacaoTipificacaoValidacao();
                                        //Incluimos o registro na tabela com 
                                        VerificacaoTipificacaoValidacao.nCdEmpresa = nCdEmpresa;
                                        VerificacaoTipificacaoValidacao.dMovimento = dMovimento;
                                        VerificacaoTipificacaoValidacao.iSequencial = iSequencial;
                                        VerificacaoTipificacaoValidacao.iSequencialTipificacao = iSequencialTipificacao;
                                        VerificacaoTipificacaoValidacao.iBanda = iBanda;
                                        VerificacaoTipificacaoValidacao.cIdentificadorTipificacao = cIdentificadorTipificacao;
                                        VerificacaoTipificacaoValidacao.nCdCaracteristicaTipificacao = nCdCaracteristicaTipificacao;
                                        db.VTVerificacaoTipificacaoValidacao.Add(VerificacaoTipificacaoValidacao);
                                        db.SaveChanges();
                                    }
                                }
                            }
                            db.SaveChanges();

                            iBanda = verificacaoTipificacao.Banda;
                            if (existeComparacao == false)
                            {
                                //mensagem
                            }
                            else
                            {
                                try
                                {

                                    string[] ArrayComparacao = { "<GORDURA>", "<CONTUSAO>" };
                                    int ParLevel1_Id = 0;

                                    using (var db2 = new SgqDbDevEntities())
                                    {

                                        var ParLevel1 = (from p in db2.ParLevel1
                                                         where p.hashKey == 5
                                                         select p).FirstOrDefault();

                                        ParLevel1_Id = ParLevel1.Id;

                                        var ParLevel2_old =
                                                   (from p1 in db2.ParLevel1
                                                   join p321 in db2.ParLevel3Level2Level1 on p1.Id equals p321.ParLevel1_Id
                                                   join p32 in db2.ParLevel3Level2 on p321.ParLevel3Level2_Id equals p32.Id
                                                   join p2 in db2.ParLevel2 on p32.ParLevel2_Id equals p2.Id
                                                    where p1.Id == ParLevel1.Id
                                                    select new { p2 }).FirstOrDefault();
                                       
                                        var ParLevel2 = ParLevel2_old.p2;
                                        
                                        var collectionLevel2 = (from p in db2.CollectionLevel2
                                                                where p.Key == verificacaoTipificacaoChave
                                                                select p).FirstOrDefault();

                                        if (collectionLevel2 != null)
                                        {
                                            var Result_Level3 = (from p in db2.Result_Level3
                                                                 where p.CollectionLevel2_Id == collectionLevel2.Id
                                                                 select p).ToList();

                                            foreach (var r in Result_Level3)
                                            {
                                                db2.Result_Level3.Remove(r);
                                            }

                                            db2.CollectionLevel2.Remove(collectionLevel2);

                                            db.SaveChanges();

                                        }

                                        SqlConnection dbService = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString);
                                        
                                        DateTime dataC = verificacaoTipificacao.DataHora;
                                        
                                        string sql = "SELECT * FROM ConsolidationLevel1 WHERE UnitId = '" + verificacaoTipificacao.UnidadeId + "' AND ParLevel1_Id= '" + ParLevel1.Id + "' AND CONVERT(date, ConsolidationDate) = '" + dataC.ToString("yyyy-MM-dd") + "'";

                                        var consolidationLevel1 = dbService.Query<SGQDBContext.ConsolidationLevel1>(sql).FirstOrDefault();

                                        var ConsolidationLevel1DB = new SGQDBContext.ConsolidationLevel1(dbService);
                                        var ConsolidationLevel2DB = new SGQDBContext.ConsolidationLevel2(dbService);

                                        if (consolidationLevel1 == null)
                                        {
                                            consolidationLevel1 = SgqSystem.InsertConsolidationLevel1(verificacaoTipificacao.UnidadeId, ParLevel1.Id, dataC);
                                            if (consolidationLevel1 == null)
                                            {
                                                //throw new Exception();
                                                return null;
                                            }
                                        }
                                        
                                        var consolidationLevel2 = ConsolidationLevel2DB.getByConsolidationLevel1(verificacaoTipificacao.UnidadeId, consolidationLevel1.Id, ParLevel2.Id);
                                        if (consolidationLevel2 == null)
                                        {
                                            consolidationLevel2 = SgqSystem.InsertConsolidationLevel2(consolidationLevel1.Id, ParLevel2.Id, verificacaoTipificacao.UnidadeId, dataC,false,0);
                                            if (consolidationLevel2 == null)
                                            {
                                                //throw new Exception();
                                                return null;
                                            }
                                        }

                                        collectionLevel2 = new CollectionLevel2();

                                        collectionLevel2.Key = verificacaoTipificacaoChave;
                                        
                                        collectionLevel2.ConsolidationLevel2_Id = consolidationLevel2.Id;
                                        collectionLevel2.ParLevel1_Id = ParLevel1.Id;
                                        collectionLevel2.ParLevel2_Id = ParLevel2.Id;
                                        collectionLevel2.UnitId = verificacaoTipificacao.UnidadeId;
                                        collectionLevel2.AuditorId = 1;
                                        collectionLevel2.Shift = 1;
                                        collectionLevel2.Period = 1;
                                        collectionLevel2.Phase = 1;
                                        collectionLevel2.ReauditIs = false;
                                        collectionLevel2.ReauditNumber = 1;
                                        collectionLevel2.CollectionDate = verificacaoTipificacao.DataHora;
                                        collectionLevel2.StartPhaseDate = DateTime.MinValue;
                                        collectionLevel2.EvaluationNumber = verificacaoTipificacao.EvaluationNumber.GetValueOrDefault();
                                        collectionLevel2.Sample = verificacaoTipificacao.Sample.GetValueOrDefault();
                                        collectionLevel2.AddDate = DateTime.Now;
                                        collectionLevel2.ConsecutiveFailureIs = false;
                                        collectionLevel2.ConsecutiveFailureTotal = 0;
                                        collectionLevel2.NotEvaluatedIs = false;
                                        collectionLevel2.Duplicated = false;
                                        collectionLevel2.HaveCorrectiveAction = false;
                                        collectionLevel2.HaveReaudit = false;
                                        collectionLevel2.HavePhase = false;
                                        collectionLevel2.Completed = false;
                                        collectionLevel2.Sequential = iSequencial;
                                        collectionLevel2.Side = iBanda;


                                        db2.CollectionLevel2.Add(collectionLevel2);
                                        db2.SaveChanges();

                                        var ParLevel3List = (from p in db2.ParLevel3
                                                             select p).ToList();

                                        int defectsL2 = 0;

                                        for (var i = 0; i < ArrayComparacao.Length; i++)
                                        {
                                            

                                            string caracteristica = ArrayComparacao[i];

                                            var resultIdTarefa = (from x in db.VerificacaoTipificacaoTarefaIntegracao
                                                                  join y in db.CaracteristicaTipificacao
                                                                  on x.CaracteristicaTipificacaoId equals y.nCdCaracteristica
                                                                  where y.cIdentificador.Equals(caracteristica)
                                                                  select x).FirstOrDefault().TarefaId;

                                            var ParLevel3 = (from p in ParLevel3List
                                                             where p.Id == resultIdTarefa
                                                             select p).FirstOrDefault();

                                            if(ParLevel3 == null)
                                                new DTO.CreateLog(new Exception("O ParLevel3 está nulo"));


                                            bool conforme = true;

                                            verificacaoTipificaoComparacao(company.CompanyNumber.ToString(), verificacaoTipificacao.DataHora.ToString("yyyyMMdd"), 
                                                                           verificacaoTipificacao.Sequencial.ToString(), iBanda.ToString(), null,
                                                                           verificacaoTipificacao.UnidadeId.ToString(), "1", "1", conexao,
                                                                           caracteristica, ref conforme);
                                            int defectsL3 = 0;
                                            if(conforme == false)
                                            {
                                                defectsL3++;
                                                defectsL2++;
                                            }

                                            var result = new Result_Level3();
                                            result.CollectionLevel2_Id = collectionLevel2.Id;

                                            //***trocar/*******//
                                            result.ParLevel3_Id = ParLevel3.Id;
                                            result.ParLevel3_Name = ParLevel3.Name;
                                            result.Weight = 1;
                                            result.IntervalMin = "0";
                                            result.IntervalMax = "0";
                                            result.Value = "0";
                                            result.IsConform = conforme;
                                            result.IsNotEvaluate = false;
                                            result.Defects = defectsL3;
                                            result.PunishmentValue = 0;
                                            result.WeiEvaluation = ParLevel2.ParLevel3Level2.Where(x => x.ParLevel3_Id == ParLevel3.Id).FirstOrDefault().Weight;
                                            result.Evaluation = 1;
                                            result.WeiDefects = ParLevel2.ParLevel3Level2.Where(x => x.ParLevel3_Id == ParLevel3.Id).FirstOrDefault().Weight * defectsL3;

                                            db2.Result_Level3.Add(result);
                                            db2.SaveChanges();

                                        }

                                        collectionLevel2.Defects = defectsL2;

                                        //campos faltando
                                        collectionLevel2.ParFrequency_Id = ParLevel1.ParFrequency_Id;                                   
                                        collectionLevel2.TotalLevel3Evaluation = 1;
                                        collectionLevel2.LastEvaluationAlert = 0;
                                        collectionLevel2.EvaluatedResult = 1;

                                        collectionLevel2.TotalLevel3WithDefects = 0;
                                        collectionLevel2.DefectsResult = 0;

                                        collectionLevel2.WeiEvaluation = (from p in db2.Result_Level3
                                                                          where p.CollectionLevel2_Id == collectionLevel2.Id
                                                                          select p).Sum(p => p.WeiEvaluation); //peso  

                                        if(collectionLevel2.WeiEvaluation == null)
                                        {
                                            collectionLevel2.WeiEvaluation = 0;
                                        }

                                        collectionLevel2.WeiDefects = (from p in db2.Result_Level3
                                                                          where p.CollectionLevel2_Id == collectionLevel2.Id
                                                                          select p).Sum(p => p.WeiDefects); //se tiver defeitos = peso , senao 0 

                                        if (collectionLevel2.WeiDefects == null)
                                        {
                                            collectionLevel2.WeiDefects = 0;
                                        }

                                        if ((from p in db2.Result_Level3
                                         where p.CollectionLevel2_Id == collectionLevel2.Id
                                         && p.Defects > 0
                                            select p).ToList().Count() > 0)
                                        {
                                            collectionLevel2.DefectsResult = 1;//se tiver defeitos = 1 , senao 0
                                            collectionLevel2.TotalLevel3WithDefects = 1; //se tiver defeitos = 1 , senao 0
                                        }

                                        collectionLevel2.IsEmptyLevel3 = false;
                                        collectionLevel2.LastLevel2Alert = 0;
                                        collectionLevel2.ReauditLevel = 0;
                                        collectionLevel2.StartPhaseEvaluation = 0;
                                        collectionLevel2.CounterDonePhase = 0;

                                        verificacaoTipificacao.Status = true;
                                        db2.SaveChanges();
                                        db.SaveChanges();
                                        
                                    }

                                    SgqSystem._ReConsolidationByLevel1(verificacaoTipificacao.UnidadeId, ParLevel1_Id, verificacaoTipificacao.DataHora);

                                    //return null;

                                }
                                catch (Exception ex)
                                {
                                    //throw new Exception("Deu merda no número 1 ", ex);
                                    new DTO.CreateLog(new Exception("Deu merda no número 1"));
                                }
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        //throw new Exception("Deu merda no número 2 ", ex);
                        new DTO.CreateLog(new Exception("Deu merda no número 2"));
                    }
                }

            }
            //mensagem de erro
            //return Json(mensagem("Não foi possível executar a verificação de tipificação. Aguarde alguns instantes e tente novamente. Se o problema persistir entre em contato com o suporte!", alertaTipo.warning, reenviarRequisicao: true), JsonRequestBehavior.AllowGet);



            return null;
        }


        public bool MatrizStrinComparacao(string[] matriz, string valorComparar)
        {
            //retornar uma mensatgem para a equipe de desenvolvimento
            if (matriz == null)
            {
                mensagemErro = "Informe a Matriz";
                return false;
            }
            if (string.IsNullOrEmpty(valorComparar))
            {
                mensagemErro = "Informe o Valor para Comparar com os dados da matriz";
                return false;
            }

            int strNumber = 0;
            int strIndex = 0;
            for (strNumber = 0; strNumber < matriz.Length; strNumber++)
            {
                strIndex = matriz[strNumber].IndexOf(valorComparar);
                if (strIndex >= 0)
                {
                    return true;
                }
            }
            return false;
        }
        public void verificacaoTipificaoComparacao(string unidadeCodigo, string data, string sequencial, string banda, Unidades unidades,
                                                   string empresaId, string departamentoId, string tarefaIdm, string conexao,
                                                   string caracteristica, ref bool conforme)
        {
            string first = "";
            string second = "";
            string queryVFResultado = "SELECT C.nCdCaracteristica FROM VTVerificacaoTipificacaoResultados R                                 " +
                            "INNER JOIN VTVerificacaoTipificacao V ON V.Chave = R.Chave                                                     " +
                            "LEFT JOIN CaracteristicaTipificacao C on C.cNrCaracteristica = R.CaracteristicaTipificacaoId                   " +
                            "WHERE                                                                                                          " +
                            "V.UnidadeId = "+ empresaId + " AND CAST(V.datahora AS DATE) = CAST('"+ data + "' AS DATE)                                        " +
                            "AND Sequencial = '"+ sequencial + "' AND Banda = '" + banda + "' AND cIdentificador = '" + caracteristica + "' order by C.nCdCaracteristica;  ";

            string queryVFValidacao = "SELECT V.nCdCaracteristicaTipificacao FROM VTVerificacaoTipificacaoValidacao V WHERE                 " +
                            "V.nCdEmpresa = "+ unidadeCodigo + " AND CAST(V.dMovimento AS DATE) = CAST('"+ data + "' AS DATE)                                   " +
                            "AND iSequencial = '" + sequencial + "' AND iBanda = '"+ banda + "' AND cIdentificadorTipificacao = '" + caracteristica + "'                " +
                            "ORDER BY V.nCdCaracteristicaTipificacao;";

            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SGQ_GlobalADO"].ConnectionString))
            {
                SqlCommand command = new SqlCommand(queryVFResultado, connection);

                try
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //Comparamos a caracteristica das duas tabelasd
                            first += reader[0].ToString() + ",";

                        }
                    }
                    connection.Close();
                }
                catch (Exception ex)
                {
                    connection.Close();
                    //throw ex;
                    new DTO.CreateLog(new Exception("Exception queryVFResultado"));
                }

                command = new SqlCommand(queryVFValidacao, connection);

                try
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //Comparamos a caracteristica das duas tabelasd
                            second += reader[0].ToString() + ",";

                        }
                    }
                    connection.Close();
                }
                catch (Exception ex)
                {
                    connection.Close();
                    //throw ex;
                    new DTO.CreateLog(new Exception("Exception queryVFValidacao"));
                }
            }

            if (first != second)
                conforme = false;
            
            //string queryString = "(SELECT 'VTR', U.CODIGO nCdEmpresa, VT.DATAHORA dMovimento, VT.SEQUENCIAL iSequencial, VT.BANDA iBanda, CT.nCdCaracteristica cNrCaracteristica, CT.cIdentificador, VTV.nCdCaracteristicaTipificacao " +
            //                    "FROM VTVERIFICACAOTIPIFICACAO VT " +
            //                    "INNER JOIN UNIDADES U ON U.ID = VT.UNIDADEID " +
            //                    "INNER JOIN VTVERIFICACAOTIPIFICACAOresultados VTR ON VTR.CHAVE = VT.CHAVE " +
            //                    //Trocamos nCdCaracteristica para cNrCaracteristica
            //                    "INNER JOIN CaracteristicaTipificacao CT ON VTR.CARACTERISTICATIPIFICACAOID=CT.cNrCaracteristica " +
            //                    //"INNER JOIN CaracteristicaTipificacao CT ON VTR.CARACTERISTICATIPIFICACAOID=CT.cNrCaracteristica " +
            //                    //"LEFT JOIN VTVerificacaoTipificacaoValidacao VTV ON VTV.nCdEmpresa=U.CODIGO AND CAST(VTV.dMovimento AS DATE) = CAST(VT.datahora AS DATE) AND VTV.iSequencial=VT.SEQUENCIAL AND VTV.IBANDA=VT.BANDA AND VTV.nCdCaracteristicaTipificacao=VTR.CARACTERISTICATIPIFICACAOID " +
            //                    "LEFT JOIN VTVerificacaoTipificacaoValidacao VTV ON VTV.nCdEmpresa=U.CODIGO AND CAST(VTV.dMovimento AS DATE) = CAST(VT.datahora AS DATE) AND VTV.iSequencial=VT.SEQUENCIAL AND VTV.IBANDA=VT.BANDA AND VTV.nCdCaracteristicaTipificacao=CT.nCdCaracteristica " +
            //                    "WHERE U.CODIGO='" + unidadeCodigo + "' AND CAST(VT.datahora AS DATE) = CAST('" + data + "' AS DATE) AND VT.sequencial='" + sequencial + "' AND VT.Banda='" + banda + "')" +
            //                    "UNION ALL " +
            //                    "(SELECT 'VTV', VTV.nCdEmpresa, VTV.dMovimento, VTV.iSequencial, VTV.iBanda, CT.nCdCaracteristica, VTV.cIdentificadorTipificacao, VTV.nCdCaracteristicaTipificacao " +
            //                    "FROM VTVerificacaoTipificacaoValidacao VTV " +
            //                    "INNER JOIN UNIDADES U ON U.CODIGO = VTV.nCdEmpresa " +
            //                    "INNER JOIN VTVERIFICACAOTIPIFICACAO VT ON VT.UnidadeId=U.ID AND VT.Sequencial=VTV.iSequencial AND VT.Banda=VTV.iBanda AND CAST(VT.DataHora AS DATE) = CAST(VTV.dMovimento AS DATE)  LEFT JOIN " +
            //                    "CaracteristicaTipificacao CT ON VTV.nCdCaracteristicaTipificacao = CT.nCdCaracteristica LEFT OUTER JOIN " +
            //                    "VTVerificacaoTipificacaoResultados AS VTR ON VTR.Chave = VT.Chave AND VTR.CaracteristicaTipificacaoId = CT.cNrCaracteristica " +
            //                    "WHERE VTV.nCdEmpresa='" + unidadeCodigo + "' AND CAST(VTV.dMovimento AS DATE) = CAST('" + data + "' AS DATE) AND VTV.iSequencial='" + sequencial + "' AND VTV.iBanda='" + banda + "') ";

            //utiliza transacao para excluir e incluir os itens
            //using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SGQ_GlobalADO"].ConnectionString))
            //{
            //    SqlCommand command = new SqlCommand(queryString, connection);

            //    try
            //    {
            //        connection.Open();

            //        using (SqlDataReader reader = command.ExecuteReader())
            //        {
            //            //Instanciamos uma classe do Objeto de Comparacao
            //            List<ResultadoComparacaoValidacao> comparacao = new List<ResultadoComparacaoValidacao>();

            //            while (reader.Read())
            //            {
            //                //Comparamos a caracteristica das duas tabelasd
            //                string idCaracteristicaTipificacaoVTResultados = reader[5].ToString();
            //                string labelCaracteristica = reader[6].ToString();
            //                string idCaracteristicaTipificacaoVTValidacao = reader[7].ToString();

            //                //Se as caracteristicas forem diferentes o resultado da verificação é não conforme
            //                if (idCaracteristicaTipificacaoVTResultados != idCaracteristicaTipificacaoVTValidacao && MatrizStrinComparacao(comparacaoRESULTADO, labelCaracteristica))
            //                {
            //                    conforme = false;
            //                    //Incluimos o resultado na tabela de comparação
            //                    comparacao.Add(
            //                              new ResultadoComparacaoValidacao()
            //                              {

            //                                  localTabela = reader[0].ToString(),
            //                                  nCadEmpresa = reader[1].ToString(),
            //                                  dataMovimento = Convert.ToDateTime(reader[2].ToString()),
            //                                  iSequencial = Convert.ToInt32(reader[3].ToString()),
            //                                  iBanda = Convert.ToInt32(reader[4].ToString()),
            //                                  idCaracteristicaVTR = reader[5].ToString(),
            //                                  identificadorLabel = reader[6].ToString(),
            //                                  idCaracteristicaVTV = reader[7].ToString()

            //                              });
            //                }

            //            }
            //            //Se existe resultados não conformes
            //            if (conforme == false)
            //            {
            //                //Separamos os resultados que estão na tabela VERIFICACAOTIPIFICACAOresultados
            //                var valoresVTR = (from p in comparacao
            //                                  where p.localTabela == "VTR"
            //                                  select p).ToList();

            //                //separamos os resultados que estão na tabela VerificacaoTipificacaoValidacao
            //                var valoresVTV = (from p in comparacao
            //                                  where p.localTabela == "VTV"
            //                                  select p).ToList();

            //                //Varremos a tabela de resultados para comparar os valores da Matriz comparacaoUnica com a tabela de validação
            //                foreach (var p in valoresVTR)
            //                {
            //                    //Se o resultado da for uma comparação Única
            //                    if (MatrizStrinComparacao(comparacaoUnica, p.identificadorLabel) == true)
            //                    {
            //                        //Verificamos se existe a caracteristica na tabela de validação
            //                        var valorComparacaoVTV = (from pV in valoresVTV
            //                                                  where pV.identificadorLabel == p.identificadorLabel
            //                                                  select pV).FirstOrDefault();

            //                        //Se a caracteristica existir
            //                        if (valorComparacaoVTV != null)
            //                        {
            //                            //Atualizamos o valor do valor da verificação validação na tabela resultados com o valor da verificação na tabela caracteristica
            //                            p.idCaracteristicaVTV = valorComparacaoVTV.idCaracteristicaVTV;
            //                            //reovemos o valor da tabela de validação
            //                            valoresVTV.Remove(valorComparacaoVTV);

            //                            //teste de desempenho para verificamos se podemos deixar as informações na tabela de comparação ao inves de dividir em 2 objetos
            //                            //comparacao.Remove(valorComparacaoVTV);
            //                        }
            //                    }
            //                    //Incluimos o Valor dos resultados na tabela VerificacaoTipificacaoComparacao
            //                    VerificacaoTipificacaoComparacaoAdicionar(unidadeCodigo, sequencial, banda, p.identificadorLabel, p.dataMovimento.ToString(), p.idCaracteristicaVTR, p.idCaracteristicaVTV);
            //                }

            //                //Verificamos se ainda existem valores da tabela VerificacaoTipificacaoValidacao que não foram comparados
            //                foreach (var p in valoresVTV)
            //                {
            //                    //Incluimos os Valores da Tipificação na tabela VerificacaoTipificacaoComparacao 
            //                    VerificacaoTipificacaoComparacaoAdicionar(unidadeCodigo, sequencial, banda, p.identificadorLabel, p.dataMovimento.ToString(), p.idCaracteristicaVTR, p.idCaracteristicaVTV);
            //                }
            //            }

            //            comparacaoResultado = conforme;
            //            connection.Close();
            //            //retornamos o valor para tabela de resultados
            //            return conforme;
            //        }

            //    }
            //    catch (Exception ex)
            //    {
            //        //string t = ex.ToString();
            //        //mensagemErro = ex.Message;
            //        //return false;
            //        //RETORNAR O ERRO
            //        //mensagemErro = Json(t, JsonRequestBehavior.AllowGet);
            //        connection.Close();
            //        throw ex;
            //    }
            //}


        }
        protected void VerificacaoTipificacaoComparacaoAdicionar(string uCodigo, string sequencial, string banda, string labelCaracteristica, string dataRegistro,
                                                        string idCaracteristicaTipificacaoVTR, string idCaracteristicaTipificacaoVTV)
        {

            using (var db = new SGQ_GlobalEntities())
            {
                try
                {
                    var VerificacaoTipificacaoComparacao = new VerificacaoTipificacaoComparacao();
                    VerificacaoTipificacaoComparacao.nCdEmpresa = Convert.ToInt32(uCodigo);
                    VerificacaoTipificacaoComparacao.Sequencial = Convert.ToInt32(sequencial);
                    VerificacaoTipificacaoComparacao.Banda = Convert.ToInt32(banda);
                    VerificacaoTipificacaoComparacao.Identificador = labelCaracteristica;
                    VerificacaoTipificacaoComparacao.NumCaracteristica = 0;

                    VerificacaoTipificacaoComparacao.DataHora = Convert.ToDateTime(dataRegistro);
                    if (!string.IsNullOrEmpty(idCaracteristicaTipificacaoVTR))
                    {
                        VerificacaoTipificacaoComparacao.valorSGQ = Convert.ToInt32(idCaracteristicaTipificacaoVTR);
                    }
                    if (!string.IsNullOrEmpty(idCaracteristicaTipificacaoVTV))
                    {
                        VerificacaoTipificacaoComparacao.valorJBS = Convert.ToInt32(idCaracteristicaTipificacaoVTV);
                    }
                    db.VerificacaoTipificacaoComparacao.Add(VerificacaoTipificacaoComparacao);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    string erro = ex.Message;

                }
            }
        }

    }
}