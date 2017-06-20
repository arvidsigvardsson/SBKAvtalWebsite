using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var User = System.Web.HttpContext.Current.User.Identity.Name;
        userlabel.Text = User; //HttpContext.Current.User.Identity.Name
        auth.Text = System.Web.HttpContext.Current.User.Identity.IsAuthenticated.ToString();
    }
}