using AutoMapper;
using Dominio;
using DTO.DTO.Params;
using DTO.Helpers;
using SgqSystem.Services;
using SgqSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SgqSystem.Controllers.Api
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/ApontamentosDiarios")]
    public class ApontamentosDiariosApiController : ApiController
    {

        private List<ApontamentosDiariosResultSet> _mock { get; set; }
        private List<ApontamentosDiariosResultSet> _list { get; set; }
        private SgqDbDevEntities db = new SgqDbDevEntities();

        [HttpPost]
        [Route("Get")]
        public List<ApontamentosDiariosResultSet> GetApontamentosDiarios([FromBody] FormularioParaRelatorioViewModel form)
        {
            var query = new ApontamentosDiariosResultSet().Select(form);
            _list = db.Database.SqlQuery<ApontamentosDiariosResultSet>(query).ToList();
            return _list;
        }

        [HttpPost]
        [Route("Edit/{id}")]
        public Result_Level3DTO EditResultLevel3(int id)//ENVIAR UNIT
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
                ConsolidacaoEdicao(resultLevel3.Id);
                //db.Database.ExecuteSqlCommand(queryLevel2);
            }
            catch (System.Exception e)
            {
                throw e;
            }

            return Mapper.Map<Result_Level3DTO>(Result_Level3DTO.GetById(resultLevel3.Id));
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
                    resultLevel3.ParLevel3.ParLevel3Value = databaseSgq.ParLevel3Value.AsNoTracking().Where(r => r.ParLevel3_Id == resultLevel3.ParLevel3_Id).ToList();
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
                        var parLevel3Value = databaseSgq.ParLevel3Value.AsNoTracking().Where(r => r.ParLevel3_Id == resultOld.ParLevel3_Id).ToList();

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

                var query = "UPDATE [dbo].[Result_Level3] SET ";
                query += "\n [IsConform] = " + _IsConform + ",";
                query += "\n [Defects] = " + _Defects + ",";
                query += "\n [WeiDefects] = " + _WeiDefects + ",";
                query += "\n [Value] = " + _Value + ",";
                query += "\n [IsNotEvaluate] = " + _IsNotEvaluate + ",";
                query = query.Remove(query.Length - 1);//Remove a ultima virgula antes do where.
                query += "\n WHERE Id = " + Id;

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
                            if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == unit && r.ParLevel3InputType_Id == 1) != null)//BINARIO
                                return "0";
                        }
                        catch (Exception e)
                        {
                            throw new Exception("Erro ao gerar valor na RN 45 para ParLevel3InputType_Id == 1", e);
                        }

                        try
                        {
                            if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == unit && r.ParLevel3InputType_Id == 4) != null)//CALCULADO
                                return Guard.ConverteValorCalculado(Value).ToString("G29"); //10x104 = 10.0000
                        }
                        catch (Exception e)
                        {
                            throw new Exception("Erro ao gerar valor na RN 45 para ParLevel3InputType_Id == 2", e); throw;
                        }

                        try
                        {
                            if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == unit && r.ParLevel3InputType_Id == 3) != null)//INTERVALOS
                                return Guard.ConverteValorCalculado(Value).ToString("G29"); //010.0000 = 10
                        }
                        catch (Exception e)
                        {
                            throw new Exception("Erro ao gerar valor na RN 45 para ParLevel3InputType_Id == 3", e); throw;
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

                        if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == unit && r.ParLevel3InputType_Id == 1) != null)//BINARIO
                        {
                            return IsConform.GetValueOrDefault() ? "1" : "0";
                        }
                        else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == unit && r.ParLevel3InputType_Id == 4) != null)//CALCULADO
                        {

                            var vmax = Convert.ToDecimal(IntervalMax, System.Globalization.CultureInfo.InvariantCulture);
                            var vmin = Convert.ToDecimal(IntervalMin, System.Globalization.CultureInfo.InvariantCulture);
                            var valorDefinido = Guard.ConverteValorCalculado(_Value);
                            var dentroDoRange = (valorDefinido < vmax && valorDefinido > vmin);
                            return dentroDoRange ? "1" : "0";
                        }
                        else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == unit && r.ParLevel3InputType_Id == 3) != null)//INTERVALOS 
                        {
                            var vmax = Convert.ToDecimal(IntervalMax, System.Globalization.CultureInfo.InvariantCulture);
                            var vmin = Convert.ToDecimal(IntervalMin, System.Globalization.CultureInfo.InvariantCulture);
                            var valorDefinido = Guard.ConverteValorCalculado(_Value);
                            var dentroDoRange = (valorDefinido < vmax && valorDefinido > vmin);
                            return dentroDoRange ? "1" : "0";
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
                        if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == unit && r.ParLevel3InputType_Id == 1) != null)//é um BINARIO
                        {
                            defects = IsConform.GetValueOrDefault() ? 0M : 1M;
                        }
                        else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == unit && r.ParLevel3InputType_Id == 4) != null)//CALCULADO
                        {
                            return _IsConform.Equals("0") ? 1 : 0;
                        }
                        else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == unit && r.ParLevel3InputType_Id == 3) != null)//INTERVALOS 
                        {
                            return _IsConform.Equals("0") ? 1 : 0;
                        }
                        //else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == unit && r.ParLevel3InputType_Id == 2) != null)//N° DEFEITOS
                        //{
                        //    return Convert.ToDecimal(_Value, System.Globalization.CultureInfo.InvariantCulture); ;
                        //}
                    }


                    return defects;

                }
            }

            public Nullable<decimal> PunishmentValue { get; set; }
            public Nullable<decimal> WeiEvaluation { get; set; }
            public Nullable<decimal> Evaluation { get; set; }

            public Nullable<decimal> WeiDefects { get; set; }
            public string _WeiDefects
            {
                get
                {
                    var defects = 0M;
                    if (isQueryEdit)
                    {
                        if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == unit && r.ParLevel3InputType_Id == 1) != null)//é um BINARIO
                        {
                            defects = IsConform.GetValueOrDefault() ? 0M : 1M;
                        }
                        else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == unit && r.ParLevel3InputType_Id == 4) != null)//CALCULADO
                        {
                            defects = _Defects;
                        }
                        else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == unit && r.ParLevel3InputType_Id == 3) != null)//INTERVALOS 
                        {
                            defects = _Defects;
                        }
                        //else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == unit && r.ParLevel3InputType_Id == 2) != null)//N° DEFEITOS
                        //{
                        //    defects = _Defects;
                        //}
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

            public string showIntervalos
            {
                get
                {
                    if (ParLevel3.IsNotNull())
                        if (ParLevel3.ParLevel3Value.IsNotNull())
                            if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == unit && r.ParLevel3InputType_Id == 3).IsNotNull())//INTERVALOS ??
                            {
                                var naoAvaliado = IsNotEvaluate.GetValueOrDefault() ? "checked='checked'" : "";
                                return "<div>" +
                                            "<label for='Conforme: '> Intervalo Max: </label>" + IntervalMax +
                                            "<br>" +
                                            "<label for='Conforme: '> Intervalo Min: </label>" + IntervalMin +
                                            "<br>" +
                                            "<label for='Conforme: '> Valor atual: </label>" + Value +
                                            "<br>" +
                                            "<label for='Conforme: '> Novo Valor: </label> &nbsp " +
                                             "<input type='text' id='intervaloValor' class='form-control decimal' value=" + Value + " />" +
                                            "<br>" +
                                            "<label for='Conforme: '> Não Avaliado: </label> &nbsp " +
                                             "<input type='checkbox' id='IsEvaluated' " + naoAvaliado + " class='.check-box' />" +
                                        "</div>";
                            }

                    return string.Empty;
                }
            }

            public string showConform // BINARIO
            {
                get
                {
                    if (ParLevel3.IsNotNull())
                        if (ParLevel3.ParLevel3Value.IsNotNull())
                            if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == unit && r.ParLevel3InputType_Id == 1).IsNotNull())//é um BINARIO
                            {
                                var naoAvaliado = IsNotEvaluate.GetValueOrDefault() ? "checked='checked'" : "";
                                var checkedAttr = IsConform.GetValueOrDefault() ? "checked='checked'" : "";
                                return "<div>" +
                                            "<label for='Conforme: '> Conforme: </label>" +
                                             "<input class='.check-box' id='conform' name='conform' " + checkedAttr + " type='checkbox' value='true'><input name = 'conform' type='hidden' value='false'>" +
                                             "<br>" +
                                              "<label for='Conforme: '> Não Avaliado: </label> &nbsp " +
                                             "<input type='checkbox' id='IsEvaluated' " + naoAvaliado + " class='.check-box' />" +
                                        "</div>";
                            }

                    return string.Empty;
                }
            }

            public string showCalculado
            {
                get
                {
                    if (ParLevel3.IsNotNull())
                        if (ParLevel3.ParLevel3Value.IsNotNull())
                            if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == unit && r.ParLevel3InputType_Id == 4).IsNotNull())//é um CALCULADO
                            {
                                var naoAvaliado = IsNotEvaluate.GetValueOrDefault() ? "checked='checked'" : "";

                                return "<div>" +
                                            "<label for='Conforme: '> Intervalo Max: </label>" + Guard.ConverteValorCalculado(Convert.ToDecimal(IntervalMax)) +
                                            "<br>" +
                                            "<label for='Conforme: '> Intervalo Min: </label>" + Guard.ConverteValorCalculado(Convert.ToDecimal(IntervalMin)) +
                                            "<br>" +
                                            "<label for='Conforme: '> Valor atual: </label>" + Guard.ConverteValorCalculado(Convert.ToDecimal(Value)) +
                                            "<br>" +
                                            "<label for='Conforme: '> Novo Valor: </label> &nbsp" +
                                        "<input type='text' id='decimal' class='decimal' /> ^10x <input type='text' id='precisao' class='decimal' />" +
                                        "<br>" +
                                           "<label for='Conforme: '> Não Avaliado: </label> &nbsp " +
                                             "<input type='checkbox' id='IsEvaluated' " + naoAvaliado + " class='.check-box' />" +
                                        "</div>";
                            }

                    return string.Empty;
                }
            }

        }

    }
}