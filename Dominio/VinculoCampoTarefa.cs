namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VinculoCampoTarefa")]
    public partial class VinculoCampoTarefa
    {
        public int Id { get; set; }

        public int? IdMultiplaEscolha { get; set; }

        public int IdCampo { get; set; }

        public int IdTarefa { get; set; }

        public string Valor { get; set; }

        public int? IdParticipante { get; set; }

        public virtual Campo Campo { get; set; }

        public virtual MultiplaEscolha MultiplaEscolha { get; set; }

        public virtual TarefaPA TarefaPA { get; set; }

        public virtual Usuarios Usuarios { get; set; }
    }
}
