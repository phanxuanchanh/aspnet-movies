using Common.Web;
using Data.DTO;
using Data.Services;
using Ninject;
using System;
using System.Threading.Tasks;
using Web.App_Start;
using Web.Models;

namespace Web.Admin.CountryManagement
{
    public partial class CountryList : System.Web.UI.Page
    {
        private FilmMetadataService _filmMetadataService;
        protected long currentPage;
        protected long pageNumber;
        protected bool enableTool;
        protected string toolDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            _filmMetadataService = NinjectWebCommon.Kernel.Get<FilmMetadataService>();
            enableTool = false;
            toolDetail = null;
            try
            {
                hyplnkCreate.NavigateUrl = GetRouteUrl("Admin_EditCountry", new { action = "create" });

                if (!CheckLoggedIn())
                {
                    Response.RedirectToRoute("Account_Login", null);
                    return;
                }

                if (!IsPostBack)
                {
                    await SetGrvCountry();
                    SetDrdlPage();
                }
            }
            catch(Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            if (_filmMetadataService != null)
            {
                _filmMetadataService.Dispose();
                _filmMetadataService = null;
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

        protected async void drdlPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                await SetGrvCountry();
                SetDrdlPage();
            }
            catch(Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }

        private async Task SetGrvCountry()
        {
            PagedList<CountryDto> countries = await _filmMetadataService
                .GetCountriesAsync(drdlPage.SelectedIndex, 20);
            grvCountry.DataSource = countries.Items;
            grvCountry.DataBind();

            pageNumber = countries.PageNumber;
            currentPage = countries.CurrentPage;
        }

        private void SetDrdlPage()
        {
            int selectedIndex = drdlPage.SelectedIndex;
            drdlPage.Items.Clear();
            for (int i = 0; i < pageNumber; i++)
            {
                string item = (i + 1).ToString();
                if (i == currentPage)
                    drdlPage.Items.Add(string.Format("{0}*", item));
                else
                    drdlPage.Items.Add(item);
            }
            drdlPage.SelectedIndex = selectedIndex;
        }

        protected async void grvCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int key = (int)grvCountry.DataKeys[grvCountry.SelectedIndex].Value;
                CountryDto country = (await _filmMetadataService.GetCountryAsync(key)).Data;
                toolDetail = string.Format("{0} -- {1}", country.ID, country.Name);
                hyplnkDetail.NavigateUrl = GetRouteUrl("Admin_CountryDetail", new { id = country.ID });
                hyplnkEdit.NavigateUrl = GetRouteUrl("Admin_EditCountry", new { id = country.ID, action = "update" });
                enableTool = true;
            }
            catch(Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }
    }
}