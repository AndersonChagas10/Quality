namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VinculoParticipanteMultiplaEscolha")]
    public partial class VinculoParticipanteMultiplaEscolha
    {
        public int Id { get; set; }

        public int IdParticipante { get; set; }

        public int IdMultiplaEscolha { get; set; }

        public virtual MultiplaEscolha MultiplaEscolha { get; set; }

        public virtual Usuarios Usuarios { get; set; }
    }
}
