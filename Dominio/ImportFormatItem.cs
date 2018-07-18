
namespace Dominio
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    
    public partial class ImportFormatItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Key { get; set; }

        [Required]
        public DateTime Value { get; set; }

        public DateTime AddDate { get; set; }

        public DateTime? AlterDate { get; set; }

        [Required]
        public int ImportFormat_Id { get; set; }

        public ImportFormat ImportFormat { get; set; }
    }
}
