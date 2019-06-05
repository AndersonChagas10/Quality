using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceModel
{
    public class ParReprocessoSaidaOP
    {
        public decimal nCdOrdemProducao { get; set; }
        public int iItem { get; set; }
        public decimal nCdProduto { get; set; }
        public int iQtdePrevista { get; set; }
        public String cQtdeTipo { get; set; }
        public decimal nCdLocalEstoque { get; set; }
        public String cNmLocalEstoque { get; set; }
        public DateTime dProducao { get; set; }
        public DateTime dValidade { get; set; }
        public int iTotalPeca { get; set; }
        public int iTotalVolume { get; set; }
        public decimal nTotalPeso { get; set; }
        public Produto produto { get; set; }
        public decimal nCdEmpresa { get; set; }

    }
}
