namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TarefaCategorias
    {
        public int Id { get; set; }

        public int Tarefa { get; set; }

        public int Categoria { get; set; }

        public int UsuarioInsercao { get; set; }

        public DateTime DataInsercao { get; set; }

        public int? UsuarioAlteracao { get; set; }

        public DateTime? DataAlteracao { get; set; }

        public virtual Categorias Categorias { get; set; }

        public virtual Tarefas Tarefas { get; set; }
    }
}
