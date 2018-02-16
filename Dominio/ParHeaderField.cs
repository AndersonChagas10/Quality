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
    
    public partial class ParHeaderField
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ParHeaderField()
        {
            this.CollectionLevel2XParHeaderField = new HashSet<CollectionLevel2XParHeaderField>();
            this.ParLevel1XHeaderField = new HashSet<ParLevel1XHeaderField>();
            this.ParMultipleValues = new HashSet<ParMultipleValues>();
        }
    
        public int Id { get; set; }
        public int ParFieldType_Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ParLevelDefinition_Id { get; set; }
        public bool LinkNumberEvaluetion { get; set; }
        public System.DateTime AddDate { get; set; }
        public Nullable<System.DateTime> AlterDate { get; set; }
        public bool IsActive { get; set; }
        public Nullable<bool> IsRequired { get; set; }
        public bool duplicate { get; set; }
        public bool CheckBox { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CollectionLevel2XParHeaderField> CollectionLevel2XParHeaderField { get; set; }
        public virtual ParFieldType ParFieldType { get; set; }
        public virtual ParLevelDefiniton ParLevelDefiniton { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParLevel1XHeaderField> ParLevel1XHeaderField { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParMultipleValues> ParMultipleValues { get; set; }
    }
}
