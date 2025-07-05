using Common.Web;
using Data.BLL;
using Data.DTO;
using Data.Services;
using Ninject;
using System;
using System.Threading.Tasks;
using Web.App_Start;
using Web.Models;

namespace Web.Admin.CastManagement
{
    public partial class CastList : System.Web.UI.Page
    {
        private ActorService actorService;

        private CastBLL castBLL;
        protected long currentPage;
        protected long pageNumber;
        protected bool enableTool;
        protected string toolDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            actorService = NinjectWebCommon.Kernel.Get<ActorService>();
            castBLL = new CastBLL();
            enableTool = false;
            toolDetail = null;
            try
            {
                hyplnkCreate.NavigateUrl = GetRouteUrl("Admin_CreateCast", null);

                if (CheckLoggedIn())
                {
                    if (!IsPostBack)
                    {
                        await SetGrvCast();
                        SetDrdlPage();
                        castBLL.Dispose();
                    }
                }
                else
                {
                    Response.RedirectToRoute("Account_Login", null);
                    castBLL.Dispose();
                }
            }
            catch (Exception ex)
            {
                castBLL.Dispose();
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
                await SetGrvCast();
                SetDrdlPage();
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
            castBLL.Dispose();
        }

        private async Task SetGrvCast()
        {
            castBLL.IncludeTimestamp = true;
            PagedList<ActorDto> casts = await castBLL
                .GetCastsAsync(drdlPage.SelectedIndex, 20);
            grvCast.DataSource = casts.Items;
            grvCast.DataBind();

            pageNumber = casts.PageNumber;
            currentPage = casts.CurrentPage;
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

        protected async void grvCast_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                long key = (long)grvCast.DataKeys[grvCast.SelectedIndex].Value;
                ActorDto castInfo = await castBLL.GetCastAsync(key);
                toolDetail = string.Format("{0} -- {1}", castInfo.ID, castInfo.Name);
                hyplnkDetail.NavigateUrl = GetRouteUrl("Admin_CastDetail", new { id = castInfo.ID });
                hyplnkEdit.NavigateUrl = GetRouteUrl("Admin_UpdateCast", new { id = castInfo.ID });
                hyplnkDelete.NavigateUrl = GetRouteUrl("Admin_DeleteCast", new { id = castInfo.ID });
                enableTool = true;
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
            castBLL.Dispose();
        }
    }
}