using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class ParCargo : BaseModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "O campo Nome é Obrigatório")]
        public string Name { get; set; }
        public bool IsActive { get; set; }

        [NotMapped]
        public virtual int[] ParDepartment_Ids { get; set; }
    }
}
