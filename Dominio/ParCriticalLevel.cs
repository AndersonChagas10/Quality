namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ParCriticalLevel")]
    public partial class ParCriticalLevel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ParCriticalLevel()
        {
            ParLevel1XCluster = new HashSet<ParLevel1XCluster>();
        }

        public int Id { get; set; }

        [Required(AllowEmptyStrings = true)]
        [StringLength(155)]
        public string Name { get; set; }

        public DateTime AddDate { get; set; }

        public DateTime? AlterDate { get; set; }

        public bool? IsActive { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParLevel1XCluster> ParLevel1XCluster { get; set; }
    }
}
