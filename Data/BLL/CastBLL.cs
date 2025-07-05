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
    public class CastBLL : BusinessLogicLayer
    {
        private bool disposed;
        private bool includeDescription;

        public bool IncludeDescription { set { includeDescription = value; } }

        public CastBLL()
            : base()
        {
            InitDAL();
            SetDefault();
            disposed = false;
        }

        public CastBLL(BusinessLogicLayer bll)
            : base()
        {
            InitDAL(bll.db);
            SetDefault();
            disposed = false;
        }

        public override void SetDefault()
        {
            base.SetDefault();
            includeDescription = false;
        }

        private ActorDto ToActorDto(Cast cast)
        {
            if (cast == null)
                return null;

            ActorDto actorDto = new ActorDto();
            actorDto.ID = cast.ID;
            actorDto.Name = cast.Name;

            if (includeDescription)
                actorDto.Description = cast.Description;

            if (includeTimestamp)
            {
                actorDto.CreatedAt = cast.CreatedAt;
                actorDto.UpdatedAt = cast.UpdatedAt;
            }

            return actorDto;
        }

        private Cast ToCast(CreateActorDto createActorDto)
        {
            if (createActorDto == null)
                throw new Exception("@'castCreation' must not be null");

            return new Cast
            {
                Name = createActorDto.Name,
                Description = createActorDto.Description,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };
        }

        private Cast ToCast(UpdateActorDto updateActorDto)
        {
            if (updateActorDto == null)
                throw new Exception("@'castUpdate' must not be null");

            return new Cast
            {
                ID = updateActorDto.ID,
                Name = updateActorDto.Name,
                Description = updateActorDto.Description,
                UpdatedAt = DateTime.Now,
            };
        }

        public async Task<List<ActorDto>> GetCastsAsync()
        {
            List<ActorDto> casts = null;
            if (includeDescription && includeTimestamp)
                casts = (await db.Casts.ToListAsync()).Select(c => ToActorDto(c)).ToList();
            else if (includeDescription)
                casts = (await db.Casts.ToListAsync(c => new { c.ID, c.Name, c.Description }))
                     .Select(c => ToActorDto(c)).ToList();
            else if (includeTimestamp)
                casts = (await db.Casts.ToListAsync(c => new { c.ID, c.Name, c.CreatedAt, c.UpdatedAt }))
                     .Select(c => ToActorDto(c)).ToList();
            else
                casts = (await db.Casts.ToListAsync(c => new { c.ID, c.Name }))
                     .Select(c => ToActorDto(c)).ToList();

            return casts;
        }

        public List<ActorDto> GetCasts()
        {
            List<ActorDto> casts = null;
            if (includeDescription && includeTimestamp)
                casts = db.Casts.ToList().Select(c => ToActorDto(c)).ToList();
            else if (includeDescription)
                casts = db.Casts.ToList(c => new { c.ID, c.Name, c.Description })
                     .Select(c => ToActorDto(c)).ToList();
            else if (includeTimestamp)
                casts = db.Casts.ToList(c => new { c.ID, c.Name, c.CreatedAt, c.UpdatedAt })
                     .Select(c => ToActorDto(c)).ToList();
            else
                casts = db.Casts.ToList(c => new { c.ID, c.Name })
                     .Select(c => ToActorDto(c)).ToList();

            return casts;
        }

        public async Task<PagedList<ActorDto>> GetCastsAsync(int pageIndex, int pageSize)
        {
            SqlPagedList<Cast> pagedList = null;
            Expression<Func<Cast, object>> orderBy = c => new { c.ID };
            if (includeDescription && includeTimestamp)
                pagedList = await db.Casts.ToPagedListAsync(orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);
            else if (includeDescription)
                pagedList = await db.Casts.ToPagedListAsync(
                    c => new { c.ID, c.Name, c.Description }, orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);
            else if (includeTimestamp)
                pagedList = await db.Casts.ToPagedListAsync(
                    c => new { c.ID, c.Name, c.CreatedAt, c.UpdatedAt }, orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);
            else
                pagedList = await db.Casts.ToPagedListAsync(
                    c => new { c.ID, c.Name }, orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);

            return new PagedList<ActorDto>
            {
                PageNumber = pagedList.PageNumber,
                CurrentPage = pagedList.CurrentPage,
                Items = pagedList.Items.Select(c => ToActorDto(c)).ToList()
            };
        }

        public PagedList<ActorDto> GetCasts(int pageIndex, int pageSize)
        {
            SqlPagedList<Cast> pagedList = null;
            Expression<Func<Cast, object>> orderBy = c => new { c.ID };
            if (includeDescription && includeTimestamp)
                pagedList = db.Casts.ToPagedList(orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);
            else if(includeDescription)
                pagedList = db.Casts.ToPagedList(
                    c => new { c.ID, c.Name, c.Description },orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);
            else if(includeTimestamp)
                pagedList = db.Casts.ToPagedList(
                    c => new { c.ID, c.Name, c.CreatedAt, c.UpdatedAt }, orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);
            else
                pagedList = db.Casts.ToPagedList(
                    c => new { c.ID, c.Name }, orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);

            return new PagedList<ActorDto>
            {
                PageNumber = pagedList.PageNumber,
                CurrentPage = pagedList.CurrentPage,
                Items = pagedList.Items.Select(c => ToActorDto(c)).ToList()
            };
        }

        public async Task<ActorDto> GetCastAsync(long castId)
        {
            if (castId <= 0)
                throw new Exception("@'castId' must be greater than 0");

            Cast cast = null;
            if (includeDescription && includeTimestamp)
                cast = await db.Casts.SingleOrDefaultAsync(c => c.ID == castId);
            else if(includeDescription)
                cast = await db.Casts
                    .SingleOrDefaultAsync(c => new { c.ID, c.Name, c.Description }, c => c.ID == castId);
            else if(includeTimestamp)
                cast = await db.Casts
                    .SingleOrDefaultAsync(c => new { c.ID, c.Name, c.CreatedAt, c.UpdatedAt }, c => c.ID == castId);
            else
                cast = await db.Casts
                    .SingleOrDefaultAsync(c => new { c.ID, c.Name }, c => c.ID == castId);

            return ToActorDto(cast);
        }

        public ActorDto GetCast(long castId)
        {
            if (castId <= 0)
                throw new Exception("@'castId' must be greater than 0");

            Cast cast = null;
            if (includeDescription && includeTimestamp)
                cast = db.Casts.SingleOrDefault(c => c.ID == castId);
            else if(includeDescription)
                cast = db.Casts
                    .SingleOrDefault(c => new { c.ID, c.Name, c.Description }, c => c.ID == castId);
            else if(includeTimestamp)
                cast = db.Casts
                    .SingleOrDefault(c => new { c.ID, c.Name, c.CreatedAt, c.UpdatedAt }, c => c.ID == castId);
            else
                cast = db.Casts
                    .SingleOrDefault(c => new { c.ID, c.Name }, c => c.ID == castId);

            return ToActorDto(cast);
        }

        public async Task<List<ActorDto>> GetCastsByFilmIdAsync(string filmId)
        {
            if (string.IsNullOrEmpty(filmId))
                throw new Exception("@'filmId' must not be null or empty");

            string commandText;
            if (includeDescription && includeTimestamp)
                commandText = @"Select [Cast].* from [CastOfFilm], [Cast]
                                where [CastOfFilm].[castId] = [Cast].[ID]
                                    and [CastOfFilm].[filmId] = @filmId";
            else if(includeDescription)
                commandText = @"Select [Cast].[ID], [Cast].[Name], [Cast].[Description] 
                                from [CastOfFilm], [Cast]
                                where [CastOfFilm].[castId] = [Cast].[ID]
                                    and [CastOfFilm].[filmId] = @filmId";
            else if (includeTimestamp)
                commandText = @"Select [Cast].[ID], [Cast].[Name], [Cast].[CreatedAt], [Cast].[UpdatedAt] 
                                from [CastOfFilm], [Cast]
                                where [CastOfFilm].[castId] = [Cast].[ID]
                                    and [CastOfFilm].[filmId] = @filmId";
            else
                commandText = @"Select [Cast].[ID], [Cast].[Name] 
                                from [CastOfFilm], [Cast]
                                where [CastOfFilm].[castId] = [Cast].[ID]
                                    and [CastOfFilm].[filmId] = @filmId";

            return await db.Execute_ToListAsync<ActorDto>(commandText, CommandType.Text, new SqlParameter("@filmId", filmId));
        }

        public List<ActorDto> GetCastsByFilmId(string filmId)
        {
            if (string.IsNullOrEmpty(filmId))
                throw new Exception("@'filmId' must not be null or empty");

            string commandText;
            if (includeDescription && includeTimestamp)
                commandText = @"Select [Cast].* from [CastOfFilm], [Cast]
                                where [CastOfFilm].[castId] = [Cast].[ID]
                                    and [CastOfFilm].[filmId] = @filmId";
            else if (includeDescription)
                commandText = @"Select [Cast].[ID], [Cast].[Name], [Cast].[Description] 
                                from [CastOfFilm], [Cast]
                                where [CastOfFilm].[castId] = [Cast].[ID]
                                    and [CastOfFilm].[filmId] = @filmId";
            else if (includeTimestamp)
                commandText = @"Select [Cast].[ID], [Cast].[Name], [Cast].[CreatedAt], [Cast].[UpdatedAt] 
                                from [CastOfFilm], [Cast]
                                where [CastOfFilm].[castId] = [Cast].[ID]
                                    and [CastOfFilm].[filmId] = @filmId";
            else
                commandText = @"Select [Cast].[ID], [Cast].[Name] 
                                from [CastOfFilm], [Cast]
                                where [CastOfFilm].[castId] = [Cast].[ID]
                                    and [CastOfFilm].[filmId] = @filmId";

            return db.Execute_ToList<ActorDto>(commandText, CommandType.Text, new SqlParameter("@filmId", filmId));
        }

        public async Task<CreationState> CreateCastAsync(CreateActorDto createActorDto)
        {
            Cast cast = ToCast(createActorDto);
            if (cast.Name == null)
                throw new Exception("@'cast.Name' must not be null");

            int checkExists = (int)await db.Casts.CountAsync(c => c.Name == cast.Name);
            if (checkExists != 0)
                return CreationState.AlreadyExists;

            int affected;
            if (cast.Description == null)
                affected = await db.Casts.InsertAsync(cast, new List<string> { "ID", "Description" });
            else
                affected = await db.Casts.InsertAsync(cast, new List<string> { "ID" });

            return (affected == 0) ? CreationState.Failed : CreationState.Success;
        }

        public async Task<UpdateState> UpdateCastAsync(UpdateActorDto updateActorDto)
        {
            Cast cast = ToCast(updateActorDto);
            if (cast.Name == null)
                throw new Exception("@'cast.Name' must not be null");

            int affected;
            if (cast.Description == null)
                affected = await db.Casts.UpdateAsync(
                    cast,
                    c => new { c.Name, c.UpdatedAt },
                    c => c.ID == cast.ID
                );
            else
                affected = await db.Casts.UpdateAsync(
                    cast,
                    c => new { c.Name, c.Description, c.UpdatedAt },
                    c => c.ID == cast.ID
                );

            return (affected == 0) ? UpdateState.Failed : UpdateState.Success;
        }

        public async Task<DeletionState> DeleteCastAsync(int castId)
        {
            if (castId <= 0)
                throw new Exception("@'castId' must be greater than 0");

            long castOfFilmNumber = await db.CastOfFilms
                .CountAsync(cf => cf.castId == castId);
            if (castOfFilmNumber > 0)
                return DeletionState.ConstraintExists;

            int affected = await db.Casts.DeleteAsync(c => c.ID == castId);
            return (affected == 0) ? DeletionState.Failed : DeletionState.Success;
        }

        public async Task<int> CountAllAsync()
        {
            return (int)await db.Casts.CountAsync();
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