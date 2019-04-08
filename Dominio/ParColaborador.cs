using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class ParColaborador : BaseModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Documento { get; set; }
        public int ParCargo_Id { get; set; }
        public bool IsActive { get; set; }

        [ForeignKey("ParCargo_Id")]
        public virtual ParCargo ParCargo { get; set; }
    }
}
