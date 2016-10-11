using Application.AppClass;
using Application.AppServiceClass;
using Application.Interface;
using Ninject.Modules;

namespace CrossCutting.IOC.Module
{
    public class ApplicationNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind(typeof(IBaseApp<,>)).To(typeof(AppServiceBase<,>));
            Bind<IUserApp>().To<UserApp>();
            Bind<ICorrectiveActionApp>().To<CorrectiveActionApp>();
            Bind<IRelatorioColetaApp>().To<RelatorioColetaApp>();
            Bind<ISyncApp>().To<SyncApp>();
            Bind<IParApp>().To<ParApp>();
        }
    }
}