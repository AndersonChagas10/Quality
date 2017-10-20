using AutoMapper;
using Dominio;
using DTO;
using DTO.DTO.Params;
using DTO.Helpers;
using Newtonsoft.Json;
using SgqSystem.Handlres;
using SgqSystem.Helpers;
using SgqSystem.Services;
using SgqSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SgqSystem.Controllers.Api
{

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/ApontamentosDiarios")]
    public class ApontamentosDiariosApiController : ApiController
    {
        public ApontamentosDiariosApiController()
        {
            db.Configuration.LazyLoadingEnabled = false;

            //if (GlobalConfig.Brasil)
            //{
            //    Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-br");
            //    Thread.CurrentThread.CurrentUICulture = new CultureInfo("pt-br");
            //}
            //else
            //{
            //    Thread.CurrentThread.CurrentCulture = new CultureInfo("");
            //    Thread.CurrentThread.CurrentUICulture = new CultureInfo("");
            //}
        }

        private List<ApontamentosDiariosResultSet> _mock { get; set; }
        private List<ApontamentosDiariosResultSet> _list { get; set; }
        private SgqDbDevEntities db = new SgqDbDevEntities();

        [HttpPost]
        [Route("Get")]
        public List<ApontamentosDiariosResultSet> GetApontamentosDiarios([FromBody] FormularioParaRelatorioViewModel form)
        {

            CommonLog.SaveReport(form, "Report_Apontamentos_Diarios");

            var query = new ApontamentosDiariosResultSet().Select(form);
            _list = db.Database.SqlQuery<ApontamentosDiariosResultSet>(query).ToList();
            return _list;
        }

        [HttpGet]
        [Route("PhotoPreview/{ResultLevel3Id}")]
        public String GetPhotoPreview(int ResultLevel3Id)
        {
            var result = db.Result_Level3_Photos.FirstOrDefault(r => r.Result_Level3_Id == ResultLevel3Id);
            if(result != null)
                return result.Photo_Thumbnaills;
            return null;
        }

        [HttpGet]
        [Route("Photo/{ResultLevel3Id}")]
        public Result_Level3_Photos GetPhoto(int ResultLevel3Id)
        {
            return db.Result_Level3_Photos.FirstOrDefault(r => r.Result_Level3_Id == ResultLevel3Id);
        }

        [HttpPost]
        [Route("Edit/{id}")]
        public Result_Level3DTO EditResultLevel3(int id)
        {
            var retorno = Mapper.Map<Result_Level3DTO>(Result_Level3DTO.GetById(id));
            return retorno;
        }

        [HttpPost]
        [Route("Save")]
        public Result_Level3DTO SaveResultLevel3([FromBody] Result_Level3DTO resultLevel3)
        {

            var query = resultLevel3.CreateUpdate();

            try
            {
                db.Database.ExecuteSqlCommand(query);
                var level3Result = db.Result_Level3.FirstOrDefault(r => r.Id == resultLevel3.Id);
                ConsolidacaoEdicao(resultLevel3.Id);
                //db.Database.ExecuteSqlCommand(queryLevel2);
            }
            catch (System.Exception e)
            {
                throw e;
            }

            return Mapper.Map<Result_Level3DTO>(Result_Level3DTO.GetById(resultLevel3.Id));
        }

        [HttpPost]
        [Route("GetRL/{level1}/{shift}/{period}/{date}")]
        public List<CollectionLevel2> GetResultLevel3(int level1, int shift, int period, DateTime date)
        {
            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;


            var retorno = db.CollectionLevel2.Where(r => r.ParLevel1_Id == level1).Include("Result_Level3").Include("Result_Level3.ParlLevel3").ToList();
            return retorno;



            ////var query = "select * from Result_Level3 where CollectionLevel2_Id = "+id+" and IsConform = 0";
            //var list = new List<Result_Level3>();
            //List<CollectionLevel2> collectionL2 = db.CollectionLevel2.Where(r => r.ParLevel1_Id == level1 && r.Shift == shift && r.Period == period && 
            //  DbFunctions.TruncateTime(r.CollectionDate)== date).ToList();

            //foreach (var col in collectionL2)
            //{
            //    var result = db.Result_Level3.Where(r => r.CollectionLevel2_Id == col.Id && r.IsConform == false).ToList();
            //    //var item = Mapper.Map<List<Result_Level3>>(result);
            //    foreach (var res in result) {
            //        var obj = JsonConvert.SerializeObject(res, Formatting.Indented,new JsonSerializerSettings {
            //            ReferenceLoopHandling = ReferenceLoopHandling.Serialize});
            //        list.Add(res);
            //    }
            //}

            ////var lista = Newtonsoft.Json.JsonConvert.SerializeObject(list);

            //return list;
        }

        public void ConsolidacaoEdicao(int id)
        {
            var level3 = db.Result_Level3.Include("CollectionLevel2").FirstOrDefault(r => r.Id == id);

            var data = level3.CollectionLevel2.CollectionDate;
            var company_Id = level3.CollectionLevel2.UnitId;
            var level1_Id = level3.CollectionLevel2.ParLevel1_Id;

            var service = new SyncServices();
            var retorno = service._ReConsolidationByLevel1(company_Id, level1_Id, data);
        }

        public class Result_Level3DTO
        {
            public static Result_Level3 GetById(int id)
            {
                Result_Level3 resultLevel3;
                using (var databaseSgq = new SgqDbDevEntities())
                {
                    databaseSgq.Configuration.LazyLoadingEnabled = false;
                    //db.Configuration.ProxyCreationEnabled = false;
                    resultLevel3 = databaseSgq.Result_Level3.AsNoTracking().FirstOrDefault(r => r.Id == id);
                    resultLevel3.ParLevel3 = databaseSgq.ParLevel3.AsNoTracking().FirstOrDefault(r => r.Id == resultLevel3.ParLevel3_Id);
                    resultLevel3.ParLevel3.ParLevel3Value = databaseSgq.ParLevel3Value.AsNoTracking().Where(r => r.ParLevel3_Id == resultLevel3.ParLevel3_Id && r.IsActive == true).ToList();
                    resultLevel3.CollectionLevel2 = databaseSgq.CollectionLevel2.AsNoTracking().FirstOrDefault(r => r.Id == resultLevel3.CollectionLevel2_Id);
                }

                return resultLevel3;
            }

            private void GetDataToEdit()
            {
                using (var databaseSgq = new SgqDbDevEntities())
                {
                    if (ParLevel3.IsNull())
                    {
                        databaseSgq.Configuration.LazyLoadingEnabled = false;

                        var resultOld = databaseSgq.Result_Level3.FirstOrDefault(r => r.Id == Id);
                        var parL3vel3 = databaseSgq.ParLevel3.AsNoTracking().FirstOrDefault(r => r.Id == resultOld.ParLevel3_Id);
                        var parLevel3Value = databaseSgq.ParLevel3Value.AsNoTracking().Where(r => r.ParLevel3_Id == resultOld.ParLevel3_Id && r.IsActive == true).ToList();
                        CollectionLevel2 = databaseSgq.CollectionLevel2.AsNoTracking().FirstOrDefault(r => r.Id == resultOld.CollectionLevel2_Id);

                        Weight = resultOld.Weight;
                        IntervalMax = resultOld.IntervalMax;
                        IntervalMin = resultOld.IntervalMin;
                        PunishmentValue = resultOld.PunishmentValue;
                        ParLevel3 = Mapper.Map<ParLevel3DTO>(parL3vel3);
                        ParLevel3.ParLevel3Value = Mapper.Map<List<ParLevel3ValueDTO>>(parLevel3Value);
                    }
                }
            }

            internal string CreateUpdate()

            {
                isQueryEdit = true;
                GetDataToEdit();

                if (string.IsNullOrEmpty(Value))
                {
                    using (var databaseSgq = new SgqDbDevEntities())
                    {
                        Value = databaseSgq.Result_Level3.FirstOrDefault(r => r.Id == Id).Value;
                    }
                }

                var texto = "";

                if (!string.IsNullOrEmpty(ValueText))
                {
                    texto += ValueText;
                }


                //bool isBEA = false;
                //string WeiEvaluateBEA = "@WeiEvaluation";
                decimal _WeiEvaluation2 = Decimal.ToInt32(_WeiEvaluation);

                using (var databaseSgq = new SgqDbDevEntities())
                {

                    var result_level3_obj = databaseSgq.Result_Level3.FirstOrDefault(r => r.Id == Id);

                    var collectionLevel2_obj = databaseSgq.CollectionLevel2.FirstOrDefault(r => r.Id == result_level3_obj.CollectionLevel2_Id);

                    var parLeve1BEA = databaseSgq.ParLevel1VariableProductionXLevel1.FirstOrDefault(r => r.ParLevel1_Id == collectionLevel2_obj.ParLevel1_Id);

                    //Se for BEA
                    if (parLeve1BEA != null)
                        if (parLeve1BEA.ParLevel1VariableProduction_Id == 3)
                        {
                            var collectionLevel2_obj2 = databaseSgq.CollectionLevel2.Where(
                            r => DbFunctions.TruncateTime(r.CollectionDate) == DbFunctions.TruncateTime(collectionLevel2_obj.CollectionDate) &&
                            r.ParLevel1_Id == collectionLevel2_obj.ParLevel1_Id &&
                            r.Shift == collectionLevel2_obj.Shift &&
                            r.Period == collectionLevel2_obj.Period &&
                            r.UnitId == collectionLevel2_obj.UnitId &&
                            r.Sample < collectionLevel2_obj.Sample
                            ).OrderByDescending(r => r.Sample).FirstOrDefault();


                            if (collectionLevel2_obj2 != null)

                                _WeiEvaluation2 = collectionLevel2_obj.Sample - collectionLevel2_obj2.Sample;

                            else
                                _WeiEvaluation2 = collectionLevel2_obj.Sample;

                            //isBEA = true;
                            //WeiEvaluateBEA = "Sample";
                        }
                }

                var query = "UPDATE [dbo].[Result_Level3] SET ";
                query += "\n [IsConform] = " + _IsConform + ",";
                query += "\n [Defects] = " + _Defects + ",";
                query += "\n [WeiDefects] = " + _WeiDefects + ",";
                query += "\n [Value] = " + _Value + ",";
                query += "\n [IsNotEvaluate] = " + _IsNotEvaluate + ",";
                query += "\n [ValueText] = '" + texto + "',";
                query += "\n [WeiEvaluation] = " + Decimal.ToInt32(_WeiEvaluation2) + ",";//+ " " + Decimal.ToInt32(_WeiEvaluation) + ",";
                query = query.Remove(query.Length - 1);//Remove a ultima virgula antes do where.
                query += "\n WHERE Id = " + Id;


                query += "                                                                                                                    " +
                "\n DECLARE @ID INT = (SELECT TOP 1 CollectionLevel2_Id FROM Result_Level3 WHERE Id = " + Id + " )                            " +
                "\n DECLARE @Defects DECIMAL(10,3)                                                                                            " +
                "\n DECLARE @DefectsResult DECIMAL(10, 3)                                                                                     " +
                "\n DECLARE @EvatuationResult DECIMAL(10, 3)                                                                                  " +
                "\n DECLARE @WeiEvaluation DECIMAL(10, 3)                                                                                     " +
                "\n DECLARE @WeiDefects DECIMAL(10, 3)                                                                                        " +
                "\n DECLARE @TotalLevel3Evaluation  DECIMAL(10, 3)                                                                            " +
                "\n DECLARE @TotalLevel3WithDefects DECIMAL(10, 3)                                                                            " +
                "\n                                                                                                                           " +
                "\n select                                                                                                                    " +
                "\n                                                                                                                           " +
                "\n @Defects = isnull(sum(r3.Defects),0),                                                                                     " +
                "\n @DefectsResult = case when sum(r3.WeiDefects) > 0 then 1 else 0 end,                                                         " +
                "\n @EvatuationResult = case when sum(r3.Evaluation) > 0 then 1 else 0 end,                                                   " +
                "\n @WeiEvaluation = isnull(sum(r3.WeiEvaluation),0),                                                                        " +
                "\n @WeiDefects = isnull(sum(r3.WeiDefects),0),                                                                               " +
                "\n @TotalLevel3Evaluation = count(1),                                                                                        " +
                "\n @TotalLevel3WithDefects = (select count(1) from result_level3 where collectionLevel2_Id = @ID and Defects > 0  and IsNotEvaluate = 0)         " +
                "\n from result_level3 r3                                                                                                     " +
                "\n where collectionlevel2_id = @ID                                                                                           " +
                "\n and r3.IsNotEvaluate = 0                                                                                                  " +
                "\n                                                                                                                           " +
                "\n                                                                                                                           " +
                "\n UPDATE CollectionLevel2                                                                                                   " +
                "\n SET Defects = @Defects                                                                                                    " +
                "\n , DefectsResult = @DefectsResult                                                                                          " +
                "\n , EvaluatedResult = @EvatuationResult                                                                                     " +
                "\n , WeiEvaluation = @WeiEvaluation                                                                                          " +
                "\n , WeiDefects = @WeiDefects                                                                                                " +
                "\n , TotalLevel3Evaluation = @TotalLevel3Evaluation                                                                          " +
                "\n , TotalLevel3WithDefects = @TotalLevel3WithDefects                                                                        " +
                "\n , AlterDate = GETDATE()                                                                                                   " +
                "\n WHERE Id = @ID                                                                                                            ";

                return query;
            }

            public bool isQueryEdit { get; set; }
            public int Id { get; set; }
            public int CollectionLevel2_Id { get; set; }
            public int ParLevel3_Id { get; set; }
            public string ParLevel3_Name { get; set; }
            public Nullable<decimal> Weight { get; set; }
            public string IntervalMin { get; set; }
            public string IntervalMax { get; set; }

            public string Value { get; set; }
            public string _Value //RN 45
            {
                get
                {
                    if (isQueryEdit)
                    {
                        try
                        {
                            if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == CollectionLevel2.UnitId && r.ParLevel3InputType_Id == 1) != null)
                                return "0";
                        }
                        catch (Exception e)
                        {

                            throw new Exception("Erro ao gerar valor na RN 45 para ParLevel3InputType_Id == 1", e);
                        }

                        try
                        {
                            if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == CollectionLevel2.UnitId && r.ParLevel3InputType_Id == 2) != null)//NUMERO DEFEITOS
                                return Value.ToString();
                        }
                        catch (Exception e)
                        {
                            throw new Exception("Erro ao gerar valor na RN 45 para ParLevel3InputType_Id == 2", e);
                        }

                        try
                        {
                            if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == CollectionLevel2.UnitId && r.ParLevel3InputType_Id == 3) != null)//INTERVALOS
                                return Guard.ConverteValorCalculado(Value).ToString("G29").Replace(",", "."); //010.0000 = 10
                        }
                        catch (Exception e)
                        {
                            throw new Exception("Erro ao gerar valor na RN 45 para ParLevel3InputType_Id == 3", e); throw;
                        }

                        try
                        {
                            if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == CollectionLevel2.UnitId && r.ParLevel3InputType_Id == 4) != null)//CALCULADO
                                return Guard.ConverteValorCalculado(Value).ToString("G29").Replace(",", "."); //10x104 = 10.0000
                        }
                        catch (Exception e)
                        {
                            throw new Exception("Erro ao gerar valor na RN 45 para ParLevel3InputType_Id == 4", e); throw;
                        }

                        try
                        {
                            if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == CollectionLevel2.UnitId && r.ParLevel3InputType_Id == 5) != null)//TEXTO
                                return "1";
                        }
                        catch (Exception e)
                        {
                            throw new Exception("Erro ao gerar valor na RN 45 para ParLevel3InputType_Id == 5", e); throw;
                        }

                        //Verifica Todos

                        try
                        {
                            if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == null && r.ParLevel3InputType_Id == 1) != null)
                                return "0";
                        }
                        catch (Exception e)
                        {

                            throw new Exception("Erro ao gerar valor na RN 45 para ParLevel3InputType_Id == 1", e);
                        }

                        try
                        {
                            if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == null && r.ParLevel3InputType_Id == 2) != null)//NUMERO DEFEITOS
                                return Value.ToString();
                        }
                        catch (Exception e)
                        {
                            throw new Exception("Erro ao gerar valor na RN 45 para ParLevel3InputType_Id == 2", e);
                        }

                        try
                        {
                            if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == null && r.ParLevel3InputType_Id == 3) != null)//INTERVALOS
                                return Guard.ConverteValorCalculado(Value).ToString("G29").Replace(",", "."); //010.0000 = 10
                        }
                        catch (Exception e)
                        {
                            throw new Exception("Erro ao gerar valor na RN 45 para ParLevel3InputType_Id == 3", e); throw;
                        }

                        try
                        {
                            if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == null && r.ParLevel3InputType_Id == 4) != null)//CALCULADO
                                return Guard.ConverteValorCalculado(Value).ToString("G29").Replace(",", "."); //10x104 = 10.0000
                        }
                        catch (Exception e)
                        {
                            throw new Exception("Erro ao gerar valor na RN 45 para ParLevel3InputType_Id == 4", e); throw;
                        }

                        try
                        {
                            if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == null && r.ParLevel3InputType_Id == 5) != null)//TEXTO
                                return "1";
                        }
                        catch (Exception e)
                        {
                            throw new Exception("Erro ao gerar valor na RN 45 para ParLevel3InputType_Id == 5", e); throw;
                        }

                        //try
                        //{
                        //    if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == unit && r.ParLevel3InputType_Id == 1) != null)//BINARIO
                        //        return "0";
                        //}
                        //catch (Exception e)
                        //{
                        //    throw new Exception("Erro ao gerar valor na RN 45 para ParLevel3InputType_Id == 1", e);
                        //}

                        //try
                        //{
                        //    if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == unit && r.ParLevel3InputType_Id == 2) != null)//NUMERO DEFEITOS
                        //        return Value.ToString();
                        //}
                        //catch (Exception e)
                        //{
                        //    throw new Exception("Erro ao gerar valor na RN 45 para ParLevel3InputType_Id == 2", e);
                        //}

                        //try
                        //{
                        //    if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == unit && r.ParLevel3InputType_Id == 3) != null)//INTERVALOS
                        //        return Guard.ConverteValorCalculado(Value).ToString("G29"); //010.0000 = 10
                        //}
                        //catch (Exception e)
                        //{
                        //    throw new Exception("Erro ao gerar valor na RN 45 para ParLevel3InputType_Id == 3", e); throw;
                        //}

                        //try
                        //{
                        //    if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == unit && r.ParLevel3InputType_Id == 4) != null)//CALCULADO
                        //        return Guard.ConverteValorCalculado(Value).ToString("G29"); //10x104 = 10.0000
                        //}
                        //catch (Exception e)
                        //{
                        //    throw new Exception("Erro ao gerar valor na RN 45 para ParLevel3InputType_Id == 4", e); throw;
                        //}

                        //try
                        //{
                        //    if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == unit && r.ParLevel3InputType_Id == 5) != null)//TEXTO
                        //        return "1";
                        //}
                        //catch (Exception e)
                        //{
                        //    throw new Exception("Erro ao gerar valor na RN 45 para ParLevel3InputType_Id == 5", e); throw;
                        //}

                    }

                    return string.Empty;
                }
            }

            public string ValueText { get; set; }

            public Nullable<bool> IsConform { get; set; }
            public string _IsConform //RN 46
            {
                get
                {
                    if (isQueryEdit)
                    {

                        if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == CollectionLevel2.UnitId && r.ParLevel3InputType_Id == 1) != null)//BINARIO
                        {
                            return IsConform.GetValueOrDefault() ? "1" : "0";
                        }
                        else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == CollectionLevel2.UnitId && r.ParLevel3InputType_Id == 2) != null)//NUMERO DEFEITOS
                        {

                            var vmax = Convert.ToDecimal(IntervalMax, System.Globalization.CultureInfo.InvariantCulture);
                            var vmin = Convert.ToDecimal(IntervalMin, System.Globalization.CultureInfo.InvariantCulture);
                            var valorDefinido = Guard.ConverteValorCalculado(_Value);
                            var dentroDoRange = (valorDefinido <= vmax && valorDefinido >= vmin);
                            return dentroDoRange ? "1" : "0";
                        }
                        else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == CollectionLevel2.UnitId && r.ParLevel3InputType_Id == 4) != null)//CALCULADO
                        {

                            var vmax = Convert.ToDecimal(IntervalMax, System.Globalization.CultureInfo.InvariantCulture);
                            var vmin = Convert.ToDecimal(IntervalMin, System.Globalization.CultureInfo.InvariantCulture);
                            var valorDefinido = Guard.ConverteValorCalculado(_Value);
                            var dentroDoRange = (valorDefinido <= vmax && valorDefinido >= vmin);
                            return dentroDoRange ? "1" : "0";
                        }
                        else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == CollectionLevel2.UnitId && r.ParLevel3InputType_Id == 3) != null)//INTERVALOS 
                        {
                            var vmax = Convert.ToDecimal(IntervalMax, System.Globalization.CultureInfo.InvariantCulture);
                            var vmin = Convert.ToDecimal(IntervalMin, System.Globalization.CultureInfo.InvariantCulture);
                            var valorDefinido = Guard.ConverteValorCalculado(_Value);
                            var dentroDoRange = (valorDefinido <= vmax && valorDefinido >= vmin);
                            return dentroDoRange ? "1" : "0";
                        }
                        else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == CollectionLevel2.UnitId && r.ParLevel3InputType_Id == 5) != null)//TEXTO 
                        {
                            var valueText = string.IsNullOrEmpty(ValueText);
                            return valueText ? "1" : "0";
                        }

                        //Verifica "Todos"
                        if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == null && r.ParLevel3InputType_Id == 1) != null)//BINARIO
                        {
                            return IsConform.GetValueOrDefault() ? "1" : "0";
                        }
                        else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == null && r.ParLevel3InputType_Id == 2) != null)//NUMERO DEFEITOS
                        {

                            var vmax = Convert.ToDecimal(IntervalMax, System.Globalization.CultureInfo.InvariantCulture);
                            var vmin = Convert.ToDecimal(IntervalMin, System.Globalization.CultureInfo.InvariantCulture);
                            var valorDefinido = Guard.ConverteValorCalculado(_Value);
                            var dentroDoRange = (valorDefinido <= vmax && valorDefinido >= vmin);
                            return dentroDoRange ? "1" : "0";
                        }
                        else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == null && r.ParLevel3InputType_Id == 4) != null)//CALCULADO
                        {

                            var vmax = Convert.ToDecimal(IntervalMax, System.Globalization.CultureInfo.InvariantCulture);
                            var vmin = Convert.ToDecimal(IntervalMin, System.Globalization.CultureInfo.InvariantCulture);
                            var valorDefinido = Guard.ConverteValorCalculado(_Value);
                            var dentroDoRange = (valorDefinido <= vmax && valorDefinido >= vmin);
                            return dentroDoRange ? "1" : "0";
                        }
                        else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == null && r.ParLevel3InputType_Id == 3) != null)//INTERVALOS 
                        {
                            var vmax = Convert.ToDecimal(IntervalMax, System.Globalization.CultureInfo.InvariantCulture);
                            var vmin = Convert.ToDecimal(IntervalMin, System.Globalization.CultureInfo.InvariantCulture);
                            var valorDefinido = Guard.ConverteValorCalculado(_Value);
                            var dentroDoRange = (valorDefinido <= vmax && valorDefinido >= vmin);
                            return dentroDoRange ? "1" : "0";
                        }
                        else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == null && r.ParLevel3InputType_Id == 5) != null)//TEXTO 
                        {
                            var valueText = string.IsNullOrEmpty(ValueText);
                            return valueText ? "1" : "0";
                        }
                    }

                    return string.Empty;

                }
            }

            public Nullable<bool> IsNotEvaluate { get; set; }
            public int _IsNotEvaluate { get { return !IsNotEvaluate.GetValueOrDefault() ? 0 : 1; } }

            public Nullable<decimal> Defects { get; set; }
            public decimal _Defects
            {
                get
                {
                    var defects = 0M;
                    if (isQueryEdit)
                    {
                        if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == CollectionLevel2.UnitId && r.ParLevel3InputType_Id == 1) != null)//é um BINARIO
                        {
                            defects = IsConform.GetValueOrDefault() ? 0M : 1M;
                        }
                        else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == CollectionLevel2.UnitId && r.ParLevel3InputType_Id == 2) != null)//Nº DEFEITOS 
                        {
                            return _IsConform.Equals("0") ? 1 : 0;
                        }
                        else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == CollectionLevel2.UnitId && r.ParLevel3InputType_Id == 3) != null)//INTERVALOS 
                        {
                            return _IsConform.Equals("0") ? 1 : 0;
                        }
                        else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == CollectionLevel2.UnitId && r.ParLevel3InputType_Id == 4) != null)//CALCULADO
                        {
                            return _IsConform.Equals("0") ? 1 : 0;
                        }
                        else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == CollectionLevel2.UnitId && r.ParLevel3InputType_Id == 5) != null)//TEXTO 
                        {
                            return _IsConform.Equals("0") ? 1 : 0;
                        }

                        //Todos
                        else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == null && r.ParLevel3InputType_Id == 1) != null)//é um BINARIO
                        {
                            defects = IsConform.GetValueOrDefault() ? 0M : 1M;
                        }
                        else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == null && r.ParLevel3InputType_Id == 2) != null)//Nº DEFEITOS 
                        {
                            return _IsConform.Equals("0") ? 1 : 0;
                        }
                        else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == null && r.ParLevel3InputType_Id == 3) != null)//INTERVALOS 
                        {
                            return _IsConform.Equals("0") ? 1 : 0;
                        }
                        else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == null && r.ParLevel3InputType_Id == 4) != null)//CALCULADO
                        {
                            return _IsConform.Equals("0") ? 1 : 0;
                        }
                        else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == null && r.ParLevel3InputType_Id == 5) != null)//TEXTO 
                        {
                            return _IsConform.Equals("0") ? 1 : 0;
                        }

                    }

                    return defects;

                }
            }

            public Nullable<decimal> PunishmentValue { get; set; }
            public Nullable<decimal> WeiEvaluation { get; set; }
            public decimal _WeiEvaluation
            {
                get
                {

                    if (IsNotEvaluate == true)
                    {
                        return 0;
                    }

                    //Fazer a soma ponderada
                    return Weight.GetValueOrDefault();
                }
            }
            public Nullable<decimal> Evaluation { get; set; }

            public Nullable<decimal> WeiDefects { get; set; }
            public string _WeiDefects
            {
                get
                {
                    var defects = 0M;
                    if (isQueryEdit)
                    {
                        if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == CollectionLevel2.UnitId && r.ParLevel3InputType_Id == 1) != null)//é um BINARIO
                        {
                            defects = IsConform.GetValueOrDefault() ? 0M : 1M;
                        }
                        else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == CollectionLevel2.UnitId && r.ParLevel3InputType_Id == 2) != null)//N° DEFEITOS
                        {
                            defects = Convert.ToDecimal(Value);
                        }
                        else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == CollectionLevel2.UnitId && r.ParLevel3InputType_Id == 3) != null)//INTERVALOS 
                        {
                            defects = _Defects;
                        }
                        else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == CollectionLevel2.UnitId && r.ParLevel3InputType_Id == 4) != null)//CALCULADO
                        {
                            defects = _Defects;
                        }
                        else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == CollectionLevel2.UnitId && r.ParLevel3InputType_Id == 5) != null)//TEXTO
                        {
                            defects = _Defects;
                        }

                        //Se for Nulo

                        else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == null && r.ParLevel3InputType_Id == 1) != null)//é um BINARIO
                        {
                            defects = IsConform.GetValueOrDefault() ? 0M : 1M;
                        }
                        else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == null && r.ParLevel3InputType_Id == 2) != null)//N° DEFEITOS
                        {
                            defects = Convert.ToDecimal(Value);
                        }
                        else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == null && r.ParLevel3InputType_Id == 3) != null)//INTERVALOS 
                        {
                            defects = _Defects;
                        }
                        else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == null && r.ParLevel3InputType_Id == 4) != null)//CALCULADO
                        {
                            defects = _Defects;
                        }
                        else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == null && r.ParLevel3InputType_Id == 5) != null)//TEXTO
                        {
                            defects = _Defects;
                        }

                    }

                    var defeitoXPeso = (defects * Weight.GetValueOrDefault());
                    var punicaoXPeso = (PunishmentValue.GetValueOrDefault() * Weight.GetValueOrDefault());

                    return (defeitoXPeso + punicaoXPeso).ToString("G29");
                }
            }

            public Nullable<decimal> CT4Eva3 { get; set; }
            public Nullable<decimal> Sampling { get; set; }

            public ParLevel3DTO ParLevel3 { get; set; }
            public CollectionLevel2 CollectionLevel2 { get; set; }

            public int? unit { get; set; }

            //public string _HeaderEdit
            //{
            //    get
            //    {
            //        if()
            //        var level1Name = this.CollectionLevel2.ParLevel1_Id;
            //        var level2Name = this.CollectionLevel2.UnitId;
            //        using 
            //        return ParLevel3_Name
            //    }
            //}

            //public string showIntervalos
            //{
            //    get
            //    {
            //        if (ParLevel3.IsNotNull())
            //            if (ParLevel3.ParLevel3Value.IsNotNull())
            //                if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == CollectionLevel2.UnitId /*unit*/ && r.ParLevel3InputType_Id == 3).IsNotNull())//INTERVALOS ??
            //                {
            //                    return mountHtmlIntervalos();

            //                }
            //        //else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == null && r.ParLevel3InputType_Id == 3).IsNotNull())
            //        //{
            //        //    return mountHtmlIntervalos();
            //        //}

            //        return string.Empty;
            //    }

            //}

            public string mountHtmlIntervalos()
            {
                var naoAvaliado = IsNotEvaluate.GetValueOrDefault() ? "checked='checked'" : "";
                return "<div>" +
                            //"<label for='Conforme: '> Intervalo Max: </label>" + IntervalMax +
                            "<label for='Conforme: '> " + GetResources.getResource("max_interval").Value.ToString() + ": </label>" + IntervalMax +
                            "<br>" +
                            "<label for='Conforme: '> " + GetResources.getResource("min_interval").Value.ToString() + ": </label>" + IntervalMin +
                            "<br>" +
                            "<label for='Conforme: '> " + GetResources.getResource("current_value").Value.ToString() + ": </label>" + Value +
                            "<br>" +
                            "<label for='Conforme: '> " + GetResources.getResource("new_value").Value.ToString() + ": </label> &nbsp " +
                             "<input type='text' id='intervaloValor' class='form-control decimal' value=" + Value + " />" +
                            "<br>" +
                            "<label for='Conforme: '> " + GetResources.getResource("unvalued").Value.ToString() + ": </label> &nbsp " +
                             "<input type='checkbox' id='IsEvaluated' " + naoAvaliado + " class='.check-box' />" +
                        "</div>";
            }

            //public string showConform // BINARIO 
            //{
            //    get
            //    {
            //        if (ParLevel3.IsNotNull())
            //            if (ParLevel3.ParLevel3Value.IsNotNull())

            //                if (ParLevel3.ParLevel3Value.Any(r => r.ParCompany_Id == CollectionLevel2.UnitId))
            //                {
            //                    if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == CollectionLevel2.UnitId && r.ParLevel3InputType_Id == 1).IsNotNull())//é um BINARIO //É Unico
            //                    {
            //                        //if (ParLevel3.ParLevel3Value.Any(r=> r.ParCompany_Id == CollectionLevel2.UnitId)) //Verifica se corporativo ou unidade especifica
            //                        //{

            //                        //}
            //                        return mountHtmlConform();
            //                    }
            //                }
            //                else
            //                {

            //                }


            //        //else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == null && r.ParLevel3InputType_Id == 1).IsNotNull())
            //        //{
            //        //    return mountHtmlConform();
            //        //}

            //        return string.Empty;
            //    }

            //}

            public string mountHtmlConform()
            {
                var naoAvaliado = IsNotEvaluate.GetValueOrDefault() ? "checked='checked'" : "";
                var checkedAttr = IsConform.GetValueOrDefault() ? "checked='checked'" : "";
                return "<div>" +
                           "<label for='Conforme: '> " + GetResources.getResource("conform").Value.ToString() + ": </label>" +
                            "<input class='.check-box' id='conform' name='conform' " + checkedAttr + " type='checkbox' value='true'><input name = 'conform' type='hidden' value='false'>" +
                            "<br>" +
                             "<label for='Conforme: '> " + GetResources.getResource("unvalued").Value.ToString() + ": </label> &nbsp " +
                            "<input type='checkbox' id='IsEvaluated' " + naoAvaliado + " class='.check-box' />" +
                       "</div>"; ;
            }

            //public string showCalculado
            //{
            //    get
            //    {
            //        if (ParLevel3.IsNotNull())
            //            if (ParLevel3.ParLevel3Value.IsNotNull())
            //                if (ParLevel3.ParLevel3Value.FirstOrDefault(r => /*r.ParCompany_Id == CollectionLevel2.UnitId &&*/ r.ParLevel3InputType_Id == 4).IsNotNull())//é um CALCULADO
            //                {
            //                    return mountHtmlCalculado();

            //                }
            //        //else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == null && r.ParLevel3InputType_Id == 4).IsNotNull())
            //        //{
            //        //    return mountHtmlCalculado();
            //        //}

            //        return string.Empty;
            //    }

            //}

            public string mountHtmlCalculado()
            {
                var naoAvaliado = IsNotEvaluate.GetValueOrDefault() ? "checked='checked'" : "";

                return "<div>" +
                            "<label for='Conforme: '> " + GetResources.getResource("max_interval").Value.ToString() + ": </label>" + Guard.ConverteValorCalculado(Convert.ToDecimal(IntervalMax)) +
                            "<br>" +
                            "<label for='Conforme: '> " + GetResources.getResource("min_interval").Value.ToString() + ": </label>" + Guard.ConverteValorCalculado(Convert.ToDecimal(IntervalMin)) +
                            "<br>" +
                            "<label for='Conforme: '> " + GetResources.getResource("current_value").Value.ToString() + ": </label>" + Guard.ConverteValorCalculado(Convert.ToDecimal(Value)) +
                            "<br>" +
                            "<label for='Conforme: '> " + GetResources.getResource("new_value").Value.ToString() + ": </label> &nbsp" +
                        "<input type='text' id='decimal' class='decimal' /> ^10x <input type='text' id='precisao' class='decimal' />" +
                        "<br>" +
                           "<label for='Conforme: '> " + GetResources.getResource("unvalued").Value.ToString() + ": </label> &nbsp " +
                             "<input type='checkbox' id='IsEvaluated' " + naoAvaliado + " class='.check-box' />" +
                        "</div>";
            }

            //public string showNumeroDefeitos
            //{
            //    get
            //    {

            //        if (ParLevel3.IsNotNull())
            //            if (ParLevel3.ParLevel3Value.IsNotNull())
            //                if (ParLevel3.ParLevel3Value.FirstOrDefault(r => /*r.ParCompany_Id == CollectionLevel2.UnitId && */ r.ParLevel3InputType_Id == 2).IsNotNull())//é um Numero de Defeitos
            //                {
            //                    return mountHtmlNumeroDefeitos();
            //                }
            //        //else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == null && r.ParLevel3InputType_Id == 2).IsNotNull())
            //        //{
            //        //    return mountHtmlNumeroDefeitos();
            //        //}

            //        return string.Empty;
            //    }

            //}

            public string mountHtmlNumeroDefeitos()
            {
                var naoAvaliado = IsNotEvaluate.GetValueOrDefault() ? "checked='checked'" : "";

                return "<div>" +
                            "<label for='Conforme: '> " + GetResources.getResource("max_interval").Value.ToString() + ": </label>" + double.Parse(IntervalMax, CultureInfo.InvariantCulture) + //Convert.ToDecimal(IntervalMax) +//+ Guard.ConverteValorCalculado(Convert.ToDecimal(IntervalMax)) +
                            "<br>" +
                            "<label for='Conforme: '> " + GetResources.getResource("min_interval").Value.ToString() + ": </label>" + double.Parse(IntervalMin, CultureInfo.InvariantCulture) + //Convert.ToDecimal(IntervalMin) +//+ Guard.ConverteValorCalculado(Convert.ToDecimal(IntervalMin)) +
                            "<br>" +
                            "<label for='Conforme: '> " + GetResources.getResource("current_value").Value.ToString() + ": </label>" + double.Parse(Value, CultureInfo.InvariantCulture) + //Convert.ToDecimal(Value) +//+ Guard.ConverteValorCalculado(Convert.ToDecimal(Value)) +
                            "<br>" +
                            "<label for='Conforme: '> " + GetResources.getResource("new_value").Value.ToString() + ": </label> &nbsp" +
                        "<input type='text' id='numeroDeDefeitos' class='decimal' />" +
                        "<br>" +
                           "<label for='Conforme: '> " + GetResources.getResource("unvalued").Value.ToString() + ": </label> &nbsp " +
                             "<input type='checkbox' id='IsEvaluated' " + naoAvaliado + " class='.check-box' />" +
                        "</div>";
            }

            //public string showTexto
            //{
            //    //Texto = tipo 5
            //    get
            //    {
            //        if (ParLevel3.IsNotNull())
            //            if (ParLevel3.ParLevel3Value.IsNotNull())
            //                if (ParLevel3.ParLevel3Value.FirstOrDefault(r => /*r.ParCompany_Id == CollectionLevel2.UnitId  &&*/ r.ParLevel3InputType_Id == 5).IsNotNull())//TEXTO ??
            //                {
            //                    return mountHtmlTexto();
            //                }
            //        //else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == null && r.ParLevel3InputType_Id == 5).IsNotNull())
            //        //{
            //        //    return mountHtmlTexto();
            //        //}

            //        return string.Empty;
            //    }

            //}

            public string mountHtmlTexto()
            {
                var naoAvaliado = IsNotEvaluate.GetValueOrDefault() ? "checked='checked'" : "";

                if (ValueText == "undefined")
                    ValueText = "";

                return "<div>" +
                            //"<label for='Conforme: '> Intervalo Max: </label>" + IntervalMax +
                            //"<label for='Conforme: '> " + Resources.Resource.max_interval + ": </label>" + IntervalMax +
                            //"<br>" +
                            //"<label for='Conforme: '> Intervalo Min: </label>" + IntervalMin +
                            //"<br>" +
                            "<label for='Conforme: '> " + GetResources.getResource("current_value").Value.ToString() + ": </label>" + ValueText +
                            "<br>" +
                            "<label for='Conforme: '> " + GetResources.getResource("new_value").Value.ToString() + ": </label> &nbsp " +
                             "<input type='text' id='texto' class='form-control text' value='" + ValueText + "' />" +
                        //"<br>" +
                        //"<label for='Conforme: '> Não Avaliado: </label> &nbsp " +
                        // "<input type='checkbox' id='IsEvaluated' " + naoAvaliado + " class='.check-box' />" +
                        "</div>";
            }

            public string showGeneric
            {
                get
                {
                    if (ParLevel3.IsNotNull())
                        if (ParLevel3.ParLevel3Value.IsNotNull())
                            if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == CollectionLevel2.UnitId).IsNotNull())
                            {
                                if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == CollectionLevel2.UnitId && r.ParLevel3InputType_Id == 1).IsNotNull())
                                    return mountHtmlConform();
                                else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == CollectionLevel2.UnitId && r.ParLevel3InputType_Id == 2).IsNotNull())
                                    return mountHtmlNumeroDefeitos();
                                else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == CollectionLevel2.UnitId && r.ParLevel3InputType_Id == 3).IsNotNull())
                                    return mountHtmlIntervalos();
                                else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == CollectionLevel2.UnitId && r.ParLevel3InputType_Id == 4).IsNotNull())
                                    return mountHtmlCalculado();
                                else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == CollectionLevel2.UnitId && r.ParLevel3InputType_Id == 5).IsNotNull())
                                    return mountHtmlTexto();
                            }
                            else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == null && r.ParLevel3InputType_Id == 1).IsNotNull())
                                return mountHtmlConform();
                            else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == null && r.ParLevel3InputType_Id == 2).IsNotNull())
                                return mountHtmlNumeroDefeitos();
                            else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == null && r.ParLevel3InputType_Id == 3).IsNotNull())
                                return mountHtmlIntervalos();
                            else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == null && r.ParLevel3InputType_Id == 4).IsNotNull())
                                return mountHtmlCalculado();
                            else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == null && r.ParLevel3InputType_Id == 5).IsNotNull())
                                return mountHtmlTexto();

                    return string.Empty;
                }
            }

        }

    }
}