using Data.BLL;
using Data.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web.Models;
using Web.Validation;

namespace Web.Admin.FilmManagement
{
    public partial class EditDirector : System.Web.UI.Page
    {
        private FilmBLL filmBLL;
        private CustomValidation customValidation;
        protected string filmName;
        protected List<DirectorDto> directorsByFilmId;
        protected bool enableShowDetail;
        protected bool enableShowResult;
        protected string stateString;
        protected string stateDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            filmBLL = new FilmBLL();
            customValidation = new CustomValidation();
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
                hyplnkEdit_Tag.NavigateUrl = GetRouteUrl("Admin_EditTag_Film", new { id = id });
                hyplnkEdit_Cast.NavigateUrl = GetRouteUrl("Admin_EditCast_Film", new { id = id });
                hyplnkEdit_Image.NavigateUrl = GetRouteUrl("Admin_EditImage_Film", new { id = id });
                hyplnkEdit_Source.NavigateUrl = GetRouteUrl("Admin_EditSource_Film", new { id = id });
                hyplnkEdit.NavigateUrl = GetRouteUrl("Admin_UpdateFilm", new { id = id });
                hyplnkDelete.NavigateUrl = GetRouteUrl("Admin_DeleteFilm", new { id = id });
                InitValidation();

                if (CheckLoggedIn())
                {
                    await LoadDirectors();
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

        private void InitValidation()
        {
            customValidation.Init(
                cvFilmDirector_Role,
                "txtFilmDirector_Role",
                "Tên vai trò của đạo điễn không hợp lệ",
                true,
                null,
                customValidation.ValidateDirectorRole
            );
        }

        private void ValidateData()
        {
            cvFilmDirector_Role.Validate();
        }

        private bool IsValidData()
        {
            ValidateData();
            return cvFilmDirector_Role.IsValid;
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
                filmBLL.IncludeDirector = true;
                FilmDto filmInfo = await filmBLL.GetFilmAsync(id);
                if (filmInfo == null)
                {
                    Response.RedirectToRoute("Admin_FilmList", null);
                }
                else
                {
                    enableShowDetail = true;
                    directorsByFilmId = filmInfo.Directors;
                    filmName = filmInfo.Name;
                }
            }
        }

        private async Task LoadDirectors()
        {
            drdlFilmDirector.Items.Clear();
            List<DirectorDto> directorInfos = new List<DirectorDto>();// await new DirectorBLL(filmBLL).GetDirectorsAsync();
            foreach (DirectorDto directorInfo in directorInfos)
            {
                drdlFilmDirector.Items.Add(new ListItem(directorInfo.Name, directorInfo.ID.ToString()));
            }
            drdlFilmDirector.SelectedIndex = 0;
        }

        protected async void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string filmId = GetFilmId();
                if (IsValidData())
                {
                    string strDirectorId = Request.Form[drdlFilmDirector.UniqueID];
                    string directorRole = Request.Form[txtFilmDirector_Role.UniqueID];
                    if (strDirectorId == null)
                    {
                        Response.RedirectToRoute("Admin_FilmList", null);
                    }
                    else
                    {
                        long directorId = long.Parse(strDirectorId);
                        CreationState state = await filmBLL.AddDirectorAsync(filmId, directorId, directorRole);
                        if (state == CreationState.Success)
                        {
                            stateString = "Success";
                            stateDetail = "Đã thêm đạo diễn vào phim thành công";
                        }
                        else if (state == CreationState.AlreadyExists)
                        {
                            stateString = "AlreadyExists";
                            stateDetail = "Thêm đạo diễn vào phim thất bại. Lý do: Đã tồn tại đạo diễn trong phim này";
                        }
                        else
                        {
                            stateString = "Failed";
                            stateDetail = "Thêm đạo diễn vào phim thất bại";
                        }
                        enableShowResult = true;
                    }
                }
                await LoadFilmInfo(filmId);
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
                DeletionState state = await filmBLL.DeleteAllDirectorAsync(filmId);
                await LoadFilmInfo(filmId);
                if (state == DeletionState.Success)
                {
                    stateString = "Success";
                    stateDetail = "Đã xóa tất cả đạo diễn của phim thành công";
                }
                else
                {
                    stateString = "Failed";
                    stateDetail = "Xóa tất cả đạo diễn của phim thất bại";
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