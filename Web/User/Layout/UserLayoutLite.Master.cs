using Data.BLL;
using Data.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using Web.Models;

namespace Web.User.Layout
{
    public partial class UserLayoutLite : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                LoadMainMenu();
                LoadCategories();
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }

        private void LoadMainMenu()
        {
            mainMenu.Items.Add(new MenuItem { Text = "Trang chủ", NavigateUrl = GetRouteUrl("User_Home_Lite", null) });
            mainMenu.Items.Add(new MenuItem { Text = "Danh sách thể loại", NavigateUrl = GetRouteUrl("User_CategoryList", null) });
            mainMenu.Items.Add(new MenuItem { Text = "Lịch sử xem", NavigateUrl = GetRouteUrl("User_WatchedList", null) });
            
            object obj = Session["userSession"];
            if (obj == null)
                mainMenu.Items.Add(new MenuItem { Text = "Đăng nhập / Đăng ký", NavigateUrl = GetRouteUrl("Account_Login", null) });
            else
                mainMenu.Items.Add(new MenuItem { Text = "Đăng xuất", NavigateUrl = GetRouteUrl("Account_Logout", null) });
        }

        private void LoadCategories()
        {

            using (CategoryBLL categoryBLL = new CategoryBLL())
            {
                List<CategoryInfo> categoryInfos = categoryBLL.GetCategories().Select(c => new CategoryInfo
                {
                    ID = c.ID,
                    name = c.name,
                    description = c.description,
                    url = GetRouteUrl("User_FilmsByCategory_Lite", new { slug = c.name.TextToUrl(), id = c.ID })
                }).ToList();

                grvCategory.DataSource = categoryInfos;
                grvCategory.DataBind();
            }
        }
    }
}