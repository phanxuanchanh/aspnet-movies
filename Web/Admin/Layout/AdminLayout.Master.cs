using System;
using Web.Models;

namespace Web.Admin.Layout
{
    public partial class AdminLayout : System.Web.UI.MasterPage
    {
        protected string hyplnkOverview;
        protected string hyplnkCategoryList;
        protected string hyplnkCountryList;
        protected string hyplnkLanguageList;
        protected string hyplnkRoleList;
        protected string hyplnkTagList;
        protected string hyplnkDirectorList;
        protected string hyplnkUserList;
        protected string hyplnkCastList;
        protected string hyplnkFilmList;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                hyplnkOverview = GetRouteUrl("Admin_Overview", null);
                hyplnkCategoryList = GetRouteUrl("Admin_CategoryList", null);
                hyplnkCountryList = GetRouteUrl("Admin_CountryList", null);
                hyplnkLanguageList = GetRouteUrl("Admin_LanguageList", null);
                hyplnkRoleList = GetRouteUrl("Admin_RoleList", null);
                hyplnkDirectorList = GetRouteUrl("Admin_DirectorList", null);
                hyplnkTagList = GetRouteUrl("Admin_TagList", null);
                hyplnkUserList = GetRouteUrl("Admin_UserList", null);
                hyplnkCastList = GetRouteUrl("Admin_ActorList", null);
                hyplnkFilmList = GetRouteUrl("Admin_FilmList", null);

                object obj = Session["userSession"];
                if (obj == null)
                {
                    txtUsername.InnerText = "Anonymous";
                    txtRole.InnerText = "N/A";
                }
                else
                {
                    UserSession userSession = (UserSession)obj;
                    txtUsername.InnerText = userSession.username;
                    txtRole.InnerText = userSession.role;
                }
            }
            catch(Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }
    }
}