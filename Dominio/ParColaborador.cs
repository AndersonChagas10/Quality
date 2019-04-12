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
        [Required(ErrorMessage="O Nome é obrigatório")]
        public string Name { get; set; }
        [Required(ErrorMessage = "O Documento é obrigatório")]
        public string Documento { get; set; }
        [Required(ErrorMessage = "O Cargo é obrigatório")]
        public int ParCargo_Id { get; set; }
        public bool IsActive { get; set; }

        [ForeignKey("ParCargo_Id")]
        public virtual ParCargo ParCargo { get; set; }
    }
}
