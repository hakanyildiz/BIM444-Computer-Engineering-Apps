using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace duyarliol
{
    public partial class registration : System.Web.UI.Page
    {
        core.data data = new core.data();
        

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