using Common;
using Common.Upload;
using Data.BLL;
using Data.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using Web.Models;

namespace Web.User
{
    public partial class Search : System.Web.UI.Page
    {
        protected List<FilmDto> filmInfos;
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
            }
            else
            {
                using(FilmBLL filmBLL = new FilmBLL())
                {
                    filmInfos = await filmBLL.SeachFilmsAsync(searchContent);
                }

                foreach(FilmDto filmInfo in filmInfos)
                {
                    if (string.IsNullOrEmpty(filmInfo.Thumbnail))
                        filmInfo.Thumbnail = VirtualPathUtility
                            .ToAbsolute(string.Format("{0}/Default/default.png", FileUpload.ImageFilePath));
                    else
                        filmInfo.Thumbnail = VirtualPathUtility
                            .ToAbsolute(string.Format("{0}/{1}", FileUpload.ImageFilePath, filmInfo.Thumbnail));

                    Rating rating = new Rating(filmInfo.Upvote, filmInfo.Downvote);
                    filmInfo.ScoreRating = rating.SolveScore();
                    filmInfo.Url = GetRouteUrl("User_FilmDetail", new { slug = filmInfo.Name.TextToUrl(), id = filmInfo.ID });
                }
            }
        }
    }
}