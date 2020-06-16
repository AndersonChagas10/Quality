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
                        if (item.Tarefa == "2009" && cabecalho.ToUpper().Contains("MATÉRIA-PRIMA/ RAW MATERIAL"))
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

                obj.Batch1 = retornoApontamentosDiarios.Where(x => x.Tarefa == "1900"
                    && x.Turno.Trim() == "1").Count();
                obj.Batch2 = retornoApontamentosDiarios.Where(x => x.Tarefa == "1900"
                    && x.Turno.Trim() == "2").Count();

                var pesoCarneT1 = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == "1868"
                    && x.Turno.Trim() == "1").Sum(x => Convert.ToDecimal(x.Value, new CultureInfo("en-US")));
                var pesoCarneT2 = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == "1868"
                    && x.Turno.Trim() == "2").Sum(x => Convert.ToDecimal(x.Value, new CultureInfo("en-US")));
                var volumeAguaT1 = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == "1867"
                    && x.Turno.Trim() == "1").Sum(x => Convert.ToDecimal(x.Value, new CultureInfo("en-US")));
                var volumeAguaT2 = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == "1867"
                    && x.Turno.Trim() == "2").Sum(x => Convert.ToDecimal(x.Value, new CultureInfo("en-US")));

                if (volumeAguaT1 > 0 && pesoCarneT1 > 0)
                    obj.PorcWater1 = (volumeAguaT1 / pesoCarneT1) * 100;
                if (volumeAguaT2 > 0 && pesoCarneT2 > 0)
                    obj.PorcWater2 = (volumeAguaT2 / pesoCarneT2) * 100;

                obj.Meat_age_min_max1 = retornoApontamentosDiarios.Where(
                    x => x.Tarefa.Trim() == "2009").Min(x => Convert.ToDecimal(x.Value, new CultureInfo("en-US")));
                obj.Meat_age_min_max2 = retornoApontamentosDiarios.Where(
                    x => x.Tarefa.Trim() == "2009").Max(x => Convert.ToDecimal(x.Value, new CultureInfo("en-US")));

                obj.Meat_age_avg = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == "2009").Average(x => Convert.ToDecimal(x.Value, new CultureInfo("en-US")));

                var totalAmostras = qtdFlats + qtdInsides + qtdEyes;
                if (totalAmostras > 0)
                {
                    obj.Flats = (qtdFlats / totalAmostras) * 100;
                    obj.Insides = (qtdInsides / totalAmostras) * 100;
                    obj.Eyes = (qtdEyes / totalAmostras) * 100;
                }
                
                obj.Tumbler_batch_size = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == "1868").Sum(x => Convert.ToDecimal(x.Value, new CultureInfo("en-US")));

                obj.Meat_temperature_actual1 = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == "1851").Average(x => Convert.ToDecimal(x.Value, new CultureInfo("en-US")));
                obj.Meat_temperature_actual2 = (obj.Meat_temperature_actual1 * 9/5) + 32;

                obj.Thickness_avg1_CDCM = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == "1848" && x.Turno.Trim() == "1").Average(x => Convert.ToDecimal(x.Value, new CultureInfo("en-US")));
                obj.Thickness_avg2_CDCM = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == "1848" && x.Turno.Trim() == "2").Average(x => Convert.ToDecimal(x.Value, new CultureInfo("en-US")));
                obj.Thickness_sample_size1_CDCM = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == "1848" && x.Turno.Trim() == "1").Count();
                obj.Thickness_sample_size2_CDCM = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == "1848" && x.Turno.Trim() == "2").Count();

                obj.Thickness_avg1_BL = retornoApontamentosDiarios.Where(
                    x => (x.Tarefa == "1849" || x.Tarefa == "1850")
                    && x.Turno.Trim() == "1").Average(x => Convert.ToDecimal(x.Value, new CultureInfo("en-US")));
                obj.Thickness_avg2_BL = retornoApontamentosDiarios.Where(
                    x => (x.Tarefa == "1849" || x.Tarefa == "1850")
                    && x.Turno.Trim() == "2").Average(x => Convert.ToDecimal(x.Value, new CultureInfo("en-US")));
                obj.Thickness_sample_size1_BL = retornoApontamentosDiarios.Where(
                    x => (x.Tarefa == "1849" || x.Tarefa == "1850") && x.Turno.Trim() == "1").Count();
                obj.Thickness_sample_size2_BL = retornoApontamentosDiarios.Where(
                    x => (x.Tarefa == "1849" || x.Tarefa == "1850") && x.Turno.Trim() == "2").Count();

                decimal meetConformes1 = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == "1947" && x.Conforme == "True" && x.Turno.Trim() == "1").Count();
                decimal meetTotal1 = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == "1947" && x.Turno.Trim() == "1").Count();
                if (meetTotal1 > 0)
                    obj.Meet_requirements1 = (meetConformes1 / meetTotal1) * 100;

                decimal meetConformes2 = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == "1947" && x.Conforme == "True" && x.Turno.Trim() == "2").Count();
                decimal meetTotal2 = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == "1947" && x.Turno.Trim() == "2").Count();
                if (meetTotal2 > 0)
                    obj.Meet_requirements2 = (meetConformes2 / meetTotal2) * 100;

                obj.Marination_time = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == "1908").Average(x => Convert.ToDecimal(x.Value, new CultureInfo("en-US")));

                var waitTimeAvg1 = retornoApontamentosDiarios.Where(x => x.Tarefa == "1956" && x.Turno.Trim() == "1");
                if (waitTimeAvg1.Count() > 0)
                    obj.Wait_time_avg1 = waitTimeAvg1.Average(x => Convert.ToDecimal(x.Value, new CultureInfo("en-US")));

                var waitTimeAvg2 = retornoApontamentosDiarios.Where(x => x.Tarefa == "1956" && x.Turno.Trim() == "2");
                if (waitTimeAvg2.Count() > 0)
                    obj.Wait_time_avg2 = waitTimeAvg2.Average(x => Convert.ToDecimal(x.Value, new CultureInfo("en-US")));

                var maxTime1 = retornoApontamentosDiarios.Where(x => x.Tarefa == "1956" && x.Turno.Trim() == "1");
                if (maxTime1.Count() > 0)
                    obj.Max_time1 = maxTime1.Max(x => Convert.ToDecimal(x.Value, new CultureInfo("en-US")));

                var maxTime2 = retornoApontamentosDiarios.Where(x => x.Tarefa == "1956" && x.Turno.Trim() == "2");
                if (maxTime2.Count() > 0)
                    obj.Max_time2 = maxTime2.Max(x => Convert.ToDecimal(x.Value, new CultureInfo("en-US")));

                decimal foss1Conforme = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == "1999" && x.Conforme == "True").Count();
                decimal foss1Total = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == "1999").Count();
                if (foss1Total > 0)
                    obj.Foss_used_for_pull = (foss1Conforme / foss1Total) * 100;

                decimal foss2Conforme = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == "2000" && x.Conforme == "True").Count();
                decimal foss2Total = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == "2000").Count();
                if (foss2Total > 0)
                    obj.Foss_used_for_packing = (foss2Conforme / foss2Total) * 100;

                obj.Pull_moisture_avg = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == "1948").Average(x => Convert.ToDecimal(x.Value, new CultureInfo("en-US")));

                obj.Staging_room_temperature1 = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == "1954").Average(x => Convert.ToDecimal(x.Value, new CultureInfo("en-US")));
                obj.Staging_room_temperature2 = (obj.Staging_room_temperature1 * 9 / 5) + 32;

                obj.Packing_water_activity = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == "1946").Average(x => Convert.ToDecimal(x.Value, new CultureInfo("en-US")));

                obj.Packing_moisture_avg = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == "1957").Average(x => Convert.ToDecimal(x.Value, new CultureInfo("en-US")));

                obj.Reanalysis_foss_1 = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == "1883").Average(x => Convert.ToDecimal(x.Value, new CultureInfo("en-US")));

                obj.Reanalysis_foss_2 = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == "1957").Average(x => Convert.ToDecimal(x.Value, new CultureInfo("en-US")));

                obj.Packing_room_temperature_max1 = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == "2001").Max(x => Convert.ToDecimal(x.Value, new CultureInfo("en-US")));
                obj.Packing_room_temperature_max2 = (obj.Packing_room_temperature_max1 * 9 / 5) + 32;

                obj.Packing_room_temperature1 = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == "2001").Average(x => Convert.ToDecimal(x.Value, new CultureInfo("en-US")));
                obj.Packing_room_temperature2 = (obj.Packing_room_temperature1 * 9 / 5) + 32;

                obj.Wood_chips = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == "1941").Average(x => Convert.ToDecimal(x.Value, new CultureInfo("en-US")));

                obj.Final_product_thickness_avg = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == "1967").Average(x => Convert.ToDecimal(x.Value, new CultureInfo("en-US")));
                obj.Thickness_sample_size = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == "1967").Count();

                obj.Cooking_flavor = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == "2002").Average(x => Convert.ToDecimal(x.Value, new CultureInfo("en-US")));

                obj.Odor = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == "2003").Average(x => Convert.ToDecimal(x.Value, new CultureInfo("en-US")));

                obj.Texture = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == "2004").Average(x => Convert.ToDecimal(x.Value, new CultureInfo("en-US")));
                
                obj.Appearance = retornoApontamentosDiarios.Where(
                    x => x.Tarefa == "2005").Average(x => Convert.ToDecimal(x.Value, new CultureInfo("en-US")));

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
