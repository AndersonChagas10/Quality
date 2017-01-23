using DTO.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SgqSystem.ViewModels
{
    /// <summary>
    /// A Classe View Model possui apenas dados pertinentes a TELA.
    /// </summary>
    public class ContextExampleViewModel : ContextExampleDTO
    {
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

        public int IntegerProp { get; set; }

        public decimal DecimalProp { get; set; }
        public Nullable<decimal> DecimalNullProp { get; set; }

        public float FloatProp { get; set; }
        public DateTime DateTimeProp { get; set; }
    }
}