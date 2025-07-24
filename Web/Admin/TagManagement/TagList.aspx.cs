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
        protected long currentPage;
        protected long pageNumber;
        protected bool enableTool;
        protected string toolDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            _taxonomyService = NinjectWebCommon.Kernel.Get<TaxonomyService>();
            enableTool = false;
            toolDetail = null;
            try
            {
                hyplnkCreate.NavigateUrl = GetRouteUrl("Admin_EditTag", new { action = "create" });

                if (!CheckLoggedIn())
                {
                    Response.RedirectToRoute("Account_Login", null);
                    return;
                }

                if (!IsPostBack)
                {
                    await SetGrvTag();
                    SetDrdlPage();
                }
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
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
                await SetGrvTag();
                SetDrdlPage();
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }

        private async Task SetGrvTag()
        {
            PagedList<TagDto> tags = await _taxonomyService
                .GetTagsAsync(drdlPage.SelectedIndex, 20);
            grvTag.DataSource = tags.Items;
            grvTag.DataBind();

            pageNumber = tags.PageNumber;
            currentPage = tags.CurrentPage;
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

        protected async void grvTag_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int key = (int)grvTag.DataKeys[grvTag.SelectedIndex].Value;
                TagDto tag = (await _taxonomyService.GetTagAsync(key)).Data;
                toolDetail = string.Format("{0} -- {1}", tag.ID, tag.Name);
                hyplnkDetail.NavigateUrl = GetRouteUrl("Admin_TagDetail", new { id = tag.ID });
                hyplnkEdit.NavigateUrl = GetRouteUrl("Admin_EditTag", new { id = tag.ID, action = "update" });
                enableTool = true;
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }
    }
}