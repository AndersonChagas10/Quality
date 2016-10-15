﻿using Dominio.Interfaces.Services;
using Dominio.Interfaces.Repositories;
using System;
using DTO.DTO.Params;
using AutoMapper;

namespace Dominio.Services
{
    /// <summary>
    /// Rns para parametrização.
    /// </summary>
    public class ParamsDomain : IParamsDomain
    {

        #region Constructor

        private IBaseRepository<ParLevel1> _baseRepoParamLevel1;
        private IBaseRepository<Example> _baseRepoParamLevel2;
        private IBaseRepository<Example> _baseRepoParamLevel3;

        public ParamsDomain(
            IBaseRepository<ParLevel1> baseRepoParamLevel1,
            IBaseRepository<Example> baseRepoParamLevel2,
            IBaseRepository<Example> baseRepoParamLevel3
            )
        {
            _baseRepoParamLevel1 = baseRepoParamLevel1;
            _baseRepoParamLevel2 = baseRepoParamLevel2;
            _baseRepoParamLevel3 = baseRepoParamLevel3;
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
            _baseRepoParamLevel1.AddOrUpdate(saveParamLevel1);
            //paramsDto.parLevel1Dto = Mapper.Map<ParLevel1DTO>(saveParamLevel1);
            paramsDto.parLevel1Dto.Id = saveParamLevel1.Id;
            return paramsDto;
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Salva parametrização Level2
        /// 
        /// Rn1 Valida objeto
        /// Rn2 Salva objeto
        /// Rn3 Retorna objeto atualizado.
        /// 
        /// </summary>
        /// <param name="paramsDto"></param>
        /// <returns></returns>
        public ParamsDTO AddUpdateLevel2(ParamsDTO paramsDto)
        {
            //paramsDto.IsValid();
            //Params saveParamLevel2 = Mapper.Map<Params>(paramsDto);
            //_baseRepoParamLevel2.AddOrUpdate(saveParamLevel1);
            //paramsDto = Mapper.Map<ParamsDTO>(saveParamLevel1);

            //return paramsDto;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Salva parametrização Level3
        /// 
        /// Rn1 Valida objeto
        /// Rn2 Salva objeto
        /// Rn3 Retorna objeto atualizado.
        /// 
        /// </summary>
        /// <param name="paramsDto"></param>
        /// <returns></returns>
        public ParamsDTO AddUpdateLevel3(ParamsDTO paramsDto)
        {
            //paramsDto.IsValid();
            //Params saveParamLevel3 = Mapper.Map<Params>(paramsDto);
            //_baseRepoParamLevel3.AddOrUpdate(saveParamLevel3);
            //paramsDto = Mapper.Map<ParamsDTO>(saveParamLevel3);

            //return paramsDto;
            throw new NotImplementedException();
        }




        #endregion
    }
}
