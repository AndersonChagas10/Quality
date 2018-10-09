namespace Dominio
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    
    [Table("ImportFormatItems")]
    public partial class ImportFormatItem : BaseModel
    {
        [Key]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Key { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Value { get; set; }

        [Required(AllowEmptyStrings = true)]
        [Column("ImportFormat_Id")]
        public int ImportFormat_Id { get; set; }

        [ForeignKey("ImportFormat_Id")]
        public virtual ImportFormat ImportFormat { get; set; }
    }
}
