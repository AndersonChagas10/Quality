using DTO.BaseEntity;
using Resources;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DTO.DTO.Params
{
    public class ParLevel1DTO : EntityBase
    {

        [Range(0, 999, ErrorMessage = "É obrigatório selecionar o tipo de consolidação.")]
        public int ParConsolidationType_Id { get; set; }

        [Range(0, 999, ErrorMessage = "É obrigatório selecionar a frequencia.")]
        public int ParFrequency_Id { get; set; }

        //[Display(Name = "level1_name", ResourceType = typeof(Resource))]
        [Required(ErrorMessage = "O Nome deverá ter no mínimo 3 e máximo 10 caracteres.")]
        [MinLength(3, ErrorMessage = "O tamanho mínimo do Nome são 3 caracteres.")]
        [MaxLength(140, ErrorMessage = "O tamanho máximo do Nome são 10 caracteres.")]
        public string Name { get; set; }
        
        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "O Nome deverá ter no mínimo 3 e máximo 10 caracteres.")]
        [MinLength(3, ErrorMessage = "O tamanho mínimo do Nome são 3 caracteres.")]
        [MaxLength(140, ErrorMessage = "O tamanho máximo do Nome são 10 caracteres.")]
        public string Description { get; set; }

        public bool HasSaveLevel2 { get; set; }
        public bool HasNoApplicableLevel2 { get; set; }
        public bool HasGroupLevel2 { get; set; }
        public bool HasAlert { get; set; }
        public bool IsSpecific { get; set; }
        public bool IsSpecificHeaderField { get; set; }
        public bool IsSpecificNumberEvaluetion { get; set; }
        public bool IsSpecificNumberSample { get; set; }
        public bool IsSpecificLevel3 { get; set; }
        public bool IsSpecificGoal { get; set; }
        public bool IsRuleConformity { get; set; }
        public bool IsFixedEvaluetionNumber { get; set; }
        public bool IsLimitedEvaluetionNumber { get; set; }
        public bool IsActive { get; set; } = true;

        public ParConsolidationTypeDTO parConsolidationTypeDto { get; set; }
        public ParFrequencyDTO parFrequencyDto { get; set; }
        public List<ParLevel1XHeaderFieldDTO> parLevel1HeaderFieldDto { get; set; }

    }
}