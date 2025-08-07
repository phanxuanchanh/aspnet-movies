using Common;
using Data.DTO;
using Data.Services;
using Ninject;
using System;
using System.Threading.Tasks;
using Web.Shared;

namespace Web.Admin.TagManagement
{
    public partial class TagDetail : AdminPage
    {
        [Inject]
        public TaxonomyService TaxonomyService { get; set; }

        private int id;
        protected TagDto tag;

        protected async void Page_Load(object sender, EventArgs e)
        {
            id = GetId<int>();
            if (id <= 0)
            {
                Response.ForceRedirectToRoute(this, "Admin_TagList", null);
                return;
            }

            hyplnkList.NavigateUrl = GetRouteUrl("Admin_TagList", null);
            hyplnkEdit.NavigateUrl = GetRouteUrl("Admin_EditTag", new { id = id, action = "update" });

            await GetTag(id);
        }

        private async Task GetTag(int id)
        {
            ExecResult<TagDto> result = await TaxonomyService.GetTagAsync(id);
            if (result.Status == ExecStatus.Success)
                tag = result.Data;
            else
                Response.ForceRedirectToRoute(this, "Admin_TagList", null);
        }

        protected async void btnDelete_Click(object sender, EventArgs e)
        {
            await DeleteTag();
        }

        private async Task DeleteTag()
        {
            ExecResult commandResult = await TaxonomyService.DeleteAsync(id); ;
            if (commandResult.Status == ExecStatus.Success)
            {
                Response.ForceRedirectToRoute(this, "Admin_TagList", null);
                return;
            }
            else
            {
                notifControl.Set(commandResult);
            }
        }
    }
}