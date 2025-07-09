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

namespace Web.User
{
    public partial class Index : System.Web.UI.Page
    {
        private FilmBLL filmBLL;
        protected List<FilmDto> latestFilms;
        protected List<CategoryDto> categories;
        protected Dictionary<CategoryDto, List<FilmDto>> films_CategoryDict;
        protected string hyplnkCategoryList;

        protected async void Page_Load(object sender, EventArgs e)
        {
            filmBLL = new FilmBLL();
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
            filmBLL.Dispose();
        }

        private async Task GetLatestFilm()
        {
            filmBLL.IncludeCategory = true;
            latestFilms = await filmBLL.GetLatestFilmAsync();
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
            filmBLL.IncludeCategory = false;
            foreach(CategoryDto categoryInfo in categories)
            {
                List<FilmDto> filmInfos = await filmBLL.GetFilmsByCategoryIdAsync(categoryInfo.ID);
                foreach (FilmDto filmInfo in filmInfos)
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
                films_CategoryDict.Add(categoryInfo, filmInfos);
            }
        }
    }
}