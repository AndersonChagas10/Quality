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
    
    public partial class pa_vinculo_participante_projeto
    {
        public int id { get; set; }
        public int id_projeto { get; set; }
        public int id_participante { get; set; }
    
        public virtual pa_participante pa_participante { get; set; }
        public virtual pa_projeto pa_projeto { get; set; }
    }
}
