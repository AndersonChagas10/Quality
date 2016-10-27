using DTO.BaseEntity;

namespace DTO.DTO
{
    public class ParMultipleValuesDTO : EntityBase
    {
        public int ParHeaderField_Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } = "";
        public decimal PunishmentValue { get; set; }
        public bool Conformity { get; set; }
        public bool IsActive { get; set; } = true;
    }
}