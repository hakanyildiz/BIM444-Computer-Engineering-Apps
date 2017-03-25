using System;

namespace kaldirirmi_polymer.user
{
  public partial class add_system : System.Web.UI.Page
  {
    protected void Page_Init(object sender, EventArgs e)
    {
      if (Session["user"] == null) Response.Redirect("/");
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }
  }
}