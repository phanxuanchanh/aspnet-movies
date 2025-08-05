using Common.SystemInformation;
using System;
using System.Threading.Tasks;
using Common.Web;
using Data.Models;
using System.Linq;
using System.Text.Json;
using System.Collections.Generic;
using Data.Services;
using Web.App_Code;

namespace Web.Admin
{
    public partial class Index : AdminPage
    {
        protected SystemInfo systemInfo;
        protected Dictionary<string, object> mediaServerSetting;
        protected long pageVisitor;
        protected long movieNumber;
        protected long categoryNumber;
        protected long tagNumber;
        protected bool enableShowDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            pageVisitor = PageVisitor.Views;
            enableShowDetail = false;
            systemInfo = new SystemInfo();

            LoadMediaServerSetting();
            await LoadOverview();
            enableShowDetail = true;
        }

        private void LoadMediaServerSetting()
        {
            AppSetting appSetting = AppSettingValues.Get()
                .Where(x => x.Name == "cdn-server").FirstOrDefault();

            mediaServerSetting = JsonSerializer
                .Deserialize<Dictionary<string, object>>(appSetting.Value);

            mediaServerSetting.Remove("ClientId");
            mediaServerSetting.Remove("SecretKey");
        }

        private async Task LoadOverview()
        {
            using (FilmService filmService = Inject<FilmService>())
            {
                movieNumber = 0;
            }

            using (TaxonomyService taxonomyService = Inject<TaxonomyService>())
            {
                categoryNumber = 0;
                tagNumber = 0;
            }
        }
    }
}