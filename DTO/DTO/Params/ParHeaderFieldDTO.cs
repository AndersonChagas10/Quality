using DTO.BaseEntity;
using DTO.Helpers;
using System.Collections.Generic;
using System.Web.Mvc;

namespace DTO.DTO.Params
{
    public class ParHeaderFieldDTO : EntityBase
    {
        public int? ParFieldType_Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } = " ";
        public int ParLevelDefinition_Id { get; set; }
        public bool LinkNumberEvaluetion { get; set; }
        public bool IsActive { get; set; }

        //public ParLevelDefinitionDTO parLevelDefinitionDto { get; set; }
        public List<ParMultipleValuesDTO> parMultipleValuesDto { get; set; }

        public IEnumerable<SelectListItem> DropDownList { get; set; }
        public void SetMultipleValues()
        {
            DropDownList = Guard.CreateDropDownList(parMultipleValuesDto);
        }
    }
}