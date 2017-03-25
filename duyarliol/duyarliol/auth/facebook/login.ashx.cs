using System.Web;

namespace duyarliol.auth.facebook
{
    public class login : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        public string WEBSITE_NAME = "http://duyarliol1.azurewebsites.net/";
        public string CLIENT_ID = "1776520979342984";
        public void ProcessRequest(HttpContext context)
        {
            context.Session["auth-current-user"] = context.Session["user"] != null ? ((core.user)context.Session["user"]).id : 0;
            context.Session["auth-redirect"] = context.Request.UrlReferrer.PathAndQuery;
            context.Response.Redirect(
                string.Format("https://www.facebook.com/dialog/oauth?client_id={0}&redirect_uri={1}&scope=public_profile,email", CLIENT_ID, context.Request.IsLocal ? "http://localhost:56920/auth/facebook/gateway" : WEBSITE_NAME+"auth/facebook/gateway"));
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}