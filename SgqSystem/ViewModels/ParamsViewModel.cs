using DTO.DTO;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SgqSystem.ViewModels
{
    /// <summary>
    /// A Classe View Model possui apenas dados pertinentes a TELA.
    /// </summary>
    public class ParamsViewModel : ParamsDTO
    {


        public int _ShiftSelectListExample { get; set; }
        public IEnumerable<SelectListItem> ShiftSelectListExample
        {
            get
            {
                return new List<SelectListItem>(){
                                                    new SelectListItem() {Text="Shift A", Value="1"},
                                                    new SelectListItem() {Text="Shift B", Value="2"},
                                                }; ;
            }
            set
            {
                ShiftSelectListExample = value;
            }
        }
      
    }
}