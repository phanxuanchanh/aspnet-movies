using Common.Web;
using Data.DAL;
using MSSQL.Access;
using MSSQL.Query;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Data.BLL
{
    public class FilmMetadataDao
    {
        private readonly DBContext _context;

        internal FilmMetadataDao(DBContext context) {
            _context = context;
        }

        public async Task<FilmMetadata> GetAsync(int id)
        {
            return await _context.FilmMetadata.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<SqlPagedList<FilmMetadata>> GetsAsync(string type = "language", long pageIndex = 1, long pageSize = 10)
        {
            SqlPagedList<FilmMetadata> pagedList = null;
            Expression<Func<FilmMetadata, object>> orderBy = c => new { c.Id };

            pagedList = await _context.FilmMetadata.ToPagedListAsync(x => x.Type == type, orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);

            return pagedList;
        }

        public async Task<int> AddAsync(FilmMetadata metadata)
        {
            metadata.CreatedAt = DateTime.Now;

            return await _context.FilmMetadata.InsertAsync(metadata, new List<string> { "Id", "UpdatedAt", "DeletedAt" });
        }

        public async Task<int> UpdateAsync(FilmMetadata metadata)
        {
            metadata.UpdatedAt = DateTime.Now;
            return await _context.FilmMetadata.UpdateAsync(metadata, s => new { s.Name, s.Description, s.UpdatedAt }, x => x.Id == metadata.Id);
        }

        public async Task<int> DeleteAsync(int id, bool forceDelete = false)
        {
            FilmMetadata metadata = await GetAsync(id);
            if (metadata == null)
                return 0;

            if (forceDelete)
                return await _context.FilmMetadata.DeleteAsync(x => x.Id == id);

            metadata.DeletedAt = DateTime.Now;
            return await _context.FilmMetadata.UpdateAsync(metadata, s => new { s.DeletedAt }, x => x.Id == id);
        }
    }
}
