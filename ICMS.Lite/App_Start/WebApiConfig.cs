using DryIoc;
using DryIoc.WebApi;
using ICMS.Lite.App_Start;
using ICMS.Lite.Business.Services;
using ICMS.Lite.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ICMS.Lite
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var corsAttr = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(corsAttr);

            config.SuppressDefaultHostAuthentication();
            //config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            //config.RegisterDependencyResolver();

            var c = new Container()
                .WithWebApi(config, throwIfUnresolved: type => type.IsController());

            c.Register(typeof(IIndentsRepository), typeof(IndentsRepository), Reuse.Singleton);

            c.Register(typeof(IIndentService), typeof(IndentService), Reuse.Singleton);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }

        //private static void RegisterDependencyResolver(this HttpConfiguration config)
        //{
        //    try
        //    {
        //        IContainer container = new Container(rules => rules.WithoutThrowOnRegisteringDisposableTransient())
        //            .WithWebApi(config);

        //        container.RegisterDependencies();
        //    }
        //    catch (Exception ex)
        //    {
        //        var message = ex.Message.ToString();
        //    }

        //}
    }
}
