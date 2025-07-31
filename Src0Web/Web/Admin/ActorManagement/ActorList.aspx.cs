using Common;
using Common.Web;
using Data.DTO;
using Data.Services;
using Ninject;
using System;
using System.Threading.Tasks;
using Web.App_Start;

namespace Web.Admin.ActorManagement
{
    public partial class ActorList : AdminPage
    {
        private PeopleService _peopleService;
        protected PagedList<ActorDto> paged;

        protected async void Page_Load(object sender, EventArgs e)
        {
            _peopleService = NinjectWebCommon.Kernel.Get<PeopleService>();
            hyplnkCreate.NavigateUrl = GetRouteUrl("Admin_EditActor", new { action = "create" });
            paged = new PagedList<ActorDto>();
            
            if (!CheckLoggedIn())
            {
                Response.RedirectToRoute("Account_Login", null);
                return;
            }

            GetPagnationQuery();

            if (!IsPostBack)
            {
                txtPageSize.Text = paged.PageSize.ToString();
                await SetActorTable();
            }
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            if (_peopleService != null)
            {
                _peopleService.Dispose();
                _peopleService = null;
            }
        }

        private void GetPagnationQuery()
        {
            string pageQuery = Request.QueryString["page"];
            string sizeQuery = Request.QueryString["pageSize"];

            if (string.IsNullOrEmpty(pageQuery))
                paged.CurrentPage = 1;
            else
            {
                if (long.TryParse(pageQuery, out long page))
                    paged.CurrentPage = Math.Max(1, page);
                else
                    paged.CurrentPage = 1;
            }

            if (string.IsNullOrEmpty(sizeQuery))
                paged.PageSize = 20;
            else
            {
                if (long.TryParse(sizeQuery, out long size))
                    paged.PageSize = Math.Max(1, size);
                else
                    paged.PageSize = 20;
            }

            txtSearch.Text = Request.QueryString["searchText"] ?? string.Empty;
        }

        private async Task SetActorTable()
        {
            paged = await _peopleService
                .GetActorsAsync(paged.CurrentPage, paged.PageSize, txtSearch.Text);

            rptActors.DataSource = paged.Items;
            rptActors.DataBind();

            pagination.SetAndRender(paged);
            pagination.SetUrl("Admin_ActorList");
        }

        protected async void lnkDelete_Command(object sender, System.Web.UI.WebControls.CommandEventArgs e)
        {
            string strId = e.CommandArgument.ToString();
            if (string.IsNullOrEmpty(strId))
                return;

            if (int.TryParse(strId, out int id))
            {
                ExecResult commandResult = await _peopleService.DeleteAsync(id);
                notifControl.Set(commandResult);
            }
            else
            {
                notifControl.Set(ExecResult.Failure("ID không hợp lệ!"));
                return;
            }
        }

        protected void txtPageSize_TextChanged(object sender, EventArgs e)
        {
            if (long.TryParse(txtPageSize.Text, out long size))
                Response.RedirectToRoute("Admin_ActorList", new { page = paged.CurrentPage, pageSize = paged.PageSize });
            else
                notifControl.Set(ExecResult.Failure("PageSize không hợp lệ!"));
        }

        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            Response.RedirectToRoute("Admin_ActorList", new { page = paged.CurrentPage, pageSize = paged.PageSize, searchText = txtSearch.Text });
        }
    }
}