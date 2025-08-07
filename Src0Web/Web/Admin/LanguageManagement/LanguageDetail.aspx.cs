using Common;
using Data.DTO;
using Data.Services;
using Ninject;
using System;
using System.Threading.Tasks;
using Web.Shared;

namespace Web.Admin.LanguageManagement
{
    public partial class LanguageDetail : AdminPage
    {
        [Inject]
        public FilmMetadataService FilmMetadataService { get; set; }

        private int id;
        protected LanguageDto language;

        protected async void Page_Load(object sender, EventArgs e)
        {
            id = GetId<int>();
            if (id <= 0)
            {
                Response.ForceRedirectToRoute(this, "Admin_LanguageList", null);
                return;
            }

            hyplnkList.NavigateUrl = GetRouteUrl("Admin_LanguageList", null);
            hyplnkEdit.NavigateUrl = GetRouteUrl("Admin_EditLanguage", new { id = id, action = "update" });

            await GetLanguageInfo(id);
        }

        private async Task GetLanguageInfo(int id)
        {
            ExecResult<LanguageDto> result = await FilmMetadataService.GetLanguageAsync(id);
            if (result.Status == ExecStatus.Success)
                language = result.Data;
            else
                Response.ForceRedirectToRoute(this, "Admin_LanguageList", null);
        }

        protected async void btnDelete_Click(object sender, EventArgs e)
        {
            await DeleteLanguage();
        }

        private async Task DeleteLanguage()
        {
            ExecResult commandResult = await FilmMetadataService.DeleteAsync(id); ;
            if (commandResult.Status == ExecStatus.Success)
            {
                Response.ForceRedirectToRoute(this, "Admin_LanguageList", null);
                return;
            }
            else
            {
                notifControl.Set(commandResult);
            }
        }
    }
}