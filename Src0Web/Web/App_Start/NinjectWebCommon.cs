[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Web.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(Web.App_Start.NinjectWebCommon), "Stop")]

namespace Web.App_Start
{
    using System;
    using System.Net.Http;
    using System.Web;
    using Cdn;
    using Data.BLL;
    using Data.Services;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using Ninject.Web.Common.WebHost;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();
        public static IKernel Kernel;

        /// <summary>
        /// Starts the application.
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
            var kernel = new StandardKernel();
            Kernel = kernel;
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
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
            kernel.Bind<GeneralDao>().To<GeneralDao>();

            kernel.Bind<AppSettingService>().To<AppSettingService>().InRequestScope();
            kernel.Bind<FilmMetadataService>().To<FilmMetadataService>().InRequestScope();
            kernel.Bind<PeopleService>().To<PeopleService>().InRequestScope();
            kernel.Bind<TaxonomyService>().To<TaxonomyService>().InRequestScope();
            kernel.Bind<FilmService>().To<FilmService>().InRequestScope();
            kernel.Bind<RoleService>().To<RoleService>().InRequestScope();
            kernel.Bind<UserService>().To<UserService>().InRequestScope();
        }
    }
}