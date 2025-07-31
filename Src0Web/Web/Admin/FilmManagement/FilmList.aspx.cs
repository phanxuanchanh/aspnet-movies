using Common;
using Common.Upload;
using Common.Web;
using Data.DTO;
using Data.Services;
using Ninject;
using System;
using System.Threading.Tasks;
using System.Web;
using Web.App_Start;

namespace Web.Admin.FilmManagement
{
    public partial class FilmList : AdminPage
    {
        private FilmService _filmService;
        protected PagedList<FilmDto> paged;

        protected async void Page_Load(object sender, EventArgs e)
        {
            _filmService = NinjectWebCommon.Kernel.Get<FilmService>();
            hyplnkCreate.NavigateUrl = GetRouteUrl("Admin_EditFilm", new { action = "create" });
            paged = new PagedList<FilmDto>();
            if (!CheckLoggedIn())
            {
                Response.RedirectToRoute("Account_Login", null);
                return;
            }

            GetPagnationQuery();  

            if (!IsPostBack)
            {
                txtPageSize.Text = paged.PageSize.ToString();
                await SetFilmTable();
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

        private void GetPagnationQuery()
        {
            string pageQuery = Request.QueryString["page"];
            string sizeQuery = Request.QueryString["pageSize"];

            if (string.IsNullOrEmpty(pageQuery))
                paged.CurrentPage = 1;
            else
            {
                if (long.TryParse(pageQuery, out long page))
                    paged.CurrentPage = Math.Max(1, page);
                else
                    paged.CurrentPage = 1;
            }

            if (string.IsNullOrEmpty(sizeQuery))
                paged.PageSize = 20;
            else
            {
                if (long.TryParse(sizeQuery, out long size))
                    paged.PageSize = Math.Max(1, size);
                else
                    paged.PageSize = 20;
            }
        }

        private async Task SetFilmTable()
        {
            paged = await _filmService.GetFilmsAsync(paged.CurrentPage, paged.PageSize);
            foreach (FilmDto film in paged.Items)
            {
                if (string.IsNullOrEmpty(film.Thumbnail))
                    film.Thumbnail = VirtualPathUtility
                        .ToAbsolute(string.Format("{0}/Default/default.png", FileUpload.ImageFilePath));
                else
                    film.Thumbnail = VirtualPathUtility
                        .ToAbsolute(string.Format("{0}/{1}", FileUpload.ImageFilePath, film.Thumbnail));
            }

            rptFilms.DataSource = paged.Items;
            rptFilms.DataBind();

            pagination.SetAndRender(paged);
            pagination.SetUrl("Admin_FilmList");
        }

        protected async void lnkDelete_Command(object sender, System.Web.UI.WebControls.CommandEventArgs e)
        {
            string id = e.CommandArgument.ToString();
            if (string.IsNullOrEmpty(id))
                return;

            ExecResult commandResult = await _filmService.DeleteAsync(id);
            notifControl.Set(commandResult);
        }

        protected void txtPageSize_TextChanged(object sender, EventArgs e)
        {
            if (long.TryParse(txtPageSize.Text, out long size))
                Response.RedirectToRoute("Admin_FilmList", new { page = paged.CurrentPage, pageSize = paged.PageSize });
            else
                notifControl.Set(ExecResult.Failure("Invalid page size."));
        }

        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtSearch.Text.Trim();
            Response.RedirectToRoute("Admin_FilmList", new { page = paged.CurrentPage, pageSize = paged.PageSize, searchText });
        }
    }
}