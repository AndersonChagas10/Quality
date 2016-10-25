using DTO.BaseEntity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DTO.DTO.Params
{
    public class ParLevel1DTO : EntityBase
    {

        //[Display(Name = "select_the_consolidation_type", ResourceType = typeof(Resource))]
        [Range(0, 999, ErrorMessage = "É obrigatório selecionar o tipo de consolidação.")]
        public int ParConsolidationType_Id { get; set; }

        //[Display(Name = "select_the_frequency", ResourceType = typeof(Resource))]
        [Range(0, 999, ErrorMessage = "É obrigatório selecionar a frequencia.")]
        public int ParFrequency_Id { get; set; }

        //[Display(Name = "level1_name", ResourceType = typeof(Resource))]
        [Required(ErrorMessage = "O Nome deverá ter no mínimo 3 e máximo 10 caracteres.")]
        [MinLength(3, ErrorMessage = "O tamanho mínimo do Nome são 3 caracteres.")]
        [MaxLength(140, ErrorMessage = "O tamanho máximo do Nome são 10 caracteres.")]
        public string Name { get; set; }

        //[Display(Name = "level1_description", ResourceType = typeof(Resource))]
        [Required(ErrorMessage = "O Nome deverá ter no mínimo 3 e máximo 10 caracteres.")]
        [MinLength(3, ErrorMessage = "O tamanho mínimo do Nome são 3 caracteres.")]
        [MaxLength(140, ErrorMessage = "O tamanho máximo do Nome são 10 caracteres.")]
        public string Description { get; set; }

        //[Display(Name = "save_button_on_level2", ResourceType = typeof(Resource))]
        public bool HasSaveLevel2 { get; set; }

        //[Display(Name = "level1_description", ResourceType = typeof(Resource))]
        public bool HasNoApplicableLevel2 { get; set; }

        //[Display(Name = "allow_level2_group", ResourceType = typeof(Resource))]
        public bool HasGroupLevel2 { get; set; }

        //[Display(Name = "alert_emissor", ResourceType = typeof(Resource))]
        public bool HasAlert { get; set; }

        //[Display(Name = "allow_specific_register_by_unit", ResourceType = typeof(Resource))]
        public bool IsSpecific { get; set; }

        //[Display(Name = "allow_field_edition_of_the_header", ResourceType = typeof(Resource))]
        public bool IsSpecificHeaderField { get; set; }

        //[Display(Name = "allow_the_edition_of_the_evaluation_number", ResourceType = typeof(Resource))]
        public bool IsSpecificNumberEvaluetion { get; set; }

        //[Display(Name = "allow_the_edition_of_the_sample_number", ResourceType = typeof(Resource))]
        public bool IsSpecificNumberSample { get; set; }

        //[Display(Name = "?IsSpecificLevel3 ")]
        public bool IsSpecificLevel3 { get; set; }

        //[Display(Name = "allow_the_edition_of_the_goal_of_the_indicator_by_unit", ResourceType = typeof(Resource))]
        public bool IsSpecificGoal { get; set; }

        //[Display(Name = "not_conformity_rule", ResourceType = typeof(Resource))]
        public bool IsRuleConformity { get; set; }

        //[Display(Name = "?IsFixedEvaluetionNumber ")]
        public bool IsFixedEvaluetionNumber { get; set; }

        //[Display(Name = "limited_number_of_evaluations", ResourceType = typeof(Resource))]
        public bool IsLimitedEvaluetionNumber { get; set; }

        //[Display(Name = "?IsActive ", ResourceType = typeof(Resource))]
        public bool IsActive { get; set; } = true;

        public ParConsolidationTypeDTO parConsolidationTypeDto { get; set; }
        public ParFrequencyDTO parFrequencyDto { get; set; }
        public List<ParLevel1XHeaderFieldDTO> parLevel1HeaderFieldDto { get; set; }

    }
}