
namespace Dominio
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("FormatoLayoutImportacaoItem")]
    public partial class FormatoLayoutImportacaoItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "Chave")]
        public DateTime Chave { get; set; }

        [Required]
        [Column(TypeName = "Valor")]
        public DateTime Valor { get; set; }

        public FormatoLayoutImportacao FormatoLayoutImportacao { get; set; }
    }
}
