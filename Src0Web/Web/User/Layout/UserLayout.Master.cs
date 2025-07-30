using Data.BLL;
using System;
using System.Collections.Generic;
using Data.DTO;
using System.Linq;
using Web.Models;
using Data.Services;
using Web.App_Start;
using Ninject;
using System.Threading.Tasks;

namespace Web.User.Layout
{
    public partial class UserLayout : System.Web.UI.MasterPage
    {
        protected List<CategoryDto> categories;
        protected string hyplnkSearch;
        protected string hyplnkWatchedList;
        protected string hyplnkLiteVersion;
        protected string hyplnkHome;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                object obj = Session["userSession"];
                if (obj == null)
                {
                    hyplnkAccount.HRef = GetRouteUrl("Account_Login", null);
                    hyplnkAccount.InnerText = "Đăng nhập / Đăng ký";
                }
                else
                {
                    hyplnkAccount.HRef = GetRouteUrl("Account_Logout", null);
                    hyplnkAccount.InnerText = "Đăng xuất";
                }
                hyplnkSearch = GetRouteUrl("User_Search", null);
                hyplnkWatchedList = GetRouteUrl("User_WatchedList", null);
                hyplnkHome = GetRouteUrl("User_Home", null);
                GetCategories();
            }
            catch(Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }

        private void GetCategories()
        {
            using(TaxonomyService taxonomyService = NinjectWebCommon.Kernel.Get<TaxonomyService>())
            {
                categories = new List<CategoryDto>()// (taxonomyService.GetCategoriesAsync(1, 30).Result).Items
                .Select(s => {
                    s.Url = GetRouteUrl("User_FilmsByCategory", new { slug = s.Name.TextToUrl(), id = s.ID }); 
                    return s;
                }).ToList();
            }
        }
    }
}