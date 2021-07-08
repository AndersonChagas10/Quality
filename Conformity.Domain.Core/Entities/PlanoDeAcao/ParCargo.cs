using Conformity.Domain.Core.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Conformity.Domain.Core.Entities.PlanoDeAcao
{
    [Table("ParCargo")]
    public class ParCargo : BaseModel, IEntity
    {
        [Required(ErrorMessage = "O campo Nome é Obrigatório")]
        public string Name { get; set; }
        public bool IsActive { get; set; }

        [NotMapped]
        public virtual int[] ParDepartment_Ids { get; set; }
    }
}
