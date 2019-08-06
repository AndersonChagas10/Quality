using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class ParMultipleValuesGeral : BaseModel 
    {
        public int Id { get; set; }
        public int ParHeaderFieldGeral_Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal PunishmentValue { get; set; }
        public bool Conformity { get; set; }
        public bool IsActive { get; set; }
        public bool? IsDefaultOption { get; set; }

        [ForeignKey("ParHeaderFieldGeral_Id")]
        public virtual ParHeaderFieldGeral ParHeaderFieldGeral { get; set; }
    }
}
