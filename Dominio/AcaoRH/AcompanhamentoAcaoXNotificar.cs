using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.AcaoRH
{
    [Table("PA.AcompanhamentoAcaoXNotificar")]
    public class AcompanhamentoAcaoXNotificar
    {
        public int Id { get; set; }
        public int UserSgq_Id { get; set; }
        public bool IsActive { get; set; } = true;

        [ForeignKey("UserSgq_Id")]
        public virtual UserSgq UserSgq { get; set; }
    }
}
