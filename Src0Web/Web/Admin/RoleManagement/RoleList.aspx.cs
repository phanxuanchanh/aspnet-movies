using Common.Web;
using Data.DTO;
using Data.Services;
using Ninject;
using System;
using System.Threading.Tasks;
using Web.App_Start;
using Web.Shared.Result;
using Web.Shared.WebForms;

namespace Web.Admin.RoleManagement
{
    public partial class RoleList : AdminPage
    {
        private RoleService _roleService;
        protected long currentPage;
        protected long pageNumber;
        protected bool enableTool;
        protected string toolDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            _roleService = NinjectWebCommon.Kernel.Get<RoleService>();
            enableTool = false;
            toolDetail = null;
            hyplnkCreate.NavigateUrl = GetRouteUrl("Admin_EditRole", new { action = "create" });

            if (!IsPostBack)
            {
                await SetGrvRole();
                SetDrdlPage();
            }
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            if (_roleService != null)
            {
                _roleService.Dispose();
                _roleService = null;
            }
        }

        protected async void drdlPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            await SetGrvRole();
            SetDrdlPage();
        }

        private async Task SetGrvRole()
        {
            PagedList<RoleDto> roles = await _roleService
                .GetRolesAsync(drdlPage.SelectedIndex, 20);
            grvRole.DataSource = roles.Items;
            grvRole.DataBind();

            pageNumber = roles.PageSize;
            currentPage = roles.CurrentPage;
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

        protected async void grvRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            string key = (string)grvRole.DataKeys[grvRole.SelectedIndex].Value;
            RoleDto role = (await _roleService.GetRoleAsync(key));
            toolDetail = string.Format("{0} -- {1}", role.ID, role.Name);
            hyplnkDetail.NavigateUrl = GetRouteUrl("Admin_RoleDetail", new { id = role.ID });
            hyplnkEdit.NavigateUrl = GetRouteUrl("Admin_EditRole", new { id = role.ID, action = "update" });
            enableTool = true;
        }
    }
}