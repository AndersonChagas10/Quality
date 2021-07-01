using DTO.PlanoDeAcao;
using System.Collections.Generic;

namespace Data.PlanoDeAcao.Interfaces
{
    public interface IEvidenciaNaoConformeRepository
    {
        List<EvidenciaViewModel> BuscarListaEvidencias(int acao_Id);
        List<EvidenciaViewModel> RetornarEvidenciasDaAcao(int acao_Id);
        void InativarEvidencias(List<EvidenciaViewModel> listaInativar);
    }
}
