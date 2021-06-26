using Dominio;
using DTO;
using DTO.PlanoDeAcao;
using System.Collections.Generic;

namespace Data.PlanoDeAcao.Repositorio
{
    public interface IAcaoRepository
    {
        IEnumerable<AcaoViewModel> ObterAcaoPorFiltro(DataCarrierFormularioNew form);
        IEnumerable<AcaoViewModel> ObterStatusPorId(string status);
        AcaoFormViewModel ObterAcaoComVinculosPorId(int id);
        Acao ObterAcaoPorId(int id);
        List<EvidenciaViewModel> BuscarListaEvidenciasConcluidas(int acao_Id);
        void VincularUsuariosASeremNotificadosAAcao(AcaoInputModel objAcao, List<int> listaInserir);
        void InativarUsuariosASeremNotificadosAAcao(AcaoInputModel objAcao, List<int> listaDeletar);
        List<AcaoXNotificarAcao> RetornarUsuariosASeremNotificadosDaAcao(AcaoInputModel objAcao);
        void AtualizarValoresDaAcao(AcaoInputModel objAcao);
        void InativarEvidenciasDaAcaoConcluida(List<EvidenciaViewModel> listaInativar);
    }
}
