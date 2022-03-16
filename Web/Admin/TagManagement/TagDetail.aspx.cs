using Data.BLL;
using Data.DTO;
using System;
using System.Threading.Tasks;
using System.Web.UI;
using Web.Models;

namespace Web.Admin.TagManagement
{
    public partial class TagDetail : System.Web.UI.Page
    {
        protected TagInfo tagInfo;
        protected bool enableShowDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            enableShowDetail = false;
            try
            {
                long id = GetTagId();
                hyplnkList.NavigateUrl = GetRouteUrl("Admin_TagList", null);
                hyplnkEdit.NavigateUrl = GetRouteUrl("Admin_UpdateTag", new { id = id });
                hyplnkDelete.NavigateUrl = GetRouteUrl("Admin_DeleteTag", new { id = id });
                
                if(CheckLoggedIn())
                    await GetTagInfo(id);
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

        private long GetTagId()
        {
            object obj = Page.RouteData.Values["id"];
            if (obj == null)
                return -1;
            return long.Parse(obj.ToString());
        }

        private async Task GetTagInfo(long id)
        {
            if (id <= 0)
            {
                Response.RedirectToRoute("Admin_TagList", null);
            }
            else
            {
                using(TagBLL tagBLL = new TagBLL())
                {
                    tagBLL.IncludeDescription = true;
                    tagBLL.IncludeTimestamp = true;
                    tagInfo = await tagBLL.GetTagAsync(id);
                }
                
                if (tagInfo == null)
                    Response.RedirectToRoute("Admin_TagList", null);
                else
                    enableShowDetail = true;
            }
        }
    }
}