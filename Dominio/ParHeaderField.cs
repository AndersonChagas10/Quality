namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ParHeaderField")]
    public partial class ParHeaderField : BaseModel
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
        [Display(Name="Nome")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = true)]
        [Display(Name = "Descrição")]
        public string Description { get; set; }

        public int ParLevelDefinition_Id { get; set; }

        public bool LinkNumberEvaluetion { get; set; }

        public bool IsActive { get; set; }

        public bool? IsRequired { get; set; }

        public bool duplicate { get; set; }

        public bool CheckBox { get; set; }

        public virtual ICollection<CollectionLevel2XParHeaderField> CollectionLevel2XParHeaderField { get; set; }

        [ForeignKey("ParFieldType_Id")]
        public virtual ParFieldType ParFieldType { get; set; }

        [ForeignKey("ParLevelDefinition_Id")]
        public virtual ParLevelDefiniton ParLevelDefiniton { get; set; }

        public virtual ICollection<ParLevel1XHeaderField> ParLevel1XHeaderField { get; set; }

        public virtual ICollection<ParMultipleValues> ParMultipleValues { get; set; }
    }
}
