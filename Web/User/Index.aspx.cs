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

namespace Web.User
{
    public partial class Index : System.Web.UI.Page
    {
        private FilmBLL filmBLL;
        protected List<FilmInfo> latestFilms;
        protected List<CategoryInfo> categoryInfos;
        protected Dictionary<CategoryInfo, List<FilmInfo>> films_CategoryDict;
        protected string hyplnkCategoryList;

        protected async void Page_Load(object sender, EventArgs e)
        {
            filmBLL = new FilmBLL();
            films_CategoryDict = new Dictionary<CategoryInfo, List<FilmInfo>>();
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
            foreach (FilmInfo filmInfo in latestFilms)
            {
                if (string.IsNullOrEmpty(filmInfo.thumbnail))
                    filmInfo.thumbnail = VirtualPathUtility
                        .ToAbsolute(string.Format("{0}/Default/default.png", FileUpload.ImageFilePath));
                else
                    filmInfo.thumbnail = VirtualPathUtility
                        .ToAbsolute(string.Format("{0}/{1}", FileUpload.ImageFilePath, filmInfo.thumbnail));

                Rating rating = new Rating(filmInfo.upvote, filmInfo.downvote);
                filmInfo.scoreRating = rating.SolveScore();
                filmInfo.url = GetRouteUrl("User_FilmDetail", new { slug = filmInfo.name.TextToUrl(), id = filmInfo.ID });
            }
        }

        private async Task GetCategories()
        {
            categoryInfos = (await new CategoryBLL(filmBLL).GetCategoriesAsync())
                .Select(c => new CategoryInfo
                {
                    ID = c.ID,
                    Name = c.Name,
                    Description = c.Description,
                    Url = GetRouteUrl("User_FilmsByCategory", new { slug = c.Name.TextToUrl(), id = c.ID })
                }).ToList();
        }

        private async Task GetFilmsByCategory()
        {
            filmBLL.IncludeCategory = false;
            foreach(CategoryInfo categoryInfo in categoryInfos)
            {
                List<FilmInfo> filmInfos = await filmBLL.GetFilmsByCategoryIdAsync(categoryInfo.ID);
                foreach (FilmInfo filmInfo in filmInfos)
                {
                    if (string.IsNullOrEmpty(filmInfo.thumbnail))
                        filmInfo.thumbnail = VirtualPathUtility
                            .ToAbsolute(string.Format("{0}/Default/default.png", FileUpload.ImageFilePath));
                    else
                        filmInfo.thumbnail = VirtualPathUtility
                            .ToAbsolute(string.Format("{0}/{1}", FileUpload.ImageFilePath, filmInfo.thumbnail));

                    Rating rating = new Rating(filmInfo.upvote, filmInfo.downvote);
                    filmInfo.scoreRating = rating.SolveScore();
                    filmInfo.url = GetRouteUrl("User_FilmDetail", new { slug = filmInfo.name.TextToUrl(), id = filmInfo.ID });
                }
                films_CategoryDict.Add(categoryInfo, filmInfos);
            }
        }
    }
}