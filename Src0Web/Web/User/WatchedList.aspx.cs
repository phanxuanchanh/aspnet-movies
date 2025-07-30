using System;
using Web.Models;

namespace Web.User
{
    public partial class WatchedList : System.Web.UI.Page
    {
        private UserSession userSession;
        protected bool enableShowButton;

        protected void Page_Load(object sender, EventArgs e)
        {
            enableShowButton = false;
            try
            {
                GetUserSession();
                if (!IsPostBack && userSession != null)
                {
                    if (userSession.Histories != null)
                    {
                        enableShowButton = true;
                        SetDlWatchedList();
                    }
                }
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }

        private void GetUserSession()
        {
            object obj = Session["userSession"];
            if (obj == null)
                txtState.InnerText = "Danh sách phim đã xem chỉ có khi bạn đăng nhập!";
            else
                userSession = (UserSession)obj;
        }

        private void SetDlWatchedList()
        {
            dlWatchedList.DataSource = userSession.Histories;
            dlWatchedList.DataBind();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (userSession.Histories.Count > 0)
                userSession.Histories.Clear();
            SetDlWatchedList();
        }
    }
}