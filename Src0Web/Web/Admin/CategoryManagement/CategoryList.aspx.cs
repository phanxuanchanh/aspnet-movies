using Data.DTO;
using Data.Services;
using Ninject;
using System;
using System.Threading.Tasks;
using Web.Shared.Result;

namespace Web.Admin.CategoryManagement
{
    public partial class CategoryList : AdminPage, IPostbackAwarePage
    {
        [Inject]
        public TaxonomyService TaxonomyService { get; set; }

        protected string searchText;
        protected PagedList<CategoryDto> paged;

        protected void Page_Load(object sender, EventArgs e)
        {
            hyplnkCreate.NavigateUrl = GetRouteUrl("Admin_EditCategory", new { action = "create" });
            paged = new PagedList<CategoryDto>();
        }

        public async void Page_LoadNoPostback(object sender, EventArgs e)
        {
            GetPagnationQuery();
            await SetCategoryTable();
        }

        public async void Page_LoadWithPostback(object sender, EventArgs e)
        {
            string eventTarget = Request["__EVENTTARGET"];
            string eventArgument = Request["__EVENTARGUMENT"];

            if (eventTarget == "btnDeleteSelected_Click")
            {
                await DeletedSelectedItems(eventArgument.Split(','));
                await SetCategoryTable();
            }
        }

        private async Task DeletedSelectedItems(params string[] ids)
        {
            if (ids == null || ids.Length == 0)
                return;

            foreach (string strId in ids)
            {
                if (int.TryParse(strId, out int id))
                {
                    await TaxonomyService.DeleteAsync(id, forceDelete: false);
                }
            }
        }

        private void GetPagnationQuery()
        {
            string pageQuery = Request.QueryString["page"];
            string sizeQuery = Request.QueryString["pageSize"];

            if (string.IsNullOrEmpty(pageQuery))
                paged.CurrentPage = 1;
            else
            {
                if (long.TryParse(pageQuery, out long page))
                    paged.CurrentPage = Math.Max(1, page);
                else
                    paged.CurrentPage = 1;
            }

            if (string.IsNullOrEmpty(sizeQuery))
                paged.PageSize = 20;
            else
            {
                if (long.TryParse(sizeQuery, out long size))
                    paged.PageSize = Math.Max(1, size);
                else
                    paged.PageSize = 20;
            }

            searchText = Request.QueryString["searchText"] ?? string.Empty;
        }

        private async Task SetCategoryTable()
        {
            paged = await TaxonomyService
                .GetCategoriesAsync(paged.CurrentPage, paged.PageSize, searchText);

            rptCategories.DataSource = paged.Items;
            rptCategories.DataBind();

            pagination.SetAndRender(paged);
            pagination.SetUrl("Admin_CategoryList");
        }

        protected async void lnkDelete_Command(object sender, System.Web.UI.WebControls.CommandEventArgs e)
        {
            string strId = e.CommandArgument.ToString();
            if (string.IsNullOrEmpty(strId))
                return;

            if (int.TryParse(strId, out int id))
            {
                ExecResult commandResult = await TaxonomyService.DeleteAsync(id);
                notifControl.Set(commandResult);
            }
            else
            {
                notifControl.Set(ExecResult.Failure("ID không hợp lệ!"));
                return;
            }

            await SetCategoryTable();
        }
    }
}