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

namespace SgqSystem.Controllers.Api.Relatorios
{
    [RoutePrefix("api/RelatorioConsistencyCallMSP")]
    public class RelatorioConsistencyCallMSPApiController : ApiController
    {
        private List<RelatorioConsistencyCallMSPResultSet> _list { get; set; }

        [HttpGet]
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
                obj.Flavor = factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectFlavor).ToString();
                obj.Product = factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectProduct).ToString();
                obj.Batch1 = Convert.ToDecimal(factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectBatch1));
                obj.Batch2 = Convert.ToDecimal(factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectBatch2));
                var porcWater1 = Convert.ToDecimal(factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectPorcWater1));
                var porcWater2 = Convert.ToDecimal(factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectPorcWater2));
                var porcWater3 = Convert.ToDecimal(factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectPorcWater3));
                var porcWater4 = Convert.ToDecimal(factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectPorcWater4));
                obj.Meat_age_avg = Convert.ToDecimal(factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectMeatAgeAVG));
                obj.Meat_age_min_max1 = Convert.ToDecimal(factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectMeatAgeMin));
                obj.Meat_age_min_max2 = Convert.ToDecimal(factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectMeatAgeMax));
                obj.Flats = Convert.ToDecimal(factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectFlats));
                obj.Insides = Convert.ToDecimal(factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectInsiders));
                obj.Eyes = Convert.ToDecimal(factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectEyes));
                var Tumbler_batch_size1 = factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectTumblerBatchSize1).ToString();
                var Tumbler_batch_size2 = factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectTumblerBatchSize2).ToString();
                var Tumbler_batch_size3 = factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectTumblerBatchSize3).ToString();
                var Tumbler_batch_size4 = factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectTumblerBatchSize4).ToString();
                obj.Product_appearance = factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectProductAppearance).ToString();
                obj.Seasoning_distribuition = factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectSeasoningDistribuition).ToString();
                obj.Meat_temperature_actual1 = Convert.ToDecimal(factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectMeatTemperatureActual1));
                obj.Meat_temperature_actual2 = Convert.ToDecimal(factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectMeatTemperaturaActual2));
                obj.Thickness_avg1_CDCM = Convert.ToDecimal(factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectThicknessAVG1_CDCM));
                obj.Thickness_avg2_CDCM = Convert.ToDecimal(factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectThicknessAVG2_CDCM));
                obj.Thickness_sample_size1_CDCM = Convert.ToDecimal(factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectThicknessSampleSize1_CDCM));
                obj.Thickness_sample_size2_CDCM = Convert.ToDecimal(factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectThicknessSampleSize2_CDCM));
                obj.Meet_requirements1 = factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectMeatCanadianRequirements1).ToString();
                obj.Meet_requirements2 = factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectMeatCanadianRequirements2).ToString();
                var meat_time_avg1 = factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectWaitTimeAVG1).ToString();
                var meat_time_avg2 = factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectWaitTimeAVG2).ToString();
                obj.Max_time1 = factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectMaxTime1).ToString();
                obj.Max_time2 = factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectMaxTime2).ToString();
                obj.Pull_moisture_avg = Convert.ToDecimal(factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectPullMoistureAVG));
                obj.Staging_room_temperature1 = Convert.ToDecimal(factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectStagingRoomTemperature1));
                obj.Staging_room_temperature2 = Convert.ToDecimal(factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectStagingRoomTemperature2));
                obj.Packing_water_activity = Convert.ToDecimal(factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectPackingWaterActivity));
                obj.Packing_moisture_avg = Convert.ToDecimal(factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectPackingMoistureAVG));
                obj.Reanalysis_foss_1 = Convert.ToDecimal(factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectReanalysisFoss1));
                obj.Reanalysis_foss_2 = Convert.ToDecimal(factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectReanalysisFoss2));
                obj.Packing_room_temperature_max1 = Convert.ToDecimal(factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectPackingRoomTempertureMax1));
                obj.Packing_room_temperature_max2 = Convert.ToDecimal(factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectPackingRoomTempertureMax2));
                obj.Packing_room_temperature1 = Convert.ToDecimal(factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectPackingRoomTemperture1));
                obj.Packing_room_temperature2 = Convert.ToDecimal(factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectPackingRoomTemperture2));
                obj.Wood_chips = Convert.ToDecimal(factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectWoodChips));
                obj.Final_product_thickness_avg = Convert.ToDecimal(factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectFinalProductThicknessAVG));
                obj.Rework1 = Convert.ToDecimal(factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectRework1));
                obj.Rework2 = Convert.ToDecimal(factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectRework2));
                obj.Cooking_flavor = Convert.ToDecimal(factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectCookingFlavor));
                obj.Odor = Convert.ToDecimal(factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectOdor));
                obj.Texture = Convert.ToDecimal(factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectTexture));
                obj.Appearance = Convert.ToDecimal(factory.SearchQuery<RelatorioConsistencyCallMSPResultSet>(querySelectAppearance));
            }

            obj.PorcWater1 = (decimal)20.5;
            obj.PorcWater2 = (decimal)18.5;
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
            obj.Yield = (decimal)57.4;
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
