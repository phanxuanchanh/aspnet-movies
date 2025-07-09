using Common.Upload;
using Common.Web;
using Data.BLL;
using Data.DTO;
using Data.Services;
using Ninject;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Web.App_Start;
using Web.Models;

namespace Web.Admin.FilmManagement
{
    public partial class FilmList : System.Web.UI.Page
    {
        private FilmService _filmService;
        protected long currentPage;
        protected long pageNumber;
        protected bool enableTool;
        protected string toolDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            _filmService = NinjectWebCommon.Kernel.Get<FilmService>();
            enableTool = false;
            toolDetail = null;
            try
            {
                hyplnkCreate.NavigateUrl = GetRouteUrl("Admin_CreateFilm", null);

                if (!CheckLoggedIn())
                {
                    Response.RedirectToRoute("Account_Login", null);
                    return;
                }

                if (!IsPostBack)
                {
                    await SetGrvFilm();
                    SetDrdlPage();
                }
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            if (_filmService != null)
            {
                _filmService.Dispose();
                _filmService = null;
            }
        }

        private bool CheckLoggedIn()
        {
            object obj = Session["userSession"];
            if (obj == null)
                return false;

            UserSession userSession = (UserSession)obj;
            return (userSession.role == "Admin" || userSession.role == "Editor");
        }

        protected async void drdlPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                await SetGrvFilm();
                SetDrdlPage();
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }

        private async Task SetGrvFilm()
        {
            PagedList<FilmDto> films = await _filmService.GetFilmsAsync(drdlPage.SelectedIndex, 20);
            foreach(FilmDto film in films.Items)
            {
                if(string.IsNullOrEmpty(film.Thumbnail))
                    film.Thumbnail = VirtualPathUtility
                        .ToAbsolute(string.Format("{0}/Default/default.png", FileUpload.ImageFilePath));
                else
                    film.Thumbnail = VirtualPathUtility
                        .ToAbsolute(string.Format("{0}/{1}", FileUpload.ImageFilePath, film.Thumbnail));
            }
            grvFilm.DataSource = films.Items;
            grvFilm.DataBind();

            pageNumber = films.PageNumber;
            currentPage = films.CurrentPage;
        }

        private void SetDrdlPage()
        {
            int selectedIndex = drdlPage.SelectedIndex;
            drdlPage.Items.Clear();
            for (int i = 0; i < pageNumber; i++)
            {
                string item = (i + 1).ToString();
                if (i == currentPage)
                    drdlPage.Items.Add(string.Format("{0}*", item));
                else
                    drdlPage.Items.Add(item);
            }
            drdlPage.SelectedIndex = selectedIndex;
        }

        protected async void grvFilm_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string key = (string)grvFilm.DataKeys[grvFilm.SelectedIndex].Value;
                FilmDto filmInfo = (await _filmService.GetFilmAsync(key, false, false)).Data;
                toolDetail = string.Format("{0} -- {1}", filmInfo.ID, filmInfo.Name);
                hyplnkDetail.NavigateUrl = GetRouteUrl("Admin_FilmDetail", new { id = filmInfo.ID });
                hyplnkEdit_Category.NavigateUrl = GetRouteUrl("Admin_EditCategory_Film", new { id = filmInfo.ID });
                hyplnkEdit_Tag.NavigateUrl = GetRouteUrl("Admin_EditTag_Film", new { id = filmInfo.ID });
                hyplnkEdit_Director.NavigateUrl = GetRouteUrl("Admin_EditDirector_Film", new { id = filmInfo.ID });
                hyplnkEdit_Cast.NavigateUrl = GetRouteUrl("Admin_EditCast_Film", new { id = filmInfo.ID });
                hyplnkEdit_Image.NavigateUrl = GetRouteUrl("Admin_EditImage_Film", new { id = filmInfo.ID });
                hyplnkEdit_Source.NavigateUrl = GetRouteUrl("Admin_EditSource_Film", new { id = filmInfo.ID });
                hyplnkEdit.NavigateUrl = GetRouteUrl("Admin_UpdateFilm", new { id = filmInfo.ID });
                hyplnkDelete.NavigateUrl = GetRouteUrl("Admin_DeleteFilm", new { id = filmInfo.ID });
                enableTool = true;
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }
    }
}