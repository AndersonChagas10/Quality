using Dominio.Interfaces.Services;
using Dominio.Interfaces.Repositories;
using DTO.DTO.Params;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System;
using System.Data;

namespace Dominio.Services
{
    /// <summary>
    /// Rns para parametrização.
    /// </summary>
    public class ParamsDomain : IParamsDomain
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

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
        private IBaseRepository<ParLevel3Group> _baseParLevel3Group; //
        private IBaseRepository<ParLocal> _baseParLocal;
        private IBaseRepository<ParCounter> _baseParCounter;
        private IBaseRepository<ParCounterXLocal> _baseParCounterXLocal;
        private IBaseRepository<ParRelapse> _baseParRelapse;
        private IBaseRepository<ParNotConformityRule> _baseParNotConformityRule;
        private IBaseRepository<ParNotConformityRuleXLevel> _baseParNotConformityRuleXLevel;
        private IBaseRepository<ParCompany> _baseParCompany;
        private IBaseRepository<ParHeaderField> _baseRepoParHeaderField;
        private IBaseRepository<ParLevel1XHeaderField> _baseRepoParLevel1XHeaderField; //
        private IBaseRepository<ParLevel2XHeaderField> _baseRepoParLevel2XHeaderField; //
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
        private IBaseRepositoryNoLazyLoad<ParLevel3Level2Level1> _baseRepoParLevel3Level2Level1NNL;
        private IBaseRepository<ParLevel3Level2Level1> _baseRepoParLevel3Level2Level1;
        private IBaseRepository<ParCriticalLevel> _baseRepoParCriticalLevel;
        private IBaseRepository<ParCompany> _baseRepoParCompany;
        private IBaseRepository<Equipamentos> _baseRepoEquipamentos;
        private IBaseRepository<ParScoreType> _baseRepoParScore;
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
                            IBaseRepository<ParLevel2XHeaderField> baseRepoParLevel2XHeaderField,
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
                            IBaseRepository<Equipamentos> baseRepoEquipamentos,
                            IBaseRepositoryNoLazyLoad<ParLevel2Level1> baseRepoParLevel2Level1,
                            IBaseRepository<ParScoreType> baseRepoParScore,
                            IBaseRepositoryNoLazyLoad<ParLevel3Level2Level1> baseRepoParLevel3Level2Level1NNL
            )
        {
            _baseRepoParLevel3Level2Level1NNL = baseRepoParLevel3Level2Level1NNL;
            _baseRepoParScore = baseRepoParScore;
            _baseRepoParLevel2Level1 = baseRepoParLevel2Level1;
            _baseRepoParCompany = baseRepoParCompany;
            _baseRepoEquipamentos = baseRepoEquipamentos;
            _baseRepoParCriticalLevel = baseRepoParCriticalLevel;
            _paramsRepo = paramsRepo;
            _baseRepoParCounterXLocal = baseRepoParCounterXLocal;
            _baseRepoParLevel1XHeaderField = baseRepoParLevel1XHeaderField;
            _baseRepoParLevel2XHeaderField = baseRepoParLevel2XHeaderField;
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

            db.Configuration.LazyLoadingEnabled = false;
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

            //paramsDto.parLevel1Dto.IsValid();
            ParLevel1 saveParamLevel1 = Mapper.Map<ParLevel1>(paramsDto.parLevel1Dto);//ParLevel1
            List<ParGoal> listParGoal = Mapper.Map<List<ParGoal>>(paramsDto.parLevel1Dto.listParGoalLevel1);
            List<ParRelapse> listaReincidencia = Mapper.Map<List<ParRelapse>>(paramsDto.parLevel1Dto.listParRelapseDto);//Reincidencia do Level1
            List<ParHeaderField> listaParHEadField = Mapper.Map<List<ParHeaderField>>(paramsDto.listParHeaderFieldDto);//Cabeçalhos do Level1
            List<ParCounterXLocal> ListaParCounterLocal = Mapper.Map<List<ParCounterXLocal>>(paramsDto.parLevel1Dto.listParCounterXLocal);//Contadores do Level1
            List<ParLevel1XCluster> ListaParLevel1XCluster = Mapper.Map<List<ParLevel1XCluster>>(paramsDto.parLevel1Dto.listLevel1XClusterDto);//Clusters do Level1
            List<ParNotConformityRuleXLevel> listNonCoformitRule = Mapper.Map<List<ParNotConformityRuleXLevel>>(paramsDto.parLevel1Dto.listParNotConformityRuleXLevelDto);//Regra de NC do Level1

            if (saveParamLevel1.ParScoreType_Id <= 0)
                saveParamLevel1.ParScoreType_Id = null;

            if (paramsDto.listParHeaderFieldDto != null)
                foreach (var i in paramsDto.listParHeaderFieldDto.Where(r => !string.IsNullOrEmpty(r.DefaultOption)))
                    paramsDto.listParHeaderFieldDto.ForEach(r => r.parMultipleValuesDto.FirstOrDefault(c => c.Name.Equals(i.DefaultOption)).IsDefaultOption = true);

            /*Inativar*/
            List<int> removerHeadField = paramsDto.parLevel1Dto.removerParHeaderField;

            try
            {
                /*Enviando para repository salvar, envia todos, pois como existe transaction, faz rolback de tudo se der erro.*/
                _paramsRepo.SaveParLevel1(saveParamLevel1, listaParHEadField, ListaParLevel1XCluster, removerHeadField
                                            , ListaParCounterLocal, listNonCoformitRule, listaReincidencia, listParGoal);

                if (DTO.GlobalConfig.Brasil)
                {
                    if (paramsDto.parLevel1Dto.IsSpecific)
                    {
                        var query = "UPDATE PARLEVEL1 SET {0} = {1} WHERE id = {2} SELECT 1";
                        var queryExcute = string.Empty;
                        queryExcute = string.Format(query, "AllowAddLevel3", paramsDto.parLevel1Dto.AllowAddLevel3 ? 1 : 0, paramsDto.parLevel1Dto.Id);
                        db.Database.ExecuteSqlCommand(queryExcute);
                        queryExcute = string.Format(query, "AllowEditPatternLevel3Task", paramsDto.parLevel1Dto.AllowEditPatternLevel3Task ? 1 : 0, paramsDto.parLevel1Dto.Id);
                        db.Database.ExecuteSqlCommand(queryExcute);
                        queryExcute = string.Format(query, "AllowEditWeightOnLevel3", paramsDto.parLevel1Dto.AllowEditWeightOnLevel3 ? 1 : 0, paramsDto.parLevel1Dto.Id);
                        db.Database.ExecuteSqlCommand(queryExcute);
                    }
                    if (paramsDto.parLevel1Dto.IsRecravacao)
                    {
                        var query = "UPDATE PARLEVEL1 SET {0} = {1} WHERE id = {2} SELECT 1";
                        var queryExcute = string.Empty;
                        queryExcute = string.Format(query, "IsRecravacao", paramsDto.parLevel1Dto.IsRecravacao ? 1 : 0, paramsDto.parLevel1Dto.Id);
                        db.Database.ExecuteSqlCommand(queryExcute);
                    }
                }
            }
            catch (DbUpdateException e)
            {
                VerifyUniqueName(saveParamLevel1, e);
            }

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
            ParLevel1DTO parlevel1Dto;


            db.Configuration.LazyLoadingEnabled = false;

            #region Query

            var parlevel1 = _baseRepoParLevel1.GetById(idParLevel1);
            var counter = parlevel1.ParCounterXLocal.Where(r => r.IsActive == true).OrderByDescending(r => r.IsActive).ToList();
            var goal = parlevel1.ParGoal.Where(r => r.IsActive == true).OrderByDescending(r => r.IsActive).ToList();
            var cluster = parlevel1.ParLevel1XCluster.Where(r => r.IsActive == true).OrderByDescending(r => r.IsActive).ToList();
            var listL3L2L1 = db.ParLevel3Level2Level1.Include("ParLevel3Level2").AsNoTracking().Where(r => r.Active == true && r.ParLevel1_Id == idParLevel1).ToList();
            var relapse = parlevel1.ParRelapse.Where(r => r.IsActive == true).OrderByDescending(r => r.IsActive).ToList();
            var notConformityrule = parlevel1.ParNotConformityRuleXLevel.Where(r => r.IsActive == true).OrderByDescending(r => r.IsActive).ToList();
            var cabecalhos = parlevel1.ParLevel1XHeaderField.Where(r => r.IsActive == true).OrderBy(r => r.IsActive).ToList();
            var level2List = _baseRepoParLevel2NLL.GetAll().Where(r => r.IsActive == true);

            #endregion

            #region DTO

            parlevel1Dto = Mapper.Map<ParLevel1DTO>(parlevel1);
            parlevel1Dto.listParCounterXLocal = Mapper.Map<List<ParCounterXLocalDTO>>(counter);/*Contadores*/
            parlevel1Dto.listParGoalLevel1 = Mapper.Map<List<ParGoalDTO>>(goal);/*Meta*/
            parlevel1Dto.listLevel1XClusterDto = Mapper.Map<List<ParLevel1XClusterDTO>>(cluster);/*Clusters*/
            parlevel1Dto.listParLevel3Level2Level1Dto = Mapper.Map<List<ParLevel3Level2Level1DTO>>(listL3L2L1);/*Level 2 e 3 vinculados*/
            parlevel1Dto.listParRelapseDto = Mapper.Map<List<ParRelapseDTO>>(relapse);/*Reincidencia*/
            parlevel1Dto.listParNotConformityRuleXLevelDto = Mapper.Map<List<ParNotConformityRuleXLevelDTO>>(notConformityrule);/*Regra de alerta (Regra de NC)*/
            parlevel1Dto.cabecalhosInclusos = Mapper.Map<List<ParLevel1XHeaderFieldDTO>>(cabecalhos);/*Cabeçalhos*/
            parlevel1Dto.parNotConformityRuleXLevelDto = new ParNotConformityRuleXLevelDTO();

            parlevel1Dto.CreateSelectListParamsViewModelListLevel(Mapper.Map<List<ParLevel2DTO>>(level2List), parlevel1Dto.listParLevel3Level2Level1Dto);

            if (DTO.GlobalConfig.Brasil)
            {
                var query = "SELECT {0} FROM  PARLEVEL1 WHERE id = {1}";
                var queryExcute = string.Empty;
                queryExcute = string.Format(query, "AllowAddLevel3", parlevel1Dto.Id);
                parlevel1Dto.AllowAddLevel3 = db.Database.SqlQuery<bool>(queryExcute).FirstOrDefault();
                queryExcute = string.Format(query, "AllowEditPatternLevel3Task", parlevel1Dto.Id);
                parlevel1Dto.AllowEditPatternLevel3Task = db.Database.SqlQuery<bool>(queryExcute).FirstOrDefault();
                queryExcute = string.Format(query, "AllowEditWeightOnLevel3", parlevel1Dto.Id);
                parlevel1Dto.AllowEditWeightOnLevel3 = db.Database.SqlQuery<bool>(queryExcute).FirstOrDefault();

                //Recravação
                queryExcute = string.Format(query, "IsRecravacao", parlevel1Dto.Id);
                parlevel1Dto.IsRecravacao = db.Database.SqlQuery<bool>(queryExcute).FirstOrDefault();
            }

            #endregion

            #region Depreciado

            //foreach (var i in parlevel1Dto.listParLevel3Level2Level1Dto)
            //{
            //    i.ParLevel3Level2.ParLevel2 = null;
            //    i.ParLevel3Level2.ParCompany = null;
            //    i.ParLevel3Level2.ParLevel3 = null;
            //    i.ParLevel3Level2.ParLevel3Group = null;
            //    i.ParLevel1 = null;
            //}

            foreach (var i in parlevel1Dto.listParRelapseDto)
            {
                i.parLevel1 = null;
                i.parLevel2 = null;
                i.parLevel3 = null;
            }

            #endregion


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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdParLevel2"></param>
        /// <returns></returns>
        public ParamsDTO GetLevel2(int idParLevel2, int level3Id, int level1Id)
        {

            #region Query

            var paramsDto = new ParamsDTO();
            var parLevel2 = _baseRepoParLevel2.GetById(idParLevel2);
            var level2 = Mapper.Map<ParLevel2DTO>(parLevel2);
            var headerFieldLevel1 = db.ParLevel1XHeaderField.Include("ParHeaderField").ToList();
            var headerFieldLevel2 = db.ParLevel2XHeaderField.Where(r => r.IsActive == true).ToList();
            var evaluation = parLevel2.ParEvaluation.Where(r => r.IsActive == true);
            var relapse = parLevel2.ParRelapse.Where(r => r.IsActive == true).OrderByDescending(r => r.IsActive);
            var counter = parLevel2.ParCounterXLocal.Where(r => r.IsActive == true).OrderByDescending(r => r.IsActive);
            var nonConformityrule = parLevel2.ParNotConformityRuleXLevel.Where(r => r.IsActive == true).OrderByDescending(r => r.IsActive);
            var headerAdd = headerFieldLevel1.Where(r => r.IsActive == true && r.ParLevel1_Id == level1Id);
            var headerRemove = headerFieldLevel2.Where(r => r.IsActive == true && r.ParLevel1_Id == level1Id && r.ParLevel2_Id == idParLevel2);
            var parLevel3Group = parLevel2.ParLevel3Group.Where(r => r.IsActive == true).OrderByDescending(r => r.IsActive);
            /*Avaliação e amostra*/
            level2.listEvaluation = Mapper.Map<List<ParEvaluationDTO>>(evaluation);
            if (parLevel2.ParSample.Count() > 0)
                level2.listSample = Mapper.Map<List<ParSampleDTO>>(parLevel2.ParSample.Where(r => r.IsActive == true));

            #endregion

            #region Mappers, Rn e DropDown Level3

            level2.RecuperaListaSampleEvaluation();
            level2.listParRelapseDto = Mapper.Map<List<ParRelapseDTO>>(relapse);/*Reincidencia*/
            level2.listParCounterXLocal = Mapper.Map<List<ParCounterXLocalDTO>>(counter);/*Contadores*/
            level2.listParNotConformityRuleXLevelDto = Mapper.Map<List<ParNotConformityRuleXLevelDTO>>(nonConformityrule);/*Regra de Alerta*/
            level2.cabecalhosInclusos = Mapper.Map<List<ParLevel1XHeaderFieldDTO>>(headerAdd);/*Cabeçalhos do Level 1*/
            level2.cabecalhosExclusos = Mapper.Map<List<ParLevel2XHeaderFieldDTO>>(headerRemove);/*Cabeçalhos não permitidos no Level 2*/

            #region DropDown - Level3

            /*Todos os Vinculos com este level2 / level3.*/
            var vinculosComOLevel2 = parLevel2.ParLevel3Level2.Where(r => r.IsActive == true);/*Vinculo L3 L2*/

            /*Se houver level 1 selecionado na tela filtro somente os que estão vinculados com level2 / Level3, e tem id do level 1 em ParLevel3Level2level1*/
            if (level1Id > 0)
            {
                //var teste = db.ParLevel3Level2Level1.Where(r => r.ParLevel1_Id == level1Id);
                vinculosComOLevel2 = db.ParLevel3Level2.Where(r => r.ParLevel2_Id == idParLevel2 && r.IsActive == true && r.ParLevel3Level2Level1.Any(c => c.ParLevel1_Id == level1Id)).ToList();
            }

            /*Depreciado*/
            //vinculosComOLevel2 = vinculosComOLevel2.Where(r => r.ParLevel3Level2Level1.Any(c => c.ParLevel1_Id == level1Id));
            //else if(level1Id > 0 && level3Id <= 0)
            //    vinculosComOLevel2 = _baseRepoParLevel2Level1.GetAll().Where(r=>r.ParLevel1_Id == level1Id && r.ParLevel2_Id == level2.Id ).Select(r=>r.)

            level2.listParLevel3Level2Dto = Mapper.Map<List<ParLevel3Level2DTO>>(vinculosComOLevel2);
            level2.CreateSelectListParamsViewModelListLevel(Mapper.Map<List<ParLevel3DTO>>(_baseRepoParLevel3NLL.GetAll().Where(r => r.IsActive == true)), level2.listParLevel3Level2Dto);

            #endregion

            paramsDto.parNotConformityRuleXLevelDto = new ParNotConformityRuleXLevelDTO();/*Estas prop deveriam estar dentro do parlevel2 Regra de Alerta, sem isto dava estouro...*/
            paramsDto.listParLevel3GroupDto = new List<ParLevel3GroupDTO>();/*Cria select Level 2 e 3 vinculados*/
            level2.listParLevel3GroupDto = Mapper.Map<List<ParLevel3GroupDTO>>(parLevel3Group);

            if (parLevel2.ParLevel3Level2.FirstOrDefault(r => r.ParLevel3_Id == level3Id && r.IsActive == true) != null)/*Peso do vinculo*/
                level2.pesoDoVinculoSelecionado = parLevel2.ParLevel3Level2.FirstOrDefault(r => r.ParLevel3_Id == level3Id && r.IsActive == true).Weight;
            else
                level2.pesoDoVinculoSelecionado = 0;

            paramsDto.parLevel2Dto = level2;
            paramsDto.parLevel2Dto.listParLevel3Level2Dto = null;

            if (level1Id > 0)
            {
                var possuiVinculoComLevel1 = db.ParLevel2Level1.Where(r => r.ParLevel1_Id == level1Id && r.ParLevel2_Id == level2.Id);
                if (possuiVinculoComLevel1 != null && possuiVinculoComLevel1.Count() > 0)
                    paramsDto.parLevel2Dto.isVinculado = true;
            }
            if (level1Id > 0)/*Caso exista Level1 Selecionado, é necessario verificar regras especificas para este ao mostrar os fields do level2*/
                paramsDto.parLevel2Dto.RegrasParamsLevel1(Mapper.Map<ParLevel1DTO>(db.ParLevel1.FirstOrDefault(r => r.Id == level1Id)));//Configura regras especificas do level2 de acordo com level1.


            #endregion

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
                {
                    paramsDto.parLevel3Dto.listLevel3Level2.ForEach(r => r.preparaParaInsertEmBanco());
                }

            List<ParLevel3Level2> parLevel3Level2peso = Mapper.Map<List<ParLevel3Level2>>(paramsDto.parLevel3Dto.listLevel3Level2);

            #endregion

            #region Save / Update

            try
            {

                _paramsRepo.SaveParLevel3(saveParamLevel3, listSaveParamLevel3Value, listParRelapse, parLevel3Level2peso?.ToList(), paramsDto.level1Selected);

                if (paramsDto.parLevel3Dto.ParLevel3Value_OuterList != null)
                    foreach (var i in paramsDto.parLevel3Dto.ParLevel3Value_OuterList)
                    {
                        if (i.Id <= 0)
                        {
                            i.ParLevel3_Id = saveParamLevel3.Id;
                            i.ParLevel3_Name = saveParamLevel3.Name;

                            var queryInsertParLevel3Value_OuterList = string.Format(@"
                            INSERT INTO [dbo].[ParLevel3Value_Outer]
                                (
                                    AddDate
                                    ,AlterDate
                                    ,IsActive
                                    ,OuterEmpresa_Id
                                    ,OuterEmpresa_Text
                                    ,OuterLevel3_Id
                                    ,OuterLevel3_Text
                                    ,OuterLevel3Value_Id
                                    ,OuterLevel3Value_Text
                                    ,OuterLevel3ValueIntervalMaxValue
                                   ,OuterLevel3ValueIntervalMinValue
                                   ,Operator
                                   ,[Order]
                                   ,ParLevel3_Id
                                   ,ParLevel3_Name
                                   ,ParLevel3InputType_Id
                                   ,ParLevel3InputType_Name
                                   ,ParCompany_Id
                                   ,ParCompany_Name
                                   ,ParMeasurementUnit_Id
                                   ,ParMeasurementUnit_Name
                               )
                           VALUES
                               (
                                   GETDATE()
                                   ,null
                                   ,1
                                   ,{0}
                                   ,N'{1}'
                                   ,{2}
                                   ,N'{3}'
                                   ,{4}
                                   ,N'{5}'
                                   ,{6}
                                   ,{7}
                                   ,N'{8}'
                                   ,{9}
                                   ,{10}
                                   ,N'{11}'        
                                   ,{12}
                                   ,N'{13}'
                                   ,{14}
                                   ,N'{15}'
                                   ,{16}
                                   ,N'{17}'
                               );
                           SELECT SCOPE_IDENTITY()"
                            , i.OuterEmpresa_Id
                            , i.OuterEmpresa_Text
                            , i.OuterLevel3_Id
                            , i.OuterLevel3_Text
                            , i.OuterLevel3Value_Id
                            , i.OuterLevel3Value_Text
                            , i.OuterLevel3ValueIntervalMaxValue
                            , i.OuterLevel3ValueIntervalMinValue
                            , i.Operator
                            , i.Order
                            , i.ParLevel3_Id
                            , i.ParLevel3_Name
                            , i.ParLevel3InputType_Id
                            , i.ParLevel3InputType_Name
                            , i.ParCompany_Id
                            , i.ParCompany_Name
                            , i.ParMeasurementUnit_Id
                            , i.ParMeasurementUnit_Name
                            );

                            var idSaved = db.Database.SqlQuery<decimal>(queryInsertParLevel3Value_OuterList).FirstOrDefault();
                            i.Id = int.Parse(idSaved.ToString());
                        }
                        else
                        {
                            var queryUpdateParLevel3Value_OuterList = string.Format(@"
                                UPDATE ParLevel3Value_Outer 
                                SET AlterDate = {0}, IsActive = {1}
                                WHERE Id = {2}", "GETDATE()", i.IsActive ? "1" : "0", i.Id);

                            db.Database.ExecuteSqlCommand(queryUpdateParLevel3Value_OuterList);
                        }

                    }

                if (parLevel3Level2peso != null)
                    foreach (var i in parLevel3Level2peso?.Where(r => r.IsActive))
                        AddVinculoL1L2(paramsDto.level1Selected, paramsDto.level2Selected, saveParamLevel3.Id, 0, i.ParCompany_Id);

            }
            catch (DbUpdateException e)
            {
                VerifyUniqueName(saveParamLevel3, e);
            }

            #endregion

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

            #region Query / Parametros

            ParamsDTO retorno = new ParamsDTO();
            var parlevel3 = _baseRepoParLevel3.GetById(idParLevel3);/*ParLevel3*/
            var level3 = Mapper.Map<ParLevel3DTO>(parlevel3);//Level3
            var relapse = parlevel3.ParRelapse.Where(r => r.IsActive == true).OrderByDescending(r => r.IsActive);
            var group = db.ParLevel3Group.Where(r => r.ParLevel2_Id == idParLevel2 && r.IsActive == true).ToList();
            var level3Level2 = parlevel3.ParLevel3Level2.Where(r => r.ParLevel2_Id == idParLevel2 && r.ParLevel3_Id == idParLevel3 && r.IsActive == true).OrderByDescending(r => r.IsActive);
            var level3Value = parlevel3.ParLevel3Value.Where(r => r.IsActive == true).OrderByDescending(r => r.IsActive);
            var parlevel3Reencravacao = db.Database.SqlQuery<ParLevel3Value_OuterListDTO>(string.Format(@"SELECT * FROM ParLevel3Value_Outer WHERE Parlevel3_Id = {0} AND IsActive = 1", parlevel3.Id)).ToList();
            #endregion

            #region Mapper

            level3.listParRelapseDto = Mapper.Map<List<ParRelapseDTO>>(relapse);/*Reincidencia*/
            level3.listGroupsLevel2 = Mapper.Map<List<ParLevel3GroupDTO>>(group);//DDL
            level3.listLevel3Level2 = Mapper.Map<List<ParLevel3Level2DTO>>(level3Level2);
            level3.listLevel3Value = Mapper.Map<List<ParLevel3ValueDTO>>(level3Value);
            retorno.parLevel3Value = new ParLevel3ValueDTO(); // Mini Gambi....
            level3.ParLevel3Value_OuterList = parlevel3Reencravacao;
            level3.ParLevel3Value_OuterListGrouped = parlevel3Reencravacao.GroupBy(r=>r.ParCompany_Id);
            #endregion

            #region Rn's

            if (level3.listLevel3Level2.Count() > 0)/*Id do grupo selecionado no vinculo Level 3 com level 2*/
                level3.hasVinculo = true;

            foreach (var Level3Value in level3.listLevel3Value)/*ParLevel 3 Value*/
                Level3Value.PreparaGet();

            #endregion

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

                var DdlParCounter_Level1 = Mapper.Map<List<ParCounterDTO>>(_baseParCounter.GetAllAsNoTracking().Where(p => p.Level == 1).Where(p => p.Hashkey != null));
                var DdlParCounter_Level2 = Mapper.Map<List<ParCounterDTO>>(_baseParCounter.GetAllAsNoTracking().Where(p => p.Level == 2).Where(p => p.Hashkey != null));

                var DdlParLevel3InputType = Mapper.Map<List<ParLevel3InputTypeDTO>>(_baseParLevel3InputType.GetAllAsNoTracking());
                var DdlParMeasurementUnit = Mapper.Map<List<ParMeasurementUnitDTO>>(_baseParMeasurementUnit.GetAllAsNoTracking());

                var DdlParLevel3BoolFalse = Mapper.Map<List<ParLevel3BoolFalseDTO>>(_baseParLevel3BoolFalse.GetAllAsNoTracking());
                var DdlParLevel3BoolTrue = Mapper.Map<List<ParLevel3BoolTrueDTO>>(_baseParLevel3BoolTrue.GetAllAsNoTracking());

                var DdlparCrit = Mapper.Map<List<ParCriticalLevelDTO>>(_baseRepoParCriticalLevel.GetAllAsNoTracking());

                var DdlparCompany = Mapper.Map<List<ParCompanyDTO>>(_baseRepoParCompany.GetAllAsNoTracking());
                var DdlScoretype = Mapper.Map<List<ParScoreTypeDTO>>(_baseRepoParScore.GetAllAsNoTracking());

                var retorno = new ParamsDdl();



                retorno.SetDdlsNivel123(DdlparLevel1,
                                DdlparLevel2,
                                DdlparLevel3);

                retorno.SetDdls(DdlParConsolidation, DdlFrequency, DdlparCluster, DdlparLevelDefinition, DdlParFieldType, DdlParDepartment, DdlParCounter_Level1,
                                DdlParLocal_Level1, DdlParCounter_Level2, DdlParLocal_Level2, DdlParNotConformityRule, DdlParLevel3InputType, DdlParMeasurementUnit,
                                DdlParLevel3BoolFalse, DdlParLevel3BoolTrue, DdlparCrit, DdlparCompany, DdlScoretype);
                return retorno;
            }
        }

        #endregion

        #region Vinculo ParLevel2Level1

        public List<ParLevel3Level2Level1DTO> AddVinculoL1L2(int idLevel1, int idLevel2, int idLevel3, int? userId = 0, int? companyId = null)
        {

            var retorno = new List<ParLevel3Level2Level1DTO>();
            _paramsRepo.SaveVinculoL3L2L1(idLevel1, idLevel2, idLevel3, userId, companyId);
            return retorno;

        }

        public bool VerificaShowBtnRemVinculoL1L2(int idLevel1, int idLevel2)
        {
            var response = false;
            using (var db = new SgqDbDevEntities())
            {
                var sql1 = "SELECT * FROM ParLevel3Level2Level1 WHERE ParLevel1_Id = " + idLevel1 + " AND ParLevel3Level2_Id IN (select id from ParLevel3Level2 where parlevel2_id = " + idLevel2 + " AND Active = 1)";
                var result1 = db.Database.SqlQuery<ParLevel3Level2Level1>(sql1).ToList();
                var result2 = db.ParLevel2Level1.Where(r => r.ParLevel1_Id == idLevel1 && r.ParLevel2_Id == idLevel2 && r.IsActive == true).ToList();
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
                var result2 = db.ParLevel2Level1.Where(r => r.ParLevel1_Id == idLevel1 && r.ParLevel2_Id == idLevel2).ToList();
                if (result1?.Count() > 0 || result2?.Count() > 0)
                {
                    if (result1?.Count() > 0)
                        sql1 = "DELETE ParLevel3Level2Level1 WHERE ParLevel1_Id = " + idLevel1 + " AND ParLevel3Level2_Id IN (select id from ParLevel3Level2 where parlevel2_id = " + idLevel2 + ")";
                    if (result2?.Count() > 0)
                        sql2 = "DELETE ParLevel2Level1 WHERE ParLevel1_Id = " + idLevel1 + " AND ParLevel2_Id = " + idLevel2;
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

        public bool SetRequiredCamposCabecalho(int id, int required)
        {
            var headerField = _baseRepoParHeaderField.GetById(id);
            if (required == 1)
                headerField.IsRequired = true;
            else
                headerField.IsRequired = false;

            _baseRepoParHeaderField.AddOrUpdate(headerField);

            return headerField.IsRequired.Value;

        }

        public ParMultipleValues SetDefaultMultiplaEscolha(int idHeader, int idMultiple)
        {

            var headerFieldList = _baseRepoParMultipleValues.GetAll().Where(r => r.ParHeaderField_Id == idHeader);

            foreach (ParMultipleValues m in headerFieldList)
            {
                m.IsDefaultOption = false;
                _baseRepoParMultipleValues.AddOrUpdate(m);
            }

            var multiple = new ParMultipleValues();

            if (idMultiple > 0)
            {
                multiple = _baseRepoParMultipleValues.GetById(idMultiple);
                if (multiple.IsDefaultOption == null || multiple.IsDefaultOption == false)
                    multiple.IsDefaultOption = true;
                else
                    multiple.IsDefaultOption = false;

                _baseRepoParMultipleValues.AddOrUpdate(multiple);
            }

            return multiple;

        }

        public ParLevel2XHeaderField AddRemoveParHeaderLevel2(ParLevel2XHeaderField parLevel2XHeaderField)
        {
            parLevel2XHeaderField.AddDate = DateTime.Now;
            parLevel2XHeaderField.IsActive = true;

            parLevel2XHeaderField = _paramsRepo.SaveParHeaderLevel2(parLevel2XHeaderField);

            return parLevel2XHeaderField;
        }

    }

}
