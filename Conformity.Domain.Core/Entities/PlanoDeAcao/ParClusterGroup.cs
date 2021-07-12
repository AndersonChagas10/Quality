using Conformity.Domain.Core.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Conformity.Domain.Core.Entities.PlanoDeAcao
{
    [Table("ParClusterGroup")]
    public partial class ParClusterGroup : BaseModel, IEntity
    {
        public ParClusterGroup()
        {
            ParCluster = new HashSet<ParCluster>();
        }

        [Required(AllowEmptyStrings = true)]
        [StringLength(155)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Description { get; set; }

        public int? ParClusterGroupParent_Id { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<ParCluster> ParCluster { get; set; }
    }
}
