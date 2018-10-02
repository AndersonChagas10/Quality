using DTO.BaseEntity;

namespace DTO.DTO.Params
{
    public class ParLevel1XHeaderFieldDTO : EntityBase
    {
        public int ParLevel1_Id { get; set; }
        public int ParHeaderField_Id { get; set; }
        public bool Active { get; set; } = true;
        public string HeaderFieldGroup { get; set; }

        /*Para inclusão*/
        public ParHeaderFieldDTO parHeaderFieldDto { get; set; }
        /*Para alteração*/
        public ParHeaderFieldDTO ParHeaderField { get; set; }
    }
}