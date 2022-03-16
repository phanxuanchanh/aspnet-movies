using Common.Web;
using Data.BLL;
using Data.DTO;
using System;
using System.Threading.Tasks;
using Web.Models;

namespace Web.Admin.DirectorManagement
{
    public partial class DirectorList : System.Web.UI.Page
    {
        private DirectorBLL directorBLL;
        protected long currentPage;
        protected long pageNumber;
        protected bool enableTool;
        protected string toolDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            directorBLL = new DirectorBLL();
            enableTool = false;
            toolDetail = null;
            try
            {
                hyplnkCreate.NavigateUrl = GetRouteUrl("Admin_CreateDirector", null);

                if (CheckLoggedIn())
                {
                    if (!IsPostBack)
                    {
                        await SetGrvDirector();
                        SetDrdlPage();
                        directorBLL.Dispose();
                    }
                }
                else
                {
                    Response.RedirectToRoute("Account_Login", null);
                    directorBLL.Dispose();
                }
            }
            catch (Exception ex)
            {
                directorBLL.Dispose();
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
            return (userSession.role == "Admin" || userSession.role == "Editor");
        }

        protected async void drdlPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                await SetGrvDirector();
                SetDrdlPage();
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
            directorBLL.Dispose();
        }

        private async Task SetGrvDirector()
        {
            directorBLL.IncludeTimestamp = true;
            PagedList<DirectorInfo> directors = await directorBLL
                .GetDirectorsAsync(drdlPage.SelectedIndex, 20);
            grvDirector.DataSource = directors.Items;
            grvDirector.DataBind();

            pageNumber = directors.PageNumber;
            currentPage = directors.CurrentPage;
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

        protected async void grvDirector_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                long key = (long)grvDirector.DataKeys[grvDirector.SelectedIndex].Value;
                DirectorInfo directorInfo = await directorBLL.GetDirectorAsync(key);
                toolDetail = string.Format("{0} -- {1}", directorInfo.ID, directorInfo.name);
                hyplnkDetail.NavigateUrl = GetRouteUrl("Admin_DirectorDetail", new { id = directorInfo.ID });
                hyplnkEdit.NavigateUrl = GetRouteUrl("Admin_UpdateDirector", new { id = directorInfo.ID });
                hyplnkDelete.NavigateUrl = GetRouteUrl("Admin_DeleteDirector", new { id = directorInfo.ID });
                enableTool = true;
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
            directorBLL.Dispose();
        }
    }
}