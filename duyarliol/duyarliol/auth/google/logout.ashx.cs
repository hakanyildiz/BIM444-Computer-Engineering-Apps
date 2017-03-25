using System;
using System.Collections.Generic;
using System.Web;

namespace duyarliol.auth.google
{
  public class logout : IHttpHandler, System.Web.SessionState.IRequiresSessionState
  {

    public void ProcessRequest(HttpContext context)
    {
        if (context.Session["user"] != null)
        {
          core.data data = new core.data();

          int authcount = Convert.ToInt32(data.getsinglecolumndb("auths", "count(*)", new List<core.db>() {
            new core.db() { column = "userid", value = ((core.user)context.Session["user"]).id }
          }) ?? 0);
          if (authcount > 0)
          {
            if (authcount == 1)
            {
              context.Session["site-message"] = "Hesabına en az 1 sosyal medya hesabı bağlı olmalıdır.";
              context.Response.Redirect("/uye/profil#auths");
            }

            if (!data.removedb("auths", new List<core.db>() {
              new core.db() { column = "userid", value = ((core.user)context.Session["user"]).id },
              new core.db() { column = "authtype", value = 1 }
            }))
            {
              context.Session["site-message"] = "Bağlantı hatası.";
              context.Response.Redirect("/uye/profil#auths");
            }

            context.Session["site-message"] = "Google hesabın kaldırıldı.";
            context.Response.Redirect("/uye/profil#auths");
          }
          else
          {
            context.Session["site-message"] = "Bağlantı hatası.";
            context.Response.Redirect("/uye/profil#auths");
          }
        }
        else context.Response.Redirect("/");
    
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