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

namespace Web.Admin.LanguageManagement
{
    public partial class LanguageDetail : AdminPage
    {
        protected LanguageDto language;
        protected bool enableShowDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            enableShowDetail = false;
            try
            {
                int id = GetLanguageId();
                hyplnkList.NavigateUrl = GetRouteUrl("Admin_LanguageList", null);
                hyplnkEdit.NavigateUrl = GetRouteUrl("Admin_EditLanguage", new { id = id, action = "update" });

                if (CheckLoggedIn())
                    await GetLanguageInfo(id);
                else
                    Response.RedirectToRoute("Account_Login", null);
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }

        private int GetLanguageId()
        {
            object obj = Page.RouteData.Values["id"];
            if (obj == null)
                return -1;
            return int.Parse(obj.ToString());
        }

        private async Task GetLanguageInfo(int id)
        {
            if (id <= 0)
            {
                Response.RedirectToRoute("Admin_LanguageList", null);
                return;
            }

            using (FilmMetadataService filmMetadataService = NinjectWebCommon.Kernel.Get<FilmMetadataService>())
            {
                ExecResult<LanguageDto> result = await filmMetadataService.GetLanguageAsync(id);
                if (result.Status == ExecStatus.Success)
                {
                    language = result.Data;
                    enableShowDetail = true;
                }
                else
                {
                    Response.RedirectToRoute("Admin_LanguageList", null);
                }
            }
        }

        protected async void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                await DeleteLanguage();
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }

        private async Task DeleteLanguage()
        {
            int id = GetLanguageId();
            if (id <= 0)
            {
                Response.RedirectToRoute("Admin_CountryList", null);
                return;
            }

            using (FilmMetadataService filmMetadataService = NinjectWebCommon.Kernel.Get<FilmMetadataService>())
            {
                 ExecResult commandResult = await filmMetadataService.DeleteAsync(id); ;
                if (commandResult.Status == ExecStatus.Success)
                {
                    Response.RedirectToRoute("Admin_CountryList", null);
                    return;
                }
            }
        }
    }
}