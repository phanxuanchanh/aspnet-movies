using System;
using System.Collections.Generic;
using Data.DTO;
using System.Linq;
using Data.Services;
using Ninject;
using System.Web;

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
            if (HttpContext.Current.User?.Identity?.IsAuthenticated != true)
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

        private void GetCategories()
        {
            categories = new List<CategoryDto>()// (taxonomyService.GetCategoriesAsync(1, 30).Result).Items
            .Select(s =>
            {
                s.Url = GetRouteUrl("User_FilmsByCategory", new { slug = s.Name.TextToUrl(), id = s.ID });
                return s;
            }).ToList();
        }
    }
}