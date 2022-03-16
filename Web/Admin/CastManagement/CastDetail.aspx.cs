using Data.BLL;
using Data.DTO;
using System;
using System.Threading.Tasks;
using System.Web.UI;
using Web.Models;

namespace Web.Admin.CastManagement
{
    public partial class CastDetail : System.Web.UI.Page
    {
        protected CastInfo castInfo;
        protected bool enableShowDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            enableShowDetail = false;
            try
            {
                long id = GetCastId();
                hyplnkList.NavigateUrl = GetRouteUrl("Admin_CastList", null);
                hyplnkEdit.NavigateUrl = GetRouteUrl("Admin_UpdateCast", new { id = id });
                hyplnkDelete.NavigateUrl = GetRouteUrl("Admin_DeleteCast", new { id = id });

                if (CheckLoggedIn())
                    await GetCastInfo(id);
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

        private long GetCastId()
        {
            object obj = Page.RouteData.Values["id"];
            if (obj == null)
                return -1;
            return long.Parse(obj.ToString());
        }

        private async Task GetCastInfo(long id)
        {
            if (id <= 0)
            {
                Response.RedirectToRoute("Admin_CastList", null);
            }
            else
            {
                using(CastBLL castBLL = new CastBLL())
                {
                    castBLL.IncludeDescription = true;
                    castBLL.IncludeTimestamp = true;
                    castInfo = await castBLL.GetCastAsync(id);
                }

                if (castInfo == null)
                    Response.RedirectToRoute("Admin_CastList", null);
                else
                    enableShowDetail = true;
            }
        }
    }
}