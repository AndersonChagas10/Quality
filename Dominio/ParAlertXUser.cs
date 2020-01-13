using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class ParAlertXUser : BaseModel
    {
        public int Id { get; set; }

        public bool IsActive { get; set; }

        [Column("ParCompany_Id")]
        public int? ParCompany_Ids { get; set; }

        public int ParAlert_Id { get; set; }

        public int UserSgq_Id { get; set; }

        [ForeignKey("ParCompany_Ids")]
        public virtual ParCompany ParCompany { get; set; }

        [ForeignKey("ParAlert_Id")]
        public virtual ParAlert ParAlert { get; set; }

        [ForeignKey("UserSgq_Id")]
        public virtual UserSgq UserSgq { get; set; }

    }
}
