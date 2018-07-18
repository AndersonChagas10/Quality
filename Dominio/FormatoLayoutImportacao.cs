namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("FormatoLayoutImportacao")]
    public partial class FormatoLayoutImportacao
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "Titulo")]
        public DateTime Titulo { get; set; }

        public virtual ICollection<FormatoLayoutImportacaoItem> FormatoLayoutImportacaoItens { get; set; }
    }
}
