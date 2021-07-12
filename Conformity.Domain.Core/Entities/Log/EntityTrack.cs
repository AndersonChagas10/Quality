using System;
using Conformity.Domain.Core.Enums.Log;
using Conformity.Domain.Core.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Conformity.Domain.Core.Entities.Log
{
    [Table("EntityTrack", Schema = "LOG")]
    public partial class EntityTrack : BaseModel, IEntity
    {
        public EntityTrack()
        {
        }

        public DateTime RegisterDate { get; set; }
        public int User_Id { get; set; }
        public int Entity_Id { get; set; }
        public string TableName { get; set; }
        public string FieldName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public ELogTrackEvent TypeEntityTrack { get; set; }

    }
}
