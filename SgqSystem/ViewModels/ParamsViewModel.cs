﻿using DTO.DTO;
using DTO.DTO.Params;
using System.Collections.Generic;

namespace SgqSystem.ViewModels
{
    public class ParamsViewModel
    {

        #region Constructors

        /// <summary>
        /// Construtor para o MVC
        /// </summary>
        public ParamsViewModel() { }

       /// <summary>
       /// Construtor Padrão.
       /// </summary>
       /// <param name="paramsDdl"></param>
        public ParamsViewModel(ParamsDdl paramsDdl)
        {
            this.paramsDdl = paramsDdl;
            paramsDto = new ParamsDTO();
            paramsDto.parNotConformityRuleXLevelDto = new ParNotConformityRuleXLevelDTO();
            paramsDto.parLevel1Dto = new ParLevel1DTO();
            paramsDto.parLevel1Dto.parNotConformityRuleXLevelDto = new ParNotConformityRuleXLevelDTO();
            paramsDto.parLevel2Dto = new ParLevel2DTO();
            paramsDto.parLevel3Dto = new ParLevel3DTO() { listGroupsLevel2 = new List<ParLevel3GroupDTO>() };
            paramsDto.parLevel3Value = new ParLevel3ValueDTO();
            //paramsDto.parLevel1XClusterDto = new List<ParLevel1XClusterDTO>();
        }

        /// <summary>
        /// Construtor para View Model Level1
        /// </summary>
        /// <param name="paramsDdl"></param>
        /// <param name="paramsDb"></param>
        public ParamsViewModel(ParamsDdl paramsDdl, ParamsDTO paramsDb)
        {
        }

        #endregion

        public ParamsDTO paramsDto { get; set; }

        public RotinaIntegracaoDTO rotinaIngracaoDTO { get; set; }

        public ParamsDdl paramsDdl { get; set; }

        public int levelControl { get; set; }
    }
}
