using Data.BLL;
using Data.DTO;
using System;
using System.Threading.Tasks;
using Web.Models;
using Web.Validation;

namespace Web.Admin.CastManagement
{
    public partial class CreateCast : System.Web.UI.Page
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
                hyplnkList.NavigateUrl = GetRouteUrl("Admin_CastList", null);
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
            return (userSession.role == "Admin" || userSession.role == "Editor");
        }

        private void InitValidation()
        {
            customValidation.Init(
                cvCastName,
                "txtCastName",
                "Tên diễn viên không hợp lệ",
                true,
                null,
                customValidation.ValidateCastName
            );
        }

        private void ValidateData()
        {
            cvCastName.Validate();
        }

        private bool IsValidData()
        {
            ValidateData();
            return cvCastName.IsValid;
        }

        private CastCreation GetCastCreation()
        {
            return new CastCreation
            {
                name = Request.Form[txtCastName.UniqueID],
                description = Request.Form[txtCastDescription.UniqueID]
            };
        }

        public async Task Create()
        {
            if (IsValidData())
            {
                CastCreation cast = GetCastCreation();
                CreationState state;
                using(CastBLL castBLL = new CastBLL())
                {
                    state = await castBLL.CreateCastAsync(cast);
                }

                if (state == CreationState.Success)
                {
                    stateString = "Success";
                    stateDetail = "Đã thêm diễn viên thành công";
                }
                else if (state == CreationState.AlreadyExists)
                {
                    stateString = "AlreadyExists";
                    stateDetail = "Thêm diễn viên thất bại. Lý do: Đã tồn tại diễn viên này";
                }
                else
                {
                    stateString = "Failed";
                    stateDetail = "Thêm diễn viên thất bại";
                }
                enableShowResult = true;
            }
        }
    }
}