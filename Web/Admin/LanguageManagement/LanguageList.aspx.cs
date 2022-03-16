using Common.Web;
using Data.BLL;
using Data.DTO;
using System;
using System.Threading.Tasks;
using Web.Models;

namespace Web.Admin.LanguageManagement
{
    public partial class LanguageList : System.Web.UI.Page
    {
        private LanguageBLL languageBLL;
        protected long currentPage;
        protected long pageNumber;
        protected bool enableTool;
        protected string toolDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            languageBLL = new LanguageBLL();
            enableTool = false;
            toolDetail = null;
            try
            {

                hyplnkCreate.NavigateUrl = GetRouteUrl("Admin_CreateLanguage", null);

                if (CheckLoggedIn())
                {
                    if (!IsPostBack)
                    {
                        await SetGrvLanguage(0);
                        SetDrdlPage();
                        languageBLL.Dispose();
                    }
                }
                else
                {
                    Response.RedirectToRoute("Account_Login", null);
                    languageBLL.Dispose();
                }
            }
            catch (Exception ex)
            {
                languageBLL.Dispose();
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

        private async Task SetGrvLanguage(int pageIndex)
        {
            languageBLL.IncludeTimestamp = true;
            PagedList<LanguageInfo> languages = await languageBLL
                .GetLanguagesAsync(drdlPage.SelectedIndex, 20);
            grvLanguage.DataSource = languages.Items;
            grvLanguage.DataBind();

            pageNumber = languages.PageNumber;
            currentPage = languages.CurrentPage;
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

        protected async void grvLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int key = (int)grvLanguage.DataKeys[grvLanguage.SelectedIndex].Value;
                LanguageInfo languageInfo = await languageBLL.GetLanguageAsync(key);
                toolDetail = string.Format("{0} -- {1}", languageInfo.ID, languageInfo.name);
                hyplnkDetail.NavigateUrl = GetRouteUrl("Admin_LanguageDetail", new { id = languageInfo.ID });
                hyplnkEdit.NavigateUrl = GetRouteUrl("Admin_UpdateLanguage", new { id = languageInfo.ID });
                hyplnkDelete.NavigateUrl = GetRouteUrl("Admin_DeleteLanguage", new { id = languageInfo.ID });
                enableTool = true;
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
            languageBLL.Dispose();
        }

        protected async void drdlPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                await SetGrvLanguage(drdlPage.SelectedIndex);
                SetDrdlPage();
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
            languageBLL.Dispose();
        }
    }
}