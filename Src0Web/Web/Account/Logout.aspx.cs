using System;
using System.Web;
using System.Web.Security;

namespace Web.Account
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            hyplnkHome.NavigateUrl = GetRouteUrl("User_Home", null);

            if (HttpContext.Current.User?.Identity?.IsAuthenticated != true)
                Response.RedirectToRoute("User_Home");
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Response.RedirectToRoute("User_Home");
        }
    }
}