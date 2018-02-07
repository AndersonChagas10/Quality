using DTO.BaseEntity;
using DTO.Helpers;
using System;
using Resources;
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
        public List<int> listVinculoExistenLevel1Id;
        public bool isVinculado { get; set; }

        public List<ParCounterXLocalDTO> listParCounterXLocal { get; set; }

        [Range(0, 9999999999, ErrorMessageResourceName = "select_the_frequency", ErrorMessageResourceType = typeof(Resource))]
        public int ParFrequency_Id { get; set; }

        [Range(0, 9999999999, ErrorMessageResourceName = "select_the_department", ErrorMessageResourceType = typeof(Resource))]
        public int ParDepartment_Id { get; set; }

        //[Required(ErrorMessage = "O Nome deverá ter no mínimo 3 e máximo 10 caracteres.")]
        //[MinLength(3, ErrorMessage = "O tamanho mínimo do Nome são 3 caracteres.")]
        //[MaxLength(300, ErrorMessage = "O tamanho máximo do Nome são 300 caracteres.")]
        [Required(ErrorMessageResourceName = "name_has_between_3_and_10", ErrorMessageResourceType = typeof(Resource))]
        [MinLength(3, ErrorMessageResourceName = "minimum_name_3_characteres", ErrorMessageResourceType = typeof(Resource))]
        [MaxLength(300, ErrorMessageResourceName = "maximum_name_300_characteres", ErrorMessageResourceType = typeof(Resource))]
        public string Name { get; set; }

        //[Required(ErrorMessage = "A Descrição deverá ter no mínimo 1 e máximo 10 caracteres.")]
        //[MinLength(1, ErrorMessage = "O tamanho mínimo da Descrição deve ser 1 caracter.")]
        //[MaxLength(300, ErrorMessage = "O tamanho máximo da Descrição são 300 caracteres.")]
        [Required(ErrorMessageResourceName = "description_has_between_1_and_300", ErrorMessageResourceType = typeof(Resource))]
        [MinLength(1, ErrorMessageResourceName = "minimum_description_1_characteres", ErrorMessageResourceType = typeof(Resource))]
        [MaxLength(300, ErrorMessageResourceName = "maximum_description_300_characteres", ErrorMessageResourceType = typeof(Resource))]
        public string Description { get; set; }

        public bool IsEmptyLevel3 { get; set; }
        public bool HasShowLevel03 { get; set; }
        public bool HasGroupLevel3 { get; set; }
        public bool IsActive { get; set; } = true;
        public bool HasTakePhoto { get; set; } = false;

        public ParConsolidationTypeDTO parConsolidationTypeDto { get; set; }
        public ParFrequencyDTO parFrequencyDto { get; set; }
        public List<ParLevel3Level2DTO> listParLevel3Level2Dto { get; set; }
        public List<ParRelapseDTO> listParRelapseDto { get; set; }
        public List<int> removeReincidencia { get; set; }

     

        public List<ParLevel3GroupDTO> ParLevel3Group { get; set; }
        public ParEvaluationDTO ParamEvaluation { get; set; }
        public ParSampleDTO ParamSample { get; set; }
        public bool HasSampleTotal { get; set; }

        //public List<ParLevel3Level2DTO> ParLevel3Level2 { get; set; }

        public IEnumerable<SelectListItem> DdlLevel3Vinculados { get; set; }

        public void CreateSelectListParamsViewModelListLevel(List<ParLevel3DTO> listLevel3, List<ParLevel3Level2DTO> listParLevel3Level2Dto)
        {
            List<SelectListItem> retorno = new List<SelectListItem>();
            retorno.Insert(0, new SelectListItem() { Text = GlobalConfig.PrimeiraOption, Value = "-1" });
            var counter = 1;

            var group = new SelectListGroup() { Name = (GlobalConfig.Eua || GlobalConfig.Canada) ? "Unlinked" : "Não vinculado:" };
            var groupSelecionado = new SelectListGroup();

            foreach (var i in listLevel3)
            {

                var text = i.Name;
                var prop = i.Id;
                var opt = new SelectListItem() { Text = i.Id.ToString() + " - " + i.Name, Value = i.Id.ToString() };
                if (listParLevel3Level2Dto.Where(r => r.ParLevel3_Id == i.Id).Count() > 0)
                {
                    groupSelecionado.Name = (GlobalConfig.Eua || GlobalConfig.Canada) ? "Linked" : "Vinculado:";//: " + listParLevel3Level2Dto.FirstOrDefault(r => r.ParLevel3_Id == i.Id).ParLevel2.Name;
                    opt.Group = groupSelecionado;
                    retorno.Insert(counter, opt);
                    counter++;
                }
                          
            }

            foreach (var i in listLevel3)
            {

                var text = i.Name;
                var prop = i.Id;
                var opt = new SelectListItem() { Text = i.Id.ToString() + " - " + i.Name, Value = i.Id.ToString() };
                if (listParLevel3Level2Dto.Where(r => r.ParLevel3_Id == i.Id).Count() == 0)
                {
                    opt.Group = group;
                    retorno.Insert(counter, opt);
                    counter++;
                }
                             
            }

            DdlLevel3Vinculados = retorno;
        }
        public List<ParNotConformityRuleXLevelDTO> listParNotConformityRuleXLevelDto { get; set; }
        public List<ParLevel3GroupDTO> listParLevel3GroupDto { get; set; }
        public List<ParLevel2SampleEvaluationDTO> listParLevel2SampleEvaluationDTO { get; set; }

        public List<ParSampleDTO> listSample { get; set; }
        public List<ParEvaluationDTO> listEvaluation { get; set; }
        public List<ParLevel1XHeaderFieldDTO> cabecalhosInclusos { get; set; }
        public List<ParLevel2XHeaderFieldDTO> cabecalhosExclusos { get; set; }

        public void CriaListaSampleEvaluation()
        {
            listSample = new List<ParSampleDTO>();
            listEvaluation = new List<ParEvaluationDTO>();

            //var avAmTodas = listParLevel2SampleEvaluationDTO.FirstOrDefault(r => r.companyId == null || r.companyId == 0);

            //if (avAmTodas != null)
            //{
            //    var evaluationIgualTodas = listParLevel2SampleEvaluationDTO.Where(r => r.evaluationNumber == avAmTodas.evaluationNumber && r.companyId == avAmTodas.companyId);
            //    var sampleIgualTodas = listParLevel2SampleEvaluationDTO.Where(r => r.sampleNumber == avAmTodas.sampleNumber && r.companyId == avAmTodas.companyId);
            //}

            if (listParLevel2SampleEvaluationDTO != null)
                foreach (var i in listParLevel2SampleEvaluationDTO)
                {

                    var sample = new ParSampleDTO()
                    {
                        Number = i.sampleNumber,
                        Id = i.sampleId ?? 0,
                        IsActive = i.IsActive
                    };

                    var evaluation = new ParEvaluationDTO()
                    {
                        Number = i.evaluationNumber,
                        Id = i.evaluationId ?? 0,
                        IsActive = i.IsActive
                    };

                    if (i.companyId > 0)
                    {
                        sample.ParCompany_Id = i.companyId;
                        evaluation.ParCompany_Id = i.companyId;
                    }

                    listSample.Add(sample);
                    listEvaluation.Add(evaluation);
                }
        }

        public void RecuperaListaSampleEvaluation()
        {
            listParLevel2SampleEvaluationDTO = new List<ParLevel2SampleEvaluationDTO>();
            if (listEvaluation != null)
                foreach (var i in listEvaluation)
                {
                    var coiso = new ParLevel2SampleEvaluationDTO();
                    coiso.evaluationId = i.Id;
                    coiso.evaluationNumber = i.Number;

                    var sample = listSample.FirstOrDefault(r => r.ParCompany_Id == i.ParCompany_Id);
                    if (sample != null)
                    {
                        coiso.sampleId = sample.Id;
                        coiso.sampleNumber = sample.Number;
                    }
                    else
                    {
                        coiso.sampleId = 0;
                        coiso.sampleNumber = listEvaluation.FirstOrDefault(r => r.ParCompany_Id == null).Number;
                    }

                    coiso.IsActive = i.IsActive;
                    coiso.Id = 1;
                    if (i.ParCompany_Id != null && i.ParCompany_Id > 0)
                        coiso.companyId = i.ParCompany_Id;

                    listParLevel2SampleEvaluationDTO.Add(coiso);

                }

        }

        public void RegrasParamsLevel1(ParLevel1DTO parLevel1)
        {
            if (parLevel1.IsSpecific)
            {
                _PermiteCadastroEspecificoPorUnidade = parLevel1.IsSpecific;
                _PermiteEditarCamposCabecalho = parLevel1.IsSpecificHeaderField;
                _PermiteEditarNumeroDeAv = parLevel1.IsSpecificNumberEvaluetion || parLevel1.IsSpecificNumberSample ? true : false;
                _PermiteEditarVinculoComTarefa = parLevel1.IsSpecificLevel3;
            }
        }

        public bool _PermiteCadastroEspecificoPorUnidade { get; set; }
        public bool _PermiteEditarCamposCabecalho { get; set; }
        public bool _PermiteEditarNumeroDeAv { get; set; }
        public bool _PermiteEditarNumeroDeAm { get; set; }
        public bool _PermiteEditarVinculoComTarefa { get; set; }
        public bool _PermiteEditarMetaDoIndicadorComTarefa { get; set; }


    }
}