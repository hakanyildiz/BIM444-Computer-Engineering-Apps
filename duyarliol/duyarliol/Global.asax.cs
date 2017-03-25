using System;
using System.Web;
using System.Web.Routing;

namespace duyarliol
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            RegisterRoutes(RouteTable.Routes);

        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.Clear();
            routes.RouteExistingFiles = false;
            routes.Ignore("{resource}.axd/{*pathInfo}");

            routes.MapPageRoute("main", "", "~/default.aspx");
            routes.MapPageRoute("registration", "registration", "~/registration.aspx");


            routes.MapPageRoute("search", "ara", "~/search.aspx");
            routes.MapPageRoute("games-view", "oyun/{url}", "~/game.aspx");
            routes.MapPageRoute("authenticate", "authenticate", "~/authenticate.aspx");
            routes.MapPageRoute("playground", "playground", "~/app-box-test.aspx");

            /*yeni sayfa Games*/
            routes.MapPageRoute("oyunlar", "oyunlar", "~/games.aspx");
            /*sistem tanımlama için geliştirilen ve kurulum anlatımı içeren sihirbaz sayfası */
            routes.MapPageRoute("Sihirbaz", "sistem-sihirbazi", "~/SystemWizard.aspx");

            /* haberler sayfası*/
            routes.MapPageRoute("haberler", "haberler", "~/newscast.aspx");
            /* haber detay */
            routes.MapPageRoute("news-view", "haber/{url}", "~/news.aspx");



            routes.MapPageRoute("user-public-page", "u/{username}", "~/user-public-page.aspx");
            routes.MapPageRoute("user-profile", "uye/profil", "~/user/profile.aspx");
            routes.MapPageRoute("user-add-system", "uye/sistemini-tanimla", "~/user/add-system.aspx");
            routes.MapPageRoute("user-logout", "uye/cikis", "~/user/logout.aspx");

            routes.Add(new Route("h/main", new HttpHandlerRoute("~/handlers/main.ashx")));

            routes.Add(new Route("auth/google/login", new HttpHandlerRoute("~/auth/google/login.ashx")));
            routes.Add(new Route("auth/google/gateway", new HttpHandlerRoute("~/auth/google/gateway.ashx")));
            routes.Add(new Route("auth/google/confirm", new HttpHandlerRoute("~/auth/google/confirm.ashx")));
            routes.Add(new Route("auth/google/logout", new HttpHandlerRoute("~/auth/google/logout.ashx")));
           
            routes.Add(new Route("auth/facebook/login", new HttpHandlerRoute("~/auth/facebook/login.ashx")));
            routes.Add(new Route("auth/facebook/gateway", new HttpHandlerRoute("~/auth/facebook/gateway.ashx")));
            routes.Add(new Route("auth/facebook/confirm", new HttpHandlerRoute("~/auth/facebook/confirm.ashx")));
            routes.Add(new Route("auth/facebook/logout", new HttpHandlerRoute("~/auth/facebook/logout.ashx")));
        }
    }

    public class HttpHandlerRoute : IRouteHandler
    {
        private string _VirtualPath = null;
        public HttpHandlerRoute(string virtualPath)
        {
            _VirtualPath = virtualPath;
        }
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            IHttpHandler httpHandler = (IHttpHandler)System.Web.Compilation.BuildManager.CreateInstanceFromVirtualPath(_VirtualPath, typeof(IHttpHandler));
            return httpHandler;
        }
    }


}