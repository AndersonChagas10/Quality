using ADOFactory;
using Dominio;
using SgqSystem.Controllers.Api;
using SgqSystem.Helpers;
using SgqSystem.Jobs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SgqSystem.Controllers.V2.Api
{
    [RoutePrefix("api/ConsolidationSeara")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ConsolidationSearaApiController : BaseApiController
    {

        #region Coleta Padrão RH
        [Route("SetCollect")]
        public IHttpActionResult SetCollect(List<Collection> listSimpleCollect)
        {
            #region Gambi Log de Coletas
            var guiid = Guid.NewGuid();
            LogSystem.LogErrorBusiness.TryRegister(new Exception("Iniciado o registro das coletas (" + listSimpleCollect.Count + ")"), new { GUIID = guiid.ToString() });
            #endregion

            List<Collection> listaSimpleCollectDuplicadas = new List<Collection>();

            foreach (var item in listSimpleCollect)
            {

                try
                {
                    item.AddDate = DateTime.Now;
                    item.Shift_Id = 1;
                    item.Period_Id = 1;
                    item.IsProcessed = false;

                    List<Collection> collectionDuplicada = new List<Collection>();

                    #region busca coletas duplicadas
                    string sqlSelecionaCollectionDuplicada = $@"
DECLARE @Shift_Id int = @@Shift_Id;
DECLARE @Period_Id int = @@Period_Id;
DECLARE @ParCargo_Id int = @@ParCargo_Id;
DECLARE @ParCompany_Id int = @@ParCompany_Id;
DECLARE @ParDepartment_Id int = @@ParDepartment_Id;
DECLARE @ParCluster_Id int = @@ParCluster_Id;
DECLARE @ParLevel1_Id int = @@ParLevel1_Id;
DECLARE @ParLevel2_Id int = @@ParLevel2_Id;
DECLARE @ParLevel3_Id int = @@ParLevel3_Id;
DECLARE @Evaluation int = @@Evaluation;
DECLARE @Sample int = @@Sample;
DECLARE @Parfrequency_Id int = @@Parfrequency_Id;
DECLARE @ParHeaderField_Id int = @@ParHeaderField_Id;
DECLARE @ParHeaderField_Value  varchar(255) = @@ParHeaderField_Value;
DECLARE @CollectionDate datetime = @@CollectionDate;
DECLARE @IsConform bit = @@IsConform;

WITH cte
AS
(SELECT top 20000
	CollectionDate
	,IsConform
	,Shift_Id
	,Period_Id
	,ParCargo_Id
	,ParCompany_Id
	,ParDepartment_Id
	,ParCluster_Id
	,ParLevel1_Id
	,ParLevel2_Id
	,ParLevel3_Id
	,Evaluation
	,Sample
	,Parfrequency_Id
	,ParHeaderField_Id
	,ParHeaderField_Value
FROM Collection WITH (NOLOCK)
order by id desc)
						
SELECT
	CollectionDate
	,IsConform
	,Shift_Id
	,Period_Id
	,ParCargo_Id
	,ParCompany_Id
	,ParDepartment_Id
	,ParCluster_Id
	,ParLevel1_Id
	,ParLevel2_Id
	,ParLevel3_Id
	,Evaluation
	,Sample
	,Parfrequency_Id
	,ParHeaderField_Id
	,ParHeaderField_Value
FROM cte c WITH (NOLOCK)
WHERE 1 = 1
AND c.IsConform = @IsConform
AND c.CollectionDate = @CollectionDate
AND c.Shift_Id = @Shift_Id
AND c.Period_Id = @Period_Id
AND c.ParCargo_Id = @ParCargo_Id
AND c.ParCompany_Id = @ParCompany_Id
AND c.ParDepartment_Id = @ParDepartment_Id
AND c.ParCluster_Id = @ParCluster_Id
AND c.ParLevel1_Id = @ParLevel1_Id
AND c.ParLevel2_Id = @ParLevel2_Id
AND c.ParLevel3_Id = @ParLevel3_Id
AND c.Evaluation = @Evaluation
AND c.Sample = @Sample
AND c.Parfrequency_Id = @Parfrequency_Id
AND ((@ParHeaderField_Id IS NOT NULL AND c.ParHeaderField_Id = @ParHeaderField_Id) OR (@ParHeaderField_Id IS NULL AND c.ParHeaderField_Id IS NULL))
AND ((@ParHeaderField_Value IS NOT NULL AND c.ParHeaderField_Id = @ParHeaderField_Value) OR (@ParHeaderField_Value IS NULL AND c.ParHeaderField_Id IS NULL))
                    ";

                    using (Factory factory = new Factory("DefaultConnection"))
                    {
                        using (SqlCommand cmd = new SqlCommand(sqlSelecionaCollectionDuplicada, factory.connection))
                        {
                            cmd.CommandType = CommandType.Text;
                            UtilSqlCommand.AddParameterNullable(cmd, "@@CollectionDate", item.CollectionDate);
                            UtilSqlCommand.AddParameterNullable(cmd, "@@Shift_Id", item.Shift_Id);
                            UtilSqlCommand.AddParameterNullable(cmd, "@@Period_Id", item.Period_Id);
                            UtilSqlCommand.AddParameterNullable(cmd, "@@ParCargo_Id", item.ParCargo_Id);
                            UtilSqlCommand.AddParameterNullable(cmd, "@@ParCompany_Id", item.ParCompany_Id);
                            UtilSqlCommand.AddParameterNullable(cmd, "@@ParDepartment_Id", item.ParDepartment_Id);
                            UtilSqlCommand.AddParameterNullable(cmd, "@@ParCluster_Id", item.ParCluster_Id);
                            UtilSqlCommand.AddParameterNullable(cmd, "@@ParLevel1_Id", item.ParLevel1_Id);
                            UtilSqlCommand.AddParameterNullable(cmd, "@@ParLevel2_Id", item.ParLevel2_Id);
                            UtilSqlCommand.AddParameterNullable(cmd, "@@ParLevel3_Id", item.ParLevel3_Id);
                            UtilSqlCommand.AddParameterNullable(cmd, "@@IsConform", item.IsConform);
                            UtilSqlCommand.AddParameterNullable(cmd, "@@Evaluation", item.Evaluation);
                            UtilSqlCommand.AddParameterNullable(cmd, "@@Sample", item.Sample);
                            UtilSqlCommand.AddParameterNullable(cmd, "@@Parfrequency_Id", item.Parfrequency_Id);
                            UtilSqlCommand.AddParameterNullable(cmd, "@@ParHeaderField_Id", item.ParHeaderField_Id);
                            UtilSqlCommand.AddParameterNullable(cmd, "@@ParHeaderField_Value", item.ParHeaderField_Value);

                            collectionDuplicada = factory.SearchQuery<Collection>(cmd).ToList();
                        }
                    }
                    #endregion

                    if (collectionDuplicada.Count == 0)
                    {
                        #region inserir collection
                        string sql = $@"
INSERT INTO [dbo].[Collection]
           ([CollectionDate]
           ,[AddDate]
           ,[UserSgq_Id]
           ,[Shift_Id]
           ,[Period_Id]
           ,[ParCargo_Id]
           ,[ParCompany_Id]
           ,[ParDepartment_Id]
           ,[ParCluster_Id]
           ,[ParLevel1_Id]
           ,[ParLevel2_Id]
           ,[ParLevel3_Id]
           ,[CollectionType]
           ,[Weigth]
           ,[IntervalMin]
           ,[IntervalMax]
           ,[Value]
           ,[ValueText]
           ,[IsNotEvaluate]
           ,[IsConform]
           ,[Defects]
           ,[PunishimentValue]
           ,[WeiEvaluation]
           ,[Evaluation]
           ,[WeiDefects]
           ,[HasPhoto]
           ,[Sample]
           ,[HaveCorrectiveAction]
           ,[Parfrequency_Id]
           ,[AlertLevel]
           ,[ParHeaderField_Id]
           ,[ParHeaderField_Value]
           ,[Outros]
           ,[IsProcessed])
     VALUES
           (@CollectionDate
           ,@AddDate
           ,@UserSgq_Id
           ,@Shift_Id
           ,@Period_Id
           ,@ParCargo_Id
           ,@ParCompany_Id
           ,@ParDepartment_Id
           ,@ParCluster_Id
           ,@ParLevel1_Id
           ,@ParLevel2_Id
           ,@ParLevel3_Id
           ,@CollectionType
           ,@Weigth
           ,@IntervalMin
           ,@IntervalMax
           ,@Value
           ,@ValueText
           ,@IsNotEvaluate
           ,@IsConform
           ,@Defects
           ,@PunishimentValue
           ,@WeiEvaluation
           ,@Evaluation
           ,@WeiDefects
           ,@HasPhoto
           ,@Sample
           ,@HaveCorrectiveAction
           ,@Parfrequency_Id
           ,@AlertLevel
           ,@ParHeaderField_Id
           ,@ParHeaderField_Value
           ,@Outros
           ,@IsProcessed);
            SELECT @@IDENTITY AS 'Identity';";

                        using (Factory factory = new Factory("DefaultConnection"))
                        {
                            using (SqlCommand cmd = new SqlCommand(sql, factory.connection))
                            {
                                cmd.CommandType = CommandType.Text;
                                UtilSqlCommand.AddParameterNullable(cmd, "@CollectionDate", item.CollectionDate);
                                UtilSqlCommand.AddParameterNullable(cmd, "@AddDate", item.AddDate);
                                UtilSqlCommand.AddParameterNullable(cmd, "@UserSgq_Id", item.UserSgq_Id);
                                UtilSqlCommand.AddParameterNullable(cmd, "@Shift_Id", item.Shift_Id);
                                UtilSqlCommand.AddParameterNullable(cmd, "@Period_Id", item.Period_Id);
                                UtilSqlCommand.AddParameterNullable(cmd, "@ParCargo_Id", item.ParCargo_Id);
                                UtilSqlCommand.AddParameterNullable(cmd, "@ParCompany_Id", item.ParCompany_Id);
                                UtilSqlCommand.AddParameterNullable(cmd, "@ParDepartment_Id", item.ParDepartment_Id);
                                UtilSqlCommand.AddParameterNullable(cmd, "@ParCluster_Id", item.ParCluster_Id);
                                UtilSqlCommand.AddParameterNullable(cmd, "@ParLevel1_Id", item.ParLevel1_Id);
                                UtilSqlCommand.AddParameterNullable(cmd, "@ParLevel2_Id", item.ParLevel2_Id);
                                UtilSqlCommand.AddParameterNullable(cmd, "@ParLevel3_Id", item.ParLevel3_Id);
                                UtilSqlCommand.AddParameterNullable(cmd, "@CollectionType", item.CollectionType);
                                UtilSqlCommand.AddParameterNullable(cmd, "@Weigth", item.Weigth);
                                UtilSqlCommand.AddParameterNullable(cmd, "@IntervalMin", item.IntervalMin);
                                UtilSqlCommand.AddParameterNullable(cmd, "@IntervalMax", item.IntervalMax);
                                UtilSqlCommand.AddParameterNullable(cmd, "@Value", item.Value);
                                UtilSqlCommand.AddParameterNullable(cmd, "@ValueText", item.ValueText);
                                UtilSqlCommand.AddParameterNullable(cmd, "@IsNotEvaluate", item.IsNotEvaluate);
                                UtilSqlCommand.AddParameterNullable(cmd, "@IsConform", item.IsConform);
                                UtilSqlCommand.AddParameterNullable(cmd, "@Defects", item.Defects);
                                UtilSqlCommand.AddParameterNullable(cmd, "@PunishimentValue", item.PunishimentValue);
                                UtilSqlCommand.AddParameterNullable(cmd, "@WeiEvaluation", item.WeiEvaluation);
                                UtilSqlCommand.AddParameterNullable(cmd, "@Evaluation", item.Evaluation);
                                UtilSqlCommand.AddParameterNullable(cmd, "@WeiDefects", item.WeiDefects);
                                UtilSqlCommand.AddParameterNullable(cmd, "@HasPhoto", item.HasPhoto);
                                UtilSqlCommand.AddParameterNullable(cmd, "@Sample", item.Sample);
                                UtilSqlCommand.AddParameterNullable(cmd, "@HaveCorrectiveAction", item.HaveCorrectiveAction);
                                UtilSqlCommand.AddParameterNullable(cmd, "@Parfrequency_Id", item.Parfrequency_Id);
                                UtilSqlCommand.AddParameterNullable(cmd, "@AlertLevel", item.AlertLevel);
                                UtilSqlCommand.AddParameterNullable(cmd, "@ParHeaderField_Id", item.ParHeaderField_Id);
                                UtilSqlCommand.AddParameterNullable(cmd, "@ParHeaderField_Value", item.ParHeaderField_Value);
                                UtilSqlCommand.AddParameterNullable(cmd, "@IsProcessed", item.IsProcessed);
                                UtilSqlCommand.AddParameterNullable(cmd, "@Outros", item.Outros);
                                var id = Convert.ToInt32(cmd.ExecuteScalar());

                                item.Id = id;
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        listaSimpleCollectDuplicadas.Add(item);
                        item.Id = collectionDuplicada[0].Id;
                    }
                }
                catch (Exception ex)
                {
                    item.HasError = true;
                    item.GUIID = guiid.ToString();

                    LogSystem.LogErrorBusiness.TryRegister(ex, new { GUIID = guiid.ToString() });
                }
            }

            var listaDeColetasComErro = listSimpleCollect.Where(x => x.HasError == true).ToList();
            var listaDeColetasSemErro = listSimpleCollect
                .Where(x => x.HasError != true && !listaSimpleCollectDuplicadas.Any(y => y.Id == x.Id))
                .ToList();
            var listaDeColetasDuplicadas = listSimpleCollect
                .Where(x => x.HasError != true && listaSimpleCollectDuplicadas.Any(y => y.Id == x.Id))
                .ToList();

            #region Gambi Log de Coletas
            LogSystem.LogErrorBusiness.TryRegister(new Exception("Finalizou a inserção das coletas (" + listaDeColetasDuplicadas.Count + "/" + listaDeColetasSemErro.Count + "/" + listSimpleCollect.Count + ")"),
                new { GUIID = guiid.ToString(), ListaCollection = string.Join(",", listaDeColetasSemErro.Select(x => x.Id)) });
            #endregion

            if (listaDeColetasComErro.Count == listSimpleCollect.Count)
                return BadRequest("Ocorreu erro em todas as tentativas de registrar as coletas.");

            #region Consolidação Sincrona
            int intervalTimeCollectionJob = 0;
            try
            {
                Int32.TryParse(DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.CollectionJobTime0IsDisabled, out intervalTimeCollectionJob);
            }
            catch (Exception)
            {
            }

            if (intervalTimeCollectionJob <= 0)
            {
                var coletasRegistradasPorCollectionLevel2 = listaDeColetasSemErro
                .Where(x => x.ParHeaderField_Id == null
                && x.ParHeaderField_Value == null
                && x.Evaluation != null
                && x.Sample != null)
                .Select(x => new CollectionLevel2()
                {
                    EvaluationNumber = (int)x.Evaluation,
                    Sample = x.Sample.Value,
                    ParLevel1_Id = x.ParLevel1_Id.Value,
                    ParLevel2_Id = x.ParLevel2_Id.Value,
                    Shift = x.Shift_Id.Value,
                    Period = x.Period_Id.Value,
                    UnitId = x.ParCompany_Id.Value,
                    ParCargo_Id = x.ParCargo_Id,
                    ParCluster_Id = x.ParCluster_Id,
                    ParDepartment_Id = x.ParDepartment_Id,
                    ParFrequency_Id = x.Parfrequency_Id,
                    AuditorId = x.UserSgq_Id ?? 0,
                    CollectionDate = x.CollectionDate.Value
                })
                .Distinct()
                .ToList();

                CollectionJob.ConsolidarCollectionLevel2(coletasRegistradasPorCollectionLevel2);
            }
            #endregion

            #region Gambi Log de Coletas
            try
            {
                LogSystem.LogErrorBusiness.Register(new Exception("Finalizado sem nenhum problema"), new { GUIID = guiid.ToString() });
            }
            catch { }
            #endregion

            return Ok(listaDeColetasSemErro);
        }

        #endregion
    }
}
