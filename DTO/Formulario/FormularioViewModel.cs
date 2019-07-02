using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Formulario
{
    public class FormularioViewModel
    {
        public List<ParStructure> ParStructures { get; set; }
        public List<ParCompany> ParCompanies { get; set; }
        public List<ParLevel1> ParLevel1s { get; set; }
        public List<ParLevel2> ParLevel2s { get; set; }
        public List<ParLevel3> ParLevel3s { get; set; }
    }
}
