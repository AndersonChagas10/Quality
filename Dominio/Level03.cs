//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Dominio
{
    using System;
    using System.Collections.Generic;
    
    public partial class Level03
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Level03()
        {
            this.CorrectiveActionLevels = new HashSet<CorrectiveActionLevels>();
            this.CollectionLevel03 = new HashSet<CollectionLevel03>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public System.DateTime AddDate { get; set; }
        public Nullable<System.DateTime> AlterDate { get; set; }
        public bool Active { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CorrectiveActionLevels> CorrectiveActionLevels { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CollectionLevel03> CollectionLevel03 { get; set; }
    }
}
