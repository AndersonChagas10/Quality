using DTO;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SgqSystem.ViewModels
{
    public class FormularioParaRelatorioViewModel : DataCarrierFormulario
    {

        #region Retorno

        public ResultSetRelatorioColeta result { get; set; }

        #region SelectListItem

        public IEnumerable<SelectListItem> level01DTO { get; set; }
        public IEnumerable<SelectListItem> level02DTO { get; set; }
        public IEnumerable<SelectListItem> level03DTO { get; set; }
        
        #endregion

        #endregion



    }
}