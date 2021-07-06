using Data.PlanoDeAcao;
using Data.PlanoDeAcao.Interfaces;
using Data.PlanoDeAcao.Repositorio;
using DTO.Interfaces.Services;
using DTO.Services;
using Ninject.Modules;
using Services.PlanoDeAcao;
using Services.PlanoDeAcao.Interfaces;

namespace CrossCutting.IOC.Module
{
    public class AcaoRHNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IAcaoRepository>().To<AcaoRepository>();
            Bind<IAcaoService>().To<AcaoService>();
            Bind<IAcompanhamentoAcaoRepository>().To<AcompanhamentoAcaoRepository>();
            Bind<IAcompanhamentoAcaoService>().To<AcompanhamentoAcaoService>();
            Bind<IEvidenciaNaoConformeRepository>().To<EvidenciaNaoConformeRepository>();
            Bind<IEvidenciaNaoConformeService>().To<EvidenciaNaoConformeService>();
            Bind<IEvidenciaConcluidaRepository>().To<EvidenciaConcluidaRepository>();
            Bind<IEvidenciaConcluidaService>().To<EvidenciaConcluidaService>();
        }
    }
}
