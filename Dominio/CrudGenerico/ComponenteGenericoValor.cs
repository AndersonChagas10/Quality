using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class ComponenteGenericoValor : BaseModel
    {
        public int Id { get; set; }
        public int SaveId { get; set; }
        public int ComponenteGenerico_Id { get; set; }
        public int ComponenteGenericoColuna_Id { get; set; }
        public int Valor { get; set; }

        [ForeignKey("ComponenteGenerico_Id")]
        public ComponenteGenerico ComponenteGenerico { get; set; }
        [ForeignKey("ComponenteGenericoColuna_Id")]
        public ComponenteGenericoColuna ComponenteGenericoColuna { get; set; }
    }
}
