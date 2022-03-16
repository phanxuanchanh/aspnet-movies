using Data.BLL;
using Data.DTO;
using System;
using System.Threading.Tasks;
using System.Web.UI;
using Web.Models;

namespace Web.Admin.LanguageManagement
{
    public partial class DeleteLanguage : System.Web.UI.Page
    {
        protected LanguageInfo languageInfo;
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
                hyplnkList.NavigateUrl = GetRouteUrl("Admin_LanguageList", null);

                if (CheckLoggedIn())
                {
                    if (!IsPostBack)
                    {
                        await GetLanguageInfo();
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

        private int GetLanguageId()
        {
            object obj = Page.RouteData.Values["id"];
            if (obj == null)
                return -1;
            return int.Parse(obj.ToString());
        }

        private async Task GetLanguageInfo()
        {
            int id = GetLanguageId();
            if (id <= 0)
            {
                Response.RedirectToRoute("Admin_LanguageList", null);
            }
            else
            {
                using(LanguageBLL languageBLL = new LanguageBLL())
                {
                    languageInfo = await languageBLL.GetLanguageAsync(id);
                }
                
                if (languageInfo == null)
                    Response.RedirectToRoute("Admin_LanguageList", null);
                else
                    enableShowInfo = true;
            }
        }

        private async Task DeleteLanguageInfo()
        {
            int id = GetLanguageId();
            DeletionState state;
            using(LanguageBLL languageBLL = new LanguageBLL())
            {
                state = await languageBLL.DeleteLanguageAsync(id);
            }

            if (state == DeletionState.Success)
            {
                stateString = "Success";
                stateDetail = "Đã xóa ngôn ngữ thành công";
            }
            else if (state == DeletionState.Failed)
            {
                stateString = "Failed";
                stateDetail = "Xóa ngôn ngữ thất bại";
            }
            else
            {
                stateString = "ConstraintExists";
                stateDetail = "Không thể xóa ngôn ngữ. Lý do: Ngôn ngữ này đang được sử dụng!";
            }
            enableShowResult = true;
        }

        protected async void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                await DeleteLanguageInfo();
            }
            catch(Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }
    }
}