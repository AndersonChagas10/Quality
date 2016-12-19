using System;
using DTO.BaseEntity;

namespace DTO.DTO.Params
{
    public class ParLevel3Level2DTO : EntityBase
    {
        public int ParLevel2_Id { get; set; }
        public int ParLevel3_Id { get; set; }
        public int? ParLevel3Group_Id { get; set; }
        public int? ParCompany_Id { get; set; }
        public decimal Weight { get; set; }
        public bool IsActive { get; set; } = true;

        public ParLevel2DTO ParLevel2 { get; set; }
        public ParLevel3DTO ParLevel3 { get; set; }
        public ParLevel3GroupDTO ParLevel3Group { get; set; }
        public ParCompanyDTO ParCompany { get; set; }

        public void preparaParaInsertEmBanco()
        {
            if (ParCompany_Id <= 0)
                ParCompany_Id = null;

            if (ParLevel3Group_Id <= 0)
                ParLevel3Group_Id = null;

        }
    }
}