using Dominio;
using DTO.BaseEntity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SgqSystem.Controllers.Recravacao
{
    public class ParRecravacao_Linhas : EntityBase
    {
        [Display(Name = "Nome")]
        [Required(ErrorMessage = "É necessário preencher o campo nome.")]
        [MaxLength(100, ErrorMessage = "O tamanho máximo do Nome são 100 caracteres.")]
        public string Name { get; set; }

        [Display(Name = "Unidade")]
        [Required(ErrorMessage = "É necessário selecionar uma empresa.")]
        public int ParCompany_Id { get; set; }

        [Display(Name = "Tipo de Lata")]
        [Required(ErrorMessage = "É necessário selecionar um tipo de lata.")]
        public int ParRecravacao_TypeLata_Id { get; set; }

        [Display(Name = "Quantidade de Cabeças")]
        [Range(1, 9999999999, ErrorMessage = "Ao menos uma cabeça é necessária.")]
        public int NumberOfHeads { get; set; }

        [Display(Name = "Descrição")]
        [MaxLength(100, ErrorMessage = "O tamanho máximo do Nome são 100 caracteres.")]
        public string Description { get; set; }

        [Display(Name = "Ativo")]
        public bool IsActive { get; set; }

        [ForeignKey("ParCompany_Id")]
        public virtual ParCompany ParCompany { get; set; }

        [ForeignKey("ParRecravacao_TypeLata_Id")]
        public virtual ParRecravacao_TipoLata TipoLata { get; set; }
    }
}