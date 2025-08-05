using Common;
using Common.Web;
using Data.DTO;
using Data.Services;
using Ninject;
using System;
using System.Threading.Tasks;
using Web.App_Start;
using Web.Models;

namespace Web.Admin.TagManagement
{
    public partial class TagList : AdminPage
    {
        private TaxonomyService _taxonomyService;
        protected PagedList<TagDto> paged;

        protected async void Page_Load(object sender, EventArgs e)
        {
            _taxonomyService = NinjectWebCommon.Kernel.Get<TaxonomyService>();
            hyplnkCreate.NavigateUrl = GetRouteUrl("Admin_EditTag", new { action = "create" });
            paged = new PagedList<TagDto>();

            GetPagnationQuery();

            if (!IsPostBack)
            {
                txtPageSize.Text = paged.PageSize.ToString();
                await SetTagTable();
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


        private async Task SetTagTable()
        {
            paged = await _taxonomyService
                .GetTagsAsync(paged.CurrentPage, paged.PageSize, txtSearch.Text);

            rptTags.DataSource = paged.Items;
            rptTags.DataBind();

            pagination.SetAndRender(paged);
            pagination.SetUrl("Admin_TagList");
        }

        protected async void lnkDelete_Command(object sender, System.Web.UI.WebControls.CommandEventArgs e)
        {
            string strId = e.CommandArgument.ToString();
            if (string.IsNullOrEmpty(strId))
                return;

            if (int.TryParse(strId, out int id))
            {
                ExecResult commandResult = await _taxonomyService.DeleteAsync(id);
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
                Response.RedirectToRoute("Admin_TagList", new { page = paged.CurrentPage, pageSize = paged.PageSize });
            else
                notifControl.Set(ExecResult.Failure("PageSize không hợp lệ!"));
        }

        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            Response.RedirectToRoute("Admin_TagList", new { page = paged.CurrentPage, pageSize = paged.PageSize, searchText = txtSearch.Text });
        }
    }
}