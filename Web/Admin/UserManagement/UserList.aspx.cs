using Common.Web;
using Data.DTO;
using Data.Services;
using Ninject;
using System;
using System.Threading.Tasks;
using Web.App_Start;

namespace Web.Admin.UserManagement
{
    public partial class UserList : AdminPage
    {
        private UserService _userService;
        protected long currentPage;
        protected long pageNumber;
        protected bool enableTool;
        protected string toolDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            _userService = NinjectWebCommon.Kernel.Get<UserService>();
            enableTool = false;
            toolDetail = null;
            hyplnkCreate.NavigateUrl = GetRouteUrl("Admin_CreateUser", null);

            if (!CheckLoggedIn())
            {
                Response.RedirectToRoute("Account_Login", null);
                return;
            }

            if (!IsPostBack)
            {
                await SetGrvUser();
                SetDrdlPage();
            }
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            if (_userService != null)
            {
                _userService.Dispose();
                _userService = null;
            }
        }

        protected async void drdlPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            await SetGrvUser();
            SetDrdlPage();
        }

        private async Task SetGrvUser()
        {
            PagedList<UserDto> users = await _userService
                .GetUsersAsync(drdlPage.SelectedIndex, 20);
            grvUser.DataSource = users.Items;
            grvUser.DataBind();

            pageNumber = users.PageSize;
            currentPage = users.CurrentPage;
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

        protected async void grvUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            string key = (string)grvUser.DataKeys[grvUser.SelectedIndex].Value;
            UserDto user = (await _userService.GetUserAsync(key)).Data;
            toolDetail = string.Format("{0} -- {1}", user.ID, user.Name);
            hyplnkDetail.NavigateUrl = GetRouteUrl("Admin_UserDetail", new { id = user.ID });
            hyplnkEdit.NavigateUrl = GetRouteUrl("Admin_UpdateUser", new { id = user.ID });
            hyplnkDelete.NavigateUrl = GetRouteUrl("Admin_DeleteUser", new { id = user.ID });
            enableTool = true;
        }
    }
}