using DTO.BaseEntity;
using System.ComponentModel.DataAnnotations;

namespace SgqSystem.Controllers.Recravacao
{
    public class ParRecravacao_TipoLata : EntityBase
    {
        [Display(Name = "Nome")]
        [Required(ErrorMessage = "É necessário preencher o campo nome.")]
        [MaxLength(100, ErrorMessage = "O tamanho máximo do Nome são 100 caracteres.")]
        public string Name { get; set; }

        [Display(Name = "Descrição")]
        [MaxLength(100, ErrorMessage = "O tamanho máximo do Nome são 100 caracteres.")]
        public string Description { get; set; }

        [Display(Name = "Quantidade de Pontos de Medida")]
        [Range(1, 9999999999, ErrorMessage = "Ao menos um ponto de medida é necessário.")]
        public int NumberOfPoints { get; set; }

        [Display(Name = "Ativo")]
        public bool IsActive { get; set; }
    }
}