using System;

namespace Web.Account
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            hyplnkHome.NavigateUrl = GetRouteUrl("User_Home", null);
            if (Session["userSession"] == null)
                Response.RedirectToRoute("User_Home");
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session["userSession"] = null;
            Response.RedirectToRoute("User_Home");
        }
    }
}