using DTO.BaseEntity;
using System;

namespace DTO.DTO.Params
{
    public class ParEvaluationDTO : EntityBase
    {
        public Nullable<int> ParCompany_Id { get; set; }
        public int ParLevel2_Id { get; set; }
        public int Number { get; set; }
        public bool IsActive { get; set; } = true;

    }
}