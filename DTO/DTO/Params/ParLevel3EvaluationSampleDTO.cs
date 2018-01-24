using DTO.BaseEntity;
using DTO.Helpers;

namespace DTO.DTO.Params
{
    public class ParLevel3EvaluationSampleDTO : EntityBase
    {
        
        public int ParLevel3EvaluationSample_Id { get; set; }
        
        public int? CompanyId { get; set; }
        public int? ParLevel1_Id { get; set; }
        public int? ParLevel2_Id { get; set; }

        public decimal? EampleNumber { get; set; } = 0;
        public decimal? EvaluationNumber { get; set; } = 0;

        public string EvaluationInterval { get; set; }

        public bool IsActive { get; set; } = true;
        
        public ParCompanyDTO ParCompany { get; set; }
        public ParLevel1DTO ParLevel1 { get; set; }
        public ParLevel2DTO ParLevel2 { get; set; }

        public void PreparaParaInsertEmBanco()
        {
            if (CompanyId <= 0)
                CompanyId = null;

            if (ParLevel1_Id <= 0)
                ParLevel1_Id = null;

            if (ParLevel2_Id <= 0)
                ParLevel2_Id = null;
        }

        public void PreparaGet()
        {
        }
    }
}