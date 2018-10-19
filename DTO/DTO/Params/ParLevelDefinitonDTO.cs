using DTO.BaseEntity;
using System.ComponentModel.DataAnnotations;

namespace DTO.DTO.Params
{
    public class ParLevelDefinitonDTO : EntityBase
    {
        public bool IsActive { get; set; }
        [Display(Name = "Nome")]
        public string Name { get; set; }
        [Display(Name = "Descrição")]
        public string Description { get; set; }


    }
}
