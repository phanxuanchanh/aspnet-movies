using Common.Web;
using Data.DTO;
using Data.Services;
using Ninject;
using System;
using System.Threading.Tasks;
using Web.App_Start;
using Web.Models;

namespace Web.Admin.CategoryManagement
{
    public partial class CategoryList : AdminPage
    {
        private TaxonomyService _taxonomyService;
        protected long currentPage;
        protected long pageNumber;
        protected bool enableTool;
        protected string toolDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            _taxonomyService = NinjectWebCommon.Kernel.Get<TaxonomyService>();
            enableTool = false;
            toolDetail = null;
            hyplnkCreate.NavigateUrl = GetRouteUrl("Admin_EditCategory", new { action = "create" });

            if (!CheckLoggedIn())
            {
                Response.RedirectToRoute("Account_Login", null);
                return;
            }

            if (!IsPostBack)
            {
                await SetGrvCategory();
                SetDrdlPage();
            }
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            if (_taxonomyService != null)
            {
                _taxonomyService.Dispose();
                _taxonomyService = null;
            }
        }

        protected async void drdlPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                await SetGrvCategory();
                SetDrdlPage();
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }

        private async Task SetGrvCategory()
        {
            PagedList<CategoryDto> categories = await _taxonomyService
                .GetCategoriesAsync(1/*drdlPage.SelectedIndex*/, 20);
            grvCategory.DataSource = categories.Items;
            grvCategory.DataBind();

            pageNumber = categories.PageSize;
            currentPage = categories.CurrentPage;

            pagination.SetAndRender(categories);
        }

        private void SetDrdlPage()
        {
            //int selectedIndex = drdlPage.SelectedIndex;
            //drdlPage.Items.Clear();
            //for (int i = 0; i < pageNumber; i++)
            //{
            //    string item = (i + 1).ToString();
            //    if (i == currentPage)
            //        drdlPage.Items.Add(string.Format("{0}*", item));
            //    else
            //        drdlPage.Items.Add(item);
            //}
            //drdlPage.SelectedIndex = selectedIndex;
        }

        protected async void grvCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int key = (int)grvCategory.DataKeys[grvCategory.SelectedIndex].Value;
            CategoryDto category = (await _taxonomyService.GetCategoryAsync(key)).Data;
            toolDetail = string.Format("{0} -- {1}", category.ID, category.Name);
            hyplnkDetail.NavigateUrl = GetRouteUrl("Admin_CategoryDetail", new { id = category.ID });
            hyplnkEdit.NavigateUrl = GetRouteUrl("Admin_EditCategory", new { id = category.ID, action = "update" });
            enableTool = true;
        }
    }
}