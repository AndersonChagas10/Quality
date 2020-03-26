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
        public RelatorioConsistencyCallMSPResultSet listaTabelaConsistencyCall()
        {
            RelatorioConsistencyCallMSPResultSet obj = new RelatorioConsistencyCallMSPResultSet();
            obj.Flavor = "BDJE OR";
            obj.Raw_side = "March 27, 2019";
            obj.Batch1 = 6;
            obj.Batch2 = 4;
            obj.PorcWater1 = (decimal)20.5;
            obj.PorcWater2 = (decimal)18.5;
            obj.Meat_age_target = 14;
            obj.Meat_age_min_max1 = 8;
            obj.Meat_age_min_max2 = 21;
            obj.Meat_age_avg = (obj.Meat_age_min_max2 + obj.Meat_age_min_max1) / 2;
            obj.Flats = 66;
            obj.Insides = 1;
            obj.Eyes = 33;
            obj.Tumbler_batch_size = 3977;
            obj.Product_appearance = "Good";
            obj.Seasoning_distribuition = "Good";
            obj.Meat_temperature_max1 = 34;
            obj.Meat_temperature_min1 = 32;
            obj.Meat_temperature_actual1 = 34;
            obj.Thickness_avg_max = (decimal)8.2;
            obj.Thickness_avg_min = (decimal)5.8;
            obj.Thickness_avg1 = (decimal)6.99;
            obj.Thickness_avg2 = (decimal)6.12;
            obj.Thickness_sample_size1 = 1008;
            obj.Thickness_sample_size2 = 672;
            obj.Out_spec_target = 7;
            obj.Porc_out_spec1 = (decimal)2.6;
            obj.Porc_out_spec2 = (decimal)4.2;
            obj.Porc_LSL1 = (decimal)1.3;
            obj.Porc_LSL2 = (decimal)2.0;
            obj.Porc_USL1 = (decimal)1.2;
            obj.Porc_USL2 = (decimal)2.2;

            obj.Meat_weight_inside_smokehouse_target = 4000;
            obj.Meat_weight_inside_smokehouse = (decimal)5.64;
            obj.Porc_purge_target = 5;
            obj.Porc_purge = 4;
            obj.Cooking = "March 29, 2019";
            obj.Meet_requirements1 = "YES";
            obj.Meet_requirements2 = "NA";
            obj.Marination_time_max = 76;
            obj.Marination_time_min = 44;
            obj.Marination_time = 45;
            obj.Wait_time_avg = 57;
            obj.Max_time1 = "4:50";
            obj.Max_time2 = "8:25";
            obj.Time_by_sample1 = "5:38";
            obj.Time_by_sample2 = "6:01";

            obj.Foss_used_for_pull = "Foss 1";
            obj.Foss_used_for_packing = "Foss 2";
            obj.Cooking_time_target = "4:15";
            obj.Cooking_time_avg = "3.36";
            obj.Standard_pull_moisture_max = (decimal)32.0;
            obj.Standard_pull_moisture_min = (decimal)28.0;
            obj.Pull_moisture_avg = (decimal)31.53;
            obj.Standing_room_temperature = 60;
            obj.Packing_water_activity_max = (decimal)0.850;
            obj.Packing_water_activity = (decimal)0.794;
            obj.Packing_moisture_avg_min = 28;
            obj.Packing_moisture_avg_max = 30;
            obj.Packing_moisture_avg = (decimal)29.17;
            obj.Reanalysis_foss_1 = (decimal)28.5;
            obj.Reanalysis_foss_2 = (decimal)28.8;
            obj.Alpenas_moisture = 10;
            obj.Packing_room_temperature_max = 60;
            obj.Packing_room_temperature = 54;
            obj.Yield_target = (decimal)55.6;
            obj.Yield_target_min = (decimal)50.4;
            obj.Yield = (decimal)57.4;
            obj.Wood_chips_target = (decimal)7.0;
            obj.Wood_chips = (decimal)6.88;
            obj.Final_product_thickness_avg = (decimal)5.55;
            obj.Thickness_sample_size = 480;
            obj.Out_of_spec_target = 39;
            obj.Porc_out_spec = 55;
            obj.Porc_lsl = 15;
            obj.Porc_usl = 40;
            obj.Reprocessing_target = 1;
            obj.Rework = (decimal)2.49;
            obj.Cooking_flavor = 5;
            obj.Odor = 5;
            obj.Texture = 5;
            obj.Appearance = 5;
            obj.Observations = "The increase in the % of water was no tested with insides during the visit (Jan 15th - 19th)";

            return obj;
        }
    }
}
