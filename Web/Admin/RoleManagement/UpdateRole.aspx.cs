using Data.BLL;
using Data.DTO;
using System;
using System.Threading.Tasks;
using System.Web.UI;
using Web.Models;
using Web.Validation;

namespace Web.Admin.RoleManagement
{
    public partial class UpdateRole : System.Web.UI.Page
    {
        private RoleBLL roleBLL;
        protected RoleInfo roleInfo;
        private CustomValidation customValidation;
        protected bool enableShowResult;
        protected string stateString;
        protected string stateDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            roleBLL = new RoleBLL();
            customValidation = new CustomValidation();
            enableShowResult = false;
            stateString = null;
            stateDetail = null;
            try
            {
                string id = GetRoleId();
                hyplnkList.NavigateUrl = GetRouteUrl("Admin_RoleList", null);
                hyplnkDetail.NavigateUrl = GetRouteUrl("Admin_RoleDetail", new { id = id });
                hyplnkDelete.NavigateUrl = GetRouteUrl("Admin_DeleteRole", new { id = id });
                InitValidation();

                if (CheckLoggedIn())
                {
                    if (IsPostBack)
                    {
                        if (IsValidData())
                        {
                            await Update();
                            await LoadRoleInfo(id);
                        }
                    }
                    else
                    {
                        await LoadRoleInfo(id);
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
            roleBLL.Dispose();
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

        private async Task LoadRoleInfo(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                Response.RedirectToRoute("Admin_CategoryList", null);
            }
            else
            {
                RoleInfo roleInfo = await roleBLL.GetRoleAsync(id);
                if (roleInfo == null)
                {
                    Response.RedirectToRoute("Admin_CategoryList", null);
                }
                else
                {
                    hdRoleId.Value = roleInfo.ID;
                    txtRoleName.Text = roleInfo.name;
                }
            }
        }

        private void InitValidation()
        {
            customValidation.Init(
                cvRoleName,
                "txtRoleName",
                "Tên vai trò không hợp lệ",
                true,
                null,
                customValidation.ValidateRoleName
            );
        }

        private void ValidateData()
        {
            cvRoleName.Validate();
        }

        private bool IsValidData()
        {
            ValidateData();
            return cvRoleName.IsValid;
        }

        private RoleUpdate GetRoleUpdate()
        {
            return new RoleUpdate
            {
                ID = Request.Form[hdRoleId.UniqueID],
                name = Request.Form[txtRoleName.UniqueID],
            };
        }

        private async Task Update()
        {
            RoleUpdate roleUpdate = GetRoleUpdate();
            UpdateState state = await roleBLL.UpdateRoleAsync(roleUpdate);
            if (state == UpdateState.Success)
            {
                stateString = "Success";
                stateDetail = "Đã cập nhật vai trò thành công";
            }
            else
            {
                stateString = "Failed";
                stateDetail = "Cập nhật vai trò thất bại";
            }
            enableShowResult = true;
        }
    }
}