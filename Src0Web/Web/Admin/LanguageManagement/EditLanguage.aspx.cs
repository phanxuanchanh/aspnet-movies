using Common;
using Data.DTO;
using Data.Services;
using Ninject;
using System;
using System.Threading.Tasks;
using Web.Shared.Result;

namespace Web.Admin.LanguageManagement
{
    public partial class EditLanguage : AdminPage
    {
        private CustomValidation customValidation;
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

            customValidation = new CustomValidation();
            hyplnkList.NavigateUrl = GetRouteUrl("Admin_LanguageList", null);
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
                    await LoadLanguage(string.IsNullOrEmpty(Request.QueryString["Id"]) ? -1 : int.Parse(Request.QueryString["Id"]));
            }
        }

        private async Task LoadLanguage(int id)
        {
            if (id <= 0)
            {
                Response.RedirectToRoute("Admin_CountryList", null);
                return;
            }

            FilmMetadataService filmMetadataService = Inject<FilmMetadataService>();

            ExecResult<CountryDto> result = await filmMetadataService.GetCountryAsync(id);
            if (result.Status == ExecStatus.Success)
            {
                hdLanguageId.Value = result.Data.ID.ToString();
                txtLanguageName.Text = result.Data.Name;
                txtLanguageDescription.Text = result.Data.Description;
            }
            else
            {
                Response.RedirectToRoute("Admin_LanguageList", null);
            }
        }

        private void InitValidation()
        {
            customValidation.Init(
                cvLanguageName,
                "txtLanguageName",
                "Tên ngôn ngữ không hợp lệ",
                true,
                null,
                customValidation.ValidateCategoryName
            );
        }

        private void ValidateData()
        {
            cvLanguageName.Validate();
        }

        private bool IsValidData()
        {
            ValidateData();
            return cvLanguageName.IsValid;
        }

        private CreateLanguageDto InitCreateLanguageDto()
        {
            return new CreateLanguageDto
            {
                Name = Request.Form[txtLanguageName.UniqueID],
                Description = Request.Form[txtLanguageDescription.UniqueID]
            };
        }

        private UpdateLanguageDto InitUpdateLanguageDto()
        {
            return new UpdateLanguageDto
            {
                ID = int.Parse(Request.Form[hdLanguageId.UniqueID]),
                Name = Request.Form[txtLanguageName.UniqueID],
                Description = Request.Form[txtLanguageDescription.UniqueID]
            };
        }

        public async Task Create()
        {
            if (!IsValidData())
                return;

            CreateLanguageDto language = InitCreateLanguageDto();
            FilmMetadataService filmMetadataService = Inject<FilmMetadataService>();

            ExecResult<LanguageDto> commandResult = await filmMetadataService.AddLanguageAsync(language);
            notifControl.Set<LanguageDto>(commandResult);
        }

        public async Task Update()
        {
            if (!IsValidData())
                return;

            UpdateLanguageDto language = InitUpdateLanguageDto();
            FilmMetadataService filmMetadataService = Inject<FilmMetadataService>();

            ExecResult<LanguageDto> commandResult = await filmMetadataService.UpdateLanguageAsync(language);
            notifControl.Set<LanguageDto>(commandResult);
        }
    }
}