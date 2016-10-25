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
        private IBaseRepository<ParCompany> _baseParCompany;
        /*Repo Especifico, manejam os itens*/
        private IParamsRepository _paramsRepo;

        public ParamsDomain(IBaseRepository<ParLevel1> baseRepoParLevel1,
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
            IBaseRepository<ParCounterXLocal> baseParCounterLocal,
            IBaseRepository<ParRelapse> baseParRelapse,
            IBaseRepository<ParNotConformityRule> baseParNotConformityRule,
            IBaseRepository<ParCompany> baseParCompany)
        {
            _paramsRepo = paramsRepo;
            _baseRepoParLevel1 = baseRepoParLevel1;
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
            _baseParCounterXLocal = baseParCounterLocal;
            _baseParRelapse = baseParRelapse;
            _baseParNotConformityRule = baseParNotConformityRule;
            _baseParCompany = baseParCompany;
        }

        #endregion

        #region Metods

        /// <summary>
        /// Salva parametrização Level1
        /// 
        /// Rn1 Valida objeto
        /// Rn2 Salva objeto
        /// Rn3 Retorna objeto atualizado.
        /// 
        /// </summary>
        /// <param name="paramsDto"></param>
        /// <returns></returns>
        public ParamsDTO AddUpdateLevel1(ParamsDTO paramsDto)
        {
            //paramsDto.parLevel1Dto.IsValid();
            ParLevel1 saveParamLevel1 = Mapper.Map<ParLevel1>(paramsDto.parLevel1Dto);
            List<ParHeaderField> listaParHEadField = Mapper.Map<List<ParHeaderField>>(paramsDto.listParHeaderFieldDto);
            List<ParLevel1XCluster> ListaParLevel1XCluster = Mapper.Map<List<ParLevel1XCluster>>(paramsDto.parLevel1XClusterDto);

            _paramsRepo.SaveParLevel1(saveParamLevel1, listaParHEadField, ListaParLevel1XCluster);
            paramsDto.parLevel1Dto.Id = saveParamLevel1.Id;
            return paramsDto;
        }

        public ParamsDTO AddUpdateLevel2(ParamsDTO paramsDto)
        {
            //paramsDto.parLevel1Dto.IsValid();
            ParLevel2 saveParamLevel2 = Mapper.Map<ParLevel2>(paramsDto.parLevel2Dto);
            List<ParLevel3Group> listaParLevel3Group = Mapper.Map<List<ParLevel3Group>>(paramsDto.listParLevel3GroupDto);


            _paramsRepo.SaveParLevel2(saveParamLevel2, listaParLevel3Group);

            paramsDto.parLevel2Dto.Id = saveParamLevel2.Id;

            ///*Salva Clueter X*/
            //SalvarParLevel1XCluster(paramsDto, saveParamLevel1);

            return paramsDto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdParLevel1"></param>
        /// <returns></returns>
        public ParamsDTO GetLevel1(int IdParLevel1)
        {
            var retorno = new ParamsDTO();
            
            //var queryResult = _baseRepoParLevel1.GetById(IdParLevel1);
            //retorno.parLevel1Dto = Mapper.Map<ParLevel1DTO>(queryResult);
            //retorno.parLevel1XClusterDto = Mapper.Map<ParLevel1XClusterDTO>(queryResult.ParLevel1XCluster.FirstOrDefault());

            return retorno;
        }

        public ParamsDdl CarregaDropDownsParams()
        {
            var DdlParConsolidation = Mapper.Map<List<ParConsolidationTypeDTO>>(_baseParConsolidationType.GetAll());
            var DdlFrequency = Mapper.Map<List<ParFrequencyDTO>>(_baseParFrequency.GetAll());
            var DdlparLevel1 = Mapper.Map<List<ParLevel1DTO>>(_baseRepoParLevel1.GetAll());
            var DdlparCluster = Mapper.Map<List<ParClusterDTO>>(_baseParCluster.GetAll());
            var DdlparLevelDefinition = Mapper.Map<List<ParLevelDefinitonDTO>>(_baseParLevelDefiniton.GetAll());
            var DdlParFieldType = Mapper.Map<List<ParFieldTypeDTO>>(_baseParFieldType.GetAll());
            var DdlParDepartment = Mapper.Map<List<ParDepartmentDTO>>(_baseParDepartment.GetAll());


            var DdlParLocal_Level1 = Mapper.Map<List<ParLocalDTO>>(_baseParLocal.GetAll().Where(p => p.Level == 1));
            var DdlParLocal_Level2 = Mapper.Map<List<ParLocalDTO>>(_baseParLocal.GetAll().Where(p => p.Level == 2));

            var DdbParCounter_Level1 = Mapper.Map<List<ParCounterDTO>>(_baseParLocal.GetAll().Where(p => p.Level == 1));
            var DdbParCounter_Level2 = Mapper.Map<List<ParCounterDTO>>(_baseParLocal.GetAll().Where(p => p.Level == 2));


            var retorno = new ParamsDdl();
            retorno.SetDdls(DdlParConsolidation, DdlFrequency, DdlparLevel1, DdlparCluster, DdlparLevelDefinition, DdlParFieldType, DdlParDepartment);
            return retorno;
        }

        public ParamsDTO AddUpdateParLocal(ParamsDTO paramsDto)
        {
            //paramsDto.parLevel1Dto.IsValid();
            ParLocal saveParLocal = Mapper.Map<ParLocal>(paramsDto.parLocalDto);

            _paramsRepo.SaveParLocal(saveParLocal);

            paramsDto.parLocalDto.Id = saveParLocal.Id;

            ///*Salva Clueter X*/
            //SalvarParLevel1XCluster(paramsDto, saveParamLevel1);

            return paramsDto;
        }

        public ParamsDTO AddUpdateParCounter(ParamsDTO paramsDto)
        {
            //paramsDto.parLevel1Dto.IsValid();
            ParCounter saveParCounter = Mapper.Map<ParCounter>(paramsDto.parCounterDto);

            _paramsRepo.SaveParCounter(saveParCounter);

            paramsDto.parCounterDto.Id = saveParCounter.Id;

            ///*Salva Clueter X*/
            //SalvarParLevel1XCluster(paramsDto, saveParamLevel1);

            return paramsDto;
        }

        public ParamsDTO AddUpdateParCounterLocal(ParamsDTO paramsDto)
        {
            ParCounterXLocal saveParCounterXLocal = Mapper.Map<ParCounterXLocal>(paramsDto.parCounterXLocalDto);
            _paramsRepo.SaveParCounterXLocal(saveParCounterXLocal);
            paramsDto.parCounterXLocalDto.Id = saveParCounterXLocal.Id;
            return paramsDto;
        }

        public ParamsDTO AddUpdateParRelapse(ParamsDTO paramsDto)
        {
            ParRelapse saveParRelapse = Mapper.Map<ParRelapse>(paramsDto.parRelapseDto);
            _paramsRepo.SaveParRelapse(saveParRelapse);
            paramsDto.parRelapseDto.Id = saveParRelapse.Id;
            return paramsDto;
        }

        public ParamsDTO AddUpdateParNotConformityRule(ParamsDTO paramsDto)
        {
            ParNotConformityRule saveParNotConformityRule = Mapper.Map<ParNotConformityRule>(paramsDto.parNotConformityRuleDto);
            _paramsRepo.SaveParNotConformityRule(saveParNotConformityRule);
            paramsDto.parNotConformityRuleDto.Id = saveParNotConformityRule.Id;
            return paramsDto;
        }

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
    }
}
