using Dominio;
using DTO;
using DTO.PlanoDeAcao;
using System.Collections.Generic;

namespace Services.PlanoDeAcao.Interfaces
{
    public interface IAcaoService
    {
        IEnumerable<AcaoViewModel> ObterAcaoPorFiltro(DataCarrierFormularioNew form, UserSgq usuarioLogado);
        void EnviarEmail(AcaoInputModel acao);
        void AtualizarUsuarios(AcaoInputModel objAcao);
    }
}
