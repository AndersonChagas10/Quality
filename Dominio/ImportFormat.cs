namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    
    [Table("ImportFormats")]
    public partial class ImportFormat : BaseModel
    {
        [Key]
        public int Id { get; set; }

        //[Required(AllowEmptyStrings = true)]
        [DisplayName("Titulo")]
        public string Title { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<ImportFormatItem> ImportFormatItems { get; set; }
    }
}
