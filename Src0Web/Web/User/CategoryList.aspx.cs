using Data.DTO;
using Data.Services;
using Ninject;
using System;
using Web.Shared.Result;

namespace Web.User
{
    public partial class CategoryList : GeneralPage
    {
        [Inject]
        public TaxonomyService TaxonomyService { get; set; }

        protected async void Page_Load(object sender, EventArgs e)
        {
            PagedList<CategoryDto> paged = await TaxonomyService.GetCategoriesAsync();
            foreach (CategoryDto category in paged.Items)
            {
                category.Url = GetRouteUrl("User_FilmsByCategory", new { slug = category.Name.TextToUrl(), id = category.ID });
            }

            rptCategories.DataSource = paged.Items;
            rptCategories.DataBind();
        }
    }
}