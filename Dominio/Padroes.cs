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
    
    public partial class Padroes
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Padroes()
        {
            this.PadraoMonitoramentos = new HashSet<PadraoMonitoramentos>();
            this.PadraoTolerancias = new HashSet<PadraoTolerancias>();
            this.PadraoTolerancias1 = new HashSet<PadraoTolerancias>();
        }
    
        public int Id { get; set; }
        public string Minimo { get; set; }
        public string Maximo { get; set; }
        public int UsuarioInsercao { get; set; }
        public System.DateTime DataInsercao { get; set; }
        public Nullable<int> UsuarioAlteracao { get; set; }
        public Nullable<System.DateTime> DataAlteracao { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PadraoMonitoramentos> PadraoMonitoramentos { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PadraoTolerancias> PadraoTolerancias { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PadraoTolerancias> PadraoTolerancias1 { get; set; }
    }
}
