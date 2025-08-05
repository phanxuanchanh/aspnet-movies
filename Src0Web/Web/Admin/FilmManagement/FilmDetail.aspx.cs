using Common;
using Common.Upload;
using Data.DTO;
using Data.Services;
using Ninject;
using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using Web.App_Start;
using Web.Models;

namespace Web.Admin.FilmManagement
{
    public partial class FilmDetail : AdminPage
    {
        protected FilmDto film;
        protected ExecResult commandResult;
        protected bool enableShowDetail;
        protected bool enableShowResult;

        protected async void Page_Load(object sender, EventArgs e)
        {
            enableShowDetail = false;
            try
            {
                string id = GetFilmId();
                hyplnkList.NavigateUrl = GetRouteUrl("Admin_FilmList", null);
                hyplnkEdit_Category.NavigateUrl = GetRouteUrl("Admin_EditCategory_Film", new { id = id });
                hyplnkEdit_Tag.NavigateUrl = GetRouteUrl("Admin_EditTag_Film", new { id = id });
                hyplnkEdit_Cast.NavigateUrl = GetRouteUrl("Admin_EditCast_Film", new { id = id });
                hyplnkEdit_Director.NavigateUrl = GetRouteUrl("Admin_EditDirector_Film", new { id = id });
                hyplnkEdit_Image.NavigateUrl = GetRouteUrl("Admin_EditImage_Film", new { id = id });
                hyplnkEdit_Source.NavigateUrl = GetRouteUrl("Admin_EditSource_Film", new { id = id });
                hyplnkEdit.NavigateUrl = GetRouteUrl("Admin_UpdateFilm", new { id = id });

                await GetFilm(id);
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

        private async Task GetFilm(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                Response.RedirectToRoute("Admin_FilmList", null);
                return;
            }

            using (FilmService filmService = NinjectWebCommon.Kernel.Get<FilmService>())
            {
                ExecResult<FilmDto> result = await filmService.GetFilmAsync(id, includeMetadata: true, includeTaxonomy: true);
                if (result.Status == ExecStatus.Success)
                {
                    film = result.Data;
                    enableShowDetail = true;

                    if (string.IsNullOrEmpty(film.Thumbnail))
                        film.Thumbnail = VirtualPathUtility
                            .ToAbsolute(string.Format("{0}/Default/default.png", FileUpload.ImageFilePath));
                    else
                        film.Thumbnail = VirtualPathUtility
                            .ToAbsolute(string.Format("{0}/{1}", FileUpload.ImageFilePath, film.Thumbnail));
                }
                else
                {
                    Response.RedirectToRoute("Admin_FilmList", null);
                }

            }
        }

        protected async void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                await DeleteFilm();
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }

        private async Task DeleteFilm()
        {
            string id = GetFilmId();
            if (string.IsNullOrEmpty(id))
            {
                Response.RedirectToRoute("Admin_CategoryList", null);
                return;
            }

            using (FilmService filmService = NinjectWebCommon.Kernel.Get<FilmService>())
            {
                commandResult = await filmService.DeleteAsync(id); ;
                if (commandResult.Status == ExecStatus.Success)
                {
                    Response.RedirectToRoute("Admin_CategoryList", null);
                    return;
                }
            }
        }
    }
}