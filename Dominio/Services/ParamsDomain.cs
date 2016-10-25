﻿using Dominio.Interfaces.Services;
using Dominio.Interfaces.Repositories;
using DTO.DTO.Params;
using AutoMapper;
using System.Collections.Generic;

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
            IBaseRepository<ParDepartment> baseParDepartment)
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

            /*Salva Clueter X*/
            SalvarParLevel1XCluster(paramsDto, saveParamLevel1);

            return paramsDto;
        }

        public ParamsDTO AddUpdateLevel2(ParamsDTO paramsDto)
        {
            //paramsDto.parLevel1Dto.IsValid();
            ParLevel2 saveParamLevel2 = Mapper.Map<ParLevel2>(paramsDto.parLevel2Dto);
            List<ParDepartment> listaParDepartment = Mapper.Map<List<ParDepartment>>(paramsDto.listParDepartmentdDto);
            List<ParFrequency> ParFrequency = Mapper.Map<List<ParFrequency>>(paramsDto.listParFrequencydDto);


            _paramsRepo.SaveParLevel2(saveParamLevel2, listaParDepartment, ParFrequency);

            paramsDto.parLevel2Dto.Id = saveParamLevel2.Id;

            ///*Salva Clueter X*/
            //SalvarParLevel1XCluster(paramsDto, saveParamLevel1);

            return paramsDto;
        }


        private void SalvarParLevel1XCluster(ParamsDTO paramsDto, ParLevel1 saveParamLevel1)
        {
            foreach (var parLevel1XCluster in paramsDto.parLevel1XClusterDto)
            {
                parLevel1XCluster.ParLevel1_Id = saveParamLevel1.Id;
                _baseRepoParLevel1XCluster.Add(Mapper.Map<ParLevel1XCluster>(parLevel1XCluster));
            }
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

            var retorno = new ParamsDdl();
            retorno.SetDdls(DdlParConsolidation, DdlFrequency, DdlparLevel1, DdlparCluster, DdlparLevelDefinition, DdlParFieldType, DdlParDepartment);
            return retorno;
        }


        #endregion
    }
}
