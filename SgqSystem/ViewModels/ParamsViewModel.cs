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
        /// Construtor para carregar dados do banco
        /// </summary>
        /// <param name="_baseParLevel1"></param>
        /// <param name="_baseParFrequency"></param>
        /// <param name="_baseParConsolidationType"></param>
        /// <param name="_baseParCluster"></param>
        public ParamsViewModel(ParamsDdl paramsDdl)
        {
            this.paramsDdl = paramsDdl;
            paramsDto = new ParamsDTO();
            paramsDto.parLevel1Dto = new ParLevel1DTO();
            paramsDto.parLevel2Dto = new ParLevel2DTO();
            paramsDto.parLevel1XClusterDto = new List<ParLevel1XClusterDTO>();
        }

        /// <summary>
        /// Construtor para View Model Level1
        /// </summary>
        /// <param name="_baseParLevel1"></param>
        /// <param name="_baseParFrequency"></param>
        /// <param name="_baseParConsolidationType"></param>
        /// <param name="parLevel1DTO"></param>
        public ParamsViewModel(ParamsDdl paramsDdl, ParamsDTO paramsDb)
        {
        }

        #endregion

        public ParamsDTO paramsDto { get; set; }
        public ParamsDdl paramsDdl { get; set; }
        

    }
}