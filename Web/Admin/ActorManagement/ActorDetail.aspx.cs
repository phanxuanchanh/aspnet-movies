using Common;
using Data.BLL;
using Data.DTO;
using Data.Services;
using Ninject;
using System;
using System.Threading.Tasks;
using System.Web.UI;
using Web.App_Start;
using Web.Models;

namespace Web.Admin.ActorManagement
{
    public partial class ActorDetail : System.Web.UI.Page
    {
        protected ActorDto actor;
        protected ExecResult commandResult;
        protected bool enableShowDetail;
        protected bool enableShowResult;

        protected async void Page_Load(object sender, EventArgs e)
        {
            enableShowDetail = false;
            try
            {
                long id = GetActorId();
                hyplnkList.NavigateUrl = GetRouteUrl("Admin_ActorList", null);
                hyplnkEdit.NavigateUrl = GetRouteUrl("Admin_EditActor", new { id = id, action = "update" });

                if (CheckLoggedIn())
                    await GetActor(id);
                else
                    Response.RedirectToRoute("Account_Login", null);
            }
            catch (Exception ex)
            {
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

        private long GetActorId()
        {
            object obj = Page.RouteData.Values["id"];
            if (obj == null)
                return -1;
            return long.Parse(obj.ToString());
        }

        private async Task GetActor(long id)
        {
            if (id <= 0)
            {
                Response.RedirectToRoute("Admin_ActorList", null);
                return;
            }

            using (PeopleService peopleService = NinjectWebCommon.Kernel.Get<PeopleService>())
            {
                ExecResult<ActorDto> result = await peopleService.GetActorAsync(id);
                if (result.Status == ExecStatus.Success)
                {
                    actor = result.Data;
                    enableShowDetail = true;
                }
                else
                {
                    Response.RedirectToRoute("Admin_CountryList", null);
                }
            }
        }

        protected async void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                await DeleteCountry();
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }

        private async Task DeleteCountry()
        {
            long id = GetActorId();
            if (id <= 0)
            {
                Response.RedirectToRoute("Admin_ActorList", null);
                return;
            }

            using (PeopleService peopleService = NinjectWebCommon.Kernel.Get<PeopleService>())
            {
                commandResult = await peopleService.DeleteAsync(id); ;
                if (commandResult.Status == ExecStatus.Success)
                {
                    Response.RedirectToRoute("Admin_ActorList", null);
                    return;
                }
            }
        }
    }
}