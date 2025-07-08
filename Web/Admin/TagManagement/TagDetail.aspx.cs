using Common;
using Data.BLL;
using Data.DAL;
using Data.DTO;
using Data.Services;
using Ninject;
using System;
using System.Threading.Tasks;
using System.Web.UI;
using Web.App_Start;
using Web.Models;

namespace Web.Admin.TagManagement
{
    public partial class TagDetail : System.Web.UI.Page
    {
        protected TagDto tag;
        protected ExecResult commandResult;
        protected bool enableShowDetail;
        protected bool enableShowResult;

        protected async void Page_Load(object sender, EventArgs e)
        {
            enableShowDetail = false;
            try
            {
                int id = GetTagId();
                hyplnkList.NavigateUrl = GetRouteUrl("Admin_TagList", null);
                hyplnkEdit.NavigateUrl = GetRouteUrl("Admin_EditTag", new { id = id, action = "update" });

                if (CheckLoggedIn())
                    await GetTag(id);
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

        private int GetTagId()
        {
            object obj = Page.RouteData.Values["id"];
            if (obj == null)
                return -1;
            return int.Parse(obj.ToString());
        }

        private async Task GetTag(int id)
        {
            if (id <= 0)
            {
                Response.RedirectToRoute("Admin_TagList", null);
                return;
            }

            using (TaxonomyService taxonomyService = NinjectWebCommon.Kernel.Get<TaxonomyService>())
            {
                ExecResult<TagDto> result = await taxonomyService.GetTagAsync(id);
                if (result.Status == ExecStatus.Success)
                {
                    tag = result.Data;
                    enableShowDetail = true;
                }
                else
                {
                    Response.RedirectToRoute("Admin_TagList", null);
                }
            }
        }

        protected async void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                await DeleteTag();
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }

        private async Task DeleteTag()
        {
            int id = GetTagId();
            if (id <= 0)
            {
                Response.RedirectToRoute("Admin_TagList", null);
                return;
            }

            using (TaxonomyService taxonomyService = NinjectWebCommon.Kernel.Get<TaxonomyService>())
            {
                commandResult = await taxonomyService.DeleteAsync(id); ;
                if (commandResult.Status == ExecStatus.Success)
                {
                    Response.RedirectToRoute("Admin_TagList", null);
                    return;
                }
            }
        }
    }
}