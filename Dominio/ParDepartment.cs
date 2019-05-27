namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ParDepartment")]
    public partial class ParDepartment : BaseModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ParDepartment()
        {
            ParLevel2 = new HashSet<ParLevel2>();
        }

        public int Id { get; set; }

        [Required(AllowEmptyStrings = true)]
        [StringLength(155)]
        [DisplayName("Nome")]
        public string Name { get; set; }

        [Display(Name = "description", ResourceType = typeof(Resources.Resource))]
        [Required(AllowEmptyStrings = true)]
        public string Description { get; set; }

        public bool Active { get; set; } = true;
       
        public string Hash { get; set; }

        public int? ParCompany_Id { get; set; }

        [DisplayName("É filho de")]
        public int? Parent_Id { get; set; }

        public int? ParDepartmentGroup_Id { get; set; }

        [ForeignKey("Parent_Id")]
        public virtual ParDepartment ParDepartmentPai { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParLevel2> ParLevel2 { get; set; }

        [ForeignKey("ParCompany_Id")]
        public virtual ICollection<ParCompany> ParCompany { get; set; }

        [ForeignKey("ParDepartmentGroup_Id")]
        public virtual ParDepartmentGroup ParDepartmentGroup { get; set; }
    }
}
