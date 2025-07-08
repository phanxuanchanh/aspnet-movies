using Data.BLL;
using Data.DTO;
using Data.Services;
using Ninject;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web.App_Start;
using Web.Models;

namespace Web.Admin.FilmManagement
{
    public partial class EditTag : System.Web.UI.Page
    {
        private FilmBLL filmBLL;
        protected string filmName;
        protected List<TagDto> tagsByFilmId;
        protected bool enableShowDetail;
        protected bool enableShowResult;
        protected string stateString;
        protected string stateDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            filmBLL = new FilmBLL();
            enableShowDetail = false;
            enableShowResult = false;
            stateString = null;
            stateDetail = null;
            try
            {
                string id = GetFilmId();
                hyplnkList.NavigateUrl = GetRouteUrl("Admin_FilmList", null);
                hyplnkDetail.NavigateUrl = GetRouteUrl("Admin_FilmDetail", new { id = id });
                hyplnkEdit_Category.NavigateUrl = GetRouteUrl("Admin_EditCategory_Film", new { id = id });
                hyplnkEdit_Director.NavigateUrl = GetRouteUrl("Admin_EditDirector_Film", new { id = id });
                hyplnkEdit_Cast.NavigateUrl = GetRouteUrl("Admin_EditCast_Film", new { id = id });
                hyplnkEdit_Image.NavigateUrl = GetRouteUrl("Admin_EditImage_Film", new { id = id });
                hyplnkEdit_Source.NavigateUrl = GetRouteUrl("Admin_EditSource_Film", new { id = id });
                hyplnkEdit.NavigateUrl = GetRouteUrl("Admin_UpdateFilm", new { id = id });
                hyplnkDelete.NavigateUrl = GetRouteUrl("Admin_DeleteFilm", new { id = id });

                if (CheckLoggedIn())
                {
                    await LoadTags();
                    if (!IsPostBack)
                    {
                        await LoadFilmInfo(id);
                        filmBLL.Dispose();
                    }
                }
                else
                {
                    Response.RedirectToRoute("Account_Login", null);
                    filmBLL.Dispose();
                }
            }
            catch (Exception ex)
            {
                filmBLL.Dispose();
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
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

        private string GetFilmId()
        {
            object obj = Page.RouteData.Values["id"];
            if (obj == null)
                return null;
            return obj.ToString();
        }

        private async Task LoadFilmInfo(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                Response.RedirectToRoute("Admin_FilmList", null);
            }
            else
            {
                filmBLL.IncludeTag = true;
                FilmInfo filmInfo = await filmBLL.GetFilmAsync(id);
                if (filmInfo == null)
                {
                    Response.RedirectToRoute("Admin_FilmList", null);
                }
                else
                {
                    enableShowDetail = true;
                    tagsByFilmId = filmInfo.Tags;
                    filmName = filmInfo.name;
                }
            }
        }

        private async Task LoadTags()
        {
            drdlFilmTag.Items.Clear();
            using(TaxonomyService taxonomyService = NinjectWebCommon.Kernel.Get<TaxonomyService>())
            {
                List<TagDto> tags = (await taxonomyService.GetTagsAsync(1, 30)).Items;
                foreach (TagDto tag in tags)
                {
                    drdlFilmTag.Items.Add(new ListItem(tag.Name, tag.ID.ToString()));
                }

                drdlFilmTag.SelectedIndex = 0;
            }
        }

        protected async void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string filmId = GetFilmId();
                string strTagId = Request.Form[drdlFilmTag.UniqueID];
                if (strTagId == null)
                {
                    Response.RedirectToRoute("Admin_FilmList", null);
                }
                else
                {
                    int tagId = int.Parse(strTagId);
                    CreationState state = await filmBLL.AddTagAsync(filmId, tagId);
                    await LoadFilmInfo(filmId);
                    if (state == CreationState.Success)
                    {
                        stateString = "Success";
                        stateDetail = "Đã thêm thẻ tag vào phim thành công";
                    }
                    else if (state == CreationState.AlreadyExists)
                    {
                        stateString = "AlreadyExists";
                        stateDetail = "Thêm thẻ tag vào phim thất bại. Lý do: Đã tồn tại thẻ tag trong phim này";
                    }
                    else
                    {
                        stateString = "Failed";
                        stateDetail = "Thêm thẻ tag vào phim thất bại";
                    }
                    enableShowResult = true;
                }
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
            filmBLL.Dispose();
        }

        protected async void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string filmId = GetFilmId();
                DeletionState state = await filmBLL.DeleteAllTagAsync(filmId);
                await LoadFilmInfo(filmId);
                if (state == DeletionState.Success)
                {
                    stateString = "Success";
                    stateDetail = "Đã xóa tất cả thẻ tag của phim thành công";
                }
                else
                {
                    stateString = "Failed";
                    stateDetail = "Xóa tất cả thẻ tag của phim thất bại";
                }
                enableShowResult = true;
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
            filmBLL.Dispose();
        }
    }
}