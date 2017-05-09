using System;
using System.Collections.Generic;
using System.Web;

namespace duyarliol
{
    public partial class _default : System.Web.UI.Page
    {
        public int uid = 0;

        protected void Page_Init(object sender, EventArgs e)
        {
            if (Session["user"] != null)
            {
                uid = ((core.user)Session["user"]).id;

                core.data data = new core.data();
                int count = Convert.ToInt32(data.getsinglecolumndbcount("userincome", new List<core.db>() {
                                new core.db() { column = "userid", value = uid }
                    }));

                if (count > 0)
                {

                }
                else
                {
                    Response.Redirect("~/welcome");
                }

                HttpCookie cookie = Request.Cookies["duyarliol"];
                if (cookie == null)
                {
                    HttpCookie userCookie = new HttpCookie("duyarliol");
                    userCookie["auth"] = ((core.user)Session["user"]).apikey.ToString();
                    userCookie.Expires = DateTime.Now.AddDays(30);
                    Response.Cookies.Add(userCookie);
                    
                }
            }
        }
    
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page.Title = "Duyarlı.ol";
                Page.MetaKeywords = "duyarlı,ol,duyarlılık,web,asistan,web economist";
                Page.MetaDescription = "duyarlıol.com bir duyarlılık kampanyasıdır.";
            }


        }

    }
}