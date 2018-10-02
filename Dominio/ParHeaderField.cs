namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ParHeaderField")]
    public partial class ParHeaderField
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ParHeaderField()
        {
            CollectionLevel2XParHeaderField = new HashSet<CollectionLevel2XParHeaderField>();
            ParLevel1XHeaderField = new HashSet<ParLevel1XHeaderField>();
            ParMultipleValues = new HashSet<ParMultipleValues>();
        }

        public int Id { get; set; }

        public int ParFieldType_Id { get; set; }

        [Required(AllowEmptyStrings = true)]
        [StringLength(155)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Description { get; set; }

        public int ParLevelDefinition_Id { get; set; }

        public bool LinkNumberEvaluetion { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime AddDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? AlterDate { get; set; }

        public bool IsActive { get; set; }

        public bool? IsRequired { get; set; }

        public bool duplicate { get; set; }

        public bool CheckBox { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CollectionLevel2XParHeaderField> CollectionLevel2XParHeaderField { get; set; }

        public virtual ParFieldType ParFieldType { get; set; }

        public virtual ParLevelDefiniton ParLevelDefiniton { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParLevel1XHeaderField> ParLevel1XHeaderField { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParMultipleValues> ParMultipleValues { get; set; }
    }
}
