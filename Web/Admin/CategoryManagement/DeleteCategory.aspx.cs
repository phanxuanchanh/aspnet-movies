using Data.BLL;
using Data.DTO;
using System;
using System.Threading.Tasks;
using System.Web.UI;
using Web.Models;

namespace Web.Admin.CategoryManagement
{
    public partial class DeleteCategory : System.Web.UI.Page
    {
        protected CategoryInfo categoryInfo;
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
                hyplnkList.NavigateUrl = GetRouteUrl("Admin_CategoryList", null);

                if (CheckLoggedIn())
                {
                    if (!IsPostBack)
                    {
                        await GetCategoryInfo();
                    }
                }
                else
                {
                    Response.RedirectToRoute("Account_Login", null);
                }    
            }
            catch(Exception ex)
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

        private int GetCategoryId()
        {
            object obj = Page.RouteData.Values["id"];
            if (obj == null)
                return -1;
            return int.Parse(obj.ToString());
        }

        private async Task GetCategoryInfo()
        {
            int id = GetCategoryId();
            if(id <= 0)
            {
                Response.RedirectToRoute("Admin_CategoryList", null);
            }
            else
            {
                using(CategoryBLL categoryBLL = new CategoryBLL())
                {
                    categoryInfo = await categoryBLL.GetCategoryAsync(id);
                }

                if (categoryInfo == null)
                    Response.RedirectToRoute("Admin_CategoryList", null);
                else
                    enableShowInfo = true;
            }
        }

        private async Task DeleteCategoryInfo()
        {
            int id = GetCategoryId();
            DeletionState state;
            using(CategoryBLL categoryBLL = new CategoryBLL())
            {
                state = await categoryBLL.DeleteCategoryAsync(id);
            }

            if (state == DeletionState.Success)
            {
                stateString = "Success";
                stateDetail = "Đã xóa thể loại thành công";
            }
            else if (state == DeletionState.Failed)
            {
                stateString = "Failed";
                stateDetail = "Xóa thể loại thất bại";
            }
            else
            {
                stateString = "ConstraintExists";
                stateDetail = "Không thể xóa thể loại. Lý do: Thể loại này đang được sử dụng!";
            }
            enableShowResult = true;
        }

        protected async void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                await DeleteCategoryInfo();
            }
            catch(Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }
    }
}