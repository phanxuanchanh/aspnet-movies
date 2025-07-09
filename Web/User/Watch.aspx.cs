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
        protected FilmDto filmInfo;
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
                        name = filmInfo.Name,
                        thumbnail = filmInfo.Thumbnail,
                        url = filmInfo.Url,
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
                    if (string.IsNullOrEmpty(filmInfo.Thumbnail))
                        filmInfo.Thumbnail = VirtualPathUtility
                            .ToAbsolute(string.Format("{0}/Default/default.png", FileUpload.ImageFilePath));
                    else
                        filmInfo.Thumbnail = VirtualPathUtility
                            .ToAbsolute(string.Format("{0}/{1}", FileUpload.ImageFilePath, filmInfo.Thumbnail));

                    if (string.IsNullOrEmpty(filmInfo.Source))
                        filmInfo.Source = VirtualPathUtility
                            .ToAbsolute(string.Format("{0}/Default/default.mp4", FileUpload.VideoFilePath));
                    else
                        filmInfo.Source = VirtualPathUtility
                            .ToAbsolute(string.Format("{0}/{1}", FileUpload.VideoFilePath, filmInfo.Source));

                    filmInfo.Url = GetRouteUrl("User_FilmDetail", new { slug = filmInfo.Name.TextToUrl(), id = filmInfo.ID });
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