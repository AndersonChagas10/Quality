[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(SgqSystem.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(SgqSystem.App_Start.NinjectWebCommon), "Stop")]

namespace SgqSystem.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    //using Data.Repositories;
    using System.Web.Http;
    using CrossCutting.IOC;

    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var ioc = new IoC();
            //var kernel = new StandardKernel();
            var kernel = ioc.Kernel;
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                // Install our Ninject-based IDependencyResolver into the Web API config
                GlobalConfiguration.Configuration.DependencyResolver = new Ninject.WebApi.DependencyResolver.NinjectDependencyResolver(kernel);
                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            //kernel.Bind(typeof(IAppServiceBase<>)).To(typeof(AppServiceBase<>));
            //kernel.Bind<IUserAppService>().To<UserAppService>();

            //kernel.Bind(typeof(IServiceBase<>)).To(typeof(ServiceBase<>));
            //kernel.Bind<IUserService>().To<UserService>();

            //kernel.Bind(typeof(IRepositoryBase<>)).To(typeof(RepositoryBase<>));
            //kernel.Bind<IUserRepository>().To<UserRepository>();
        }
    }
}