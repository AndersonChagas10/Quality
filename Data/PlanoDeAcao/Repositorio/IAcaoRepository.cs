using Dominio;
using DTO;
using DTO.PlanoDeAcao;
using System.Collections.Generic;

namespace Data.PlanoDeAcao.Repositorio
{
    public interface IAcaoRepository
    {
        IEnumerable<AcaoViewModel> ObterAcao(DataCarrierFormularioNew form, UserSgq usuarioLogado);
        IEnumerable<AcaoViewModel> ObterStatusPorId(string status);
        AcaoFormViewModel ObterAcaoComVinculosPorId(int id, UserSgq usuarioLogado);
        Acao ObterAcaoPorId(int id);
        void VincularUsuariosASeremNotificadosAAcao(AcaoInputModel objAcao, List<int> listaInserir);
        void InativarUsuariosASeremNotificadosAAcao(AcaoInputModel objAcao, List<int> listaDeletar);
        List<AcaoXNotificarAcao> RetornarUsuariosASeremNotificadosDaAcao(AcaoInputModel objAcao);
        void AtualizarValoresDaAcao(AcaoInputModel objAcao);
        int SalvarAcao(Acao item);  
    }
}
