using Common;
using Common.Web;
using Data.DTO;
using Data.Services;
using Ninject;
using System;
using System.Threading.Tasks;

namespace Web.Admin.CategoryManagement
{
    public partial class CategoryList : AdminPage
    {
        [Inject]
        public TaxonomyService TaxonomyService { get; set; }

        protected PagedList<CategoryDto> paged;

        protected async void Page_Load(object sender, EventArgs e)
        {
            hyplnkCreate.NavigateUrl = GetRouteUrl("Admin_EditCategory", new { action = "create" });
            paged = new PagedList<CategoryDto>();

            GetPagnationQuery();

            if (!IsPostBack)
            {
                txtPageSize.Text = paged.PageSize.ToString();
                await SetCategoryTable();
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

            txtSearch.Text = Request.QueryString["searchText"] ?? string.Empty;
        }

        private async Task SetCategoryTable()
        {
            paged = await TaxonomyService
                .GetCategoriesAsync(paged.CurrentPage, paged.PageSize, txtSearch.Text);

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
        }

        protected void txtPageSize_TextChanged(object sender, EventArgs e)
        {
            if (long.TryParse(txtPageSize.Text, out long size))
                Response.RedirectToRoute("Admin_CategoryList", new { page = paged.CurrentPage, pageSize = paged.PageSize });
            else
                notifControl.Set(ExecResult.Failure("PageSize không hợp lệ!"));
        }

        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            Response.RedirectToRoute("Admin_CategoryList", new { page = paged.CurrentPage, pageSize = paged.PageSize, searchText = txtSearch.Text });
        }
    }
}