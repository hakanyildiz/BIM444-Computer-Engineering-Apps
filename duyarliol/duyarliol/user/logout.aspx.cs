using System;

namespace kaldirirmi_polymer.user
{
  public partial class logout : System.Web.UI.Page
  {
    protected void Page_Init(object sender, EventArgs e)
    {
      Session["user"] = null;
      Response.Redirect(Request.UrlReferrer != null ? Request.UrlReferrer.PathAndQuery : "/");
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }
  }
}