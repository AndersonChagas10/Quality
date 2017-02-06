using DTO.BaseEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTO
{
    public class DefectDTO : EntityBase
    {
        public int ParCompany_Id { get; set; }
        public int ParLevel1_Id { get; set; }
        public int CurrentEvaluation { get; set; }
        public int Evaluations { get; set; }
        public decimal Defects { get; set; }
        public System.DateTime Date { get; set; }
        public bool Active { get; set; }

    }
}
