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
        private IBaseDomain<ParLevel1XCluster, ParLevel1XClusterDTO> _baseParLevel1XCluster;

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
            IBaseDomain<ParCluster, ParClusterDTO> _baseParCluster,
            IBaseDomain<ParLevel1XCluster, ParLevel1XClusterDTO> _baseParLevel1XCluster
            )
        {
            SetInterfaces(_baseParLevel1, _baseParFrequency, _baseParConsolidationType, _baseParCluster, _baseParLevel1XCluster);
            SetAllDdl();
            paramsDto = new ParamsDTO();
            paramsDto.parLevel1Dto = new ParLevel1DTO();
            paramsDto.parLevel1XClusterDto = new ParLevel1XClusterDTO();
        }

        /// <summary>
        /// Construtor para View Model Level1
        /// </summary>
        /// <param name="_baseParLevel1"></param>
        /// <param name="_baseParFrequency"></param>
        /// <param name="_baseParConsolidationType"></param>
        /// <param name="parLevel1DTO"></param>
        public ParamsViewModel(
             IBaseDomain<ParLevel1, ParLevel1DTO> _baseParLevel1,
             IBaseDomain<ParFrequency, ParFrequencyDTO> _baseParFrequency,
             IBaseDomain<ParConsolidationType, ParConsolidationTypeDTO> _baseParConsolidationType,
             IBaseDomain<ParCluster, ParClusterDTO> _baseParCluster,
             IBaseDomain<ParLevel1XCluster, ParLevel1XClusterDTO> _baseParLevel1XCluster,
             ParamsDTO parLevel1DTO)
        {
            SetInterfaces(_baseParLevel1, _baseParFrequency, _baseParConsolidationType, _baseParCluster, _baseParLevel1XCluster);
            paramsDto = parLevel1DTO;
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
            DdlParConsolidation = CreateSelectListParamsViewModel(_baseParConsolidationType.GetAll());
            DdlFrequency = CreateSelectListParamsViewModel(_baseParFrequency.GetAll());
            DdlparLevel1 = CreateSelectListParamsViewModelListLevel(_baseParLevel1.GetAll());
            DdlparCluster = CreateSelectListParamsViewModel(_baseParCluster.GetAll());
        }

        private void SetInterfaces(IBaseDomain<ParLevel1, ParLevel1DTO> _baseParLevel1, IBaseDomain<ParFrequency, ParFrequencyDTO> _baseParFrequency, IBaseDomain<ParConsolidationType, ParConsolidationTypeDTO> _baseParConsolidationType, IBaseDomain<ParCluster, ParClusterDTO> _baseParCluster, IBaseDomain<ParLevel1XCluster, ParLevel1XClusterDTO> _baseParLevel1XCluster)
        {
            this._baseParLevel1XCluster = _baseParLevel1XCluster;
            this._baseParLevel1 = _baseParLevel1;
            this._baseParFrequency = _baseParFrequency;
            this._baseParConsolidationType = _baseParConsolidationType;
            this._baseParCluster = _baseParCluster;
        }

        private List<SelectListItem> CreateSelectListParamsViewModel<T>(IEnumerable<T> enumerable)
        {
            List<SelectListItem> retorno = new List<SelectListItem>();
            retorno.Insert(0, new SelectListItem() { Text = "Selecione...", Value = "-1" });
            var counter = 1;
            foreach (var i in enumerable)
            {
                var text = i.GetType().GetProperty("Name") ?? i.GetType().GetProperty("Description");
                var prop = i.GetType().GetProperty("Id");
                retorno.Insert(counter, new SelectListItem() { Text = text.GetValue(i).ToString(), Value = prop.GetValue(i).ToString() });
                counter++;
            }

           return retorno;
        }

        private List<SelectListItem> CreateSelectListParamsViewModelListLevel<T>(IEnumerable<T> enumerable)
        {
            List<SelectListItem> retorno = new List<SelectListItem>();
            retorno.Insert(0, new SelectListItem() { Text = "Selecione...", Value = "-1" });
            var counter = 1;
            foreach (var i in enumerable)
            {
                var text = i.GetType().GetProperty("Name") ?? i.GetType().GetProperty("Description");
                var prop = i.GetType().GetProperty("Id");
                retorno.Insert(counter, new SelectListItem() { Text = prop.GetValue(i).ToString() + " - " + text.GetValue(i).ToString(), Value = prop.GetValue(i).ToString() });
                counter++;
            }

            return retorno;
        }

        #endregion
    }
}