using DTO.DTO.Params;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SgqSystem.ViewModels
{
    public class ParamsViewModel
    {
        public IEnumerable<SelectListItem> DdlParConsolidation
        {
            get
            {
                return new List<SelectListItem>(){
                                                    new SelectListItem() {Text="por Tarefa (level 3)", Value="1"},
                                                    new SelectListItem() {Text="por monitormento (level 2)", Value="2"},
                                                    new SelectListItem() {Text="outro", Value="3"},
                                                }; ;
            }
            set
            {
                DdlParConsolidation = value;
            }
        }

        public IEnumerable<SelectListItem> DdlFrequency
        {
            get
            {
                return new List<SelectListItem>(){
                                                    new SelectListItem() {Text="Por Período", Value="1"},
                                                    new SelectListItem() {Text="Por Turno", Value="2"},
                                                    new SelectListItem() {Text="Diario", Value="3"},
                                                    new SelectListItem() {Text="Semanal", Value="4"},
                                                    new SelectListItem() {Text="Quinzenal", Value="5"},
                                                    new SelectListItem() {Text="Mensal", Value="6"},
                                                }; ;
            }
            set
            {
                DdlParConsolidation = value;
            }
        }

        public ParamsDTO paramsDto { get; set; }
    }
}