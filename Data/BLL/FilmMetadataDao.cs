using Common.Web;
using Data.DAL;
using MSSQL.Mapper;
using System;
using System.Collections.Generic;
using System.Text;
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
            return await _context.FilmMetadata.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<FilmMetadata>> GetsByIdsAsync(List<int> ids)
        {
            StringBuilder commandTextBuilder = new StringBuilder("SELECT * FROM FilmMetadata WHERE Id IN (");
            //SqlParameter[] parameters = new SqlParameter[ids.Count];
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            for (int i = 0; i < ids.Count; i++)
            {
                commandTextBuilder.Append($"@Id{i}, ");
                //parameters[i] = new SqlParameter($"@Id{i}", ids[i]);
                parameters.Add($"@Id{i}", ids[i]);
            }
            commandTextBuilder.Append(")").Replace(", )", " )");

            return await _context.GetHelper()
                .ExecuteReaderAsync<FilmMetadata>(commandTextBuilder.ToString(), parameters, r => SqlMapper.MapRow<FilmMetadata>(r));
        }

        public async Task<PagedList<FilmMetadata>> GetsAsync(string type = "language", long pageIndex = 1, long pageSize = 10)
        {
            long skip = (pageIndex - 1) * pageSize;
            List<FilmMetadata> filmMetadata = await _context.FilmMetadata
                .Where(x => x.Type == type).OrderBy(o => new { o.Id }).ToListAsync();

            //long count = await _context.FilmMetadata.CountAsync(x => x.Type == type);

            return new PagedList<FilmMetadata>
            {
                Items = filmMetadata,
                CurrentPage = pageIndex,
            };
        }

        public async Task<int> AddAsync(FilmMetadata metadata)
        {
            metadata.CreatedAt = DateTime.Now;

            return await _context.FilmMetadata.InsertAsync(metadata, new List<string> { "Id", "UpdatedAt", "DeletedAt" });
        }

        public async Task<int> UpdateAsync(FilmMetadata metadata)
        {
            metadata.UpdatedAt = DateTime.Now;
            return await _context.FilmMetadata
                .Where(x => x.Id == metadata.Id)
                .UpdateAsync(metadata, s => new { s.Name, s.Description, s.UpdatedAt });
        }

        public async Task<int> DeleteAsync(int id, bool forceDelete = false)
        {
            FilmMetadata metadata = await GetAsync(id);
            if (metadata == null)
                return 0;

            if (forceDelete)
                return await _context.FilmMetadata.DeleteAsync(x => x.Id == id);

            metadata.DeletedAt = DateTime.Now;
            return await _context.FilmMetadata
                .Where(x => x.Id == id)
                .UpdateAsync(metadata, s => new { s.DeletedAt });
        }
    }
}
