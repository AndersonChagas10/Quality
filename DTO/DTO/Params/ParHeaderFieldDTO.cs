using DTO.BaseEntity;
using DTO.Helpers;
using System;
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
        public string DefaultSelected { get; set; }
        public Nullable<bool> IsRequired { get; set; }

        //public ParLevelDefinitionDTO parLevelDefinitionDto { get; set; }
        /*Inclusão*/
        public List<ParMultipleValuesDTO> parMultipleValuesDto { get; set; }

        /*Alteracao*/
        public List<ParMultipleValuesDTO> ParMultipleValues { get; set; }

        public ParLevelDefinitonDTO ParLevelDefiniton { get; set; }

        public ParFieldTypeDTO ParFieldType { get; set; }

        public IEnumerable<SelectListItem> _DropDownList;

        public IEnumerable<SelectListItem> DropDownList
        {
            get
            {
                if (ParMultipleValues == null)
                    return _DropDownList;
                else
                    return Guard.CreateDropDownList(ParMultipleValues);
            }
            set
            {
                DropDownList = value;
            }
        }

    }
}