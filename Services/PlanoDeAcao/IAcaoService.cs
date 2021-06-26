using DTO.PlanoDeAcao;

namespace Services.PlanoDeAcao
{
    public interface IAcaoService
    {
        void EnviarEmail(AcaoInputModel acao);
        void AtualizarUsuarios(AcaoInputModel objAcao);
    }
}
