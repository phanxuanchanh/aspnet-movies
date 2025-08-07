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

        private int id;
        protected CountryDto country;

        protected async void Page_Load(object sender, EventArgs e)
        {
            id = GetId<int>();
            if (id <= 0)
            {
                Response.ForceRedirectToRoute(this, "Admin_CountryList", null);
                return;
            }

            hyplnkList.NavigateUrl = GetRouteUrl("Admin_CountryList", null);
            hyplnkEdit.NavigateUrl = GetRouteUrl("Admin_EditCountry", new { id = id, action = "update" });

            await GetCountry(id);
        }

        private async Task GetCountry(int id)
        {
            ExecResult<CountryDto> result = await FilmMetadataService.GetCountryAsync(id);
            if (result.Status == ExecStatus.Success)
            {
                country = result.Data;
            }
            else
            {
                Response.ForceRedirectToRoute(this, "Admin_CountryList", null);
            }
        }

        protected async void btnDelete_Click(object sender, EventArgs e)
        {
            await DeleteCountry();
        }

        private async Task DeleteCountry()
        {
            ExecResult commandResult = await FilmMetadataService.DeleteAsync(id); ;
            if (commandResult.Status == ExecStatus.Success)
            {
                Response.ForceRedirectToRoute(this, "Admin_CountryList", null);
                return;
            }
            else
            {
                notifControl.Set(commandResult);
            }
        }
    }
}