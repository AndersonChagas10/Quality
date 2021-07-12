using Conformity.Domain.Core.Interfaces;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Conformity.Domain.Core.Entities.PlanoDeAcao
{

    [Table("ParLevel2")]
    public partial class ParLevel2 : BaseModel, IEntity
    {

        [Required(AllowEmptyStrings = true)]
        [StringLength(155)]
        [DisplayName("Monitoramento")]
        public string Name { get; set; }
    }
}
