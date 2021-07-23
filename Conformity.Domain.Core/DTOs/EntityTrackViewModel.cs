namespace Conformity.Domain.Core.DTOs
{
    public class EntityTrackViewModel
    {
        public string UserName { get; set; }
        public string UpdateDate { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string FieldName { get; set; }
    }
}
