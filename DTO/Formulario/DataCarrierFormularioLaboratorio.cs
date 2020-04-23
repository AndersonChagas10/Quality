using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Formulario
{
    public class DataCarrierFormularioLaboratorio
    {
        public int[] nCdProduto { get; set; }

        public DateTime dColetaAmostra { get; set; }

        public string endDate { get; set; }
        public string startDate { get; set; }
    }
}
