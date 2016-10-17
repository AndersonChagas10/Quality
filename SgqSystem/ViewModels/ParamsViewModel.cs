using DTO.DTO.Params;
using System.Collections.Generic;
using System.Web.Mvc;
using Dominio;
using Dominio.Interfaces.Services;

namespace SgqSystem.ViewModels
{
    public class ParamsViewModel
    {
        private IBaseDomain<ParLevel1, ParLevel1DTO> _baseParLevel1;
        private IBaseDomain<ParFrequency, ParFrequencyDTO> _baseParFrequency;
        private IBaseDomain<ParConsolidationType, ParConsolidationTypeDTO> _baseParConsolidationType;

        //Construtor para o MVC
        public ParamsViewModel() { }
        //Construtor para carregar dados do banco
        public ParamsViewModel(IBaseDomain<ParLevel1, ParLevel1DTO> _baseParLevel1,
            IBaseDomain<ParFrequency, ParFrequencyDTO> _baseParFrequency,
            IBaseDomain<ParConsolidationType, ParConsolidationTypeDTO> _baseParConsolidationType)
        {
            this._baseParLevel1 = _baseParLevel1;
            this._baseParFrequency = _baseParFrequency;
            this._baseParConsolidationType = _baseParConsolidationType;
            SetDdlParConsolidation();
            SetDdlFrequency();
            SetDdlparLevel1();
        }

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
            SetDdlParConsolidation();
            SetDdlFrequency();
            SetDdlparLevel1();
        }

        public IEnumerable<SelectListItem> DdlParConsolidation { get; set; }
        private void SetDdlParConsolidation()
        {
            DdlParConsolidation = CreateSelectListParamsViewModel(_baseParConsolidationType.GetAll());
        }

        public IEnumerable<SelectListItem> DdlFrequency { get; set; }
        private void SetDdlFrequency()
        {
            DdlFrequency = CreateSelectListParamsViewModel(_baseParFrequency.GetAll());
        }

        public IEnumerable<SelectListItem> DdlparLevel1 { get; set; }
        private void SetDdlparLevel1()
        {
            DdlparLevel1 = CreateSelectListParamsViewModelListLevel(_baseParLevel1.GetAll());
        }

        public ParamsDTO paramsDto { get; set; }


        #region Auxiliares

        private List<SelectListItem> CreateSelectListParamsViewModel<T>(IEnumerable<T> enumerable)
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

            return retorno;
        }

        private List<SelectListItem> CreateSelectListParamsViewModelListLevel<T>(IEnumerable<T> enumerable)
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

            return retorno;
        }

        #endregion
    }
}