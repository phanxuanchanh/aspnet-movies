using Data.DAL;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.BLL
{
    public class FilmMetadataDao
    {
        private readonly DBContext _context;

        public FilmMetadataDao(DBContext context) {
            _context = context;
        }

        public async Task<FilmMetadata> GetAsync(int id)
        {
            return await _context.FilmMetadata.FirstOrDefaultAsync(x => x.ID);
        }

        public async Task<List<FilmMetadata>> GetsAsync(string type = "language")
        {
            return await _context.FilmMetadata.ToListAsync(x => x.Type == type);
        }

        public async Task AddAsync(FilmMetadata metadata)
        {
            metadata.CreatedAt = DateTime.Now;

            await _context.FilmMetadata.InsertAsync(metadata);
        }

        public async Task UpdateAsync(FilmMetadata metadata)
        {
            metadata.UpdatedAt = DateTime.Now;
            await _context.FilmMetadata.UpdateAsync(metadata, s => new { });
        }

        public async Task DeleteAsync(int id, bool forceDelete = false)
        {
            FilmMetadata metadata = await GetAsync(id);
            if (metadata == null)
                return;

            if (forceDelete)
            {
                await _context.FilmMetadata.DeleteAsync(x => x.ID == id);
                return;
            }

            metadata.DeletedAt = DateTime.Now;
            await _context.FilmMetadata.UpdateAsync(metadata, s => new { s.DeletedAt });
        }
    }
}
