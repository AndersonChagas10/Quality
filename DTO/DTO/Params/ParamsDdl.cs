using DTO.Helpers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System;
using System.Linq;

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

        private List<SelectListItem> CreateSelectListParamsViewModelListLevel<T>(IEnumerable<T> enumerable)
        {
            List<SelectListItem> retorno = new List<SelectListItem>();
            retorno.Insert(0, new SelectListItem() { Text = "Selecione...", Value = "-1" });
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
                            List<ParLevel3BoolTrueDTO> ddlParLevel3BoolTrue)
        {
            DdlParConsolidation = Guard.CreateDropDownList(ddlParConsolidation);
            DdlFrequency = Guard.CreateDropDownList(ddlFrequency);
            //DdlparLevel1 = Guard.CreateDropDownList(ddlparLevel1);
            //DdlparLevel2 = Guard.CreateDropDownList(ddlparLevel2);
            //DdlparLevel3 = Guard.CreateDropDownList(ddlparLevel3);
            DdlparCluster = Guard.CreateDropDownList(ddlparCluster);
            DdlparLevelDefinition = Guard.CreateDropDownList(ddlparLevelDefinition);
            DdlParFieldType = Guard.CreateDropDownList(ddlParFieldType);
            DdlParDepartment = Guard.CreateDropDownList(ddlParDepartment);
            DdlParCounter_Level1 = Guard.CreateDropDownList(ddlParCounter_Level1);
            DdlParLocal_Level1 = Guard.CreateDropDownList(ddlParLocal_Level1);
            DdlParCounter_Level2 = Guard.CreateDropDownList(ddlParCounter_Level2);
            DdlParLocal_Level2 = Guard.CreateDropDownList(ddlParLocal_Level2);
            DdlParNotConformityRule = Guard.CreateDropDownList(ddlParNotConformityRule);
            DdlParLevel3InputType = Guard.CreateDropDownList(ddlParLevel3InptType);
            DdlParMeasurementUnit = Guard.CreateDropDownList(ddlParMeasurementUnit);
            DdlParLevel3BoolFalse = Guard.CreateDropDownList(ddlParLevel3BoolFalse);
            DdlParLevel3BoolTrue = Guard.CreateDropDownList(ddlParLevel3BoolTrue);
        }

        public void SetDdlsNivel123(List<ParLevel1DTO> ddlparLevel1, List<ParLevel2DTO> ddlparLevel2, List<ParLevel3DTO> ddlparLevel3)
        {
            DdlparLevel1 = CreateSelectListParamsViewModelListLevel(ddlparLevel1);
            DdlparLevel2 = CreateSelectListParamsViewModelListLevel(ddlparLevel2);
            DdlparLevel3 = CreateSelectListParamsViewModelListLevel(ddlparLevel3);
        }

        private List<SelectListItem> CreateSelectListParamsViewModelListLevel<T>(List<T> lista)
        {

            List<SelectListItem> retorno = new List<SelectListItem>();
            retorno.Insert(0, new SelectListItem() { Text = "Selecione...", Value = "-1" });
            var counter = 1;


            var group = new SelectListGroup() { Name = "Não vinculados:" };
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

    }
}
