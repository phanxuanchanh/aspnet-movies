using Data.BLL;
using Data.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web.Models;

namespace Web.Admin.CountryManagement
{
    public partial class DeleteCountry : System.Web.UI.Page
    {
        protected CountryInfo countryInfo;
        protected bool enableShowInfo;
        protected bool enableShowResult;
        protected string stateString;
        protected string stateDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            enableShowInfo = false;
            enableShowResult = false;
            stateString = null;
            stateDetail = null;
            try
            {
                hyplnkList.NavigateUrl = GetRouteUrl("Admin_CountryList", null);

                if (CheckLoggedIn())
                {
                    if (!IsPostBack)
                    {
                        await GetCountryInfo();
                    }
                }
                else
                {
                    Response.RedirectToRoute("Account_Login", null);
                }   
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

        private async Task GetCountryInfo()
        {
            int id = GetCountryId();
            if (id <= 0)
            {
                Response.RedirectToRoute("Admin_CountryList", null);
            }
            else
            {
                using(CountryBLL countryBLL = new CountryBLL())
                {
                    countryInfo = await countryBLL.GetCountryAsync(id);
                }
                
                if (countryInfo == null)
                    Response.RedirectToRoute("Admin_CountryList", null);
                else
                    enableShowInfo = true;
            }
        }

        private async Task DeleteCountryInfo()
        {
            int id = GetCountryId();
            DeletionState state;
            using(CountryBLL countryBLL = new CountryBLL())
            {
                state = await countryBLL.DeleteCountryAsync(id);
            }

            if (state == DeletionState.Success)
            {
                stateString = "Success";
                stateDetail = "Đã xóa quốc gia thành công";
            }
            else if (state == DeletionState.Failed)
            {
                stateString = "Failed";
                stateDetail = "Xóa quốc gia thất bại";
            }
            else
            {
                stateString = "ConstraintExists";
                stateDetail = "Không thể xóa quốc gia. Lý do: Quốc gia này đang được sử dụng!";
            }
            enableShowResult = true;
        }

        protected async void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                await DeleteCountryInfo();
            }
            catch(Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }
    }
}