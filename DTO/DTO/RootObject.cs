using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTO
{
    public class RootObject
    {
        public string biasedunbiased { get; set; }
        public string @class { get; set; }
        public string completed { get; set; }
        public string completereaudit { get; set; }
        public string correctiveactioncomplete { get; set; }
        public string date { get; set; }
        public string datetime { get; set; }
        public string lastevaluate { get; set; }
        public string lastsample { get; set; }
        public string level01id { get; set; }
        public string more3defects { get; set; }
        public string period { get; set; }
        public string reaudit { get; set; }
        public string reauditnumber { get; set; }
        public string shift { get; set; }
        public string sidewitherros { get; set; }
        public string totalevaluate { get; set; }
        public string totalreaudits { get; set; }
        public string unidadeid { get; set; }
        public List<NextRoot> nextRoot { get; set; }

        public ConsolidationLevel01DTO consolidationLevel01DTO { get; set; }

        public void ValidateAndCreateDtoConsolidationLevel01DTO()
        {
            consolidationLevel01DTO = new ConsolidationLevel01DTO(this);
                


        }
    }
}
