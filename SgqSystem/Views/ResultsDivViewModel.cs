using Dominio;
using System.Collections.Generic;

namespace SgqSystem.ViewModels
{
    public class ResultsDivViewModel
    {

        public List<ConsolidationLevel01> consolidationLevel01 { get; set; }

        public List<ConsolidationLevel01> consolidcaoCFF
        {
            get
            {
                return consolidationLevel01;
            }
        }

    }
}