namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class NiveisUsuarios
    {
        public int Id { get; set; }

        public int Usuario { get; set; }

        public int Nivel { get; set; }

        public int UsuarioInsercao { get; set; }

        public DateTime DataInsercao { get; set; }

        public int? UsuarioAlteracao { get; set; }

        public DateTime? DataAlteracao { get; set; }

        public virtual Niveis Niveis { get; set; }

        public virtual Usuarios Usuarios { get; set; }
    }
}
