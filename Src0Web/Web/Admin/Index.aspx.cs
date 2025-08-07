using Common.SystemInformation;
using System;
using System.Threading.Tasks;
using Common.Web;
using System.Collections.Generic;
using Data.Services;
using Web.Shared;

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
            mediaServerSetting = AppSettingValues.GetAndParseValue<object>("cdn-server");

            mediaServerSetting.Remove("ClientId");
            mediaServerSetting.Remove("SecretKey");
        }

        private async Task LoadOverview()
        {
            FilmService filmService = Inject<FilmService>();
            TaxonomyService taxonomyService = Inject<TaxonomyService>();

            movieNumber = 0;
            categoryNumber = 0;
            tagNumber = 0;
        }
    }
}