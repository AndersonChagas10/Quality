using Dominio;
using DTO.PlanoDeAcao;

namespace Services.PlanoDeAcao.Interfaces
{
    public interface IAcompanhamentoAcaoService
    {
        AcaoViewModel SalvarAcompanhamentoComNotificaveis(int id, AcompanhamentoAcaoInputModel objAcompanhamentoAcao, UserSgq usuarioLogado);
    }
}
