using Common;
using Common.Upload;
using Data.DTO;
using Data.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using Web.Shared.Result;

namespace Web.User
{
    public partial class Search : GeneralPage
    {
        protected List<FilmDto> films;
        protected string keyword;

        protected async void Page_Load(object sender, EventArgs e)
        {
            keyword = Request.QueryString["input"];
            if (string.IsNullOrEmpty(keyword))
            {
                Response.RedirectToRoute("User_Home", null);
                return;
            }

            await SearchFilms(keyword);
        }

        public async Task SearchFilms(string searchContent)
        {
            FilmService filmService = Inject<FilmService>();
            PagedList<FilmDto> pagedList = await filmService.GetFilmsAsync(1, 30);

            films = pagedList.Items;

            foreach (FilmDto film in films)
            {
                if (string.IsNullOrEmpty(film.Thumbnail))
                    film.Thumbnail = VirtualPathUtility
                        .ToAbsolute(string.Format("{0}/Default/default.png", FileUpload.ImageFilePath));
                else
                    film.Thumbnail = VirtualPathUtility
                        .ToAbsolute(string.Format("{0}/{1}", FileUpload.ImageFilePath, film.Thumbnail));

                Rating rating = new Rating(film.Upvote, film.Downvote);
                film.ScoreRating = rating.SolveScore();
                film.Url = GetRouteUrl("User_FilmDetail", new { slug = film.Name.TextToUrl(), id = film.ID });
            }
        }
    }
}