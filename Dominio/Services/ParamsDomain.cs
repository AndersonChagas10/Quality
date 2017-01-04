using Dominio.Interfaces.Services;
using Dominio.Interfaces.Repositories;
using DTO.DTO.Params;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System;

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
        private IBaseRepositoryNoLazyLoad<ParLevel1> _baseRepoParLevel1NLL;
        private IBaseRepositoryNoLazyLoad<ParLevel2> _baseRepoParLevel2NLL;
        private IBaseRepositoryNoLazyLoad<ParLevel3> _baseRepoParLevel3NLL;
        private IBaseRepositoryNoLazyLoad<ParLevel2Level1> _baseRepoParLevel2Level1;
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
        private IBaseRepository<ParCompany> _baseRepoParCompany;
        private IParLevel3Repository _repoParLevel3;
        /*Repo Especifico, manejam os itens*/
        private IParamsRepository _paramsRepo;

        public ParamsDomain(IBaseRepository<ParLevel1> baseRepoParLevel1,
                            IBaseRepository<ParLevel2> baseRepoParLevel2,
                            IBaseRepository<ParLevel3> baseRepoParLevel3,
                            IBaseRepositoryNoLazyLoad<ParLevel1> baseRepoParLevel1NoLazyLoad,
                            IBaseRepositoryNoLazyLoad<ParLevel2> baseRepoParLevel2NoLazyLoad,
                            IBaseRepositoryNoLazyLoad<ParLevel3> baseRepoParLevel3NoLazyLoad,
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
                            IBaseRepository<ParCriticalLevel> baseRepoParCriticalLevel,
                            IBaseRepository<ParCompany> baseRepoParCompany,
                            IBaseRepositoryNoLazyLoad<ParLevel2Level1> baseRepoParLevel2Level1)
        {
            _baseRepoParLevel2Level1 = baseRepoParLevel2Level1;
            _baseRepoParCompany = baseRepoParCompany;
            _baseRepoParCriticalLevel = baseRepoParCriticalLevel;
            _paramsRepo = paramsRepo;
            _baseRepoParCounterXLocal = baseRepoParCounterXLocal;
            _baseRepoParLevel1XHeaderField = baseRepoParLevel1XHeaderField;
            _baseRepoParMultipleValues = baseRepoParMultipleValues;
            _baseRepoParHeaderField = baseRepoParHeaderField;
            _baseRepoParLevel1 = baseRepoParLevel1;
            _baseRepoParLevel2 = baseRepoParLevel2;
            _baseRepoParLevel3 = baseRepoParLevel3;
            _baseRepoParLevel1NLL = baseRepoParLevel1NoLazyLoad;
            _baseRepoParLevel2NLL = baseRepoParLevel2NoLazyLoad;
            _baseRepoParLevel3NLL = baseRepoParLevel3NoLazyLoad;
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
            ParLevel1 saveParamLevel1 = Mapper.Map<ParLevel1>(paramsDto.parLevel1Dto);//ParLevel1
            List<ParGoal> listParGoal = Mapper.Map<List<ParGoal>>(paramsDto.parLevel1Dto.listParGoalLevel1);
            List<ParRelapse> listaReincidencia = Mapper.Map<List<ParRelapse>>(paramsDto.parLevel1Dto.listParRelapseDto);//Reincidencia do Level1
            List<ParHeaderField> listaParHEadField = Mapper.Map<List<ParHeaderField>>(paramsDto.listParHeaderFieldDto);//Cabeçalhos do Level1
            List<ParCounterXLocal> ListaParCounterLocal = Mapper.Map<List<ParCounterXLocal>>(paramsDto.parLevel1Dto.listParCounterXLocal);//Contadores do Level1
            List<ParLevel1XCluster> ListaParLevel1XCluster = Mapper.Map<List<ParLevel1XCluster>>(paramsDto.parLevel1Dto.listLevel1XClusterDto);//Clusters do Level1
            List<ParNotConformityRuleXLevel> listNonCoformitRule = Mapper.Map<List<ParNotConformityRuleXLevel>>(paramsDto.parLevel1Dto.listParNotConformityRuleXLevelDto);//Regra de NC do Level1

            /*Inativar*/
            List<int> removerHeadField = paramsDto.parLevel1Dto.removerParHeaderField;

            try
            {
                /*Enviando para repository salvar, envia todos, pois como existe transaction, faz rolback de tudo se der erro.*/
                _paramsRepo.SaveParLevel1(saveParamLevel1, listaParHEadField, ListaParLevel1XCluster, removerHeadField
                                            , ListaParCounterLocal, listNonCoformitRule, listaReincidencia, listParGoal);
            }
            catch (DbUpdateException e)
            {
                VerifyUniqueName(saveParamLevel1, e);
            }

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
           
            var parlevel1Dto = Mapper.Map<ParLevel1DTO>(parlevel1);
            parlevel1Dto.listParCounterXLocal = Mapper.Map<List<ParCounterXLocalDTO>>(parlevel1.ParCounterXLocal.OrderByDescending(r => r.IsActive));/*Contadores*/
            parlevel1Dto.listParGoalLevel1 = Mapper.Map<List<ParGoalDTO>>(parlevel1.ParGoal.OrderByDescending(r => r.IsActive));/*Meta*/
            parlevel1Dto.listLevel1XClusterDto = Mapper.Map<List<ParLevel1XClusterDTO>>(parlevel1.ParLevel1XCluster.OrderByDescending(r => r.IsActive));/*Clusters*/
            parlevel1Dto.listParLevel3Level2Level1Dto = Mapper.Map<List<ParLevel3Level2Level1DTO>>(parlevel1.ParLevel3Level2Level1);/*Level 2 e 3 vinculados*/
            parlevel1Dto.listParRelapseDto = Mapper.Map<List<ParRelapseDTO>>(parlevel1.ParRelapse.OrderByDescending(r => r.IsActive));/*Reincidencia*/
            parlevel1Dto.listParNotConformityRuleXLevelDto = Mapper.Map<List<ParNotConformityRuleXLevelDTO>>(parlevel1.ParNotConformityRuleXLevel.OrderByDescending(r => r.IsActive));/*Regra de alerta (Regra de NC)*/

            parlevel1Dto.cabecalhosInclusos = Mapper.Map<List<ParLevel1XHeaderFieldDTO>>(parlevel1.ParLevel1XHeaderField.Where(r => r.IsActive == true).OrderBy(r => r.IsActive));/*Cabeçalhos*/

            parlevel1Dto.parNotConformityRuleXLevelDto = new ParNotConformityRuleXLevelDTO();
            parlevel1Dto.CreateSelectListParamsViewModelListLevel(Mapper.Map<List<ParLevel2DTO>>(_baseRepoParLevel2NLL.GetAll()), parlevel1Dto.listParLevel3Level2Level1Dto);

            foreach (var i in parlevel1Dto.listParLevel3Level2Level1Dto)
            {
                i.ParLevel3Level2.ParLevel2 = null;
                i.ParLevel3Level2.ParCompany = null;
                i.ParLevel3Level2.ParLevel3 = null;
                i.ParLevel3Level2.ParLevel3Group = null;
                i.ParLevel1 = null;
            }

            foreach (var i in parlevel1Dto.listParRelapseDto)
            {
                i.parLevel1 = null;
                i.parLevel2 = null;
                i.parLevel3 = null;
            }

            return parlevel1Dto;
        }

        #endregion

        #region Level2

        public ParamsDTO AddUpdateLevel2(ParamsDTO paramsDto)
        {
            //paramsDto.parLevel1Dto.IsValid();
            ParLevel2 saveParamLevel2 = Mapper.Map<ParLevel2>(paramsDto.parLevel2Dto);
            paramsDto.parLevel2Dto.CriaListaSampleEvaluation();
            List<ParSample> saveParamSample = Mapper.Map<List<ParSample>>(paramsDto.parLevel2Dto.listSample);
            List<ParEvaluation> saveParamEvaluation = Mapper.Map<List<ParEvaluation>>(paramsDto.parLevel2Dto.listEvaluation);
            List<ParRelapse> listParRelapse = Mapper.Map<List<ParRelapse>>(paramsDto.parLevel2Dto.listParRelapseDto);/*Reincidencia*/
            List<ParLevel3Group> listaParLevel3Group = Mapper.Map<List<ParLevel3Group>>(paramsDto.parLevel2Dto.listParLevel3GroupDto);
            List<ParCounterXLocal> listParCounterXLocal = Mapper.Map<List<ParCounterXLocal>>(paramsDto.parLevel2Dto.listParCounterXLocal);/*Contadores*/
            List<ParNotConformityRuleXLevel> listNonCoformitRule = Mapper.Map<List<ParNotConformityRuleXLevel>>(paramsDto.parLevel2Dto.listParNotConformityRuleXLevelDto);/*Regra de alerta*/

            try
            {
                _paramsRepo.SaveParLevel2(saveParamLevel2, listaParLevel3Group, listParCounterXLocal, listNonCoformitRule, saveParamEvaluation, saveParamSample, listParRelapse);
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

            /*Avaliação e amostra*/
            level2.listEvaluation = Mapper.Map<List<ParEvaluationDTO>>(parLevel2.ParEvaluation);
            level2.listSample = Mapper.Map<List<ParSampleDTO>>(parLevel2.ParSample);
            level2.RecuperaListaSampleEvaluation();
            //paramsDto.parEvaluationDto = Mapper.Map<ParEvaluationDTO>(parLevel2.ParEvaluation.FirstOrDefault());
            //paramsDto.parSampleDto = Mapper.Map<ParSampleDTO>(parLevel2.ParSample.FirstOrDefault());

            level2.listParRelapseDto = Mapper.Map<List<ParRelapseDTO>>(parLevel2.ParRelapse.OrderByDescending(r => r.IsActive));/*Reincidencia*/
            level2.listParCounterXLocal = Mapper.Map<List<ParCounterXLocalDTO>>(parLevel2.ParCounterXLocal.OrderByDescending(r => r.IsActive));/*Contadores*/
            level2.listParNotConformityRuleXLevelDto = Mapper.Map<List<ParNotConformityRuleXLevelDTO>>(parLevel2.ParNotConformityRuleXLevel.OrderByDescending(r => r.IsActive));/*Regra de Alerta*/
            level2.listParLevel3Level2Dto = Mapper.Map<List<ParLevel3Level2DTO>>(parLevel2.ParLevel3Level2);/*Vinculo L3 L2*/

            /*Estas prop deveriam estar dentro do parlevel2*/
            paramsDto.parNotConformityRuleXLevelDto = new ParNotConformityRuleXLevelDTO();/*Regra de Alerta, sem isto dava estouro...*/
            
            /*Cria select Level 2 e 3 vinculados*/
            paramsDto.listParLevel3GroupDto = new List<ParLevel3GroupDTO>();
            level2.listParLevel3GroupDto = Mapper.Map<List<ParLevel3GroupDTO>>(parLevel2.ParLevel3Group.OrderByDescending(r => r.IsActive));
            level2.CreateSelectListParamsViewModelListLevel(Mapper.Map<List<ParLevel3DTO>>(_baseRepoParLevel3NLL.GetAll()), level2.listParLevel3Level2Dto);

            if (parLevel2.ParLevel3Level2.FirstOrDefault(r => r.ParLevel3_Id == level3Id) != null)/*Peso do vinculo*/
                level2.pesoDoVinculoSelecionado = parLevel2.ParLevel3Level2.FirstOrDefault(r => r.ParLevel3_Id == level3Id).Weight;
            else
                level2.pesoDoVinculoSelecionado = 0;

            paramsDto.parLevel2Dto = level2;
            //foreach (var i in paramsDto.parLevel2Dto.listParLevel3Level2Dto)
            //{
            //    i.ParLevel2 = null;
            //    i.ParLevel3 = null;
            //    i.ParCompany = null;
            //}
            return paramsDto;
        }

        #endregion

        #region Level3

        public ParamsDTO AddUpdateLevel3(ParamsDTO paramsDto)
        {
            //paramsDto.parLevel1Dto.IsValid();
            ParLevel3 saveParamLevel3 = Mapper.Map<ParLevel3>(paramsDto.parLevel3Dto);
            
            #region Level3Value

            if (paramsDto.parLevel3Dto.listLevel3Value != null)
                if (paramsDto.parLevel3Dto.listLevel3Value.Count() > 0)
                    paramsDto.parLevel3Dto.listLevel3Value.ForEach(r => r.preparaParaInsertEmBanco());

            List<ParLevel3Value> listSaveParamLevel3Value = Mapper.Map<List<ParLevel3Value>>(paramsDto.parLevel3Dto.listLevel3Value);

            #endregion

            #region Reincidencia

            List<ParRelapse> listParRelapse = Mapper.Map<List<ParRelapse>>(paramsDto.parLevel3Dto.listParRelapseDto);/*Reincidencia*/

            #endregion

            #region Peso de vinculo

            if (paramsDto.parLevel3Dto.listLevel3Level2 != null)
                if (paramsDto.parLevel3Dto.listLevel3Level2.Count() > 0)
                    paramsDto.parLevel3Dto.listLevel3Level2.ForEach(r => r.preparaParaInsertEmBanco());

            List<ParLevel3Level2> parLevel3Level2peso = Mapper.Map<List<ParLevel3Level2>>(paramsDto.parLevel3Dto.listLevel3Level2.Where(r => r.IsActive == true));
            List<ParLevel3Level2> parLevel3Level2pesoInativo = Mapper.Map<List<ParLevel3Level2>>(paramsDto.parLevel3Dto.listLevel3Level2.Where(r => r.IsActive == false));
            var existeLevel3VinculadoComLevel1 = _baseRepoParLevel3Level2Level1.GetAll();/*Verifica se existe vinculos com L3 a L1*/

            #endregion

            try
            {
                foreach (var i in parLevel3Level2pesoInativo)
                {
                    var sql = "DELETE FROM ParLevel3Level2Level1 WHERE ParLevel3Level2_Id = " + i.Id + "; DELETE FROM ParLevel3Level2 WHERE id = " + i.Id;
                   _paramsRepo.ExecuteSql(sql);
                }

                _paramsRepo.SaveParLevel3(saveParamLevel3, listSaveParamLevel3Value, listParRelapse, parLevel3Level2peso);

                foreach (var l32 in parLevel3Level2peso)
                    if (existeLevel3VinculadoComLevel1.FirstOrDefault(r => r.ParLevel3Level2_Id == l32.Id) == null)
                        AddVinculoL1L2(existeLevel3VinculadoComLevel1.FirstOrDefault(r=> parLevel3Level2peso.Any(c=>c.Id == r.ParLevel3Level2_Id)).ParLevel1_Id, parLevel3Level2peso.FirstOrDefault().ParLevel2_Id, saveParamLevel3.Id);

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
            var parlevel3 = _baseRepoParLevel3.GetById(idParLevel3);/*ParLevel3*/
            var level3 = Mapper.Map<ParLevel3DTO>(parlevel3);//Level3
            level3.listParRelapseDto = Mapper.Map<List<ParRelapseDTO>>(parlevel3.ParRelapse.OrderByDescending(r => r.IsActive));/*Reincidencia*/
            level3.listGroupsLevel2 = Mapper.Map<List<ParLevel3GroupDTO>>(_baseParLevel3Group.GetAll().Where(r => r.ParLevel2_Id == idParLevel2 && r.IsActive == true).ToList());//DDL
            level3.listLevel3Level2 = Mapper.Map<List<ParLevel3Level2DTO>>(parlevel3.ParLevel3Level2.Where(r => r.ParLevel2_Id == idParLevel2 && r.ParLevel3_Id == level3.Id).OrderByDescending(r => r.IsActive));

            if (level3.listLevel3Level2.Count() > 0)/*Id do grupo selecionado no vinculo Level 3 com level 2*/
                level3.hasVinculo = true;

            /*ParLevel 3 Value*/
            retorno.parLevel3Value = new ParLevel3ValueDTO();
            level3.listLevel3Value = Mapper.Map<List<ParLevel3ValueDTO>>(parlevel3.ParLevel3Value.OrderByDescending(r => r.IsActive));
            foreach (var Level3Value in level3.listLevel3Value)
                Level3Value.PreparaGet();

            retorno.parLevel3Dto = level3;

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
            lock (this)
            {
                var DdlParConsolidation = Mapper.Map<List<ParConsolidationTypeDTO>>(_baseParConsolidationType.GetAllAsNoTracking());

                var DdlFrequency = Mapper.Map<List<ParFrequencyDTO>>(_baseParFrequency.GetAllAsNoTracking());
                var DdlparLevel1 = Mapper.Map<List<ParLevel1DTO>>(_baseRepoParLevel1NLL.GetAllAsNoTracking());
                var DdlparLevel2 = Mapper.Map<List<ParLevel2DTO>>(_baseRepoParLevel2NLL.GetAllAsNoTracking());
                var DdlparLevel3 = Mapper.Map<List<ParLevel3DTO>>(_baseRepoParLevel3NLL.GetAllAsNoTracking());

                var DdlparCluster = Mapper.Map<List<ParClusterDTO>>(_baseParCluster.GetAllAsNoTracking());
                var DdlparLevelDefinition = Mapper.Map<List<ParLevelDefinitonDTO>>(_baseParLevelDefiniton.GetAllAsNoTracking());
                var DdlParFieldType = Mapper.Map<List<ParFieldTypeDTO>>(_baseParFieldType.GetAllAsNoTracking());
                var DdlParDepartment = Mapper.Map<List<ParDepartmentDTO>>(_baseParDepartment.GetAllAsNoTracking());
                var DdlParNotConformityRule = Mapper.Map<List<ParNotConformityRuleDTO>>(_baseParNotConformityRule.GetAllAsNoTracking());

                var DdlParLocal_Level1 = Mapper.Map<List<ParLocalDTO>>(_baseParLocal.GetAllAsNoTracking().Where(p => p.Level == 1));
                var DdlParLocal_Level2 = Mapper.Map<List<ParLocalDTO>>(_baseParLocal.GetAllAsNoTracking().Where(p => p.Level == 2));

                var DdlParCounter_Level1 = Mapper.Map<List<ParCounterDTO>>(_baseParCounter.GetAllAsNoTracking().Where(p => p.Level == 1));
                var DdlParCounter_Level2 = Mapper.Map<List<ParCounterDTO>>(_baseParCounter.GetAllAsNoTracking().Where(p => p.Level == 2));

                var DdlParLevel3InputType = Mapper.Map<List<ParLevel3InputTypeDTO>>(_baseParLevel3InputType.GetAllAsNoTracking());
                var DdlParMeasurementUnit = Mapper.Map<List<ParMeasurementUnitDTO>>(_baseParMeasurementUnit.GetAllAsNoTracking());

                var DdlParLevel3BoolFalse = Mapper.Map<List<ParLevel3BoolFalseDTO>>(_baseParLevel3BoolFalse.GetAllAsNoTracking());
                var DdlParLevel3BoolTrue = Mapper.Map<List<ParLevel3BoolTrueDTO>>(_baseParLevel3BoolTrue.GetAllAsNoTracking());

                var DdlparCrit = Mapper.Map<List<ParCriticalLevelDTO>>(_baseRepoParCriticalLevel.GetAllAsNoTracking());

                var DdlparCompany = Mapper.Map<List<ParCompanyDTO>>(_baseRepoParCompany.GetAllAsNoTracking());

                var retorno = new ParamsDdl();

                retorno.SetDdlsNivel123(DdlparLevel1,
                                DdlparLevel2,
                                DdlparLevel3);

                retorno.SetDdls(DdlParConsolidation, DdlFrequency, DdlparCluster, DdlparLevelDefinition, DdlParFieldType, DdlParDepartment, DdlParCounter_Level1,
                                DdlParLocal_Level1, DdlParCounter_Level2, DdlParLocal_Level2, DdlParNotConformityRule, DdlParLevel3InputType, DdlParMeasurementUnit,
                                DdlParLevel3BoolFalse, DdlParLevel3BoolTrue, DdlparCrit, DdlparCompany);
                return retorno;
            }
        }

        #endregion

        #region Vinculo ParLevel2Level1

        public List<ParLevel3Level2Level1DTO> AddVinculoL1L2(int idLevel1, int idLevel2, int idLevel3)
        {
            var allLevel1Level2 = _baseRepoParLevel3Level2.GetAll();
            /*Verifica se existe vinculo no level2 e level 3 selecionado na tela*/
            var listExistsL3L2 = allLevel1Level2.Where(r => r.ParLevel2_Id == idLevel2 && r.ParLevel3_Id == idLevel3);
            if (listExistsL3L2 == null)
                if (listExistsL3L2.Count() <= 0)
                    throw new ExceptionHelper("É necessário vincular o level3 ao level 2 antes de realizar esta operação.");

            var listObjToSave = new List<ParLevel3Level2Level1DTO>();
            /*Se o vinculo level2 e level3 Cria objeto do Level3Level2Level1*/
            foreach (var existsL3L2 in listExistsL3L2)
            {
                var objToSave = new ParLevel3Level2Level1();
                objToSave.ParLevel1_Id = idLevel1;
                objToSave.ParLevel3Level2_Id = existsL3L2.Id;
                objToSave.Active = true;

                /*Verifica se o vinculo ja existe, se já existe, ele ALTERA colocando o ID no objeto NOVO*/
                var existsLevel3Level2Level1 = _baseRepoParLevel3Level2Level1.GetAll().FirstOrDefault(r => r.ParLevel3Level2_Id == existsL3L2.Id);
                if (existsLevel3Level2Level1 != null)
                {
                    existsLevel3Level2Level1.ParLevel1_Id = objToSave.ParLevel1_Id;
                    existsLevel3Level2Level1.ParLevel3Level2_Id = objToSave.ParLevel3Level2_Id;
                    existsLevel3Level2Level1.Active = objToSave.Active;
                    _baseRepoParLevel3Level2Level1.AddOrUpdate(existsLevel3Level2Level1);/*Salva Vinculo Level3Level2Level1*/
                    objToSave = existsLevel3Level2Level1;
                }
                else
                {
                    _baseRepoParLevel3Level2Level1.AddOrUpdate(objToSave);/*Salva Vinculo Level3Level2Level1*/
                }

                /*Objeto ParLEvel2Level1 que é salvo caso não exista vinculo com key level1 e level2 já registrados na tabela ParLevel2Level1*/
                var level2level1 = new ParLevel2Level1();
                level2level1.IsActive = true;
                level2level1.ParCompany_Id = null;
                level2level1.ParLevel1_Id = objToSave.ParLevel1_Id;
                level2level1.ParLevel2_Id = objToSave.ParLevel3Level2.ParLevel2_Id;

                var existeParLevel2Level1 = _baseRepoParLevel2Level1.GetAll().FirstOrDefault(r => r.ParLevel1_Id == level2level1.ParLevel1_Id && r.ParLevel2_Id == level2level1.ParLevel2_Id);
                if (existeParLevel2Level1 == null)
                    _baseRepoParLevel2Level1.AddOrUpdate(level2level1);


                listObjToSave.Add(Mapper.Map<ParLevel3Level2Level1DTO>(objToSave));
            }

            return listObjToSave;

        }

        public bool VerificaShowBtnRemVinculoL1L2(int idLevel1, int idLevel2)
        {
            var response = false;
            using (var db = new SgqDbDevEntities())
            {
                var sql1 = "SELECT * FROM ParLevel3Level2Level1 WHERE ParLevel1_Id = " + idLevel1 + " AND ParLevel3Level2_Id IN (select id from ParLevel3Level2 where parlevel2_id = " + idLevel2 + ")";
                var result1 = db.Database.SqlQuery<ParLevel3Level2Level1>(sql1).ToList();
                var result2 = db.ParLevel2Level1.Where(r => r.ParLevel1_Id == idLevel1 && r.ParLevel2_Id == idLevel2).ToList();
                if (result1?.Count() > 0 || result2?.Count() > 0)
                    response = true;
                else
                    response = false;
            }
            return response;
        }

        public bool RemVinculoL1L2(int idLevel1, int idLevel2)
        {
            //throw new Exception("teste");

            using (var db = new SgqDbDevEntities())
            {
                var sql1 = "SELECT * FROM ParLevel3Level2Level1 WHERE ParLevel1_Id = " + idLevel1 + " AND ParLevel3Level2_Id IN (select id from ParLevel3Level2 where parlevel2_id = " + idLevel2 + ")";
                var sql2 = "SELECT * FROM ParLevel2Level1 WHERE ParLevel1_Id = " + idLevel1 + " AND ParLevel2_Id = " + idLevel2;
                var result1 = db.Database.SqlQuery<ParLevel3Level2Level1>(sql1).ToList();
                var result2 = db.ParLevel2Level1.Where(r=>r.ParLevel1_Id == idLevel1 && r.ParLevel2_Id == idLevel2).ToList();
                if (result1?.Count() > 0 || result2?.Count() > 0)
                {
                    if (result1?.Count() > 0)
                        sql1 = "DELETE FROM ParLevel3Level2Level1 WHERE ParLevel1_Id = " + idLevel1 + " AND ParLevel3Level2_Id IN (select id from ParLevel3Level2 where parlevel2_id = " + idLevel2 + ")";
                    if (result2?.Count() > 0)
                        sql2 = "DELETE FROM ParLevel2Level1 WHERE ParLevel1_Id = " + idLevel1 + " AND ParLevel2_Id = " + idLevel2;
                }
                else
                {
                    return false;
                }

                var r1 = db.Database.ExecuteSqlCommand(sql1);
                var r2 = db.Database.ExecuteSqlCommand(sql2);
            }

            return true;
        }

        #endregion

        #region Vinculo L3L2

        public ParLevel3Level2DTO AddVinculoL3L2(int idLevel2, int idLevel3, decimal peso, int? groupLevel2 = 0)
        {
            ParLevel3Level2 objLelvel2Level3ToSave;
            var level2 = _baseRepoParLevel2.GetById(idLevel2);

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
                objLelvel2Level3ToSave.Weight = existente.Weight;/*Mantem o PESO*/
                objLelvel2Level3ToSave.ParLevel3Group_Id = groupLevel2 == 0 ? null : groupLevel2;
            }
            else
            {
                objLelvel2Level3ToSave.Weight = 1;
            }
            objLelvel2Level3ToSave.IsActive = true;
            _baseRepoParLevel3Level2.AddOrUpdate(objLelvel2Level3ToSave);
            ParLevel3Level2DTO objtReturn = Mapper.Map<ParLevel3Level2DTO>(objLelvel2Level3ToSave);

            

            return objtReturn;
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
            retorno.listLevel1XClusterDto = Mapper.Map<List<ParLevel1XClusterDTO>>(parlevel1.ParLevel1XCluster.Where(r => r.IsActive == true));

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
