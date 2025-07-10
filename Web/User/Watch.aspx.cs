using Common;
using Common.Upload;
using Data.DTO;
using Data.Services;
using Ninject;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using Web.App_Start;
using Web.Models;

namespace Web.User
{
    public partial class Watch : System.Web.UI.Page
    {
        protected FilmDto film;
        protected string title_HeadTag;
        protected string keywords_MetaTag;
        protected string description_MetaTag;
        protected string hyplnkIncreaseView;

        protected async void Page_Load(object sender, EventArgs e)
        {
            try
            {
                hyplnkIncreaseView = GetRouteUrl("User_IncreaseView", null);
                await GetFilm();
                GenerateHeadTag();
                object obj = Session["userSession"];
                if (obj != null && film != null)
                {
                    UserSession userSession = (UserSession)obj;
                    if (userSession.Histories == null)
                    {
                        userSession.Histories = new List<History>();
                    }
                    userSession.Histories.Add(new History
                    {
                        filmId = film.ID,
                        name = film.Name,
                        thumbnail = film.Thumbnail,
                        url = film.Url,
                        timestamp = DateTime.Now
                    });
                }
            }
            catch (Exception ex)
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

        private async Task GetFilm()
        {
            string id = GetFilmId();
            if (id == null)
            {
                Response.RedirectToRoute("User_Home", null);
                return;
            }

            using (FilmService filmService = NinjectWebCommon.Kernel.Get<FilmService>())
            {
                ExecResult<FilmDto> resutl = await filmService.GetFilmAsync(id);
                if (resutl.Status == ExecStatus.Success)
                    film = resutl.Data;
            }

            if (film == null)
            {
                Response.RedirectToRoute("User_Home", null);
                return;
            }

            if (string.IsNullOrEmpty(film.Thumbnail))
                film.Thumbnail = VirtualPathUtility
                    .ToAbsolute(string.Format("{0}/Default/default.png", FileUpload.ImageFilePath));
            else
                film.Thumbnail = VirtualPathUtility
                    .ToAbsolute(string.Format("{0}/{1}", FileUpload.ImageFilePath, film.Thumbnail));

            if (string.IsNullOrEmpty(film.Source))
                film.Source = VirtualPathUtility
                    .ToAbsolute(string.Format("{0}/Default/default.mp4", FileUpload.VideoFilePath));
            else
                film.Source = VirtualPathUtility
                    .ToAbsolute(string.Format("{0}/{1}", FileUpload.VideoFilePath, film.Source));

            film.Url = GetRouteUrl("User_FilmDetail", new { slug = film.Name.TextToUrl(), id = film.ID });
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