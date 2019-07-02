using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceModel
{
    public partial class _PCC1B
    {
        public int Sequential { get; set; }
        public int Side { get; set; }
        public string serverSide { get; set; }
    };

    public partial class _Receive
    {
        public int sequencialAtual { get; set; }
        public String Data { get; set; }
        public int Unit { get; set; }
        public int ParLevel2 { get; set; }
        public int HashKey { get; set; }
    }

    public class CollectionLevel2PCC1B
    {
        public string CollectionDate { get; set; }
        public int ParLevel1_Id { get; set; }
        public int ParLevel2_Id { get; set; }
        public int UnitId { get; set; }
        public int Sequential { get; set; }
        public int Side { get; set; }
        public int DefectsResult { get; set; }
        public string Key { get; set; }
    }
}
