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
    
    public partial class pa_acompanhamento_tarefa
    {
        public int id { get; set; }
        public int id_tarefa { get; set; }
        public System.DateTime data_envio { get; set; }
        public string comentario { get; set; }
        public string enviado { get; set; }
        public string status { get; set; }
        public string nome_participante_envio { get; set; }
    
        public virtual pa_tarefa pa_tarefa { get; set; }
    }
}
