using Dominio.Interfaces.Services;
using Dominio.Interfaces.Repositories;
using DTO.DTO.Params;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;

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
        private IBaseRepository<ParLevel2> _baseRepoParLevel2;
        private IBaseRepository<ParLevel3> _baseRepoParLevel3;
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
        private IBaseRepository<ParLevel3Value> _baseParLevel3Value;
        private IBaseRepository<ParLevel3InputType> _baseParLevel3InputType;
        private IBaseRepository<ParLevel3BoolFalse> _baseParLevel3BoolFalse;
        private IBaseRepository<ParLevel3BoolTrue> _baseParLevel3BoolTrue;
        private IBaseRepository<ParCounterXLocal> _baseRepoParCounterXLocal;
        private IBaseRepository<ParMeasurementUnit> _baseParMeasurementUnit;
        private IBaseRepository<ParLevel3Level2> _baseParLevel3Level2;
        private IBaseRepository<ParLevel3Level2> _baseRepoParLevel3Level2;
        private IBaseRepository<ParLevel3Level2Level1> _baseRepoParLevel3Level2Level1;
        private IBaseRepository<ParCriticalLevel> _baseRepoParCriticalLevel;
        private IParLevel3Repository _repoParLevel3;
        /*Repo Especifico, manejam os itens*/
        private IParamsRepository _paramsRepo;

        public ParamsDomain(IBaseRepository<ParLevel1> baseRepoParLevel1,
                            IBaseRepository<ParLevel2> baseRepoParLevel2,
                            IBaseRepository<ParLevel3> baseRepoParLevel3,
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
                            IBaseRepository<ParLevel3Value> baseParLevel3Value,
                            IBaseRepository<ParLevel3InputType> baseParLevel3InputType,
                            IBaseRepository<ParCounterXLocal> baseRepoParCounterXLocal,
                            IBaseRepository<ParMeasurementUnit> baseParMeasurementUnit,
                            IBaseRepository<ParLevel3BoolFalse> baseParLevel3BoolFalse,
                            IBaseRepository<ParLevel3BoolTrue> baseParLevel3BoolTrue,
                            IBaseRepository<ParLevel3Level2> baseParLevel3Level2,
                            IBaseRepository<ParLevel3Level2> baseRepoParLevel3Level2,
                            IBaseRepository<ParLevel3Level2Level1> baseRepoParLevel3Level2Level1,
                            IParLevel3Repository repoParLevel3,
                            IBaseRepository<ParCriticalLevel> baseRepoParCriticalLevel)
        {
            _baseRepoParCriticalLevel = baseRepoParCriticalLevel;
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
            _baseParLevel3Value = baseParLevel3Value;
            _baseParLevel3InputType = baseParLevel3InputType;
            _baseParMeasurementUnit = baseParMeasurementUnit;
            _baseParLevel3BoolFalse = baseParLevel3BoolFalse;
            _baseParLevel3BoolTrue = baseParLevel3BoolTrue;
            _baseParLevel3Level2 = baseParLevel3Level2;
            _baseRepoParLevel3Level2 = baseRepoParLevel3Level2;
            _baseRepoParLevel3Level2Level1 = baseRepoParLevel3Level2Level1;
            _repoParLevel3 = repoParLevel3;
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
            ParLevel1 saveParamLevel1 = Mapper.Map<ParLevel1>(paramsDto.parLevel1Dto);                                                                                      //ParLevel1
            List<ParHeaderField> listaParHEadField = Mapper.Map<List<ParHeaderField>>(paramsDto.listParHeaderFieldDto);                                                     //Cabeçalhos do Level1
            List<ParLevel1XCluster> ListaParLevel1XCluster = Mapper.Map<List<ParLevel1XCluster>>(paramsDto.parLevel1XClusterDto);                                           //Clusters do Level1
            List<ParCounterXLocal> ListaParCounterLocal = Mapper.Map<List<ParCounterXLocal>>(paramsDto.parLevel1Dto.listParCounterXLocal);                                               //Contadores do Level1
            List<ParRelapse> listaReincidencia = Mapper.Map<List<ParRelapse>>(paramsDto.parLevel1Dto.listParRelapseDto);                                                    //Reincidencia do Level1
            List<ParNotConformityRuleXLevel> listNonCoformitRule = Mapper.Map<List<ParNotConformityRuleXLevel>>(paramsDto.parLevel1Dto.listParNotConformityRuleXLevelDto);  //Regra de NC do Level1

            /*Inativar*/
            List<int> removerHeadField = paramsDto.parLevel1Dto.removerParHeaderField;
            //List<int> removerCluster = paramsDto.parLevel1Dto.removerParCluster;
            //List<int> removeCounter = paramsDto.parLevel1Dto.removerParCounterXlocal;
            //List<int> removeReincidencia = paramsDto.parLevel1Dto.removeReincidencia;
            //List<int> removeNotConformityRuleXLevel = paramsDto.parLevel1Dto.listParNotConformityRuleXLevelDto.Where(r => r.IsActive == false).Select(r => r.Id).ToList();

            /*Reativar*/


            /*Enviando para repository salvar, envia todos, pois como existe transaction, faz rolback de tudo se der erro.*/
            try
            {
                _paramsRepo.SaveParLevel1(saveParamLevel1, listaParHEadField, ListaParLevel1XCluster, removerHeadField
                                            , ListaParCounterLocal, listNonCoformitRule, listaReincidencia);
            }
            catch (DbUpdateException e)
            {
                VerifyUniqueName(saveParamLevel1, e);
            }

            /*Retorno*/
            paramsDto.parLevel1Dto.Id = saveParamLevel1.Id;
            return paramsDto;
        }

        private static void VerifyUniqueName<T>(T obj, DbUpdateException e)
        {
            if (e.InnerException != null)
                if (e.InnerException.InnerException != null)
                {
                    SqlException innerException = e.InnerException.InnerException as SqlException;
                    if (innerException != null && (innerException.Number == 2627 || innerException.Number == 2601))
                    {
                        if (innerException.Message.IndexOf("Name") > 0)
                            throw new ExceptionHelper("O Nome: " + obj.GetType().GetProperty("Name").GetValue(obj) + " já existe.");
                    }
                    else
                    {
                        throw e;
                    }

                }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdParLevel1"></param>
        /// <returns></returns>
        public ParLevel1DTO GetLevel1(int idParLevel1)
        {
            /*ParLevel1*/
            var parlevel1 = _baseRepoParLevel1.GetById(idParLevel1);
            var retorno = Mapper.Map<ParLevel1DTO>(parlevel1);

            /*Clusters*/
            retorno.listLevel1XClusterDto = Mapper.Map<List<ParLevel1XClusterDTO>>(parlevel1.ParLevel1XCluster.OrderBy(r => r.IsActive));

            /*Cabeçalhos*/
            retorno.cabecalhosInclusos = Mapper.Map<List<ParLevel1XHeaderFieldDTO>>(parlevel1.ParLevel1XHeaderField.Where(r => r.ParLevel1_Id == retorno.Id && r.IsActive == true).OrderBy(r => r.IsActive));

            /*Contadores*/
            retorno.listParCounterXLocal = Mapper.Map<List<ParCounterXLocalDTO>>(parlevel1.ParCounterXLocal.OrderBy(r => r.IsActive));

            /*Level 2 e 3 vinculados*/
            retorno.listParLevel3Level2Level1Dto = Mapper.Map<List<ParLevel3Level2Level1DTO>>(parlevel1.ParLevel3Level2Level1);

            retorno.CreateSelectListParamsViewModelListLevel(Mapper.Map<List<ParLevel2DTO>>(_baseRepoParLevel2.GetAll()), retorno.listParLevel3Level2Level1Dto);

            //foreach (var i in retorno.listParLevel3Level2Level1Dto)
            //{
            //    var idLevel2 = i.ParLevel3Level2.ParLevel2_Id;
            //    _baseRepoParLevel3Level2.GetAll().Where(r => r.ParLevel2_Id == idLevel2);
            //}

            //non conf
            retorno.parNotConformityRuleXLevelDto = new ParNotConformityRuleXLevelDTO();
            retorno.listParNotConformityRuleXLevelDto = Mapper.Map<List<ParNotConformityRuleXLevelDTO>>(parlevel1.ParNotConformityRuleXLevel.OrderBy(r => r.IsActive));
            
            /*Reincidencia*/
            retorno.listParRelapseDto = Mapper.Map<List<ParRelapseDTO>>(parlevel1.ParRelapse.OrderBy(r => r.IsActive));

            return retorno;
        }

        #endregion

        #region Level2

        public ParamsDTO AddUpdateLevel2(ParamsDTO paramsDto)
        {
            //paramsDto.parLevel1Dto.IsValid();
            ParLevel2 saveParamLevel2 = Mapper.Map<ParLevel2>(paramsDto.parLevel2Dto);
            List<ParLevel3Group> listaParLevel3Group = Mapper.Map<List<ParLevel3Group>>(paramsDto.listParLevel3GroupDto);
            List<ParCounterXLocal> listParCounterXLocal = Mapper.Map<List<ParCounterXLocal>>(paramsDto.parLevel2Dto.listParCounterXLocal);

            List<ParNotConformityRuleXLevel> listNonCoformitRule = Mapper.Map<List<ParNotConformityRuleXLevel>>(paramsDto.parLevel2Dto.listParNotConformityRuleXLevelDto);  //

            //ParNotConformityRuleXLevel saveParamNotConformityRuleXLevel = Mapper.Map<ParNotConformityRuleXLevel>(paramsDto.parNotConformityRuleXLevelDto);
            ParEvaluation saveParamEvaluation = Mapper.Map<ParEvaluation>(paramsDto.parEvaluationDto);
            ParSample saveParamSample = Mapper.Map<ParSample>(paramsDto.parSampleDto);
            List<ParRelapse> listParRelapse = Mapper.Map<List<ParRelapse>>(paramsDto.parLevel2Dto.listParRelapseDto);
            List<int> listParRelapseRemove = paramsDto.parLevel2Dto.removeReincidencia;

            try
            {
                _paramsRepo.SaveParLevel2(saveParamLevel2,
                                           listaParLevel3Group,
                                           listParCounterXLocal,
                                           listNonCoformitRule,
                                           saveParamEvaluation,
                                           saveParamSample,
                                           listParRelapse,
                                           listParRelapseRemove);
            }
            catch (DbUpdateException e)
            {
                VerifyUniqueName(saveParamLevel2, e);
            }

            paramsDto.parLevel2Dto.Id = saveParamLevel2.Id;
            return paramsDto;
        }

        public ParamsDTO RemoveLevel03Group(int Id)
        {

            var parLevel3Group = Mapper.Map<ParLevel3Group>(_baseParLevel3Group.GetAll().Where(r => r.Id == Id).FirstOrDefault());
            if (parLevel3Group != null)
            {
                _paramsRepo.RemoveParLevel3Group(parLevel3Group);
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdParLevel2"></param>
        /// <returns></returns>
        public ParamsDTO GetLevel2(int idParLevel2, int? level3Id = 0)
        {
            /*ParLevel2*/
            var paramsDto = new ParamsDTO();
            var parLevel2 = _baseRepoParLevel2.GetById(idParLevel2);
            var level2 = Mapper.Map<ParLevel2DTO>(parLevel2);

            /*Level 2 e 3 vinculados*/
            level2.listParLevel3Level2Dto = Mapper.Map<List<ParLevel3Level2DTO>>(_baseRepoParLevel3Level2.GetAll().Where(r => r.ParLevel2_Id == level2.Id).ToList());
            level2.CreateSelectListParamsViewModelListLevel(Mapper.Map<List<ParLevel3DTO>>(_baseRepoParLevel3.GetAll()), level2.listParLevel3Level2Dto);

            paramsDto.listParCounterXLocal = Mapper.Map<List<ParCounterXLocalDTO>>(parLevel2.ParCounterXLocal);
            level2.listParCounterXLocal = paramsDto.listParCounterXLocal;

            paramsDto.parEvaluationDto = Mapper.Map<ParEvaluationDTO>(_baseParEvaluation.GetAll().FirstOrDefault(r => r.ParLevel2_Id == level2.Id && r.IsActive == true));
            paramsDto.parSampleDto = Mapper.Map<ParSampleDTO>(_baseParSample.GetAll().FirstOrDefault(r => r.ParLevel2_Id == level2.Id && r.IsActive == true));
            //paramsDto.parNotConformityRuleXLevelDto = Mapper.Map<ParNotConformityRuleXLevelDTO>(_baseParNotConformityRuleXLevel.GetAll().FirstOrDefault(r => r.ParLevel2_Id == level2.Id));
            paramsDto.listParLevel3GroupDto = Mapper.Map<List<ParLevel3GroupDTO>>(_baseParLevel3Group.GetAll().Where(r => r.ParLevel2_Id == level2.Id && r.IsActive == true).ToList());

            level2.listParRelapseDto = Mapper.Map<List<ParRelapseDTO>>(_baseParRelapse.GetAll().Where(r => r.ParLevel2_Id == level2.Id && r.IsActive == true).ToList());

            paramsDto.listParLevel3GroupDto = Mapper.Map<List<ParLevel3GroupDTO>>(_baseParLevel3Group.GetAll().Where(r => r.ParLevel2_Id == level2.Id && r.IsActive == true).ToList());

            var vinculoLevel3Level2 = _baseRepoParLevel3Level2.GetAll().FirstOrDefault(r => r.ParLevel2_Id == idParLevel2 && r.ParLevel3_Id == level3Id);
            if (vinculoLevel3Level2 != null)
            {
                level2.pesoDoVinculoSelecionado = vinculoLevel3Level2.Weight;
            }
            else
            {
                level2.pesoDoVinculoSelecionado = 0;
            }

            //non conf
            paramsDto.parNotConformityRuleXLevelDto = new ParNotConformityRuleXLevelDTO();
            level2.listParNotConformityRuleXLevelDto = Mapper.Map<List<ParNotConformityRuleXLevelDTO>>(_baseParNotConformityRuleXLevel.GetAll().Where(r => r.ParLevel2_Id == level2.Id));

            //parNotConformityRuleXLevelDto
            paramsDto.parLevel2Dto = level2;

            return paramsDto;
        }

        #endregion

        #region Level3

        public ParamsDTO AddUpdateLevel3(ParamsDTO paramsDto)
        {
            //paramsDto.parLevel1Dto.IsValid();
            ParLevel3 saveParamLevel3 = Mapper.Map<ParLevel3>(paramsDto.parLevel3Dto);
            ParLevel3Value saveParamLevel3Value = Mapper.Map<ParLevel3Value>(paramsDto.parLevel3Value);
            List<ParRelapse> listParRelapse = Mapper.Map<List<ParRelapse>>(paramsDto.parLevel3Dto.listParRelapseDto);
            List<int> listParRelapseRemove = paramsDto.parLevel3Dto.removeReincidencia;

            try
            {
                _paramsRepo.SaveParLevel3(saveParamLevel3, saveParamLevel3Value, listParRelapse, listParRelapseRemove);
            }
            catch (DbUpdateException e)
            {
                VerifyUniqueName(saveParamLevel3, e);
            }

            paramsDto.parLevel3Dto.Id = saveParamLevel3.Id;
            return paramsDto;
        }

        public ParamsDTO AddUpdateLevel3Level2(ParamsDTO paramsDto)
        {
            ParLevel3Level2 saveParamLevel3Leve2 = Mapper.Map<ParLevel3Level2>(paramsDto.parLevel3Level2);
            _paramsRepo.SaveParLevel3Level2(saveParamLevel3Leve2);
            paramsDto.parLevel3Dto.Id = saveParamLevel3Leve2.Id;
            return paramsDto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdParLevel2"></param>
        /// <returns></returns>
        public ParamsDTO GetLevel3(int idParLevel3, int? idParLevel2 = 0)
        {
            ParamsDTO retorno = new ParamsDTO();
            /*ParLevel3*/
            var level3 = Mapper.Map<ParLevel3DTO>(_baseRepoParLevel3.GetById(idParLevel3));

            if (idParLevel2 > 0)
            {
                var vinculoLevel3Level2 = _baseRepoParLevel3Level2.GetAll().FirstOrDefault(r => r.ParLevel2_Id == idParLevel2 && r.ParLevel3_Id == idParLevel3);
                if (vinculoLevel3Level2 != null)
                {
                    level3.pesoDoVinculo = vinculoLevel3Level2.Weight;
                }
                else
                {
                    level3.pesoDoVinculo = 0;
                }
            }

            var parLevel3Value = Mapper.Map<ParLevel3ValueDTO>(_baseParLevel3Value.GetAll().FirstOrDefault(r => r.ParLevel3_Id == level3.Id));
            level3.listParRelapseDto = Mapper.Map<List<ParRelapseDTO>>(_baseParRelapse.GetAll().Where(r => r.ParLevel3_Id == level3.Id && r.IsActive == true).ToList());

            level3.listGroupsLevel2 = new List<ParLevel3GroupDTO>();
            level3.listGroupsLevel2 = Mapper.Map<List<ParLevel3GroupDTO>>(_baseParLevel3Group.GetAll().Where(r => r.ParLevel2_Id == idParLevel2 && r.IsActive == true).ToList());

            var parLevel3Level2 = _baseParLevel3Level2.GetAll().FirstOrDefault(r => r.ParLevel2_Id == idParLevel2 && r.ParLevel3_Id == level3.Id);
            if (parLevel3Level2 != null)
                level3.groupLevel2Selected = _baseParLevel3Level2.GetAll().FirstOrDefault(r => r.ParLevel2_Id == idParLevel2 && r.ParLevel3_Id == level3.Id).ParLevel3Group_Id;

            retorno.parLevel3Dto = level3;
            retorno.parLevel3Value = parLevel3Value;


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

            var DdlParLevel3InputType = Mapper.Map<List<ParLevel3InputTypeDTO>>(_baseParLevel3InputType.GetAll());
            var DdlParMeasurementUnit = Mapper.Map<List<ParMeasurementUnitDTO>>(_baseParMeasurementUnit.GetAll());

            var DdlParLevel3BoolFalse = Mapper.Map<List<ParLevel3BoolFalseDTO>>(_baseParLevel3BoolFalse.GetAll());
            var DdlParLevel3BoolTrue = Mapper.Map<List<ParLevel3BoolTrueDTO>>(_baseParLevel3BoolTrue.GetAll());

            var DdlparCrit = Mapper.Map<List<ParCriticalLevelDTO>>(_baseRepoParCriticalLevel.GetAll());

            var retorno = new ParamsDdl();

            retorno.SetDdlsNivel123(DdlparLevel1,
                            DdlparLevel2,
                            DdlparLevel3);

            retorno.SetDdls(DdlParConsolidation,
                            DdlFrequency,
                            //DdlparLevel1,
                            //DdlparLevel2,
                            //DdlparLevel3,
                            DdlparCluster,
                            DdlparLevelDefinition,
                            DdlParFieldType,
                            DdlParDepartment,
                            DdlParCounter_Level1,
                            DdlParLocal_Level1,
                            DdlParCounter_Level2,
                            DdlParLocal_Level2,
                            DdlParNotConformityRule,
                            DdlParLevel3InputType,
                            DdlParMeasurementUnit,
                            DdlParLevel3BoolFalse,
                            DdlParLevel3BoolTrue,
                            DdlparCrit);
            return retorno;
        }

        #endregion

        #region Vinculo L3L2

        public ParLevel3Level2DTO AddVinculoL3L2(int idLevel2, int idLevel3, decimal peso, int? groupLevel2 = 0)
        {
            ParLevel3Level2 objLelvel2Level3ToSave;
            var level2 = _baseRepoParLevel2.GetById(idLevel2);

            //if (peso <= 0)
            //    peso = 1;

            objLelvel2Level3ToSave = new ParLevel3Level2()
            {
                ParLevel2_Id = idLevel2,
                ParLevel3_Id = idLevel3,
                Weight = peso,
                ParLevel3Group_Id = groupLevel2 == 0 ? null : groupLevel2
            };

            var existente = _baseRepoParLevel3Level2.GetAll().FirstOrDefault(r => r.ParLevel2_Id == idLevel2 && r.ParLevel3_Id == idLevel3);
            if (existente != null)
            {
                objLelvel2Level3ToSave = existente;
                objLelvel2Level3ToSave.Weight = peso;
                objLelvel2Level3ToSave.ParLevel3Group_Id = groupLevel2 == 0 ? null : groupLevel2;
            }


            _baseRepoParLevel3Level2.AddOrUpdate(objLelvel2Level3ToSave);
            ParLevel3Level2DTO objtReturn = Mapper.Map<ParLevel3Level2DTO>(objLelvel2Level3ToSave);
            return objtReturn;
        }

        public ParLevel3Level2Level1DTO AddVinculoL1L2(int idLevel1, int idLevel2, int idLevel3)
        {
            var existsL3L2 = _baseRepoParLevel3Level2.GetAll().FirstOrDefault(r => r.ParLevel2_Id == idLevel2 && r.ParLevel3_Id == idLevel3);
            if (existsL3L2 == null)
                throw new ExceptionHelper("É necessário vincular o level3 ao level 2 antes de realizar esta operação.");

            var vinculoLevel2Level3 = _baseRepoParLevel3Level2.GetAll().FirstOrDefault(r => r.ParLevel2_Id == idLevel2 && r.ParLevel3_Id == idLevel3);
            var objToSave = new ParLevel3Level2Level1()
            {
                ParLevel1_Id = idLevel1,
                ParLevel3Level2_Id = vinculoLevel2Level3.Id
            };

            var exists = _baseRepoParLevel3Level2Level1.GetAll().FirstOrDefault(r => r.ParLevel3Level2_Id == vinculoLevel2Level3.Id);
            if (exists != null)
                objToSave.Id = exists.Id;

            _baseRepoParLevel3Level2Level1.AddOrUpdate(objToSave);

            return Mapper.Map<ParLevel3Level2Level1DTO>(objToSave);
        }


        public ParLevel3GroupDTO RemoveParLevel3Group(int Id)
        {
            var parLevel3Group = _baseParLevel3Group.GetAll().FirstOrDefault(r => r.Id == Id);
            parLevel3Group.IsActive = false;
            _baseParLevel3Group.AddOrUpdate(parLevel3Group);
            //_paramsRepo.RemoveParLevel3Group(parLevel3Group);
            return Mapper.Map<ParLevel3GroupDTO>(parLevel3Group);
        }


        #endregion

        #region Collection

        public List<ParLevel1DTO> GetAllLevel1()
        {
            var retorno = new List<ParLevel1DTO>();

            var listLevel1 = _baseRepoParLevel1.GetAll().Where(r => r.IsActive == true);

            foreach (var level1 in listLevel1)
            {
                retorno.Add(GetLevel1TelaColeta(level1.Id));
            }

            return retorno;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdParLevel1"></param>
        /// <returns></returns>
        public ParLevel1DTO GetLevel1TelaColeta(int idParLevel1)
        {
            /*ParLevel1*/
            var parlevel1 = _baseRepoParLevel1.GetById(idParLevel1);
            var retorno = Mapper.Map<ParLevel1DTO>(_baseRepoParLevel1.GetById(idParLevel1));

            /*Clusters*/
            retorno.listLevel1XClusterDto = Mapper.Map<List<ParLevel1XClusterDTO>>(parlevel1.ParLevel1XCluster.Where(r=>r.IsActive == true));

            /*Cabeçalhos*/
            retorno.cabecalhosInclusos = Mapper.Map<List<ParLevel1XHeaderFieldDTO>>(_baseRepoParLevel1XHeaderField.GetAll().Where(r => r.ParLevel1_Id == retorno.Id && r.IsActive == true));

            /*Contadores*/
            retorno.contadoresIncluidos = Mapper.Map<List<ParCounterXLocalDTO>>(_baseRepoParCounterXLocal.GetAll().Where(r => r.ParLevel1_Id == retorno.Id));

            /*Level 2 e 3 vinculados*/
            retorno.listParLevel3Level2Level1Dto = Mapper.Map<List<ParLevel3Level2Level1DTO>>(_baseRepoParLevel3Level2Level1.GetAll().Where(r => r.ParLevel1_Id == retorno.Id).ToList());
            retorno.CreateSelectListParamsViewModelListLevel(Mapper.Map<List<ParLevel2DTO>>(_baseRepoParLevel2.GetAll()), retorno.listParLevel3Level2Level1Dto);

            retorno.listParLevel2Colleta = new List<ParLevel2DTO>();
            foreach (var ParLevel3Level2Level1Dto in retorno.listParLevel3Level2Level1Dto.Distinct())
            {
                if (!retorno.listParLevel2Colleta.Any(r => r.Id == ParLevel3Level2Level1Dto.ParLevel3Level2.ParLevel2.Id))
                {
                    ParLevel3Level2Level1Dto.ParLevel3Level2.ParLevel2.listParCounterXLocal = Mapper.Map<List<ParCounterXLocalDTO>>(_baseParCounterXLocal.GetAll().Where(r => r.ParLevel2_Id == ParLevel3Level2Level1Dto.ParLevel3Level2.ParLevel2.Id && r.IsActive == true).ToList());
                    ParLevel3Level2Level1Dto.ParLevel3Level2.ParLevel2.ParamEvaluation = Mapper.Map<ParEvaluationDTO>(_baseParEvaluation.GetAll().Where(r => r.ParLevel2_Id == ParLevel3Level2Level1Dto.ParLevel3Level2.ParLevel2.Id).FirstOrDefault());
                    ParLevel3Level2Level1Dto.ParLevel3Level2.ParLevel2.ParamSample = Mapper.Map<ParSampleDTO>(_baseParSample.GetAll().Where(r => r.ParLevel2_Id == ParLevel3Level2Level1Dto.ParLevel3Level2.ParLevel2.Id).FirstOrDefault());
                    retorno.listParLevel2Colleta.Add(ParLevel3Level2Level1Dto.ParLevel3Level2.ParLevel2);
                }
                var parLevel3Level2DoLevel2 = _repoParLevel3.GetLevel3VinculadoLevel2(retorno.Id);

                retorno.listParLevel2Colleta.LastOrDefault().listaParLevel3Colleta = new List<ParLevel3DTO>();
                foreach (var level3Level2 in parLevel3Level2DoLevel2.Where(r => r.ParLevel2_Id == ParLevel3Level2Level1Dto.ParLevel3Level2.ParLevel2.Id))
                {
                    if (!retorno.listParLevel2Colleta.LastOrDefault().listaParLevel3Colleta.Any(r => r.Id == level3Level2.ParLevel3_Id))
                        retorno.listParLevel2Colleta.LastOrDefault().listaParLevel3Colleta.Add(Mapper.Map<ParLevel3DTO>(_baseRepoParLevel3.GetById(level3Level2.ParLevel3_Id)));
                }

            }

            //non conf
            retorno.parNotConformityRuleXLevelDto = Mapper.Map<ParNotConformityRuleXLevelDTO>(_baseParNotConformityRuleXLevel.GetAll().FirstOrDefault(r => r.ParLevel1_Id == retorno.Id)) ?? new ParNotConformityRuleXLevelDTO();

            /*Reincidencia*/
            retorno.listParRelapseDto = Mapper.Map<List<ParRelapseDTO>>(_baseParRelapse.GetAll().Where(r => r.ParLevel1_Id == retorno.Id));

            return retorno;
        }


        #endregion


    }
}
