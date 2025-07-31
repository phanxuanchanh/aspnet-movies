using Web.Models;

namespace Web.Admin
{
    public class AdminPage : System.Web.UI.Page
    {
        protected bool CheckLoggedIn()
        {
            object obj = Session["userSession"];
            if (obj == null)
                return false;

            UserSession userSession = (UserSession)obj;
            return (userSession.role == "Admin" || userSession.role == "Editor");
        }
    }
}