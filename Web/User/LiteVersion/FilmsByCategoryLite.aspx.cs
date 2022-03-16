using Common.Upload;
using Data.BLL;
using Data.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using Web.Models;

namespace Web.User.LiteVersion
{
    public partial class FilmsByCategoryLite : System.Web.UI.Page
    {
        protected string categoryName;

        protected async void Page_Load(object sender, EventArgs e)
        {
            try
            {
                await LoadFilmsByCategoryId();
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }

        private int GetCategoryId()
        {
            object obj = Page.RouteData.Values["id"];
            if (obj == null)
                return -1;
            return int.Parse(obj.ToString());
        }

        private async Task LoadFilmsByCategoryId()
        {
            int id = GetCategoryId();
            if (id <= 0)
            {
                Response.RedirectToRoute("User_Home_Lite", null);
            }
            else
            {
                using(FilmBLL filmBLL = new FilmBLL())
                {
                    CategoryInfo categoryInfo = await new CategoryBLL(filmBLL).GetCategoryAsync(id);
                    if (categoryInfo == null)
                    {
                        Response.RedirectToRoute("User_Home_Lite", null);
                    }
                    else
                    {
                        categoryName = categoryInfo.name;
                        List<FilmInfo> filmInfos = await filmBLL.GetFilmsByCategoryIdAsync(id, 24);
                        foreach (FilmInfo filmInfo in filmInfos)
                        {
                            if (string.IsNullOrEmpty(filmInfo.thumbnail))
                                filmInfo.thumbnail = VirtualPathUtility
                                    .ToAbsolute(string.Format("{0}/Default/default.png", FileUpload.ImageFilePath));
                            else
                                filmInfo.thumbnail = VirtualPathUtility
                                    .ToAbsolute(string.Format("{0}/{1}", FileUpload.ImageFilePath, filmInfo.thumbnail));

                            filmInfo.url = GetRouteUrl("User_FilmDetail_Lite", new { slug = filmInfo.name.TextToUrl(), id = filmInfo.ID });
                        }

                        dlFilm.DataSource = filmInfos;
                        dlFilm.DataBind();
                    }
                }
            }
        }
    }
}