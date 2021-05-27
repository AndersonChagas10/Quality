using ADOFactory;
using Dominio;
using Helper;
using SgqSystem.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace SgqServiceBusiness.Controllers.RH
{
    public class AppColetaBusiness
    {
        public List<Collection> GetCollectionDuplicada(Collection item)
        {
            List<Collection> collectionDuplicada = new List<Collection>();

            try
            {

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
                AND ((@ParHeaderField_Value IS NOT NULL AND c.ParHeaderField_Value = @ParHeaderField_Value) OR (@ParHeaderField_Value IS NULL AND c.ParHeaderField_Value IS NULL))
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
                return collectionDuplicada;
            }
            catch (Exception ex)
            {
                LogSystem.LogErrorBusiness.Register(ex, collectionDuplicada);
                return null;
            }
        }

        public List<Collection> SaveCollectionPartial(List<Collection> listSimpleCollect, Guid guiid)
        {
            //Não salvar header field
            listSimpleCollect = listSimpleCollect.Where(x => x.IsPartialSave).ToList();

            for (int i = 0; i < listSimpleCollect.Count; i++)
            {
                listSimpleCollect[i] = SaveCollectionPartial(listSimpleCollect[i], guiid);
            }

            return listSimpleCollect;

        }

        public Collection SaveCollectionPartial(Collection item, Guid guiid)
        {
            item.AddDate = DateTime.Now;
            item.Shift_Id = 1;
            item.Period_Id = 1;
            item.IsProcessed = false;

            try
            {
                string sql = $@"
                INSERT INTO [CollectionPartial]
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
            }
            catch (Exception ex)
            {
                item.HasError = true;
                item.GUIID = guiid.ToString();
                LogSystem.LogErrorBusiness.Register(ex, item);
            }

            return item;
        }

        public void DeleteCollectionsPartialDuplicadas(List<Collection> listSimpleCollect, Guid guiid)
        {
            //Somente coleta sem header field
            listSimpleCollect = listSimpleCollect.Where(x => x.ParHeaderField_Id == null).ToList();

            foreach (var coleta in listSimpleCollect)
            {
                DeleteCollectionPartialDuplicada(coleta, guiid);
            }
        }

        private void DeleteCollectionPartialDuplicada(Collection coleta, Guid guiid)
        {
            //Verificar quais coletas ja foram coletadas retornar e pagar as que já existem
            try
            {

                var sql = $@"
DECLARE @ParFrequency_Id INT = @@ParFrequency_Id;
DECLARE @ParCompany_Id INT = @@ParCompany_Id;
DECLARE @DataColeta DATETIME = @@CollectionDate;
DECLARE @DateTimeInicio DATETIME;
DECLARE @DateTimeFinal DATETIME;

SET @DateTimeInicio =
CASE @ParFrequency_Id
	WHEN 1 THEN CAST(CONCAT(CONVERT(VARCHAR(10), @DataColeta, 120), ' 00:00:00') AS DATETIME)  -- Período
	WHEN 2 THEN CAST(CONCAT(CONVERT(VARCHAR(10), @DataColeta, 120), ' 00:00:00') AS DATETIME)  -- Turno
	WHEN 3 THEN CAST(CONCAT(CONVERT(VARCHAR(10), @DataColeta, 120), ' 00:00:00') AS DATETIME)  -- Diario
	WHEN 4 THEN CAST(DATEADD(DAY, -DATEPART(WEEKDAY, @DataColeta) + 1, @DataColeta) AS DATE)  -- Semanal
	WHEN 5 THEN IIF(DATEPART(DAY, @DataColeta) <= 15, CONCAT(CONVERT(VARCHAR(7), @DataColeta, 120), '-01'), CONCAT(CONVERT(VARCHAR(7), @DataColeta, 120), '-16'))  -- Quinzenal
	WHEN 6 THEN CONCAT(CONVERT(VARCHAR(7), @DataColeta, 120), '-01')  -- Mensal
	WHEN 10 THEN CAST(CONCAT(CONVERT(VARCHAR(10), @DataColeta, 120), ' 00:00:00') AS DATETIME) -- Diario com Intervalo 
END

SET @DateTimeFinal =
CASE @ParFrequency_Id
	WHEN 1 THEN CAST(CONCAT(CONVERT(VARCHAR(10), @DataColeta, 120), ' 23:59:59') AS DATETIME) -- Período
	WHEN 2 THEN CAST(CONCAT(CONVERT(VARCHAR(10), @DataColeta, 120), ' 23:59:59') AS DATETIME) -- Turno
	WHEN 3 THEN CAST(CONCAT(CONVERT(VARCHAR(10), @DataColeta, 120), ' 23:59:59') AS DATETIME) -- Diario
	WHEN 4 THEN CAST(CONCAT(CAST(DATEADD(DAY, 7 - DATEPART(WEEKDAY, @DataColeta), @DataColeta) AS DATE), ' 23:59:59') AS DATETIME) -- Semanal
	WHEN 5 THEN IIF(DATEPART(DAY, @DataColeta) <= 15, CONCAT(CONVERT(VARCHAR(7), @DataColeta, 120), '-15 23:59:59'), CONCAT(EOMONTH(@DataColeta), ' 23:59:59'))  -- Quinzenal
	WHEN 6 THEN EOMONTH(@DataColeta)  -- Mensal
	WHEN 10 THEN CAST(CONCAT(CONVERT(VARCHAR(10), @DataColeta, 120), ' 23:59:59') AS DATETIME) -- Diario com Intervalo 
END

DELETE CollectionPartial
WHERE 1 = 1
	AND ParCargo_Id = @@ParCargo_Id
	AND ParCompany_Id = @ParCompany_Id
	AND ParDepartment_Id = @@ParDepartment_Id
	AND ParCluster_Id = @@ParCluster_Id
	AND ((ParLevel1_Id = @@ParLevel1_Id AND ParLevel2_Id = @@ParLevel2_Id AND ParLevel3_Id = @@ParLevel3_Id) OR (ParHeaderField_Id IS NOT NULL))
	AND Parfrequency_Id = @ParFrequency_Id
	AND Evaluation = @@Evaluation
	AND Sample = @@Sample
	AND CollectionDate BETWEEN @DateTimeInicio AND @DateTimeFinal";

                using (Factory factory = new Factory("DefaultConnection"))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, factory.connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        UtilSqlCommand.AddParameterNullable(cmd, "@@ParCargo_Id", coleta.ParCargo_Id);
                        UtilSqlCommand.AddParameterNullable(cmd, "@@ParCompany_Id", coleta.ParCompany_Id);
                        UtilSqlCommand.AddParameterNullable(cmd, "@@ParDepartment_Id", coleta.ParDepartment_Id);
                        UtilSqlCommand.AddParameterNullable(cmd, "@@ParCluster_Id", coleta.ParCluster_Id);
                        UtilSqlCommand.AddParameterNullable(cmd, "@@ParLevel1_Id", coleta.ParLevel1_Id);
                        UtilSqlCommand.AddParameterNullable(cmd, "@@ParLevel2_Id", coleta.ParLevel2_Id);
                        UtilSqlCommand.AddParameterNullable(cmd, "@@ParLevel3_Id", coleta.ParLevel3_Id);
                        UtilSqlCommand.AddParameterNullable(cmd, "@@ParFrequency_Id", coleta.Parfrequency_Id);
                        UtilSqlCommand.AddParameterNullable(cmd, "@@Evaluation", coleta.Evaluation);
                        UtilSqlCommand.AddParameterNullable(cmd, "@@Sample", coleta.Sample);
                        UtilSqlCommand.AddParameterNullable(cmd, "@@CollectionDate", coleta.CollectionDate);
                        var id = Convert.ToInt32(cmd.ExecuteScalar());

                    }
                }
            }
            catch (Exception ex)
            {
                coleta.HasError = true;
                coleta.GUIID = guiid.ToString();
                LogSystem.LogErrorBusiness.Register(ex, coleta);
            }
        }


        public void SaveAction(Acao item)
        {
            try
            {
                string sql = $@"INSERT INTO Pa.Acao(
                                    ParLevel1_Id				
                                    ,ParLevel2_Id				
                                    ,ParLevel3_Id				
                                    ,ParCompany_Id				
                                    ,ParDepartment_Id			
                                    ,ParDepartmentParent_Id	
                                    ,ParCargo_Id				
                                    ,Acao_Naoconformidade		
                                    ,AcaoText					
                                    ,DataConclusao				
                                    ,HoraConclusao				
                                    ,Referencia				
                                    ,Responsavel								
                                    ,DataEmissao				
                                    ,HoraEmissao				
                                    ,Emissor	
                                    ,Prioridade
                                    ,ParCluster_Id
                                    ,ParClusterGroup_Id
                                    ,Status)
                                    VALUES(
                                          @ParLevel1_Id			
                                         ,@ParLevel2_Id			
                                         ,@ParLevel3_Id			
                                         ,@ParCompany_Id			
                                         ,@ParDepartment_Id		
                                         ,@ParDepartmentParent_Id	
                                         ,@ParCargo_Id			
                                         ,@Acao_Naoconformidade	
                                         ,@AcaoText				
                                         ,@DataConclusao			
                                         ,@HoraConclusao			
                                         ,@Referencia				
                                         ,@Responsavel							
                                         ,@DataEmissao			
                                         ,@HoraEmissao			
                                         ,@Emissor				
                                         ,@Prioridade
                                         ,@ParCluster_Id
                                         ,@ParClusterGroup_Id
                                         ,@Status
                                        );

                SELECT CAST(scope_identity() AS int)";

                using (Factory factory = new Factory("DefaultConnection"))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, factory.connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        UtilSqlCommand.AddParameterNullable(cmd, "@ParLevel1_Id", item.ParLevel1_Id);
                        UtilSqlCommand.AddParameterNullable(cmd, "@ParLevel2_Id", item.ParLevel2_Id);
                        UtilSqlCommand.AddParameterNullable(cmd, "@ParLevel3_Id", item.ParLevel3_Id);
                        UtilSqlCommand.AddParameterNullable(cmd, "@ParCompany_Id", item.ParCompany_Id);
                        UtilSqlCommand.AddParameterNullable(cmd, "@ParDepartment_Id", item.ParDepartment_Id);
                        UtilSqlCommand.AddParameterNullable(cmd, "@ParDepartmentParent_Id", item.ParDepartmentParent_Id);
                        UtilSqlCommand.AddParameterNullable(cmd, "@ParCargo_Id", item.ParCargo_Id);
                        UtilSqlCommand.AddParameterNullable(cmd, "@Acao_Naoconformidade", item.Acao_Naoconformidade);
                        UtilSqlCommand.AddParameterNullable(cmd, "@AcaoText", item.AcaoText);
                        UtilSqlCommand.AddParameterNullable(cmd, "@DataConclusao", item.DataConclusao);
                        UtilSqlCommand.AddParameterNullable(cmd, "@HoraConclusao", item.HoraConclusao);
                        UtilSqlCommand.AddParameterNullable(cmd, "@Referencia", item.Referencia);
                        UtilSqlCommand.AddParameterNullable(cmd, "@Responsavel", item.Responsavel);
                        UtilSqlCommand.AddParameterNullable(cmd, "@DataEmissao", item.DataEmissao);
                        UtilSqlCommand.AddParameterNullable(cmd, "@HoraEmissao", item.HoraEmissao);
                        UtilSqlCommand.AddParameterNullable(cmd, "@Emissor", item.Emissor);
                        UtilSqlCommand.AddParameterNullable(cmd, "@Prioridade", item.Prioridade);
                        UtilSqlCommand.AddParameterNullable(cmd, "@ParCluster_Id", item.ParCluster_Id);
                        UtilSqlCommand.AddParameterNullable(cmd, "@ParClusterGroup_Id", item.ParClusterGroup_Id);
                        UtilSqlCommand.AddParameterNullable(cmd, "@Status", item.Status);

                        var id = (int)cmd.ExecuteScalar();

                        item.Id = id;
                    }
                }
            }
            catch (Exception e)
            {

            }
        }

        public void SaveAcaoXNotificarAcao(AcaoXNotificarAcao listAcaoXNotificarAcao)
        {
            try
            {
                string sql = $@"INSERT INTO Pa.AcaoXNotificarAcao(
                                    Acao_Id				
                                    ,UserSgq_Id				
                                    ,AddDate)
                                    VALUES(
                                          @Acao_Id			
                                         ,@UserSgq_Id			
                                         ,@AddDate			
                                        )";


                using (Factory factory = new Factory("DefaultConnection"))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, factory.connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        UtilSqlCommand.AddParameterNullable(cmd, "@Acao_Id", listAcaoXNotificarAcao.Acao_Id);
                        UtilSqlCommand.AddParameterNullable(cmd, "@UserSgq_Id", listAcaoXNotificarAcao.UserSgq_Id);
                        UtilSqlCommand.AddParameterNullable(cmd, "@AddDate", DateTime.Now);

                        var id = Convert.ToInt32(cmd.ExecuteScalar());

                        listAcaoXNotificarAcao.Id = id;
                    }
                }
            }
            catch (Exception e)
            {

            }
        }

        public string SaveFileEvidenciaNaoConformidade(int parLevel1_Id, int parLevel2_Id, int parLevel3_Id, string fileBase64)
        {
            var basePath = DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.StorageRoot ?? "~";
            if (basePath.Equals("~"))
            {
                basePath = @AppDomain.CurrentDomain.BaseDirectory;
            }

            string fileName = parLevel1_Id + parLevel2_Id + parLevel3_Id + DateTime.Now.GetHashCode() + new Random().Next(1000, 9999) + ".png";

            Exception exception;

            FileHelper.SavePhoto(fileBase64, basePath + "/Acao", fileName
                , DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.credentialUserServerPhoto
                , DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.credentialPassServerPhoto
                , DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.StorageRoot, out exception);

            if (exception != null)
                LogSystem.LogErrorBusiness.Register(exception);

            var path = Path.Combine(basePath, fileName);

            return path;
        }

        public void SaveEvidenciaNaoConformidade(EvidenciaNaoConformidade evidenciaNaoConformidade)
        {
            try
            {
                string sql = $@"INSERT INTO Pa.EvidenciaNaoConformidade(
                                    Acao_Id				
                                    ,Path
                                    ,AddDate)
                                    VALUES(
                                          @Acao_Id			
                                         ,@Path	
                                         ,@AddDate			
                                        )";


                using (Factory factory = new Factory("DefaultConnection"))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, factory.connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        UtilSqlCommand.AddParameterNullable(cmd, "@Acao_Id", evidenciaNaoConformidade.Acao_Id);
                        UtilSqlCommand.AddParameterNullable(cmd, "@Path", evidenciaNaoConformidade.Path);
                        UtilSqlCommand.AddParameterNullable(cmd, "@AddDate", DateTime.Now);

                        var id = Convert.ToInt32(cmd.ExecuteScalar());

                        evidenciaNaoConformidade.Id = id;
                    }
                }
            }
            catch (Exception e)
            {

            }
        }

        public string SaveFileEvidenciaAcaoConcluida(int parLevel1_Id, int parLevel2_Id, int parLevel3_Id, string fileBase64)
        {
            var basePath = DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.StorageRoot ?? "~";
            if (basePath.Equals("~"))
            {
                basePath = @AppDomain.CurrentDomain.BaseDirectory;
            }

            string fileName = parLevel1_Id + parLevel2_Id + parLevel3_Id + DateTime.Now.GetHashCode() + new Random().Next(1000, 9999) + ".png";

            Exception exception;

            FileHelper.SavePhoto(fileBase64, basePath + "\\Acao", fileName
                      , DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.credentialUserServerPhoto
                      , DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.credentialPassServerPhoto
                      , DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.StorageRoot, out exception);

            if (exception != null)
                LogSystem.LogErrorBusiness.Register(exception);

            var path = Path.Combine(basePath, fileName);

            return path;
        }

        public void SaveEvidenciaAcaoConcluida(EvidenciaAcaoConcluida evidenciaAcaoConcluida)
        {
            try
            {
                string sql = $@"INSERT INTO Pa.EvidenciaAcaoConcluida(
                                    Acao_Id				
                                    ,Path
                                    ,AddDate)
                                    VALUES(
                                          @Acao_Id			
                                         ,@Path	
                                         ,@AddDate			
                                        )";


                using (Factory factory = new Factory("DefaultConnection"))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, factory.connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        UtilSqlCommand.AddParameterNullable(cmd, "@Acao_Id", evidenciaAcaoConcluida.Acao_Id);
                        UtilSqlCommand.AddParameterNullable(cmd, "@Path", evidenciaAcaoConcluida.Path);
                        UtilSqlCommand.AddParameterNullable(cmd, "@AddDate", DateTime.Now);

                        var id = Convert.ToInt32(cmd.ExecuteScalar());

                        evidenciaAcaoConcluida.Id = id;
                    }
                }
            }
            catch (Exception e)
            {

            }
        }

        public void SaveCollection(Collection item)
        {
            try
            {
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
                   ,[IsProcessed]
                   ,[Hash])
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
                   ,@IsProcessed
                   ,@Hash);
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
                        UtilSqlCommand.AddParameterNullable(cmd, "@Hash", item.Hash);
                        var id = Convert.ToInt32(cmd.ExecuteScalar());

                        item.Id = id;
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.LogErrorBusiness.Register(ex, item);
            }
        }

        public List<UserSgq> GetUsersByCompany(int ParCompany_Id)
        {
            var query = $@"select* from usersgq
                          where 1 = 1
                          and isactive = 1
                          and id in (
                              SELECT
                          
                                  PCXU.UserSgq_Id
                              FROM
                          
                                  ParCompanyXUserSgq PCXU WITH(NOLOCK)
                          
                              INNER JOIN UserSgq US ON PCXU.ParCompany_Id = {ParCompany_Id}
                          UNION ALL
                          
                              SELECT US.Id
                              FROM UserSgq US WITH(NOLOCK)
                          
                              WHERE US.ParCompany_Id = {ParCompany_Id})";

            using (Factory factory = new Factory("DefaultConnection"))
            {
                var retorno = factory.SearchQuery<UserSgq>(query).ToList();

                return retorno;
            }
        }
    }
}
