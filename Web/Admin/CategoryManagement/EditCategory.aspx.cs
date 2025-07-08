using Common;
using Data.BLL;
using Data.DTO;
using Data.Services;
using Ninject;
using System;
using System.Threading.Tasks;
using Web.App_Start;
using Web.Models;
using Web.Validation;

namespace Web.Admin.CategoryManagement
{
    public partial class CreateCategory : System.Web.UI.Page
    {
        private CustomValidation customValidation;
        protected ExecResult<CategoryDto> commandResult;
        protected bool isCreateAction;
        protected bool enableShowResult;

        protected async void Page_Load(object sender, EventArgs e)
        {
            string action = Request.QueryString["action"];
            if (string.IsNullOrEmpty(action))
            {
                Response.StatusCode = 400;
                Response.ContentType = "text/plain";
                Response.Write("Invalid or missing parameters.");
                Context.ApplicationInstance.CompleteRequest();
            }

            isCreateAction = action == "create";
            btnSubmit.Text = isCreateAction ? "Create" : "Update";

            customValidation = new CustomValidation();
            enableShowResult = false;
            try
            {
                hyplnkList.NavigateUrl = GetRouteUrl("Admin_CategoryList", null);
                InitValidation();

                if (!CheckLoggedIn())
                {
                    Response.RedirectToRoute("Account_Login", null);
                    return;
                }

                if (IsPostBack)
                {
                    if (isCreateAction)
                        await Create();
                    else
                        await Update();
                }
                else
                {
                    if (!isCreateAction)
                        await LoadCategory(string.IsNullOrEmpty(Request.QueryString["Id"]) ? -1 : int.Parse(Request.QueryString["Id"]));
                }
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }

        private async Task LoadCategory(int id)
        {
            if (id <= 0)
            {
                Response.RedirectToRoute("Admin_CategoryList", null);
                return;
            }

            using (TaxonomyService taxonomyService = NinjectWebCommon.Kernel.Get<TaxonomyService>())
            {
                ExecResult<CategoryDto> result = await taxonomyService.GetCategoryAsync(id);
                if (result.Status == ExecStatus.Success)
                {
                    hdCategoryId.Value = result.Data.ID.ToString();
                    txtCategoryName.Text = result.Data.Name;
                    txtCategoryDescription.Text = result.Data.Description;
                }
                else
                {
                    Response.RedirectToRoute("Admin_CountryList", null);
                }
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

        private CreateCategoryDto InitCreateCategoryDto()
        {
            return new CreateCategoryDto
            {
                Name = Request.Form[txtCategoryName.UniqueID],
                Description = Request.Form[txtCategoryDescription.UniqueID]
            };
        }

        private UpdateCategoryDto InitUpdateCategoryDto()
        {
            return new UpdateCategoryDto
            {
                ID = int.Parse(Request.Form[hdCategoryId.UniqueID]),
                Name = Request.Form[txtCategoryName.UniqueID],
                Description = Request.Form[txtCategoryDescription.UniqueID]
            };
        }

        public async Task Create()
        {
            if (!IsValidData())
                return;

            CreateCategoryDto category = InitCreateCategoryDto();
            using(TaxonomyService taxonomyService = NinjectWebCommon.Kernel.Get<TaxonomyService>())
            {
                commandResult = await taxonomyService.AddCategoryAsync(category);
            }

            enableShowResult = true;
        }

        public async Task Update()
        {
            if (!IsValidData())
                return;

            UpdateCategoryDto category = InitUpdateCategoryDto();
            using (TaxonomyService taxonomyService = NinjectWebCommon.Kernel.Get<TaxonomyService>())
            {
                commandResult = await taxonomyService.UpdateCategoryAsync(category);
            }

            enableShowResult = true;
        }
    }
}