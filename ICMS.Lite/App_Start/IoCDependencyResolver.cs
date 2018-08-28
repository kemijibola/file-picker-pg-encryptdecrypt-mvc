using DryIoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;

namespace ICMS.Lite.App_Start
{
    public static class IoCDependencyResolver
    {
        private static readonly Assembly CurrentAssembly = typeof(ICMS.Lite.MvcApplication).Assembly;
        private static readonly Type ApiControllerType = typeof(ApiController);

        //public static void RegisterDependencies(this IContainer container)
        //{
        //    container.RegisterControllers();
        //}

        public static void RegisterControllers(IContainer container)
        {
            foreach (Type controller in CurrentAssembly.GetTypes().Where(t => !t.IsAbstract && t.IsClass && ApiControllerType.IsAssignableFrom(t)))
            {
                container.Register(controller, Reuse.InResolutionScope);
            }
        }


    }
}