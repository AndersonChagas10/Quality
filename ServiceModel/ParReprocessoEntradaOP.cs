using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceModel
{
    public class ParReprocessoEntradaOP
    {
        public decimal nCdOrdemProducao { get; set; }
        public decimal nCdProduto { get; set; }
        public DateTime dProducao { get; set; }
        public DateTime dEmbalagem { get; set; }
        public DateTime dValidade { get; set; }
        public decimal nCdLocalEstoque { get; set; }
        public String cNmLocalEstoque { get; set; }
        public String cCdOrgaoRegulador { get; set; }
        public String cCdRastreabilidade { get; set; }
        public int iVolume { get; set; }
        public decimal nPesoLiquido { get; set; }
        public Produto produto { get; set; }
        public decimal nCdEmpresa { get; set; }
    }
}
