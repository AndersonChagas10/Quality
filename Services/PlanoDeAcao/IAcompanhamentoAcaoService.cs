using Dominio;
using DTO.PlanoDeAcao;

namespace Services.PlanoDeAcao
{
    public interface IAcompanhamentoAcaoService
    {
        AcaoViewModel SalvarAcompanhamentoComNotificaveis(int id, AcompanhamentoAcaoInputModel objAcompanhamentoAcao, UserSgq usuarioLogado);
    }
}
