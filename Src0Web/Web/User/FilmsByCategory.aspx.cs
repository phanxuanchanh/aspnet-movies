using Common;
using Common.Upload;
using Data.DTO;
using Data.Services;
using Ninject;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using Web.Shared.Result;
using Web.Shared.WebForms;

namespace Web.User
{
    public partial class FilmsByCategory : GeneralPage
    {
        [Inject]
        public FilmService FilmService { get; set; }

        [Inject]
        public TaxonomyService TaxonomyService { get; set; }

        protected List<FilmDto> films;
        protected string categoryName;

        protected async void Page_Load(object sender, EventArgs e)
        {
            await GetFilmsByCategoryId();
        }

        private int GetCategoryId()
        {
            object obj = Page.RouteData.Values["id"];
            if (obj == null)
                return -1;
            return int.Parse(obj.ToString());
        }

        private async Task GetFilmsByCategoryId()
        {
            int id = GetCategoryId();
            if (id <= 0)
            {
                Response.RedirectToRoute("User_Home", null);
                return;
            }

            CategoryDto category = null;

            ExecResult<CategoryDto> result = await TaxonomyService.GetCategoryAsync(id);
            if (result.Status == ExecStatus.Success)
                category = result.Data;

            if (category == null)
            {
                Response.RedirectToRoute("User_Home", null);
                return;
            }

            categoryName = category.Name;

            PagedList<FilmDto> filmResult = await FilmService.GetFilmsByCategoryIdAsync(id, 24);
            films = filmResult.Items;


            foreach (FilmDto film in films)
            {
                if (string.IsNullOrEmpty(film.Thumbnail))
                    film.Thumbnail = VirtualPathUtility
                        .ToAbsolute(string.Format("{0}/Default/default.png", FileUpload.ImageFilePath));
                else
                    film.Thumbnail = VirtualPathUtility
                        .ToAbsolute(string.Format("{0}/{1}", FileUpload.ImageFilePath, film.Thumbnail));

                Rating rating = new Rating(film.Upvote, film.Downvote);
                film.ScoreRating = rating.SolveScore();
                film.Url = GetRouteUrl("User_FilmDetail", new { slug = film.Name.TextToUrl(), id = film.ID });
            }
        }
    }
}