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
    
    public partial class UsuarioUnidades
    {
        public int Id { get; set; }
        public int Usuario { get; set; }
        public int Unidade { get; set; }
        public int UsuarioInsercao { get; set; }
        public System.DateTime DataInsercao { get; set; }
        public Nullable<int> UsuarioAlteracao { get; set; }
        public Nullable<System.DateTime> DataAlteracao { get; set; }
    
        public virtual Unidades Unidades { get; set; }
    }
}
