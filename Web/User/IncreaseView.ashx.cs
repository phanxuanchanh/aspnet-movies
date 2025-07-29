using Data.BLL;
using Data.Services;
using Ninject;
using System;
using System.Web;
using Web.App_Start;

namespace Web.User
{
    /// <summary>
    /// Summary description for IncreaseView
    /// </summary>
    public class IncreaseView : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            try
            {
                string filmId = context.Request.Form["filmId"];
                if (string.IsNullOrEmpty(filmId))
                {
                    context.Response.Write("Không thể thực hiện. Lý do: Dữ liệu đầu vào không hợp lệ");
                    return;
                }

                using (FilmService filmService = NinjectWebCommon.Kernel.Get<FilmService>())
                {
                    //UpdateState state = UpdateState.Failed;// filmBLL.IncreaseView(filmId);
                    //if (state == UpdateState.Success)
                    //    context.Response.Write("Đã tăng lượt xem");
                    //else
                    //    context.Response.Write("Lỗi tăng lượt xem");
                }
            }
            catch (Exception ex)
            {
                context.Response.Write(string.Format("Đã xảy ra ngoại lệ: {0}", ex.Message));
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}