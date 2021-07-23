using Conformity.Domain.Core.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Conformity.Domain.Core.Entities.PlanoDeAcao
{
    [Table("AcaoXAttributes", Schema = "PA")]
    public partial class AcaoXAttributes : BaseModel, IEntity
    {
        public int Acao_Id { get; set; }
        public string FieldName { get; set; }
        public string Value { get; set; }

        [ForeignKey("Acao_Id")]
        public virtual Acao Acao { get; set; }
    }
}
