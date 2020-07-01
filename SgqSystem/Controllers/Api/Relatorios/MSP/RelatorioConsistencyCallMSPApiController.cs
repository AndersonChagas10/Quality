using ADOFactory;
using Dominio;
using DTO;
using DTO.ResultSet;
using DTO.Helpers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using SgqService.ViewModels;
using System.Globalization;

namespace SgqSystem.Controllers.Api.Relatorios
{
    [RoutePrefix("api/RelatorioConsistencyCallMSP")]
    public class RelatorioConsistencyCallMSPApiController : ApiController
    {
        private List<RelatorioConsistencyCallMSPResultSet> _list { get; set; }

        [HttpPost]
        [Route("GetlistaTabelaConsistencyCall")]
        public RelatorioConsistencyCallMSPResultSet listaTabelaConsistencyCall([FromBody] DataCarrierFormularioNew form)
        {
            RelatorioConsistencyCallMSPResultSet obj = new RelatorioConsistencyCallMSPResultSet();

            var queryApontamentosDiarios = new RelatorioConsistencyCallMSPResultSet().Select(form);

            using (Factory factory = new Factory("DefaultConnection"))
            {
                var retornoApontamentosDiarios = factory.SearchQuery<TabelaConsistencyCallMSPResultSet>(queryApontamentosDiarios).ToList();

                if (retornoApontamentosDiarios.Count() == 0)
                    return obj;

                var productAndFlavor = "";
                decimal qtdFlats = 0;
                decimal qtdInsides = 0;
                decimal qtdEyes = 0;

                foreach (var item in retornoApontamentosDiarios)
                {
                    var headerFields = item.HeaderFieldList.Split(',');
                    string[] produto = { };
                    string[] sabor = { };
                    foreach (var cabecalho in headerFields)
                    {
                        if (cabecalho.ToUpper().Contains("PRODUTO"))
                            produto = cabecalho.Split(':');
                        if (cabecalho.ToUpper().Contains("SABOR"))
                            sabor = cabecalho.Split(':');
                        if (item.Tarefa == DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.flatsInsidesEyes as string && cabecalho.ToUpper().Contains("MATÉRIA-PRIMA/ RAW MATERIAL"))
                        {
                            if (cabecalho.ToUpper().Contains("FLATS"))
                                qtdFlats++;
                            if (cabecalho.ToUpper().Contains("INSIDES"))
                                qtdInsides++;
                            if (cabecalho.ToUpper().Contains("EYES"))
                                qtdEyes++;
                        }
                    }
                    if (produto.Count() > 0 && sabor.Count() > 0)
                    {
                        var produtoSabor = produto[1] + " -" + sabor[1];

                        if (productAndFlavor == "")
                        {
                            productAndFlavor += produtoSabor;
                        }
                        else
                        {
                            if (!productAndFlavor.ToLower().Contains(produtoSabor.ToLower()))
                                productAndFlavor += " / " + produtoSabor;
                        }
                    }
                }

                obj.Product = productAndFlavor;

                obj.Batch1 = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.batch as string
                    && x.Turno.Trim() == "1").Count();

                obj.Batch2 = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.batch as string
                    && x.Turno.Trim() == "2").Count();


                var pesoCarneT1 = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.porcWaterPesoCarne as string
                    && x.Turno.Trim() == "1");

                var pesoCarneT2 = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.porcWaterPesoCarne as string
                    && x.Turno.Trim() == "2");

                var volumeAguaT1 = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.porcWaterVolumeAgua as string
                    && x.Turno.Trim() == "1");

                var volumeAguaT2 = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.porcWaterVolumeAgua as string
                    && x.Turno.Trim() == "2");

                if (volumeAguaT1.Count() > 0 && pesoCarneT1.Count() > 0)
                    obj.PorcWater1 = (volumeAguaT1.Sum(x => Convert.ToDecimal(x.Value, new CultureInfo("en-US"))) 
                        / pesoCarneT1.Sum(x => Convert.ToDecimal(x.Value, new CultureInfo("en-US")))) * 100;

                if (volumeAguaT2.Count() > 0 && pesoCarneT2.Count() > 0)
                    obj.PorcWater2 = (volumeAguaT2.Sum(x => Convert.ToDecimal(x.Value, new CultureInfo("en-US"))) 
                        / pesoCarneT2.Sum(x => Convert.ToDecimal(x.Value, new CultureInfo("en-US")))) * 100;


                var meatAgeMinMax1 = retornoApontamentosDiarios.Where(
                    x => x.Tarefa.Trim() == DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.meatAge as string);
                if (meatAgeMinMax1.Count() > 0)
                    obj.Meat_age_min_max1 = meatAgeMinMax1.Min(x => Convert.ToDecimal(x.Value, new CultureInfo("en-US")));

                var meatAgeMinMax2 = retornoApontamentosDiarios.Where(
                    x => x.Tarefa.Trim() == DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.meatAge as string);
                if (meatAgeMinMax2.Count() > 0)
                    obj.Meat_age_min_max2 = meatAgeMinMax2.Max(x => Convert.ToDecimal(x.Value, new CultureInfo("en-US")));

                var meatAgeAvg = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.meatAge as string);
                if (meatAgeAvg.Count() > 0)
                    obj.Meat_age_avg = meatAgeAvg.Average(x => Convert.ToDecimal(x.Value, new CultureInfo("en-US")));


                var totalAmostras = qtdFlats + qtdInsides + qtdEyes;
                if (totalAmostras > 0)
                {
                    obj.Flats = (qtdFlats / totalAmostras) * 100;
                    obj.Insides = (qtdInsides / totalAmostras) * 100;
                    obj.Eyes = (qtdEyes / totalAmostras) * 100;
                }


                var tumblerBatchSize = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.tumblerBatchSize as string);
                if (tumblerBatchSize.Count() > 0)
                    obj.Tumbler_batch_size = tumblerBatchSize.Sum(x => Convert.ToDecimal(x.Value, new CultureInfo("en-US")));


                var meatTemperatureActual1 = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.meatTemperature as string);
                if (meatTemperatureActual1.Count() > 0)
                    obj.Meat_temperature_actual1 = meatTemperatureActual1.Average(x => Convert.ToDecimal(x.Value, new CultureInfo("en-US")));
                
                obj.Meat_temperature_actual2 = (obj.Meat_temperature_actual1 * 9/5) + 32;


                var thicknessAvg1_CDCM = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.thicknessAvgCoxaoDuro as string
                    && x.Turno.Trim() == "1");
                if (thicknessAvg1_CDCM.Count() > 0) 
                    obj.Thickness_avg1_CDCM = thicknessAvg1_CDCM.Average(x => Convert.ToDecimal(x.Value, new CultureInfo("en-US")));
                
                var thicknessAvg2_CDCM = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.thicknessAvgCoxaoDuro as string
                    && x.Turno.Trim() == "2");
                if (thicknessAvg2_CDCM.Count() > 0)
                    obj.Thickness_avg2_CDCM = thicknessAvg2_CDCM.Average(x => Convert.ToDecimal(x.Value, new CultureInfo("en-US")));
                
                obj.Thickness_sample_size1_CDCM = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.thicknessAvgCoxaoDuro as string
                    && x.Turno.Trim() == "1").Count();
                
                obj.Thickness_sample_size2_CDCM = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.thicknessAvgCoxaoDuro as string
                    && x.Turno.Trim() == "2").Count();


                var thicknessAvg1_BL = retornoApontamentosDiarios.Where(
                    x => (x.Tarefa == DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.thicknessAvgLagarto as string
                    || x.Tarefa == DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.thicknessAvgBorboleta as string)
                    && x.Turno.Trim() == "1");
                if (thicknessAvg1_BL.Count() > 0)
                    obj.Thickness_avg1_BL = thicknessAvg1_BL.Average(x => Convert.ToDecimal(x.Value, new CultureInfo("en-US")));
                
                var thicknessAvg2_BL = retornoApontamentosDiarios.Where(
                    x => (x.Tarefa == DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.thicknessAvgLagarto as string
                    || x.Tarefa == DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.thicknessAvgBorboleta as string)
                    && x.Turno.Trim() == "2");
                if (thicknessAvg2_BL.Count() > 0)
                    obj.Thickness_avg2_BL = thicknessAvg2_BL.Average(x => Convert.ToDecimal(x.Value, new CultureInfo("en-US")));
                
                obj.Thickness_sample_size1_BL = retornoApontamentosDiarios.Where(
                    x => (x.Tarefa == DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.thicknessAvgLagarto as string
                    || x.Tarefa == DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.thicknessAvgBorboleta as string) 
                    && x.Turno.Trim() == "1").Count();
                
                obj.Thickness_sample_size2_BL = retornoApontamentosDiarios.Where(
                    x => (x.Tarefa == DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.thicknessAvgLagarto as string
                    || x.Tarefa == DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.thicknessAvgBorboleta as string)
                    && x.Turno.Trim() == "2").Count();


                decimal meetCanadianConformes1 = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.meetCanadianRequirements as string
                    && x.Conforme == "True" && x.Turno.Trim() == "1").Count();                
                decimal meetCanadianTotal1 = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.meetCanadianRequirements as string
                    && x.Turno.Trim() == "1").Count();
                if (meetCanadianTotal1 > 0)
                    obj.Meet_requirements1 = (meetCanadianConformes1 / meetCanadianTotal1) * 100;

                decimal meetCanadianConformes2 = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.meetCanadianRequirements as string
                    && x.Conforme == "True" && x.Turno.Trim() == "2").Count();               
                decimal meetCanadianTotal2 = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.meetCanadianRequirements as string
                    && x.Turno.Trim() == "2").Count();
                if (meetCanadianTotal2 > 0)
                    obj.Meet_requirements2 = (meetCanadianConformes2 / meetCanadianTotal2) * 100;


                var marinationTime = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.marinationTime as string);
                if (marinationTime.Count() > 0)
                    obj.Marination_time = marinationTime.Average(x => Convert.ToDecimal(x.Value, new CultureInfo("en-US")));


                var waitTimeAvg1 = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.waitTime as string 
                    && x.Turno.Trim() == "1");
                if (waitTimeAvg1.Count() > 0)
                    obj.Wait_time_avg1 = waitTimeAvg1.Average(x => Convert.ToDecimal(x.Value, new CultureInfo("en-US")));

                var waitTimeAvg2 = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.waitTime as string
                    && x.Turno.Trim() == "2");
                if (waitTimeAvg2.Count() > 0)
                    obj.Wait_time_avg2 = waitTimeAvg2.Average(x => Convert.ToDecimal(x.Value, new CultureInfo("en-US")));


                var maxTime1 = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.waitTime as string
                    && x.Turno.Trim() == "1");
                if (maxTime1.Count() > 0)
                    obj.Max_time1 = maxTime1.Max(x => Convert.ToDecimal(x.Value, new CultureInfo("en-US")));

                var maxTime2 = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.waitTime as string
                    && x.Turno.Trim() == "2");
                if (maxTime2.Count() > 0)
                    obj.Max_time2 = maxTime2.Max(x => Convert.ToDecimal(x.Value, new CultureInfo("en-US")));


                decimal foss1Conforme = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.fossPull as string
                    && x.Conforme == "True").Count();                
                decimal foss1Total = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.fossPull as string).Count();
                if (foss1Total > 0)
                    obj.Foss_used_for_pull = (foss1Conforme / foss1Total) * 100;

                decimal foss2Conforme = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.fossPacking as string
                    && x.Conforme == "True").Count();                
                decimal foss2Total = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.fossPacking as string
                    ).Count();
                if (foss2Total > 0)
                    obj.Foss_used_for_packing = (foss2Conforme / foss2Total) * 100;


                var pullMoistureAvg = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.pullMoistureAvg as string);
                if (pullMoistureAvg.Count() > 0)
                    obj.Pull_moisture_avg = pullMoistureAvg.Average(x => Convert.ToDecimal(x.Value, new CultureInfo("en-US")));


                var stagingRoomTemperature1 = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.stagingRoomTemperature as string);
                if (stagingRoomTemperature1.Count() > 0)
                    obj.Staging_room_temperature1 = stagingRoomTemperature1.Average(x => Convert.ToDecimal(x.Value, new CultureInfo("en-US")));
                
                obj.Staging_room_temperature2 = (obj.Staging_room_temperature1 * 9 / 5) + 32;


                var packingWaterActivity = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.packingWaterActivity as string);
                if (packingWaterActivity.Count() > 0)
                    obj.Packing_water_activity = packingWaterActivity.Average(x => Convert.ToDecimal(x.Value, new CultureInfo("en-US")));

                var packingMoistureAvg = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.packingMoistureAvg as string);
                if (packingMoistureAvg.Count() > 0)
                    obj.Packing_moisture_avg = packingMoistureAvg.Average(x => Convert.ToDecimal(x.Value, new CultureInfo("en-US")));


                var reanalysisFoss1 = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.reanalysisFoss1 as string);
                if (reanalysisFoss1.Count() > 0)
                    obj.Reanalysis_foss_1 = reanalysisFoss1.Average(x => Convert.ToDecimal(x.Value, new CultureInfo("en-US")));

                var reanalysisFoss2 = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.reanalysisFoss2 as string);
                if (reanalysisFoss2.Count() > 0)
                    obj.Reanalysis_foss_2 = reanalysisFoss2.Average(x => Convert.ToDecimal(x.Value, new CultureInfo("en-US")));


                var packingRoomTemperatureMax1 = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.packingRoomTemperature as string);
                if (packingRoomTemperatureMax1.Count() > 0)
                    obj.Packing_room_temperature_max1 = packingRoomTemperatureMax1.Max(x => Convert.ToDecimal(x.Value, new CultureInfo("en-US")));
                
                obj.Packing_room_temperature_max2 = (obj.Packing_room_temperature_max1 * 9 / 5) + 32;

                var packingRoomTemperature1 = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.packingRoomTemperature as string);
                if (packingRoomTemperature1.Count() > 0)
                    obj.Packing_room_temperature1 = packingRoomTemperature1.Average(x => Convert.ToDecimal(x.Value, new CultureInfo("en-US")));
                
                obj.Packing_room_temperature2 = (obj.Packing_room_temperature1 * 9 / 5) + 32;


                var woodChips = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.woodChips as string);
                if (woodChips.Count() > 0)
                    obj.Wood_chips = woodChips.Average(x => Convert.ToDecimal(x.Value, new CultureInfo("en-US")));


                var finalProductThicknessAvg = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.finalProductThickness as string);
                if (finalProductThicknessAvg.Count() > 0)
                    obj.Final_product_thickness_avg = finalProductThicknessAvg.Average(x => Convert.ToDecimal(x.Value, new CultureInfo("en-US")));
                obj.Thickness_sample_size = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.finalProductThickness as string).Count();


                var cookingFlavor = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.flavorSensorial as string);
                if (cookingFlavor.Count() > 0)
                    obj.Cooking_flavor = cookingFlavor.Average(x => Convert.ToDecimal(x.Value, new CultureInfo("en-US")));

                var odor = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.odorSensorial as string);
                if (odor.Count() > 0)
                    obj.Odor = odor.Average(x => Convert.ToDecimal(x.Value, new CultureInfo("en-US")));

                var texture = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.textureSensorial as string);
                if (texture.Count() > 0)
                    obj.Texture = texture.Average(x => Convert.ToDecimal(x.Value, new CultureInfo("en-US")));

                var appearance = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.appearanceSensorial as string);
                if (appearance.Count() > 0)
                    obj.Appearance = appearance.Average(x => Convert.ToDecimal(x.Value, new CultureInfo("en-US")));

            }

            // Itens que não tem query \/

            obj.Raw_side = form.startDate.ToString("dd/MM/yyyy");
            obj.Meat_age_target = 14; //dado correto
            obj.Meat_temperature_max1 = 7; //dado correto
            obj.Meat_temperature_max2 = (decimal)44.6; //dado correto
            obj.Meat_temperature_min1 = -1; //dado correto
            obj.Meat_temperature_min2 = (decimal)30.2; //dado correto

            obj.Thickness_avg_max_CDCM = (decimal)8.2; //dado correto
            obj.Thickness_avg_min_CDCM = (decimal)5.8; //dado correto
            obj.Out_spec_target_CDCM = 7; //dado correto
            obj.Porc_out_spec1_CDCM = null;
            obj.Porc_out_spec2_CDCM = null;
            obj.Porc_LSL1_CDCM = null;
            obj.Porc_LSL2_CDCM = null;
            obj.Porc_USL1_CDCM = null;
            obj.Porc_USL2_CDCM = null;

            obj.Thickness_avg_max_BL = (decimal)7.3; //dado correto
            obj.Thickness_avg_min_BL = (decimal)4.9; //dado correto
            obj.Out_spec_target_BL = 7;//dado correto
            obj.Porc_out_spec1_BL = null;
            obj.Porc_out_spec2_BL = null;
            obj.Porc_LSL1_BL = null;
            obj.Porc_LSL2_BL = null;
            obj.Porc_USL1_BL = null;
            obj.Porc_USL2_BL = null;

            obj.Meat_weight_inside_smokehouse_target = 4000; //dado correto
            obj.Meat_weight_inside_smokehouse = null;
            obj.Porc_purge_target = 6; //dado correto
            obj.Porc_purge = null;

            obj.Cooking = form.startDate.ToString("dd/MM/yyyy");
            obj.Marination_time_max = 96; //dado correto
            obj.Marination_time_min = 44; //dado correto
            obj.Time_by_sample1 = null;
            obj.Time_by_sample2 = null;

            obj.Cooking_time_target = "4:15"; //dado correto
            obj.Cooking_time_avg = null;
            obj.Standard_pull_moisture_max = (decimal)32.0; //dado correto
            obj.Standard_pull_moisture_min = (decimal)31.0; //dado correto
            obj.Packing_water_activity_max = (decimal)0.850; //dado correto
            obj.Packing_moisture_avg_min = 26; //dado correto
            obj.Packing_moisture_avg_max = 30; //dado correto
            obj.Alpenas_moisture = null;
            obj.Yield_target = (decimal)55.6; //dado correto
            obj.Yield_target_min = (decimal)54; //dado correto
            obj.Yield1 = null;
            obj.Yield2 = null;
            obj.Wood_chips_target = (decimal)7.0; //dado correto
            obj.Final_product_thickness_max = (decimal)5.78; //dado correto
            obj.Final_product_thickness_min = (decimal)4.20; //dado correto
            obj.Out_of_spec_target = 39; //dado correto
            obj.Porc_lsl = null;  //************* TARGET 20% *************
            obj.Porc_usl = null;  //************* TARGET 10% *************
            obj.Filters_on_smokehouse_exhaustion = "All Smokehouses"; //dado correto
            obj.Reprocessing_target = 1; //dado correto

            return obj;
        }
    }
}
