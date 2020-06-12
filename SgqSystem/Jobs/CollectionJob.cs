using ADOFactory;
using Dominio;
using Helper;
using Quartz;
using SgqSystem.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SgqSystem.Jobs
{
    public class CollectionJob : IJob
    {
        private static int quantidadeMaximaProcessada = 100;
        private static int quantidadeProcessada = 100;
        public void Execute(IJobExecutionContext context)
        {
            ExecuteCollectionJob(null);
        }

        public static void ExecuteCollectionJob(object stateInfo)
        {

            Task.Run(() =>
                Execute()
            );

        }

        private static void Execute()
        {
            while (true)
            {
                int intervalTimeCollectionJob = 0;
                try
                {
                    Int32.TryParse(DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.CollectionJobTime0IsDisabled, out intervalTimeCollectionJob);
                }
                catch (Exception)
                {

                }

                if (intervalTimeCollectionJob > 0)
                {
                    if (quantidadeProcessada == quantidadeMaximaProcessada)
                    {
                        //Pegar as collectionsLevel2 da Collection
                        List<CollectionLevel2> collectionsLevel2MontadoDaCollection = GetCollectionsLevel2NotProcess();
                        quantidadeProcessada = quantidadeMaximaProcessada - collectionsLevel2MontadoDaCollection.Count;

                        LogSystem.LogErrorBusiness.Register(new Exception("Iniciado CollectionJob - quantidadeProcessada: " + quantidadeProcessada));
                        ConsolidarCollectionLevel2(collectionsLevel2MontadoDaCollection);
                    }

                    Thread.Sleep(intervalTimeCollectionJob);
                }
                else
                {
                    Thread.Sleep(30000);
                }
            }

        }

        public static void ConsolidarCollectionLevel2(List<CollectionLevel2> collectionsLevel2MontadoDaCollection)
        {
            try
            {
                List<ParLevel3> parLevel3List = new List<ParLevel3>();
                int consolidationLevel2_Id = 0;
                using (var db = new SgqDbDevEntities())
                {
                    db.Configuration.AutoDetectChangesEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;
                    parLevel3List = db.ParLevel3.ToList();
                    //var headerFieldsProcess_Id = new List<int>();
                    consolidationLevel2_Id = returnConsolidationLevel2Id();
                }

                foreach (var collectionLevel2MontadoDaCollection in collectionsLevel2MontadoDaCollection)
                {
                    try
                    {
                        using (var db = new SgqDbDevEntities())
                        {
                            db.Configuration.AutoDetectChangesEnabled = false;
                            db.Configuration.LazyLoadingEnabled = false;

                            var resultsLevel3 = GetResultLevel3NotProcess(collectionLevel2MontadoDaCollection);
                            var collectionLevel2Consolidada = SetConsolidation(collectionLevel2MontadoDaCollection, resultsLevel3);
                            var collectionLevel2DoBanco = db.CollectionLevel2.Where(x => x.Key == collectionLevel2Consolidada.Key).FirstOrDefault();

                            if (collectionLevel2DoBanco == null)
                            {
                                collectionLevel2Consolidada.ConsolidationLevel2_Id = consolidationLevel2_Id;

                                var collectionLevel2Save = db.CollectionLevel2.Add(collectionLevel2Consolidada);
                                db.SaveChanges();

                                if (collectionLevel2MontadoDaCollection.ParCargo_Id != null)
                                    db.CollectionLevel2XParCargo.Add(new CollectionLevel2XParCargo() { AddDate = DateTime.Now, CollectionLevel2_Id = collectionLevel2Save.Id, ParCargo_Id = collectionLevel2MontadoDaCollection.ParCargo_Id.Value });

                                if (collectionLevel2MontadoDaCollection.ParCluster_Id != null)
                                    db.CollectionLevel2XCluster.Add(new CollectionLevel2XCluster() { /*AddDate = DateTime.Now,*/ CollectionLevel2_Id = collectionLevel2Save.Id, ParCluster_Id = collectionLevel2MontadoDaCollection.ParCluster_Id.Value });

                                if (collectionLevel2MontadoDaCollection.ParDepartment_Id != null)
                                    db.CollectionLevel2XParDepartment.Add(new CollectionLevel2XParDepartment() { AddDate = DateTime.Now, CollectionLevel2_Id = collectionLevel2Save.Id, ParDepartment_Id = collectionLevel2MontadoDaCollection.ParDepartment_Id.Value });

                                if (collectionLevel2MontadoDaCollection.Outros?.GetIntFromJsonText("ParFamiliaProduto_Id") != null)
                                    db.CollectionLevel2XParFamiliaProdutoXParProduto.Add(
                                        new Dominio.Seara.CollectionLevel2XParFamiliaProdutoXParProduto()
                                        {
                                            AddDate = DateTime.Now,
                                            CollectionLevel2_Id = collectionLevel2Save.Id,
                                            ParFamiliaProduto_Id = collectionLevel2MontadoDaCollection.Outros.GetIntFromJsonText("ParFamiliaProduto_Id").Value,
                                            ParProduto_Id = collectionLevel2MontadoDaCollection.Outros.GetIntFromJsonText("ParProduto_Id")
                                        });

                                db.SaveChanges();
                            }
                            else
                            {
                                collectionLevel2DoBanco.ParDepartment_Id = collectionLevel2Consolidada.ParDepartment_Id; //db.CollectionLevel2XParDepartment.Where(x => x.CollectionLevel2_Id == collection.Id).Select(x => x.ParDepartment_Id).FirstOrDefault();
                                collectionLevel2DoBanco.ParCargo_Id = collectionLevel2Consolidada.ParCargo_Id; //db.CollectionLevel2XParCargo.Where(x => x.CollectionLevel2_Id == collection.Id).Select(x => x.ParCargo_Id).FirstOrDefault();
                                collectionLevel2DoBanco.ParCluster_Id = collectionLevel2Consolidada.ParCluster_Id;//db.CollectionLevel2XCluster.Where(x => x.CollectionLevel2_Id == collection.Id).Select(x => x.ParCluster_Id).FirstOrDefault();
                                collectionLevel2DoBanco.CollectionDate = collectionLevel2Consolidada.CollectionDate;
                                collectionLevel2Consolidada = collectionLevel2DoBanco;
                            }

                            var collectionsProcessed_Id = new List<int>();
                            var collectionsProcessWithError_Id = new List<int>();

                            using (Factory factory = new Factory("DefaultConnection"))
                            {
                                foreach (var resultLevel3 in resultsLevel3)
                                {
                                    if (!(resultLevel3.ParLevel3_Id > 0))
                                        continue;

                                    int collectionId = resultLevel3.Id;

                                    try
                                    {
                                        resultLevel3.CollectionLevel2_Id = collectionLevel2Consolidada.Id;
                                        resultLevel3.HasPhoto = resultLevel3.HasPhoto == null ? false : resultLevel3.HasPhoto;
                                        resultLevel3.ParLevel3_Name = parLevel3List
                                        .Where(x => x.Id == resultLevel3.ParLevel3_Id)
                                        .Select(x => x.Name).FirstOrDefault();

                                        #region inserir collection
                                        string sql = $@"
INSERT INTO [Result_Level3]
           ([CollectionLevel2_Id]
           ,[ParLevel3_Id]
           ,[ParLevel3_Name]
           ,[Weight]
           ,[IntervalMin]
           ,[IntervalMax]
           ,[Value]
           ,[ValueText]
           ,[IsConform]
           ,[IsNotEvaluate]
           ,[Defects]
           ,[PunishmentValue]
           ,[WeiEvaluation]
           ,[Evaluation]
           ,[WeiDefects]
           ,[CT4Eva3]
           ,[Sampling]
           ,[HasPhoto])
     VALUES
           (@CollectionLevel2_Id
           ,@ParLevel3_Id
           ,@ParLevel3_Name
           ,@Weight
           ,@IntervalMin
           ,@IntervalMax
           ,@Value
           ,@ValueText
           ,@IsConform
           ,@IsNotEvaluate
           ,@Defects
           ,@PunishmentValue
           ,@WeiEvaluation
           ,@Evaluation
           ,@WeiDefects
           ,@CT4Eva3
           ,@Sampling
           ,@HasPhoto);
            SELECT @@IDENTITY AS 'Identity';";

                                        using (SqlCommand cmd = new SqlCommand(sql, factory.connection))
                                        {
                                            cmd.CommandType = CommandType.Text;
                                            UtilSqlCommand.AddParameterNullable(cmd, "@CollectionLevel2_Id", resultLevel3.CollectionLevel2_Id);
                                            UtilSqlCommand.AddParameterNullable(cmd, "@ParLevel3_Id", resultLevel3.ParLevel3_Id);
                                            UtilSqlCommand.AddParameterNullable(cmd, "@ParLevel3_Name", resultLevel3.ParLevel3_Name);
                                            UtilSqlCommand.AddParameterNullable(cmd, "@Weight", resultLevel3.Weight);
                                            UtilSqlCommand.AddParameterNullable(cmd, "@IntervalMin", resultLevel3.IntervalMin);
                                            UtilSqlCommand.AddParameterNullable(cmd, "@IntervalMax", resultLevel3.IntervalMax);
                                            UtilSqlCommand.AddParameterNullable(cmd, "@Value", resultLevel3.Value);
                                            UtilSqlCommand.AddParameterNullable(cmd, "@ValueText", resultLevel3.ValueText);
                                            UtilSqlCommand.AddParameterNullable(cmd, "@IsConform", resultLevel3.IsConform);
                                            UtilSqlCommand.AddParameterNullable(cmd, "@IsNotEvaluate", resultLevel3.IsNotEvaluate);
                                            UtilSqlCommand.AddParameterNullable(cmd, "@Defects", resultLevel3.Defects);
                                            UtilSqlCommand.AddParameterNullable(cmd, "@PunishmentValue", resultLevel3.PunishmentValue);
                                            UtilSqlCommand.AddParameterNullable(cmd, "@WeiEvaluation", resultLevel3.WeiEvaluation);
                                            UtilSqlCommand.AddParameterNullable(cmd, "@Evaluation", resultLevel3.Evaluation);
                                            UtilSqlCommand.AddParameterNullable(cmd, "@WeiDefects", resultLevel3.WeiDefects);
                                            UtilSqlCommand.AddParameterNullable(cmd, "@CT4Eva3", resultLevel3.CT4Eva3);
                                            UtilSqlCommand.AddParameterNullable(cmd, "@Sampling", resultLevel3.Sampling);
                                            UtilSqlCommand.AddParameterNullable(cmd, "@HasPhoto", resultLevel3.HasPhoto);
                                            var id = Convert.ToInt32(cmd.ExecuteScalar());
                                        }
                                        #endregion

                                        collectionsProcessed_Id.Add(collectionId);
                                    }
                                    catch (Exception ex)
                                    {
                                        LogSystem.LogErrorBusiness.Register(ex, new { collectionId });
                                        collectionsProcessWithError_Id.Add(collectionId);
                                    }
                                }
                            }

                            UpdateCollectionStatus(collectionsProcessed_Id, collectionsProcessWithError_Id);

                            //validar primeiro se existe o headerField Inserido
                            //se existir header Fields para essa collectionLevel2, remove os cabeçalhos
                            DeleteHeaderFieldIfExists(collectionLevel2Consolidada);

                            RegisterHeaderField(collectionLevel2Consolidada);
                        }
                    }
                    catch (Exception ex)
                    {
                        LogSystem.LogErrorBusiness.Register(ex);
                    }
                    finally
                    {
                        quantidadeProcessada++;
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.LogErrorBusiness.Register(ex);
            }
        }

        public static void UpdateCollectionStatus(List<int> collectionsProcessed_Id, List<int> collectionWithError_Id)
        {
            try
            {
                if (collectionsProcessed_Id.Count > 0)
                {
                    using (var factory = new Factory("DefaultConnection"))
                    {
                        try
                        {
                            factory.ExecuteSql("UPDATE Collection set IsProcessed = 1 where Id in (" + string.Join(",", collectionsProcessed_Id) + ")");
                        }
                        catch (Exception ex)
                        {
                            LogSystem.LogErrorBusiness.Register(ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.LogErrorBusiness.Register(ex, new { collectionsProcessed_Id });
            }

            try
            {
                if (collectionWithError_Id.Count > 0)
                {
                    using (var factory = new Factory("DefaultConnection"))
                    {
                        try
                        {
                            factory.ExecuteSql("UPDATE Collection set IsProcessed = null where Id in (" + string.Join(",", collectionWithError_Id) + ")");
                        }
                        catch (Exception ex)
                        {
                            LogSystem.LogErrorBusiness.Register(ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.LogErrorBusiness.Register(ex, new { collectionWithError_Id });
            }
        }

        private static List<CollectionLevel2> GetCollectionsLevel2NotProcess()
        {
            var sql = $@"
                        SELECT DISTINCT top {quantidadeMaximaProcessada}
                    	Evaluation as EvaluationNumber
                       ,Sample
                       ,ParLevel1_Id
                       ,ParLevel2_Id
                       ,Shift_Id as Shift
                       ,Period_Id as Period
                       ,ParCompany_Id as UnitId
                       ,ParCargo_Id
                       ,ParCluster_Id
                       ,ParDepartment_Id
                       ,ParFrequency_Id
                       ,IIF(UserSgq_Id is null, 0,UserSgq_Id) as AuditorId
                       ,CONVERT(VARCHAR(19),IIF(DATEPART(MILLISECOND,CollectionDate)>500,DATEADD(SECOND,1,CollectionDate),CollectionDate),120) AS CollectionDate
                       ,GETDATE() as StartPhaseDate
                        ,Outros
                        FROM Collection with (nolock)
                        WHERE IsProcessed = 0
                        AND ParHeaderField_Id IS NULL
                        AND ParHeaderField_Value IS NULL
                        AND Evaluation is not null
						AND Sample is not null
                        and ParCompany_Id is not null
                        and adddate < '{DateTime.Now.AddMinutes(-2).ToString("yyyy-MM-dd HH:mm")}'";

            var collectionLevel2 = new List<CollectionLevel2>();

            using (Factory factory = new Factory("DefaultConnection"))
            {

                try
                {
                    collectionLevel2 = factory.SearchQuery<CollectionLevel2>(sql).ToList();
                }
                catch (Exception ex)
                {
                    LogSystem.LogErrorBusiness.Register(ex, new { sql });
                }

                return collectionLevel2;
            }
        }

        private static List<Result_Level3> GetResultLevel3NotProcess(CollectionLevel2 collection)
        {
            var resultsLevel3 = new List<Result_Level3>();

            var sql = $@"
                    SELECT ParLevel3_Id
                        ,Id
                        ,Weigth as Weight
                        ,IntervalMin
                        ,IntervalMax
                        ,Value
                        ,ValueText
                        ,IsConform
                        ,IsNotEvaluate
                        ,Defects
                        ,PunishimentValue
                        ,WeiEvaluation
                        ,Evaluation
                        ,WeiDefects
                        ,HasPhoto  FROM Collection with (nolock)
                    WHERE Evaluation = {collection.EvaluationNumber} AND IsProcessed = 0 AND
                        Sample = {collection.Sample} AND ParLevel1_Id = {collection.ParLevel1_Id} AND
                        ParLevel2_Id = {collection.ParLevel2_Id} AND Shift_Id = {collection.Shift} AND
                        Period_Id = {collection.Period} AND ParCompany_Id = {collection.UnitId} 
                        AND (ParCluster_Id = {collection.ParCluster_Id ?? 0} OR ParCluster_Id IS NULL)
                        AND (ParCargo_Id = {collection.ParCargo_Id ?? 0} OR ParCargo_Id IS NULL)
                        AND ParFrequency_Id {((collection.ParFrequency_Id > 0) ? (" = " + collection.ParFrequency_Id) : " IS NULL")} 
                        AND (ParDepartment_Id = {collection.ParDepartment_Id ?? 0}  OR ParDepartment_Id IS NULL)
                        AND CAST(CONVERT(VARCHAR(19), IIF(DATEPART(MILLISECOND, CollectionDate) > 500, DATEADD(SECOND, 1, CollectionDate), CollectionDate), 120) AS DATE) = '{collection.CollectionDate.ToString("yyyy-MM-dd")}'";

            using (Factory factory = new Factory("DefaultConnection"))
            {
                try
                {
                    resultsLevel3 = factory.SearchQuery<Result_Level3>(sql).ToList();
                }
                catch (Exception ex)
                {
                    LogSystem.LogErrorBusiness.Register(ex, collection);
                }

                return resultsLevel3;
            }
        }

        private static CollectionLevel2 SetConsolidation(CollectionLevel2 collection, List<Result_Level3> results_Level3)
        {

            decimal defects = 0;
            decimal weiDefects = 0;
            int evaluation = 0;
            decimal weiEavaluation = 0;

            foreach (var result_Level3 in results_Level3)
            {
                defects += result_Level3.Defects == null ? 0 : result_Level3.Defects.Value;
                weiDefects += result_Level3.WeiDefects == null ? 0 : result_Level3.WeiDefects.Value;
                evaluation++;
                weiEavaluation += result_Level3.Weight == null ? 0 : result_Level3.Weight.Value;
            }

            collection.Defects = defects;
            collection.WeiDefects = weiDefects;
            collection.TotalLevel3Evaluation = evaluation;
            collection.WeiEvaluation = weiEavaluation;
            collection.AuditorId = collection.AuditorId == 0 ? 1 : collection.AuditorId;
            collection.AlterDate = null;
            collection.AddDate = DateTime.Now;
            collection.Key = collection.CollectionDate.ToString("yyyy-MM-dd") + "-" + collection.UnitId + "-" +
                collection.ParLevel1_Id + "-" + collection.ParLevel2_Id + "-" + collection.Shift + "-" +
                collection.ParCluster_Id + "-" + collection.ParCargo_Id + "-" + collection.ParDepartment_Id + "-" +
                collection.EvaluationNumber + "-" + collection.Sample + "-" + collection.ParFrequency_Id + "-" +
                collection.Outros?.GetFromJsonText("ParFamiliaProduto_Id");

            return collection;
        }

        private static void RegisterHeaderField(CollectionLevel2 collectionLevel2)
        {
            //se não existir insere um novo
            var headerFields = GetHeaderFieldsByCollectionLevel2(collectionLevel2);

            if (headerFields.Count > 0)
            {
                try
                {
                    foreach (var headerField in headerFields)
                    {
                        var sql = $@"INSERT INTO [dbo].[CollectionLevel2XParHeaderFieldGeral]
                               ([CollectionLevel2_Id]
                               ,[ParHeaderFieldGeral_Id]
                               ,[ParHeaderField_Name]
                               ,[ParFieldType_Id]
                               ,[Value]
                               ,[Evaluation]
                               ,[Sample])
                         VALUES
                               ({headerField.CollectionLevel2_Id}
                               ,{headerField.ParHeaderFieldGeral_Id}
                               ,'{headerField.ParHeaderField_Name}'
                               ,{headerField.ParFieldType_Id}
                               ,'{headerField.Value}'
                               ,{(headerField.Evaluation == null ? "null" : headerField.Evaluation.ToString())}
                               ,{(headerField.Sample == null ? "null" : headerField.Sample.ToString())});";


                        using (var factory = new Factory("DefaultConnection"))
                        {
                            try
                            {
                                factory.ExecuteSql(sql);
                            }
                            catch (Exception ex)
                            {
                                LogSystem.LogErrorBusiness.Register(ex, new { sql });
                            }
                        }
                    }

                    var headerFieldsCollectionsIds = headerFields.Select(x => x.Collection_Id).ToList();
                    using (var factory = new Factory("DefaultConnection"))
                    {
                        try
                        {
                            factory.ExecuteSql("UPDATE Collection set IsProcessed = 1 where Id in (" + string.Join(",", headerFieldsCollectionsIds) + ")");
                        }
                        catch (Exception ex)
                        {
                            LogSystem.LogErrorBusiness.Register(ex);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogSystem.LogErrorBusiness.Register(ex);
                }
            }

        }
        private static void DeleteHeaderFieldIfExists(CollectionLevel2 collectionLevel2)
        {
            var sql = $@"delete CollectionLevel2XParHeaderFieldGeral WHERE Id in(
                         select CL2XHF.Id FROM CollectionLevel2XParHeaderFieldGeral CL2XHF with (nolock)
                         inner JOIN CollectionLevel2 C2 with (nolock) on C2.Id = CL2XHF.CollectionLevel2_Id
                         AND C2.[key] = '{ collectionLevel2.Key }'
                         AND CL2XHF.CollectionLevel2_Id = { collectionLevel2.Id }
                         )";

            using (var factory = new Factory("DefaultConnection"))
            {
                factory.ExecuteSql(sql);
            }
        }

        private static List<CollectionLevel2XParHeaderFieldGeral> GetHeaderFieldsByCollectionLevel2(CollectionLevel2 collectionLevel2)
        {
            var headerFields = new List<CollectionLevel2XParHeaderFieldGeral>();

            using (var factory = new Factory("DefaultConnection"))
            {

                var collectionDate = collectionLevel2.CollectionDate.ToString("yyyy-MM-dd HH:mm:ss");

                var queryParCargo = "";
                var queryParDepartment = "";

                if (collectionLevel2.ParCargo_Id == null || collectionLevel2.ParCargo_Id == 0)
                    queryParCargo = "Is NULL";
                else
                    queryParCargo = " = " + collectionLevel2.ParCargo_Id;


                if (collectionLevel2.ParDepartment_Id == null || collectionLevel2.ParDepartment_Id == 0)
                    queryParDepartment = "Is NULL";
                else
                    queryParDepartment = " = " + collectionLevel2.ParDepartment_Id;

                var sql = $@"SELECT
                            	CL.ParHeaderField_Id as ParHeaderFieldGeral_Id
                               ,CL.ParHeaderField_Value as Value
                               ,PHFG.ParFieldType_Id
                               ,PHFG.Name as ParHeaderField_Name
                               ,CL.Evaluation
                               ,CL.Sample
                               ,CL.Id as Collection_Id
                               ,{collectionLevel2.Id} as CollectionLevel2_Id
                            FROM Collection CL with (nolock)
                            INNER JOIN ParHeaderFieldGeral PHFG with (nolock) on CL.ParHeaderField_Id = PHFG.Id
                            WHERE 1 =1 AND ParHeaderField_Id IS NOT NULL
                            AND CL.UserSgq_Id = {collectionLevel2.AuditorId}
                            AND cl.Shift_Id = {collectionLevel2.Shift}
                            AND cl.Period_Id = {collectionLevel2.Period}
                            AND CL.ParCargo_Id {queryParCargo}
                            AND cl.ParCompany_Id = {collectionLevel2.UnitId}
                            AND ((cl.ParDepartment_Id {queryParDepartment} AND cl.ParLevel1_Id IS NULL AND cl.ParLevel2_Id IS NULL) OR
                                (cl.ParDepartment_Id {queryParDepartment} AND cl.ParLevel1_Id = {collectionLevel2.ParLevel1_Id} AND cl.ParLevel2_Id IS NULL) OR
                                (cl.ParDepartment_Id {queryParDepartment} AND cl.ParLevel1_Id = {collectionLevel2.ParLevel1_Id} AND cl.ParLevel2_Id = {collectionLevel2.ParLevel2_Id}))
                            AND cl.Evaluation = {collectionLevel2.EvaluationNumber}
                            AND (cl.Sample = {collectionLevel2.Sample} 
                                OR cl.Outros like '%ParFamiliaProduto_Id%') 
                            AND Cl.CollectionDate BETWEEN DATEADD(minute, -5, '{collectionDate}') and DATEADD(minute, 5, '{collectionDate}')";

                headerFields = factory.SearchQuery<CollectionLevel2XParHeaderFieldGeral>(sql).ToList();
            }

            return headerFields;
        }

        private static int getConsolidationLevel2_Id()
        {
            var id = 0;

            using (var factory = new Factory("DefaultConnection"))
            {
                var consolidation = factory.SearchQuery<ConsolidationLevel1>("SELECT TOP 1 * FROM ConsolidationLevel2 ORDER BY id DESC").FirstOrDefault();

                return consolidation == null ? id : consolidation.Id;
            }
        }

        private static int setConsolidationLevel1()
        {
            using (var db = new SgqDbDevEntities())
            {
                try
                {
                    var consolidationLevel1 = new ConsolidationLevel1()
                    {
                        AddDate = DateTime.Now,
                        UnitId = db.ParCompany.FirstOrDefault()?.Id ?? 1,
                        DepartmentId = getDepartment(),
                        ParLevel1_Id = GetParLevel1(),
                        ConsolidationDate = DateTime.Now
                    };

                    db.ConsolidationLevel1.Add(consolidationLevel1);
                    db.SaveChanges();
                    return consolidationLevel1.Id;
                }
                catch (Exception ex)
                {

                }
                return 0;

            }
        }

        private static int GetParLevel1()
        {
            var level1 = new ParLevel1();
            using (var db = new SgqDbDevEntities())
            {
                level1 = db.ParLevel1.FirstOrDefault();
            }
            return level1.Id;
        }

        private static int setConsolidationLevel2(int consolidationLevel1_Id)
        {
            using (var db = new SgqDbDevEntities())
            {
                var consolidationLevel2 = new ConsolidationLevel2()
                {
                    ConsolidationLevel1_Id = consolidationLevel1_Id,
                    ParLevel2_Id = GetParLevel2(),
                    AddDate = DateTime.Now,
                    UnitId = db.ParCompany.FirstOrDefault()?.Id ?? 1
                };

                try
                {
                    db.ConsolidationLevel2.Add(consolidationLevel2);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {

                }

                return consolidationLevel2.Id;
            }
        }

        private static int GetParLevel2()
        {
            var level2 = new ParLevel2();
            using (var db = new SgqDbDevEntities())
            {
                level2 = db.ParLevel2.FirstOrDefault();
            }
            return level2.Id;
        }

        public static int returnConsolidationLevel2Id()
        {
            var idConsolidationL2 = getConsolidationLevel2_Id();

            if (idConsolidationL2 == 0)
            {
                var cl1 = setConsolidationLevel1();

                return setConsolidationLevel2(cl1);
            }
            else
            {
                return idConsolidationL2;
            }
        }

        private static int getDepartment()
        {
            var id = 0;

            using (var factory = new Factory("DefaultConnection"))
            {
                var department = factory.SearchQuery<ParDepartment>("SELECT TOP 1 Id FROM Department ORDER BY id DESC").FirstOrDefault();
                if (department != null)
                    id = department.Id;
            }

            if (id == 0)
                id = setDepartment();

            return id;
        }

        private static int setDepartment()
        {
            using (var factory = new Factory("DefaultConnection"))
            {
                try
                {
                    factory.ExecuteSql("INSERT INTO Department (AddDate, Name) VALUES(GETDATE(), 'Department Padrão')");
                }
                catch (Exception ex)
                {

                }

                return getDepartment();
            }
        }
    }
}