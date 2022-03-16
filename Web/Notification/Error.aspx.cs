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
            error = Session["error"] as ErrorModel;
            if (error == null)
                error = new ErrorModel
                {
                    ErrorTitle = "Không có lỗi!",
                    ErrorDetail = "Đây là nội dung mặc định khi bạn cố truy cập vào trang này. Lỗi chi tiết sẽ hiện khi thực sự có lỗi xảy ra"
                };
        }
    }
}