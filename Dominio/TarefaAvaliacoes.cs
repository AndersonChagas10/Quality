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
    
    public partial class TarefaAvaliacoes
    {
        public int Id { get; set; }
        public int Departamento { get; set; }
        public int Operacao { get; set; }
        public int Tarefa { get; set; }
        public int Avaliacao { get; set; }
        public string Acesso { get; set; }
        public int UsuarioInsercao { get; set; }
        public System.DateTime DataInsercao { get; set; }
        public Nullable<int> UsuarioAlteracao { get; set; }
        public Nullable<System.DateTime> DataAlteracao { get; set; }
        public Nullable<int> Unidade { get; set; }
    
        public virtual Departamentos Departamentos { get; set; }
        public virtual Operacoes Operacoes { get; set; }
        public virtual Tarefas Tarefas { get; set; }
        public virtual Unidades Unidades { get; set; }
    }
}
