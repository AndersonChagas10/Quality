using DTO.BaseEntity;
using System;

namespace DTO.DTO.Params
{
    public class ParLevel2Level1DTO : EntityBase
    {
        public int ParLevel1_Id { get; set; }
        public int ParLevel2_Id { get; set; }
        public Nullable<int> ParCompany_Id { get; set; }
        public bool IsActive { get; set; }

        public ParLevel1DTO ParLevel1 { get; set; }
        public ParLevel2DTO ParLevel2 { get; set; }
    }
}
