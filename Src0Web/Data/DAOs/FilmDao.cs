using Data.DAL;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using MSSQL.Mapper;
using Web.Shared.Result;
using Data.Base;

namespace Data.DAOs
{
    public class FilmDao : GenericDao<Film>
    {
        public FilmDao(DBContext context)
            : base(context, x => x.Films)
        {

        }

        public async Task<List<Film>> GetsAsync(long skip = 0, long take = 0)
        {
            List<Film> films = await Context.Films.Where(x => x.DeletedAt == null)
                .OrderBy(o => new { o.Id }).ToListAsync();

            return films;
        }

        public async Task<long> CountAsync()
        {
            return await Context.Films.CountAsync();
        }

        public async Task<PagedList<Film>> GetsByCategoryIdAsync(int categoryId, long pageIndex = 1, long pageSize = 10)
        {
            string countCommand = @"
                SELECT CAST(COUNT(*) AS BIGINT)
                FROM [Films]
                INNER JOIN [TaxonomyLinks] tl ON [Films].[ID] = tl.FilmId
                WHERE tl.TaxonomyId = @categoryId";

            long totalRecord = await Context.GetHelper()
                .ExecuteScalarQueryAsync<long>(countCommand, new Dictionary<string, object> { { "@categoryId", categoryId } });

            string command = @"
                SELECT [Films].*
                FROM [Films]
                INNER JOIN [TaxonomyLinks] tl ON [Films].[ID] = tl.FilmId
                WHERE tl.TaxonomyId = @categoryId
                ORDER BY [Films].[ID]
                OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY";

            long offest = ((pageIndex - 1) * pageSize);

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@categoryId", categoryId },
                { "@offset", offest },
                { "@pageSize", pageSize }
            };

            PagedList<Film> pagedList = new PagedList<Film>();

            pagedList.Items = await Context.GetHelper()
                .ExecuteReaderAsync<Film>(command, parameters, r => SqlMapper.MapRow<Film>(r));

            //pagedList.Solve(totalRecord, pageIndex - 1, pageSize);

            return pagedList;
        }

        public Task<List<Film>> GetManyAsync(int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }
    }
}