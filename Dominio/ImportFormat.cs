namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    
    public partial class ImportFormat
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Title { get; set; }

        public DateTime AddDate { get; set; }

        public DateTime? AlterDate { get; set; }

        public bool? IsActive { get; set; }

        public virtual ICollection<ImportFormatItem> ImportFormatItems { get; set; }
    }
}
