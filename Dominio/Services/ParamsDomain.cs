using Dominio.Interfaces.Services;
using Dominio.Interfaces.Repositories;
using DTO.DTO.Params;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;

namespace Dominio.Services
{
    /// <summary>
    /// Rns para parametrização.
    /// </summary>
    public class ParamsDomain : IParamsDomain
    {

        #region Constructor
        /*Repo Genericos, carregam ddls*/
        private IBaseRepository<ParLevel1> _baseRepoParLevel1;
        private IBaseRepository<ParLevel1> _baseRepoParLevel2;
        private IBaseRepository<ParLevel1> _baseRepoParLevel3;
        private IBaseRepository<ParLevel1XCluster> _baseRepoParLevel1XCluster;
        private IBaseRepository<ParFrequency> _baseParFrequency;
        private IBaseRepository<ParConsolidationType> _baseParConsolidationType;
        private IBaseRepository<ParCluster> _baseParCluster;
        private IBaseRepository<ParLevelDefiniton> _baseParLevelDefiniton;
        private IBaseRepository<ParFieldType> _baseParFieldType;
        private IBaseRepository<ParDepartment> _baseParDepartment;
        private IBaseRepository<ParLevel3Group> _baseParLevel3Group;
        private IBaseRepository<ParLocal> _baseParLocal;
        private IBaseRepository<ParCounter> _baseParCounter;
        private IBaseRepository<ParCounterXLocal> _baseParCounterXLocal;
        private IBaseRepository<ParRelapse> _baseParRelapse;
        private IBaseRepository<ParNotConformityRule> _baseParNotConformityRule;
        private IBaseRepository<ParNotConformityRuleXLevel> _baseParNotConformityRuleXLevel;
        private IBaseRepository<ParCompany> _baseParCompany;
        private IBaseRepository<ParHeaderField> _baseRepoParHeaderField;
        private IBaseRepository<ParLevel1XHeaderField> _baseRepoParLevel1XHeaderField;
        private IBaseRepository<ParMultipleValues> _baseRepoParMultipleValues;
        private IBaseRepository<ParEvaluation> _baseParEvaluation;
        private IBaseRepository<ParSample> _baseParSample;
        private IBaseRepository<ParLevel3> _baseParLevel3;
        private IBaseRepository<ParCounterXLocal> _baseRepoParCounterXLocal;
        /*Repo Especifico, manejam os itens*/
        private IParamsRepository _paramsRepo;

        public ParamsDomain(IBaseRepository<ParLevel1> baseRepoParLevel1,
                            IBaseRepository<ParLevel1> baseRepoParLevel2,
                            IBaseRepository<ParLevel1> baseRepoParLevel3,
                            IBaseRepository<ParLevel1XCluster> baseParLevel1XCluster,
                            IBaseRepository<ParFrequency> baseParFrequency,
                            IBaseRepository<ParConsolidationType> baseParConsolidationType,
                            IBaseRepository<ParCluster> baseParCluster,
                            IBaseRepository<ParLevelDefiniton> baseParLevelDefiniton,
                            IBaseRepository<ParFieldType> baseParFieldType,
                            IParamsRepository paramsRepo,
                            IBaseRepository<ParDepartment> baseParDepartment,
                            IBaseRepository<ParLevel3Group> baseParLevel3Group,
                            IBaseRepository<ParLocal> baseParLocal,
                            IBaseRepository<ParCounter> baseParCounter,
                            IBaseRepository<ParCounterXLocal> baseParCounterXLocal,
                            IBaseRepository<ParRelapse> baseParRelapse,
                            IBaseRepository<ParNotConformityRule> baseParNotConformityRule,
                            IBaseRepository<ParNotConformityRuleXLevel> baseParNotConformityRuleXLevel,
                            IBaseRepository<ParCompany> baseParCompany,
                            IBaseRepository<ParLevel1XHeaderField> baseRepoParLevel1XHeaderField,
                            IBaseRepository<ParMultipleValues> baseRepoParMultipleValues,
                            IBaseRepository<ParHeaderField> baseRepoParHeaderField,
                            IBaseRepository<ParEvaluation> baseParEvaluation,
                            IBaseRepository<ParSample> baseParSample,
                            IBaseRepository<ParLevel3> baseParLevel3,
                           IBaseRepository<ParCounterXLocal> baseRepoParCounterXLocal)
        {
            _paramsRepo = paramsRepo;
            _baseRepoParCounterXLocal = baseRepoParCounterXLocal;
            _baseRepoParLevel1XHeaderField = baseRepoParLevel1XHeaderField;
            _baseRepoParMultipleValues = baseRepoParMultipleValues;
            _baseRepoParHeaderField = baseRepoParHeaderField;
            _baseRepoParLevel1 = baseRepoParLevel1;
            _baseRepoParLevel2 = baseRepoParLevel2;
            _baseRepoParLevel3 = baseRepoParLevel3;
            _baseRepoParLevel1XCluster = baseParLevel1XCluster;
            _baseParFrequency = baseParFrequency;
            _baseParConsolidationType = baseParConsolidationType;
            _baseParCluster = baseParCluster;
            _baseParFieldType = baseParFieldType;
            _baseParLevelDefiniton = baseParLevelDefiniton;
            _baseParDepartment = baseParDepartment;
            _baseParLevel3Group = baseParLevel3Group;
            _baseParLocal = baseParLocal;
            _baseParCounter = baseParCounter;
            _baseParCounterXLocal = baseParCounterXLocal;
            _baseParRelapse = baseParRelapse;
            _baseParNotConformityRule = baseParNotConformityRule;
            _baseParNotConformityRuleXLevel = baseParNotConformityRuleXLevel;
            _baseParCompany = baseParCompany;
            _baseParEvaluation = baseParEvaluation;
            _baseParSample = baseParSample;
            _baseParLevel3 = baseParLevel3;
        }

        #endregion

        #region Level1

        /// <summary>
        /// Salva parametrização Level1
        /// </summary>
        /// <param name="paramsDto"></param>
        /// <returns></returns>
        public ParamsDTO AddUpdateLevel1(ParamsDTO paramsDto)
        {
            /*Validação*/
            //paramsDto.parLevel1Dto.IsValid();

            /*Mappers: mapeia elementos que vão ser salvos*/
            ParLevel1 saveParamLevel1 = Mapper.Map<ParLevel1>(paramsDto.parLevel1Dto);
            List<ParHeaderField> listaParHEadField = Mapper.Map<List<ParHeaderField>>(paramsDto.listParHeaderFieldDto);
            List<ParLevel1XCluster> ListaParLevel1XCluster = Mapper.Map<List<ParLevel1XCluster>>(paramsDto.parLevel1XClusterDto);
            List<ParCounterXLocal> ListaParCounterLocal = Mapper.Map<List<ParCounterXLocal>>(paramsDto.listParCounterXLocal);
            List<int> removerHeadField = paramsDto.parLevel1Dto.removerParHeaderField;
            List<int> removerCluster = paramsDto.parLevel1Dto.removerParCluster;
            List<int> removeCounter = paramsDto.parLevel1Dto.removerParCounterXlocal;

            /*Enviando para repository salvar, envia todos, pois como existe transaction, faz rolback de tudo se der erro.*/
            _paramsRepo.SaveParLevel1(saveParamLevel1, listaParHEadField, ListaParLevel1XCluster, removerHeadField, removerCluster, removeCounter, ListaParCounterLocal);

            /*Retorno*/
            paramsDto.parLevel1Dto.Id = saveParamLevel1.Id;
            return paramsDto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdParLevel1"></param>
        /// <returns></returns>
        public ParLevel1DTO GetLevel1(int idParLevel1)
        {
            /*ParLevel1*/
            var retorno = Mapper.Map<ParLevel1DTO>(_baseRepoParLevel1.GetById(idParLevel1));

            /*Clusters*/
            retorno.clustersInclusos = Mapper.Map<List<ParLevel1XClusterDTO>>(_baseRepoParLevel1XCluster.GetAll().Where(r => r.ParLevel1_Id == retorno.Id && r.IsActive == true));

            /*Cabeçalhos*/
            retorno.cabecalhosInclusos = Mapper.Map<List<ParLevel1XHeaderFieldDTO>>(_baseRepoParLevel1XHeaderField.GetAll().Where(r => r.ParLevel1_Id == retorno.Id && r.IsActive == true));

            /*Contadores*/
            retorno.contadoresIncluidos = Mapper.Map<List<ParCounterXLocalDTO>>(_baseRepoParCounterXLocal.GetAll().Where(r => r.ParLevel1_Id == retorno.Id && r.IsActive == true));

            return retorno;
        }

        #endregion


        #region Level2

        public ParamsDTO AddUpdateLevel2(ParamsDTO paramsDto)
        {
            //paramsDto.parLevel1Dto.IsValid();
            ParLevel2 saveParamLevel2 = Mapper.Map<ParLevel2>(paramsDto.parLevel2Dto);
            List<ParLevel3Group> listaParLevel3Group = Mapper.Map<List<ParLevel3Group>>(paramsDto.listParLevel3GroupDto);
            List<ParCounterXLocal> listParCounterXLocal = Mapper.Map<List<ParCounterXLocal>>(paramsDto.listParCounterXLocalDto);
            ParNotConformityRuleXLevel saveParamNotConformityRuleXLevel = Mapper.Map<ParNotConformityRuleXLevel>(paramsDto.parNotConformityRuleXLevelDto);
            ParEvaluation saveParamEvaluation = Mapper.Map<ParEvaluation>(paramsDto.parEvaluationDto);
            ParSample saveParamSample = Mapper.Map<ParSample>(paramsDto.parSampleDto);
            List<ParRelapse> listParRelapse = Mapper.Map<List<ParRelapse>>(paramsDto.listParRelapseDto);

            _paramsRepo.SaveParLevel2(saveParamLevel2, 
                                     listaParLevel3Group, 
                                     listParCounterXLocal, 
                                     saveParamNotConformityRuleXLevel,
                                     saveParamEvaluation,
                                     saveParamSample,
                                     listParRelapse);

            paramsDto.parLevel2Dto.Id = saveParamLevel2.Id;
            return paramsDto;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdParLevel2"></param>
        /// <returns></returns>
        public ParLevel2DTO GetLevel2(int idParLevel2)
        {
            /*ParLevel2*/
            var retorno = Mapper.Map<ParLevel2DTO>(_baseRepoParLevel2.GetById(idParLevel2));

            return retorno;
        }

        #endregion

        #region Level3
        public ParamsDTO AddUpdateLevel3(ParamsDTO paramsDto)
        {
            //paramsDto.parLevel1Dto.IsValid();
            ParLevel3 saveParamLevel3 = Mapper.Map<ParLevel3>(paramsDto.parLevel3Dto);
            _paramsRepo.SaveParLevel3(saveParamLevel3);
            paramsDto.parLevel3Dto.Id = saveParamLevel3.Id;
            return paramsDto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdParLevel2"></param>
        /// <returns></returns>
        public ParLevel3DTO GetLevel3(int idParLevel3)
        {
            /*ParLevel3*/
            var retorno = Mapper.Map<ParLevel3DTO>(_baseRepoParLevel3.GetById(idParLevel3));

            return retorno;
        }

        #endregion

        #region Auxiliares


        //public ParamsDTO AddUpdateParCounter(ParamsDTO paramsDto)
        //{
        //    //paramsDto.parLevel1Dto.IsValid();
        //    ParCounter saveParCounter = Mapper.Map<ParCounter>(paramsDto.parCounterDto);

        //    _paramsRepo.SaveParCounter(saveParCounter);

        //    paramsDto.parCounterDto.Id = saveParCounter.Id;

        //    ///*Salva Clueter X*/
        //    //SalvarParLevel1XCluster(paramsDto, saveParamLevel1);

        //    return paramsDto;
        //}

        //public ParamsDTO AddUpdateParCounterXLocal(ParamsDTO paramsDto)
        //{
        //    ParCounterXLocal saveParCounterLocal = Mapper.Map<ParCounterXLocal>(paramsDto.parCounterXLocalDto);
        //    _paramsRepo.SaveParCounterXLocal(saveParCounterLocal);
        //    paramsDto.parCounterXLocalDto.Id = saveParCounterLocal.Id;
        //    return paramsDto;
        //}

        //public ParamsDTO AddUpdateParRelapse(ParamsDTO paramsDto)
        //{
        //    ParRelapse saveParRelapse = Mapper.Map<ParRelapse>(paramsDto.parRelapseDto);
        //    _paramsRepo.SaveParRelapse(saveParRelapse);
        //    paramsDto.parRelapseDto.Id = saveParRelapse.Id;
        //    return paramsDto;
        //}

        //public ParamsDTO AddUpdateParNotConformityRule(ParamsDTO paramsDto)
        //{
        //     saveParNotConformityRule = Mapper.Map<ParNotConformityRule>(paramsDto.parNotConformityRuleDto);
        //    _paramsRepo.SaveParNotConformityRule(saveParNotConformityRule);
        //    paramsDto.parNotConformityRuleDto.Id = saveParNotConformityRule.Id;
        //    return paramsDto;
        //}

        public ParamsDTO AddUpdateParNotConformityRuleXLevel(ParamsDTO paramsDto)
        {
            ParNotConformityRuleXLevel saveParNotConformityRuleXLevel = Mapper.Map<ParNotConformityRuleXLevel>(paramsDto.parNotConformityRuleXLevelDto);
            _paramsRepo.SaveParNotConformityRuleXLevel(saveParNotConformityRuleXLevel);
            paramsDto.parNotConformityRuleXLevelDto.Id = saveParNotConformityRuleXLevel.Id;
            return paramsDto;
        }

        public ParamsDTO AddUpdateParCompany(ParamsDTO paramsDto)
        {
            ParCompany saveParCompany = Mapper.Map<ParCompany>(paramsDto.parCompanyDto);
            _paramsRepo.SaveParCompany(saveParCompany);
            paramsDto.parCompanyDto.Id = saveParCompany.Id;
            return paramsDto;
        }

        #endregion

        #region DropDowns

        public ParamsDdl CarregaDropDownsParams()
        {
            var DdlParConsolidation = Mapper.Map<List<ParConsolidationTypeDTO>>(_baseParConsolidationType.GetAll());
            var DdlFrequency = Mapper.Map<List<ParFrequencyDTO>>(_baseParFrequency.GetAll());
            var DdlparLevel1 = Mapper.Map<List<ParLevel1DTO>>(_baseRepoParLevel1.GetAll());
            var DdlparLevel2 = Mapper.Map<List<ParLevel2DTO>>(_baseRepoParLevel2.GetAll());
            var DdlparLevel3 = Mapper.Map<List<ParLevel3DTO>>(_baseRepoParLevel3.GetAll());
            var DdlparCluster = Mapper.Map<List<ParClusterDTO>>(_baseParCluster.GetAll());
            var DdlparLevelDefinition = Mapper.Map<List<ParLevelDefinitonDTO>>(_baseParLevelDefiniton.GetAll());
            var DdlParFieldType = Mapper.Map<List<ParFieldTypeDTO>>(_baseParFieldType.GetAll());
            var DdlParDepartment = Mapper.Map<List<ParDepartmentDTO>>(_baseParDepartment.GetAll());
            var DdlParNotConformityRule = Mapper.Map<List<ParNotConformityRuleDTO>>(_baseParNotConformityRule.GetAll());

            var DdlParLocal_Level1 = Mapper.Map<List<ParLocalDTO>>(_baseParLocal.GetAll().Where(p => p.Level == 1));
            var DdlParLocal_Level2 = Mapper.Map<List<ParLocalDTO>>(_baseParLocal.GetAll().Where(p => p.Level == 2));

            var DdlParCounter_Level1 = Mapper.Map<List<ParCounterDTO>>(_baseParCounter.GetAll().Where(p => p.Level == 1));
            var DdlParCounter_Level2 = Mapper.Map<List<ParCounterDTO>>(_baseParCounter.GetAll().Where(p => p.Level == 2));

            var retorno = new ParamsDdl();
            retorno.SetDdls(DdlParConsolidation, 
                            DdlFrequency, 
                            DdlparLevel1,
                            DdlparLevel2,
                            DdlparLevel3,
                            DdlparCluster, 
                            DdlparLevelDefinition, 
                            DdlParFieldType, 
                            DdlParDepartment,
                            DdlParCounter_Level1,
                            DdlParLocal_Level1,
                            DdlParCounter_Level2,
                            DdlParLocal_Level2,
                            DdlParNotConformityRule);
            return retorno;
        }

        #endregion
    }
}
