using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SgqSystem.ViewModels
{
    public class PlanejamentoViewModel
    {
        public int ParDepartment_Id { get; set; }
        public int? ParCargo_Id { get; set; }
        public int? Indicador_Id { get; set; }
    }
}