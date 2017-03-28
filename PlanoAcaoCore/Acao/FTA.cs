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

        public int MetaFTA { get; set; }

        public int PercentualNCFTA { get; set; }

        public int ReincidenciaDesvioFTA { get; set; }

        public int Assinatura1 { get; set; }

        public int Assinatura2 { get; set; }
        
    }
}
