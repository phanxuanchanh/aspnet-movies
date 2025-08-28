[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Web.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(Web.App_Start.NinjectWebCommon), "Stop")]

namespace Web.App_Start
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json;
    using System.Web;
    using Data.Context;
    using Data.DAOs;
    using Data.Mapping;
    using Data.Models;
    using Data.Services;
    using MediaSrv;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using MSSQL.Access;
    using Ninject;
    using Ninject.Web.Common;
    using Ninject.Web.Common.WebHost;
    using Web.Shared.Mapper;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();
        public static IKernel Kernel => bootstrapper.Kernel;

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
            StandardKernel kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                kernel.Components.Add<INinjectHttpApplicationPlugin, NinjectWebFormsHttpApplicationPlugin>();

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
            kernel.Bind<IMapper>().ToMethod(m =>
            {
                Mapper mapper = new Mapper();
                mapper.AddProfile(new MappingProfile());

                return mapper;
            }).InSingletonScope();

            kernel.Bind<DBContextPool>()
                .ToMethod(m => new DBContextPool()).InSingletonScope();
            kernel.Bind<DBContextPoolHandle>().ToSelf().InRequestScope();

            kernel.Bind<DBContext>().ToMethod(m =>
            {
                DBContextPoolHandle handle = m.Kernel.Get<DBContextPoolHandle>();
                return handle.Context;
            }).InTransientScope();

            kernel.Bind<AppSettingDao>().ToSelf().InRequestScope();
            kernel.Bind<TaxonomyDao>().ToSelf().InRequestScope();
            kernel.Bind<TaxonomyLinkDao>().ToSelf().InRequestScope();
            kernel.Bind<FilmMetadataDao>().ToSelf().InRequestScope();
            kernel.Bind<FilmMetaLinkDao>().ToSelf().InRequestScope();
            kernel.Bind<PeopleDao>().ToSelf().InRequestScope();
            kernel.Bind<PeopleLinkDao>().ToSelf().InRequestScope();
            kernel.Bind<FilmDao>().ToSelf().InRequestScope();
            kernel.Bind<UserDao>().ToSelf().InRequestScope();
            kernel.Bind<RoleDao>().ToSelf().InSingletonScope();

            kernel.Bind<AppSettingService>().ToSelf().InRequestScope();
            kernel.Bind<FilmMetadataService>().ToSelf().InRequestScope();
            kernel.Bind<PeopleService>().ToSelf().InRequestScope();
            kernel.Bind<TaxonomyService>().ToSelf().InRequestScope();
            kernel.Bind<FilmService>().ToSelf().InRequestScope();
            kernel.Bind<RoleService>().ToSelf().InRequestScope();
            kernel.Bind<UserService>().ToSelf().InRequestScope();

            kernel.Bind<MediaServiceWrapper>()
                .ToMethod(m =>
                {
                    AppSetting appSetting = AppSettingValues.Get("cdn-server");

                    Dictionary<string, string> mediaServerSetting = JsonSerializer
                        .Deserialize<Dictionary<string, string>>(appSetting.Value);

                    return MediaServiceWrapper.Init(mediaServerSetting["CdnHost"], mediaServerSetting["ClientId"], mediaServerSetting["SecretKey"]);
                })
                .InRequestScope();
        }
    }
}