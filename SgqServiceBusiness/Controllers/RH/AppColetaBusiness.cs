using ADOFactory;
using Dominio;
using SgqSystem.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                return collectionDuplicada;
            }
            catch (Exception ex)
            {
                LogSystem.LogErrorBusiness.Register(ex, collectionDuplicada);
                return null;
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
                LogSystem.LogErrorBusiness.Register(ex, item);
            }
        }
    }
}
