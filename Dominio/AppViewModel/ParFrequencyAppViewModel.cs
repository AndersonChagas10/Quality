using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.AppViewModel
{
    public class ParFrequencyAppViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<ParLevel1AppViewModel> ParLevel1 { get; set; } = new List<ParLevel1AppViewModel>();
    }
}
