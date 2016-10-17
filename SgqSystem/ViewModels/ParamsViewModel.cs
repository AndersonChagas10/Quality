using DTO.DTO.Params;
using System.Collections.Generic;
using System.Web.Mvc;
using Dominio;
using Dominio.Interfaces.Services;
using System;

namespace SgqSystem.ViewModels
{
    public class ParamsViewModel
    {
        #region Constructors

        private IBaseDomain<ParLevel1, ParLevel1DTO> _baseParLevel1;
        private IBaseDomain<ParFrequency, ParFrequencyDTO> _baseParFrequency;
        private IBaseDomain<ParConsolidationType, ParConsolidationTypeDTO> _baseParConsolidationType;
        private IBaseDomain<ParCluster, ParClusterDTO> _baseParCluster;

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
        public ParamsViewModel(
            IBaseDomain<ParLevel1, ParLevel1DTO> _baseParLevel1,
            IBaseDomain<ParFrequency, ParFrequencyDTO> _baseParFrequency,
            IBaseDomain<ParConsolidationType, ParConsolidationTypeDTO> _baseParConsolidationType,
            IBaseDomain<ParCluster, ParClusterDTO> _baseParCluster
            )
        {
            this._baseParLevel1 = _baseParLevel1;
            this._baseParFrequency = _baseParFrequency;
            this._baseParConsolidationType = _baseParConsolidationType;
            this._baseParCluster = _baseParCluster;
            SetAllDdl();
        }

        /// <summary>
        /// Construtor para View Model Level1
        /// </summary>
        /// <param name="_baseParLevel1"></param>
        /// <param name="_baseParFrequency"></param>
        /// <param name="_baseParConsolidationType"></param>
        /// <param name="parLevel1DTO"></param>
        public ParamsViewModel(IBaseDomain<ParLevel1, ParLevel1DTO> _baseParLevel1,
            IBaseDomain<ParFrequency, ParFrequencyDTO> _baseParFrequency,
            IBaseDomain<ParConsolidationType, ParConsolidationTypeDTO> _baseParConsolidationType,
            ParLevel1DTO parLevel1DTO)
        {
            this._baseParLevel1 = _baseParLevel1;
            this._baseParFrequency = _baseParFrequency;
            this._baseParConsolidationType = _baseParConsolidationType;
            paramsDto = new ParamsDTO();
            paramsDto.parLevel1Dto = parLevel1DTO;
            SetAllDdl();
        }

        #endregion

        public IEnumerable<SelectListItem> DdlParConsolidation { get; set; }
        public IEnumerable<SelectListItem> DdlFrequency { get; set; }
        public IEnumerable<SelectListItem> DdlparLevel1 { get; set; }
        public IEnumerable<SelectListItem> DdlparCluster { get; set; }
        public ParamsDTO paramsDto { get; set; }
        
        #region Auxiliares

        private void SetAllDdl()
        {
            CreateSelectListParamsViewModel(_baseParConsolidationType.GetAll(), DdlParConsolidation);
            CreateSelectListParamsViewModel(_baseParFrequency.GetAll(), DdlFrequency);
            CreateSelectListParamsViewModelListLevel(_baseParLevel1.GetAll(), DdlparLevel1);
            CreateSelectListParamsViewModel(_baseParCluster.GetAll(), DdlparCluster);
        }

        private void CreateSelectListParamsViewModel<T>(IEnumerable<T> enumerable, IEnumerable<SelectListItem> dll)
        {
            List<SelectListItem> retorno = new List<SelectListItem>();
            var counter = 0;
            foreach (var i in enumerable)
            {
                var text = i.GetType().GetProperty("Name") ?? i.GetType().GetProperty("Description");
                var prop = i.GetType().GetProperty("Id");
                retorno.Insert(counter, new SelectListItem() { Text = text.GetValue(i).ToString(), Value = prop.GetValue(i).ToString() });
                counter++;
            }

            dll = retorno;
        }

        private void CreateSelectListParamsViewModelListLevel<T>(IEnumerable<T> enumerable, IEnumerable<SelectListItem> dll)
        {
            List<SelectListItem> retorno = new List<SelectListItem>();
            var counter = 0;
            foreach (var i in enumerable)
            {
                var text = i.GetType().GetProperty("Name") ?? i.GetType().GetProperty("Description");
                var prop = i.GetType().GetProperty("Id");
                retorno.Insert(counter, new SelectListItem() { Text = prop.GetValue(i).ToString() + " - " + text.GetValue(i).ToString(), Value = prop.GetValue(i).ToString() });
                counter++;
            }

            dll =  retorno;
        }

        #endregion
    }
}