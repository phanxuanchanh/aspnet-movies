using Common;
using Common.Upload;
using Data.BLL;
using Data.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using Web.Models;

namespace Web.User
{
    public partial class FilmsByCategory : System.Web.UI.Page
    {
        private FilmBLL filmBLL;
        protected List<FilmInfo> filmInfos;
        protected string categoryName;

        protected async void Page_Load(object sender, EventArgs e)
        {
            filmBLL = new FilmBLL();
            try
            {
                await GetFilmsByCategoryId();
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
            filmBLL.Dispose();
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
            }
            else
            {
                CategoryDto categoryInfo = new CategoryDto();//  await new CategoryBLL(filmBLL).GetCategoryAsync(id);
                if(categoryInfo == null)
                {
                    Response.RedirectToRoute("User_Home", null);
                }
                else
                {
                    categoryName = categoryInfo.Name;
                    filmInfos = await filmBLL.GetFilmsByCategoryIdAsync(id, 24);
                    foreach (FilmInfo filmInfo in filmInfos)
                    {
                        if (string.IsNullOrEmpty(filmInfo.thumbnail))
                            filmInfo.thumbnail = VirtualPathUtility
                                .ToAbsolute(string.Format("{0}/Default/default.png", FileUpload.ImageFilePath));
                        else
                            filmInfo.thumbnail = VirtualPathUtility
                                .ToAbsolute(string.Format("{0}/{1}", FileUpload.ImageFilePath, filmInfo.thumbnail));

                        Rating rating = new Rating(filmInfo.upvote, filmInfo.downvote);
                        filmInfo.scoreRating = rating.SolveScore();
                        filmInfo.url = GetRouteUrl("User_FilmDetail", new { slug = filmInfo.name.TextToUrl(), id = filmInfo.ID });
                    }
                }
            }
        }
    }
}