using Conformity.Domain.Core.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Conformity.Domain.Core.Entities.PlanoDeAcao
{
    [Table("AcompanhamentoAcaoXAttributes", Schema = "PA")]
    public partial class AcompanhamentoAcaoXAttributes : BaseModel, IEntity
    {
        public int AcompanhamentoAcao_Id { get; set; }
        public string FieldName { get; set; }
        public string Value { get; set; }

        [ForeignKey("AcompanhamentoAcao_Id")]
        public virtual AcompanhamentoAcao AcompanhamentoAcao { get; set; }
    }
}
