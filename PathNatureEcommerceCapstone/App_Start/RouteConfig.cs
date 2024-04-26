using System.Web.Mvc;
using System.Web.Routing;

namespace PathNatureEcommerceCapstone
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            /*** Custom Route ***/
            routes.MapRoute(
                name: "Product",
                url: "product/{id}",
                defaults: new { controller = "Product", action = "Index", id = UrlParameter.Optional }
                );

            /*** Custom Route ***/
            routes.MapRoute(
                name: "IncreaseQty",
                url: "Home/IncreaseQty",
                defaults: new { controller = "Home", action = "IncreaseQty" }
                );

            /*** Custom Route ***/
            routes.MapRoute(
                name: "DecreaseQty",
                url: "Home/DecreaseQty",
                defaults: new { controller = "Home", action = "DecreaseQty" }
                );

            /*** Custom Route ***/
            routes.MapRoute(
                name: "SignUp",
                url: "Account/SignUp",
                defaults: new { controller = "SignUp", action = "SignUp" }
                );

            /*** Custom Route ***/

            routes.MapRoute(
                name: "UserDashboard",
                url: "User/Dashboard",
                defaults: new { controller = "LayoutPageUserController", action = "Dashboard" }
                                                                            );

            // Route per il logout degli utenti
            routes.MapRoute(
                name: "UserLogout",
                url: "User/Logout",
                defaults: new { controller = "Account", action = "UserLogout" }
                                                                        );
            // Route per la pagina di checkout
            routes.MapRoute(
                name: "CheckoutDetails",
                url: "Home/CheckoutDetails/{orderId}",
                defaults: new { controller = "Home", action = "GetCheckoutDetails", orderId = UrlParameter.Optional }
                );

            // Route per il login degli utenti
            routes.MapRoute(
                name: "UserLogin",
                url: "User/Login",
                defaults: new { controller = "Account", action = "UserLogin" }
            );

            routes.MapRoute(
    name: "Checkout",
    url: "Home/Checkout",
    defaults: new { controller = "Home", action = "Checkout" }
);


            routes.MapRoute(
                name: "AdminDashboard",
                url: "Admin/Dashboard",
                defaults: new { controller = "LayoutPageAdminController", action = "Dashboard" }
);


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
