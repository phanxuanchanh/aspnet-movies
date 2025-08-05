using Data.DAL;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using System.Text;
using MSSQL.Mapper;
using MSSQL.Access;

namespace Data.BLL
{
    public class TaxonomyDao
    {
        private readonly DBContext _context;

        public TaxonomyDao(DBContext context)
        {
            _context = context;
        }

        public async Task<Taxonomy> GetAsync(int id)
        {
            return await _context.Taxonomies.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Taxonomy>> GetsByIdsAsync(List<int> ids)
        {
            StringBuilder commandTextBuilder = new StringBuilder("SELECT * FROM Taxonomy WHERE Id IN (");
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
                .ExecuteReaderAsync<Taxonomy>(commandTextBuilder.ToString(), parameters, r => SqlMapper.MapRow<Taxonomy>(r));
        }

        public async Task<List<Taxonomy>> GetsAsync(string type = "category", long skip = 0, long take = 0, string searchText = null)
        {
            SqlAccess<Taxonomy> access = _context.Taxonomies
                .Where(x => x.DeletedAt == null).OrderBy(o => new { o.Id });

            if(string.IsNullOrEmpty(searchText))
                access.Where(x => x.Type == type);
            else
                access.Where(x => x.Type == type && x.Name.Contains(searchText));

            return await access.ToListAsync();
        }

        public async Task<long> CountAsync(string type = "category", string searchText = null)
        {
            SqlAccess<Taxonomy> access = _context.Taxonomies
                .Where(x => x.DeletedAt == null).OrderBy(o => new { o.Id });

            if (string.IsNullOrEmpty(searchText))
                access.Where(x => x.Type == type);
            else
                access.Where(x => x.Type == type && x.Name.Contains(searchText));

            return await access.CountAsync();
        }

        public async Task<int> AddAsync(Taxonomy taxonomy)
        {
            taxonomy.CreatedAt = DateTime.Now;

            return await _context.Taxonomies.InsertAsync(taxonomy, new List<string> { "Id", "UpdatedAt", "DeletedAt" });
        }

        public async Task<int> UpdateAsync(Taxonomy taxonomy)
        {
            taxonomy.UpdatedAt = DateTime.Now;
            return await _context.Taxonomies
                .Where(x => x.Id == taxonomy.Id)
                .UpdateAsync(taxonomy, s => new { s.Name, s.Description, s.UpdatedAt });
        }

        public async Task<int> DeleteAsync(int id, bool forceDelete = false)
        {
            Taxonomy taxonomy = await GetAsync(id);
            if (taxonomy == null)
                return 0;

            if (forceDelete)
                return await _context.Taxonomies.DeleteAsync(x => x.Id == id);

            taxonomy.DeletedAt = DateTime.Now;
            return await _context.Taxonomies
                .Where(x => x.Id == id)
                .UpdateAsync(taxonomy, s => new { s.DeletedAt });
        }
    }
}
