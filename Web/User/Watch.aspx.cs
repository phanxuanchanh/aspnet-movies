using Common.Upload;
using Data.BLL;
using Data.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using Web.Models;

namespace Web.User
{
    public partial class Watch : System.Web.UI.Page
    {
        protected FilmInfo filmInfo;
        protected string title_HeadTag;
        protected string keywords_MetaTag;
        protected string description_MetaTag;
        protected string hyplnkIncreaseView;

        protected async void Page_Load(object sender, EventArgs e)
        {
            try
            {
                hyplnkIncreaseView = GetRouteUrl("User_IncreaseView", null);
                await GetFilmInfo();
                GenerateHeadTag();
                object obj = Session["userSession"];
                if (obj != null && filmInfo != null)
                {
                    UserSession userSession = (UserSession)obj;
                    if (userSession.Histories == null)
                    {
                        userSession.Histories = new List<History>();
                    }
                    userSession.Histories.Add(new History
                    {
                        filmId = filmInfo.ID,
                        name = filmInfo.name,
                        thumbnail = filmInfo.thumbnail,
                        url = filmInfo.url,
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

        private async Task GetFilmInfo()
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
                    filmBLL.IncludeTag = true;
                    filmInfo = await filmBLL.GetFilmAsync(id);
                }

                if (filmInfo == null)
                {
                    Response.RedirectToRoute("User_Home", null);
                }
                else
                {
                    if (string.IsNullOrEmpty(filmInfo.thumbnail))
                        filmInfo.thumbnail = VirtualPathUtility
                            .ToAbsolute(string.Format("{0}/Default/default.png", FileUpload.ImageFilePath));
                    else
                        filmInfo.thumbnail = VirtualPathUtility
                            .ToAbsolute(string.Format("{0}/{1}", FileUpload.ImageFilePath, filmInfo.thumbnail));

                    if (string.IsNullOrEmpty(filmInfo.source))
                        filmInfo.source = VirtualPathUtility
                            .ToAbsolute(string.Format("{0}/Default/default.mp4", FileUpload.VideoFilePath));
                    else
                        filmInfo.source = VirtualPathUtility
                            .ToAbsolute(string.Format("{0}/{1}", FileUpload.VideoFilePath, filmInfo.source));

                    filmInfo.url = GetRouteUrl("User_FilmDetail", new { slug = filmInfo.name.TextToUrl(), id = filmInfo.ID });
                }
            }
        }

        private void GenerateHeadTag()
        {
            if (filmInfo != null)
            {
                title_HeadTag = filmInfo.name;
                description_MetaTag = (string.Format("{0}...", filmInfo.description.TakeStr(100))).Replace("\n", " ");

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