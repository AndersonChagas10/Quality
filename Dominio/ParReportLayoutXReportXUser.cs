using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class ParReportLayoutXReportXUser : BaseModel
    {
        public int Id { get; set; }

        public int ReportXUserSgq_Id { get; set; }

        public string Value { get; set; }

        public int LayoutLevel { get; set; }

        public int IdReference { get; set; }

        public string NickName { get; set; }

        public bool IsActive { get; set; }

        public string TableReference { get; set; }

        public int Ordenacao { get; set; }

        [NotMapped]
        public int Level1_Id { get; set; }

    }
}
