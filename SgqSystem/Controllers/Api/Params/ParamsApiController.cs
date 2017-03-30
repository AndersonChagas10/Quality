using AutoMapper;
using Dominio;
using Dominio.Interfaces.Services;
using DTO;
using DTO.DTO.Params;
using SgqSystem.Handlres;
using SgqSystem.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.Http;

namespace SgqSystem.Controllers.Api.Params
{
    [HandleApi()]
    [RoutePrefix("api/ParamsApi")]
    public class ParamsApiController : ApiController
    {

        #region Constructor

        private IParamsDomain _paramdDomain;
        private IBaseDomain<ParLevel1, ParLevel1DTO> _baseParLevel1;
        private IBaseDomain<ParLevel2, ParLevel2DTO> _baseParLevel2;
        private IBaseDomain<ParLevel3, ParLevel3DTO> _baseParLevel3;

        public ParamsApiController(IParamsDomain paramdDomain
            ,IBaseDomain<ParLevel2, ParLevel2DTO> baseParLevel2
            ,IBaseDomain<ParLevel3, ParLevel3DTO> baseParLevel3
            , IBaseDomain<ParLevel1, ParLevel1DTO> baseParLevel1)
        {
            _baseParLevel1 = baseParLevel1;
            _baseParLevel2 = baseParLevel2;
            _baseParLevel3 = baseParLevel3;
            _paramdDomain = paramdDomain;
        }

        #endregion

        #region SET

        [HttpPost]
        [ValidateModel]
        [Route("AddUpdateLevel1")]
        public ParamsViewModel AddUpdateLevel1([FromBody] ParamsViewModel paramsViewModel)
        {
            #region GAMBIARRA LEVEL 100!
            paramsViewModel.paramsDto.parLevel1Dto.IsSpecificNumberEvaluetion = paramsViewModel.paramsDto.parLevel1Dto.IsSpecificNumberSample; 
            #endregion
            paramsViewModel.paramsDto = _paramdDomain.AddUpdateLevel1(paramsViewModel.paramsDto);
            return paramsViewModel;
        }

        [HttpPost]
        [Route("SetRequiredCCAB/{Id}/{Required}")]
        public bool SetRequiredCamposCabecalho(int id, int required)
        {
            return _paramdDomain.SetRequiredCamposCabecalho(id, required);
        }

        [HttpPost]
        [Route("SetDefaultCCAB/{IdHeader}/{IdMultiple}")]
        public ParMultipleValues SetDefaultMultiplaEscolha(int idHeader, int idMultiple)
        {
            return _paramdDomain.SetDefaultMultiplaEscolha(idHeader, idMultiple);
        }

        [HttpPost]
        [Route("AddUpdateLevel2")]
        public ParamsViewModel AddUpdateLevel2([FromBody] ParamsViewModel paramsViewModel)
        {
            paramsViewModel.paramsDto = _paramdDomain.AddUpdateLevel2(paramsViewModel.paramsDto);
            return paramsViewModel;
        }

        [HttpPost]
        [Route("AddUpdateParLevel3Group")]
        public ParamsViewModel AddUpdateParLevel3Group([FromBody] ParamsViewModel paramsViewModel)
        {
            paramsViewModel.paramsDto = _paramdDomain.AddUpdateLevel2(paramsViewModel.paramsDto);
            return paramsViewModel;
        }
       
        [HttpPost]
        [Route("RemoveParLevel3Group/{Id}")]
        public ParLevel3GroupDTO RemoveParLevel3Group(int Id)
        {
            return _paramdDomain.RemoveParLevel3Group(Id);
        }

        [HttpPost]
        [Route("AddUpdateLevel3")]
        public ParamsViewModel AddUpdateLevel3([FromBody] ParamsViewModel paramsViewModel)
        {
            paramsViewModel.paramsDto = _paramdDomain.AddUpdateLevel3(paramsViewModel.paramsDto);
            return paramsViewModel;
        }

        [HttpPost]
        [Route("AlteraAvaliacaoAmostra")]
        public void AlteraAvaliacaoAmostra(ParLevel2SampleEvaluationDTO alterObj)
        {
            using (var db = new SgqDbDevEntities())
            {
                /*Busco do DB*/
                var sample = db.ParSample.FirstOrDefault(r => r.Id == alterObj.sampleId);
                var evaluation = db.ParEvaluation.FirstOrDefault(r => r.Id == alterObj.evaluationId);

                /*Altero*/
                sample.Number = alterObj.sampleNumber;
                evaluation.Number = alterObj.evaluationNumber;

                /*Explico para o EF que alterei*/
                db.ParSample.Attach(sample);
                var entrySample = db.Entry(sample);
                entrySample.Property(e => e.Number).IsModified = true;
              
                db.ParEvaluation.Attach(evaluation);
                var entryEvaluation = db.Entry(evaluation);
                entryEvaluation.Property(e => e.Number).IsModified = true;

                /*Salvo*/
                db.SaveChanges();
            }
          
        }

        #endregion

        #region Vinculo Level3 com Level2

        [HttpGet]
        [Route("AddVinculoL3L2/{idLevel2}/{idLevel3}/{peso}/{groupLevel2}")]
        public ParLevel3Level2DTO AddVinculoL3L2(int idLevel2, int idLevel3, decimal peso, int? groupLevel2 = 0)
        {
            return _paramdDomain.AddVinculoL3L2(idLevel2, idLevel3, peso, groupLevel2);
        }


        #endregion

        #region Vinculo Level1 com Level2

        [HttpGet]
        [Route("AddVinculoL1L2/{idLevel1}/{idLevel2}/{idLevel3}")]
        public List<ParLevel3Level2Level1DTO> AddVinculoL1L2(int idLevel1, int idLevel2,int idLevel3)
        {
            return _paramdDomain.AddVinculoL1L2(idLevel1, idLevel2, idLevel3);
        }

        [HttpPost]
        [Route("RemVinculoL1L2/{idLevel1}/{idLevel2}")]
        public bool RemVinculoL1L2(int idLevel1, int idLevel2)
        {
            return _paramdDomain.RemVinculoL1L2(idLevel1, idLevel2);
        }

        [HttpPost]
        [Route("VerificaShowBtnRemVinculoL1L2/{idLevel1}/{idLevel2}")]
        public bool VerificaShowBtnRemVinculoL1L2(int idLevel1, int idLevel2)
        {
            return _paramdDomain.VerificaShowBtnRemVinculoL1L2(idLevel1, idLevel2);
        }

        #endregion
      
        [HttpPost]
        [Route("AddRemoveParHeaderLevel2")]
        public ParLevel2XHeaderField AddRemoveParHeaderLevel2(ParLevel2XHeaderField parLevel2XHeaderField)
        {
            return _paramdDomain.AddRemoveParHeaderLevel2(parLevel2XHeaderField);
        }

        [HttpPost]
        [Route("AddUnidadeDeMedida/{valor}")]
        public ParMeasurementUnit AddUnidadeDeMedida(string valor)
        {
            var save = new ParMeasurementUnit() { Name = valor , AddDate = DateTime.Now, Description = string.Empty, IsActive = true };
            using (var db = new SgqDbDevEntities())
            {
                db.ParMeasurementUnit.Add(save);
                db.SaveChanges();
            }
            return save;
        }

        [HttpPost]
        [Route("GetListLevel1")]
        public List<ParLevel1DTO> GetListLevel1()
        {
            return _baseParLevel1.GetAllNoLazyLoad().ToList();
        }

        [HttpPost]
        [Route("GetListLevel2")]
        public List<ParLevel2DTO> GetListLevel2()
        {
            return _baseParLevel2.GetAllNoLazyLoad().ToList();

        }

        [HttpPost]
        [Route("GetListLevel3")]
        public List<ParLevel3DTO> GetListLevel3()
        {
            return _baseParLevel3.GetAllNoLazyLoad().ToList();
        }

        [HttpPost]
        [Route("GetListLevel2VinculadoLevel1/{level1Id}")]
        public List<ParLevel2DTO> GetListLevel2VinculadoLevel1(int level1Id)
        {
            var list = new List<ParLevel2DTO>();

            using (var db = new SgqDbDevEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                var result = db.ParLevel3Level2Level1.Where(r => r.ParLevel1_Id == level1Id).Select(r => r.ParLevel3Level2.ParLevel2).ToList().GroupBy(r => r.Id);
                list = Mapper.Map< List<ParLevel2DTO>>(result.Select(r=>r.First()));
            }

            return list;
        }

        [HttpPost]
        [Route("GetListLevel3VinculadoLevel2/{level2Id}")]
        public List<ParLevel3DTO> GetListLevel3VinculadoLevel2(int level2Id)
        {
            var list = new List<ParLevel3DTO>();

            using (var db = new SgqDbDevEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                var result = db.ParLevel3Level2.Where(r => r.ParLevel2_Id == level2Id).Select(r => r.ParLevel3).ToList().GroupBy(r => r.Id);
                list = Mapper.Map<List<ParLevel3DTO>>(result.Select(r => r.First()));
            }

            return list;
        }

        [HttpPost]
        [Route("GetListLevel3VinculadoLevel2Level1/{level1Id}/{level2Id}")]
        public List<ParLevel3DTO> GetListLevel3VinculadoLevel2Level1(int level1Id, int level2Id)
        {
            var list = new List<ParLevel3DTO>();

            using (var db = new SgqDbDevEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                var result = db.ParLevel3Level2Level1.Where(r => r.ParLevel3Level2.ParLevel2_Id == level2Id && r.ParLevel1_Id == level1Id).Select(r => r.ParLevel3Level2.ParLevel3).ToList().GroupBy(r => r.Id).Select(r => r.First());
                //var result = db.ParLevel3Level2.Where(r => r.ParLevel2_Id == level2.Id).Select(r => r.ParLevel3).ToList().GroupBy(r => r.Id).Select(r => r.First());
                list = Mapper.Map<List<ParLevel3DTO>>(result);
            }

            return list;
        }

        [HttpGet]
        [Route("GetResource/{language}")]
        public IEnumerable<DictionaryEntry> GetResource(string language)
        {
            if (language.Equals("pt-br") || (language.Equals("default") && GlobalConfig.Brasil)) //se portugues
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("pt-BR");
            }
            else if(language.Equals("en-us") || (language.Equals("default") && GlobalConfig.Eua))//inglês
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("");
            }
            
            System.Resources.ResourceManager resourceManager = Resources.Resource.ResourceManager;

            return resourceManager.GetResourceSet(
                Thread.CurrentThread.CurrentUICulture, true, false).Cast<DictionaryEntry>();
        }

    }
}
