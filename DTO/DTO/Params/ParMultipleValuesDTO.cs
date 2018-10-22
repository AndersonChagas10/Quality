using DTO.BaseEntity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DTO.DTO
{
    public class ParMultipleValuesDTO : EntityBase
    {
        public int ParHeaderField_Id { get; set; }
        [Display(Name = "Nome")]
        public string Name { get; set; }
        [DisplayName("Descrição")]
        public string Description { get; set; } = "";
        [DisplayName("Valor de punição")]
        public decimal PunishmentValue { get; set; }
        [DisplayName("Está conforme")]
        public bool Conformity { get; set; }
        [DisplayName("Está Ativo")]
        public bool IsActive { get; set; } = true;
        public bool IsDefaultOption { get; set; } = false;
    }
}