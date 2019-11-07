using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.AppViewModel
{
    public class ComponenteGenericoViewModel
    {
        public ComponenteGenerico ComponenteGenerico { get; set; }
        public List<ComponenteGenericoColuna> ComponentesGenericosColuna { get; set; }
    }
}
