using Common;
using Data.DTO;
using Data.Services;
using Ninject;
using System;
using System.Threading.Tasks;
using Web.App_Start;
using Web.Models;
using Web.Validation;

namespace Web.Admin.LanguageManagement
{
    public partial class CreateLanguage : System.Web.UI.Page
    {
        private CustomValidation customValidation;
        protected ExecResult<LanguageDto> commandResult;
        protected bool isCreateAction;
        protected bool enableShowResult;
        protected string stateString;
        protected string stateDetail;

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
            stateString = null;
            stateDetail = null;
            try
            {
                hyplnkList.NavigateUrl = GetRouteUrl("Admin_LanguageList", null);
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
                        await LoadLanguage(string.IsNullOrEmpty(Request.QueryString["Id"]) ? -1 : int.Parse(Request.QueryString["Id"]));
                }
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }

        private async Task LoadLanguage(int id)
        {
            if (id <= 0)
            {
                Response.RedirectToRoute("Admin_CountryList", null);
                return;
            }

            using (FilmMetadataService filmMetadataService = NinjectWebCommon.Kernel.Get<FilmMetadataService>())
            {
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
            using (FilmMetadataService filmMetadataService = NinjectWebCommon.Kernel.Get<FilmMetadataService>())
            {
                commandResult = await filmMetadataService.AddLanguageAsync(language);
            }

            enableShowResult = true;
        }

        public async Task Update()
        {
            if (!IsValidData())
                return;

            UpdateLanguageDto language = InitUpdateLanguageDto();
            using (FilmMetadataService filmMetadataService = NinjectWebCommon.Kernel.Get<FilmMetadataService>())
            {
                commandResult = await filmMetadataService.UpdateLanguageAsync(language);
            }
            enableShowResult = true;
        }
    }
}