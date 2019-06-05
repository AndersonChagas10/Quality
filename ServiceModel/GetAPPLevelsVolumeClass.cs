using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceModel
{
    public class GetAPPLevelsVolumeClass
    {
        public int UserSgq_Id { get; set; }
        public int ParCompany_Id { get; set; }
        public DateTime Date { get; set; }
        public string Level1ListId { get; set; }
        public int Shift_Id { get; set; }
    }
}
