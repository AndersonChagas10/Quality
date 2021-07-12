using Conformity.Domain.Core.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Conformity.Domain.Core.Entities.PlanoDeAcao
{
    [Table("ParDepartment")]
    public partial class ParDepartment : BaseModel, IEntity
    {
        public ParDepartment()
        {
            ParLevel2 = new HashSet<ParLevel2>();
        }

        [Required(AllowEmptyStrings = true)]
        [StringLength(155)]
        [DisplayName("Nome")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Description { get; set; }

        public bool Active { get; set; } = true;

        public string Hash { get; set; }

        public int? ParCompany_Id { get; set; }

        [DisplayName("É filho de")]
        public int? Parent_Id { get; set; }

        [NotMapped]
        public string Parent { get; set; }

        public int? ParDepartmentGroup_Id { get; set; }

        [ForeignKey("Parent_Id")]
        public virtual ParDepartment ParDepartmentPai { get; set; }

        public virtual ICollection<ParLevel2> ParLevel2 { get; set; }

        [ForeignKey("ParCompany_Id")]
        public virtual ICollection<ParCompany> ParCompany { get; set; }

        [NotMapped]
        public ICollection<ParDepartment> ParDepartmentFilho { get; set; }
    }
}
