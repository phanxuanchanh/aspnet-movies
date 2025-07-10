using Data.BLL;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using Data.DTO;
using Common.Upload;
using Web.Models;
using System.Linq;
using Common;
using Data.Services;
using Web.App_Start;
using Ninject;
using Common.Web;

namespace Web.User
{
    public partial class Index : System.Web.UI.Page
    {
        protected List<FilmDto> latestFilms;
        protected List<CategoryDto> categories;
        protected Dictionary<CategoryDto, List<FilmDto>> films_CategoryDict;
        protected string hyplnkCategoryList;

        protected async void Page_Load(object sender, EventArgs e)
        {
            films_CategoryDict = new Dictionary<CategoryDto, List<FilmDto>>();
            try
            {
                hyplnkCategoryList = GetRouteUrl("User_CategoryList", null);
                await GetLatestFilm();
                await GetCategories();
                await GetFilmsByCategory();
            }
            catch(Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }

        private async Task GetLatestFilm()
        {
            await Task.CompletedTask;
            latestFilms = new List<FilmDto>();// await filmBLL.GetLatestFilmAsync();
            foreach (FilmDto filmInfo in latestFilms)
            {
                if (string.IsNullOrEmpty(filmInfo.Thumbnail))
                    filmInfo.Thumbnail = VirtualPathUtility
                        .ToAbsolute(string.Format("{0}/Default/default.png", FileUpload.ImageFilePath));
                else
                    filmInfo.Thumbnail = VirtualPathUtility
                        .ToAbsolute(string.Format("{0}/{1}", FileUpload.ImageFilePath, filmInfo.Thumbnail));

                Rating rating = new Rating(filmInfo.Upvote, filmInfo.Downvote);
                filmInfo.ScoreRating = rating.SolveScore();
                filmInfo.Url = GetRouteUrl("User_FilmDetail", new { slug = filmInfo.Name.TextToUrl(), id = filmInfo.ID });
            }
        }

        private async Task GetCategories()
        {
            using (TaxonomyService taxonomyService = NinjectWebCommon.Kernel.Get<TaxonomyService>())
            {
                categories = (await taxonomyService.GetCategoriesAsync(1, 30)).Items
                    .Select(s =>
                    {
                        s.Url = GetRouteUrl("User_FilmsByCategory", new { slug = s.Name.TextToUrl(), id = s.ID });
                        return s;
                    }).ToList();
            }
        }

        private async Task GetFilmsByCategory()
        {
            foreach(CategoryDto category in categories)
            {

                List<FilmDto> films = null;
                using(FilmService filmBLL = NinjectWebCommon.Kernel.Get<FilmService>())
                {
                    PagedList<FilmDto> result = await filmBLL.GetFilmsByCategoryIdAsync(category.ID, 24);
                    films = result.Items;
                }

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
                films_CategoryDict.Add(category, films);
            }
        }
    }
}