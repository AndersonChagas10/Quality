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
            var querySelectFlats = new RelatorioConsistencyCallMSPResultSet().SelectFlats(form);
            var querySelectInsiders = new RelatorioConsistencyCallMSPResultSet().SelectInsiders(form);
            var querySelectEyes = new RelatorioConsistencyCallMSPResultSet().SelectEyes(form);
            var querySelectTumblerBatchSize1 = new RelatorioConsistencyCallMSPResultSet().SelectTumblerBatchSize1(form);
            var querySelectTumblerBatchSize2 = new RelatorioConsistencyCallMSPResultSet().SelectTumblerBatchSize2(form);
            var querySelectTumblerBatchSize3 = new RelatorioConsistencyCallMSPResultSet().SelectTumblerBatchSize3(form);
            var querySelectTumblerBatchSize4 = new RelatorioConsistencyCallMSPResultSet().SelectTumblerBatchSize4(form);
            var querySelectProductAppearance = new RelatorioConsistencyCallMSPResultSet().SelectProductAppearance(form);
            var querySelectSeasoningDistribuition = new RelatorioConsistencyCallMSPResultSet().SelectSeasoningDistribuition(form);
            var querySelectMeatTemperatureActual1 = new RelatorioConsistencyCallMSPResultSet().SelectMeatTemperatureActual1(form);
            var querySelectMeatTemperaturaActual2 = new RelatorioConsistencyCallMSPResultSet().SelectMeatTemperaturaActual2(form);
            var querySelectThicknessAVG1_CDCM = new RelatorioConsistencyCallMSPResultSet().SelectThicknessAVG1_CDCM(form);
            var querySelectThicknessAVG2_CDCM = new RelatorioConsistencyCallMSPResultSet().SelectThicknessAVG2_CDCM(form);
            var querySelectThicknessSampleSize1_CDCM = new RelatorioConsistencyCallMSPResultSet().SelectThicknessSampleSize1_CDCM(form);
            var querySelectThicknessSampleSize2_CDCM = new RelatorioConsistencyCallMSPResultSet().SelectThicknessSampleSize2_CDCM(form);
            var querySelectMeatCanadianRequirements1 = new RelatorioConsistencyCallMSPResultSet().SelectMeatCanadianRequirements1(form);
            var querySelectMeatCanadianRequirements2 = new RelatorioConsistencyCallMSPResultSet().SelectMeatCanadianRequirements2(form);
            var querySelectWaitTimeAVG1 = new RelatorioConsistencyCallMSPResultSet().SelectWaitTimeAVG1(form);
            var querySelectWaitTimeAVG2 = new RelatorioConsistencyCallMSPResultSet().SelectWaitTimeAVG2(form);
            var querySelectMaxTime1 = new RelatorioConsistencyCallMSPResultSet().SelectMaxTime1(form);
            var querySelectMaxTime2 = new RelatorioConsistencyCallMSPResultSet().SelectMaxTime2(form);
            var querySelectPullMoistureAVG = new RelatorioConsistencyCallMSPResultSet().SelectPullMoistureAVG(form);
            var querySelectStagingRoomTemperature1 = new RelatorioConsistencyCallMSPResultSet().SelectStagingRoomTemperature1(form);
            var querySelectStagingRoomTemperature2 = new RelatorioConsistencyCallMSPResultSet().SelectStagingRoomTemperature2(form);
            var querySelectPackingWaterActivity = new RelatorioConsistencyCallMSPResultSet().SelectPackingWaterActivity(form);
            var querySelectPackingMoistureAVG = new RelatorioConsistencyCallMSPResultSet().SelectPackingMoistureAVG(form);
            var querySelectReanalysisFoss1 = new RelatorioConsistencyCallMSPResultSet().SelectReanalysisFoss1(form);
            var querySelectReanalysisFoss2 = new RelatorioConsistencyCallMSPResultSet().SelectReanalysisFoss2(form);
            var querySelectPackingRoomTempertureMax1 = new RelatorioConsistencyCallMSPResultSet().SelectPackingRoomTempertureMax1(form);
            var querySelectPackingRoomTempertureMax2 = new RelatorioConsistencyCallMSPResultSet().SelectPackingRoomTempertureMax2(form);
            var querySelectPackingRoomTemperture1 = new RelatorioConsistencyCallMSPResultSet().SelectPackingRoomTemperture1(form);
            var querySelectPackingRoomTemperture2 = new RelatorioConsistencyCallMSPResultSet().SelectPackingRoomTemperture2(form);
            var querySelectWoodChips = new RelatorioConsistencyCallMSPResultSet().SelectWoodChips(form);
            var querySelectFinalProductThicknessAVG = new RelatorioConsistencyCallMSPResultSet().SelectFinalProductThicknessAVG(form);
            var querySelectRework1 = new RelatorioConsistencyCallMSPResultSet().SelectRework1(form);
            var querySelectRework2 = new RelatorioConsistencyCallMSPResultSet().SelectRework2(form);
            var querySelectCookingFlavor = new RelatorioConsistencyCallMSPResultSet().SelectCookingFlavor(form);
            var querySelectOdor = new RelatorioConsistencyCallMSPResultSet().SelectOdor(form);
            var querySelectTexture = new RelatorioConsistencyCallMSPResultSet().SelectTexture(form);
            var querySelectAppearance = new RelatorioConsistencyCallMSPResultSet().SelectAppearance(form);


            using (Factory factory = new Factory("DefaultConnection"))
            {
                var flavor = factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectFlavor).ToList();
                obj.Flavor = flavor.ToString();

                var product = factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectProduct).ToList();
                obj.Product = product.ToString();

                var batch1 = factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectBatch1).ToList();
                obj.Batch1 = Convert.ToDecimal(batch1);

                var batch2 = factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectBatch2).ToList();
                obj.Batch2 = Convert.ToDecimal(batch2);

                var porcWater1 = factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectPorcWater1).ToList();
                var porcWater2 = factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectPorcWater2).ToList();
                var porcWater3 = factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectPorcWater3).ToList();
                var porcWater4 = factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectPorcWater4).ToList();
                obj.PorcWater1 = Convert.ToDecimal(porcWater1);
                obj.PorcWater2 = Convert.ToDecimal(porcWater2);

                var meat_age_avg = factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectMeatAgeAVG).ToList();
                obj.Meat_age_avg = Convert.ToDecimal(meat_age_avg);

                var meat_age_min_max1 = factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectMeatAgeMin).ToList();
                obj.Meat_age_min_max1 = Convert.ToDecimal(meat_age_min_max1);

                var meat_age_min_max2 = factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectMeatAgeMax).ToList();
                obj.Meat_age_min_max2 = Convert.ToDecimal(meat_age_min_max2);

                var flats = factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectFlats).ToList();
                obj.Flats = Convert.ToDecimal(flats);

                var insides = factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectInsiders).ToList();
                obj.Insides = Convert.ToDecimal(insides);

                var eyes = factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectEyes).ToList();
                obj.Eyes = Convert.ToDecimal(eyes);

                var tumbler_batch_size1 = factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectTumblerBatchSize1).ToList();
                var tumbler_batch_size2 = factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectTumblerBatchSize2).ToList();
                var tumbler_batch_size3 = factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectTumblerBatchSize3).ToList();
                var tumbler_batch_size4 = factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectTumblerBatchSize4).ToList();
                obj.Tumbler_batch_size = Convert.ToDecimal(tumbler_batch_size1);

                var product_appearance = factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectProductAppearance).ToList();
                obj.Product_appearance = product_appearance.ToString();

                var seasoning_distribuition = factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectSeasoningDistribuition).ToList();
                obj.Seasoning_distribuition = seasoning_distribuition.ToString();

                var meat_temperature_actual1 = factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectMeatTemperatureActual1).ToList();
                obj.Meat_temperature_actual1 = Convert.ToDecimal(meat_temperature_actual1);

                var meat_temperature_actual2 = factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectMeatTemperaturaActual2).ToList();
                obj.Meat_temperature_actual2 = Convert.ToDecimal(meat_temperature_actual2);

                var thickness_avg1_CDCM = factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectThicknessAVG1_CDCM).ToList();
                obj.Thickness_avg1_CDCM = Convert.ToDecimal(thickness_avg1_CDCM);

                var thickness_avg2_CDCM = factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectThicknessAVG2_CDCM).ToList();
                obj.Thickness_avg2_CDCM = Convert.ToDecimal(thickness_avg2_CDCM);

                var thickness_sample_size1_CDCM = factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectThicknessSampleSize1_CDCM).ToList();
                obj.Thickness_sample_size1_CDCM = Convert.ToDecimal(thickness_sample_size1_CDCM);

                var thickness_sample_size2_CDCM = factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectThicknessSampleSize2_CDCM).ToList();
                obj.Thickness_sample_size2_CDCM = Convert.ToDecimal(thickness_sample_size2_CDCM);

                var meet_requirements1 = factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectMeatCanadianRequirements1).ToList();
                obj.Meet_requirements1 = meet_requirements1.ToString();

                var meet_requirements2 = factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectMeatCanadianRequirements2).ToList();
                obj.Meet_requirements2 = meet_requirements2.ToString();

                var wait_time_avg1 = factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectWaitTimeAVG1).ToList();
                obj.Wait_time_avg1 = Convert.ToInt32(wait_time_avg1);

                var wait_time_avg2 = factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectWaitTimeAVG2).ToList();
                obj.Wait_time_avg2 = Convert.ToInt32(wait_time_avg2);

                var max_time1 = factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectMaxTime1).ToList();
                obj.Max_time1 = max_time1.ToString();

                var max_time2 = factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectMaxTime2).ToList();
                obj.Max_time2 = max_time2.ToString();

                var pull_moisture_avg = factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectPullMoistureAVG).ToList();
                obj.Pull_moisture_avg = Convert.ToDecimal(pull_moisture_avg);

                var staging_room_temperature1 = factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectStagingRoomTemperature1).ToList();
                obj.Staging_room_temperature1 = Convert.ToDecimal(staging_room_temperature1);

                var staging_room_temperature2 = factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectStagingRoomTemperature2).ToList();
                obj.Staging_room_temperature2 = Convert.ToDecimal(staging_room_temperature2);

                var packing_water_activity = factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectPackingWaterActivity).ToList();
                obj.Packing_water_activity = Convert.ToDecimal(packing_water_activity);

                var packing_moisture_avg = factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectPackingMoistureAVG).ToList();
                obj.Packing_moisture_avg = Convert.ToDecimal(packing_moisture_avg);

                var reanalysis_foss_1 = factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectReanalysisFoss1).ToList();
                obj.Reanalysis_foss_1 = Convert.ToDecimal(reanalysis_foss_1);

                var reanalysis_foss_2 = factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectReanalysisFoss2).ToList();
                obj.Reanalysis_foss_2 = Convert.ToDecimal(reanalysis_foss_2);

                var packing_room_temperature_max1 = factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectPackingRoomTempertureMax1).ToList();
                obj.Packing_room_temperature_max1 = Convert.ToDecimal(packing_room_temperature_max1);

                var packing_room_temperature_max2 = factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectPackingRoomTempertureMax2).ToList();
                obj.Packing_room_temperature_max2 = Convert.ToDecimal(packing_room_temperature_max2);

                var packing_room_temperature1 = factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectPackingRoomTemperture1).ToList();
                obj.Packing_room_temperature1 = Convert.ToDecimal(packing_room_temperature1);

                var packing_room_temperature2 = factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectPackingRoomTemperture2).ToList();
                obj.Packing_room_temperature2 = Convert.ToDecimal(packing_room_temperature2);

                var wood_chips = factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectWoodChips).ToList();
                obj.Wood_chips = Convert.ToDecimal(wood_chips);

                var final_product_thickness_avg = factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectFinalProductThicknessAVG).ToList();
                obj.Final_product_thickness_avg = Convert.ToDecimal(final_product_thickness_avg);

                var rework1 = factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectRework1).ToList();
                obj.Rework1 = Convert.ToDecimal(rework1);

                var rework2 = factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectRework2).ToList();
                obj.Rework2 = Convert.ToDecimal(rework2);

                var cooking_flavor = factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectCookingFlavor).ToList();
                obj.Cooking_flavor = Convert.ToDecimal(cooking_flavor);

                var odor = factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectOdor).ToList();
                obj.Odor = Convert.ToDecimal(odor);

                var texture = factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectTexture).ToList();
                obj.Texture = Convert.ToDecimal(texture);

                var appearance = factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectAppearance).ToList();
                obj.Appearance = Convert.ToDecimal(appearance);
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
