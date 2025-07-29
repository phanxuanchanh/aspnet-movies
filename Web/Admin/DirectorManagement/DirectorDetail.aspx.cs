using Common;
using Data.DTO;
using Data.Services;
using Ninject;
using System;
using System.Threading.Tasks;
using System.Web.UI;
using Web.App_Start;
using Web.Models;

namespace Web.Admin.DirectorManagement
{
    public partial class DirectorDetail : AdminPage
    {
        protected DirectorDto director;
        protected bool enableShowDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            enableShowDetail = false;

            long id = GetDirectorId();
            hyplnkList.NavigateUrl = GetRouteUrl("Admin_DirectorList", null);
            hyplnkEdit.NavigateUrl = GetRouteUrl("Admin_EditDirector", new { id = id, action = "update" });

            if (CheckLoggedIn())
                await GetDirectorInfo(id);
            else
                Response.RedirectToRoute("Account_Login", null);
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

            using (PeopleService peopleService = NinjectWebCommon.Kernel.Get<PeopleService>())
            {
                ExecResult<DirectorDto> result = await peopleService.GetDirectorAsync(id);
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

            using (PeopleService peopleService = NinjectWebCommon.Kernel.Get<PeopleService>())
            {
                ExecResult commandResult = await peopleService.DeleteAsync(id); ;
                if (commandResult.Status == ExecStatus.Success)
                {
                    Response.RedirectToRoute("Admin_DirectorList", null);
                    return;
                }
            }
        }
    }
}