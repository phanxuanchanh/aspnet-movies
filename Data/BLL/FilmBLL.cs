using Common.Hash;
using Common.Web;
using Data.DAL;
using Data.DTO;
using MSSQL.Access;
using MSSQL.Query;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Data.BLL
{
    public class FilmBLL : BusinessLogicLayer
    {
        private bool disposed;
        private bool includeCategory;
        private bool includeTag;
        private bool includeLanguage;
        private bool includeCountry;
        private bool includeCast;
        private bool includeDirector;

        public bool IncludeCategory { set { includeCategory = value; } }
        public bool IncludeTag { set { includeTag = value; } }
        public bool IncludeLanguage { set { includeLanguage = value; } }
        public bool IncludeCountry { set { includeCountry = value; } }
        public bool IncludeCast { set { includeCast = value; } }
        public bool IncludeDirector { set { includeDirector = value; } }

        public FilmBLL()
            : base()
        {
            InitDAL();
            disposed = false;
        }

        public FilmBLL(BusinessLogicLayer bll)
            : base()
        {
            InitDAL(bll.db);
            disposed = false;
        }

        public override void SetDefault()
        {
            base.SetDefault();
            includeCategory = false;
            includeTag = false;
            includeCountry = false;
            includeLanguage = false;
            includeCast = false;
            includeDirector = false;
        }

        private FilmDto ToFilmDto(Film film)
        {
            if (film == null)
                return null;

            FilmDto FilmDto = new FilmDto()
            {
                ID = film.ID,
                Name = film.Name,
                Description = film.Description,
                ProductionCompany = film.ProductionCompany,
                Thumbnail = film.Thumbnail,
                ReleaseDate = film.ReleaseDate,
                Upvote = film.Upvote,
                Downvote = film.Downvote,
                Views = film.Views,
                Duration = film.Duration,
                Source = film.Source
            };

            //if (includeCategory)
            //    FilmDto.Categories = new CategoryBLL(this).GetCategoriesByFilmId(film.ID);

            //if (includeTag)
            //    FilmDto.Tags = new TagBLL(this).GetTagsByFilmId(film.ID);

            //if (includeDirector)
            //    FilmDto.Directors = new DirectorBLL(this).GetDirectorsByFilmId(film.ID);

            //if (includeCast)
            //    FilmDto.Casts = new CastBLL(this).GetCastsByFilmId(film.ID);

            //if (includeLanguage)
            //    FilmDto.Language = ((film.languageId != 0)
            //        ? new LanguageBLL(this).GetLanguage(film.languageId) : null);

            //if (includeCountry)
            //    FilmDto.Country = ((film.countryId != 0)
            //        ? new CountryBLL(this).GetCountry(film.countryId) : null);

            if (includeTimestamp)
            {
                FilmDto.CreatedAt = film.CreatedAt;
                FilmDto.UpdatedAt = film.UpdatedAt;
            }

            return FilmDto;
        }

        private Film ToFilm(FilmCreation filmCreation)
        {
            if (filmCreation == null)
                throw new Exception("@'filmCreation' must not be null");

            HashFunction hash = new HashFunction();
            string filmId = hash.MD5_Hash(string
                .Format("Name:{0}//random:{1}", filmCreation.name, new Random().NextString(25)));

            return new Film
            {
                ID = filmId,
                Name = filmCreation.name,
                Description = filmCreation.description,
                ProductionCompany = filmCreation.productionCompany,
                Thumbnail = filmCreation.thumbnail,
                ReleaseDate = filmCreation.releaseDate,
                Duration = filmCreation.duration,
                Source = filmCreation.source,
                Views = 0,
                Downvote = 0,
                Upvote = 0,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };
        }

        private Film ToFilm(FilmUpdate filmUpdate)
        {
            if (filmUpdate == null)
                throw new Exception("@'filmUpdate' must not be null");

            return new Film
            {
                ID = filmUpdate.ID,
                Name = filmUpdate.name,
                Description = filmUpdate.description,
                ProductionCompany = filmUpdate.productionCompany,
                Thumbnail = filmUpdate.thumbnail,
                ReleaseDate = filmUpdate.releaseDate,
                Duration = filmUpdate.duration,
                Source = filmUpdate.source,
                UpdatedAt = DateTime.Now
            };
        }

        public async Task<object> CountFilmByCategoryAsync()
        {
            string commandText = @"Select [Category].[ID], [Category].[Name], 
                                    (select count([filmId]) from[CategoryDistribution] 
                                    where[CategoryDistribution].[categoryId] = [Category].[ID]) as 'count'
                                from Category";

            return await db.Execute_ToOriginalDataAsync(commandText, CommandType.Text);
        }

        public async Task<List<FilmDto>> SeachFilmsAsync(string Name)
        {
            if (string.IsNullOrEmpty(Name))
                throw new Exception("@'Name' must not be null");

            string commandText;
            if (includeTimestamp)
                commandText = @"Select [Film].* from [Film]
                            where [Film].[Name] like @Name";
            else
                commandText = @"Select [Film].[ID],[Film].[Name],[Film].[description],
                                [Film].[languageId],[Film].[countryId],[Film].[ProductionCompany],
                                [Film].[releaseDate],[Film].[duration],[Film].[Thumbnail],
                                [Film].[upvote],[Film].[downvote],[Film].[Views], [Film].[Source]
                            from [Film]
                            where [Film].[Name] like @Name";

            return await db.Execute_ToListAsync<FilmDto>(
                commandText, CommandType.Text, new SqlParameter("@Name", string.Format("%{0}%", Name)));
        }

        public async Task<List<FilmDto>> GetLatestFilmAsync(int count = 12)
        {
            string commandText = string.Format(@"Select top {0} [Film].[ID], [Film].[Name], 
                                [Film].[Thumbnail], [Film].[countryId], [Film].[upvote], [Film].[downvote]
                            from [Film] order by [createAt] desc", count);

            return (await db.Execute_ToListAsync<Film>(commandText, CommandType.Text))
                .Select(f => ToFilmDto(f)).ToList();
        }

        public async Task<List<FilmDto>> GetFilmsAsync()
        {
            List<FilmDto> films = null;
            if (includeTimestamp)
                films = (await db.Films.ToListAsync()).Select(f => ToFilmDto(f)).ToList();
            else
                films = (await db.Films.ToListAsync(
                            f => new
                            {
                                f.ID,
                                f.Name,
                                f.Description,
                                f.ProductionCompany,
                                f.ReleaseDate,
                                f.Thumbnail,
                                f.Upvote,
                                f.Downvote,
                                f.Views,
                                f.Duration,
                                f.Source
                            })
                    ).Select(f => ToFilmDto(f)).ToList();

            return films;
        }

        public async Task<FilmDto> GetFilmAsync(string filmId)
        {
            if (string.IsNullOrEmpty(filmId))
                throw new Exception("@'filmId' must not be null or empty");

            Film film = null;
            if (includeTimestamp)
                film = await db.Films.SingleOrDefaultAsync(f => f.ID == filmId);
            else
                film = await db.Films.SingleOrDefaultAsync(f => new
                {
                    f.ID,
                    f.Name,
                    f.Description,
                    f.ProductionCompany,
                    f.ReleaseDate,
                    f.Thumbnail,
                    f.Upvote,
                    f.Downvote,
                    f.Views,
                    f.Duration,
                    f.Source
                }, f => f.ID == filmId);

            return ToFilmDto(film);
        }

        public async Task<PagedList<FilmDto>> GetFilmsAsync(int pageIndex, int pageSize)
        {
            SqlPagedList<Film> pagedList = null;
            Expression<Func<Film, object>> orderBy = f => new { f.ID };
            if (includeTimestamp)
                pagedList = await db.Films.ToPagedListAsync(orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);
            else
                pagedList = await db.Films.ToPagedListAsync(
                    f => new
                    {
                        f.ID,
                        f.Name,
                        f.Description,
                        f.ProductionCompany,
                        f.ReleaseDate,
                        f.Thumbnail,
                        f.Upvote,
                        f.Downvote,
                        f.Views,
                        f.Duration,
                        f.Source
                    }, orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize
                );

            return new PagedList<FilmDto>
            {
                PageNumber = pagedList.PageNumber,
                CurrentPage = pagedList.CurrentPage,
                Items = pagedList.Items.Select(f => ToFilmDto(f)).ToList()
            };
        }

        public PagedList<FilmDto> GetFilms(int pageIndex, int pageSize)
        {
            SqlPagedList<Film> pagedList = null;
            Expression<Func<Film, object>> orderBy = f => new { f.ID };
            if (includeTimestamp)
                pagedList = db.Films.ToPagedList(orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);
            else
                pagedList = db.Films.ToPagedList(
                    f => new
                    {
                        f.ID,
                        f.Name,
                        f.Description,
                        f.ProductionCompany,
                        f.ReleaseDate,
                        f.Thumbnail,
                        f.Upvote,
                        f.Downvote,
                        f.Views,
                        f.Duration,
                        f.Source
                    }, orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize
                );

            return new PagedList<FilmDto>
            {
                PageNumber = pagedList.PageNumber,
                CurrentPage = pagedList.CurrentPage,
                Items = pagedList.Items.Select(f => ToFilmDto(f)).ToList()
            };
        }

        public FilmDto GetFilm(string filmId)
        {
            if (string.IsNullOrEmpty(filmId))
                throw new Exception("@'filmId' must not be null or empty");

            Film film = null;
            if (includeTimestamp)
                film = db.Films.SingleOrDefault(f => f.ID == filmId);
            else
                film = db.Films.SingleOrDefault(f => new
                {
                    f.ID,
                    f.Name,
                    f.Description,
                    f.ProductionCompany,
                    f.ReleaseDate,
                    f.Thumbnail,
                    f.Upvote,
                    f.Downvote,
                    f.Views,
                    f.Duration,
                    f.Source
                }, f => f.ID == filmId);

            return ToFilmDto(film);
        }

        public async Task<List<FilmDto>> GetFilmsByCategoryIdAsync(int categoryId, int count = 12)
        {
            if (categoryId <= 0)
                throw new Exception("@'categoryId' must be greater than 0");

            string commandText;
            if (includeTimestamp)
                commandText = string.Format(@"Select top {0} [Film].* from [Film], [CategoryDistribution]
                            where [Film].[ID] = [CategoryDistribution].[filmId]
                                and [CategoryDistribution].[categoryId] = @categoryId", count);
            else
                commandText = string.Format(@"Select top {0} [Film].[ID],[Film].[Name],[Film].[description],
                                [Film].[languageId],[Film].[countryId],[Film].[ProductionCompany],
                                [Film].[releaseDate],[Film].[duration],[Film].[Thumbnail],
                                [Film].[upvote],[Film].[downvote],[Film].[Views], [Film].[Source]
                            from [Film], [CategoryDistribution]
                            where [Film].[ID] = [CategoryDistribution].[filmId]
                                and [CategoryDistribution].[categoryId] = @categoryId", count);

            return await db.Execute_ToListAsync<FilmDto>(commandText, CommandType.Text, new SqlParameter("@categoryId", categoryId));
        }

        public async Task<CreationState> CreateFilmAsync(FilmCreation filmCreation)
        {
            Film film = ToFilm(filmCreation);
            if (string.IsNullOrEmpty(film.Name) 
                || string.IsNullOrEmpty(film.ProductionCompany) 
            )
            {
                throw new Exception("");
            }

            long checkExists = await db.Films.CountAsync(
                f => (f.ID == film.ID) || (f.Name == film.Name)
            );
            if (checkExists != 0)
                return CreationState.AlreadyExists;

            int affected;
            if (film.Description == null)
                affected = await db.Films.InsertAsync(film, new List<string> { "description", "duration", "Thumbnail", "Source" });
            else
                affected = await db.Films.InsertAsync(film, new List<string> { "duration", "Thumbnail", "Source" });

            return (affected == 0) ? CreationState.Failed : CreationState.Success;
        }

        public async Task<UpdateState> UpdateFilmAsync(FilmUpdate filmUpdate)
        {
            Film film = ToFilm(filmUpdate);
            if (string.IsNullOrEmpty(film.Name)
                || string.IsNullOrEmpty(film.ProductionCompany)
            )
            {
                throw new Exception("");
            }

            int affected;
            if (film.Description == null)
                affected = await db.Films.UpdateAsync(film, f => new
                {
                    f.Name,
                    f.ReleaseDate,
                    f.ProductionCompany,
                    f.UpdatedAt
                }, f => f.ID == film.ID);
            else
                affected = await db.Films.UpdateAsync(film, f => new
                {
                    f.Name,
                    f.Description,
                    f.ReleaseDate,
                    f.ProductionCompany,
                    f.UpdatedAt
                }, f => f.ID == film.ID);

            return (affected == 0) ? UpdateState.Failed : UpdateState.Success;
        }

        public async Task<DeletionState> DeleteFilmAsync(string filmId)
        {
            if (string.IsNullOrEmpty(filmId))
                throw new Exception("@'filmId' must not be null or empty");

            DeletionState state1 = await DeleteAllCategoryAsync(filmId);
            DeletionState state2 = await DeleteAllTagAsync(filmId);
            DeletionState state3 = await DeleteAllDirectorAsync(filmId);
            DeletionState state4 = await DeleteAllCastAsync(filmId);
            DeletionState state5 = await new UserReactionBLL(this).DeleteAllUserReactionAsync(filmId);

            if (state1 == DeletionState.Success && state2 == DeletionState.Success
                && state3 == DeletionState.Success && state4 == DeletionState.Success)
            {
                int affected = await db.Films.DeleteAsync(f => f.ID == filmId);
                return (affected == 0) ? DeletionState.Failed : DeletionState.Success;
            }

            return DeletionState.ConstraintExists;
        }

        public async Task<CreationState> AddCategoryAsync(string filmId, int categoryId)
        {
            if (string.IsNullOrEmpty(filmId) || categoryId <= 0)
                throw new Exception("");

            long checkExists = 0;// await db.CategoryDistributions
                //.CountAsync(cd => cd.filmId == filmId && cd.categoryId == categoryId);
            if (checkExists != 0)
                return CreationState.AlreadyExists;

            //CategoryDistribution categoryDistribution = new CategoryDistribution
            //{
            //    filmId = filmId,
            //    categoryId = categoryId,
            //    createAt = DateTime.Now,
            //    updateAt = DateTime.Now
            //};

            int affected = 0;// await db.CategoryDistributions.InsertAsync(categoryDistribution);
            return (affected == 0) ? CreationState.Failed : CreationState.Success;
        }

        public async Task<DeletionState> DeleteCategoryAsync(string filmId, int categoryId)
        {
            if (string.IsNullOrEmpty(filmId) || categoryId <= 0)
                throw new Exception("");

            int affected = 0;// await db.CategoryDistributions
                //.DeleteAsync(cd => cd.filmId == filmId && cd.categoryId == categoryId);

            return (affected == 0) ? DeletionState.Failed : DeletionState.Success;
        }

        public async Task<DeletionState> DeleteAllCategoryAsync(string filmId)
        {
            if (string.IsNullOrEmpty(filmId))
                throw new Exception("@'filmId' must not be null or empty");

            int affected = 0; // await db.CategoryDistributions
                //.DeleteAsync(cd => cd.filmId == filmId);

            return (affected == 0) ? DeletionState.Failed : DeletionState.Success;
        }

        public async Task<CreationState> AddTagAsync(string filmId, int tagId)
        {
            if (string.IsNullOrEmpty(filmId) || tagId <= 0)
                throw new Exception("");

            long checkExists = 0;// await db.TagDistributions
                //.CountAsync(td => td.filmId == filmId && td.tagId == tagId);
            if (checkExists != 0)
                return CreationState.AlreadyExists;

            //TagDistribution tagDistribution = new TagDistribution
            //{
            //    filmId = filmId,
            //    tagId = tagId,
            //    createAt = DateTime.Now,
            //    updateAt = DateTime.Now
            //};

            int affected = 0; // await db.TagDistributions.InsertAsync(null);
            return (affected == 0) ? CreationState.Failed : CreationState.Success;
        }

        public async Task<DeletionState> DeleteTagAsync(string filmId, int tagId)
        {
            if (string.IsNullOrEmpty(filmId) || tagId <= 0)
                throw new Exception("");

            int affected = 0;// await db.TagDistributions
                //.DeleteAsync(td => td.filmId == filmId && td.tagId == tagId);

            return (affected == 0) ? DeletionState.Failed : DeletionState.Success;
        }

        public async Task<DeletionState> DeleteAllTagAsync(string filmId)
        {
            if (string.IsNullOrEmpty(filmId))
                throw new Exception("@'filmId' must not be null or empty");

            int affected = 0;// await db.TagDistributions
                //.DeleteAsync(td => td.filmId == filmId);

            return (affected == 0) ? DeletionState.Failed : DeletionState.Success;
        }

        public async Task<CreationState> AddDirectorAsync(string filmId, long directorId, string directorRole)
        {
            if (string.IsNullOrEmpty(filmId) || directorId <= 0 || string.IsNullOrEmpty(directorRole))
                throw new Exception("");

            long checkExists = 0;// await db.DirectorOfFilms
                //.CountAsync(df => df.filmId == filmId && df.directorId == directorId);
            if (checkExists != 0)
                return CreationState.AlreadyExists;

            //DirectorOfFilm directorOfFilm = new DirectorOfFilm
            //{
            //    filmId = filmId,
            //    directorId = directorId,
            //    role = directorRole,
            //    createAt = DateTime.Now,
            //    updateAt = DateTime.Now
            //};

            int affected = 0;// await db.DirectorOfFilms.InsertAsync(directorOfFilm);
            return (affected == 0) ? CreationState.Failed : CreationState.Success;
        }

        public async Task<DeletionState> DeleteDirectorAsync(string filmId, long directorId)
        {
            if (string.IsNullOrEmpty(filmId) || directorId <= 0)
                throw new Exception("");

            int affected = 0;/// await db.DirectorOfFilms
                //.DeleteAsync(df => df.filmId == filmId && df.directorId == directorId);

            return (affected == 0) ? DeletionState.Failed : DeletionState.Success;
        }

        public async Task<DeletionState> DeleteAllDirectorAsync(string filmId)
        {
            if (string.IsNullOrEmpty(filmId))
                throw new Exception("@'filmId' must not be null or empty");

            int affected = 0;// await db.DirectorOfFilms
                //.DeleteAsync(df => df.filmId == filmId);

            return (affected == 0) ? DeletionState.Failed : DeletionState.Success;
        }

        public async Task<CreationState> AddCastAsync(string filmId, long castId, string castRole)
        {
            if (string.IsNullOrEmpty(filmId) || castId <= 0 || string.IsNullOrEmpty(castRole))
                throw new Exception("");

            long checkExists = 0;// await db.CastOfFilms
                //.CountAsync(cf => cf.filmId == filmId && cf.castId == castId);
            if (checkExists != 0)
                return CreationState.AlreadyExists;

            //CastOfFilm castOfFilm = new CastOfFilm
            //{
            //    filmId = filmId,
            //    castId = castId,
            //    role = castRole,
            //    createAt = DateTime.Now,
            //    updateAt = DateTime.Now
            //};

            int affected = 0;// await db.CastOfFilms.InsertAsync(castOfFilm);
            return (affected == 0) ? CreationState.Failed : CreationState.Success;
        }

        public async Task<DeletionState> DeleteCastAsync(string filmId, long castId)
        {
            if (string.IsNullOrEmpty(filmId) || castId <= 0)
                throw new Exception("");

            int affected = 0;// await db.CastOfFilms
                //.DeleteAsync(cf => cf.filmId == filmId && cf.castId == castId);

            return (affected == 0) ? DeletionState.Failed : DeletionState.Success;
        }

        public async Task<DeletionState> DeleteAllCastAsync(string filmId)
        {
            if (string.IsNullOrEmpty(filmId))
                throw new Exception("@'filmId' must not be null or empty");

            int affected = 0;// await db.CastOfFilms
                //.DeleteAsync(cf => cf.filmId == filmId);

            return (affected == 0) ? DeletionState.Failed : DeletionState.Success;
        }

        public async Task<UpdateState> AddImageAsync(string filmId, string filePath)
        {
            if (string.IsNullOrEmpty(filmId) || string.IsNullOrEmpty(filePath))
                throw new Exception("");

            Film film = await db.Films.SingleOrDefaultAsync(f => new { f.ID, f.Thumbnail }, f => f.ID == filmId);
            if (film == null)
                return UpdateState.Failed;

            if (!string.IsNullOrEmpty(film.Thumbnail))
                return UpdateState.Failed;

            int affected = await db.Films
                .UpdateAsync(new Film { Thumbnail = filePath }, f => new { f.Thumbnail }, f => f.ID == film.ID);

            return (affected == 0) ? UpdateState.Failed : UpdateState.Success;
        }

        public async Task<UpdateState> DeleteImageAsync(string filmId)
        {
            if (string.IsNullOrEmpty(filmId))
                throw new Exception("@'filmId' must not be null or empty");

            int affected = await db.Films
                .UpdateAsync(new Film { Thumbnail = null }, f => new { f.Thumbnail }, f => f.ID == filmId);

            return (affected == 0) ? UpdateState.Failed : UpdateState.Success;
        }

        public async Task<UpdateState> AddSourceAsync(string filmId, string filePath)
        {
            if (string.IsNullOrEmpty(filmId) || string.IsNullOrEmpty(filePath))
                throw new Exception("");

            Film film = await db.Films.SingleOrDefaultAsync(f => new { f.ID, f.Source }, f => f.ID == filmId);
            if (film == null)
                return UpdateState.Failed;

            if (!string.IsNullOrEmpty(film.Source))
                return UpdateState.Failed;

            int affected = await db.Films
                .UpdateAsync(new Film { Source = filePath }, f => new { f.Source }, f => f.ID == film.ID);

            return (affected == 0) ? UpdateState.Failed : UpdateState.Success;
        }

        public async Task<UpdateState> DeleteSourceAsync(string filmId)
        {
            if (string.IsNullOrEmpty(filmId))
                throw new Exception("@'filmId' must not be null or empty");

            int affected = await db.Films
                .UpdateAsync(new Film { Source = null }, f => new { f.Source }, f => f.ID == filmId);

            return (affected == 0) ? UpdateState.Failed : UpdateState.Success;
        }

        public UpdateState Upvote(string filmId)
        {
            if (string.IsNullOrEmpty(filmId))
                throw new Exception("@'filmId' must not be null or empty");

            string commandText = @"Update [Film] set [upvote] = (
                                            Select count([userId]) from [UserReaction] 
                                            where [filmId] = @filmId and [upvoted] = 1),
                                        [downvote] = (
                                            Select count([userId]) from [UserReaction] 
                                            where [filmId] = @filmId and [downvoted] = 1)
                                        where [Film].[ID] = @filmId";

            int affected = db.ExecuteNonQuery(commandText, CommandType.Text, new SqlParameter("@filmId", filmId));
            return (affected == 0) ? UpdateState.Failed : UpdateState.Success;
        }

        public UpdateState Downvote(string filmId)
        {
            if (string.IsNullOrEmpty(filmId))
                throw new Exception("@'filmId' must not be null or empty");

            string commandText = @"Update [Film] set [downvote] = (
                                            Select count([userId]) from [UserReaction] 
                                            where [filmId] = @filmId and [downvoted] = 1),
                                        [upvote] = (
                                            Select count([userId]) from [UserReaction] 
                                            where [filmId] = @filmId and [upvoted] = 1)
                                        where [Film].[ID] = @filmId";

            int affected = db.ExecuteNonQuery(commandText, CommandType.Text, new SqlParameter("@filmId", filmId));
            return (affected == 0) ? UpdateState.Failed : UpdateState.Success;
        }

        public UpdateState IncreaseView(string filmId)
        {
            if (string.IsNullOrEmpty(filmId))
                throw new Exception("@'filmId' must not be null or empty");

            Film film = db.Films.SingleOrDefault(f => new { f.ID, f.Views }, f => f.ID == filmId);
            if (film == null)
                return UpdateState.Failed;

            int affected = db.Films.Update(new Film { Views = film.Views + 1 }, f => new { f.Views }, f => f.ID == filmId);
            return (affected == 0) ? UpdateState.Failed : UpdateState.Success;
        }

        public async Task<long> CountAllAsync()
        {
            return await db.Films.CountAsync();
        }

        protected override void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                try
                {
                    if (disposing)
                    {

                    }
                    this.disposed = true;
                }
                finally
                {
                    base.Dispose(disposing);
                }
            }
        }
    }
}
