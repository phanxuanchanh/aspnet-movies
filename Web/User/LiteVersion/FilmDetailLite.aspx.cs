using Common.Upload;
using Data.BLL;
using Data.DTO;
using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using Web.Models;

namespace Web.User.LiteVersion
{
    public partial class FilmDetailLite : System.Web.UI.Page
    {
        protected FilmInfo filmInfo;
        protected async void Page_Load(object sender, EventArgs e)
        {
            try
            {
                await LoadFilmById();
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

        private async Task LoadFilmById()
        {
            string id = GetFilmId();
            if (id == null)
            {
                Response.RedirectToRoute("User_Home", null);
            }
            else
            {
                using (FilmBLL filmBLL = new FilmBLL())
                {
                    filmBLL.IncludeCategory = true;
                    filmBLL.IncludeCountry = true;
                    filmBLL.IncludeLanguage = true;
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

                    filmInfo.url = GetRouteUrl("User_Watch_Lite", new { slug = filmInfo.name.TextToUrl(), id = filmInfo.ID });
                }
            }
        }
    }
}