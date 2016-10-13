using CommonServiceLocator.NinjectAdapter.Unofficial;
using CrossCutting.IOC.Module;
using Microsoft.Practices.ServiceLocation;
using Ninject;

namespace CrossCutting.IOC
{
    public class IoC
    {
        public IKernel Kernel { get; private set; }

        public IoC()
        {
            Kernel = GetNinjectModules();
            ServiceLocator.SetLocatorProvider(() => new NinjectServiceLocator(Kernel));
            //GlobalConfiguration.Configuration.DependencyResolver = new Ninject.WebApi.DependencyResolver.NinjectDependencyResolver(kernel);
        }

        public StandardKernel GetNinjectModules()
        {
            return new StandardKernel(
                new ApplicationNinjectModule(),
                new DomainNinjectModule(),
                new DataNinjectModule()
                );
        }
    }
}