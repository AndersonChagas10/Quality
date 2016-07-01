using Application;
using Application.Interface;
using Ninject.Modules;

namespace CrossCutting.IOC.Module
{
    public class ApplicationNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind(typeof(IAppServiceBase<>)).To(typeof(AppServiceBase<>));
            Bind<IUserAppService>().To<UserAppService>();
            Bind<IResultOldAppService>().To<ResultOldAppService>();
        }
    }
}