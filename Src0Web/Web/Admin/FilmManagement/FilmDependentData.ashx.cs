using Data.DTO;
using Data.Services;
using Ninject;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using Web.App_Start;
using Web.Shared.Result;

namespace Web.Admin.FilmManagement
{
    /// <summary>
    /// Summary description for FilmDependentData
    /// </summary>
    public class FilmDependentData : HttpTaskAsyncHandler
    {
        private static readonly string[] Actions = new string[]
        {
            "LoadCategories", "LoadTags", "LoadCountries", "LoadLanguages"
        };

        public override async Task ProcessRequestAsync(HttpContext context)
        {
            context.Response.ContentType = "application/json";

            string action = context.Request.Form["action"];
            bool actionExists = Actions.Any(a => a.Equals(action, StringComparison.OrdinalIgnoreCase));
            if (!actionExists)
            {
                context.Response.StatusCode = 400;
                ExecResult execResult = ExecResult.Failure("Invalid or missing parameters.");
                string json = new JavaScriptSerializer().Serialize(execResult);

                context.Response.Write(json);
            }
            ;

            if (action.Equals(Actions[0], StringComparison.OrdinalIgnoreCase))
            {
                string categorySearchText = context.Request.Form["categorySearchText"];
                string json = await LoadCategories(categorySearchText);

                context.Response.Write(json);
            }
            else if (action.Equals(Actions[1], StringComparison.OrdinalIgnoreCase))
            {
                string tagSearchText = context.Request.Form["tagSearchText"];
                string json = await LoadTags(tagSearchText);
                context.Response.Write(json);
            }
            else if (action.Equals(Actions[2], StringComparison.OrdinalIgnoreCase))
            {
                string countrySearchText = context.Request.Form["countrySearchText"];
                string json = await LoadCountries(countrySearchText);
                context.Response.Write(json);
            }
            else
            {
                string languageSearchText = context.Request.Form["languageSearchText"];
                string json = await LoadLanguages(languageSearchText);
                context.Response.Write(json);
            }

            context.ApplicationInstance.CompleteRequest();
        }

        public override bool IsReusable => false;

        private async Task<string> LoadCategories(string searchText)
        {
            TaxonomyService taxonomyService = NinjectWebCommon.Kernel.Get<TaxonomyService>();
            PagedList<CategoryDto> paged = await taxonomyService
                .GetCategoriesAsync(pageIndex: 1, pageSize: 20, searchText);

            ExecResult<PagedList<CategoryDto>> execResult = ExecResult<PagedList<CategoryDto>>
                .Success("Categories retrieved successfully.", paged);
            string json = new JavaScriptSerializer().Serialize(execResult);

            return json;
        }

        private async Task<string> LoadTags(string searchText)
        {
            TaxonomyService taxonomyService = NinjectWebCommon.Kernel.Get<TaxonomyService>();
            PagedList<TagDto> paged = await taxonomyService
                .GetTagsAsync(pageIndex: 1, pageSize: 20, searchText);

            ExecResult<PagedList<TagDto>> execResult = ExecResult<PagedList<TagDto>>
                .Success("Categories retrieved successfully.", paged);
            string json = new JavaScriptSerializer().Serialize(execResult);

            return json;
        }

        private async Task<string> LoadCountries(string searchText)
        {
            FilmMetadataService filmMetadataService = NinjectWebCommon.Kernel.Get<FilmMetadataService>();
            PagedList<CountryDto> paged = await filmMetadataService
                .GetCountriesAsync(pageIndex: 1, pageSize: 20, searchText);

            ExecResult<PagedList<CountryDto>> execResult = ExecResult<PagedList<CountryDto>>
                .Success("Categories retrieved successfully.", paged);
            string json = new JavaScriptSerializer().Serialize(execResult);

            return json;
        }

        private async Task<string> LoadLanguages(string searchText)
        {
            FilmMetadataService filmMetadataService = NinjectWebCommon.Kernel.Get<FilmMetadataService>();
            PagedList<LanguageDto> paged = await filmMetadataService
                .GetLanguagesAsync(pageIndex: 1, pageSize: 20, searchText);

            ExecResult<PagedList<LanguageDto>> execResult = ExecResult<PagedList<LanguageDto>>
                .Success("Categories retrieved successfully.", paged);
            string json = new JavaScriptSerializer().Serialize(execResult);

            return json;
        }
    }
}