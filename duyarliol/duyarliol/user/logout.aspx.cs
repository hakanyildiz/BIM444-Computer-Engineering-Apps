using System;
using System.Web;

namespace kaldirirmi_polymer.user
{
    public partial class logout : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            Session["user"] = null;
            HttpCookie cookie = Request.Cookies["duyarliol"];
            if (cookie != null)
            {
                HttpCookie userCookie = Request.Cookies["duyarliol"];
                userCookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(userCookie);
            }
            Response.Redirect(Request.UrlReferrer != null ? Request.UrlReferrer.PathAndQuery : "/");
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}