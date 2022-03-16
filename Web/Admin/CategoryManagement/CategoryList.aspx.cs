using Common.Web;
using Data.BLL;
using Data.DTO;
using System;
using System.Threading.Tasks;
using Web.Models;

namespace Web.Admin.CategoryManagement
{
    public partial class CategoryList : System.Web.UI.Page
    {
        private CategoryBLL categoryBLL;
        protected long currentPage;
        protected long pageNumber;
        protected bool enableTool;
        protected string toolDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            categoryBLL = new CategoryBLL();
            enableTool = false;
            toolDetail = null;
            try
            {
                
                hyplnkCreate.NavigateUrl = GetRouteUrl("Admin_CreateCategory", null);

                if (CheckLoggedIn())
                {
                    if (!IsPostBack)
                    {
                        await SetGrvCategory();
                        SetDrdlPage();
                        categoryBLL.Dispose();
                    }
                }
                else
                {
                    Response.RedirectToRoute("Account_Login", null);
                    categoryBLL.Dispose();
                }
            }catch(Exception ex)
            {
                categoryBLL.Dispose();
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

        protected async void drdlPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                await SetGrvCategory();
                SetDrdlPage();
            }
            catch(Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
            categoryBLL.Dispose();
        }

        private async Task SetGrvCategory()
        {
            categoryBLL.IncludeTimestamp = true;
            PagedList<CategoryInfo> categories = await categoryBLL
                .GetCategoriesAsync(drdlPage.SelectedIndex, 20);
            grvCategory.DataSource = categories.Items;
            grvCategory.DataBind();

            pageNumber = categories.PageNumber;
            currentPage = categories.CurrentPage;
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

        protected async void grvCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int key = (int)grvCategory.DataKeys[grvCategory.SelectedIndex].Value;
                CategoryInfo categoryInfo = await categoryBLL.GetCategoryAsync(key);
                toolDetail = string.Format("{0} -- {1}", categoryInfo.ID, categoryInfo.name);
                hyplnkDetail.NavigateUrl = GetRouteUrl("Admin_CategoryDetail", new { id = categoryInfo.ID });
                hyplnkEdit.NavigateUrl = GetRouteUrl("Admin_UpdateCategory", new { id = categoryInfo.ID });
                hyplnkDelete.NavigateUrl = GetRouteUrl("Admin_DeleteCategory", new { id = categoryInfo.ID });
                enableTool = true;
            }
            catch(Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
            categoryBLL.Dispose();
        }
    }
}