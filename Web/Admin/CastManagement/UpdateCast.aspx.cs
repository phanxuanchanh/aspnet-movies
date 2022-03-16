using Data.BLL;
using Data.DTO;
using System;
using System.Threading.Tasks;
using System.Web.UI;
using Web.Models;
using Web.Validation;

namespace Web.Admin.CastManagement
{
    public partial class UpdateCast : System.Web.UI.Page
    {
        private CastBLL castBLL;
        protected CastInfo castInfo;
        private CustomValidation customValidation;
        protected bool enableShowResult;
        protected string stateString;
        protected string stateDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            castBLL = new CastBLL();
            customValidation = new CustomValidation();
            enableShowResult = false;
            stateString = null;
            stateDetail = null;
            try
            {   
                long id = GetCastId();
                hyplnkList.NavigateUrl = GetRouteUrl("Admin_CastList", null);
                hyplnkDetail.NavigateUrl = GetRouteUrl("Admin_CastDetail", new { id = id });
                hyplnkDelete.NavigateUrl = GetRouteUrl("Admin_DeleteCast", new { id = id });
                InitValidation();

                if (CheckLoggedIn())
                {
                    if (IsPostBack)
                    {
                        if (IsValidData())
                        {
                            await Update();
                            await LoadCastInfo(id);
                        }
                    }
                    else
                    {
                        await LoadCastInfo(id);
                    }
                }
                else
                {
                    Response.RedirectToRoute("Account_Login", null);
                }
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
            castBLL.Dispose();
        }

        private bool CheckLoggedIn()
        {
            object obj = Session["userSession"];
            if (obj == null)
                return false;

            UserSession userSession = (UserSession)obj;
            return (userSession.role == "Admin" || userSession.role == "Editor");
        }

        private long GetCastId()
        {
            object obj = Page.RouteData.Values["id"];
            if (obj == null)
                return -1;
            return long.Parse(obj.ToString());
        }

        private async Task LoadCastInfo(long id)
        {
            if (id <= 0)
            {
                Response.RedirectToRoute("Admin_CastList", null);
            }
            else
            {
                castBLL.IncludeDescription = true;
                CastInfo castInfo = await castBLL.GetCastAsync(id);
                if (castInfo == null)
                {
                    Response.RedirectToRoute("Admin_CastList", null);
                }
                else
                {
                    hdCastId.Value = castInfo.ID.ToString();
                    txtCastName.Text = castInfo.name;
                    txtCastDescription.Text = castInfo.description;
                }
            }
        }

        private void InitValidation()
        {
            customValidation.Init(
                cvCastName,
                "txtCastName",
                "Tên diễn viên không hợp lệ",
                true,
                null,
                customValidation.ValidateCastName
            );
        }

        private void ValidateData()
        {
            cvCastName.Validate();
        }

        private bool IsValidData()
        {
            ValidateData();
            return cvCastName.IsValid;
        }

        private CastUpdate GetCastUpdate()
        {
            return new CastUpdate
            {
                ID = long.Parse(Request.Form[hdCastId.UniqueID]),
                name = Request.Form[txtCastName.UniqueID],
                description = Request.Form[txtCastDescription.UniqueID]
            };
        }

        private async Task Update()
        {
            CastUpdate castUpdate = GetCastUpdate();
            UpdateState state = await castBLL.UpdateCastAsync(castUpdate);
            if (state == UpdateState.Success)
            {
                stateString = "Success";
                stateDetail = "Đã cập nhật diễn viên thành công";
            }
            else
            {
                stateString = "Failed";
                stateDetail = "Cập nhật diễn viên thất bại";
            }
            enableShowResult = true;
        }
    }
}