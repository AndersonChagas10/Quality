using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceModel
{
    /// <summary>
    /// Objeto de auxilio para retorno.
    /// </summary>
    public class RetornoParaTablet
    {
        public bool ready { get; set; }
        public int pool { get; set; }
        public string ParteDaTela { get; set; }
    }
}
