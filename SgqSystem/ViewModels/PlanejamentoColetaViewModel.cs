using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SgqSystem.ViewModels
{
    public class PlanejamentoColetaViewModel
    {
        public int ParCompany_Id { get; set; }
        public int ParFrequency_Id { get; set; }
        public DateTime AppDate { get; set; }
        public List<PlanejamentoViewModel> Planejamento { get; set; }

    }
}