using DTO.BaseEntity;
using System;
using System.Globalization;

namespace DTO.DTO
{
    public class CollectionHtmlDTO : EntityBase
    {
        public string Html { get; set; }
        public int Period { get; set; }
        public int Shift { get; set; }
        public Nullable<System.DateTime> CollectionDate { get; set; }


        public string CollectionDateFormatado
        {
            get { return CollectionDate.Value != null ? CollectionDate.Value.ToString("MM/dd/yyyy") : null; }
            set
            {
                CollectionDate = Convert.ToDateTime(value.ToString(), CultureInfo.InvariantCulture);
            }
        }

    }
}
