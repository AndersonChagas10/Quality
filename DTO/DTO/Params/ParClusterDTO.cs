using DTO.BaseEntity;

namespace DTO.DTO.Params
{
    public class ParClusterDTO : EntityBase
    {
        public int ParClusterGroup_Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ParClusterParent_Id { get; set; }
        public bool IsActive { get; set; }
    }
}