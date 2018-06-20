namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VinculoCampoCabecalho")]
    public partial class VinculoCampoCabecalho
    {
        public int Id { get; set; }

        public int? IdMultiplaEscolha { get; set; }

        public int IdCampo { get; set; }

        public int IdCabecalho { get; set; }

        public string Valor { get; set; }

        public int? IdParticipante { get; set; }

        public int? IdGrupoCabecalho { get; set; }

        public virtual Cabecalho Cabecalho { get; set; }

        public virtual Campo Campo { get; set; }

        public virtual GrupoCabecalho GrupoCabecalho { get; set; }

        public virtual MultiplaEscolha MultiplaEscolha { get; set; }

        public virtual Usuarios Usuarios { get; set; }
    }
}
