using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class ComponenteGenericoColuna : BaseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ComponenteGenerico_Id { get; set; }
        public int ComponenteGenericoTipoColuna_Id { get; set; }
        public bool IsRequired { get; set; }
        public string ValorPadrao { get; set; }
        public bool IsActive { get; set; }
        public string TabelaVinculo { get; set; }

        [ForeignKey("ComponenteGenerico_Id")]
        public ComponenteGenerico ComponenteGenerico { get; set; }

        [ForeignKey("ComponenteGenericoTipoColuna_Id")]
        public ComponenteGenericoTipoColuna ComponenteGenericoTipoColuna { get; set; }
    }
}
