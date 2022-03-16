using Common.Upload;
using Data.BLL;
using Data.DTO;
using System;
using System.Collections.Generic;
using System.Web;
using Web.Models;

namespace Web.User.LiteVersion
{
    public partial class IndexLite : System.Web.UI.Page
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            try
            {
                List<FilmInfo> latestFilms;
                using (FilmBLL filmBLL = new FilmBLL())
                {
                    latestFilms = await filmBLL.GetLatestFilmAsync();
                }

                foreach (FilmInfo filmInfo in latestFilms)
                {
                    if (string.IsNullOrEmpty(filmInfo.thumbnail))
                        filmInfo.thumbnail = VirtualPathUtility
                            .ToAbsolute(string.Format("{0}/Default/default.png", FileUpload.ImageFilePath));
                    else
                        filmInfo.thumbnail = VirtualPathUtility
                            .ToAbsolute(string.Format("{0}/{1}", FileUpload.ImageFilePath, filmInfo.thumbnail));

                    filmInfo.url = GetRouteUrl("User_FilmDetail_Lite", new { slug = filmInfo.name.TextToUrl(), id = filmInfo.ID });
                }

                dlFilm.DataSource = latestFilms;
                dlFilm.DataBind();
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }
    }
}