using Data.BLL;
using Data.DTO;
using System;
using System.Threading.Tasks;
using System.Web.UI;
using Web.Models;

namespace Web.Admin.RoleManagement
{
    public partial class DeleteRole : System.Web.UI.Page
    {
        protected RoleDto roleInfo;
        protected bool enableShowInfo;
        protected bool enableShowResult;
        protected string stateString;
        protected string stateDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            enableShowInfo = false;
            enableShowResult = false;
            stateString = null;
            stateDetail = null;
            try
            {
                hyplnkList.NavigateUrl = GetRouteUrl("Admin_RoleList", null);

                if (CheckLoggedIn())
                {
                    if (!IsPostBack)
                    {
                        await GetRoleInfo();
                    }
                }
                else
                {
                    Response.RedirectToRoute("Account_Login", null);
                }
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

        private async Task GetRoleInfo()
        {
            string id = GetRoleId();
            if (String.IsNullOrEmpty(id))
            {
                Response.RedirectToRoute("Admin_RoleList", null);
            }
            else
            {
                //using (RoleDao roleBLL = new RoleDao())
                //{
                //    roleInfo = await roleBLL.GetRoleAsync(id);
                //}

                if (roleInfo == null)
                    Response.RedirectToRoute("Admin_RoleList", null);
                else
                    enableShowInfo = true;
            }
        }

        private async Task DeleteRoleInfo()
        {
            string id = GetRoleId();
            DeletionState state = DeletionState.Success;
            //using (RoleDao roleBLL = new RoleDao())
            //{
            //    state = await roleBLL.DeleteRoleAsync(id);
            //}

            if (state == DeletionState.Success)
            {
                stateString = "Success";
                stateDetail = "Đã xóa vai trò thành công";
            }
            else if (state == DeletionState.Failed)
            {
                stateString = "Failed";
                stateDetail = "Xóa vai trò thất bại";
            }
            else
            {
                stateString = "ConstraintExists";
                stateDetail = "Không thể xóa vai trò. Lý do: Vai trò này đang được sử dụng!";
            }
            enableShowResult = true;
        }

        protected async void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                await DeleteRoleInfo();
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }
    }
}