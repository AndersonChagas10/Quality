using System;
using System.Globalization;

namespace DTO
{
    public class DataCarrierFormulario
    {
        private DateTime _dtvalueInicio, _dtvalueFim;
        public bool hasErros;

        public DateTime _dataInicio
        {
            get
            {
                if (startDate != null)
                {
                    if(!DateTime.TryParseExact(startDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _dtvalueInicio))
                        DateTime.TryParseExact(startDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _dtvalueInicio);
                    return _dtvalueInicio;
                }
                return DateTime.Now;
            }
        }

        public DateTime _dataFim
        {
            get
            {
                if (endDate != null)
                {
                    if(!DateTime.TryParseExact(endDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _dtvalueFim))
                        DateTime.TryParseExact(endDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _dtvalueFim);
                    return _dtvalueFim;
                }
                return DateTime.Now;
            }
        }

        public string startDate { get; set; }
        public string endDate { get; set; }
        public int level01Id { get; set; }
        public int level02Id { get; set; }
        public int level03Id { get; set; }
        public int unitId { get; set; }
        public int auditorId { get; set; }
        public int shift { get; set; }
        public int period { get; set; }
    }
}
