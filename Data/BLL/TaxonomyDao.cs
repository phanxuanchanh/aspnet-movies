using Data.DAL;
using MSSQL.Access;
using MSSQL.Query;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;
using System.Threading.Tasks;

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
            return await _context.Taxonomy.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<SqlPagedList<Taxonomy>> GetsAsync(string type = "category", long pageIndex = 1, long pageSize = 10)
        {
            SqlPagedList<Taxonomy> pagedList = null;
            Expression<Func<Taxonomy, object>> orderBy = c => new { c.Id };

            pagedList = await _context.Taxonomy.ToPagedListAsync(x => x.Type == type, orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);

            return pagedList;
        }

        public async Task<int> AddAsync(Taxonomy Taxonomy)
        {
            Taxonomy.CreatedAt = DateTime.Now;

            return await _context.Taxonomy.InsertAsync(Taxonomy, new List<string> { "Id", "UpdatedAt", "DeletedAt" });
        }

        public async Task<int> UpdateAsync(Taxonomy Taxonomy)
        {
            Taxonomy.UpdatedAt = DateTime.Now;
            return await _context.Taxonomy.UpdateAsync(Taxonomy, s => new { s.Name, s.Description, s.UpdatedAt }, x => x.Id == Taxonomy.Id);
        }

        public async Task<int> DeleteAsync(int id, bool forceDelete = false)
        {
            Taxonomy Taxonomy = await GetAsync(id);
            if (Taxonomy == null)
                return 0;

            if (forceDelete)
                return await _context.Taxonomy.DeleteAsync(x => x.Id == id);

            Taxonomy.DeletedAt = DateTime.Now;
            return await _context.Taxonomy.UpdateAsync(Taxonomy, s => new { s.DeletedAt }, x => x.Id == id);
        }
    }
}
