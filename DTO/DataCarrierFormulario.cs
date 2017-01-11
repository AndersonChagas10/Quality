using DTO.Helpers;
using System;

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
                    Guard.ParseDateToSql(startDate, ref _dtvalueInicio);
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
                    Guard.ParseDateToSql(endDate, ref _dtvalueFim);
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
