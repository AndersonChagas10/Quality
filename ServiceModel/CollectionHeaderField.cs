using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceModel
{
    public partial class CollectionHeaderField
    {
        public long ParLevel1_Id { get; set; }
        public long ParLevel2_Id { get; set; }
        public int Period { get; set; }
        public int Shift { get; set; }
        public int ParHeaderField_Id { get; set; }
        public int Evaluation { get; set; }
        public int Sample { get; set; }
        public string Value { get; set; }
    }
}
