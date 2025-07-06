using Common;
using Data.BLL;
using Data.DAL;
using Data.DTO;
using Data.Services;
using Ninject;
using System;
using System.Threading.Tasks;
using Web.App_Start;
using Web.Models;
using Web.Validation;

namespace Web.Admin.CountryManagement
{
    public partial class CreateCountry : System.Web.UI.Page
    {
        private CustomValidation customValidation;
        protected ExecResult<CountryDto> commandResult;
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
                hyplnkList.NavigateUrl = GetRouteUrl("Admin_CountryList", null);
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
                        await LoadCountry(string.IsNullOrEmpty(Request.QueryString["Id"]) ? -1 : int.Parse(Request.QueryString["Id"]));

                }
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }

        private async Task LoadCountry(int id)
        {
            if (id <= 0)
            {
                Response.RedirectToRoute("Admin_CountryList", null);
                return;
            }

            using(FilmMetadataService filmMetadataService = NinjectWebCommon.Kernel.Get<FilmMetadataService>())
            {
                ExecResult<CountryDto> result = await filmMetadataService.GetCountryAsync(id);
                if (result.Status == ExecStatus.Success)
                {
                    hdCountryId.Value = result.Data.ID.ToString();
                    txtCountryName.Text = result.Data.Name;
                    txtCountryDescription.Text = result.Data.Description;
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
                cvCountryName,
                "txtCountryName",
                "Tên quốc gia không hợp lệ",
                true,
                null,
                customValidation.ValidateCountryName
            );
        }

        private void ValidateData()
        {
            cvCountryName.Validate();
        }

        private bool IsValidData()
        {
            ValidateData();
            return cvCountryName.IsValid;
        }

        private CreateCountryDto InitCreateCountryDto()
        {
            return new CreateCountryDto
            {
                Name = Request.Form[txtCountryName.UniqueID],
                Description = Request.Form[txtCountryDescription.UniqueID]
            };
        }

        private UpdateCountryDto InitUpdateCountryDto()
        {
            return new UpdateCountryDto
            {
                ID = int.Parse(Request.Form[hdCountryId.UniqueID]),
                Name = Request.Form[txtCountryName.UniqueID],
                Description = Request.Form[txtCountryDescription.UniqueID]
            };
        }

        public async Task Create()
        {
            if (!IsValidData())
                return;

            CreateCountryDto country = InitCreateCountryDto();
            using (FilmMetadataService filmMetadataService = NinjectWebCommon.Kernel.Get<FilmMetadataService>())
            {
                commandResult = await filmMetadataService.AddCountryAsync(country);
            }

            enableShowResult = true;
        }

        private async Task Update()
        {
            if (!IsValidData())
                return;

            UpdateCountryDto countryUpdate = InitUpdateCountryDto();
            using(FilmMetadataService filmMetadataService = NinjectWebCommon.Kernel.Get<FilmMetadataService>())
            {
                commandResult = await filmMetadataService.UpdateCountryAsync(countryUpdate);
            }

            enableShowResult = true;
        }
    }
}