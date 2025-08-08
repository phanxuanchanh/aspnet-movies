using Data.DTO;
using Data.Services;
using Ninject;
using System;
using System.Threading.Tasks;
using Web.Shared.Result;

namespace Web.Admin.DirectorManagement
{
    public partial class DirectorDetail : AdminPage
    {
        [Inject]
        public PeopleService PeopleService { get; set; }

        private long id;
        protected DirectorDto director;

        protected async void Page_Load(object sender, EventArgs e)
        {
            id = GetId<long>();
            if (id <= 0)
            {
                Response.ForceRedirectToRoute(this, "Admin_DirectorList", null);
                return;
            }

            hyplnkList.NavigateUrl = GetRouteUrl("Admin_DirectorList", null);
            hyplnkEdit.NavigateUrl = GetRouteUrl("Admin_EditDirector", new { id = id, action = "update" });

            await GetDirectorInfo(id);
        }

        private async Task GetDirectorInfo(long id)
        {
            ExecResult<DirectorDto> result = await PeopleService.GetDirectorAsync(id);
            if (result.Status == ExecStatus.Success)
            {
                director = result.Data;
            }
            else
            {
                Response.ForceRedirectToRoute(this, "Admin_DirectorList", null);
            }
        }

        protected async void btnDelete_Click(object sender, EventArgs e)
        {
            await DeleteDirector();
        }

        private async Task DeleteDirector()
        {
            ExecResult commandResult = await PeopleService.DeleteAsync(id); ;
            if (commandResult.Status == ExecStatus.Success)
            {
                Response.ForceRedirectToRoute(this, "Admin_DirectorList", null);
                return;
            }
            else
            {
                notifControl.Set(commandResult);
            }
        }
    }
}