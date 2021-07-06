using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conformity.Domain.Core.DTOs
{
    public class EntityTrackViewModel
    {
        public string UserName { get; set; }
        public DateTime UpdateDate { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string FieldName { get; set; }
    }
}
