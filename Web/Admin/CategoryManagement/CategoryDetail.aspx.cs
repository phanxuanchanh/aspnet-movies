using Data.BLL;
using Data.DTO;
using System;
using System.Threading.Tasks;
using Web.Models;

namespace Web.Admin.CategoryManagement
{
    public partial class CategoryDetail : System.Web.UI.Page
    {
        protected CategoryInfo categoryInfo;
        protected bool enableShowDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            enableShowDetail = false;
            try
            {
                int id = GetCategoryId();
                hyplnkList.NavigateUrl = GetRouteUrl("Admin_CategoryList", null);
                hyplnkEdit.NavigateUrl = GetRouteUrl("Admin_UpdateCategory", new { id = id });
                hyplnkDelete.NavigateUrl = GetRouteUrl("Admin_DeleteCategory", new { id = id });

                if (CheckLoggedIn())
                    await GetCategoryInfo(id);
                else
                    Response.RedirectToRoute("Account_Login", null);
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

        private async Task GetCategoryInfo(int id)
        {
            if (id <= 0)
            {
                Response.RedirectToRoute("Admin_CategoryList", null);
            }
            else
            {
                using(CategoryBLL categoryBLL = new CategoryBLL())
                {
                    categoryBLL.IncludeDescription = true;
                    categoryBLL.IncludeTimestamp = true;
                    categoryInfo = await categoryBLL.GetCategoryAsync(id);
                }

                if (categoryInfo == null)
                    Response.RedirectToRoute("Admin_CategoryList", null);
                else
                    enableShowDetail = true;
            }
        }
    }
}