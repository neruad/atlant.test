using Microsoft.Web.Http.Routing;
using System.Web.Http;
using System.Web.Http.Routing;

namespace BitcoinTransactions.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var constraintResolver = new DefaultInlineConstraintResolver
            {
                ConstraintMap =
                {
                    ["apiVersion"] = typeof(ApiVersionRouteConstraint)
                }
            };
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "VersionedApi",
                routeTemplate: "api/v{version:apiVersion}/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            
            config.Formatters.Remove(config.Formatters.XmlFormatter);
        }
    }
}
