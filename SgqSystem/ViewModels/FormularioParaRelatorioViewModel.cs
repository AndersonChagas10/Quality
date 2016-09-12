using DTO;
using DTO.DTO;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SgqSystem.ViewModels
{
    public class FormularioParaRelatorioViewModel : DataCarrierFormulario
    {

        #region Retorno

        public ResultSetRelatorioColeta result { get; set; }
        public ResultSetGetCorrectiveAction resultSetGetCorrectiveAction { get; set; }

        #region SelectListItem

        public IEnumerable<SelectListItem> level01DTOSelectList
        {
            get
            {
                return new List<SelectListItem>(){
                                                    new SelectListItem() {Text="HTP", Value="1"},
                                                    new SelectListItem() {Text="Carcass Contamination Audit", Value="2"},
                                                    new SelectListItem() {Text="CFF (Cut, Fold and Flaps)", Value="3"},
                                                };
            }
            set { level01DTOSelectList = value; }
        }
        public IEnumerable<SelectListItem> level02DTOSelectList { get; set; }
        public IEnumerable<SelectListItem> level03DTOSelectList { get; set; }
        public IEnumerable<SelectListItem> Units
        {
            get
            {
                return new List<SelectListItem>(){
                                                    new SelectListItem() {Text="Colorado", Value="1"},
                                                };
            }
            set { Units = value; }
        }
        public IEnumerable<SelectListItem> ShiftSelectList
        {
            get
            {
                return new List<SelectListItem>(){
                                                    new SelectListItem() {Text="Shift A", Value="1"},
                                                    new SelectListItem() {Text="Shift B", Value="2"},
                                                };
            }
            set { ShiftSelectList = value; }
        }
        public IEnumerable<SelectListItem> PeriodSelectList
        {
            get
            {
                return new List<SelectListItem>(){
                                                    new SelectListItem() {Text="Period 1", Value="1"},
                                                    new SelectListItem() {Text="Period 2", Value="2"},
                                                    new SelectListItem() {Text="Period 3", Value="3"},
                                                    new SelectListItem() {Text="Period 4", Value="4"},
                                                };
            }
            set { PeriodSelectList = value; }
        }

        public IEnumerable<SelectListItem> UserSelectList { get; set; }

        public void SetUsers(List<UserDTO> users)
        {

            var adicionar = new List<SelectListItem>();

            foreach (var i in users)
                adicionar.Add(new SelectListItem() { Text = i.Name, Value = i.Id.ToString() });

            UserSelectList = adicionar;

        }

        #endregion

        #endregion



    }
}