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

            var querySelectFlavor = new RelatorioConsistencyCallMSPResultSet().SelectFlavor(form);
            var querySelectProduct = new RelatorioConsistencyCallMSPResultSet().SelectProduct(form);
            var querySelectBatch1 = new RelatorioConsistencyCallMSPResultSet().SelectBatch1(form);
            var querySelectBatch2 = new RelatorioConsistencyCallMSPResultSet().SelectBatch2(form);
            var querySelectPorcWater1 = new RelatorioConsistencyCallMSPResultSet().SelectPorcWater1(form);
            var querySelectPorcWater2 = new RelatorioConsistencyCallMSPResultSet().SelectPorcWater2(form);
            var querySelectPorcWater3 = new RelatorioConsistencyCallMSPResultSet().SelectPorcWater3(form);
            var querySelectPorcWater4 = new RelatorioConsistencyCallMSPResultSet().SelectPorcWater4(form);
            var querySelectMeatAgeAVG = new RelatorioConsistencyCallMSPResultSet().SelectMeatAgeAVG(form);
            var querySelectMeatAgeMin = new RelatorioConsistencyCallMSPResultSet().SelectMeatAgeMin(form);
            var querySelectMeatAgeMax = new RelatorioConsistencyCallMSPResultSet().SelectMeatAgeMax(form);
            var querySelectMeats = new RelatorioConsistencyCallMSPResultSet().SelectMeats(form);
            var querySelectTumblerBatchSize1 = new RelatorioConsistencyCallMSPResultSet().SelectTumblerBatchSize1(form);
            var querySelectTumblerBatchSize2 = new RelatorioConsistencyCallMSPResultSet().SelectTumblerBatchSize2(form);
            var querySelectTumblerBatchSize3 = new RelatorioConsistencyCallMSPResultSet().SelectTumblerBatchSize3(form);
            var querySelectTumblerBatchSize4 = new RelatorioConsistencyCallMSPResultSet().SelectTumblerBatchSize4(form);
            var querySelectProductAppearance = new RelatorioConsistencyCallMSPResultSet().SelectProductAppearance(form);
            var querySelectSeasoningDistribuition = new RelatorioConsistencyCallMSPResultSet().SelectSeasoningDistribuition(form);
            var querySelectMeatTemperature = new RelatorioConsistencyCallMSPResultSet().SelectMeatTemperature(form);
            var querySelectThickness = new RelatorioConsistencyCallMSPResultSet().SelectThickness(form);
            var querySelectMeetCanadianRequirements = new RelatorioConsistencyCallMSPResultSet().SelectMeetCanadianRequirements(form);
            var querySelectWaitTime = new RelatorioConsistencyCallMSPResultSet().SelectWaitTime(form);
            var querySelectPullMoistureAVG = new RelatorioConsistencyCallMSPResultSet().SelectPullMoistureAVG(form);
            var querySelectStagingRoomTemperature = new RelatorioConsistencyCallMSPResultSet().SelectStagingRoomTemperature(form);
            var querySelectPackingWaterActivity = new RelatorioConsistencyCallMSPResultSet().SelectPackingWaterActivity(form);
            var querySelectPackingMoistureAVG = new RelatorioConsistencyCallMSPResultSet().SelectPackingMoistureAVG(form);
            var querySelectReanalysisFoss1 = new RelatorioConsistencyCallMSPResultSet().SelectReanalysisFoss1(form);
            var querySelectReanalysisFoss2 = new RelatorioConsistencyCallMSPResultSet().SelectReanalysisFoss2(form);
            var querySelectPackingRoomTemperture = new RelatorioConsistencyCallMSPResultSet().SelectPackingRoomTemperture(form);
            var querySelectWoodChips = new RelatorioConsistencyCallMSPResultSet().SelectWoodChips(form);
            var querySelectFinalProductThicknessAVG = new RelatorioConsistencyCallMSPResultSet().SelectFinalProductThicknessAVG(form);
            var querySelectRework = new RelatorioConsistencyCallMSPResultSet().SelectRework(form);


            using (Factory factory = new Factory("DefaultConnection"))
            {
                var flavor = factory.SearchQuery<TabelaConsistencyCallMSPResultSet>(querySelectFlavor).ToList();
                if (flavor.Count > 0)
                    obj.Flavor = flavor[0].Name;

                var product = factory.SearchQuery<TabelaConsistencyCallMSPResultSet>(querySelectProduct).ToList();
                if (product.Count > 0)
                    obj.Product = product[0].Name;

                var batch1 = factory.SearchQuery<TabelaConsistencyCallMSPResultSet>(querySelectBatch1).ToList();
                if (batch1.Count > 0)
                    obj.Batch1 = Convert.ToDecimal(batch1[0].Batch);

                var batch2 = factory.SearchQuery<TabelaConsistencyCallMSPResultSet>(querySelectBatch2).ToList();
                if(batch2.Count > 0)
                    obj.Batch2 = Convert.ToDecimal(batch2[0].Batch);

                var porcWater1 = factory.SearchQuery<TabelaConsistencyCallMSPResultSet>(querySelectPorcWater1).ToList();
                var porcWater2 = factory.SearchQuery<TabelaConsistencyCallMSPResultSet>(querySelectPorcWater2).ToList();
                var porcWater3 = factory.SearchQuery<TabelaConsistencyCallMSPResultSet>(querySelectPorcWater3).ToList();
                var porcWater4 = factory.SearchQuery<TabelaConsistencyCallMSPResultSet>(querySelectPorcWater4).ToList();
                if (porcWater4.Count > 0)
                    obj.PorcWater1 = Convert.ToDecimal(porcWater1[0].PorcWater);
                if (porcWater4.Count > 0)
                    obj.PorcWater2 = Convert.ToDecimal(porcWater2[0].PorcWater);

                var meat_age_avg = factory.SearchQuery<TabelaConsistencyCallMSPResultSet>(querySelectMeatAgeAVG).ToList();
                if (meat_age_avg.Count > 0)
                    obj.Meat_age_avg = Convert.ToDecimal(meat_age_avg[0].MeatAgeAVG);

                var meat_age_min_max1 = factory.SearchQuery<TabelaConsistencyCallMSPResultSet>(querySelectMeatAgeMin).ToList();
                if (meat_age_min_max1.Count > 0)
                    obj.Meat_age_min_max1 = Convert.ToDecimal(meat_age_min_max1[0].MeatAgeMin);

                var meat_age_min_max2 = factory.SearchQuery<TabelaConsistencyCallMSPResultSet>(querySelectMeatAgeMax).ToList();
                if (meat_age_min_max2.Count > 0)
                    obj.Meat_age_min_max2 = Convert.ToDecimal(meat_age_min_max2[0].MeatAgeMax);

                var meats = factory.SearchQuery<TabelaConsistencyCallMSPResultSet>(querySelectMeats).ToList();
                if (meats.Count > 0)
                {
                    foreach(var item in meats)
                    {
                        if(item.Meat == "Flats")
                            obj.Flats = Convert.ToDecimal(item.PorcMeat);
                        if(item.Meat == "Wedges")
                            obj.Insides = Convert.ToDecimal(item.PorcMeat);
                        if(item.Meat == "Eyes")
                            obj.Eyes = Convert.ToDecimal(item.PorcMeat);
                    }
                }

                var tumbler_batch_size1 = factory.SearchQuery<TabelaConsistencyCallMSPResultSet>(querySelectTumblerBatchSize1).ToList();
                var tumbler_batch_size2 = factory.SearchQuery<TabelaConsistencyCallMSPResultSet>(querySelectTumblerBatchSize2).ToList();
                var tumbler_batch_size3 = factory.SearchQuery<TabelaConsistencyCallMSPResultSet>(querySelectTumblerBatchSize3).ToList();
                var tumbler_batch_size4 = factory.SearchQuery<TabelaConsistencyCallMSPResultSet>(querySelectTumblerBatchSize4).ToList();
                //obj.Tumbler_batch_size = Convert.ToDecimal(tumbler_batch_size1);

                var product_appearance = factory.SearchQuery<TabelaConsistencyCallMSPResultSet>(querySelectProductAppearance).ToList();
                //obj.Product_appearance = product_appearance.ToString();

                var seasoning_distribuition = factory.SearchQuery<TabelaConsistencyCallMSPResultSet>(querySelectSeasoningDistribuition).ToList();
                //obj.Seasoning_distribuition = seasoning_distribuition.ToString();

                var meat_temperature = factory.SearchQuery<TabelaConsistencyCallMSPResultSet>(querySelectMeatTemperature).ToList();
                if(meat_temperature.Count > 0)
                {
                    foreach(var item in meat_temperature)
                    {
                        if (item.Tipo_Graus == "C")
                            obj.Meat_temperature_actual1 = Convert.ToDecimal(item.Temperatura_Atual);
                        if (item.Tipo_Graus == "F")
                            obj.Meat_temperature_actual2 = Convert.ToDecimal(item.Temperatura_Atual);
                    }
                }

                var thickness_avg = factory.SearchQuery<TabelaConsistencyCallMSPResultSet>(querySelectThickness).ToList();
                //obj.Thickness_avg1_CDCM = Convert.ToDecimal(thickness_avg1_CDCM);
                //obj.Thickness_avg2_CDCM = Convert.ToDecimal(thickness_avg2_CDCM);
                //obj.Thickness_sample_size1_CDCM = Convert.ToDecimal(thickness_sample_size1_CDCM);
                //obj.Thickness_sample_size2_CDCM = Convert.ToDecimal(thickness_sample_size2_CDCM);

                var meet_requirements = factory.SearchQuery<TabelaConsistencyCallMSPResultSet>(querySelectMeetCanadianRequirements).ToList();
                //obj.Meet_requirements1 = meet_requirements1.ToString();
                //obj.Meet_requirements2 = meet_requirements2.ToString();

                var wait_time = factory.SearchQuery<TabelaConsistencyCallMSPResultSet>(querySelectWaitTime).ToList();
                if (wait_time.Count > 0)
                {
                    obj.Wait_time_avg1 = Convert.ToDecimal(wait_time[0].TempoMedio1);
                    obj.Wait_time_avg2 = Convert.ToDecimal(wait_time[0].TempoMedio2);
                    obj.Max_time1 = wait_time[0].TempoMaximo1;
                    obj.Max_time2 = wait_time[0].TempoMaximo2;
                }

                var pull_moisture_avg = factory.SearchQuery<TabelaConsistencyCallMSPResultSet>(querySelectPullMoistureAVG).ToList();
                if (pull_moisture_avg.Count > 0)
                    obj.Pull_moisture_avg = Convert.ToDecimal(pull_moisture_avg[0].AVG_Packing_Moisture);

                var staging_room_temperature = factory.SearchQuery<TabelaConsistencyCallMSPResultSet>(querySelectStagingRoomTemperature).ToList();
                if (staging_room_temperature.Count > 0)
                {
                    foreach(var item in staging_room_temperature)
                    {
                        if (item.Tipo_Graus == "C")
                            obj.Staging_room_temperature1 = Convert.ToDecimal(item.Temperatura_Media);
                        if (item.Tipo_Graus == "F")
                            obj.Staging_room_temperature2 = Convert.ToDecimal(item.Temperatura_Media);
                    }
                }

                var packing_water_activity = factory.SearchQuery<TabelaConsistencyCallMSPResultSet>(querySelectPackingWaterActivity).ToList();
                if (packing_water_activity.Count > 0)
                    obj.Packing_water_activity = Convert.ToDecimal(packing_water_activity[0].AVG_Packing_Water_Activity);

                var packing_moisture_avg = factory.SearchQuery<TabelaConsistencyCallMSPResultSet>(querySelectPackingMoistureAVG).ToList();
                if (packing_moisture_avg.Count > 0)
                    obj.Packing_moisture_avg = Convert.ToDecimal(packing_moisture_avg[0].AVG_Packing_Moisture);

                var reanalysis_foss_1 = factory.SearchQuery<TabelaConsistencyCallMSPResultSet>(querySelectReanalysisFoss1).ToList();
                //obj.Reanalysis_foss_1 = Convert.ToDecimal(reanalysis_foss_1);

                var reanalysis_foss_2 = factory.SearchQuery<TabelaConsistencyCallMSPResultSet>(querySelectReanalysisFoss2).ToList();
                //obj.Reanalysis_foss_2 = Convert.ToDecimal(reanalysis_foss_2);

                var packing_room_temperature = factory.SearchQuery<TabelaConsistencyCallMSPResultSet>(querySelectPackingRoomTemperture).ToList();
                if (packing_room_temperature.Count > 0)
                {
                    obj.Packing_room_temperature_max1 = Convert.ToDecimal(packing_room_temperature[0].Temperatura_Maxima_C);
                    obj.Packing_room_temperature_max2 = Convert.ToDecimal(packing_room_temperature[0].Temperatura_Maxima_F);
                    obj.Packing_room_temperature1 = Convert.ToDecimal(packing_room_temperature[0].Temperatura_Media_C);
                    obj.Packing_room_temperature2 = Convert.ToDecimal(packing_room_temperature[0].Temperatura_Media_F);
                }
                

                var wood_chips = factory.SearchQuery<TabelaConsistencyCallMSPResultSet>(querySelectWoodChips).ToList();
                if (wood_chips.Count > 0)  
                    obj.Wood_chips = Convert.ToDecimal(wood_chips[0].AVG_Chips);

                var final_product_thickness_avg = factory.SearchQuery<TabelaConsistencyCallMSPResultSet>(querySelectFinalProductThicknessAVG).ToList();
                if (final_product_thickness_avg.Count > 0)
                    obj.Final_product_thickness_avg = Convert.ToDecimal(final_product_thickness_avg[0].Espessura);

                var rework1 = factory.SearchQuery<TabelaConsistencyCallMSPResultSet>(querySelectRework).ToList();
                //obj.Rework1 = Convert.ToDecimal(rework1);
                //obj.Rework2 = Convert.ToDecimal(rework2);
                //obj.Cooking_flavor = Convert.ToDecimal(cooking_flavor);
                //obj.Odor = Convert.ToDecimal(odor);
                //obj.Texture = Convert.ToDecimal(texture);
                //obj.Appearance = Convert.ToDecimal(appearance);
            }

            // Itens que não tem query \/

            obj.Raw_side = "March 27, 2019";
            obj.Meat_age_target = 14; //dado correto
            obj.Meat_temperature_max1 = 7; //dado correto
            obj.Meat_temperature_max2 = (decimal)44.6; //dado correto
            obj.Meat_temperature_min1 = -1; //dado correto
            obj.Meat_temperature_min2 = (decimal)30.2; //dado correto

            obj.Thickness_avg_max_CDCM = (decimal)8.2; //dado correto
            obj.Thickness_avg_min_CDCM = (decimal)5.8; //dado correto
            obj.Out_spec_target_CDCM = 7;
            obj.Porc_out_spec1_CDCM = (decimal)2.6;
            obj.Porc_out_spec2_CDCM = (decimal)4.2;
            obj.Porc_LSL1_CDCM = (decimal)1.3;
            obj.Porc_LSL2_CDCM = (decimal)2.0;
            obj.Porc_USL1_CDCM = (decimal)1.2;
            obj.Porc_USL2_CDCM = (decimal)2.2;

            obj.Thickness_avg_max_BL = (decimal)7.3; //dado correto
            obj.Thickness_avg_min_BL = (decimal)4.9; //dado correto
            obj.Thickness_avg1_BL = (decimal)6.99;
            obj.Thickness_avg2_BL = (decimal)6.12;
            obj.Thickness_sample_size1_BL = 1008;
            obj.Thickness_sample_size2_BL = 672;
            obj.Out_spec_target_BL = 7;
            obj.Porc_out_spec1_BL = (decimal)2.6;
            obj.Porc_out_spec2_BL = (decimal)4.2;
            obj.Porc_LSL1_BL = (decimal)1.3;
            obj.Porc_LSL2_BL = (decimal)2.0;
            obj.Porc_USL1_BL = (decimal)1.2;
            obj.Porc_USL2_BL = (decimal)2.2;

            obj.Meat_weight_inside_smokehouse_target = 4000; //dado correto
            obj.Meat_weight_inside_smokehouse = (decimal)5.64;
            obj.Porc_purge_target = 6; //dado correto
            obj.Porc_purge = 4;

            obj.Cooking = "March 29, 2019";
            obj.Marination_time_max = 96; //dado correto
            obj.Marination_time_min = 44; //dado correto
            obj.Marination_time = 45;
            obj.Time_by_sample1 = "5:38";
            obj.Time_by_sample2 = "6:01";

            obj.Foss_used_for_pull = "Foss 1";
            obj.Foss_used_for_packing = "Foss 2";
            obj.Cooking_time_target = "4:15"; //dado correto
            obj.Cooking_time_avg = "3.36";
            obj.Standard_pull_moisture_max = (decimal)32.0; //dado correto
            obj.Standard_pull_moisture_min = (decimal)31.0; //dado correto
            obj.Packing_water_activity_max = (decimal)0.850; //dado correto
            obj.Packing_moisture_avg_min = 26; //dado correto
            obj.Packing_moisture_avg_max = 30; //dado correto
            obj.Alpenas_moisture = 10;
            obj.Yield_target = (decimal)54; //dado correto
            obj.Yield_target_min = (decimal)55.6; //dado correto
            obj.Yield1 = (decimal)57.4;
            obj.Yield2 = (decimal)57.4;
            obj.Wood_chips_target = (decimal)7.0; //dado correto
            obj.Final_product_thickness_max = (decimal)5.79; //dado correto
            obj.Final_product_thickness_min = (decimal)4.21; //dado correto
            obj.Out_of_spec_target = 39; //dado correto
            obj.Porc_lsl = 20; //dado correto
            obj.Porc_usl = 10; //dado correto
            obj.Filters_on_smokehouse_exhaustion = "All Smokehouses"; //dado correto
            obj.Reprocessing_target = 1; //dado correto
            obj.Observations = "The increase in the % of water was no tested with insides during the visit (Jan 15th - 19th)";

            return obj;
        }
    }
}
