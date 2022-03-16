using Data.BLL;
using Data.DTO;
using System;
using System.Threading.Tasks;
using Web.Models;
using Web.Validation;

namespace Web.Admin.RoleManagement
{
    public partial class CreateRole : System.Web.UI.Page
    {
        private CustomValidation customValidation;
        protected bool enableShowResult;
        protected string stateString;
        protected string stateDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            customValidation = new CustomValidation();
            enableShowResult = false;
            stateString = null;
            stateDetail = null;
            try
            {
                hyplnkList.NavigateUrl = GetRouteUrl("Admin_RoleList", null);
                InitValidation();

                if (CheckLoggedIn())
                {
                    if (IsPostBack)
                    {
                        await Create();
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

        private RoleCreation GetRoleCreation()
        {
            return new RoleCreation
            {
                name = Request.Form[txtRoleName.UniqueID],
            };
        }

        public async Task Create()
        {
            if (IsValidData())
            {
                RoleCreation role = GetRoleCreation();
                CreationState state;
                using(RoleBLL roleBLL = new RoleBLL())
                {
                    state = await roleBLL.CreateRoleAsync(role);
                }

                if (state == CreationState.Success)
                {
                    stateString = "Success";
                    stateDetail = "Đã thêm vai trò thành công";
                }
                else if (state == CreationState.AlreadyExists)
                {
                    stateString = "AlreadyExists";
                    stateDetail = "Thêm vài trò thất bại. Lý do: Đã tồn tại vai trò này";
                }
                else
                {
                    stateString = "Failed";
                    stateDetail = "Thêm vai trò thất bại";
                }
                enableShowResult = true;
            }
        }
    }
}