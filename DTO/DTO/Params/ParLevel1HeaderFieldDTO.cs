using DTO.BaseEntity;

namespace DTO.DTO
{
    public class ParLevel1HeaderFieldDTO : EntityBase
    {
        public int ParLevel1_Id { get; set; }
        public int ParHeaderField_Id { get; set; }
        public bool Active { get; set; }
    }
}