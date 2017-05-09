using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace duyarliol
{
    public partial class firstinit : System.Web.UI.Page
    {
        public int uid = 0;
        protected void Page_Init(object sender, EventArgs e)
        {

            if (Session["user"] != null)
            {
                uid = ((core.user)Session["user"]).id;
            }
            else
            {
                Response.Redirect("/");
            }

            core.data data = new core.data();
            int count = Convert.ToInt32(data.getsinglecolumndbcount("userincome", new List<core.db>() {
                                new core.db() { column = "userid", value = uid }
            }));

            if(count > 0) Response.Redirect("/");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
             
        }
    }
}