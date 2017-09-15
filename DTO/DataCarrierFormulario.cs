using DTO.Helpers;
using System;
using System.Collections.Generic;

namespace DTO
{
    public class DataCarrierFormulario
    {
        //private DateTime _dtvalueInicio, _dtvalueFim;
        public bool hasErros;
      
        public DateTime _dataInicio
        {
            get
            {
                if (startDate != null)
                {
                    //Guard.ParseDateToSql(startDate, ref _dtvalueInicio);
                    //return _dtvalueInicio;
                    return Guard.ParseDateToSqlV2(startDate);
                    
                }
                return DateTime.Now;
            }
            
        }

        public String _dataInicioSQL
        {
            get
            {
                if (startDate != null)
                {
                    return _dataInicio.ToString("yyyyMMdd");
                }
                return DateTime.Now.ToString("yyyyMMdd");
            }
            
        }

        public DateTime _dataFim
        {
            get
            {
                if (endDate != null)
                {
                    //Guard.ParseDateToSql(endDate, ref _dtvalueFim);
                    //return _dtvalueFim;
                    return Guard.ParseDateToSqlV2(endDate);
                }
                return DateTime.Now;
            }
        }

        public String _dataFimSQL
        {
            get
            {
                if (endDate != null)
                {
                    return _dataFim.ToString("yyyyMMdd");
                }
                return DateTime.Now.ToString("yyyyMMdd");
            }
        }

        public string startDate { get; set; }
        public string endDate { get; set; }

        public int level1Id { get; set; }
        public string level1Name { get; set; }

        public int level2Id { get; set; }
        public string level2Name { get; set; }

        public int level3Id { get; set; }
        public string level3Name { get; set; }

        public int unitId { get; set; }
        public string unitName { get; set; }

        public int auditorId { get; set; }
        public string auditorName { get; set; }

        public int shift { get; set; }
        public int period { get; set; }


        public int MetaFTA { get; set; }
        public int PercentualNCFTA { get; set; }
        public int ReincidenciaDesvioFTA { get; set; }

        public string CallBackTableBody { get; set; }
        public string CallBackTableEsquerda { get; set; }
        public string CallBackTableTituloColunas { get; set; }
        public string CallBackTableX { get; set; }
        public string Query { get; set; }
        public string Title { get; set; }
        public List<string> ParametroTableRow { get; set; }
        public List<string> ParametroTableCol { get; set; }

        public int clusterSelected_Id { get; set; }

    }
}
