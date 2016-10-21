using DTO.BaseEntity;

namespace DTO.DTO.Params
{
    public class ParHeaderFieldDTO : EntityBase
    {
        public int ParFieldType_Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int LevelShowm { get; set; }
        public bool LinkNumberEvaluetion { get; set; }
        public bool Active { get; set; }
    }
}