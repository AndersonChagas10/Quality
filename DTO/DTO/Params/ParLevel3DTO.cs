using DTO.BaseEntity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DTO.DTO.Params
{
    public class ParLevel3DTO : EntityBase
    {
        [Required(ErrorMessage = "O Nome é obrigatório.")]
        [MinLength(3, ErrorMessage = "O tamanho mínimo do Nome são 3 caracteres.")]
        [MaxLength(140, ErrorMessage = "O tamanho máximo do Nome são 140 caracteres.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "A Descrição é obrigatória.")]
        [MinLength(3, ErrorMessage = "O tamanho mínimo da Descrição deve ser 3 caracteres.")]
        [MaxLength(140, ErrorMessage = "O tamanho máximo da Descrição deve ser no 140 caracteres.")]
        public string Description { get; set; }

        public bool IsActive { get; set; } = true;

        public decimal pesoDoVinculo { get; set; }
        public virtual List<ParLevel3ValueDTO> ParLevel3Value { get; set; }
        public List<ParLevel3GroupDTO> listGroupsLevel2 { get; set; }
        public int? groupLevel2Selected;

        public List<ParRelapseDTO> listParRelapseDto { get; set; }
        public List<int> removeReincidencia { get; set; }
    }
}