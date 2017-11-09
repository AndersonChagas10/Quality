using DTO.BaseEntity;
using DTO.Helpers;
using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace DTO.DTO.Params
{
    public class ParLevel1DTO : EntityBase
    {

        public bool AllowAddLevel3 { get; set; }
        public bool AllowEditWeightOnLevel3 { get; set; }
        public bool AllowEditPatternLevel3Task { get; set; }

        public List<ParLevel2DTO> listParLevel2Colleta;

        //[Display(Name = "select_the_consolidation_type", ResourceType = typeof(Resource))]
        //[Range(0, 9999999999, ErrorMessage = "É obrigatório selecionar o tipo de consolidação.")]
        [Range(0, 9999999999, ErrorMessageResourceName = "select_the_colosolidation_type", ErrorMessageResourceType = typeof(Resource))]
        public int ParConsolidationType_Id { get; set; }

        //[Display(Name = "select_the_frequency", ResourceType = typeof(Resource))]
        //[Range(0, 9999999999, ErrorMessage = "É obrigatório selecionar a frequencia.")]
        [Range(0, 9999999999, ErrorMessageResourceName = "select_the_frequency", ErrorMessageResourceType = typeof(Resource))]
        public int ParFrequency_Id { get; set; }

        //[Display(Name = "level1_name", ResourceType = typeof(Resource))]
        //[Required(ErrorMessage = "O Nome deverá ter no mínimo 3 e máximo 10 caracteres.")]
        //[MinLength(3, ErrorMessage = "O tamanho mínimo do Nome são 3 caracteres.")]
        //[MaxLength(300, ErrorMessage = "O tamanho máximo do Nome são 300 caracteres.")]
        [MinLength(3, ErrorMessageResourceName = "minimum_name_3_characteres", ErrorMessageResourceType = typeof(Resource))]
        [MaxLength(300, ErrorMessageResourceName = "maximum_name_300_characteres", ErrorMessageResourceType = typeof(Resource))]
        public string Name { get; set; }

        //[Display(Name = "level1_description", ResourceType = typeof(Resource))]
        //[Required(ErrorMessage = "A Descrição deverá ter no mínimo 0 e máximo 10 caracteres.")]
        //[MinLength(1, ErrorMessage = "O tamanho mínimo da Descrição deve ser 1 caracter.")]
        //[MaxLength(300, ErrorMessage = "O tamanho máximo da Descrição são 300 caracteres.")]
        [Required(ErrorMessageResourceName = "description_has_between_3_and_10", ErrorMessageResourceType = typeof(Resource))]
        [MinLength(1, ErrorMessageResourceName = "minimum_description_1_characteres", ErrorMessageResourceType = typeof(Resource))]
        [MaxLength(300, ErrorMessageResourceName = "maximum_description_300_characteres", ErrorMessageResourceType = typeof(Resource))]
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
        public bool ShowScorecard { get; set; } = true;
        
        public bool HasTakePhoto { get; set; } = false;
        public bool IsPartialSave { get; set; } = false;

        public bool haveRealTimeConsolidation { get; set; }
        public bool HasCompleteEvaluation { get; set; }
        public bool EditLevel2 { get; set; }
        public Nullable<int> RealTimeConsolitationUpdate { get; set; }

        public bool IsChildren { get; set; }

        private Nullable<int> parLevel1Origin_Id;
        public Nullable<int> ParLevel1Origin_Id
        {
            get
            {
                if (IsChildren)
                    return parLevel1Origin_Id;
                else
                    return null;
            }
            set
            {
                parLevel1Origin_Id = value;
            }
        }
        private bool pointsDestiny;
        public bool PointsDestiny
        {
            get
            {
                if (IsChildren)
                    return pointsDestiny;
                else
                    return false;
            }
            set
            {
                pointsDestiny = value;
            }
        }

        private Nullable<int> parLevel1Destiny_Id;
        public Nullable<int> ParLevel1Destiny_Id
        {
            get
            {
                if (IsChildren)
                    return parLevel1Destiny_Id;
                else
                    return null;
            }
            set
            {
                parLevel1Destiny_Id = value;
            }
        }

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
            retorno.Insert(0, new SelectListItem() { Text = GlobalConfig.PrimeiraOption, Value = "-1" });
            var counter = 1;


            var group = new SelectListGroup() { Name = (GlobalConfig.Eua || GlobalConfig.Canada) ? "Unlinked" : "Não vinculado:" };
            var groupSelecionado = new SelectListGroup();
            //listParLevel3Level2Level1Dto = listParLevel3Level2Level1Dto.OrderBy(r => listLevel2.Any(ll2 => ll2.Id == r.ParLevel3Level2.ParLevel2_Id)).ToList();


            foreach (var i in listLevel2)
            {

                var text = i.Name;
                var prop = i.Id;
                var opt = new SelectListItem() { Text = i.Id.ToString() + " - " + i.Name, Value = i.Id.ToString() };

                if (listParLevel3Level2Level1Dto.Where(r => r.ParLevel3Level2.ParLevel2_Id == i.Id).Count() > 0)
                {
                    groupSelecionado.Name = (GlobalConfig.Eua || GlobalConfig.Canada) ? "Linked" : "Vinculado:";
                    opt.Group = groupSelecionado;
                    retorno.Insert(counter, opt);
                    counter++;
                }

            }

            foreach (var i in listLevel2)
            {

                var text = i.Name;
                var prop = i.Id;
                var opt = new SelectListItem() { Text = i.Id.ToString() + " - " + i.Name, Value = i.Id.ToString() };

                if (listParLevel3Level2Level1Dto.Where(r => r.ParLevel3Level2.ParLevel2_Id == i.Id).Count() == 0)
                {
                    opt.Group = group;
                    retorno.Insert(counter, opt);
                    counter++;
                }

            }


            DdlLevel2Vinculados = retorno;
            //DdlLevel2Vinculados = retorno.OrderBy(r=>r.Group);
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
        public Nullable<int> hashKey { get; set; }

        public Nullable<int> ParScoreType_Id { get; set; }
        public string DataInit { get; set; }



    }
}