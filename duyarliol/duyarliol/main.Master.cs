using System;
using System.Web;

namespace duyarliol
{
  public partial class main : System.Web.UI.MasterPage
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            if(Session["user"] == null)
            {
                HttpCookie userCookie = Request.Cookies["duyarliol"];
                if(userCookie != null)
                {
                    userCookie.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(userCookie);
                }
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
  }
}