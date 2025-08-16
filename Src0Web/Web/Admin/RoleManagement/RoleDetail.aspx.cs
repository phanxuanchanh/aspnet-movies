using Data.DTO;
using Data.Services;
using System;
using System.Threading.Tasks;
using Web.Shared.Result;

namespace Web.Admin.RoleManagement
{
    public partial class RoleDetail : AdminPage
    {
        protected RoleDto role;

        protected async void Page_Load(object sender, EventArgs e)
        {
            string id = GetId<string>();
            if (string.IsNullOrEmpty(id))
            {
                Response.ForceRedirectToRoute(this, "Admin_RoleList", null);
                return;
            }

            hyplnkList.NavigateUrl = GetRouteUrl("Admin_RoleList", null);
            hyplnkEdit.NavigateUrl = GetRouteUrl("Admin_UpdateRole", new { id = id });
            hyplnkDelete.NavigateUrl = GetRouteUrl("Admin_DeleteRole", new { id = id });

            await GetRoleInfo(id);
        }

        private async Task GetRoleInfo(string id)
        {
            RoleService roleService = Inject<RoleService>();

            ExecResult commandResult = await roleService.GetRoleAsync(id);

            if (commandResult.Status != ExecStatus.Success)
            {
                Response.ForceRedirectToRoute(this, "Admin_RoleList", null);
                return;
            }
        }
    }
}