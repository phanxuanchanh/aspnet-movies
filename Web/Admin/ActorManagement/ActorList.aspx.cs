using Common.Web;
using Data.DTO;
using Data.Services;
using Ninject;
using System;
using System.Threading.Tasks;
using Web.App_Start;
using Web.Models;

namespace Web.Admin.ActorManagement
{
    public partial class ActorList : System.Web.UI.Page
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
            hyplnkCreate.NavigateUrl = GetRouteUrl("Admin_EditActor", new { action = "create" });

            if (!CheckLoggedIn())
            {
                Response.RedirectToRoute("Account_Login", null);
                return;
            }

            if (!IsPostBack)
            {
                await SetGrvActor();
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
            await SetGrvActor();
            SetDrdlPage();
        }

        private async Task SetGrvActor()
        {
            PagedList<ActorDto> actors = await _peopleService
                .GetActorsAsync(drdlPage.SelectedIndex, 20);
            grvActor.DataSource = actors.Items;
            grvActor.DataBind();

            pageNumber = actors.PageNumber;
            currentPage = actors.CurrentPage;
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

        protected async void grvActor_SelectedIndexChanged(object sender, EventArgs e)
        {
            long key = (long)grvActor.DataKeys[grvActor.SelectedIndex].Value;
            ActorDto actor = (await _peopleService.GetActorAsync(key)).Data;
            toolDetail = string.Format("{0} -- {1}", actor.ID, actor.Name);
            hyplnkDetail.NavigateUrl = GetRouteUrl("Admin_ActorDetail", new { id = actor.ID });
            hyplnkEdit.NavigateUrl = GetRouteUrl("Admin_EditActor", new { id = actor.ID, action = "update" });
            enableTool = true;
        }
    }
}