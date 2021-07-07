using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Conformity.Domain.Core.Entities.PlanoDeAcao
{
    [Table("PA.AcaoXNotificarAcao")]
    public class AcaoXNotificarAcao
    {
        public int Id { get; set; }
        public int Acao_Id { get; set; }
        public int UserSgq_Id { get; set; }
        public bool IsActive { get; set; }
        public DateTime AddDate { get; set; }
        public DateTime AlterDate { get; set; }
    }
}
