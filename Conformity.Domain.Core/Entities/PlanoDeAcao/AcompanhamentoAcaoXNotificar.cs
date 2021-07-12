using Conformity.Domain.Core.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Conformity.Domain.Core.Entities.PlanoDeAcao
{
    [Table("PA.AcompanhamentoAcaoXNotificar")]
    public partial class AcompanhamentoAcaoXNotificar : BaseModel, IEntity
    {
        public int UserSgq_Id { get; set; }
        public bool IsActive { get; set; } = true;

        [ForeignKey("UserSgq_Id")]
        public virtual UserSgq UserSgq { get; set; }
    }
}
