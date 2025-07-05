using Data.BLL;
using Data.DTO;
using System;
using System.Threading.Tasks;
using Web.Models;
using Web.Validation;

namespace Web.Admin.CountryManagement
{
    public partial class CreateCountry : System.Web.UI.Page
    {
        private CustomValidation customValidation;
        protected bool enableShowResult;
        protected string stateString;
        protected string stateDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            customValidation = new CustomValidation();
            enableShowResult = false;
            stateString = null;
            stateDetail = null;
            try
            {
                hyplnkList.NavigateUrl = GetRouteUrl("Admin_CountryList", null);
                InitValidation();

                if (CheckLoggedIn())
                {
                    if (IsPostBack)
                    {
                        await Create();
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

        private CreateCountryDto GetCountryCreation()
        {
            return new CreateCountryDto
            {
                Name = Request.Form[txtCountryName.UniqueID],
                Description = Request.Form[txtCountryDescription.UniqueID]
            };
        }

        public async Task Create()
        {
            if (IsValidData())
            {
                CreateCountryDto country = GetCountryCreation();
                CreationState state;
                using(CountryBLL countryBLL = new CountryBLL())
                {
                    state = await countryBLL.CreateCountryAsync(country);
                }

                if (state == CreationState.Success)
                {
                    stateString = "Success";
                    stateDetail = "Đã thêm quốc gia thành công";
                }
                else if (state == CreationState.AlreadyExists)
                {
                    stateString = "AlreadyExists";
                    stateDetail = "Thêm quốc gia thất bại. Lý do: Đã tồn tại quốc gia này";
                }
                else
                {
                    stateString = "Failed";
                    stateDetail = "Thêm quốc gia thất bại";
                }
                enableShowResult = true;
            }
        }
    }
}