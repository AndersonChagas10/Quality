using DTO.Helpers;

namespace DTO.BaseEntity
{
    public class DataCollectionBase : EntityBase
    {
        public decimal Evaluate { get; set; }
        public decimal NotConform { get; set; }

        public void ValidaDataCollectionBase()
        {
            Guard.ForNegative(Evaluate, "Evaluate");
            Guard.ForNegative(NotConform, "Not Conform");
        }
    }
}
