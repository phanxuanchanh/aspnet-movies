using System;
using Web.Models;

namespace Web.Notification
{
    public partial class Error : System.Web.UI.Page
    {
        protected ErrorModel error;
        protected void Page_Load(object sender, EventArgs e)
        {
            GetError();
            hyplnkHome.NavigateUrl = GetRouteUrl("User_Home", null);
        }

        private void GetError()
        {
            error = new ErrorModel
            {
                ErrorTitle = "Lỗi đã xảy ra!",
                ErrorDetail = "Đã có lỗi xảy ra, vui lòng thử lại sau, có thể do hệ thống hoặc đã bị chặn truy cập vào!"
            };
        }
    }
}