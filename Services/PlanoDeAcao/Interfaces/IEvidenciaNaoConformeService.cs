using Dominio;
using DTO.PlanoDeAcao;
using System.Collections.Generic;

namespace Services.PlanoDeAcao.Interfaces
{
    public interface IEvidenciaNaoConformeService
    {
        List<ImagemDaEvidenciaViewModel> ObterFotosEvidencia(int id);
        void RetornarListaDeEvidencias(AcaoInputModel objAcao);
        void VincularEvidenciasAAcao(AcaoInputModel objAcao, List<EvidenciaViewModel> listaInserir);
    }
}
