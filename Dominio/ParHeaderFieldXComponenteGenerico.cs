using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class ParHeaderFieldXComponenteGenerico : BaseModel
    {
        public int Id { get; set; }
        public int ComponenteGenerico_Id { get; set; }
        public int ParHeaderField_Id { get; set; }
        public string Value { get; set; }
        public string Text { get; set; }

        [ForeignKey("ComponenteGenerico_Id")]
        public ComponenteGenerico ComponenteGenerico { get; set; }

        [ForeignKey("ParHeaderField_Id")]
        public ParHeaderField ParHeaderField { get; set; }
    }
}
