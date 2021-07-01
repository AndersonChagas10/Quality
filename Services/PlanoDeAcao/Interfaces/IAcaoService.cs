using DTO.PlanoDeAcao;

namespace Services.PlanoDeAcao.Interfaces
{
    public interface IAcaoService
    {
        void EnviarEmail(AcaoInputModel acao);
        void AtualizarUsuarios(AcaoInputModel objAcao);
    }
}
