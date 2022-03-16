using Common.Web;
using Data.BLL;
using Data.DTO;
using System;
using System.Threading.Tasks;
using Web.Models;

namespace Web.Admin.UserManagement
{
    public partial class UserList : System.Web.UI.Page
    {
        private UserBLL userBLL;
        protected long currentPage;
        protected long pageNumber;
        protected bool enableTool;
        protected string toolDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            userBLL = new UserBLL();
            enableTool = false;
            toolDetail = null;
            try
            {
                hyplnkCreate.NavigateUrl = GetRouteUrl("Admin_CreateUser", null);

                if (CheckLoggedIn())
                {
                    if (!IsPostBack)
                    {
                        await SetGrvUser();
                        SetDrdlPage();
                        userBLL.Dispose();
                    }
                }
                else
                {
                    Response.RedirectToRoute("Account_Login", null);
                    userBLL.Dispose();
                }
            }
            catch (Exception ex)
            {
                userBLL.Dispose();
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }

        private bool CheckLoggedIn()
        {
            object obj = Session["userSession"];
            if (obj == null)
                return false;

            UserSession userSession = (UserSession)obj;
            return (userSession.role == "Admin");
        }

        protected async void drdlPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                await SetGrvUser();
                SetDrdlPage();
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
            userBLL.Dispose();
        }

        private async Task SetGrvUser()
        {
            userBLL.IncludeTimestamp = true;
            PagedList<UserInfo> users = await userBLL
                .GetUsersAsync(drdlPage.SelectedIndex, 20);
            grvUser.DataSource = users.Items;
            grvUser.DataBind();

            pageNumber = users.PageNumber;
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
            try
            {
                string key = (string)grvUser.DataKeys[grvUser.SelectedIndex].Value;
                UserInfo userInfo = await userBLL.GetUserAsync(key);
                toolDetail = string.Format("{0} -- {1}", userInfo.ID, userInfo.name);
                hyplnkDetail.NavigateUrl = GetRouteUrl("Admin_UserDetail", new { id = userInfo.ID });
                hyplnkEdit.NavigateUrl = GetRouteUrl("Admin_UpdateUser", new { id = userInfo.ID });
                hyplnkDelete.NavigateUrl = GetRouteUrl("Admin_DeleteUser", new { id = userInfo.ID });
                enableTool = true;
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
            userBLL.Dispose();
        }
    }
}