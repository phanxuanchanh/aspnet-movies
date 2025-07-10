using Common.SystemInformation;
using System;
using System.Threading.Tasks;
using Data.BLL;
using Common.Web;
using Web.Models;

namespace Web.Admin
{
    public partial class Index : System.Web.UI.Page
    {
        protected SystemInfo systemInfo;
        protected long pageVisitor;
        protected long movieNumber;
        protected long categoryNumber;
        protected long tagNumber;
        protected bool enableShowDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            pageVisitor = PageVisitor.Views;
            enableShowDetail = false;
            try
            {
                if (CheckLoggedIn())
                {
                    systemInfo = new SystemInfo();
                    await LoadOverview();
                    enableShowDetail = true;
                }
                else
                {
                    Response.RedirectToRoute("Account_Login", null);
                }
            }
            catch(Exception ex)
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

        private async Task LoadOverview()
        {
            movieNumber = 0;// await filmBLL.CountAllAsync();
            categoryNumber = 0;// await new CategoryBLL(filmBLL).CountAllAsync();
            tagNumber = 0; // await new TagBLL(filmBLL).CountAllAsync();
        }
    }
}