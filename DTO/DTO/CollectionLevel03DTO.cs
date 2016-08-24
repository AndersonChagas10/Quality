using DTO.BaseEntity;
using DTO.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTO
{
    public class CollectionLevel03DTO : EntityBase
    {
        public CollectionLevel03DTO(NextnextRoot nextnextRoot)
        {
            ValidaBaseEntity();

            //CollectionLevel02_ID

            Level03_Id = int.Parse(nextnextRoot.level03id);
            Guard.ForValidFk(Level03_Id.Value, "Level03 Id must be valid.");

            //TotalError = decimal.Parse(nextnextRoot.totalerror);
            //Guard.ForNegative(TotalError, "Total Error level03");

            ConformedIs = Convert.ToBoolean(nextnextRoot.conform);
            Guard.VerifyIfIsBool(ConformedIs, "ConformedIs");

            Value = decimal.Parse(nextnextRoot.value);
            Guard.ForNegative(Value, "Value level03");

            ValueText = "";

        }

        public int CollectionLevel02_ID { get; set; }
        public Nullable<int> Level03_Id { get; set; }
        public decimal TotalError { get; set; }
        public bool ConformedIs { get; set; }
        public decimal Value { get; set; }
        public string ValueText { get; set; }


    }
}
