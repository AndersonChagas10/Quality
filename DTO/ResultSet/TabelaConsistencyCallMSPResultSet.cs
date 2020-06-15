using System;

namespace DTO.ResultSet
{
    public class TabelaConsistencyCallMSPResultSet
    {
        public System.DateTime Data { get; set; }
        public string _Data { get { return Data.ToShortDateString(); /*+ " " + Data.ToShortTimeString();*/ } }
        public string _Hora { get { return Data.ToShortTimeString(); } }

        public string Indicador { get; set; }
        public string Monitoramento { get; set; }
        public string Tarefa { get; set; }
        public string Lancado { get; set; }

        public Nullable<decimal> AV_Peso { get; set; }
        public Nullable<decimal> _AV_Peso { get { return AV_Peso.HasValue ? AV_Peso.Value : 0M; } }

        public Nullable<decimal> NC_Peso { get; set; }
        public int Amostra { get; set; }
        public Nullable<int> Sequencial { get; set; }
        public Nullable<int> Banda { get; set; }
        public int ResultLevel3Id { get; set; }
        public Nullable<int> HashKey { get; set; }

        public string Unidade { get; set; }
        public string Periodo { get; set; }
        public string Turno { get; set; }
        public string ValueText { get; set; }
        public string HeaderFieldList { get; set; }

        public System.DateTime AddDate { get; set; }
        public string _AddDate { get { return AddDate.ToShortDateString(); /*+ " " + Data.ToShortTimeString();*/ } }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Batch { get; set; }
        public string PorcWater { get; set; }
        public string MeatAgeAVG { get; set; }
        public string MeatAgeMin { get; set; }
        public string MeatAgeMax { get; set; }
        public string PorcMeat { get; set; }
        public string Meat { get; set; }
        public string Flats { get; set; }
        public string Eyes { get; set; }
        public string Wedges { get; set; }
        public string Temperatura_Atual { get; set; }
        public string Tipo_Graus { get; set; }
        public string TempoMedio1 { get; set; }
        public string TempoMedio2 { get; set; }
        public string TempoMaximo1 { get; set; }
        public string TempoMaximo2 { get; set; }
        public string AVG_Packing_Moisture { get; set; }
        public string Temperatura_Media { get; set; }
        public string AVG_Packing_Water_Activity { get; set; }
        public string Temperatura_Maxima_C { get; set; }
        public string Temperatura_Media_C { get; set; }
        public string Temperatura_Maxima_F { get; set; }
        public string Temperatura_Media_F { get; set; }
        public string AVG_Chips { get; set; }
        public string Espessura { get; set; }
        public string Flavor { get; set; }
        public string Odor { get; set; }
        public string Texture { get; set; }
        public string Appearance { get; set; }
        public string Reanalysis { get; set; }
        public string Av_T1 { get; set; }
        public string Av_T2 { get; set; }
        public string Espessura_T1 { get; set; }
        public string Espessura_T2 { get; set; }
        public string Kg { get; set; }
        public string Amostras { get; set; }
        public string Avaliacao { get; set; }
        public string Marination_time { get; set; }
        public string PorcC { get; set; }
    }
}
