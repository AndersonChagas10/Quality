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
    
    public partial class ParCluster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ParCluster()
        {
            this.ParCompanyCluster = new HashSet<ParCompanyCluster>();
            this.ParLevel1XCluster = new HashSet<ParLevel1XCluster>();
            this.ParClusterXModule = new HashSet<ParClusterXModule>();
        }
    
        public int Id { get; set; }
        public int ParClusterGroup_Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<int> ParClusterParent_Id { get; set; }
        public System.DateTime AddDate { get; set; }
        public Nullable<System.DateTime> AlterDate { get; set; }
        public bool IsActive { get; set; }
    
        public virtual ParClusterGroup ParClusterGroup { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParCompanyCluster> ParCompanyCluster { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParLevel1XCluster> ParLevel1XCluster { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParClusterXModule> ParClusterXModule { get; set; }
    }
}
