using Common;
using Data.DTO;
using Data.Services;
using Ninject;
using System;
using System.Threading.Tasks;
using System.Web.UI;
using Web.App_Start;
using Web.Models;

namespace Web.Admin.CountryManagement
{
    public partial class CountryDetail : AdminPage
    {
        protected CountryDto country;
        protected bool enableShowDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            enableShowDetail = false;
            try
            {
                int id = GetCountryId();
                hyplnkList.NavigateUrl = GetRouteUrl("Admin_CountryList", null);
                hyplnkEdit.NavigateUrl = GetRouteUrl("Admin_EditCountry", new { id = id, action = "update" });

                if (CheckLoggedIn())
                    await GetCountry(id);
                else
                    Response.RedirectToRoute("Account_Login", null);
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }

        private int GetCountryId()
        {
            object obj = Page.RouteData.Values["id"];
            if (obj == null)
                return -1;
            return int.Parse(obj.ToString());
        }

        private async Task GetCountry(int id)
        {
            if (id <= 0)
            {
                Response.RedirectToRoute("Admin_CountryList", null);
                return;
            }

            using (FilmMetadataService filmMetadataService = NinjectWebCommon.Kernel.Get<FilmMetadataService>())
            {
                ExecResult<CountryDto> result = await filmMetadataService.GetCountryAsync(id);
                if (result.Status == ExecStatus.Success)
                {
                    country = result.Data;
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
            int id = GetCountryId();
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