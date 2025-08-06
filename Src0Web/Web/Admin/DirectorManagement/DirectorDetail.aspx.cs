using Common;
using Data.DTO;
using Data.Services;
using Ninject;
using System;
using System.Threading.Tasks;
using System.Web.UI;

namespace Web.Admin.DirectorManagement
{
    public partial class DirectorDetail : AdminPage
    {
        [Inject]
        public PeopleService PeopleService { get; set; }

        protected DirectorDto director;
        protected bool enableShowDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            enableShowDetail = false;

            long id = GetDirectorId();
            hyplnkList.NavigateUrl = GetRouteUrl("Admin_DirectorList", null);
            hyplnkEdit.NavigateUrl = GetRouteUrl("Admin_EditDirector", new { id = id, action = "update" });

            await GetDirectorInfo(id);
        }

        private long GetDirectorId()
        {
            object obj = Page.RouteData.Values["id"];
            if (obj == null)
                return -1;
            return long.Parse(obj.ToString());
        }

        private async Task GetDirectorInfo(long id)
        {
            if (id <= 0)
            {
                Response.RedirectToRoute("Admin_DirectorList", null);
                return;
            }

            ExecResult<DirectorDto> result = await PeopleService.GetDirectorAsync(id);
            if (result.Status == ExecStatus.Success)
            {
                director = result.Data;
                enableShowDetail = true;
            }
            else
            {
                Response.RedirectToRoute("Admin_DirectorList", null);
            }
        }

        protected async void btnDelete_Click(object sender, EventArgs e)
        {
            await DeleteDirector();
        }

        private async Task DeleteDirector()
        {
            long id = GetDirectorId();
            if (id <= 0)
            {
                Response.RedirectToRoute("Admin_DirectorList", null);
                return;
            }

            ExecResult commandResult = await PeopleService.DeleteAsync(id); ;
            if (commandResult.Status == ExecStatus.Success)
            {
                Response.RedirectToRoute("Admin_DirectorList", null);
                return;
            }
        }
    }
}