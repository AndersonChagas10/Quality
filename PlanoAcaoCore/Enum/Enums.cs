using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanoAcaoCore.Enum
{
    public class Enums
    {
        public enum UnidadeMedida
        {
            Monetario,
            Absoluto
        }

        public enum Status
        {
            Atrasado = 1, Cancelado, Concluido, ConcluidoComAtraso, EmAndamento, Retorno, Finalizada, FinalizadaComAtraso
        };
    }
}
