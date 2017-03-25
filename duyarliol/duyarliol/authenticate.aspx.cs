using System;

namespace duyarliol
{
    public partial class authenticate : System.Web.UI.Page
    {
        public string method { get; set; }
        public string email { get; set; }
        public string accesstoken { get; set; }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (Session["auth-method"] != null && Session["auth-email"] != null)
            {
                method = Session["auth-method"].ToString();
                email = Session["auth-email"].ToString();
                accesstoken = Session["auth-accesstoken"].ToString();
            }
            else Response.Redirect("/");
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}