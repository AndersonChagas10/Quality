namespace Dominio
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    
    [Table("ImportFormatItems")]
    public partial class ImportFormatItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Key { get; set; }

        [Required]
        public string Value { get; set; }

        public DateTime AddDate { get; set; }

        public DateTime? AlterDate { get; set; }

        [Required]
        [Column("ImportFormat_Id")]
        public int ImportFormat_Id { get; set; }

        [ForeignKey("ImportFormat_Id")]
        public virtual ImportFormat ImportFormat { get; set; }
    }
}
