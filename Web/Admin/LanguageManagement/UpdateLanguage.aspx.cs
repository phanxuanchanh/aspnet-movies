using Data.BLL;
using Data.DTO;
using System;
using System.Threading.Tasks;
using System.Web.UI;
using Web.Models;
using Web.Validation;

namespace Web.Admin.LanguageManagement
{
    public partial class UpdateLanguage : System.Web.UI.Page
    {
        private LanguageBLL languageBLL;
        protected LanguageInfo languageInfo;
        private CustomValidation customValidation;
        protected bool enableShowResult;
        protected string stateString;
        protected string stateDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            languageBLL = new LanguageBLL();
            customValidation = new CustomValidation();
            enableShowResult = false;
            stateString = null;
            stateDetail = null;
            try
            {
                int id = GetLanguageId();
                hyplnkList.NavigateUrl = GetRouteUrl("Admin_LanguageList", null);
                hyplnkDetail.NavigateUrl = GetRouteUrl("Admin_LanguageDetail", new { id = id });
                hyplnkDelete.NavigateUrl = GetRouteUrl("Admin_DeleteLanguage", new { id = id });
                InitValidation();

                if (CheckLoggedIn())
                {
                    if (IsPostBack)
                    {
                        if (IsValidData())
                        {
                            await Update();
                            await LoadLanguageInfo(id);
                        }
                    }
                    else
                    {
                        await LoadLanguageInfo(id);
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
            languageBLL.Dispose();
        }

        private bool CheckLoggedIn()
        {
            object obj = Session["userSession"];
            if (obj == null)
                return false;

            UserSession userSession = (UserSession)obj;
            return (userSession.role == "Admin" || userSession.role == "Editor");
        }

        private int GetLanguageId()
        {
            object obj = Page.RouteData.Values["id"];
            if (obj == null)
                return -1;
            return int.Parse(obj.ToString());
        }

        private async Task LoadLanguageInfo(int id)
        {
            if (id <= 0)
            {
                Response.RedirectToRoute("Admin_LanguageList", null);
            }
            else
            {
                languageBLL.IncludeDescription = true;
                LanguageInfo languageInfo = await languageBLL.GetLanguageAsync(id);
                if (languageInfo == null)
                {
                    Response.RedirectToRoute("Admin_LanguageList", null);
                }
                else
                {
                    hdLanguageId.Value = languageInfo.ID.ToString();
                    txtLanguageName.Text = languageInfo.name;
                    txtLanguageDescription.Text = languageInfo.description;
                }
            }
        }

        private void InitValidation()
        {
            customValidation.Init(
                cvLanguageName,
                "txtLanguageName",
                "Tên ngôn ngữ không hợp lệ",
                true,
                null,
                customValidation.ValidateLanguageName
            );
        }

        private void ValidateData()
        {
            cvLanguageName.Validate();
        }

        private bool IsValidData()
        {
            ValidateData();
            return cvLanguageName.IsValid;
        }

        private LanguageUpdate GetLanguageUpdate()
        {
            return new LanguageUpdate
            {
                ID = int.Parse(Request.Form[hdLanguageId.UniqueID]),
                name = Request.Form[txtLanguageName.UniqueID],
                description = Request.Form[txtLanguageDescription.UniqueID]
            };
        }

        private async Task Update()
        {
            LanguageUpdate languageUpdate = GetLanguageUpdate();
            UpdateState state = await languageBLL.UpdateLanguageAsync(languageUpdate);
            if (state == UpdateState.Success)
            {
                stateString = "Success";
                stateDetail = "Đã cập nhật ngôn ngữ thành công";
            }
            else
            {
                stateString = "Failed";
                stateDetail = "Cập nhật ngôn ngữ thất bại";
            }
            enableShowResult = true;
        }
    }
}