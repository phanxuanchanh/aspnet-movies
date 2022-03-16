using Data.BLL;
using Data.DTO;
using System;
using System.Threading.Tasks;
using System.Web.UI;
using Web.Models;
using Web.Validation;

namespace Web.Admin.CountryManagement
{
    public partial class UpdateCountry : System.Web.UI.Page
    {
        private CountryBLL countryBLL;
        protected CountryInfo countryInfo;
        private CustomValidation customValidation;
        protected bool enableShowResult;
        protected string stateString;
        protected string stateDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            countryBLL = new CountryBLL();
            customValidation = new CustomValidation();
            enableShowResult = false;
            stateString = null;
            stateDetail = null;
            try
            {
                int id = GetCountryId();
                hyplnkList.NavigateUrl = GetRouteUrl("Admin_CountryList", null);
                hyplnkDetail.NavigateUrl = GetRouteUrl("Admin_CountryDetail", new { id = id });
                hyplnkDelete.NavigateUrl = GetRouteUrl("Admin_DeleteCountry", new { id = id });
                InitValidation();

                if (CheckLoggedIn())
                {
                    if (IsPostBack)
                    {
                        if (IsValidData())
                        {
                            await Update();
                            await LoadCountryInfo(id);
                        }
                    }
                    else
                    {
                        await LoadCountryInfo(id);
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
            countryBLL.Dispose();
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

        private async Task LoadCountryInfo(int id)
        {
            if (id <= 0)
            {
                Response.RedirectToRoute("Admin_CountryList", null);
            }
            else
            {
                countryBLL.IncludeDescription = true;
                CountryInfo countryInfo = await countryBLL.GetCountryAsync(id);
                if (countryInfo == null)
                {
                    Response.RedirectToRoute("Admin_CountryList", null);
                }
                else
                {
                    hdCountryId.Value = countryInfo.ID.ToString();
                    txtCountryName.Text = countryInfo.name;
                    txtCountryDescription.Text = countryInfo.description;
                }
            }
        }

        private void InitValidation()
        {
            customValidation.Init(
                cvCountryName,
                "txtCountryName",
                "Tên quốc gia không hợp lệ",
                true,
                null,
                customValidation.ValidateCountryName
            );
        }

        private void ValidateData()
        {
            cvCountryName.Validate();
        }

        private bool IsValidData()
        {
            ValidateData();
            return cvCountryName.IsValid;
        }

        private CountryUpdate GetCountryUpdate()
        {
            return new CountryUpdate
            {
                ID = int.Parse(Request.Form[hdCountryId.UniqueID]),
                name = Request.Form[txtCountryName.UniqueID],
                description = Request.Form[txtCountryDescription.UniqueID]
            };
        }

        private async Task Update()
        {
            CountryUpdate countryUpdate = GetCountryUpdate();
            UpdateState state = await countryBLL.UpdateCountryAsync(countryUpdate);
            if (state == UpdateState.Success)
            {
                stateString = "Success";
                stateDetail = "Đã cập nhật quốc gia thành công";
            }
            else
            {
                stateString = "Failed";
                stateDetail = "Cập nhật quốc gia thất bại";
            }
            enableShowResult = true;
        }
    }
}