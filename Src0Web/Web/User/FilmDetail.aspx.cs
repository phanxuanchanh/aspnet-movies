using Data.BLL;
using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using Web.Models;
using Data.DTO;
using Common.Upload;
using Common;
using System.Text;
using Data.Services;
using Web.App_Start;
using Ninject;
using Web.Shared.Result;

namespace Web.User
{
    public partial class FilmDetail : System.Web.UI.Page
    {
        protected FilmDto film;
        protected string title_HeadTag;
        protected string keywords_MetaTag;
        protected string description_MetaTag;
        protected string hyplnkUpvote;
        protected string hyplnkDownvote;
        protected string userId;

        protected async void Page_Load(object sender, EventArgs e)
        {
            try
            {
                hyplnkUpvote = GetRouteUrl("User_UpvoteFilm", null);
                hyplnkDownvote = GetRouteUrl("User_DownvoteFilm", null);
                await GetFilmById();
                userId = GetUserId();
                GenerateHeadTag();
            }
            catch(Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }

        private string GetFilmId()
        {
            object obj = Page.RouteData.Values["id"];
            if (obj == null)
                return null;
            return obj.ToString();
        }

        private string GetUserId()
        {
            object obj = Session["userSession"];
            if (obj == null)
                return null;

            UserSession userSession = (UserSession)obj;
            return userSession.userId;
        }

        private async Task GetFilmById()
        {
            string id = GetFilmId();
            if (id == null)
            {
                Response.RedirectToRoute("User_Home", null);
                return;
            }

            using (FilmService filmService = NinjectWebCommon.Kernel.Get<FilmService>())
            {
                ExecResult<FilmDto> result = await filmService.GetFilmAsync(id, includeMetadata: true, includeTaxonomy: true, includePeople: true);
                if(result.Status == ExecStatus.Success)
                    film = result.Data;
            }

            if (film == null)
            {
                Response.RedirectToRoute("User_Home", null);
                return;
            }

            Rating rating = new Rating(film.Upvote, film.Downvote);
            film.StarRating = rating.SolveStar();
            film.ScoreRating = rating.SolveScore();

            if (string.IsNullOrEmpty(film.Thumbnail))
                film.Thumbnail = VirtualPathUtility
                    .ToAbsolute(string.Format("{0}/Default/default.png", FileUpload.ImageFilePath));
            else
                film.Thumbnail = VirtualPathUtility
                    .ToAbsolute(string.Format("{0}/{1}", FileUpload.ImageFilePath, film.Thumbnail));

            film.Url = GetRouteUrl("User_Watch", new { slug = film.Name.TextToUrl(), id = film.ID });
        }

        private void GenerateHeadTag()
        {
            if (film != null)
            {
                title_HeadTag = film.Name;
                description_MetaTag = (string.Format("{0}...", film.Description.TakeStr(100))).Replace("\n", " ");

                StringBuilder stringBuilder = new StringBuilder();
                foreach (TagDto tag in film.Tags)
                {
                    stringBuilder.Append(string.Format("{0}, ", tag.Name));
                }
                keywords_MetaTag = stringBuilder.ToString().TrimEnd(' ').TrimEnd(',');
            }
        }
    }
}