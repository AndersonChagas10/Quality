using AutoMapper;
using Dominio;
using DTO.DTO.Params;
using DTO.Helpers;
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

        [HttpPost]
        [Route("Get")]
        public List<ApontamentosDiariosResultSet> GetApontamentosDiarios([FromBody] FormularioParaRelatorioViewModel form)
        {
            var query = new ApontamentosDiariosResultSet().Select(form);
            using (var db = new SgqDbDevEntities())
            {
                _list = db.Database.SqlQuery<ApontamentosDiariosResultSet>(query).ToList();
            }

            return _list;
        }

        [HttpPost]
        [Route("Edit/{id}")]
        public Result_Level3DTO EditResultLevel3(int id)//ENVIAR UNIT
        {
            var retorno = Mapper.Map<Result_Level3DTO>(Get(id));
            return retorno;
        }


        [HttpPost]
        [Route("Save")]
        public Result_Level3DTO SaveResultLevel3([FromBody] Result_Level3DTO resultLevel3)
        {
            resultLevel3.isQueryInsert = true;
            //Result_Level3 resultLevel3Old;
            var query = "UPDATE [dbo].[Result_Level3] SET ";

            using (var db = new SgqDbDevEntities())
            {
                if (resultLevel3.ParLevel3.IsNull())
                {
                    var resultOld = db.Result_Level3.FirstOrDefault(r => r.Id == resultLevel3.Id);
                    resultLevel3.ParLevel3 = Mapper.Map<ParLevel3DTO>(db.ParLevel3.AsNoTracking().FirstOrDefault(r => r.Id == resultOld.ParLevel3_Id));
                    resultLevel3.ParLevel3.ParLevel3Value = Mapper.Map<List<ParLevel3ValueDTO>>(db.ParLevel3Value.AsNoTracking().Where(r => r.ParLevel3_Id == resultOld.ParLevel3_Id).ToList());
                    resultLevel3.Weight = resultOld.Weight;
                    resultLevel3.PunishmentValue = resultOld.PunishmentValue;
                    resultLevel3.IntervalMax = resultOld.IntervalMax;
                    resultLevel3.IntervalMin = resultOld.IntervalMin;
                }

                #region UpdateLevel3

                query += "\n [IsConform] = " + resultLevel3._IsConform + ",";
                query += "\n [Defects] = " + resultLevel3._Defects + ",";
                query += "\n [WeiDefects] = " + resultLevel3._WeiDefects + ",";
                query += "\n [Value] = " + resultLevel3._Value + ",";
                query = query.Remove(query.Length - 1);//Remove a ultima virgula antes do where.
                query += "\n WHERE Id = " + resultLevel3.Id;

                try
                {
                    db.Database.ExecuteSqlCommand(query);
                    //db.Database.ExecuteSqlCommand(queryLevel2);
                    return Mapper.Map<Result_Level3DTO>(Get(resultLevel3.Id));
                }
                catch (System.Exception e)
                {
                    throw e;
                }

                #endregion
            }




            //return resultLevel3;
        }

        private Result_Level3 Get(int id)
        {
            Result_Level3 resultLevel3;
            using (var db = new SgqDbDevEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                //db.Configuration.ProxyCreationEnabled = false;
                resultLevel3 = db.Result_Level3.AsNoTracking().FirstOrDefault(r => r.Id == id);
                resultLevel3.ParLevel3 = db.ParLevel3.AsNoTracking().FirstOrDefault(r => r.Id == resultLevel3.ParLevel3_Id);
                resultLevel3.ParLevel3.ParLevel3Value = db.ParLevel3Value.AsNoTracking().Where(r => r.ParLevel3_Id == resultLevel3.ParLevel3_Id).ToList();
                resultLevel3.CollectionLevel2 = db.CollectionLevel2.AsNoTracking().FirstOrDefault(r => r.Id == resultLevel3.CollectionLevel2_Id);
            }

            return resultLevel3;
        }

        //public void a(int inputType, Result_Level3 resultLevel3)
        //{

        //    //Calculo da avaliação ponderada
        //    //Calculo do número de avaliações
        //    //Calculo level3 avaliados
        //    switch (inputType)
        //    {
        //        case 1:
        //            resultLevel3.WeiEvaluation = resultLevel3.Weight;
        //            break;
        //        case 2:
        //            resultLevel3.WeiEvaluation = numeroAmostragem;
        //            break;
        //        case 3:
        //            resultLevel3.WeiEvaluation = resultLevel.Weight;
        //            break;
        //        case 4:
        //            avaliacoesPonderadas = resultLevel.Weight;
        //            break;
        //        default:
        //            avaliacoesPonderadas = resultLevel.Weight;
        //            break;
        //    }
        //}

        //private void b(int inputType, Result_Level3 resultLevel)
        //{
        //    //Calculo do defeito
        //    //Calculo de Defeitos Ponderados
        //    //Is Conform
        //    switch (inputType)
        //    {
        //        case 1:

        //            var vvalor = parseFloat(resultLevel.Value.Replace(",", "."));
        //            var vmin = parseFloat(level3.attr('intervalmin').replace(",", "."));
        //            var vmax = parseFloat(level3.attr('intervalmax').replace(",", "."));

        //            if (vvalor >= vmin && vvalor <= vmax)
        //            {

        //                defeitosVar = 0;

        //            }
        //            else
        //            {

        //                if (isNaN(vvalor))
        //                {
        //                    defeitosVar = parseFloat(level3.attr('defects'));
        //                }
        //                else
        //                {
        //                    defeitosVar = 1;
        //                }

        //            }

        //            defeitos = defeitosVar > 0 ? 1 : 0;
        //            defeitosPonderados = (defeitos * peso) + (punicao * peso);
        //            level3ComDefeitos = defeitosPonderados > 0 ? 1 : 0;
        //            break;
        //        case 2:
        //            defeitos = parseFloat(level3.attr('value').replace(",", "."));
        //            defeitosPonderados = (parseFloat(level3.attr('value').replace(",", ".")) * peso) + (punicao * peso);
        //            level3ComDefeitos = defeitosPonderados > 0 ? 1 : 0;
        //            break;
        //        case 3:
        //            if (valor >= limiteInferior && valor <= limiteSuperior)
        //            {
        //                defeitos = 0;
        //            }
        //            else
        //            {
        //                defeitos = 1;
        //            }
        //            defeitosPonderados = (defeitos * peso) + (punicao * peso);
        //            level3ComDefeitos = defeitosPonderados > 0 ? 1 : 0;
        //            break;
        //        case 4:
        //            if (valor >= limiteInferior && valor <= limiteSuperior)
        //            {
        //                defeitos = 0;
        //            }
        //            else
        //            {
        //                defeitos = 1;
        //            }
        //            defeitosPonderados = (defeitos * peso) + (punicao * peso);
        //            level3ComDefeitos = defeitosPonderados > 0 ? 1 : 0;
        //            break;
        //        default:
        //            defeitos = 0;
        //            defeitosPonderados = 0;
        //            level3ComDefeitos = defeitosPonderados > 0 ? 1 : 0;
        //            break;
        //    }

        //}

        public class EditedResultLevel3
        {
            public string messageStatus { get; set; }
            public string message { get; set; }
        }

        public class Result_Level3DTO
        {



            public bool isQueryInsert { get; set; }
            public int Id { get; set; }
            public int CollectionLevel2_Id { get; set; }
            public int ParLevel3_Id { get; set; }
            public string ParLevel3_Name { get; set; }
            public Nullable<decimal> Weight { get; set; }
            public string IntervalMin { get; set; }
            public string IntervalMax { get; set; }

            public string Value { get; set; }
            public string _Value
            {
                get
                {
                    if (isQueryInsert)
                    {
                        if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == unit && r.ParLevel3InputType_Id == 1) != null)//BINARIO
                            return "0";
                        else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == unit && r.ParLevel3InputType_Id == 4) != null)//CALCULADO
                            return Guard.ConverteValorCalculado(Value).ToString("G29");
                        else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == unit && r.ParLevel3InputType_Id == 3) != null)//INTERVALOS
                            return Guard.ConverteValorCalculado(Value).ToString("G29");
                        else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == unit && r.ParLevel3InputType_Id == 2) != null)//N° DEFEITOS
                            return Guard.ConverteValorCalculado(Value).ToString("G29");
                    }

                    return string.Empty;
                }
            }

            public string ValueText { get; set; }

            public Nullable<bool> IsConform { get; set; }
            public string _IsConform
            {
                get
                {

                    if (isQueryInsert)
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
                        else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == unit && r.ParLevel3InputType_Id == 2) != null)//N° DEFEITOS
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

            public Nullable<decimal> Defects { get; set; }
            public decimal _Defects
            {
                get
                {

                    var defects = 0M;
                    if (isQueryInsert)
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
                        else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == unit && r.ParLevel3InputType_Id == 2) != null)//N° DEFEITOS
                        {
                            return _IsConform.Equals("0") ? 1 : 0;
                        }
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
                    if (isQueryInsert)
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
                        else if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == unit && r.ParLevel3InputType_Id == 2) != null)//N° DEFEITOS
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
            //public CollectionLevel2 CollectionLevel2 { get; set; }

            public int? unit { get; set; }
            public string showIntervalos
            {
                get
                {
                    if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == unit && r.ParLevel3InputType_Id == 3) != null)//INTERVALOS ??
                    {
                        return "<div>" +
                                    "<label for='Conforme: '> Intervalo Max: </label>" + IntervalMax +
                                    "<br>" +
                                    "<label for='Conforme: '> Intervalo Min: </label>" + IntervalMin +
                                    "<br>" +
                                    "<label for='Conforme: '> Valor atual: </label>" + Value +
                                    "<br>" +
                                    "<label for='Conforme: '> Novo Valor: </label> &nbsp " +
                                     "<input type='text' id='intervaloValor' class='form-control decimal' />" +
                                "</div>";
                    }
                    return string.Empty;
                }
            }

            public string showConform // BINARIO
            {
                get
                {
                    if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == unit && r.ParLevel3InputType_Id == 1) != null)//é um BINARIO
                    {
                        var checkedAttr = IsConform.GetValueOrDefault() ? "checked='checked'" : "";
                        return "<div>" +
                                    "<label for='Conforme: '> Conforme: </label>" +
                                     "<input class='.check-box' id='conform' name='conform' " + checkedAttr + " type='checkbox' value='true'><input name = 'conform' type='hidden' value='false'>" +
                                "</div>";
                    }
                    return string.Empty;
                }
            }

            public string showCalculado
            {
                get
                {
                    if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == unit && r.ParLevel3InputType_Id == 4) != null)//é um CALCULADO
                    {
                        return "<div>" +
                                    "<label for='Conforme: '> Intervalo Max: </label>" + Guard.ConverteValorCalculado(Convert.ToDecimal(IntervalMax)) +
                                    "<br>" +
                                    "<label for='Conforme: '> Intervalo Min: </label>" + Guard.ConverteValorCalculado(Convert.ToDecimal(IntervalMin)) +
                                    "<br>" +
                                    "<label for='Conforme: '> Valor atual: </label>" + Guard.ConverteValorCalculado(Convert.ToDecimal(Value)) +
                                    "<br>" +
                                    "<label for='Conforme: '> Novo Valor: </label> &nbsp" +
                                "<input type='text' id='decimal' class='decimal' /> ^10x <input type='text' id='precisao' class='decimal' />" +
                                "</div>";
                    }
                    return string.Empty;
                }
            }

            public string showNumeroDefeitos
            {
                get
                {
                    if (ParLevel3.ParLevel3Value.FirstOrDefault(r => r.ParCompany_Id == unit && r.ParLevel3InputType_Id == 2) != null)//N° DEFEITOS
                    {
                        return "<div>" +
                                    "<label for='Conforme: '> Valor atual: </label>" + Value +
                                    "<br>" +
                                    "<label for='Conforme: '> Novo Valor: </label> &nbsp <input type='text' id='numeroDeDefeitos' class='decimal form-control' />" +
                                //"<input type='text' id='decimal' class='decimal' /> ^10x <input type='text' id='precisao' class='decimal' />" +
                                "</div>";
                    }
                    return string.Empty;
                }
            }

        }

    }
}




//var query = "UPDATE [dbo].[Result_Level3]
//   SET[CollectionLevel2_Id] = < CollectionLevel2_Id, int,>
//      ,[ParLevel3_Id] = <ParLevel3_Id, int,>
//      ,[ParLevel3_Name] = <ParLevel3_Name, nvarchar(max),>
//      ,[Weight] = <Weight, decimal(10,5),>
//      ,[IntervalMin] = <IntervalMin, nvarchar(max),>
//      ,[IntervalMax] = <IntervalMax, nvarchar(max),>
//      ,[Value] = <Value, nvarchar(max),>
//      ,[ValueText] = <ValueText, nvarchar(max),>
//      ,[IsConform] = <IsConform, bit,>
//      ,[IsNotEvaluate] = <IsNotEvaluate, bit,>
//      ,[Defects] = <Defects, decimal(10,5),>
//      ,[PunishmentValue] = <PunishmentValue, decimal(10,5),>
//      ,[WeiEvaluation] = <WeiEvaluation, decimal(10,5),>
//      ,[Evaluation] = <Evaluation, decimal(10,5),>
//      ,[WeiDefects] = <WeiDefects, decimal(30,8),>
//      ,[CT4Eva3] = <CT4Eva3, decimal(10,5),>
//      ,[Sampling] = <Sampling, decimal(10,5),>
// WHERE<Search Conditions,,>"