using Conformity.Domain.Core.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Conformity.Domain.Core.Entities.PlanoDeAcao
{
    [Table("PA.AcaoXNotificarAcao")]
    public partial class AcaoXNotificarAcao : BaseModel, IEntity
    {
        public int Acao_Id { get; set; }
        public int UserSgq_Id { get; set; }
        public bool IsActive { get; set; }
        public DateTime AddDate { get; set; }
        public DateTime AlterDate { get; set; }
    }
}
