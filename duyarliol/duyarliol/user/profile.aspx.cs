using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace kaldirirmi_polymer.user
{
    public partial class profile : System.Web.UI.Page
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