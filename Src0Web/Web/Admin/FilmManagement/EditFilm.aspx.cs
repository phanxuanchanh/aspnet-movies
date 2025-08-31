using Data.DTO;
using Data.Services;
using System;
using System.Threading.Tasks;
using Web.Shared.Result;

namespace Web.Admin.FilmManagement
{
    public partial class EditFilm : AdminPage, IPostbackAwarePage
    {
        private FilmMetadataService _filmMetaService;
        protected ExecResult<FilmDto> commandResult;
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

            hyplnkList.NavigateUrl = GetRouteUrl("Admin_FilmList", null);
            InitValidation();
        }

        public async void Page_LoadNoPostback(object sender, EventArgs e)
        {
            if (!isCreateAction)
                await LoadFilm(Request.QueryString["Id"]);
        }

        public async void Page_LoadWithPostback(object sender, EventArgs e)
        {
            if (isCreateAction)
                return;

            string target = Request["__EVENTTARGET"];
            string argument = Request["__EVENTARGUMENT"];
            if (target == "CategorySelected_Click" && !string.IsNullOrEmpty(argument))
            {
                string filmId = hdFilmId.Value;
                string categoryId = argument;

                await Task.CompletedTask;
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

        protected async void btnSubmit_Click(object sender, EventArgs e)
        {
            if (isCreateAction)
                await Create();
            else
                await Update();
        }

        private void InitValidation()
        {
            cvFilmName.SetValidator(
                nameof(txtFilmName),
                "Tên phim không hợp lệ",
                true,
                null,
                CustomValidation.ValidateFilmName
            );

            cvProductionCompany.SetValidator(
                nameof(txtProductionCompany),
                "Tên công ty sản xuất không hợp lệ",
                true,
                null,
                CustomValidation.ValidateProductionCompany
            );

            cvReleaseDate.SetValidator(
                nameof(txtReleaseDate),
                "Năm phát hành không hợp lệ",
                true,
                null,
                CustomValidation.ValidateReleaseDate
            );
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
            Page.Validate();

            if (!Page.IsValid)
                return;

            CreateFilmDto film = InitCreateFilmDto();
            FilmService filmService = Inject<FilmService>();

            commandResult = await filmService.AddFilmAsync(film);
            notifControl.Set(commandResult);
        }

        private async Task Update()
        {
            Page.Validate();

            if (!Page.IsValid)
                return;

            UpdateFilmDto film = InitUpdateFilmDto();
            FilmService filmService = Inject<FilmService>();

            commandResult = await filmService.UpdateFilmAsync(film);
            notifControl.Set(commandResult);
        }
    }
}