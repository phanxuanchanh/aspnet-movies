using Common.Web;
using Data.BLL;
using Data.DTO;
using Data.Services;
using Ninject;
using System;
using System.Threading.Tasks;
using Web.App_Start;
using Web.Models;

namespace Web.Admin.LanguageManagement
{
    public partial class LanguageList : AdminPage
    {
        private FilmMetadataService _filmMetadataService;
        protected long currentPage;
        protected long pageNumber;
        protected bool enableTool;
        protected string toolDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            _filmMetadataService = NinjectWebCommon.Kernel.Get<FilmMetadataService>();
            enableTool = false;
            toolDetail = null;
            hyplnkCreate.NavigateUrl = GetRouteUrl("Admin_EditLanguage", new { action = "create" });

            if (!CheckLoggedIn())
            {
                Response.RedirectToRoute("Account_Login", null);
                return;
            }

            if (!IsPostBack)
            {
                await SetGrvLanguage(0);
                SetDrdlPage();
            }
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            if (_filmMetadataService != null)
            {
                _filmMetadataService.Dispose();
                _filmMetadataService = null;
            }
        }

        private async Task SetGrvLanguage(int pageIndex)
        {
            PagedList<LanguageDto> languages = await _filmMetadataService
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
            int key = (int)grvLanguage.DataKeys[grvLanguage.SelectedIndex].Value;
            LanguageDto language = (await _filmMetadataService.GetLanguageAsync(key)).Data;
            toolDetail = string.Format("{0} -- {1}", language.ID, language.Name);
            hyplnkDetail.NavigateUrl = GetRouteUrl("Admin_LanguageDetail", new { id = language.ID });
            hyplnkEdit.NavigateUrl = GetRouteUrl("Admin_EditLanguage", new { id = language.ID, action = "update" });
            enableTool = true;
        }

        protected async void drdlPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            await SetGrvLanguage(drdlPage.SelectedIndex);
            SetDrdlPage();
        }
    }
}