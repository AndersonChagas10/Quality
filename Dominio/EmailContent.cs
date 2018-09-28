namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EmailContent")]
    public partial class EmailContent : BaseModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EmailContent()
        {
            CorrectiveAction = new HashSet<CorrectiveAction>();
            Deviation = new HashSet<Deviation>();
        }

        public int Id { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string To { get; set; }

        public string Body { get; set; }
        
        public string SendStatus { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? SendDate { get; set; }

        [StringLength(15)]
        public string Project { get; set; }

        public bool? IsBodyHtml { get; set; }

        [StringLength(350)]
        public string From { get; set; }

        [StringLength(2900)]
        public string Subject { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CorrectiveAction> CorrectiveAction { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Deviation> Deviation { get; set; }
    }
}
