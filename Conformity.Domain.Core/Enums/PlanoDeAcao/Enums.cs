﻿namespace Conformity.Domain.Core.Enums.PlanoDeAcao
{
    public class Enums
    {
        public enum EAcaoStatus
        {
            Pendente = 1,
            Em_Andamento,
            Concluída,
            Atrasada,
            Cancelada
        }

        public enum EAcaoPrioridade
        {
            Baixa = 1,
            Media,
            Alta
        }
    }
}
