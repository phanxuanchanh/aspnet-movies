using Common;
using Common.Web;
using Data.DTO;
using Data.Services;
using Ninject;
using System;
using System.Threading.Tasks;
using Web.Shared.Result;
using Web.Shared.WebForms;

namespace Web.Admin.CountryManagement
{
    public partial class CountryList : AdminPage
    {
        [Inject]
        public FilmMetadataService FilmMetadataService { get; set; }

        protected PagedList<CountryDto> paged;

        protected async void Page_Load(object sender, EventArgs e)
        {
            hyplnkCreate.NavigateUrl = GetRouteUrl("Admin_EditCountry", new { action = "create" });
            paged = new PagedList<CountryDto>();

            GetPagnationQuery();

            if (!IsPostBack)
            {
                txtPageSize.Text = paged.PageSize.ToString();
                await SetCountryTable();
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

        private async Task SetCountryTable()
        {
            paged = await FilmMetadataService
                .GetCountriesAsync(paged.CurrentPage, paged.PageSize, txtSearch.Text);

            rptCountries.DataSource = paged.Items;
            rptCountries.DataBind();

            pagination.SetAndRender(paged);
            pagination.SetUrl("Admin_CountryList");
        }

        protected async void lnkDelete_Command(object sender, System.Web.UI.WebControls.CommandEventArgs e)
        {
            string strId = e.CommandArgument.ToString();
            if (string.IsNullOrEmpty(strId))
                return;

            if (int.TryParse(strId, out int id))
            {
                ExecResult commandResult = await FilmMetadataService.DeleteAsync(id);
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
                Response.RedirectToRoute("Admin_CountryList", new { page = paged.CurrentPage, pageSize = paged.PageSize });
            else
                notifControl.Set(ExecResult.Failure("PageSize không hợp lệ!"));
        }

        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            Response.RedirectToRoute("Admin_CountryList", new { page = paged.CurrentPage, pageSize = paged.PageSize, searchText = txtSearch.Text });
        }
    }
}