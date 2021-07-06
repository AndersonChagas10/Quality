using DTO.PlanoDeAcao;
using System.Collections.Generic;

namespace Data.PlanoDeAcao.Interfaces
{
    public interface IEvidenciaConcluidaRepository
    {
        List<EvidenciaViewModel> BuscarListaEvidenciasConcluidas(int acao_Id);
        void InativarEvidenciasDaAcaoConcluida(List<EvidenciaViewModel> listaInativar);
    }
}
