using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class ParReportXFilter : BaseModel
    {
        public int Id { get; set; }

        [NotMapped]
        public string FilterLevel_Name { get; set; }

        public int FilterLevel { get; set; }

        public bool IsActive { get; set; }

        public int ReportXUserSgq_Id { get; set; }

        [NotMapped]
        public int Level1_Id { get; set; }
    }
}
