using Data.DAL;
using MSSQL.Access;
using MSSQL.Query;
using System.Linq.Expressions;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using Common.Web;

namespace Data.BLL
{
    public class FilmDao
    {
        private readonly DBContext _context;

        internal FilmDao(DBContext context)
        {
            _context = context;
        }

        public async Task<Film> GetAsync(string id)
        {
            return await _context.Films.SingleOrDefaultAsync(x => x.ID == id);
        }

        public async Task<PagedList<Film>> GetsAsync(long pageIndex = 1, long pageSize = 10)
        {
            long skip = (pageIndex - 1) * pageSize;
            List<Film> films = await _context.Films
                .OrderBy(o => new { o.ID }).ToListAsync();

            //long count = await _context.Films.CountAsync();

            return new PagedList<Film>
            {
                Items = films,
                CurrentPage = pageIndex,
            };
        }

        public async Task<SqlPagedList<Film>> GetsByCategoryIdAsync(int categoryId, long pageIndex = 1, long pageSize = 10)
        {
            string countCommand = @"
                SELECT COUNT(*)
                FROM [Film]
                INNER JOIN [TaxonomyLink] tl ON [Film].[ID] = tl.FilmId
                WHERE tl.TaxonomyId = @categoryId";

            long totalRecord = Convert.ToInt64(
                await _context.ExecuteScalarAsync(countCommand, CommandType.Text, new SqlParameter("@categoryId", categoryId))
            );

            string command = @"
                SELECT [Film].*
                FROM [Film]
                INNER JOIN [TaxonomyLink] tl ON [Film].[ID] = tl.FilmId
                WHERE tl.TaxonomyId = @categoryId
                ORDER BY [Film].[ID]
                OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY";

            long offest = ((pageIndex - 1) * pageSize);

            SqlPagedList<Film> pagedList = new SqlPagedList<Film>();

            pagedList.Items = await _context.Execute_ToListAsync<Film>(command, CommandType.Text, new SqlParameter("@categoryId", categoryId), new SqlParameter("@offset", offest), new SqlParameter("@pageSize", pageSize));
            pagedList.Solve(totalRecord, pageIndex - 1, pageSize);

            return pagedList;
        }

        public async Task<int> AddAsync(Film film)
        {
            film.CreatedAt = DateTime.Now;

            return await _context.Films.InsertAsync(film, new List<string> { "UpdatedAt", "DeletedAt" });
        }
    }
}