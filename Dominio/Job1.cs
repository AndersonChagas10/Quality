namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HangFire.Job")]
    public partial class Job1
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Job1()
        {
            JobParameter1 = new HashSet<JobParameter1>();
            State1 = new HashSet<State1>();
        }

        public int Id { get; set; }

        public int? StateId { get; set; }

        [StringLength(20)]
        public string StateName { get; set; }

        [Required]
        public string InvocationData { get; set; }

        [Required]
        public string Arguments { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? ExpireAt { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<JobParameter1> JobParameter1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<State1> State1 { get; set; }
    }
}
