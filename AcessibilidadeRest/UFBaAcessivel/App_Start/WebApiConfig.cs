using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace UFBaAcessivel
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { action = "estabelecimento", id = RouteParameter.Optional },
                constraints: new { id = @"\d+" });

            config.Routes.MapHttpRoute(
                name: "estabelecimentoVoz",
                routeTemplate: "api/{controller}/{action}/{estabelecimentoVoz}",
                defaults: new { action = "estabelecimentoVoz", estabelecimentoVoz = RouteParameter.Optional }
                );

            config.Routes.MapHttpRoute(
                name: "vozApi",
                routeTemplate: "api/{controller}/{voz}",
                defaults: new { action = "voz", voz = RouteParameter.Optional }
                );

            config.Routes.MapHttpRoute(
                name: "caminhoApi",
                routeTemplate: "api/{controller}/{estabelecimento}/{tag}/{tag2}",
                defaults: new { action = "caminho", estabelecimento = RouteParameter.Optional, tag = RouteParameter.Optional, tag2 = RouteParameter.Optional }
                );

            config.Routes.MapHttpRoute(
                name: "CustomActionApi",
                routeTemplate: "api/{controller}/{action}");
        }
    }
}
