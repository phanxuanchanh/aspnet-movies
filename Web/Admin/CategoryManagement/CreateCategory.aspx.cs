using Data.BLL;
using Data.DTO;
using System;
using System.Threading.Tasks;
using Web.Models;
using Web.Validation;

namespace Web.Admin.CategoryManagement
{
    public partial class CreateCategory : System.Web.UI.Page
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
                hyplnkList.NavigateUrl = GetRouteUrl("Admin_CategoryList", null);
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
                cvCategoryName,
                "txtCategoryName",
                "Tên thể loại không hợp lệ",
                true,
                null,
                customValidation.ValidateCategoryName
            );
        }

        private void ValidateData()
        {
            cvCategoryName.Validate();
        }

        private bool IsValidData()
        {
            ValidateData();
            return cvCategoryName.IsValid;
        }

        private CategoryCreation GetCategoryCreation()
        {
            return new CategoryCreation
            {
                name = Request.Form[txtCategoryName.UniqueID],
                description = Request.Form[txtCategoryDescription.UniqueID]
            };
        }

        public async Task Create()
        {
            if (IsValidData())
            {
                CategoryCreation category = GetCategoryCreation();
                CreationState state;
                using(CategoryBLL categoryBLL = new CategoryBLL())
                {
                    state = await categoryBLL.CreateCategoryAsync(category);
                }

                if (state == CreationState.Success)
                {
                    stateString = "Success";
                    stateDetail = "Đã thêm thể loại thành công";
                }
                else if (state == CreationState.AlreadyExists)
                {
                    stateString = "AlreadyExists";
                    stateDetail = "Thêm thể loại thất bại. Lý do: Đã tồn tại thể loại này";
                }
                else
                {
                    stateString = "Failed";
                    stateDetail = "Thêm thể loại thất bại";
                }
                enableShowResult = true;
            }
        }
    }
}