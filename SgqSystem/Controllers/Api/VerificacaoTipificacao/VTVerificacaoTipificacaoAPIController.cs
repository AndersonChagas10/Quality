using Dapper;
using Dominio;
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

        private bool running = false;
        [Route("Save")]
        [HttpPost]
        public void SaveVTVerificacaoTipificacao(TipificacaoViewModel model)
        {

            if (running)
                return;

            running = true;

            var _verificacao = model.VerificacaoTipificacao;

            using (var db = new SGQ_GlobalEntities())
            {
                var verificacaoTipificacao = db.VTVerificacaoTipificacao.FirstOrDefault(r => r.Chave == _verificacao.Chave);

                //Variavel para atribuir data correta na verificãção da tipificação
                DateTime dataHoraTipificacao = _verificacao.DataHora;

                //Caso exista uma verificação da tipidicação a mesma será removida, mantando somente a data para atribuir na nova verificação que será criada
                if (verificacaoTipificacao != null)
                {
                    //Atribui a data da verificação que for encontrada
                    dataHoraTipificacao = verificacaoTipificacao.DataHora;

                    string queryValidacaoVDelete = "DELETE FROM VTVerificacaoTipificacaoResultados WHERE chave='" + verificacaoTipificacao.Chave + "'";
                    int noOfRowDeleted = db.Database.ExecuteSqlCommand(queryValidacaoVDelete);


                    string queryValidacaoVDelete1 = "DELETE FROM VTVerificacaoTipificacao WHERE id='" + verificacaoTipificacao.Id + "'";
                    int noOfRowDeleted1 = db.Database.ExecuteSqlCommand(queryValidacaoVDelete1);

                    ////Busca todos os resultados referente a verificação localizada
                    //var resultados = db.VTVerificacaoTipificacaoResultados.Where(p => p.Chave == verificacaoTipificacao.Chave).ToList();
                    ////Deleta os resultados da verificaçao
                    //foreach (var r in resultados)
                    //{
                    //    db.VTVerificacaoTipificacaoResultados.Remove(r);
                    //}
                    //db.SaveChanges();

                    //Deleta a Verificação
                    //db.VTVerificacaoTipificacao.Remove(verificacaoTipificacao);
                    //db.SaveChanges();

                }


                //instanciamos um novo objeto na verificacao da tipificacao
                verificacaoTipificacao = new VTVerificacaoTipificacao();

                verificacaoTipificacao.Sequencial = _verificacao.Sequencial;
                verificacaoTipificacao.Banda = _verificacao.Banda;
                verificacaoTipificacao.DataHora = dataHoraTipificacao;
                verificacaoTipificacao.UnidadeId = _verificacao.UnidadeId;
                verificacaoTipificacao.Chave = _verificacao.Chave;
                verificacaoTipificacao.Status = false;
                verificacaoTipificacao.EvaluationNumber = _verificacao.EvaluationNumber;
                verificacaoTipificacao.Sample = _verificacao.Sample;

                //gravamos o objeto no banco
                db.VTVerificacaoTipificacao.Add(verificacaoTipificacao);

                for (var i = 0; i < model.VerificacaoTipificacaoResultados.Count; i++)
                {
                    if (model.VerificacaoTipificacaoResultados[i].CaracteristicaTipificacaoId == "null")
                    {
                        //PEGA ID DA AREA PARTICIPANTES
                        // var aa = model.VerificacaoTipificacaoResultados[i].AreasParticipantesId.GetValueOrDefault().ToString();

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
                        //CARACTERISTICA TIPIFICACAO

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

                //
                db.SaveChanges();

                //Consolidar resultados e tratamento de erro
                //_verificacao.Chave = "1245120170215";
                GetDadosGet(_verificacao.Chave);

                running = false;



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

            //problemas que podem ocorrer

            //se a tabela resultados já tiver resultados, vai duplicar na hora de trazer a comparacao
            //o idel é migrar os valores antigos da tabela de verficacaotipificacaocomparacao NumCaracteristica
            //verificar se o registro foi sincronizado e nao deu time out




            using (var db = new SGQ_GlobalEntities())
            {
                //teste do token é a validação para entrar na verificação de tipificação

                //se teste for sim, vai testar no ambiente de teste

               





                //Verifica se existe na VerificacaoTipificacao uma verificação com a chave informada no parametro do WebService.

                //confirmar se a funcionalidade do statuus da Verificacao é para informar se tem ou nao verificacao.
                var verificacaoTipificacao = (from p in db.VTVerificacaoTipificacao
                                              where p.Chave == verificacaoTipificacaoChave
                                              select p).FirstOrDefault();


                

                if (verificacaoTipificacao == null)
                {


                    //mensagem de erro que ñão existe
                    return null;
                }

                // var VerificacaoTipificacao = db.VerificacaoTipificacao.Where(v => v.Chave == verificacaoTipificacaoChave).FirstOrDefault();
                verificacaoTipificacao.Status = false;

                //Instanciamos a variável existeComparacao para verificar se existe comparação no no sistema.
                bool existeComparacao = false;

                //Instanciamos a variável excluirVerificacaoAntiga para verificar se existe alguma verificação na tabela VerificacaoTipificacaoValidacao.
                bool excluirVerificacaoAntiga = false;

                string conexao = null;
                var company = new ParCompany();

                connectionString(verificacaoTipificacao.UnidadeId, ref conexao, ref company);


           
                // Query String para verificação das Caracteristicas da tipificação
                string queryString = "exec FBED_GRTTipificacaoCaracteristica " + company.CompanyNumber + ", '" + verificacaoTipificacao.DataHora.ToString("yyyyMMdd") + "', " + verificacaoTipificacao.Sequencial;

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



                            //A comparação pode vir a partir do banco de dados (Identificadores).
                            //São os dados que serão comparadors para incluir na tabela de tipificação.

                            //string[] comparacao = { "<IDADE>", "<SEXO>", "<GORDURA>", "<CONTUSAO>", "<FALHAOP>" };

                            string[] comparacao = { "<IDADE>", "<SEXO>", "<GORDURA>", "<CONTUSAO>", "<FALHAOP>" };

                            //Intanciamos iSequencial e iBanda fora do do while pois utilizamos a informação em outros métodos.



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

                                    //Entramos apenas uma vez no bloco de código abaixo para verificação de exclusão.
                                    if (excluirVerificacaoAntiga == false)
                                    {
                                        //Exclui qualquer Verificação Tipificação registrado para a empresa informada na data informado e com o sequencial informado.
                                        //por padrão o sequencial deve ser um campo unico para a empresa na data informada.
                                        //Chamamos o método ExcluiVerificacaoTipificacaoValidacao dentro do while, pois o valor iSequencial é recuperado somente no DataReader
                                        ///podemos informar o iSequencial no parametro e não seria necessário entrar na estrutuda de decisão para excluir a verificação
                                        ///a mesma poderia ser excluida no inicio do WebService
                                        ///



                                        //var VerificacaoTipificacaoValidacaoExiste = (from p in db.VerificacaoTipificacaoValidacao
                                        //                                             where p.nCdEmpresa == nCdEmpresa && p.dMovimento == dMovimento.Date && p.iSequencial == iSequencial
                                        //                                             select p);

                                        //if (VerificacaoTipificacaoValidacaoExiste != null)
                                        //{
                                        //    db.VerificacaoTipificacaoValidacao.Remove(VerificacaoTipificacaoValidacaoExiste);
                                        //    db.SaveChanges();
                                        //}
                                        //podemos excluir resultados e tbm
                                        //deleta validacao
                                        string queryValidacaoVDelete = "DELETE FROM VTVerificacaoTipificacaoValidacao WHERE nCdEmpresa='" + nCdEmpresa + "' AND CAST(dMovimento AS DATE) ='" + dMovimento.ToString("yyyy-MM-dd 00:00:00") + "' AND iSequencial='" + iSequencial + "' AND iBanda='" + iBanda + "'";
                                        int noOfRowDeleted = db.Database.ExecuteSqlCommand(queryValidacaoVDelete);


                                        //deleta validacao de comparacao
                                        string queryValidacaoCDelete = "DELETE FROM VTVerificacaoTipificacaoComparacao WHERE nCdEmpresa='" + nCdEmpresa + "' AND CAST(DataHora AS DATE) = '" + dMovimento.ToString("yyyy-MM-dd 00:00:00") + "' AND Sequencial='" + iSequencial + "' AND Banda='" + iBanda + "'";
                                        noOfRowDeleted = db.Database.ExecuteSqlCommand(queryValidacaoCDelete);
                                        //if (ExcluiVerificacaoTipificacaoValidacao(nCdEmpresa, dMovimento, iSequencial) == false)
                                        //{
                                        //    return Json("{mensagem:'Existe uma verificação para este sequencial!'}", JsonRequestBehavior.AllowGet);
                                        //}
                                        excluirVerificacaoAntiga = true;
                                        //Após a primeira verificação definimos o valor da variável excluirVerificacaoAntiga para true, pois sua utilização é necessária apenas uma vez.

                                    }

                                    //Se existir o valor da comparacao na matriz de comparação então iremos adicionar um registro na tabela VerificacaoTipificacaoValidacao
                                    if (MatrizStrinComparacao(comparacao, cIdentificadorTipificacao) == true)
                                    {
                                        //se existir comparação incluiremos na tabela VerificacaoTipificacaoValidacao os valores que serão comparados a partir da tabela do cliente
                                        existeComparacao = true;
                                        //Informamos na tabela verificação que o status da verificação é verdadeiro
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
                                //db.SaveChanges();
                                //tratamento de erro, como fica?
                                //Verifica se a comparação entre a tabela Verificação Comparação Resultados está conforme a tabela Verificação Tipificação Validacao
                                //
                                try
                                {

                                    string[] ArrayComparacao = { "<GORDURA>", "<CONTUSAO>" };

                                    using (var db2 = new SgqDbDevEntities())
                                    {

                                        var ParLevel1 = (from p in db2.ParLevel1
                                                         where p.hashKey == 5
                                                         select p).FirstOrDefault();


                                        var ParLevel2_old = db2.ParLevel1
                                            .Join(db2.ParLevel3Level2Level1, p1 => p1.Id, p321 => p321.ParLevel1_Id, (p1, p321) => new { p1, p321 })
                                            .Join(db2.ParLevel3Level2, p321xp1 => p321xp1.p321.ParLevel3Level2_Id, p32 => p32.Id, (p321, p32) => new { p321, p32 })
                                            .Join(db2.ParLevel2, p32xp1 => p32xp1.p321.p321.ParLevel3Level2_Id, p2 => p2.Id, (p32, p2) => new { p32, p2})
                                            .Select(x => new { x.p2 }).FirstOrDefault();

                                        var ParLevel2 = ParLevel2_old.p2;

       

                                        //var ParLevel2 = (from p1 in db2.ParLevel1
                                        //                 join p321 in db2.ParLevel3Level2Level1 on p1.Id  equals p321.ParLevel1_Id
                                        //                 join p32  in db2.ParLevel3Level2       on p32.Id equals p321.ParLevel3Level2_Id
                                        //                 join p2   in db2.ParLevel2             on p2.Id  equals p32.ParLevel2_Id
                                        //                 where p1.Id == ParLevel1.Id
                                        //                 select p2).FirstOrDefault();

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

                                        var SgqSystem = new SgqSystem.Services.SyncServices();

                                        SqlConnection dbService = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString);
                                        //dbService.Open();

                                        
                                        

                                        ///****trocar***//
                                      
                                        
                                        DateTime dataC = verificacaoTipificacao.DataHora;


                                        //var consolidationLevel1 = ConsolidationLevel1DB.getConsolidation(verificacaoTipificacao.UnidadeId, ParLevel1.Id, dataC);

                                        string sql = "SELECT * FROM ConsolidationLevel1 WHERE UnitId = '" + verificacaoTipificacao.UnidadeId + "' AND ParLevel1_Id= '" + ParLevel1.Id + "' AND CONVERT(date, ConsolidationDate) = '" + dataC.ToString("yyyy-MM-dd") + "'";

                                        var consolidationLevel1 = dbService.Query<SGQDBContext.ConsolidationLevel1>(sql).FirstOrDefault();

                                        var ConsolidationLevel1DB = new SGQDBContext.ConsolidationLevel1(dbService);
                                        var ConsolidationLevel2DB = new SGQDBContext.ConsolidationLevel2(dbService);

                                        if (consolidationLevel1 == null)
                                        {
                                            consolidationLevel1 = SgqSystem.InsertConsolidationLevel1(verificacaoTipificacao.UnidadeId, ParLevel1.Id, dataC);
                                            if (consolidationLevel1 == null)
                                            {
                                                throw new Exception();
                                            }
                                        }

                                        //******Trocar

                                        var consolidationLevel2 = ConsolidationLevel2DB.getByConsolidationLevel1(verificacaoTipificacao.UnidadeId, consolidationLevel1.Id, ParLevel2.Id);
                                        if (consolidationLevel2 == null)
                                        {
                                            consolidationLevel2 = SgqSystem.InsertConsolidationLevel2(consolidationLevel1.Id, ParLevel2.Id, verificacaoTipificacao.UnidadeId, dataC,false,0);
                                            if (consolidationLevel2 == null)
                                            {
                                                throw new Exception();
                                            }
                                        }

                                        collectionLevel2 = new CollectionLevel2();

                                        collectionLevel2.Key = verificacaoTipificacaoChave;


                                        ///////************TROCAR**************/////////////
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

                                            string[] varComparacao = new string[1];

                                            varComparacao[0] = ArrayComparacao[i];

                                            string comparacaoToString = ArrayComparacao[i];

                                            var resultIdTarefa = (from x in db.VerificacaoTipificacaoTarefaIntegracao
                                                                  join y in db.CaracteristicaTipificacao
                                                                  on x.CaracteristicaTipificacaoId equals y.nCdCaracteristica
                                                                  where y.cIdentificador.Equals(comparacaoToString)
                                                                  select x).FirstOrDefault().TarefaId;

                                            var ParLevel3 = (from p in ParLevel3List
                                                             where p.Id == resultIdTarefa
                                                             select p).FirstOrDefault();

                                            bool conforme = true;

                                            verificacaoTipificaoComparacao(company.CompanyNumber.ToString(), verificacaoTipificacao.DataHora.ToString("yyyyMMdd"), 
                                                                           verificacaoTipificacao.Sequencial.ToString(), iBanda.ToString(), null,
                                                                           verificacaoTipificacao.UnidadeId.ToString(), "1", "1", conexao,
                                                                           varComparacao, ref conforme);
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
                                            result.WeiEvaluation = 0;
                                            result.Evaluation = 1;
                                            result.WeiDefects = 0;

                                            db2.Result_Level3.Add(result);
                                        
                                        }
                                        collectionLevel2.Defects = defectsL2;
                                        verificacaoTipificacao.Status = true;
                                        db2.SaveChanges();
                                        db.SaveChanges();




                                    }




                                    return null;
                                    //mensagem de confirmacao

                                }
                                catch (Exception ex)
                                {
                                    throw new Exception("Deu merda no número 1 ", ex);
                                    //mernsagem de erro
                                    //string t = ex.ToString();
                                    //var inner = ex.InnerException.IsNotNull() ? ex.InnerException.Message : "Não consta.";
                                    //return Json(mensagem("Não foi possível registrar os dados de comparação. Tente novamente! EXCEPTION" + t + ", INNER: " + inner + ". CONNECTION: " + conexao + ".", alertaTipo.warning, reenviarRequisicao: true));

                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Deu merda no número 2 ", ex);
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
        public bool verificacaoTipificaoComparacao(string unidadeCodigo, string data, string sequencial, string banda, Unidades unidades,
                                                   string empresaId, string departamentoId, string tarefaIdm, string conexao,
                                                   string[] varComparacao, ref bool comparacaoResultado)
        {

            //melhorar tabela de comparacao
            //criar valorSGQ, valorJBS, nCdEmpresa

            //Instanciamos uma variável conforme partindo do principicio que a verificação está conforme
            bool conforme = true;

            //Criamos uma variável de comparação unica para comparar os parametros unicos da tabela
            string[] comparacaoUnica = { "<IDADE>", "<SEXO>", "<GORDURA>" };

            //string[] comparacaoRESULTADO = { "<IDADE>", "<SEXO>", "<GORDURA>", "<CONTUSAO>" };

            string[] comparacaoRESULTADO = varComparacao;


            string queryString = "(SELECT 'VTR', U.CODIGO nCdEmpresa, VT.DATAHORA dMovimento, VT.SEQUENCIAL iSequencial, VT.BANDA iBanda, CT.nCdCaracteristica cNrCaracteristica, CT.cIdentificador, VTV.nCdCaracteristicaTipificacao " +
                                "FROM VTVERIFICACAOTIPIFICACAO VT " +
                                "INNER JOIN UNIDADES U ON U.ID = VT.UNIDADEID " +
                                "INNER JOIN VTVERIFICACAOTIPIFICACAOresultados VTR ON VTR.CHAVE = VT.CHAVE " +
                                //Trocamos nCdCaracteristica para cNrCaracteristica
                                "INNER JOIN CaracteristicaTipificacao CT ON VTR.CARACTERISTICATIPIFICACAOID=CT.cNrCaracteristica " +
                                //"INNER JOIN CaracteristicaTipificacao CT ON VTR.CARACTERISTICATIPIFICACAOID=CT.cNrCaracteristica " +
                                //"LEFT JOIN VTVerificacaoTipificacaoValidacao VTV ON VTV.nCdEmpresa=U.CODIGO AND CAST(VTV.dMovimento AS DATE) = CAST(VT.datahora AS DATE) AND VTV.iSequencial=VT.SEQUENCIAL AND VTV.IBANDA=VT.BANDA AND VTV.nCdCaracteristicaTipificacao=VTR.CARACTERISTICATIPIFICACAOID " +
                                "LEFT JOIN VTVerificacaoTipificacaoValidacao VTV ON VTV.nCdEmpresa=U.CODIGO AND CAST(VTV.dMovimento AS DATE) = CAST(VT.datahora AS DATE) AND VTV.iSequencial=VT.SEQUENCIAL AND VTV.IBANDA=VT.BANDA AND VTV.nCdCaracteristicaTipificacao=CT.nCdCaracteristica " +
                                "WHERE U.CODIGO='" + unidadeCodigo + "' AND CAST(VT.datahora AS DATE) = CAST('" + data + "' AS DATE) AND VT.sequencial='" + sequencial + "' AND VT.Banda='" + banda + "')" +
                                "UNION ALL " +
                                "(SELECT 'VTV', VTV.nCdEmpresa, VTV.dMovimento, VTV.iSequencial, VTV.iBanda, CT.nCdCaracteristica, VTV.cIdentificadorTipificacao, VTV.nCdCaracteristicaTipificacao " +
                                "FROM VTVerificacaoTipificacaoValidacao VTV " +
                                "INNER JOIN UNIDADES U ON U.CODIGO = VTV.nCdEmpresa " +
                                "INNER JOIN VTVERIFICACAOTIPIFICACAO VT ON VT.UnidadeId=U.ID AND VT.Sequencial=VTV.iSequencial AND VT.Banda=VTV.iBanda AND CAST(VT.DataHora AS DATE) = CAST(VTV.dMovimento AS DATE)  LEFT JOIN " +
                                "CaracteristicaTipificacao CT ON VTV.nCdCaracteristicaTipificacao = CT.nCdCaracteristica LEFT OUTER JOIN " +
                                "VTVerificacaoTipificacaoResultados AS VTR ON VTR.Chave = VT.Chave AND VTR.CaracteristicaTipificacaoId = CT.cNrCaracteristica " +
                                "WHERE VTV.nCdEmpresa='" + unidadeCodigo + "' AND CAST(VTV.dMovimento AS DATE) = CAST('" + data + "' AS DATE) AND VTV.iSequencial='" + sequencial + "' AND VTV.iBanda='" + banda + "') ";

            //utiliza transacao para excluir e incluir os itens
            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SGQ_GlobalADO"].ConnectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);

                try
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        //Instanciamos uma classe do Objeto de Comparacao
                        List<ResultadoComparacaoValidacao> comparacao = new List<ResultadoComparacaoValidacao>();

                        while (reader.Read())
                        {
                            //Comparamos a caracteristica das duas tabelasd
                            string idCaracteristicaTipificacaoVTResultados = reader[5].ToString();
                            string labelCaracteristica = reader[6].ToString();
                            string idCaracteristicaTipificacaoVTValidacao = reader[7].ToString();

                            //Se as caracteristicas forem diferentes o resultado da verificação é não conforme
                            if (idCaracteristicaTipificacaoVTResultados != idCaracteristicaTipificacaoVTValidacao && MatrizStrinComparacao(comparacaoRESULTADO, labelCaracteristica))
                            {
                                conforme = false;
                                //Incluimos o resultado na tabela de comparação
                                comparacao.Add(
                                          new ResultadoComparacaoValidacao()
                                          {

                                              localTabela = reader[0].ToString(),
                                              nCadEmpresa = reader[1].ToString(),
                                              dataMovimento = Convert.ToDateTime(reader[2].ToString()),
                                              iSequencial = Convert.ToInt32(reader[3].ToString()),
                                              iBanda = Convert.ToInt32(reader[4].ToString()),
                                              idCaracteristicaVTR = reader[5].ToString(),
                                              identificadorLabel = reader[6].ToString(),
                                              idCaracteristicaVTV = reader[7].ToString()

                                          });
                            }

                        }
                        //Se existe resultados não conformes
                        if (conforme == false)
                        {
                            //Separamos os resultados que estão na tabela VERIFICACAOTIPIFICACAOresultados
                            var valoresVTR = (from p in comparacao
                                              where p.localTabela == "VTR"
                                              select p).ToList();

                            //separamos os resultados que estão na tabela VerificacaoTipificacaoValidacao
                            var valoresVTV = (from p in comparacao
                                              where p.localTabela == "VTV"
                                              select p).ToList();

                            //Varremos a tabela de resultados para comparar os valores da Matriz comparacaoUnica com a tabela de validação
                            foreach (var p in valoresVTR)
                            {
                                //Se o resultado da for uma comparação Única
                                if (MatrizStrinComparacao(comparacaoUnica, p.identificadorLabel) == true)
                                {
                                    //Verificamos se existe a caracteristica na tabela de validação
                                    var valorComparacaoVTV = (from pV in valoresVTV
                                                              where pV.identificadorLabel == p.identificadorLabel
                                                              select pV).FirstOrDefault();

                                    //Se a caracteristica existir
                                    if (valorComparacaoVTV != null)
                                    {
                                        //Atualizamos o valor do valor da verificação validação na tabela resultados com o valor da verificação na tabela caracteristica
                                        p.idCaracteristicaVTV = valorComparacaoVTV.idCaracteristicaVTV;
                                        //reovemos o valor da tabela de validação
                                        valoresVTV.Remove(valorComparacaoVTV);

                                        //teste de desempenho para verificamos se podemos deixar as informações na tabela de comparação ao inves de dividir em 2 objetos
                                        //comparacao.Remove(valorComparacaoVTV);
                                    }
                                }
                                //Incluimos o Valor dos resultados na tabela VerificacaoTipificacaoComparacao
                                VerificacaoTipificacaoComparacaoAdicionar(unidadeCodigo, sequencial, banda, p.identificadorLabel, p.dataMovimento.ToString(), p.idCaracteristicaVTR, p.idCaracteristicaVTV);
                            }

                            //Verificamos se ainda existem valores da tabela VerificacaoTipificacaoValidacao que não foram comparados
                            foreach (var p in valoresVTV)
                            {
                                //Incluimos os Valores da Tipificação na tabela VerificacaoTipificacaoComparacao 
                                VerificacaoTipificacaoComparacaoAdicionar(unidadeCodigo, sequencial, banda, p.identificadorLabel, p.dataMovimento.ToString(), p.idCaracteristicaVTR, p.idCaracteristicaVTV);
                            }
                        }

                        comparacaoResultado = conforme;
                        //retornamos o valor para tabela de resultados
                        return conforme;
                    }
                }
                catch (Exception ex)
                {
                    //string t = ex.ToString();
                    //mensagemErro = ex.Message;
                    //return false;
                    //RETORNAR O ERRO
                    //mensagemErro = Json(t, JsonRequestBehavior.AllowGet);
                    throw ex;
                }
            }


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