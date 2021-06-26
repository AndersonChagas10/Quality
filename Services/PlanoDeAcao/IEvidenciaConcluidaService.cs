using DTO.PlanoDeAcao;
using System.Collections.Generic;

namespace Services.PlanoDeAcao
{
    public interface IEvidenciaConcluidaService
    {
        List<ImagemDaEvidenciaViewModel> ObterFotosEvidenciaConcluida(int id);
        void RetornarListaDeEvidenciasConcluidas(AcaoInputModel objAcao);
        void VincularEvidenciasAAcaoConcluida(AcaoInputModel objAcao, List<EvidenciaViewModel> listaInserir);
    }
}
