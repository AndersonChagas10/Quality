namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VinculoParticipanteProjeto")]
    public partial class VinculoParticipanteProjeto
    {
        public int Id { get; set; }

        public int IdProjeto { get; set; }

        public int IdParticipante { get; set; }

        public virtual Projeto Projeto { get; set; }

        public virtual Usuarios Usuarios { get; set; }
    }
}
