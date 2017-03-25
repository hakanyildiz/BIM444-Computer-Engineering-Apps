using System;

namespace duyarliol
{
    public partial class _default : System.Web.UI.Page
    {
        //protected void Page_Init(object sender, EventArgs e)
        //{
        //    if (Session["user"] == null)
        //    {
        //        Response.Redirect("~/registration.aspx");
        //    }
        //}
        public int uid = 0;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["user"] != null)
            {
                uid = ((core.user)Session["user"]).id;
            }

            if (!IsPostBack)
            {
                Page.Title = "Duyarlı.ol";
                Page.MetaKeywords = "duyarlı,ol,duyarlılık,web,asistan,web economist";
                Page.MetaDescription = "duyarlıol.com bir duyarlılık kampanyasıdır.";
            }


        }

    }
}