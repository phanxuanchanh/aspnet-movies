using Common.Web;
using Data.DTO;
using Data.Services;
using Ninject;
using System;
using System.Threading.Tasks;
using Web.App_Start;
using Web.Models;

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
            try
            {
                hyplnkCreate.NavigateUrl = GetRouteUrl("Admin_EditRole", new { action = "create" });

                if (!CheckLoggedIn())
                {
                    Response.RedirectToRoute("Account_Login", null);
                    return;   
                }

                if (!IsPostBack)
                {
                    await SetGrvRole();
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
            if (_roleService != null)
            {
                _roleService.Dispose();
                _roleService = null;
            }
        }

        protected async void drdlPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                await SetGrvRole();
                SetDrdlPage();
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }

        private async Task SetGrvRole()
        {
            PagedList<RoleDto> roles = await _roleService
                .GetRolesAsync(drdlPage.SelectedIndex, 20);
            grvRole.DataSource = roles.Items;
            grvRole.DataBind();

            pageNumber = roles.PageNumber;
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
            try
            {
                string key = (string)grvRole.DataKeys[grvRole.SelectedIndex].Value;
                RoleDto role = (await _roleService.GetRoleAsync(key));
                toolDetail = string.Format("{0} -- {1}", role.ID, role.Name);
                hyplnkDetail.NavigateUrl = GetRouteUrl("Admin_RoleDetail", new { id = role.ID });
                hyplnkEdit.NavigateUrl = GetRouteUrl("Admin_EditRole", new { id = role.ID, action = "update" });
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