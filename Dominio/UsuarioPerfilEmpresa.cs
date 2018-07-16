namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UsuarioPerfilEmpresa")]
    public partial class UsuarioPerfilEmpresa
    {
        [Key]
        [Column(Order = 0)]
        public decimal nCdUsuario { get; set; }

        [Key]
        [Column(Order = 1)]
        public decimal nCdPerfil { get; set; }

        [Key]
        [Column(Order = 2)]
        public decimal nCdEmpresa { get; set; }
    }
}
