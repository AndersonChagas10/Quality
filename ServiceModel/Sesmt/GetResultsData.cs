using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceModel
{
    public class GetResultsData
    {
        public int ParCompany_Id { get; set; }
        public DateTime CollectionDate { get; set; }
        public int ParFrequency_Id { get; set; }
    }
}
