using Data.DTO;
using Data.Services;
using System;
using System.Threading.Tasks;
using Web.Shared.Result;

namespace Web.Admin.FilmManagement
{
    public partial class EditFilm : AdminPage
    {
        private FilmMetadataService _filmMetaService;
        protected ExecResult<FilmDto> commandResult;
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

            _filmMetaService = Inject<FilmMetadataService>();
            customValidation = new CustomValidation();

            hyplnkList.NavigateUrl = GetRouteUrl("Admin_FilmList", null);
            InitValidation();
            //await LoadFilmCountries();
            //await LoadFilmLanguages();

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
                    await LoadFilm(Request.QueryString["Id"]);
            }
        }

        private async Task LoadFilm(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                Response.RedirectToRoute("Admin_FilmList", null);
                return;
            }

            FilmService filmService = Inject<FilmService>();

            ExecResult<FilmDto> result = await filmService.GetFilmAsync(id);
            if (result.Status == ExecStatus.Success)
            {
                hdFilmId.Value = result.Data.ID.ToString();
                txtFilmName.Text = result.Data.Name;
                txtFilmDescription.Text = result.Data.Description;
                txtProductionCompany.Text = result.Data.ProductionCompany;
                txtReleaseDate.Text = result.Data.ReleaseDate.ToString();
            }
            else
            {
                Response.RedirectToRoute("Admin_CountryList", null);
            }
        }

        private async Task LoadFilmCountries()
        {
            //drdlFilmCountry.Items.Clear();
            //List<CountryDto> countries = (await _filmMetaService.GetCountriesAsync(1, 30)).Items;
            //foreach (CountryDto country in countries)
            //{
            //    drdlFilmCountry.Items.Add(new ListItem(country.Name, country.ID.ToString()));
            //}
            //drdlFilmCountry.SelectedIndex = 0;
        }

        private async Task LoadFilmLanguages()
        {
            //drdlFilmLanguage.Items.Clear();
            //List<LanguageDto> languages = (await _filmMetaService.GetLanguagesAsync(1, 30)).Items;
            //foreach (LanguageDto language in languages)
            //{
            //    drdlFilmLanguage.Items.Add(new ListItem(language.Name, language.ID.ToString()));
            //}
            //drdlFilmLanguage.SelectedIndex = 0;
        }

        private void InitValidation()
        {
            customValidation.Init(
                cvFilmName,
                "txtFilmName",
                "Tên phim không hợp lệ",
                true,
                null,
                customValidation.ValidateFilmName
            );
            customValidation.Init(
                cvProductionCompany,
                "txtProductionCompany",
                "Tên công ty sản xuất không hợp lệ",
                true,
                null,
                customValidation.ValidateProductionCompany
            );
            customValidation.Init(
                cvReleaseDate,
                "txtReleaseDate",
                "Năm phát hành không hợp lệ",
                true,
                null,
                customValidation.ValidateReleaseDate
            );
        }

        private void ValidateData()
        {
            cvFilmName.Validate();
            cvProductionCompany.Validate();
            cvReleaseDate.Validate();
        }

        private bool IsValidData()
        {
            ValidateData();
            return (cvFilmName.IsValid && cvProductionCompany.IsValid && cvReleaseDate.IsValid);
        }

        private CreateFilmDto InitCreateFilmDto()
        {
            return new CreateFilmDto
            {
                Name = Request.Form[txtFilmName.UniqueID],
                ProductionCompany = Request.Form[txtProductionCompany.UniqueID],
                ReleaseDate = Request.Form[txtReleaseDate.UniqueID],
                Description = Request.Form[txtFilmDescription.UniqueID]
            };
        }

        public UpdateFilmDto InitUpdateFilmDto()
        {
            return new UpdateFilmDto
            {
                ID = Request.Form[hdFilmId.UniqueID],
                Name = Request.Form[txtFilmName.UniqueID],
                ProductionCompany = Request.Form[txtProductionCompany.UniqueID],
                ReleaseDate = Request.Form[txtReleaseDate.UniqueID],
                Description = Request.Form[txtFilmDescription.UniqueID]
            };
        }

        public async Task Create()
        {
            if (!IsValidData())
                return;

            CreateFilmDto film = InitCreateFilmDto();
            FilmService filmService = Inject<FilmService>();

            commandResult = await filmService.AddFilmAsync(film);
            notifControl.Set(commandResult);
        }

        private async Task Update()
        {
            if (!IsValidData())
                return;

            UpdateFilmDto film = InitUpdateFilmDto();
            FilmService filmService = Inject<FilmService>();

            commandResult = await filmService.UpdateFilmAsync(film);
            notifControl.Set(commandResult);
        }
    }
}