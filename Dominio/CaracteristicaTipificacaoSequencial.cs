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
    
    public partial class CaracteristicaTipificacaoSequencial
    {
        public int Id { get; set; }
        public int Sequencial { get; set; }
        public Nullable<decimal> nCdCaracteristica_Id { get; set; }
    
        public virtual CaracteristicaTipificacao CaracteristicaTipificacao { get; set; }
    }
}
