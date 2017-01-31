using DTO.BaseEntity;

namespace DTO.DTO.Params
{
    public class ParLevel2XHeaderFieldDTO : EntityBase
    {
        public int ParLevel1_Id { get; set; }
        public int ParLevel2_Id { get; set; }
        public int ParHeaderField_Id { get; set; }
        public bool Active { get; set; } = true;
        
    }
}