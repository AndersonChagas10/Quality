using System;
using System.Globalization;

namespace DTO
{
    public class DataCarrierFormulario
    {
        private DateTime _dtvalueInicio, _dtvalueFim;

        public DateTime _dataInicio
        {
            get
            {
                if (dataInicio != null)
                {
                    _dtvalueInicio = DateTime.ParseExact(dataInicio, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    return _dtvalueInicio;
                }
                return DateTime.Now;
            }
        }

        public DateTime _dataFim
        {
            get
            {
                if (dataFim != null)
                {
                    _dtvalueFim = DateTime.ParseExact(dataFim, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    return _dtvalueFim;
                }
                return DateTime.Now;
            }
        }

        public string dataInicio { get; set; }
        public string dataFim { get; set; }
        public int level01id { get; set; }
        public int level02id { get; set; }
        public int level03id { get; set; }
        public int unidadeId { get; set; }
        public int userId { get; set; }
    }
}
