using Data.DAL;
using MSSQL.Access;
using MSSQL.Query;
using System.Linq.Expressions;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

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

        public async Task<SqlPagedList<Film>> GetsAsync(long pageIndex = 1, long pageSize = 10)
        {
            SqlPagedList<Film> pagedList = null;
            Expression<Func<Film, object>> orderBy = c => new { c.ID };

            pagedList = await _context.Films.ToPagedListAsync(orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);

            return pagedList;
        }
    }
}