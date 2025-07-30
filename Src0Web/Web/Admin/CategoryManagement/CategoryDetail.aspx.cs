using Common;
using Data.DTO;
using Data.Services;
using Ninject;
using System;
using System.Threading.Tasks;
using Web.App_Start;
using Web.Models;

namespace Web.Admin.CategoryManagement
{
    public partial class CategoryDetail : AdminPage
    {
        protected CategoryDto category;
        protected bool enableShowDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            enableShowDetail = false;
            int id = GetCategoryId();
            hyplnkList.NavigateUrl = GetRouteUrl("Admin_CategoryList", null);
            hyplnkEdit.NavigateUrl = GetRouteUrl("Admin_EditCategory", new { id = id, action = "update" });

            if (CheckLoggedIn())
                await GetCategory(id);
            else
                Response.RedirectToRoute("Account_Login", null);
        }

        private int GetCategoryId()
        {
            object obj = Page.RouteData.Values["id"];
            if (obj == null)
                return -1;
            return int.Parse(obj.ToString());
        }

        private async Task GetCategory(int id)
        {
            if (id <= 0)
            {
                Response.RedirectToRoute("Admin_CategoryList", null);
                return;
            }

            using (TaxonomyService taxonomyService = NinjectWebCommon.Kernel.Get<TaxonomyService>())
            {
                ExecResult<CategoryDto> result = await taxonomyService.GetCategoryAsync(id);
                if (result.Status == ExecStatus.Success)
                {
                    category = result.Data;
                    enableShowDetail = true;
                }
                else
                {
                    Response.RedirectToRoute("Admin_CategoryList", null);
                }
            }
        }

        protected async void btnDelete_Click(object sender, EventArgs e)
        {
            await DeleteCategory();
        }

        private async Task DeleteCategory()
        {
            int id = GetCategoryId();
            if (id <= 0)
            {
                Response.RedirectToRoute("Admin_CategoryList", null);
                return;
            }

            using (TaxonomyService taxonomyService = NinjectWebCommon.Kernel.Get<TaxonomyService>())
            {
                ExecResult commandResult = await taxonomyService.DeleteAsync(id); ;
                if (commandResult.Status == ExecStatus.Success)
                {
                    Response.RedirectToRoute("Admin_CategoryList", null);
                    return;
                }
            }
        }
    }
}