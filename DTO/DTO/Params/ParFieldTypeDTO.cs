using DTO.BaseEntity;
using System.ComponentModel.DataAnnotations;

namespace DTO.DTO.Params
{
    public class ParFieldTypeDTO : EntityBase
    {
        [Display(Name = "Nome")]
        public string Name { get; set; }
        [Display(Name = "Descrição")]
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}