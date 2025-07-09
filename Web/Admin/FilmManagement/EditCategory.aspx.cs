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
    public partial class EditCategory : System.Web.UI.Page
    {
        private FilmBLL filmBLL;
        protected string filmName;
        protected List<CategoryDto> categoriesByFilmId;
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
                hyplnkEdit_Tag.NavigateUrl = GetRouteUrl("Admin_EditTag_Film", new { id = id });
                hyplnkEdit_Director.NavigateUrl = GetRouteUrl("Admin_EditDirector_Film", new { id = id });
                hyplnkEdit_Cast.NavigateUrl = GetRouteUrl("Admin_EditCast_Film", new { id = id });
                hyplnkEdit_Image.NavigateUrl = GetRouteUrl("Admin_EditImage_Film", new { id = id });
                hyplnkEdit_Source.NavigateUrl = GetRouteUrl("Admin_EditSource_Film", new { id = id });
                hyplnkEdit.NavigateUrl = GetRouteUrl("Admin_UpdateFilm", new { id = id });
                hyplnkDelete.NavigateUrl = GetRouteUrl("Admin_DeleteFilm", new { id = id });

                if (CheckLoggedIn())
                {
                    await LoadCategories();
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
                filmBLL.IncludeCategory = true;
                FilmDto filmInfo = await filmBLL.GetFilmAsync(id);
                if(filmInfo == null)
                {
                    Response.RedirectToRoute("Admin_FilmList", null);
                }
                else
                {
                    enableShowDetail = true;
                    categoriesByFilmId = filmInfo.Categories;
                    filmName = filmInfo.Name;
                }
            }
        }

        private async Task LoadCategories()
        {
            drdlFilmCategory.Items.Clear();
            using (TaxonomyService taxonomyService = NinjectWebCommon.Kernel.Get<TaxonomyService>())
            {
                List<CategoryDto> categories = (await taxonomyService.GetCategoriesAsync(1, 30)).Items;
                foreach (CategoryDto category in categories)
                {
                    drdlFilmCategory.Items.Add(new ListItem(category.Name, category.ID.ToString()));
                }
                drdlFilmCategory.SelectedIndex = 0;
            }
        }

        protected async void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string filmId = GetFilmId();
                string strCategoryId = Request.Form[drdlFilmCategory.UniqueID];
                if (strCategoryId == null)
                {
                    Response.RedirectToRoute("Admin_FilmList", null);
                }
                else
                {
                    int categoryId = int.Parse(strCategoryId);
                    CreationState state = await filmBLL.AddCategoryAsync(filmId, categoryId);
                    await LoadFilmInfo(filmId);
                    if (state == CreationState.Success)
                    {
                        stateString = "Success";
                        stateDetail = "Đã thêm thể loại vào phim thành công";
                    }
                    else if (state == CreationState.AlreadyExists)
                    {
                        stateString = "AlreadyExists";
                        stateDetail = "Thêm thể loại vào phim thất bại. Lý do: Đã tồn tại thể loại trong phim này";
                    }
                    else
                    {
                        stateString = "Failed";
                        stateDetail = "Thêm thể loại vào phim thất bại";
                    }
                    enableShowResult = true;
                }
            }
            catch(Exception ex)
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
                DeletionState state = await filmBLL.DeleteAllCategoryAsync(filmId);
                await LoadFilmInfo(filmId);
                if (state == DeletionState.Success)
                {
                    stateString = "Success";
                    stateDetail = "Đã xóa tất cả thể loại của phim thành công";
                }
                else
                {
                    stateString = "Failed";
                    stateDetail = "Xóa tất cả thể loại của phim thất bại";
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