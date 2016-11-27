using DTO.BaseEntity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace DTO.DTO.Params
{
    public class ParLevel2DTO : EntityBase
    {
        public decimal pesoDoVinculoSelecionado;
        public List<ParLevel3DTO> listaParLevel3Colleta;
        public List<ParCounterXLocalDTO> listParCounterXLocal { get; set; }

        [Range(0, 9999999999, ErrorMessage = "A Frequência é obrigatória.")]
        public int ParFrequency_Id { get; set; }

        [Range(0, 9999999999, ErrorMessage = "O departamento é obrigatório.")]
        public int ParDepartment_Id { get; set; }

        [Required(ErrorMessage = "O Nome deverá ter no mínimo 3 e máximo 10 caracteres.")]
        [MinLength(3, ErrorMessage = "O tamanho mínimo do Nome são 3 caracteres.")]
        [MaxLength(140, ErrorMessage = "O tamanho máximo do Nome são 140 caracteres.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "A Descrição deverá ter no mínimo 3 e máximo 10 caracteres.")]
        [MinLength(3, ErrorMessage = "O tamanho mínimo da Descrição deve ser 3 caracteres.")]
        [MaxLength(140, ErrorMessage = "O tamanho máximo da Descrição deve ser no 140 caracteres.")]
        public string Description { get; set; }

        public bool IsEmptyLevel3 { get; set; }
        public bool HasShowLevel03 { get; set; }
        public bool HasGroupLevel3 { get; set; }
        public bool IsActive { get; set; } = true;

        public ParConsolidationTypeDTO parConsolidationTypeDto { get; set; }
        public ParFrequencyDTO parFrequencyDto { get; set; }
        public List<ParLevel3Level2DTO> listParLevel3Level2Dto { get; set; }
        public List<ParRelapseDTO> listParRelapseDto { get; set; }
        public List<int> removeReincidencia { get; set; }


        public List<ParLevel3GroupDTO> ParLevel3Group { get; set; }
        public ParEvaluationDTO ParamEvaluation { get; set; }
        public ParSampleDTO ParamSample { get; set; }

        //public List<ParLevel3Level2DTO> ParLevel3Level2 { get; set; }

        public IEnumerable<SelectListItem> DdlLevel3Vinculados { get; set; }

        public void CreateSelectListParamsViewModelListLevel(List<ParLevel3DTO> listLevel3, List<ParLevel3Level2DTO> listParLevel3Level2Dto)
        {
            List<SelectListItem> retorno = new List<SelectListItem>();
            retorno.Insert(0, new SelectListItem() { Text = "Selecione...", Value = "-1" });
            var counter = 1;

            var group = new SelectListGroup() { Name = "Não vinculados:" };
            var groupSelecionado = new SelectListGroup();
            foreach (var i in listLevel3)
            {
                var text = i.Name;
                var prop = i.Id;
                var opt = new SelectListItem() { Text = i.Id.ToString() + " - " + i.Name, Value = i.Id.ToString() };
                if (listParLevel3Level2Dto.Where(r => r.ParLevel3_Id == i.Id).Count() > 0)
                {
                    groupSelecionado.Name = "Vinculado: " + listParLevel3Level2Dto.FirstOrDefault(r => r.ParLevel3_Id == i.Id).ParLevel2.Name;
                    opt.Group = groupSelecionado;
                }
                else
                {
                    opt.Group = group;
                }

                retorno.Insert(counter, opt);

                counter++;
            }

            DdlLevel3Vinculados = retorno;
        }
        public List<ParNotConformityRuleXLevelDTO> listParNotConformityRuleXLevelDto { get; set; }

    }
}