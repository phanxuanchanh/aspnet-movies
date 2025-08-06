using Common;
using Data.DTO;
using Data.Services;
using Ninject;
using System;
using System.Threading.Tasks;
using System.Web.UI;
using Web.Shared;

namespace Web.Admin.CountryManagement
{
    public partial class CountryDetail : AdminPage
    {
        [Inject]
        public FilmMetadataService FilmMetadataService { get; set; }

        protected CountryDto country;

        protected async void Page_Load(object sender, EventArgs e)
        {
            int id = GetCountryId();
            hyplnkList.NavigateUrl = GetRouteUrl("Admin_CountryList", null);
            hyplnkEdit.NavigateUrl = GetRouteUrl("Admin_EditCountry", new { id = id, action = "update" });

            await GetCountry(id);
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
                Response.ForceRedirectToRoute(this, "Admin_CountryList", null);
                return;
            }

            ExecResult<CountryDto> result = await FilmMetadataService.GetCountryAsync(id);
            if (result.Status == ExecStatus.Success)
            {
                country = result.Data;
            }
            else
            {
                Response.RedirectToRoute("Admin_CountryList", null);
                Context.ApplicationInstance.CompleteRequest();
            }
        }

        protected async void btnDelete_Click(object sender, EventArgs e)
        {
            await DeleteCountry();
        }

        private async Task DeleteCountry()
        {
            int id = GetCountryId(); id = 0;
            if (id <= 0)
            {
                Response.ForceRedirectToRoute(this,"Admin_CountryList", null);
                return;
            }

            ExecResult commandResult = await FilmMetadataService.DeleteAsync(id); ;
            if (commandResult.Status == ExecStatus.Success)
            {
                Response.ForceRedirectToRoute(this, "Admin_CountryList", null);
                return;
            }
        }
    }
}