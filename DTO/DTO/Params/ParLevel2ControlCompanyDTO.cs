using DTO.BaseEntity;
using System;
using System.Collections.Generic;

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

        public ParLevel2ControlCompanyDTO()
        {

        }

      

        public ParLevel2ControlCompanyDTO(int level2Id, int level1Id, int? parcompanyId, DateTime initdate)
        {
            ParLevel2_Id = level2Id;
            ParLevel1_Id = level1Id;
            ParCompany_Id = parcompanyId;
            InitDate = initdate;


            if (parcompanyId != null)
                if (parcompanyId > 0)
                    ParCompany_Id = parcompanyId;
        }

    }
}
