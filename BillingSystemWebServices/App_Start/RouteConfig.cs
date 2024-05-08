// RouteConfig.cs
using System.Web.Mvc;
using System.Web.Routing;

public class RouteConfig
{
    public static void RegisterRoutes(RouteCollection routes)
    {
        routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

        // Default route for BillAccountController
        routes.MapRoute(
            name: "BillAccount",
            url: "BillAccount/{action}/{id}",
            defaults: new { controller = "BillAccount", action = "Index", id = UrlParameter.Optional }
        );

        // Default route for InstallmentController
        routes.MapRoute(
            name: "Installment",
            url: "Installment/{action}/{id}",
            defaults: new { controller = "Installment", action = "Index", id = UrlParameter.Optional }
        );
        // Default route for InstallmentController
        routes.MapRoute(
            name: "Invoice",
            url: "Invoice/{action}/{id}",
            defaults: new { controller = "Invoice", action = "Index", id = UrlParameter.Optional }
        );
        // Default route for InstallmentController
        routes.MapRoute(
            name: "Payment",
            url: "Payment/{action}/{id}",
            defaults: new { controller = "Payment", action = "Index", id = UrlParameter.Optional }
        );

        // Other routes...

        // Default route (if none of the above routes match)
        routes.MapRoute(
            name: "Default",
            url: "{controller}/{action}/{id}",
            defaults: new { controller = "BillAccount", action = "Index", id = UrlParameter.Optional }
        );
    }
}
