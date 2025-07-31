using Common.Web;
using Data.DAL;
using MSSQL.Access;
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

        public async Task<List<FilmMetadata>> GetsAsync(string type = "language", long skip = 1, long take = 10, string searchText = null)
        {
            SqlAccess<FilmMetadata> access = _context.FilmMetadata
                .Where(x => x.DeletedAt == null).OrderBy(o => new { o.Id });

            if(string.IsNullOrEmpty(searchText))
                access.Where(x => x.Type == type);
            else
                access.Where(x => x.Type == type && x.Name.Contains(searchText));

            return await access.ToListAsync();
        }

        public async Task<long> CountAsync(string type = "language", string searchText = null)
        {
            SqlAccess<FilmMetadata> access = _context.FilmMetadata
                .Where(x => x.DeletedAt == null);
            if (string.IsNullOrEmpty(searchText))
                access.Where(x => x.Type == type);
            else
                access.Where(x => x.Type == type && x.Name.Contains(searchText));

            return await access.CountAsync();
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
