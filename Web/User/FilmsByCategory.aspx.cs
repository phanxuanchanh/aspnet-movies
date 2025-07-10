using Common;
using Common.Upload;
using Common.Web;
using Data.DTO;
using Data.Services;
using Ninject;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using Web.App_Start;
using Web.Models;

namespace Web.User
{
    public partial class FilmsByCategory : System.Web.UI.Page
    {
        protected List<FilmDto> films;
        protected string categoryName;

        protected async void Page_Load(object sender, EventArgs e)
        {
            try
            {
                await GetFilmsByCategoryId();
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
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
            using (TaxonomyService taxonomyService = NinjectWebCommon.Kernel.Get<TaxonomyService>())
            {
                ExecResult<CategoryDto> result = await taxonomyService.GetCategoryAsync(id);
                if (result.Status == ExecStatus.Success)
                    category = result.Data;
            }

            if (category == null)
            {
                Response.RedirectToRoute("User_Home", null);
                return;
            }

            categoryName = category.Name;
            using (FilmService filmService = NinjectWebCommon.Kernel.Get<FilmService>())
            {
                PagedList<FilmDto> result = await filmService.GetFilmsByCategoryIdAsync(id, 24);
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
        }
    }
}