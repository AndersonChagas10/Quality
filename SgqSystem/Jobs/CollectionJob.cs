using ADOFactory;
using Dominio;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace SgqSystem.Jobs
{
    public class CollectionJob : IJob
    {

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
                //Pegar as collectionsLevel2 da Collection
                using (var db = new SgqDbDevEntities())
                {
                    var collectionsLevel2 = GetCollectionsLevel2NotProcess();
                    var headerFieldsProcess_Id = new List<int>();
                    var parLevel3List = db.ParLevel3.ToList();

                    try
                    {
                        foreach (var collectionLevel2 in collectionsLevel2)
                        {
                            var collectionsProcess_Id = new List<int>();
                            var resultsLevel3 = GetResultLevel3NotProcess(collectionLevel2);
                            var collectionLevel2Consolidada = SetConsolidation(collectionLevel2, resultsLevel3);
                            var collection = db.CollectionLevel2.Where(x => x.Key == collectionLevel2Consolidada.Key).FirstOrDefault();

                            if (collection == null)
                            {
                                db.CollectionLevel2.Add(collectionLevel2Consolidada);
                                db.SaveChanges();
                            }
                            else
                            {
                                collectionLevel2Consolidada = collection;
                            }

                            foreach (var resultLevel3 in resultsLevel3)
                            {
                                try
                                {
                                    resultLevel3.CollectionLevel2_Id = collectionLevel2Consolidada.Id;
                                    resultLevel3.HasPhoto = resultLevel3.HasPhoto == null ? false : resultLevel3.HasPhoto;
                                    resultLevel3.ParLevel3_Name = parLevel3List.Where(x => x.Id == resultLevel3.ParLevel3_Id).Select(x => x.Name).FirstOrDefault();
                                    collectionsProcess_Id.Add(resultLevel3.Id);
                                    db.Result_Level3.Add(resultLevel3);

                                    db.SaveChanges();
                                }
                                catch (Exception ex)
                                {
                                    collectionsProcess_Id.RemoveAt(collectionsProcess_Id.Count - 1);
                                }
                            }

                            try
                            {
                                if (collectionsProcess_Id.Count > 0)
                                {
                                    db.Database.ExecuteSqlCommand("UPDATE Collection set IsProcessed = 1 where Id in (" + string.Join(",", collectionsProcess_Id) + ")");
                                }
                            }
                            catch (Exception ex)
                            {

                            }

                            //validar primeiro se existe o headerField Inserido
                            //se existir header Fields para essa collectionLevel2, remove os cabeçalhos
                            DeleteHeaderFieldIfExists(collectionLevel2Consolidada);

                            //se não existir insere um novo
                            var headerFields = GetHeaderFieldsByCollectionLevel2(collectionLevel2Consolidada);

                            if (headerFields.Count > 0)
                            {
                                var headerFieldsIds = headerFields.Select(x => x.Id).ToList();

                                try
                                {
                                    db.CollectionLevel2XParHeaderField.AddRange(headerFields);
                                    db.SaveChanges();

                                }
                                catch (Exception ex)
                                {

                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //return BadRequest(ex.ToClient());
                    }
                }

                Thread.Sleep(30000);
            }

        }

        private static List<CollectionLevel2> GetCollectionsLevel2NotProcess()
        {
            var sql = $@"
                        SELECT DISTINCT top 100 
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
                       ,IIF(UserSgq_Id is null, 0,UserSgq_Id) as AuditorId
                       ,CONVERT(VARCHAR(19),IIF(DATEPART(MILLISECOND,CollectionDate)>500,DATEADD(SECOND,1,CollectionDate),CollectionDate),120) AS CollectionDate
                       ,GETDATE() as StartPhaseDate
                       ,UserSgq_Id as AuditorId
                        FROM Collection
                        WHERE IsProcessed = 0
                        AND ParHeaderField_Id IS NULL";

            var collectionLevel2 = new List<CollectionLevel2>();

            using (Factory factory = new Factory("DefaultConnection"))
            {

                try
                {
                    collectionLevel2 = factory.SearchQuery<CollectionLevel2>(sql).ToList();
                }
                catch (Exception ex)
                {
                }

                return collectionLevel2;
            }
        }

        private static List<Result_Level3> GetResultLevel3NotProcess(CollectionLevel2 collectionLevel2)
        {
            var resultsLevel3 = new List<Result_Level3>();

            var sql = $@"
                    SELECT ParLevel3_Id
                        ,Id
                        ,Weigth
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
                        ,HasPhoto  FROM Collection 
                    WHERE Evaluation = {collectionLevel2.EvaluationNumber} AND IsProcessed = 0 AND
                        Sample = {collectionLevel2.Sample} AND ParLevel1_Id = {collectionLevel2.ParLevel1_Id} AND
                        ParLevel2_Id = {collectionLevel2.ParLevel2_Id} AND Shift_Id = {collectionLevel2.Shift} AND
                        Period_Id = {collectionLevel2.Period} AND ParCompany_Id = {collectionLevel2.UnitId} AND
                        CAST(CONVERT(VARCHAR(19), IIF(DATEPART(MILLISECOND, CollectionDate) > 500, DATEADD(SECOND, 1, CollectionDate), CollectionDate), 120) AS DATE) = '{collectionLevel2.CollectionDate.ToString("yyyy-MM-dd")}'";

            using (Factory factory = new Factory("DefaultConnection"))
            {
                try
                {
                    resultsLevel3 = factory.SearchQuery<Result_Level3>(sql).ToList();
                }
                catch (Exception ex)
                {
                }

                return resultsLevel3;
            }
        }

        private static CollectionLevel2 SetConsolidation(CollectionLevel2 collectionLevel2, List<Result_Level3> results_Level3)
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

            collectionLevel2.Defects = defects;
            collectionLevel2.WeiDefects = weiDefects;
            collectionLevel2.TotalLevel3Evaluation = evaluation;
            collectionLevel2.WeiEvaluation = weiEavaluation;
            collectionLevel2.AuditorId = collectionLevel2.AuditorId == 0 ? 1 : collectionLevel2.AuditorId;
            collectionLevel2.AlterDate = null;
            collectionLevel2.AddDate = DateTime.Now;
            collectionLevel2.Key = collectionLevel2.CollectionDate.ToString("yyyy-MM-dd") + "-" + collectionLevel2.UnitId + "-" +
                collectionLevel2.ParLevel1_Id + "-" + collectionLevel2.ParLevel2_Id + "-" + collectionLevel2.Shift + "-" +
                collectionLevel2.ParCluster_Id + "-" + collectionLevel2.ParCargo_Id + "-" + collectionLevel2.ParDepartment_Id + "-" +
                collectionLevel2.EvaluationNumber + "-" + collectionLevel2.Sample;

            return collectionLevel2;
        }


        private static void DeleteHeaderFieldIfExists(CollectionLevel2 collectionLevel2)
        {
            var sql = $@"delete CollectionLevel2XParHeaderField WHERE Id in(
                         select CL2XHF.Id FROM CollectionLevel2XParHeaderField CL2XHF
                         inner JOIN CollectionLevel2 C2 on C2.Id = CL2XHF.CollectionLevel2_Id
                         --AND C2.UserSgq_Id = {collectionLevel2.UserSgq}
                         AND C2.Shift = { collectionLevel2.Shift }
                         AND C2.Period = { collectionLevel2.Period }
                         AND C2.ParCargo_Id = { collectionLevel2.ParCargo_Id }
                         AND C2.UnitId = { collectionLevel2.UnitId }
                         AND C2.ParDepartment_Id = { collectionLevel2.ParDepartment_Id }
                         AND C2.EvaluationNumber = { collectionLevel2.EvaluationNumber }
                         AND C2.Sample = { collectionLevel2.Sample }
                         AND CL2XHF.CollectionLevel2_Id = { collectionLevel2.Id }
                         AND Cast(C2.CollectionDate as DATE) = Cast('{ collectionLevel2.CollectionDate.ToString("yyyy-MM-dd")}' as DATE)
                         )";

            using (var factory = new Factory("DefaultConnection"))
            {
                factory.ExecuteSql(sql);
            }
        }

        private static List<CollectionLevel2XParHeaderField> GetHeaderFieldsByCollectionLevel2(CollectionLevel2 collectionLevel2)
        {
            var headerFields = new List<CollectionLevel2XParHeaderField>();

            using (var factory = new Factory("DefaultConnection"))
            {

                var collectionDate = collectionLevel2.CollectionDate.ToString("yyyy-MM-dd HH:mm:ss");

                var sql = $@"SELECT
                            	CL.ParHeaderField_Id
                               ,CL.ParHeaderField_Value as Value
                               ,PHF.ParFieldType_Id
                               ,PHF.Name as ParHeaderField_Name
                               ,CL.Evaluation
                               ,CL.Sample
                               ,CL.Id
                               ,{collectionLevel2.Id} as CollectionLevel2_Id
                            FROM Collection CL
                            INNER JOIN ParHeaderField PHF on CL.ParHeaderField_Id = PHF.Id
                            WHERE 1 =1 AND ParHeaderField_Id IS NOT NULL
                            AND CL.UserSgq_Id = {collectionLevel2.AuditorId}
                            AND cl.Shift_Id = {collectionLevel2.Shift}
                            AND cl.Period_Id = {collectionLevel2.Period}
                            AND CL.ParCargo_Id = {collectionLevel2.ParCargo_Id}
                            AND cl.ParCompany_Id = {collectionLevel2.UnitId}
                            AND cl.ParDepartment_Id = {collectionLevel2.ParDepartment_Id}
                            AND cl.Evaluation = {collectionLevel2.EvaluationNumber}
                            AND cl.Sample = {collectionLevel2.Sample}
                            AND Cl.CollectionDate BETWEEN DATEADD(minute, -5, '{collectionDate}') and DATEADD(minute, 5, '{collectionDate}')";

                headerFields = factory.SearchQuery<CollectionLevel2XParHeaderField>(sql).ToList();
            }

            return headerFields;
        }
    }
}