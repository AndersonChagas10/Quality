using DTO.BaseEntity;
using System.Collections.Generic;

namespace DTO.DTO.Params
{
    public class ParHeaderFieldDTO : EntityBase
    {
        public int ParFieldType_Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } = "";
        public int ParLevelDefinition_Id { get; set; }
        public bool LinkNumberEvaluetion { get; set; }
        public bool IsActive { get; set; }

        //public ParLevelDefinitionDTO parLevelDefinitionDto { get; set; }
        public List<ParMultipleValuesDTO> parMultipleValuesDto { get; set; }

    }
}