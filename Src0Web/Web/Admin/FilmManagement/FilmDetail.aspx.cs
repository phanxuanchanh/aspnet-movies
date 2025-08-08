using Common;
using Common.Upload;
using Data.DTO;
using Data.Services;
using MediaSrv;
using Ninject;
using System;
using System.Threading.Tasks;
using System.Web;
using Web.Shared.Result;

namespace Web.Admin.FilmManagement
{
    public partial class FilmDetail : AdminPage
    {
        [Inject]
        public FilmService FilmService { get; set; }

        private string id;
        protected FilmDto film;

        protected async void Page_Load(object sender, EventArgs e)
        {
            id = GetId<string>();
            if (string.IsNullOrEmpty(id))
            {
                Response.RedirectToRoute("Admin_FilmList", null);
                return;
            }

            hyplnkList.NavigateUrl = GetRouteUrl("Admin_FilmList", null);
            hyplnkEdit.NavigateUrl = GetRouteUrl("Admin_EditFilm", new { id = id, action = "update" });

            await GetFilm(id);

            //MediaServiceWrapper serviceWrapper = Inject<MediaServiceWrapper>();
            //var a = await serviceWrapper.GetDefaultImageAsync();
            //string url = a.Url;
        }

        private async Task GetFilm(string id)
        {
            ExecResult<FilmDto> result = await FilmService.GetFilmAsync(id, includeMetadata: true, includeTaxonomy: true);
            if (result.Status == ExecStatus.Success)
            {
                film = result.Data;

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
            ExecResult commandResult = await FilmService.DeleteAsync(id); ;
            if (commandResult.Status == ExecStatus.Success)
            {
                Response.RedirectToRoute("Admin_FilmList", null);
                return;
            }
            else
            {
                notifControl.Set(commandResult);
            }
        }
    }
}