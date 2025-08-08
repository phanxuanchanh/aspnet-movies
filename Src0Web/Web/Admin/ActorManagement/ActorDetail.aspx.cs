using Data.DTO;
using Data.Services;
using Ninject;
using System;
using System.Threading.Tasks;
using Web.Shared.Result;

namespace Web.Admin.ActorManagement
{
    public partial class ActorDetail : AdminPage
    {
        [Inject]
        public PeopleService PeopleService { get; set; }

        private long id;
        protected ActorDto actor;

        protected async void Page_Load(object sender, EventArgs e)
        {
            id = GetId<long>();
            if (id <= 0)
            {
                Response.ForceRedirectToRoute(this, "Admin_ActorList", null);
                return;
            }

            hyplnkList.NavigateUrl = GetRouteUrl("Admin_ActorList", null);
            hyplnkEdit.NavigateUrl = GetRouteUrl("Admin_EditActor", new { id = id, action = "update" });

            await GetActor(id);
        }

        private async Task GetActor(long id)
        {
            ExecResult<ActorDto> result = await PeopleService.GetActorAsync(id);
            if (result.Status == ExecStatus.Success)
                actor = result.Data;
            else
                Response.ForceRedirectToRoute(this, "Admin_ActorList", null);
        }

        protected async void btnDelete_Click(object sender, EventArgs e)
        {
            await DeleteCountry();
        }

        private async Task DeleteCountry()
        {
            ExecResult commandResult = await PeopleService.DeleteAsync(id); ;
            if (commandResult.Status == ExecStatus.Success)
            {
                Response.RedirectToRoute("Admin_ActorList", null);
                return;
            }
            else
            {
                notifControl.Set(commandResult);
            }
        }
    }
}