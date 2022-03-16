using Data.BLL;
using Data.DTO;
using System;
using System.Threading.Tasks;
using System.Web.UI;
using Web.Models;

namespace Web.Admin.RoleManagement
{
    public partial class RoleDetail : System.Web.UI.Page
    {
        protected RoleInfo roleInfo;
        protected bool enableShowDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            enableShowDetail = false;
            try
            {
                string id = GetRoleId();
                hyplnkList.NavigateUrl = GetRouteUrl("Admin_RoleList", null);
                hyplnkEdit.NavigateUrl = GetRouteUrl("Admin_UpdateRole", new { id = id });
                hyplnkDelete.NavigateUrl = GetRouteUrl("Admin_DeleteRole", new { id = id });

                if (CheckLoggedIn())
                    await GetRoleInfo(id);
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
            return (userSession.role == "Admin");
        }

        private string GetRoleId()
        {
            object obj = Page.RouteData.Values["id"];
            if (obj == null)
                return null;
            return obj.ToString();
        }

        private async Task GetRoleInfo(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                Response.RedirectToRoute("Admin_RoleList", null);
            }
            else
            {
                using(RoleBLL roleBLL = new RoleBLL())
                {
                    roleBLL.IncludeTimestamp = true;
                    roleInfo = await roleBLL.GetRoleAsync(id);
                }

                if (roleInfo == null)
                    Response.RedirectToRoute("Admin_RoleList", null);
                else
                    enableShowDetail = true;
            }
        }
    }
}