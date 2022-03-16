using Data.BLL;
using Data.DTO;
using System;
using System.Threading.Tasks;
using Web.Models;
using Web.Validation;

namespace Web.Admin.TagManagement
{
    public partial class CreateTag : System.Web.UI.Page
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
                hyplnkList.NavigateUrl = GetRouteUrl("Admin_TagList", null);
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
                cvTagName,
                "txtTagName",
                "Tên thẻ tag không hợp lệ",
                true,
                null,
                customValidation.ValidateTagName
            );
        }

        private void ValidateData()
        {
            cvTagName.Validate();
        }

        private bool IsValidData()
        {
            ValidateData();
            return cvTagName.IsValid;
        }

        private TagCreation GetTagCreation()
        {
            return new TagCreation
            {
                name = Request.Form[txtTagName.UniqueID],
                description = Request.Form[txtTagDescription.UniqueID]
            };
        }

        public async Task Create()
        {
            if (IsValidData())
            {
                TagCreation tag = GetTagCreation();
                CreationState state;
                using(TagBLL tagBLL = new TagBLL())
                {
                    state = await tagBLL.CreateTagAsync(tag);
                }

                if (state == CreationState.Success)
                {
                    stateString = "Success";
                    stateDetail = "Đã thêm thẻ tag thành công";
                }
                else if (state == CreationState.AlreadyExists)
                {
                    stateString = "AlreadyExists";
                    stateDetail = "Thêm thẻ tag thất bại. Lý do: Đã tồn tại thẻ tag này";
                }
                else
                {
                    stateString = "Failed";
                    stateDetail = "Thêm thẻ tag thất bại";
                }
                enableShowResult = true;
            }
        }
    }
}