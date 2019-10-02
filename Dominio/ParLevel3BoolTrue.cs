namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ParLevel3BoolTrue : BaseModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ParLevel3BoolTrue()
        {
            ParLevel3Value = new HashSet<ParLevel3Value>();
        }

        public int Id { get; set; }

        //[Required(AllowEmptyStrings = true)]
        [StringLength(155)]
        public string Name { get; set; }

        public bool IsActive { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParLevel3Value> ParLevel3Value { get; set; }
    }
}
