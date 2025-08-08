using Common;
using Data.DTO;
using Data.Services;
using Ninject;
using System;
using System.Threading.Tasks;
using Web.App_Start;
using Web.Models;
using Web.Shared.Result;
using Web.Shared.WebForms;
using Web.Validation;

namespace Web.Admin.CountryManagement
{
    public partial class EditCountry : AdminPage
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

            hyplnkList.NavigateUrl = GetRouteUrl("Admin_CountryList", null);
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
                    await LoadCountry(string.IsNullOrEmpty(Request.QueryString["Id"]) ? -1 : int.Parse(Request.QueryString["Id"]));

            }
        }

        private async Task LoadCountry(int id)
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
                ExecResult<CountryDto> commandResult = await filmMetadataService.AddCountryAsync(country);
                notifControl.Set<CountryDto>(commandResult);
            }
        }

        private async Task Update()
        {
            if (!IsValidData())
                return;

            UpdateCountryDto countryUpdate = InitUpdateCountryDto();
            using (FilmMetadataService filmMetadataService = NinjectWebCommon.Kernel.Get<FilmMetadataService>())
            {
                ExecResult<CountryDto> commandResult = await filmMetadataService.UpdateCountryAsync(countryUpdate);
                notifControl.Set<CountryDto>(commandResult);
            }
        }
    }
}