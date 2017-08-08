using System;

namespace PlanoAcaoCore.Acao
{
    public class FTA : Pa_Acao
    {

        public void ValidaFTA()
        {

        }

        public DateTime DataInicioFTA { get; set; }
        public string _DataInicioFTA { get; set; }

        public DateTime DataFimFTA { get; set; }
        public string _DataFimFTA { get; set; }

        public string _Supervisor { get; set; }
        public int Supervisor_Id { get; set; }

        public string _Departamento { get; set; }

        public string MetaFTA { get; set; }

        public string PercentualNCFTA { get; set; }

        public string ReincidenciaDesvioFTA { get; set; }

        public int Assinatura1 { get; set; }

        public int Assinatura2 { get; set; }

        public bool IsFTA { get; set; }
        
    }
}
