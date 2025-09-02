using Data.DTO;
using Data.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Shared.Result;

namespace Web.Admin.FilmManagement
{
    public partial class EditFilm : AdminPage, IPostbackAwarePage
    {
        protected ExecResult<FilmDto> commandResult;
        protected bool isCreateAction;

        protected void Page_Load(object sender, EventArgs e)
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

            hyplnkList.NavigateUrl = GetRouteUrl("Admin_FilmList", null);
            InitValidation();
        }

        public async void Page_LoadNoPostback(object sender, EventArgs e)
        {
            if (isCreateAction)
                return;

            string filmId = GetId<string>(fromQueryString: true);
            if (string.IsNullOrEmpty(filmId))
            {
                Response.ForceRedirectToRoute(this, "Admin_FilmList", null);
                return;
            }

            await LoadFilm(filmId);
            await LoadCategories(filmId);
            await LoadTags(filmId);
        }

        public async void Page_LoadWithPostback(object sender, EventArgs e)
        {
            if (isCreateAction)
                return;

            string target = Request["__EVENTTARGET"];
            string argument = Request["__EVENTARGUMENT"];
            string filmId = hdFilmId.Value;

            if (target == "CategorySelected_Click" && !string.IsNullOrEmpty(argument))
            {
                string categoryId = argument;
                await LoadCategories(filmId);
            }

            if(target == "TagSelected_Click" && !string.IsNullOrEmpty(argument))
            {
                string tagId = argument;
                await LoadTags(filmId);
            }
        }

        private async Task LoadFilm(string id)
        {
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

        private async Task LoadCategories(string filmId)
        {
            TaxonomyService taxonomyService = Inject<TaxonomyService>();
            List<CategoryDto> categories = await taxonomyService
                .GetCategoriesByFilmIdAsync(filmId);

            rptCategories.DataSource = categories;
            rptCategories.DataBind();
        }

        private async Task LoadTags(string filmId)
        {
            TaxonomyService taxonomyService = Inject<TaxonomyService>();
            List<CategoryDto> categories = await taxonomyService
                .GetCategoriesByFilmIdAsync(filmId);

            rptTags.DataSource = categories;
            rptTags.DataBind();
        }

        protected async void btnSubmit_Click(object sender, EventArgs e)
        {
            if (isCreateAction)
                await Create();
            else
                await Update();
        }
        
        protected void lnkCategoryDelete_Command(object sender, System.Web.UI.WebControls.CommandEventArgs e)
        {
            string categoryId = e.CommandArgument.ToString();

        }

        protected void lnkTagDelete_Command(object sender, System.Web.UI.WebControls.CommandEventArgs e)
        {
            string tagId = e.CommandArgument.ToString();

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