using ADOFactory;
using AutoMapper;
using Dominio;
using DTO;
using DTO.DTO.Params;
using DTO.Helpers;
using DTO.ResultSet;
using Newtonsoft.Json;
using SgqSystem.Handlres;
using SgqSystem.Helpers;
using SgqSystem.Services;
using SgqSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
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
        private List<ApontamentosDiariosDomingoResultSet> _listApontomentosDiarioDomingo { get; set; }
        private SgqDbDevEntities db = new SgqDbDevEntities();

        [HttpPost]
        [Route("Get")]
        public List<ApontamentosDiariosResultSet> GetApontamentosDiarios([FromBody] FormularioParaRelatorioViewModel form)
        {

            CommonLog.SaveReport(form, "Report_Apontamentos_Diarios");

            var query = new ApontamentosDiariosResultSet().Select(form);

            using (Factory factory = new Factory("DefaultConnection"))
            {
                _list = factory.SearchQuery<ApontamentosDiariosResultSet>(query).ToList();

                return _list;
            }
        }

        [HttpPost]
        [Route("GetApontamentosDomingo")]
        public List<ApontamentosDiariosDomingoResultSet> GetApontamentosDomingo([FromBody] FormularioParaRelatorioViewModel form)
        {

            CommonLog.SaveReport(form, "Report_Apontamentos_Diarios");

            var query = new ApontamentosDiariosDomingoResultSet().Select(form);

            using (Factory factory = new Factory("DefaultConnection"))
            {
                _listApontomentosDiarioDomingo = factory.SearchQuery<ApontamentosDiariosDomingoResultSet>(query).ToList();
            }
            return _listApontomentosDiarioDomingo;
        }

        [HttpGet]
        [Route("PhotoPreview/{ResultLevel3Id}")]
        public String GetPhotoPreview(int ResultLevel3Id)
        {
            var result = db.Result_Level3_Photos.FirstOrDefault(r => r.Result_Level3_Id == ResultLevel3Id);
            if (result != null)
                return result.Photo_Thumbnaills;
            return null;
        }

        [HttpGet]
        [Route("Photos/{ResultLevel3Id}")]
        public List<Result_Level3_Photos> GetPhotos(int ResultLevel3Id)
        {
            return db.Result_Level3_Photos.Where(r => r.Result_Level3_Id == ResultLevel3Id).ToList();
        }

        [HttpPost]
        [Route("Edit/{id}")]
        public Result_Level3DTO EditResultLevel3(int id)
        {
            var parLevel3Value = new ParLevel3Value();
            bool possuiVinculosResultado = false;
            using (var databaseSgq = new SgqDbDevEntities())
            {
                //select* from Result_Level3 rl3
                //left join ParLevel3Value pl3v on rl3.ParLevel3_Id = pl3v.ParLevel3_Id
                //where rl3.CollectionLevel2_Id = 31567 and pl3v.ParLevel3InputType_Id = 10
                //and pl3v.DynamicValue like('%1360%')

                var resultlevel3 = databaseSgq.Result_Level3.Where(x => x.Id == id).FirstOrDefault();

                var resultlevel3Final = databaseSgq.Result_Level3
                   .Join(databaseSgq.ParLevel3Value, pl3v => pl3v.ParLevel3_Id, rl3 => rl3.ParLevel3_Id, (rl3, pl3v) => new { rl3, pl3v })
                   .Where(x => x.rl3.CollectionLevel2_Id == resultlevel3.CollectionLevel2_Id
                   && x.pl3v.ParLevel3InputType_Id == 10 
                   && (x.pl3v.DynamicValue.Contains("{" + resultlevel3.ParLevel3_Id.ToString() + "}")
                   || x.pl3v.DynamicValue.Contains("{" + resultlevel3.ParLevel3_Id.ToString() + "?}"))
                   && x.pl3v.IsActive).ToList();

                if (resultlevel3Final.Count > 0)
                    possuiVinculosResultado = true;
            }
            if (!possuiVinculosResultado)
            {
                var retorno = Mapper.Map<Result_Level3DTO>(Result_Level3DTO.GetById(id));
                retorno.IntervalMin = retorno.IntervalMin == "-9999999999999.9000000000" ? "Sem limite mínimo" : retorno.IntervalMin;
                retorno.IntervalMax = retorno.IntervalMax == "9999999999999.9000000000" ? "Sem limite Máximo" : retorno.IntervalMax;
                return retorno;
            }
            else
            {
                return null;
            }
        }

        [HttpPost]
        [Route("Save")]
        public Result_Level3DTO SaveResultLevel3([FromBody] Result_Level3DTO resultLevel3)
        {
            var parLevel3Value = new ParLevel3Value();
            using (var databaseSgq = new SgqDbDevEntities())
            {
                var resultlevel3 = databaseSgq.Result_Level3.Where(x => x.Id == resultLevel3.Id).FirstOrDefault();
                var parLevel3 = databaseSgq.ParLevel3.Where(x => x.Id == resultlevel3.ParLevel3_Id).FirstOrDefault();
                parLevel3Value = databaseSgq.ParLevel3Value.Where(x => x.ParLevel3_Id == parLevel3.Id).FirstOrDefault();
                var parInputTypeValues = databaseSgq.ParInputTypeValues.Where(x => x.ParLevel3Value_Id == parLevel3Value.Id && resultLevel3.Value == x.Intervalo.ToString()).FirstOrDefault();
                if(parLevel3Value.ParLevel3InputType_Id == 8)
                    resultLevel3.ValueText = parInputTypeValues.Valor.ToString();

            }
            var query = resultLevel3.CreateUpdate();
            try
            {
                db.Database.ExecuteSqlCommand(query);
                var level3Result = db.Result_Level3.FirstOrDefault(r => r.Id == resultLevel3.Id);

                ConsolidacaoEdicao(resultLevel3.Id);
                return Mapper.Map<Result_Level3DTO>(Result_Level3DTO.GetById(resultLevel3.Id));
            }
            catch (System.Exception e)
            {
                throw e;
            }


        }

        [HttpPost]
        [Route("GetRL/{level1}/{shift}/{period}/{date}")]
        public List<CollectionLevel2> GetResultLevel3(int level1, int shift, int period, DateTime date)
        {
            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;


            var retorno = db.CollectionLevel2.Where(r => r.ParLevel1_Id == level1).Include("Result_Level3").Include("Result_Level3.ParlLevel3").ToList();
            return retorno;

        }

        public void ConsolidacaoEdicao(int id)
        {
            var level3 = db.Result_Level3.Include("CollectionLevel2").FirstOrDefault(r => r.Id == id);

            var data = level3.CollectionLevel2.CollectionDate;
            var company_Id = level3.CollectionLevel2.UnitId;
            var level1_Id = level3.CollectionLevel2.ParLevel1_Id;

            var service = new SyncServices();

            service.ReconsolidationLevel3ByCollectionLevel2Id(level3.CollectionLevel2_Id.ToString());

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

                if (string.IsNullOrEmpty(Value) || Value.Substring(0, 1) == "x")
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
                            if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == CollectionLevel2.UnitId && (r.ParLevel3InputType_Id == 1 || r.ParLevel3InputType_Id == 6)) != null)
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
                            if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == CollectionLevel2.UnitId && (r.ParLevel3InputType_Id == 3 || r.ParLevel3InputType_Id == 8 || r.ParLevel3InputType_Id == 7 || r.ParLevel3InputType_Id == 9)) != null)//INTERVALOS
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
                            if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == CollectionLevel2.UnitId && (r.ParLevel3InputType_Id == 5 || r.ParLevel3InputType_Id == 6)) != null)//TEXTO
                                return "1";
                        }
                        catch (Exception e)
                        {
                            throw new Exception("Erro ao gerar valor na RN 45 para ParLevel3InputType_Id == 5", e); throw;
                        }

                        //Verifica Todos

                        try
                        {
                            if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == null && (r.ParLevel3InputType_Id == 1 || r.ParLevel3InputType_Id == 6)) != null)
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
                            if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == null && (r.ParLevel3InputType_Id == 3 || r.ParLevel3InputType_Id == 8 || r.ParLevel3InputType_Id == 7 || r.ParLevel3InputType_Id == 9)) != null)//INTERVALOS
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
                            if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == null && (r.ParLevel3InputType_Id == 5 || r.ParLevel3InputType_Id == 6)) != null)//TEXTO
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

                        if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == CollectionLevel2.UnitId && (r.ParLevel3InputType_Id == 1 || r.ParLevel3InputType_Id == 6)) != null)//BINARIO
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
                        else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == CollectionLevel2.UnitId && (r.ParLevel3InputType_Id == 3 || r.ParLevel3InputType_Id == 9)) != null)//INTERVALOS 
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
                        else if (ParLevel3.ParLevel3Value.FirstOrDefault(x => x.ParLevel3InputType_Id == 8) != null) //Likert
                        {
                            var vmax = Convert.ToDecimal(IntervalMax, System.Globalization.CultureInfo.InvariantCulture);
                            var vmin = Convert.ToDecimal(IntervalMin, System.Globalization.CultureInfo.InvariantCulture);
                            var valorDefinido = Guard.ConverteValorCalculado(_Value);
                            var dentroDoRange = (valorDefinido <= vmax && valorDefinido >= vmin);
                            return dentroDoRange ? "1" : "0";
                        }

                        //Verifica "Todos"
                        if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == null && (r.ParLevel3InputType_Id == 1 || r.ParLevel3InputType_Id == 6)) != null)//BINARIO
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
                        else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == null && (r.ParLevel3InputType_Id == 3 || r.ParLevel3InputType_Id == 9)) != null)//INTERVALOS 
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
                        if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == CollectionLevel2.UnitId && (r.ParLevel3InputType_Id == 1 || r.ParLevel3InputType_Id == 6)) != null)//é um BINARIO
                        {
                            defects = IsConform.GetValueOrDefault() ? 0M : 1M;
                        }
                        else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == CollectionLevel2.UnitId && r.ParLevel3InputType_Id == 2) != null)//Nº DEFEITOS 
                        {
                            return _IsConform.Equals("0") ? 1 : 0;
                        }
                        else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == CollectionLevel2.UnitId && (r.ParLevel3InputType_Id == 3 || r.ParLevel3InputType_Id == 9)) != null)//INTERVALOS 
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
                        else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == null && (r.ParLevel3InputType_Id == 1 || r.ParLevel3InputType_Id == 6)) != null)//é um BINARIO
                        {
                            defects = IsConform.GetValueOrDefault() ? 0M : 1M;
                        }
                        else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == null && r.ParLevel3InputType_Id == 2) != null)//Nº DEFEITOS 
                        {
                            return _IsConform.Equals("0") ? 1 : 0;
                        }
                        else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == null && (r.ParLevel3InputType_Id == 3 || r.ParLevel3InputType_Id == 9)) != null)//INTERVALOS 
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
                        if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == CollectionLevel2.UnitId && (r.ParLevel3InputType_Id == 1 || r.ParLevel3InputType_Id == 6)) != null)//é um BINARIO
                        {
                            defects = IsConform.GetValueOrDefault() ? 0M : 1M;
                        }
                        else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == CollectionLevel2.UnitId && r.ParLevel3InputType_Id == 2) != null)//N° DEFEITOS
                        {
                            defects = Convert.ToDecimal(Value);
                        }
                        else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == CollectionLevel2.UnitId && (r.ParLevel3InputType_Id == 3 || r.ParLevel3InputType_Id == 9)) != null)//INTERVALOS 
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

                        else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == null && (r.ParLevel3InputType_Id == 1 || r.ParLevel3InputType_Id == 6)) != null)//é um BINARIO
                        {
                            defects = IsConform.GetValueOrDefault() ? 0M : 1M;
                        }
                        else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == null && r.ParLevel3InputType_Id == 2) != null)//N° DEFEITOS
                        {
                            defects = Convert.ToDecimal(Value);
                        }
                        else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == null && (r.ParLevel3InputType_Id == 3 || r.ParLevel3InputType_Id == 9)) != null)//INTERVALOS 
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

                    if (IsConform.GetValueOrDefault())
                    {
                        return "0";
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
                            "<label  for='Conforme: '> " + GetResources.getResource("max_interval").Value.ToString() + ": </label>" + "<label id='intervalMax'>" + IntervalMax + "</label>" +
                            "<br>" +
                            "<label for='Conforme: '> " + GetResources.getResource("min_interval").Value.ToString() + ": </label>" + "<label id='intervalMin'>" + IntervalMin + "</label>" +
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
                           "<label for='Conforme: '> " + GetResources.getResource("conform2").Value.ToString() + ": </label> &nbsp " +
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
                                if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == CollectionLevel2.UnitId && (r.ParLevel3InputType_Id == 1 || r.ParLevel3InputType_Id == 6)).IsNotNull())
                                    return mountHtmlConform();
                                else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == CollectionLevel2.UnitId && r.ParLevel3InputType_Id == 2).IsNotNull())
                                    return mountHtmlNumeroDefeitos();
                                else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == CollectionLevel2.UnitId && (r.ParLevel3InputType_Id == 3 || r.ParLevel3InputType_Id == 8 || r.ParLevel3InputType_Id == 7 || r.ParLevel3InputType_Id == 9)).IsNotNull())
                                    return mountHtmlIntervalos();
                                else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == CollectionLevel2.UnitId && r.ParLevel3InputType_Id == 4).IsNotNull())
                                    return mountHtmlCalculado();
                                else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == CollectionLevel2.UnitId && (r.ParLevel3InputType_Id == 5)).IsNotNull())
                                    return mountHtmlTexto();
                            }
                            else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == null && (r.ParLevel3InputType_Id == 1 || r.ParLevel3InputType_Id == 6)).IsNotNull())
                                return mountHtmlConform();
                            else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == null && r.ParLevel3InputType_Id == 2).IsNotNull())
                                return mountHtmlNumeroDefeitos();
                            else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == null && r.ParLevel3InputType_Id == 3 || r.ParLevel3InputType_Id == 8 || r.ParLevel3InputType_Id == 7 || r.ParLevel3InputType_Id == 9).IsNotNull())
                                return mountHtmlIntervalos();
                            else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == null && r.ParLevel3InputType_Id == 4).IsNotNull())
                                return mountHtmlCalculado();
                            else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == null && r.ParLevel3InputType_Id == 5).IsNotNull())
                                return mountHtmlTexto();

                    return string.Empty;
                }
            }

        }

        [HttpPost]
        [Route("EditCabecalho/{ResultLevel3_Id}")]
        public string EditCabecalho(int ResultLevel3_Id)
        {
            var retorno = getSelects(ResultLevel3_Id);

            var json = JsonConvert.SerializeObject(retorno, Formatting.None, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            return json;
        }

        [HttpPost]
        [Route("SaveCabecalho")]
        public bool SaveCabecalho([FromBody] ListCollectionLevel2XParHeaderField Lsc2xhf)
        {
            try
            {
                foreach (var item in Lsc2xhf.HeaderField)
                {
                    if (item.Id > 0)//Update
                    {
                        var original = db.CollectionLevel2XParHeaderField.FirstOrDefault(c => c.Id == item.Id);

                        if (string.IsNullOrEmpty(item.Value))//Remover
                        {
                            db.CollectionLevel2XParHeaderField.Remove(original);
                        }
                        else //Update
                        {
                            original.Value = item.Value;
                            original.ParHeaderField_Id = item.ParHeaderField_Id;
                            original.ParHeaderField_Name = item.ParHeaderField_Name;
                        }
                    }
                    else if (!string.IsNullOrEmpty(item.Value)) //Add
                    {
                        db.CollectionLevel2XParHeaderField.Add(item);
                    }
                }

                db.SaveChanges();

                //Reconsolida
                if (Lsc2xhf.HeaderField.Count > 0)
                {
                    var syncServices = new SyncServices();

                    syncServices.ReconsolidationToLevel3(Lsc2xhf.HeaderField[0].CollectionLevel2_Id.ToString());
                }

            }
            catch (Exception)
            {
                return false;
                throw;
            }

            return true;
        }

        public class Select
        {
            public ParHeaderField HeaderField { get; set; }
            public List<ParMultipleValues> Values { get; set; }
            public string ValueSelected { get; set; }
            public int CollectionLevel2XParHeaderField_Id { get; set; }
            public CollectionLevel2 CollectionLevel2 { get; set; }
        }

        public class ParLevels
        {
            public int Id { get; set; }
            public int ParLevel1_Id { get; set; }
            public int ParLevel2_Id { get; set; }
            public int EvaluationNumber { get; set; }
            public int Sample { get; set; }
        }

        public List<Select> getSelects(int ResultLevel3_Id)
        {
            //pegar os cabeçalhos
            var resultHeaderField = new List<Select>();
            var collectionLevel2 = getCollectionLevel2ByResultLevel3(ResultLevel3_Id);

            var query = $@"SELECT *
            FROM CollectionLevel2XParHeaderField
            WHERE CollectionLevel2_Id IN (SELECT
            		Id
            	FROM CollectionLevel2
            	WHERE id IN (SELECT
            			CollectionLevel2_Id
            		FROM Result_Level3
            		WHERE id = { ResultLevel3_Id }))";

            var coletas = db.Database.SqlQuery<CollectionLevel2XParHeaderField>(query).ToList();

            //Ids dos cabeçalhos de monitoramentos
            var level1HeaderFields_Id = db.ParLevel1XHeaderField.Include("ParHeaderField").Where(r => r.ParLevel1_Id == collectionLevel2.ParLevel1_Id && r.IsActive && r.ParHeaderField.ParLevelDefinition_Id == 1).Select(r => r.ParHeaderField_Id).ToList();

            //Ids dos cabeçalhos que não fazem parte do Monitoramento
            var headerFields_IdNot = db.ParLevel2XHeaderField.Where(r => r.ParLevel1_Id == collectionLevel2.ParLevel1_Id && r.ParLevel2_Id == collectionLevel2.ParLevel2_Id && r.IsActive).Select(r => r.ParHeaderField_Id).Except(level1HeaderFields_Id).ToList();

            //Ids dos cabeçalhos válidos
            var headerFields_Ids = db.ParLevel1XHeaderField.Where(r => r.ParLevel1_Id == collectionLevel2.ParLevel1_Id && !headerFields_IdNot.Contains(r.ParHeaderField_Id) && r.IsActive).Select(r => r.ParHeaderField_Id).ToList();

            //Seleciona os cabeçalhos
            var headerFields = db.ParHeaderField.Where(r => headerFields_Ids.Contains(r.Id)).OrderBy(r => r.ParLevelDefinition_Id).ThenBy(r => r.Id).ToList();

            var values = db.ParMultipleValues.ToList();

            foreach (var headerField in headerFields)
            {
                var select = new Select();

                //Atribui o campo de cabeçalho
                select.HeaderField = headerField;

                if (headerField.ParFieldType_Id == 2) //Se for campo integração
                {
                    /* Se não for produto que digito o código e busco em uma lista*/
                    if (headerField.Description != "Produto")
                    {
                        SGQDBContext.ParFieldType ParFieldTypeDB = new SGQDBContext.ParFieldType();

                        select.Values = ParFieldTypeDB.getIntegrationValues(headerField.Id, headerField.Description, collectionLevel2.UnitId).ToList();

                    }
                }
                else
                {
                    //pegar values dos campos de cabeçalho
                    select.Values = values.Where(r => r.ParHeaderField_Id == headerField.Id).ToList();
                }

                //Atribui o selecionado
                //Se tiver mais do que um valor duplica a inserção
                var resultados = coletas.Where(r => r.ParHeaderField_Id == headerField.Id).ToList();

                select.CollectionLevel2 = collectionLevel2;

                //Quantidades de campos coletados
                if (resultados.Count > 0)
                {
                    foreach (var resultado in resultados)
                    {
                        //Atribui a quantidade de cabeçalhos                       
                        select.CollectionLevel2XParHeaderField_Id = resultado.Id;
                        select.ValueSelected = resultado.Value;
                        resultHeaderField.Add(new Select()
                        {
                            CollectionLevel2 = select.CollectionLevel2,
                            CollectionLevel2XParHeaderField_Id = select.CollectionLevel2XParHeaderField_Id,
                            HeaderField = select.HeaderField,
                            Values = select.Values,
                            ValueSelected =
                            select.ValueSelected
                        });
                    }
                }
                else
                {
                    //Somente atribui sem valor selecionado
                    resultHeaderField.Add(select);
                }
            }

            //pegar os valor que está selecionado
            return resultHeaderField;
        }

        public CollectionLevel2 getCollectionLevel2ByResultLevel3(int ResultLevel3_Id)
        {
            var query = $@"SELECT
                        *
                    FROM CollectionLevel2
                    WHERE id = (SELECT
                    		CollectionLevel2_Id
                    	FROM Result_Level3
                    	WHERE id = {ResultLevel3_Id})";

            return db.Database.SqlQuery<CollectionLevel2>(query).FirstOrDefault();
        }

        public class ListCollectionLevel2XParHeaderField
        {
            public List<CollectionLevel2XParHeaderField> HeaderField { get; set; }
        }
    }
}