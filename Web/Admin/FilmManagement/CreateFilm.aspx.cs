using Data.BLL;
using Data.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using Web.Models;
using Web.Validation;

namespace Web.Admin.FilmManagement
{
    public partial class CreateFilm : System.Web.UI.Page
    {
        private FilmBLL filmBLL;
        private CustomValidation customValidation;
        protected bool enableShowResult;
        protected string stateString;
        protected string stateDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            filmBLL = new FilmBLL();
            customValidation = new CustomValidation();
            enableShowResult = false;
            stateString = null;
            stateDetail = null;
            try
            {
                hyplnkList.NavigateUrl = GetRouteUrl("Admin_FilmList", null);
                InitValidation();
                await LoadFilmCountries();
                await LoadFilmLanguages();

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
            filmBLL.Dispose();
        }

        private bool CheckLoggedIn()
        {
            object obj = Session["userSession"];
            if (obj == null)
                return false;

            UserSession userSession = (UserSession)obj;
            return (userSession.role == "Admin" || userSession.role == "Editor");
        }

        private async Task LoadFilmCountries()
        {
            drdlFilmCountry.Items.Clear();
            List<CountryDto> countryInfos = await new CountryBLL(filmBLL).GetCountriesAsync();
            foreach (CountryDto countryInfo in countryInfos)
            {
                drdlFilmCountry.Items.Add(new ListItem(countryInfo.Name, countryInfo.ID.ToString()));
            }
            drdlFilmCountry.SelectedIndex = 0;
        }

        private async Task LoadFilmLanguages()
        {
            drdlFilmLanguage.Items.Clear();
            List<LanguageInfo> languageInfos = await new LanguageBLL(filmBLL).GetLanguagesAsync();
            foreach (LanguageInfo languageInfo in languageInfos)
            {
                drdlFilmLanguage.Items.Add(new ListItem(languageInfo.name, languageInfo.ID.ToString()));
            }
            drdlFilmLanguage.SelectedIndex = 0;
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

        private FilmCreation GetFilmCreation()
        {
            return new FilmCreation
            {
                name = Request.Form[txtFilmName.UniqueID],
                countryId = int.Parse(Request.Form[drdlFilmCountry.UniqueID]),
                productionCompany = Request.Form[txtProductionCompany.UniqueID],
                languageId = int.Parse(Request.Form[drdlFilmLanguage.UniqueID]),
                releaseDate = Request.Form[txtReleaseDate.UniqueID],
                description = Request.Form[txtFilmDescription.UniqueID]
            };
        }

        public async Task Create()
        {
            if (IsValidData())
            {
                FilmCreation filmCreation = GetFilmCreation();
                CreationState state = await filmBLL.CreateFilmAsync(filmCreation);
                if (state == CreationState.Success)
                {
                    stateString = "Success";
                    stateDetail = "Đã thêm phim thành công";
                }
                else if (state == CreationState.AlreadyExists)
                {
                    stateString = "AlreadyExists";
                    stateDetail = "Thêm phim thất bại. Lý do: Đã tồn tại phim này";
                }
                else
                {
                    stateString = "Failed";
                    stateDetail = "Thêm phim thất bại";
                }
                enableShowResult = true;
            }
        }
    }
}