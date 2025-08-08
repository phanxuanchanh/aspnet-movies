using Data.DTO;
using Data.Services;
using Ninject;
using System;
using System.Threading.Tasks;
using Web.Shared.Result;

namespace Web.Admin.CategoryManagement
{
    public partial class CategoryDetail : AdminPage
    {
        [Inject]
        public TaxonomyService TaxonomyService { get; set; }

        private int id;
        protected CategoryDto category;

        protected async void Page_Load(object sender, EventArgs e)
        {
            id = GetId<int>();
            if (id <= 0)
            {
                Response.ForceRedirectToRoute(this, "Admin_CategoryList", null);
                return;
            }

            hyplnkList.NavigateUrl = GetRouteUrl("Admin_CategoryList", null);
            hyplnkEdit.NavigateUrl = GetRouteUrl("Admin_EditCategory", new { id = id, action = "update" });

            await GetCategory(id);
        }

        private async Task GetCategory(int id)
        {
            ExecResult<CategoryDto> result = await TaxonomyService.GetCategoryAsync(id);
            if (result.Status == ExecStatus.Success)
            {
                category = result.Data;
            }
            else
            {
                Response.ForceRedirectToRoute(this, "Admin_CategoryList", null);
            }
        }

        protected async void btnDelete_Click(object sender, EventArgs e)
        {
            await DeleteCategory();
        }

        private async Task DeleteCategory()
        {
            ExecResult commandResult = await TaxonomyService.DeleteAsync(id); ;
            if (commandResult.Status == ExecStatus.Success)
            {
                Response.ForceRedirectToRoute(this, "Admin_CategoryList", null);
                return;
            }else
            {
                notifControl.Set(commandResult);
            }    
        }
    }
}