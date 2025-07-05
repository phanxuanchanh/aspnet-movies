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
    public partial class EditCast : System.Web.UI.Page
    {
        private FilmBLL filmBLL;
        private CustomValidation customValidation;
        protected string filmName;
        protected List<ActorDto> castsByFilmId;
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
                hyplnkEdit_Director.NavigateUrl = GetRouteUrl("Admin_EditDirector_Film", new { id = id });
                hyplnkEdit_Image.NavigateUrl = GetRouteUrl("Admin_EditImage_Film", new { id = id });
                hyplnkEdit_Source.NavigateUrl = GetRouteUrl("Admin_EditSource_Film", new { id = id });
                hyplnkEdit.NavigateUrl = GetRouteUrl("Admin_UpdateFilm", new { id = id });
                hyplnkDelete.NavigateUrl = GetRouteUrl("Admin_DeleteFilm", new { id = id });
                InitValidation();

                if (CheckLoggedIn())
                {
                    await LoadCasts();
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
                cvFilmCast_Role,
                "txtFilmCast_Role",
                "Tên vai trò của diễn viên không hợp lệ",
                true,
                null,
                customValidation.ValidateCastRole
            );
        }

        private void ValidateData()
        {
            cvFilmCast_Role.Validate();
        }

        private bool IsValidData()
        {
            ValidateData();
            return cvFilmCast_Role.IsValid;
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
                filmBLL.IncludeCast = true;
                FilmInfo filmInfo = await filmBLL.GetFilmAsync(id);
                if (filmInfo == null)
                {
                    Response.RedirectToRoute("Admin_FilmList", null);
                }
                else
                {
                    enableShowDetail = true;
                    castsByFilmId = filmInfo.Casts;
                    filmName = filmInfo.name;
                }
            }
        }

        private async Task LoadCasts()
        {
            drdlFilmCast.Items.Clear();
            List<ActorDto> castInfos = await new CastBLL(filmBLL).GetCastsAsync();
            foreach (ActorDto castInfo in castInfos)
            {
                drdlFilmCast.Items.Add(new ListItem(castInfo.Name, castInfo.ID.ToString()));
            }
            drdlFilmCast.SelectedIndex = 0;
        }

        protected async void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string filmId = GetFilmId();
                if (IsValidData())
                {
                    string strCastId = Request.Form[drdlFilmCast.UniqueID];
                    string castRole = Request.Form[txtFilmCast_Role.UniqueID];
                    if (strCastId == null)
                    {
                        Response.RedirectToRoute("Admin_FilmList", null);
                    }
                    else
                    {
                        long castId = long.Parse(strCastId);
                        CreationState state = await filmBLL.AddCastAsync(filmId, castId, castRole);
                        if (state == CreationState.Success)
                        {
                            stateString = "Success";
                            stateDetail = "Đã thêm diễn viên vào phim thành công";
                        }
                        else if (state == CreationState.AlreadyExists)
                        {
                            stateString = "AlreadyExists";
                            stateDetail = "Thêm diễn viên vào phim thất bại. Lý do: Đã tồn tại diễn viên trong phim này";
                        }
                        else
                        {
                            stateString = "Failed";
                            stateDetail = "Thêm diễn viên vào phim thất bại";
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
                DeletionState state = await filmBLL.DeleteAllCastAsync(filmId);
                await LoadFilmInfo(filmId);
                if (state == DeletionState.Success)
                {
                    stateString = "Success";
                    stateDetail = "Đã xóa tất cả diễn viên của phim thành công";
                }
                else
                {
                    stateString = "Failed";
                    stateDetail = "Xóa tất cả diễn viên của phim thất bại";
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