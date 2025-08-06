using Common;
using Common.Upload;
using Data.DTO;
using Data.Services;
using MediaSrv;
using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;

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
            string id = GetFilmId();
            hyplnkList.NavigateUrl = GetRouteUrl("Admin_FilmList", null);
            hyplnkEdit.NavigateUrl = GetRouteUrl("Admin_EditFilm", new { id = id, action = "update" });

            await GetFilm(id);

            //MediaServiceWrapper serviceWrapper = Inject<MediaServiceWrapper>();
            //var a = await serviceWrapper.GetDefaultImageAsync();
            //string url = a.Url;
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

            FilmService filmService = Inject<FilmService>();

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

        protected async void btnDelete_Click(object sender, EventArgs e)
        {
            await DeleteFilm();
        }

        private async Task DeleteFilm()
        {
            string id = GetFilmId();
            if (string.IsNullOrEmpty(id))
            {
                Response.RedirectToRoute("Admin_CategoryList", null);
                return;
            }

            FilmService filmService = Inject<FilmService>();

            commandResult = await filmService.DeleteAsync(id); ;
            if (commandResult.Status == ExecStatus.Success)
            {
                Response.RedirectToRoute("Admin_CategoryList", null);
                return;
            }
        }
    }
}