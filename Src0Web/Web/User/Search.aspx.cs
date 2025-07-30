using Common;
using Common.Upload;
using Data.BLL;
using Data.DTO;
using Data.Services;
using Ninject;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using Web.App_Start;
using Web.Models;

namespace Web.User
{
    public partial class Search : System.Web.UI.Page
    {
        protected List<FilmDto> films;
        protected string keyword;

        protected async void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string searchContent = Request.QueryString["input"];
                keyword = searchContent;
                await SearchFilms(searchContent);
            }catch(Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }

        public async Task SearchFilms(string searchContent)
        {
            if (string.IsNullOrEmpty(searchContent))
            {
                Response.RedirectToRoute("User_Home", null);
                return;
            }

            using (FilmService filmService = NinjectWebCommon.Kernel.Get<FilmService>())
            {
                films = new List<FilmDto>();// await filmBLL.SeachFilmsAsync(searchContent);
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