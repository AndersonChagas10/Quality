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
    using System.Web.Http.Validation;
    using Ninject.Web.WebApi.Validation;

    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        public static IKernel Kernel { get; set; }

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
            Kernel = ioc.Kernel;
            try
            {
                Kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                Kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
                Kernel.Rebind<ModelValidatorProvider>().To<NinjectDefaultModelValidatorProvider>();

                // Install our Ninject-based IDependencyResolver into the Web API config
                GlobalConfiguration.Configuration.DependencyResolver = new Ninject.WebApi.DependencyResolver.NinjectDependencyResolver(Kernel);
                RegisterServices(Kernel);
                return Kernel;
            }
            catch
            {
                Kernel.Dispose();
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

            //kernel.Bind(typeof(IBaseRepository<>)).To(typeof(RepositoryBase<>));
            //kernel.Bind<IUserRepository>().To<UserRepository>();
        }
    }
}