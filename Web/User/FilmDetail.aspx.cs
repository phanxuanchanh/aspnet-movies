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

namespace Web.User
{
    public partial class FilmDetail : System.Web.UI.Page
    {
        protected FilmDto filmInfo;
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
            }
            else
            {
                using(FilmBLL filmBLL = new FilmBLL())
                {
                    filmBLL.IncludeCategory = true;
                    filmBLL.IncludeTag = true;
                    filmBLL.IncludeCountry = true;
                    filmBLL.IncludeLanguage = true;
                    filmBLL.IncludeDirector = true;
                    filmBLL.IncludeCast = true;
                    filmInfo = await filmBLL.GetFilmAsync(id);
                }

                if(filmInfo == null)
                {
                    Response.RedirectToRoute("User_Home", null);
                }
                else
                {
                    Rating rating = new Rating(filmInfo.Upvote, filmInfo.Downvote);
                    filmInfo.StarRating = rating.SolveStar();
                    filmInfo.ScoreRating = rating.SolveScore();

                    if (string.IsNullOrEmpty(filmInfo.Thumbnail))
                        filmInfo.Thumbnail = VirtualPathUtility
                            .ToAbsolute(string.Format("{0}/Default/default.png", FileUpload.ImageFilePath));
                    else
                        filmInfo.Thumbnail = VirtualPathUtility
                            .ToAbsolute(string.Format("{0}/{1}", FileUpload.ImageFilePath, filmInfo.Thumbnail));

                    filmInfo.Url = GetRouteUrl("User_Watch", new { slug = filmInfo.Name.TextToUrl(), id = filmInfo.ID });
                }
            }
        }

        private void GenerateHeadTag()
        {
            if (filmInfo != null)
            {
                title_HeadTag = filmInfo.Name;
                description_MetaTag = (string.Format("{0}...", filmInfo.Description.TakeStr(100))).Replace("\n", " ");

                StringBuilder stringBuilder = new StringBuilder();
                foreach (TagDto tagInfo in filmInfo.Tags)
                {
                    stringBuilder.Append(string.Format("{0}, ", tagInfo.Name));
                }
                keywords_MetaTag = stringBuilder.ToString().TrimEnd(' ').TrimEnd(',');
            }
        }
    }
}