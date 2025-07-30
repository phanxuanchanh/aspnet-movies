using Common.Web;
using Data.DTO;
using Data.Services;
using Ninject;
using System;
using System.Threading.Tasks;
using Web.App_Start;

namespace Web.Admin.DirectorManagement
{
    public partial class DirectorList : AdminPage
    {
        private PeopleService _peopleService;
        protected long currentPage;
        protected long pageNumber;
        protected bool enableTool;
        protected string toolDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            _peopleService = NinjectWebCommon.Kernel.Get<PeopleService>();
            enableTool = false;
            toolDetail = null;

            hyplnkCreate.NavigateUrl = GetRouteUrl("Admin_EditDirector", new { action = "create" });

            if (!CheckLoggedIn())
            {
                Response.RedirectToRoute("Account_Login", null);
                return;

            }

            if (!IsPostBack)
            {
                await SetGrvDirector();
                SetDrdlPage();
            }
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            if (_peopleService != null)
            {
                _peopleService.Dispose();
                _peopleService = null;
            }
        }

        protected async void drdlPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            await SetGrvDirector();
            SetDrdlPage();
        }

        private async Task SetGrvDirector()
        {
            PagedList<DirectorDto> directors = await _peopleService
                .GetDirectorsAsync(drdlPage.SelectedIndex, 20);
            grvDirector.DataSource = directors.Items;
            grvDirector.DataBind();

            pageNumber = directors.PageSize;
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
            long key = (long)grvDirector.DataKeys[grvDirector.SelectedIndex].Value;
            DirectorDto director = (await _peopleService.GetDirectorAsync(key)).Data;
            toolDetail = string.Format("{0} -- {1}", director.ID, director.Name);
            hyplnkDetail.NavigateUrl = GetRouteUrl("Admin_DirectorDetail", new { id = director.ID });
            hyplnkEdit.NavigateUrl = GetRouteUrl("Admin_EditDirector", new { id = director.ID, action = "update" });
            enableTool = true;
        }
    }
}