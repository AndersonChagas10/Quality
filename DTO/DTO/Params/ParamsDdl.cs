using DTO.Helpers;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using ADOFactory;
using System.Data.SqlClient;

namespace DTO.DTO.Params
{
    public class ParamsDdl
    {
       

        public IEnumerable<SelectListItem> DdlParConsolidation { get; set; }
        public IEnumerable<SelectListItem> DdlFrequency { get; set; }
        public IEnumerable<SelectListItem> DdlparLevel1 { get; set; }
        public IEnumerable<SelectListItem> DdlparLevel2 { get; set; }
        public IEnumerable<SelectListItem> DdlparLevel3 { get; set; }
        public IEnumerable<SelectListItem> DdlparCluster { get; set; }
        public IEnumerable<SelectListItem> DdlparCrit { get; set; }
        public IEnumerable<SelectListItem> DdlScoretype { get; set; }
        
        public IEnumerable<SelectListItem> DdlParDepartment { get; set; }

        //[Range(0, 999, ErrorMessage = "É obrigatório selecionar o Nível do Campo.")]
        public IEnumerable<SelectListItem> DdlparLevelDefinition { get; set; }
        //[Range(0, 999, ErrorMessage = "É obrigatório selecionar o Tipo, para Nível do Campo.")]
        public IEnumerable<SelectListItem> DdlParFieldType { get; set; }

        public IEnumerable<SelectListItem> DdlParCounter_Level1 { get; set; }
        public IEnumerable<SelectListItem> DdlParCounter_Level2 { get; set; }
        public IEnumerable<SelectListItem> DdlParLocal_Level1 { get; set; }
        public IEnumerable<SelectListItem> DdlParLocal_Level2 { get; set; }

        public IEnumerable<SelectListItem> DdlParNotConformityRule { get; set; }

        public IEnumerable<SelectListItem> DdlParLevel3InputType { get; set; }
        public IEnumerable<SelectListItem> DdlParMeasurementUnit { get; set; }
        public IEnumerable<SelectListItem> DdlParLevel3BoolFalse { get; set; }
        public IEnumerable<SelectListItem> DdlParLevel3BoolTrue { get; set; }

        public IEnumerable<SelectListItem> DdlParCompany { get; set; }
        public IEnumerable<SelectListItem> DdlEquipamentos { get; set; }
        public IEnumerable<SelectListItem> DdlCamaras { get; set; }
        public IEnumerable<SelectListItem> DdlPontosDeColeta { get; set; }

        private List<SelectListItem> CreateSelectListParamsViewModelListLevel<T>(IEnumerable<T> enumerable)
        {
            List<SelectListItem> retorno = new List<SelectListItem>();
            var defaultOption = GlobalConfig.PrimeiraOption;
            retorno.Insert(0, new SelectListItem() { Text = defaultOption, Value = "-1" });
            var counter = 1;
            foreach (var i in enumerable)
            {
                var text = i.GetType().GetProperty("Name") ?? i.GetType().GetProperty("Description");
                var prop = i.GetType().GetProperty("Id");
                retorno.Insert(counter, new SelectListItem() { Text = prop.GetValue(i).ToString() + " - " + text.GetValue(i).ToString(), Value = prop.GetValue(i).ToString() });
                counter++;
            }

            return retorno;
        }

        public void SetDdls(List<ParConsolidationTypeDTO> ddlParConsolidation, 
                            List<ParFrequencyDTO> ddlFrequency,
                            //List<ParLevel1DTO> ddlparLevel1, 
                            //List<ParLevel2DTO> ddlparLevel2, 
                            //List<ParLevel3DTO> ddlparLevel3, 
                            List<ParClusterDTO> ddlparCluster,
                            List<ParLevelDefinitonDTO> ddlparLevelDefinition,
                            List<ParFieldTypeDTO> ddlParFieldType,
                            List<ParDepartmentDTO> ddlParDepartment,
                            List<ParCounterDTO> ddlParCounter_Level1,
                            List<ParLocalDTO> ddlParLocal_Level1,
                            List<ParCounterDTO> ddlParCounter_Level2,
                            List<ParLocalDTO> ddlParLocal_Level2,
                            List<ParNotConformityRuleDTO> ddlParNotConformityRule,
                            List<ParLevel3InputTypeDTO> ddlParLevel3InptType,
                            List<ParMeasurementUnitDTO> ddlParMeasurementUnit,
                            List<ParLevel3BoolFalseDTO> ddlParLevel3BoolFalse,
                            List<ParLevel3BoolTrueDTO> ddlParLevel3BoolTrue,
                            List<ParCriticalLevelDTO> ddlparCrit,
                            List<ParCompanyDTO> ddlParCompany,
                             List<ParScoreTypeDTO> ddlScoretype
                            )
        {
           

            DdlParCompany = Guard.CreateDropDownList(ddlParCompany.OrderBy(r => r.Name));
            DdlParConsolidation = Guard.CreateDropDownList(ddlParConsolidation.Where(r => r.IsActive == true).OrderBy(r => r.Name));
            DdlFrequency = Guard.CreateDropDownList(ddlFrequency.OrderBy(r => r.Name));
            //DdlparLevel1 = Guard.CreateDropDownList(ddlparLevel1);
            //DdlparLevel2 = Guard.CreateDropDownList(ddlparLevel2);
            //DdlparLevel3 = Guard.CreateDropDownList(ddlparLevel3);
            DdlparCluster = Guard.CreateDropDownList(ddlparCluster.OrderBy(r => r.Name));
            DdlparLevelDefinition = Guard.CreateDropDownList(ddlparLevelDefinition.OrderBy(r => r.Name));

            DdlParFieldType = Guard.CreateDropDownList(ddlParFieldType.OrderBy(r => r.Name));

            if (GlobalConfig.Eua)
            {
                DdlParFieldType = Guard.CreateDropDownList(ddlParFieldType.Where(r => r.Id != 2).OrderBy(r => r.Name));
            }

            DdlParDepartment = Guard.CreateDropDownList(ddlParDepartment.OrderBy(r => r.Name));
            DdlParCounter_Level1 = Guard.CreateDropDownList(ddlParCounter_Level1.OrderBy(r => r.Name));
            DdlParLocal_Level1 = Guard.CreateDropDownList(ddlParLocal_Level1.OrderBy(r => r.Name));
            DdlParCounter_Level2 = Guard.CreateDropDownList(ddlParCounter_Level2.OrderBy(r => r.Name));
            DdlParLocal_Level2 = Guard.CreateDropDownList(ddlParLocal_Level2.OrderBy(r => r.Name));
            DdlParNotConformityRule = Guard.CreateDropDownList(ddlParNotConformityRule.OrderBy(r => r.Name));
            DdlParLevel3InputType = Guard.CreateDropDownList(ddlParLevel3InptType.OrderBy(r => r.Name));
            DdlParMeasurementUnit = Guard.CreateDropDownList(ddlParMeasurementUnit.OrderBy(r => r.Name));
            DdlParLevel3BoolFalse = Guard.CreateDropDownList(ddlParLevel3BoolFalse.OrderBy(r => r.Name));
            DdlParLevel3BoolTrue = Guard.CreateDropDownList(ddlParLevel3BoolTrue.OrderBy(r => r.Name));
            DdlparCrit = Guard.CreateDropDownList(ddlparCrit.OrderBy(r => r.Name));

            if (GlobalConfig.Brasil)
            {
                DdlEquipamentos = CreateSelectListEquipamentos("Equipamento");
                DdlCamaras = CreateSelectListEquipamentos("Câmara");
                DdlPontosDeColeta = CreateSelectListEquipamentos("Ponto de Coleta");
            }

            DdlScoretype = Guard.CreateDropDownList(ddlScoretype.OrderBy(r => r.Name));

        }

        public void SetDdlsNivel123(List<ParLevel1DTO> ddlparLevel1, List<ParLevel2DTO> ddlparLevel2, List<ParLevel3DTO> ddlparLevel3)
        {

            DdlparLevel1 = CreateSelectListParamsViewModelListLevel(ddlparLevel1.OrderBy(r => r.Name));
            DdlparLevel2 = CreateSelectListParamsViewModelListLevel(ddlparLevel2.OrderBy(r => r.Name));
            DdlparLevel3 = CreateSelectListParamsViewModelListLevel(ddlparLevel3.OrderBy(r => r.Name));

            DdlparLevel1 = DdlparLevel1.OrderBy(r => r.Group);
            DdlparLevel2 = DdlparLevel2.OrderBy(r => r.Group);
            DdlparLevel3 = DdlparLevel3.OrderBy(r => r.Group);

        }

        private List<SelectListItem> CreateSelectListParamsViewModelListLevel<T>(List<T> lista)
        {

            List<SelectListItem> retorno = new List<SelectListItem>();
            retorno.Insert(0, new SelectListItem() { Text = GlobalConfig.PrimeiraOption, Value = "-1" });
            var counter = 1;


            var group = new SelectListGroup() { Name = GlobalConfig.NaoVinculado/*"Não vinculado:"*/ };
            var groupSelecionado = new SelectListGroup();
            foreach (var i in lista)
            {

                var text = i.GetType().GetProperty("Name").GetValue(i) ?? i.GetType().GetProperty("Description").GetValue(i);
                var prop = i.GetType().GetProperty("Id").GetValue(i);
                var opt = new SelectListItem() { Text = prop.ToString() + " - " + text, Value = prop.ToString() };
                //if (listParLevel3Level2Dto.Where(r => r.ParLevel3_Id == i.Id).Count() > 0)
                //{
                //    groupSelecionado.Name = "Vinculado: " + listParLevel3Level2Dto.FirstOrDefault(r => r.ParLevel3_Id == i.Id).ParLevel2.Name;
                //    opt.Group = groupSelecionado;
                //}
                //else
                //{
                //    opt.Group = group;
                //}

                retorno.Insert(counter, opt);

                counter++;
            }

            return retorno;
        }

        private IEnumerable<SelectListItem> CreateSelectListEquipamentos(string tipo)
        {
            IEnumerable<SelectListItem> retorno;

            var context = "SGQ_GlobalADO";
            
            using (var db = new Factory(context))
            {
                string query = "SELECT                                                      "+
                                "Tipo + '|' + ISNULL(Subtipo, '') as Value,          "+
                                "Tipo + ' - ' + ISNULL(Subtipo, 'Todos') as Text            "+
                                "FROM Equipamentos WHERE Tipo = '"+ tipo + "' GROUP BY Tipo, Subtipo ";

                retorno = db.SearchQuery<SelectListItem>(query);
            }
            
            return retorno;
        }

    }
}
