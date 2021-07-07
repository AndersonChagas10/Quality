using Conformity.Domain.Core.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Conformity.Domain.Core.Entities.PlanoDeAcao
{
    [Table("ParCluster")]
    public partial class ParCluster : BaseModel, IEntity
    {

        public int ParClusterGroup_Id { get; set; }

        //[Required(AllowEmptyStrings = true)]
        [StringLength(155)]
        public string Name { get; set; }

        //[Required(AllowEmptyStrings = true)]
        public string Description { get; set; }

        public int? ParClusterParent_Id { get; set; }

        public bool IsActive { get; set; }

        [ForeignKey("ParClusterGroup_Id")]
        public virtual ParClusterGroup ParClusterGroup { get; set; }
    }
}
