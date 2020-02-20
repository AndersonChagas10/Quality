using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Seara
{
    public class ParVinculoPesoParLevel2 : BaseModel
    {
        public int Id { get; set; }
        public int ParLevel1_Id { get; set; }
        public int ParLevel2_Id { get; set; }
        public string Equacao { get; set; }
        public decimal Peso { get; set; }
        public bool IsActive { get; set; }

        [ForeignKey("ParLevel1_Id")]
        public virtual ParLevel1 ParLevel1 { get; set; }

        [ForeignKey("ParLevel2_Id")]
        public virtual ParLevel2 ParLevel2 { get; set; }
    }
}
