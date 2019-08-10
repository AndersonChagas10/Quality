using DTO.DTO;
using SgqService.ViewModels;

namespace SgqSystem.ViewModels
{
    public class ReportsCorrectiveActionViewModel
    {
        public FormularioParaRelatorioViewModel formularioParaRelatorioViewModel { get; set; }
        public CorrectiveActionDTO correctiveActionDTO { get; set; }
    }
}