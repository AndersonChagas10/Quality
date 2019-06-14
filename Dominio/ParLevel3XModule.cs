using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class ParLevel3XModule : BaseModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public decimal Points { get; set; }

        public DateTime? EffectiveDateEnd { get; set; }

        public DateTime? EffectiveDateStart { get; set; }

        public int ParLevel1_Id { get; set; }

        public int ParLevel2_Id { get; set; }

        public int ParLevel3_Id { get; set; }

        public int ParCompany_Id { get; set; }

        public int ParDepartment_Id { get; set; }

        public int ParModule_Id { get; set; }

        [ForeignKey("ParLevel3_Id")]
        public virtual ParLevel3 ParLevel3 { get; set; }

        [ForeignKey("ParLevel2_Id")]
        public virtual ParLevel2 ParLevel2 { get; set; }

        [ForeignKey("ParLevel1_Id")]
        public virtual ParLevel1 ParLevel1 { get; set; }

        [ForeignKey("ParCompany_Id")]
        public virtual ParCompany ParCompany { get; set; }

        [ForeignKey("ParDepartment_Id")]
        public virtual ParDepartment ParDepartment { get; set; }

        [ForeignKey("ParModule_Id")]
        public virtual ParModule ParModule { get; set; }
    }
}
