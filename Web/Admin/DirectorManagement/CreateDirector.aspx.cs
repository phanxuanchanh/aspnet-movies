using Data.BLL;
using Data.DTO;
using System;
using System.Threading.Tasks;
using Web.Models;
using Web.Validation;

namespace Web.Admin.DirectorManagement
{
    public partial class CreateDirector : System.Web.UI.Page
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
                hyplnkList.NavigateUrl = GetRouteUrl("Admin_DirectorList", null);
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
            catch(Exception ex)
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
                cvDirectorName,
                "txtDirectorName",
                "Tên đạo diễn không hợp lệ",
                true,
                null,
                customValidation.ValidateDirectorName
            );
        }

        private void ValidateData()
        {
            cvDirectorName.Validate();
        }

        private bool IsValidData()
        {
            ValidateData();
            return cvDirectorName.IsValid;
        }

        private DirectorCreation GetDirectorCreation()
        {
            return new DirectorCreation
            {
                name = Request.Form[txtDirectorName.UniqueID],
                description = Request.Form[txtDirectorDescription.UniqueID]
            };
        }

        public async Task Create()
        {
            if (IsValidData())
            {
                DirectorCreation director = GetDirectorCreation();
                CreationState state;
                using(DirectorBLL directorBLL = new DirectorBLL())
                {
                    state = await directorBLL.CreateDirectorAsync(director);
                }

                if (state == CreationState.Success)
                {
                    stateString = "Success";
                    stateDetail = "Đã thêm đạo diễn thành công";
                }
                else if (state == CreationState.AlreadyExists)
                {
                    stateString = "AlreadyExists";
                    stateDetail = "Thêm đạo diễn thất bại. Lý do: Đã tồn tại đạo diễn này";
                }
                else
                {
                    stateString = "Failed";
                    stateDetail = "Thêm đạo diễn thất bại";
                }
                enableShowResult = true;
            }
        }
    }
}