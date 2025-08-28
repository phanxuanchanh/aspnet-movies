using Data.DTO;
using Data.Services;
using System;
using System.Threading.Tasks;
using Web.Shared.Result;

namespace Web.Admin.CountryManagement
{
    public partial class EditCountry : AdminPage
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

            FilmMetadataService filmMetadataService = Inject<FilmMetadataService>();

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

        private void InitValidation()
        {
            cvCountryName.SetValidator(
                nameof(txtCountryName),
                "Tên quốc gia không hợp lệ",
                true,
                null,
                CustomValidation.ValidateCountryName
            );
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
            Page.Validate();

            if (!Page.IsValid)
                return;

            CreateCountryDto country = InitCreateCountryDto();
            FilmMetadataService filmMetadataService = Inject<FilmMetadataService>();

            ExecResult<CountryDto> commandResult = await filmMetadataService.AddCountryAsync(country);
            notifControl.Set<CountryDto>(commandResult);
        }

        private async Task Update()
        {
            Page.Validate();

            if (!Page.IsValid)
                return;

            UpdateCountryDto countryUpdate = InitUpdateCountryDto();
            FilmMetadataService filmMetadataService = Inject<FilmMetadataService>();

            ExecResult<CountryDto> commandResult = await filmMetadataService.UpdateCountryAsync(countryUpdate);
            notifControl.Set<CountryDto>(commandResult);
        }
    }
}