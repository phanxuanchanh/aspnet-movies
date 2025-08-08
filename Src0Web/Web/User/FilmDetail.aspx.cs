using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using Data.DTO;
using Common.Upload;
using Common;
using System.Text;
using Data.Services;
using Web.Shared.Result;
using System.Security.Principal;

namespace Web.User
{
    public partial class FilmDetail : GeneralPage
    {
        private string id;
        protected FilmDto film;
        protected string title_HeadTag;
        protected string keywords_MetaTag;
        protected string description_MetaTag;
        protected string hyplnkUpvote;
        protected string hyplnkDownvote;
        protected string userName;

        protected async void Page_Load(object sender, EventArgs e)
        {
            hyplnkUpvote = GetRouteUrl("User_UpvoteFilm", null);
            hyplnkDownvote = GetRouteUrl("User_DownvoteFilm", null);

            userName = GetUserName();
            id = GetFilmId();

            if (id == null)
            {
                Response.ForceRedirectToRoute(this, "User_Home", null);
                return;
            }

            await GetFilmById();
            if (film == null)
            {
                Response.ForceRedirectToRoute(this, "User_Home", null);
                return;
            }

            GenerateHeadTag();
        }

        private string GetFilmId()
        {
            object obj = Page.RouteData.Values["id"];
            if (obj == null)
                return null;
            return obj.ToString();
        }

        private string GetUserName()
        {
            if (HttpContext.Current.User?.Identity?.IsAuthenticated != true)
                return null;

            IPrincipal principal = HttpContext.Current.User;
            return principal.Identity.Name;
        }

        private async Task GetFilmById()
        {
            FilmService filmService = Inject<FilmService>();

            ExecResult<FilmDto> result = await filmService.GetFilmAsync(id, includeMetadata: true, includeTaxonomy: true, includePeople: true);
            if (result.Status == ExecStatus.Success)
                film = result.Data;

            if (film == null)
                return;

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