using Conformity.Domain.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conformity.Domain.Core.Entities
{
    public abstract class BaseModel : IEntity
    {
        public int Id { get; set; }
    }
}
