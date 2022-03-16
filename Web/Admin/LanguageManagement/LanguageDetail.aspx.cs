using Data.BLL;
using Data.DTO;
using System;
using System.Threading.Tasks;
using System.Web.UI;
using Web.Models;

namespace Web.Admin.LanguageManagement
{
    public partial class LanguageDetail : System.Web.UI.Page
    {
        protected LanguageInfo languageInfo;
        protected bool enableShowDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            enableShowDetail = false;
            try
            {
                int id = GetLanguageId();
                hyplnkList.NavigateUrl = GetRouteUrl("Admin_LanguageList", null);
                hyplnkEdit.NavigateUrl = GetRouteUrl("Admin_UpdateLanguage", new { id = id });
                hyplnkDelete.NavigateUrl = GetRouteUrl("Admin_DeleteLanguage", new { id = id });

                if (CheckLoggedIn())
                    await GetLanguageInfo(id);
                else
                    Response.RedirectToRoute("Account_Login", null);
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }

        private bool CheckLoggedIn()
        {
            object obj = Session["userSession"];
            if (obj == null)
                return false;

            UserSession userSession = (UserSession)obj;
            return (userSession.role == "Admin" || userSession.role == "Editor");
        }

        private int GetLanguageId()
        {
            object obj = Page.RouteData.Values["id"];
            if (obj == null)
                return -1;
            return int.Parse(obj.ToString());
        }

        private async Task GetLanguageInfo(int id)
        {
            if (id <= 0)
            {
                Response.RedirectToRoute("Admin_LanguageList", null);
            }
            else
            {
                using(LanguageBLL languageBLL = new LanguageBLL())
                {
                    languageBLL.IncludeDescription = true;
                    languageBLL.IncludeTimestamp = true;
                    languageInfo = await languageBLL.GetLanguageAsync(id);
                }

                if (languageInfo == null)
                    Response.RedirectToRoute("Admin_LanguageList", null);
                else
                    enableShowDetail = true;
            }
        }
    }
}