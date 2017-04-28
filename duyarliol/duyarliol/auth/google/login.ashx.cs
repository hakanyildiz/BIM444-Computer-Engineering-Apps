using System.Web;

namespace duyarliol.auth.google
{
    public class login : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Session["auth-current-user"] = context.Session["user"] != null ? ((core.user)context.Session["user"]).id : 0;
            context.Session["auth-redirect"] = context.Request.UrlReferrer.PathAndQuery;
            context.Response.Redirect(string.Format("https://accounts.google.com/o/oauth2/v2/auth?client_id=1025271688593-hjuu8h3rjk1jsqs26vl043hha8shabpk.apps.googleusercontent.com&response_type=code&scope=openid%20email&redirect_uri={1}&state=security_token%3D{0}%26url%3D{1}", new core.data().createrandomstring(10), "http://duyarliol.azurewebsites.net/auth/google/gateway"));
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