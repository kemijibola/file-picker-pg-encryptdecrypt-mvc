using DryIoc;
using DryIoc.Mvc;
//using DryIoc.WebApi;
using ICMS.Lite.App_Start;
using ICMS.Lite.Business.Services;
using ICMS.Lite.IcmsConfig;
using ICMS.Lite.Repository.Data;
using ICMS.Lite.Repository.Repositories;
using ICMS.Lite.Repository.ViewModels;
using Microsoft.Owin;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Owin;
using System;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using static ICMS.Lite.Repository.Utilities.General;
using static ICMS.Lite.Repository.ViewModels.AccountViewModel;

[assembly: OwinStartupAttribute(typeof(ICMS.Lite.Startup))]
namespace ICMS.Lite
{
    public partial class Startup
    {
        private static readonly Assembly CurrentAssembly = typeof(ICMS.Lite.MvcApplication).Assembly;
        private static readonly Type ApiControllerType = typeof(ApiController);
        public void Configuration(IAppBuilder app)
        {
            try
            {
                var config = new HttpConfiguration();

                IContainer container = new Container(rules => rules.WithTrackingDisposableTransients());

                RegisterServices(container);

                container = container.WithMvc();

                ConfigureAuth(app);
            }
            catch(Exception ex)
            {
                throw ex;
            }

        }

        public static void RegisterServices(IRegistrator registrator)
        {

            registrator.Register<CurrentUserConfig>(Reuse.Singleton);


            registrator.RegisterDelegate<UserViewModel>(r =>
                    r.Resolve<CurrentUserConfig>().GetCurrentUser(),
                Reuse.Singleton);

            //registrator.RegisterDelegate<UserViewModel>(r =>
            //{
            //    return r.Resolve<CurrentUserConfig>().GetCurrentUser();

            //},Reuse.Singleton);

            registrator.Register(typeof(ICoreRepository), typeof(CoreRepository), Reuse.Singleton);
            registrator.Register(typeof(IAccountRepository), typeof(AccountRepository), Reuse.Singleton);
            registrator.Register(typeof(IIndentsRepository), typeof(IndentsRepository), Reuse.Singleton);
            registrator.Register(typeof(IMFBRepository), typeof(MFBRepository), Reuse.Singleton);
            registrator.Register(typeof(IReportRepository), typeof(ReportRepository), Reuse.Singleton);

            registrator.Register(typeof(IAccountService), typeof(AccountService), Reuse.Singleton);
            registrator.Register(typeof(ICoreService), typeof(CoreService), Reuse.Singleton);
            registrator.Register(typeof(IIndentService), typeof(IndentService), Reuse.Singleton);
            registrator.Register(typeof(IMFBService), typeof(MFBService), Reuse.Singleton);
            registrator.Register(typeof(IReportService), typeof(ReportService), Reuse.Singleton);


        }
    }
}
