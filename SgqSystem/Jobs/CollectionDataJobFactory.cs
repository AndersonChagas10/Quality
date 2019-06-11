using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quartz;
using System.Threading;
using System.Threading.Tasks;
using DTO;
using SgqSystem.Mail;
using System.Configuration;
using Dominio;
using IntegrationModule;
using SgqSystem.Controllers.Api;
using static SgqSystem.Controllers.Api.SyncServiceApiController;
using SgqSystem.Services;

namespace Jobs
{
    public class CollectionDataJobFactory : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            ExecuteCollectionDataJobFunction(null);
        }

        public static void ExecuteCollectionDataJobFunction(object stateInfo)
        {
            Thread.Sleep(new Random().Next(1000, 2000));
            while (true)
            {
                if (ConfigurationManager.AppSettings["CollectionDataJob"] == "on")
                {
                    CollectionDataJobFactory.Execute();
                }

                Thread.Sleep(new Random().Next(50000, 80000));
            }
        }

        private static void Execute()
        {
            try
            {
                using (var db = new SgqDbDevEntities())
                {
                    
                    var integCollectionData = db.IntegCollectionData.Where(x => !(x.Coletado > 0)).Take(50).ToList();

                    foreach (var item in integCollectionData)
                    {
                        Task.Run(() =>
                            CollectionDataJobFactory.Collect(item)
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                new CreateLog(new Exception("Erro no metodo [IntegrationJob]", ex));
            }
        }

        public static void Collect(IntegCollectionData integCollectionData)
        {
            var coleta = new Coleta()
            {
                ParLevel1_Id = integCollectionData.ParLevel1_id.ToString(),
                ParLevel2_Id = integCollectionData.ParLevel2_id.ToString(),
                ParCluster_Id = integCollectionData.ParCluster_id.ToString(),
                UnidadeId = integCollectionData.ParCompany_id.ToString(),
                Weidefects = Convert.ToInt32(integCollectionData.WeiDefects).ToString(),
                Weievaluation = Convert.ToInt32(integCollectionData.WeiEvaluation).ToString(),
                ColetaTarefa = new List<ColetaTarefa>()
                {
                    new ColetaTarefa()
                    {
                        Level03Id = integCollectionData.ParLevel3_id.ToString(),
                        ValueConform = Convert.ToInt32(integCollectionData.Value).ToString(),
                        ValueText = integCollectionData.ValueText.ToString(),
                        IntervalMin = Convert.ToInt32(integCollectionData.MinInterval).ToString(),
                        IntervalMax = Convert.ToInt32(integCollectionData.MaxInterval).ToString(),
                        Conform = integCollectionData.IsConform.ToString(),
                        IsnotEvaluate = integCollectionData.IsNotEvaluate.ToString(),
                        HasPhoto = "0",
                        CollectionDate = integCollectionData.CollectionDate.ToString("yyyy-MM-dd HH:mm:ss"),
                        Defects = Convert.ToInt32(integCollectionData.Defects).ToString(),
                        WeiDefects = Convert.ToInt32(integCollectionData.WeiDefects).ToString(),
                        WeiEvaluation = Convert.ToInt32(integCollectionData.WeiEvaluation).ToString(),
                        Weight = Convert.ToInt32(integCollectionData.Weight).ToString(),
                    }
                },
                Level01DataCollect = integCollectionData.CollectionDate.ToString("yyyy-MM-dd HH:mm:ss"),
                Level02DataCollect = integCollectionData.CollectionDate.ToString("yyyy-MM-dd HH:mm:ss"),
                Evaluate = Convert.ToInt32(integCollectionData.Evaluation).ToString(),
                Sample = Convert.ToInt32(integCollectionData.Sample).ToString(),
                Defects = Convert.ToInt32(integCollectionData.Defects).ToString(),
                Cluster = integCollectionData.ParCluster_id.ToString(),
                Defectsresult = Convert.ToInt32(integCollectionData.Defects).ToString(),
                Shift = "1",
                VersaoApp = "Integração " + integCollectionData.Table_Id.ToString(),
                HashKey = integCollectionData.Key_Integ.ToString(),
            };

            try
            {
                var x = new SyncServicesOld().InsertJson(
                    coleta.ToString(),
                    "",
                    "",
                    false
                );

                if (x == null)
                {
                    using (var db = new SgqDbDevEntities())
                    {
                        integCollectionData.Coletado = 1;
                        db.Entry(integCollectionData).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                else
                {
                    using (var db = new SgqDbDevEntities())
                    {
                        integCollectionData.Coletado = 3;
                        db.Entry(integCollectionData).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                }

            }catch(Exception ex)
            {
                using (var db = new SgqDbDevEntities())
                {
                    integCollectionData.Coletado = 2;
                    db.Entry(integCollectionData).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }

        }
    }
}