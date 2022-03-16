using Data.BLL;
using Data.DTO;
using System;
using System.Threading.Tasks;
using System.Web.UI;
using Web.Models;

namespace Web.Admin.DirectorManagement
{
    public partial class DirectorDetail : System.Web.UI.Page
    {
        protected DirectorInfo directorInfo;
        protected bool enableShowDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            enableShowDetail = false;
            try
            {
                long id = GetDirectorId();
                hyplnkList.NavigateUrl = GetRouteUrl("Admin_DirectorList", null);
                hyplnkEdit.NavigateUrl = GetRouteUrl("Admin_UpdateDirector", new { id = id });
                hyplnkDelete.NavigateUrl = GetRouteUrl("Admin_DeleteDirector", new { id = id });

                if (CheckLoggedIn())
                    await GetDirectorInfo(id);
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

        private long GetDirectorId()
        {
            object obj = Page.RouteData.Values["id"];
            if (obj == null)
                return -1;
            return long.Parse(obj.ToString());
        }

        private async Task GetDirectorInfo(long id)
        {
            if (id <= 0)
            {
                Response.RedirectToRoute("Admin_DirectorList", null);
            }
            else
            {
                using(DirectorBLL directorBLL = new DirectorBLL())
                {
                    directorBLL.IncludeDescription = true;
                    directorBLL.IncludeTimestamp = true;
                    directorInfo = await directorBLL.GetDirectorAsync(id);
                }

                if (directorInfo == null)
                    Response.RedirectToRoute("Admin_DirectorList", null);
                else
                    enableShowDetail = true;
            }
        }
    }
}