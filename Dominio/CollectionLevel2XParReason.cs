using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class CollectionLevel2XParReason : BaseModel
    {
        [Key]
        public int Id { get; set; }

        public int CollectionLevel2_Id { get; set; }

        public int ParReason_Id { get; set; }

        public int ParReasonType_Id { get; set; }

        /*[ForeignKey("CollectionLevel2_Id")]
        public virtual CollectionLevel2 CollectionLevel2 { get; set; }

        [ForeignKey("ParReason_Id")]
        public virtual ParReason ParReason { get; set; }*/

        public bool IsValid()
        {
            if (CollectionLevel2_Id <= 0 || ParReason_Id <= 0 || ParReasonType_Id <= 0)
            {
                return false;
            }

            return true;
        }

    }
}
