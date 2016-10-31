using DTO.BaseEntity;

namespace DTO.DTO.Params
{
    public class ParLevel3Level2Level1DTO : EntityBase
    {

        public int ParLevel3Level2_Id { get; set; }
        public int ParLevel1_Id { get; set; }
        public bool Active { get; set; }

        public ParLevel1DTO ParLevel1 { get; set; }
        public ParLevel3Level2DTO ParLevel3Level2 { get; set; }

    }
}
