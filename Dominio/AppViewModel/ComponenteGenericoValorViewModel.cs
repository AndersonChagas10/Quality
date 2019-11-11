using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.AppViewModel
{
    public class ComponenteGenericoValorViewModel
    {
        public List<ComponenteGenericoColuna> Colunas { get; set; }
        public List<ComponenteGenericoValor> Valores { get; set; }
        public ComponenteGenerico ComponenteGenerico{ get; set; }
        public int saveId { get; set; }
    }
}
