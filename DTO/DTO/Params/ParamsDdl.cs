using DTO.Helpers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DTO.DTO.Params
{
    public class ParamsDdl
    {
        public IEnumerable<SelectListItem> DdlParConsolidation { get; set; }
        public IEnumerable<SelectListItem> DdlFrequency { get; set; }
        public IEnumerable<SelectListItem> DdlparLevel1 { get; set; }
        public IEnumerable<SelectListItem> DdlparCluster { get; set; }
        public IEnumerable<SelectListItem> DdlParDepartment { get; set; }

        //[Range(0, 999, ErrorMessage = "É obrigatório selecionar o Nível do Campo.")]
        public IEnumerable<SelectListItem> DdlparLevelDefinition { get; set; }
        //[Range(0, 999, ErrorMessage = "É obrigatório selecionar o Tipo, para Nível do Campo.")]
        public IEnumerable<SelectListItem> DdlParFieldType { get; set; }



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

        public void SetDdls(List<ParConsolidationTypeDTO> ddlParConsolidation, List<ParFrequencyDTO> ddlFrequency, List<ParLevel1DTO> ddlparLevel1, 
                            List<ParClusterDTO> ddlparCluster, List<ParLevelDefinitonDTO> ddlparLevelDefinition, List<ParFieldTypeDTO> ddlParFieldType,
                            List<ParDepartmentDTO> ddrParDepartment)
        {
            DdlParConsolidation = Guard.CreateDropDownList(ddlParConsolidation);
            DdlFrequency = Guard.CreateDropDownList(ddlFrequency);
            DdlparLevel1 = Guard.CreateDropDownList(ddlparLevel1);
            DdlparCluster = Guard.CreateDropDownList(ddlparCluster);
            DdlparLevelDefinition = Guard.CreateDropDownList(ddlparLevelDefinition);
            DdlParFieldType = Guard.CreateDropDownList(ddlParFieldType);
            DdlParDepartment = Guard.CreateDropDownList(ddrParDepartment);
        }
    }
}
