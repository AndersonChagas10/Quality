using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class ParColaboradorXCargo : BaseModel
    {
        [Key]
        public int Id { get; set; }
        public int ParCargo_Id { get; set; }
        public int ParColaborador_Id { get; set; }
        public bool IsActive { get; set; }

        [ForeignKey("ParCargo_Id")]
        public virtual ParCargo ParCargo { get; set; }

        [ForeignKey("ParColaborador_Id")]
        public virtual ParColaborador ParColaborador { get; set; }
    }
}
