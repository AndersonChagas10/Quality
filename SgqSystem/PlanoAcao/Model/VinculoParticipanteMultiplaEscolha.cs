//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SgqSystem.PlanoAcao.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class VinculoParticipanteMultiplaEscolha
    {
        public int Id { get; set; }
        public int IdParticipante { get; set; }
        public int IdMultiplaEscolha { get; set; }
    
        public virtual MultiplaEscolha MultiplaEscolha { get; set; }
        public virtual Usuarios Usuarios { get; set; }
    }
}
