using Common.Web;
using Data.BLL;
using Data.DTO;
using Ninject;
using System;
using System.Threading.Tasks;
using Web.App_Start;
using Web.Models;

namespace Web.Admin.CountryManagement
{
    public partial class CountryList : System.Web.UI.Page
    {
        private FilmMetadataDao _filmMetadataDao;
        private CountryBLL countryBLL;
        protected long currentPage;
        protected long pageNumber;
        protected bool enableTool;
        protected string toolDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            _filmMetadataDao = NinjectWebCommon.Kernel.Get<FilmMetadataDao>();
            countryBLL = new CountryBLL();
            enableTool = false;
            toolDetail = null;
            try
            {
                hyplnkCreate.NavigateUrl = GetRouteUrl("Admin_CreateCountry", null);

                if (CheckLoggedIn())
                {
                    if (!IsPostBack)
                    {
                        await SetGrvCountry();
                        SetDrdlPage();
                        countryBLL.Dispose();
                    }
                }
                else
                {
                    Response.RedirectToRoute("Account_Login", null);
                    countryBLL.Dispose();
                }
            }
            catch(Exception ex)
            {
                countryBLL.Dispose();
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
            countryBLL.Dispose();
        }

        private async Task SetGrvCountry()
        {
            countryBLL.IncludeTimestamp = true;
            PagedList<CountryDto> countries = await countryBLL
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
                CountryDto countryInfo = await countryBLL.GetCountryAsync(key);
                toolDetail = string.Format("{0} -- {1}", countryInfo.ID, countryInfo.Name);
                hyplnkDetail.NavigateUrl = GetRouteUrl("Admin_CountryDetail", new { id = countryInfo.ID });
                hyplnkEdit.NavigateUrl = GetRouteUrl("Admin_UpdateCountry", new { id = countryInfo.ID });
                hyplnkDelete.NavigateUrl = GetRouteUrl("Admin_DeleteCountry", new { id = countryInfo.ID });
                enableTool = true;
            }
            catch(Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
            countryBLL.Dispose();
        }
    }
}