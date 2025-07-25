﻿using Data.DAL;
using MSSQL.Access;
using MSSQL.Query;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using Common.Web;

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
            SqlParameter[] parameters = new SqlParameter[ids.Count];
            for (int i = 0; i < ids.Count; i++)
            {
                commandTextBuilder.Append($"@Id{i}, ");
                parameters[i] = new SqlParameter($"@Id{i}", ids[i]);
            }
            commandTextBuilder.Append(")").Replace(", )", " )");

            return await _context.Execute_ToListAsync<Taxonomy>(commandTextBuilder.ToString(), CommandType.Text, parameters);
        }

        public async Task<PagedList<Taxonomy>> GetsAsync(string type = "category", long pageIndex = 1, long pageSize = 10)
        {
            long skip = (pageIndex - 1) * pageSize;
            List<Taxonomy> taxonomies = await _context.Taxonomies
                .Where(x => x.Type == type).OrderBy(o => new { o.Id }).ToListAsync();

            //long count = await _context.Taxonomies.CountAsync(x => x.Type == type);

            return new PagedList<Taxonomy>
            {
                Items = taxonomies,
                CurrentPage = pageIndex,
            };
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
