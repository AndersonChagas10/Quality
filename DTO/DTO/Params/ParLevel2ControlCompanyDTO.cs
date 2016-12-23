using DTO.BaseEntity;
using System;

namespace DTO.DTO.Params
{
    public class ParLevel2ControlCompanyDTO : EntityBase
    {

        public Nullable<int> ParCompany_Id { get; set; }
        public Nullable<int> ParLevel1_Id { get; set; }
        public Nullable<int> ParLevel2_Id { get; set; }
        public Nullable<System.DateTime> InitDate { get; set; }
        public Nullable<bool> IsActive { get; set; }

        public ParCompanyDTO ParCompany { get; set; }
        public ParLevel1DTO ParLevel1 { get; set; }
        public ParLevel2DTO ParLevel2 { get; set; }
    }
}
