using Data.BLL;
using Data.DTO;
using System;
using System.Web;

namespace Web.User
{
    /// <summary>
    /// Summary description for UpvoteFilm
    /// </summary>
    public class UpvoteFilm : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //UserReactionBLL userReactionBLL = new UserReactionBLL();

            try
            {
                string filmId = context.Request.Form["filmId"];
                string userId = context.Request.Form["userId"];
                if (string.IsNullOrEmpty(filmId) || string.IsNullOrEmpty(userId))
                {
                    context.Response.Write("Không thể thực hiện. Lý do: Dữ liệu đầu vào không hợp lệ");
                    return;
                }

                //UserInfo userInfo = new UserDao(userReactionBLL).GetUser(userId);
                //FilmBLL filmBLL = new FilmBLL(userReactionBLL);
                FilmDto filmInfo = new FilmDto();// filmBLL.GetFilm(filmId);

                //if (userInfo == null || filmInfo == null)
                //{
                //    context.Response.Write("Người dùng hoặc có thể là phim không tồn tại");
                //    return;
                //}

                //bool upvoteResult = userReactionBLL.Upvote(filmId, userId);
                //if (upvoteResult)
                //{
                //    UpdateState state = UpdateState.Failed;// filmBLL.Upvote(filmId);
                //    if (state == UpdateState.Success)
                //        context.Response.Write("Đánh giá (thích) phim thành công");
                //    else
                //        context.Response.Write("Đánh giá (thích) phim thành công, tuy nhiên đã xảy lỗi nhỏ");
                //}
                //else
                //{
                //    context.Response.Write("Đánh giá (thích) phim thất bại");
                //}
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