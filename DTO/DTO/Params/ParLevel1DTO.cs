using DTO.BaseEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace DTO.DTO.Params
{
    public class ParLevel1DTO : EntityBase
    {
        public List<ParLevel2DTO> listParLevel2Colleta;

        //[Display(Name = "select_the_consolidation_type", ResourceType = typeof(Resource))]
        [Range(0, 9999999999, ErrorMessage = "É obrigatório selecionar o tipo de consolidação.")]
        public int ParConsolidationType_Id { get; set; }

        //[Display(Name = "select_the_frequency", ResourceType = typeof(Resource))]
        [Range(0, 9999999999, ErrorMessage = "É obrigatório selecionar a frequencia.")]
        public int ParFrequency_Id { get; set; }

        //[Display(Name = "level1_name", ResourceType = typeof(Resource))]
        //[Required(ErrorMessage = "O Nome deverá ter no mínimo 3 e máximo 10 caracteres.")]
        [MinLength(3, ErrorMessage = "O tamanho mínimo do Nome são 3 caracteres.")]
        [MaxLength(140, ErrorMessage = "O tamanho máximo do Nome são 140 caracteres.")]
        public string Name { get; set; }

        //[Display(Name = "level1_description", ResourceType = typeof(Resource))]
        [Required(ErrorMessage = "A Descrição deverá ter no mínimo 0 e máximo 10 caracteres.")]
        [MinLength(0, ErrorMessage = "O tamanho mínimo da Descrição deve ser 3 caracteres.")]
        [MaxLength(140, ErrorMessage = "O tamanho máximo da Descrição deve ser no 140 caracteres.")]
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

        public bool haveRealTimeConsolidation { get; set; }

        public Nullable<int> RealTimeConsolitationUpdate { get; set; }

        public List<ParLevel2ControlCompanyDTO> listLevel2CorporativosObj { get; set; }

        public ParFrequencyDTO parFrequencyDto { get; set; }
        public ParConsolidationTypeDTO parConsolidationTypeDto { get; set; }
        public ParNotConformityRuleXLevelDTO parNotConformityRuleXLevelDto { get; set; }
        public List<ParLevel1XHeaderFieldDTO> parLevel1HeaderFieldDto { get; set; }
        public List<ParNotConformityRuleXLevelDTO> listParNotConformityRuleXLevelDto { get; set; }

        #region Props utilizadas para alteração

        public List<ParLevel1XHeaderFieldDTO> cabecalhosInclusos { get; set; }
        
        public List<ParCounterXLocalDTO> contadoresIncluidos { get; set; }
        public List<ParRelapseDTO> listParRelapseDto { get; set; }

        /*Props novas*/
        public List<ParLevel1XClusterDTO> listLevel1XClusterDto { get; set; }
        public List<ParLevel3Level2Level1DTO> listParLevel3Level2Level1Dto { get; set; }
        public List<ParCounterXLocalDTO> listParCounterXLocal { get; set; }
        public List<ParGoalDTO> listParGoalLevel1 { get; set; }

        public IEnumerable<SelectListItem> DdlLevel2Vinculados { get; set; }
        
        public void CreateSelectListParamsViewModelListLevel(List<ParLevel2DTO> listLevel2, List<ParLevel3Level2Level1DTO> listParLevel3Level2Level1Dto)
        {

            List<SelectListItem> retorno = new List<SelectListItem>();
            retorno.Insert(0, new SelectListItem() { Text = "Selecione...", Value = "-1" });
            var counter = 1;


            var group = new SelectListGroup() { Name = "Não vinculados:" };
            var groupSelecionado = new SelectListGroup();
            foreach (var i in listLevel2)
            {
                var text = i.Name;
                var prop = i.Id;
                var opt = new SelectListItem() { Text = i.Id.ToString() + " - " + i.Name, Value = i.Id.ToString() };
                if (listParLevel3Level2Level1Dto.Where(r => r.ParLevel3Level2.ParLevel2_Id == i.Id).Count() > 0)
                {
                    groupSelecionado.Name = "Vinculado:";
                    opt.Group = groupSelecionado;
                }
                else
                {
                    opt.Group = group;
                }

                retorno.Insert(counter, opt);

                counter++;
            }

            DdlLevel2Vinculados = retorno;
        }

        #endregion

        #region Control Company 

        public List<int> removerParHeaderField { get; set; }
        public List<int> listLevel2Corporativos { get; set; }
        public List<int> level2PorCompany { get; set; }
        public Nullable<int> level2Number { get; set; }
        public Nullable<int> CompanyControl_Id { get; set; }

        #endregion

        public bool IsRequiredMultipleValue { get; set; }

    }
}