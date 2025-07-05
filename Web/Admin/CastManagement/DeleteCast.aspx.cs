using Data.BLL;
using Data.DTO;
using System;
using System.Threading.Tasks;
using System.Web.UI;
using Web.Models;

namespace Web.Admin.CastManagement
{
    public partial class DeleteCast : System.Web.UI.Page
    {
        protected ActorDto actorDto;
        protected bool enableShowInfo;
        protected bool enableShowResult;
        protected string stateString;
        protected string stateDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            enableShowInfo = false;
            enableShowResult = false;
            stateString = null;
            stateDetail = null;
            try
            {
                hyplnkList.NavigateUrl = GetRouteUrl("Admin_CastList", null);

                if (CheckLoggedIn())
                {
                    if (!IsPostBack)
                    {
                        await GetCastInfo();
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
        }

        private bool CheckLoggedIn()
        {
            object obj = Session["userSession"];
            if (obj == null)
                return false;

            UserSession userSession = (UserSession)obj;
            return (userSession.role == "Admin" || userSession.role == "Editor");
        }

        private int GetCastId()
        {
            object obj = Page.RouteData.Values["id"];
            if (obj == null)
                return -1;
            return int.Parse(obj.ToString());
        }

        private async Task GetCastInfo()
        {
            int id = GetCastId();
            if (id <= 0)
            {
                Response.RedirectToRoute("Admin_CastList", null);
            }
            else
            {
                using(CastBLL castBLL = new CastBLL())
                {
                    actorDto = await castBLL.GetCastAsync(id);
                }

                if (actorDto == null)
                    Response.RedirectToRoute("Admin_CastList", null);
                else
                    enableShowInfo = true;
            }
        }

        private async Task DeleteCastInfo()
        {
            int id = GetCastId();
            DeletionState state;
            using(CastBLL castBLL = new CastBLL())
            {
                state = await castBLL.DeleteCastAsync(id);
            }

            if (state == DeletionState.Success)
            {
                stateString = "Success";
                stateDetail = "Đã xóa diễn viên thành công";
            }
            else if (state == DeletionState.Failed)
            {
                stateString = "Failed";
                stateDetail = "Xóa diễn viên thất bại";
            }
            else
            {
                stateString = "ConstraintExists";
                stateDetail = "Không thể xóa diễn viên. Lý do: Diễn viên này đang được sử dụng!";
            }
            enableShowResult = true;
        }

        protected async void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                await DeleteCastInfo();
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }
    }
}