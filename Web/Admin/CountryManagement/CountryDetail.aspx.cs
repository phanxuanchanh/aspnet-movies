using Data.BLL;
using Data.DTO;
using System;
using System.Threading.Tasks;
using System.Web.UI;
using Web.Models;

namespace Web.Admin.CountryManagement
{
    public partial class CountryDetail : System.Web.UI.Page
    {
        protected CountryInfo countryInfo;
        protected bool enableShowDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            enableShowDetail = false;
            try
            {
                int id = GetCountryId();
                hyplnkList.NavigateUrl = GetRouteUrl("Admin_CountryList", null);
                hyplnkEdit.NavigateUrl = GetRouteUrl("Admin_UpdateCountry", new { id = id });
                hyplnkDelete.NavigateUrl = GetRouteUrl("Admin_DeleteCountry", new { id = id });

                if (CheckLoggedIn())
                    await GetCountryInfo(id);
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

        private int GetCountryId()
        {
            object obj = Page.RouteData.Values["id"];
            if (obj == null)
                return -1;
            return int.Parse(obj.ToString());
        }

        private async Task GetCountryInfo(int id)
        {
            if (id <= 0)
            {
                Response.RedirectToRoute("Admin_CountryList", null);
            }
            else
            {
                using(CountryBLL countryBLL = new CountryBLL())
                {
                    countryBLL.IncludeDescription = true;
                    countryBLL.IncludeTimestamp = true;
                    countryInfo = await countryBLL.GetCountryAsync(id);
                }
                
                if (countryInfo == null)
                    Response.RedirectToRoute("Admin_CountryList", null);
                else
                    enableShowDetail = true;
            }
        }
    }
}