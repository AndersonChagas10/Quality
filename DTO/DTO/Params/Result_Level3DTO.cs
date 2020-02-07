using AutoMapper;
using Dominio;
using DTO.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTO.Params
{
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

        public string CreateUpdate()
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
                    var filtroParLevel3Value = ParLevel3.ParLevel3Value
                        .Where(r => (r.ParCompany_Id == CollectionLevel2.UnitId || r.ParCompany_Id == null) &&
                        (CollectionLevel2.ParLevel1_Id == r.ParLevel1_Id || r.ParLevel1_Id == null) &&
                        (CollectionLevel2.ParLevel2_Id == r.ParLevel2_Id || r.ParLevel2_Id == null))
                        .OrderByDescending(r => r.ParCompany_Id).ThenBy(r => r.ParLevel1_Id).ThenBy(r => r.ParLevel2_Id)
                        .ToList();

                    try
                    {
                        if (filtroParLevel3Value.FirstOrDefault(r => (r.ParLevel3InputType_Id == 1 || r.ParLevel3InputType_Id == 6)) != null)
                            return "0";
                    }
                    catch (Exception e)
                    {

                        throw new Exception("Erro ao gerar valor na RN 45 para ParLevel3InputType_Id == 1", e);
                    }

                    try
                    {
                        if (filtroParLevel3Value.FirstOrDefault(r => r.ParLevel3InputType_Id == 2) != null)//NUMERO DEFEITOS
                            return Value.ToString();
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Erro ao gerar valor na RN 45 para ParLevel3InputType_Id == 2", e);
                    }

                    try
                    {
                        if (filtroParLevel3Value.FirstOrDefault(r => (r.ParLevel3InputType_Id == 3 || r.ParLevel3InputType_Id == 8 || r.ParLevel3InputType_Id == 7 || r.ParLevel3InputType_Id == 9)) != null)//INTERVALOS
                            return Guard.ConverteValorCalculado(Value).ToString("G29").Replace(",", "."); //010.0000 = 10
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Erro ao gerar valor na RN 45 para ParLevel3InputType_Id == 3", e); throw;
                    }

                    try
                    {
                        if (filtroParLevel3Value.FirstOrDefault(r => r.ParLevel3InputType_Id == 4) != null)//CALCULADO
                            return Guard.ConverteValorCalculado(Value).ToString("G29").Replace(",", "."); //10x104 = 10.0000
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Erro ao gerar valor na RN 45 para ParLevel3InputType_Id == 4", e); throw;
                    }

                    try
                    {
                        if (filtroParLevel3Value.FirstOrDefault(r => (r.ParLevel3InputType_Id == 5 || r.ParLevel3InputType_Id == 6)) != null)//TEXTO
                            return "1";
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Erro ao gerar valor na RN 45 para ParLevel3InputType_Id == 5", e); throw;
                    }
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
                    var filtroParLevel3Value = ParLevel3.ParLevel3Value
                        .Where(r => (r.ParCompany_Id == CollectionLevel2.UnitId || r.ParCompany_Id == null) &&
                        (CollectionLevel2.ParLevel1_Id == r.ParLevel1_Id || r.ParLevel1_Id == null) &&
                        (CollectionLevel2.ParLevel2_Id == r.ParLevel2_Id || r.ParLevel2_Id == null))
                        .OrderByDescending(r => r.ParCompany_Id).ThenBy(r => r.ParLevel1_Id).ThenBy(r => r.ParLevel2_Id)
                        .ToList();

                    if (filtroParLevel3Value.FirstOrDefault(r => (r.ParLevel3InputType_Id == 1 || r.ParLevel3InputType_Id == 6)) != null)//BINARIO
                    {
                        return IsConform.GetValueOrDefault() ? "1" : "0";
                    }
                    else if (filtroParLevel3Value.FirstOrDefault(r => r.ParLevel3InputType_Id == 2) != null)//NUMERO DEFEITOS
                    {

                        var vmax = Convert.ToDecimal(IntervalMax, System.Globalization.CultureInfo.InvariantCulture);
                        var vmin = Convert.ToDecimal(IntervalMin, System.Globalization.CultureInfo.InvariantCulture);
                        var valorDefinido = Guard.ConverteValorCalculado(_Value);
                        var dentroDoRange = (valorDefinido <= vmax && valorDefinido >= vmin);
                        return dentroDoRange ? "1" : "0";
                    }
                    else if (filtroParLevel3Value.FirstOrDefault(r => r.ParLevel3InputType_Id == 4) != null)//CALCULADO
                    {
                        var vmax = Convert.ToDecimal(IntervalMax, System.Globalization.CultureInfo.InvariantCulture);
                        var vmin = Convert.ToDecimal(IntervalMin, System.Globalization.CultureInfo.InvariantCulture);
                        var valorDefinido = Guard.ConverteValorCalculado(_Value);
                        var dentroDoRange = (valorDefinido <= vmax && valorDefinido >= vmin);
                        return dentroDoRange ? "1" : "0";
                    }
                    else if (filtroParLevel3Value.FirstOrDefault(r => (r.ParLevel3InputType_Id == 3 || r.ParLevel3InputType_Id == 9)) != null)//INTERVALOS 
                    {
                        var vmax = Convert.ToDecimal(IntervalMax, System.Globalization.CultureInfo.InvariantCulture);
                        var vmin = Convert.ToDecimal(IntervalMin, System.Globalization.CultureInfo.InvariantCulture);
                        var valorDefinido = Guard.ConverteValorCalculado(_Value);
                        var dentroDoRange = (valorDefinido <= vmax && valorDefinido >= vmin);
                        return dentroDoRange ? "1" : "0";
                    }
                    else if (filtroParLevel3Value.FirstOrDefault(x => x.ParLevel3InputType_Id == 8) != null) //Likert
                    {
                        var vmax = Convert.ToDecimal(IntervalMax, System.Globalization.CultureInfo.InvariantCulture);
                        var vmin = Convert.ToDecimal(IntervalMin, System.Globalization.CultureInfo.InvariantCulture);
                        var valorDefinido = Guard.ConverteValorCalculado(_Value);
                        var dentroDoRange = (valorDefinido <= vmax && valorDefinido >= vmin);
                        return dentroDoRange ? "1" : "0";
                    }
                    else if (filtroParLevel3Value.FirstOrDefault(r => r.ParLevel3InputType_Id == 5) != null)//TEXTO 
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
                    var filtroParLevel3Value = ParLevel3.ParLevel3Value
                        .Where(r => (r.ParCompany_Id == CollectionLevel2.UnitId || r.ParCompany_Id == null) &&
                        (CollectionLevel2.ParLevel1_Id == r.ParLevel1_Id || r.ParLevel1_Id == null) &&
                        (CollectionLevel2.ParLevel2_Id == r.ParLevel2_Id || r.ParLevel2_Id == null))
                        .OrderByDescending(r => r.ParCompany_Id).ThenBy(r => r.ParLevel1_Id).ThenBy(r => r.ParLevel2_Id)
                        .ToList();

                    if (filtroParLevel3Value.FirstOrDefault(r => (r.ParLevel3InputType_Id == 1 || r.ParLevel3InputType_Id == 6)) != null)//é um BINARIO
                    {
                        defects = IsConform.GetValueOrDefault() ? 0M : 1M;
                    }
                    else if (filtroParLevel3Value.FirstOrDefault(r => r.ParLevel3InputType_Id == 2) != null)//Nº DEFEITOS 
                    {
                        return _IsConform.Equals("0") ? 1 : 0;
                    }
                    else if (filtroParLevel3Value.FirstOrDefault(r => (r.ParLevel3InputType_Id == 3 || r.ParLevel3InputType_Id == 9)) != null)//INTERVALOS 
                    {
                        return _IsConform.Equals("0") ? 1 : 0;
                    }
                    else if (filtroParLevel3Value.FirstOrDefault(r => r.ParLevel3InputType_Id == 4) != null)//CALCULADO
                    {
                        return _IsConform.Equals("0") ? 1 : 0;
                    }
                    else if (filtroParLevel3Value.FirstOrDefault(r => r.ParLevel3InputType_Id == 5) != null)//TEXTO 
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
                    var filtroParLevel3Value = ParLevel3.ParLevel3Value
                        .Where(r => (r.ParCompany_Id == CollectionLevel2.UnitId || r.ParCompany_Id == null) &&
                        (CollectionLevel2.ParLevel1_Id == r.ParLevel1_Id || r.ParLevel1_Id == null) &&
                        (CollectionLevel2.ParLevel2_Id == r.ParLevel2_Id || r.ParLevel2_Id == null))
                        .OrderByDescending(r => r.ParCompany_Id).ThenBy(r => r.ParLevel1_Id).ThenBy(r => r.ParLevel2_Id)
                        .ToList();

                    if (filtroParLevel3Value.FirstOrDefault(r => (r.ParLevel3InputType_Id == 1 || r.ParLevel3InputType_Id == 6)) != null)//é um BINARIO
                    {
                        defects = IsConform.GetValueOrDefault() ? 0M : 1M;
                    }
                    else if (filtroParLevel3Value.FirstOrDefault(r => r.ParLevel3InputType_Id == 2) != null)//N° DEFEITOS
                    {
                        defects = Convert.ToDecimal(Value);
                    }
                    else if (filtroParLevel3Value.FirstOrDefault(r => (r.ParLevel3InputType_Id == 3 || r.ParLevel3InputType_Id == 9)) != null)//INTERVALOS 
                    {
                        defects = _Defects;
                    }
                    else if (filtroParLevel3Value.FirstOrDefault(r => r.ParLevel3InputType_Id == 4) != null)//CALCULADO
                    {
                        defects = _Defects;
                    }
                    else if (filtroParLevel3Value.FirstOrDefault(r => r.ParLevel3InputType_Id == 5) != null)//TEXTO
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

        public string mountHtmlIntervalos()
        {
            var naoAvaliado = IsNotEvaluate.GetValueOrDefault() ? "checked='checked'" : "";
            return $@"<div
                            <label for='Conforme: '> { Resources.Resource.max_interval }: </label> <label id='intervalMax'>{ IntervalMax }</label>
                            <br>
                            <label for='Conforme: '> { Resources.Resource.min_interval }: </label> <label id='intervalMin'>{ IntervalMin }</label>
                            <br>
                            <label for='Conforme: '> { Resources.Resource.current_value }: </label> { Value }
                            <br>
                            <label for='Conforme: '> { Resources.Resource.new_value }: </label> &nbsp 
                            <input type='number' id='intervaloValor' class='form-control' value={ Value } />
                            <br>
                            <label for='Conforme: '> { Resources.Resource.unvalued }: </label> &nbsp 
                            <input type='checkbox' id='IsEvaluated' { naoAvaliado } class='.check-box' />
                        </div>";
        }

        public string mountHtmlConform()
        {
            var naoAvaliado = IsNotEvaluate.GetValueOrDefault() ? "checked='checked'" : "";
            var checkedAttr = IsConform.GetValueOrDefault() ? "checked='checked'" : "";
            return "<div>" +
                       "<label for='Conforme: '> " + Resources.Resource.conform2 + ": </label> &nbsp " +
                        "<input class='.check-box' id='conform' name='conform' " + checkedAttr + " type='checkbox' value='true'><input name = 'conform' type='hidden' value='false'>" +
                        "<br>" +
                         "<label for='Conforme: '> " + Resources.Resource.unvalued + ": </label> &nbsp " +
                        "<input type='checkbox' id='IsEvaluated' " + naoAvaliado + " class='.check-box' />" +
                   "</div>"; ;
        }

        public string mountHtmlCalculado()
        {
            var naoAvaliado = IsNotEvaluate.GetValueOrDefault() ? "checked='checked'" : "";

            return "<div>" +
                        "<label for='Conforme: '> " + Resources.Resource.max_interval + ": </label>" + Guard.ConverteValorCalculado(Convert.ToDecimal(IntervalMax)) +
                        "<br>" +
                        "<label for='Conforme: '> " + Resources.Resource.min_interval + ": </label>" + Guard.ConverteValorCalculado(Convert.ToDecimal(IntervalMin)) +
                        "<br>" +
                        "<label for='Conforme: '> " + Resources.Resource.current_value + ": </label>" + Guard.ConverteValorCalculado(Convert.ToDecimal(Value)) +
                        "<br>" +
                        "<label for='Conforme: '> " + Resources.Resource.new_value + ": </label> &nbsp" +
                    "<input type='text' id='decimal' class='decimal' /> ^10x <input type='text' id='precisao' class='decimal' />" +
                    "<br>" +
                       "<label for='Conforme: '> " + Resources.Resource.unvalued + ": </label> &nbsp " +
                         "<input type='checkbox' id='IsEvaluated' " + naoAvaliado + " class='.check-box' />" +
                    "</div>";
        }

        public string mountHtmlNumeroDefeitos()
        {
            var naoAvaliado = IsNotEvaluate.GetValueOrDefault() ? "checked='checked'" : "";

            return "<div>" +
                        "<label for='Conforme: '> " + Resources.Resource.max_interval + ": </label>" + double.Parse(IntervalMax, CultureInfo.InvariantCulture) + //Convert.ToDecimal(IntervalMax) +//+ Guard.ConverteValorCalculado(Convert.ToDecimal(IntervalMax)) +
                        "<br>" +
                        "<label for='Conforme: '> " + Resources.Resource.min_interval + ": </label>" + double.Parse(IntervalMin, CultureInfo.InvariantCulture) + //Convert.ToDecimal(IntervalMin) +//+ Guard.ConverteValorCalculado(Convert.ToDecimal(IntervalMin)) +
                        "<br>" +
                        "<label for='Conforme: '> " + Resources.Resource.current_value + ": </label>" + double.Parse(Value, CultureInfo.InvariantCulture) + //Convert.ToDecimal(Value) +//+ Guard.ConverteValorCalculado(Convert.ToDecimal(Value)) +
                        "<br>" +
                        "<label for='Conforme: '> " + Resources.Resource.new_value + ": </label> &nbsp" +
                    "<input type='text' id='numeroDeDefeitos' class='decimal' />" +
                    "<br>" +
                       "<label for='Conforme: '> " + Resources.Resource.unvalued + ": </label> &nbsp " +
                         "<input type='checkbox' id='IsEvaluated' " + naoAvaliado + " class='.check-box' />" +
                    "</div>";
        }

        public string mountHtmlTexto()
        {
            var naoAvaliado = IsNotEvaluate.GetValueOrDefault() ? "checked='checked'" : "";

            if (ValueText == "undefined")
                ValueText = "";

            return "<div>" +
                        "<label for='Conforme: '> " + Resources.Resource.current_value + ": </label>" + ValueText +
                        "<br>" +
                        "<label for='Conforme: '> " + Resources.Resource.new_value + ": </label> &nbsp " +
                         "<input type='text' id='texto' class='form-control text' value='" + ValueText + "' />" +
                    "</div>";
        }

        public string mountHtmlNotEditable()
        {
            return $@"<div>
                            <div class='alert alert-warning' id='mensagemErro'>O item selecionado possui vínculos de resultado. A edição esta desabilitada.</div>
                          </div>";
        }

        public string showGeneric
        {
            get
            {
                if (ParLevel3.IsNotNull())
                {
                    if (ParLevel3.ParLevel3Value.IsNotNull())
                    {
                        var filtroParLevel3Value = ParLevel3.ParLevel3Value
                            .Where(r => (r.ParCompany_Id == CollectionLevel2.UnitId || r.ParCompany_Id == null) && 
                            (CollectionLevel2.ParLevel1_Id == r.ParLevel1_Id || r.ParLevel1_Id == null) && 
                            (CollectionLevel2.ParLevel2_Id == r.ParLevel2_Id || r.ParLevel2_Id == null))
                            .OrderByDescending(r => r.ParCompany_Id).ThenBy(r => r.ParLevel1_Id).ThenBy(r => r.ParLevel2_Id)
                            .ToList();

                        if (filtroParLevel3Value.FirstOrDefault(r => (r.ParLevel3InputType_Id == 1 || r.ParLevel3InputType_Id == 6)).IsNotNull())
                            return mountHtmlConform();
                        else if (filtroParLevel3Value.FirstOrDefault(r => r.ParLevel3InputType_Id == 2).IsNotNull())
                            return mountHtmlNumeroDefeitos();
                        else if (filtroParLevel3Value.FirstOrDefault(r => (r.ParLevel3InputType_Id == 3 || r.ParLevel3InputType_Id == 8 || r.ParLevel3InputType_Id == 7 || r.ParLevel3InputType_Id == 9)).IsNotNull())
                            return mountHtmlIntervalos();
                        else if (filtroParLevel3Value.FirstOrDefault(r => r.ParLevel3InputType_Id == 4).IsNotNull())
                            return mountHtmlCalculado();
                        else if (filtroParLevel3Value.FirstOrDefault(r => (r.ParLevel3InputType_Id == 5)).IsNotNull())
                            return mountHtmlTexto();
                        else if (filtroParLevel3Value.FirstOrDefault(r => (r.ParLevel3InputType_Id == 10)).IsNotNull())
                            return mountHtmlNotEditable();
                    }
                }

                return string.Empty;
            }
        }

    }
}
