using Data.DTO;
using Data.Services;
using System;
using System.Threading.Tasks;
using Web.Shared.Result;

namespace Web.Admin.CategoryManagement
{
    public partial class EditCategory : AdminPage
    {
        protected bool isCreateAction;

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

            hyplnkList.NavigateUrl = GetRouteUrl("Admin_CategoryList", null);
            InitValidation();

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

        private async Task LoadCategory(int id)
        {
            if (id <= 0)
            {
                Response.RedirectToRoute("Admin_CategoryList", null);
                return;
            }

            TaxonomyService taxonomyService = Inject<TaxonomyService>();

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

        private void InitValidation()
        {
            cvCategoryName.SetValidator(
                nameof(txtCategoryName),
                "Không được trống, từ 2 đến 100 ký tự",
                true,
                null,
                CustomValidation.ValidateCategoryName
            );
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
            Page.Validate();

            if (!Page.IsValid)
                return;

            CreateCategoryDto category = InitCreateCategoryDto();
            TaxonomyService taxonomyService = Inject<TaxonomyService>();

            ExecResult<CategoryDto> commandResult = await taxonomyService.AddCategoryAsync(category);
            notifControl.Set<CategoryDto>(commandResult);
        }

        public async Task Update()
        {
            Page.Validate();

            if (!Page.IsValid)
                return;

            UpdateCategoryDto category = InitUpdateCategoryDto();
            TaxonomyService taxonomyService = Inject<TaxonomyService>();

            ExecResult<CategoryDto> commandResult = await taxonomyService.UpdateCategoryAsync(category);
            notifControl.Set<CategoryDto>(commandResult);
        }
    }
}