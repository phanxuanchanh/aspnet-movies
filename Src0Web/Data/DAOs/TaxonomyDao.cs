using Data.DAL;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using System.Text;
using MSSQL.Mapper;
using MSSQL.Access;
using Data.Base;
using System.Linq.Expressions;

namespace Data.BLL
{
    public class TaxonomyDao : GenericDao<Taxonomy, int>
    {
        public TaxonomyDao()
            :base(x => x.Taxonomies)
        {

        }

        protected override Expression<Func<Taxonomy, bool>> SetPkExpr(int id)
        {
            return x => x.Id == id;
        }

        protected override Expression<Func<Taxonomy, object>> SetUpdateSelectorExpr(Taxonomy input)
        {
            return s => new { s.Name, s.Description, s.UpdatedAt };
        }

        public async Task<List<Taxonomy>> GetsByIdsAsync(List<int> ids)
        {
            StringBuilder commandTextBuilder = new StringBuilder("SELECT * FROM Taxonomies WHERE Id IN (");
            //SqlParameter[] parameters = new SqlParameter[ids.Count];
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            for (int i = 0; i < ids.Count; i++)
            {
                commandTextBuilder.Append($"@Id{i}, ");
                //parameters[i] = new SqlParameter($"@Id{i}", ids[i]);
                parameters.Add($"@Id{i}", ids[i]);
            }
            commandTextBuilder.Append(")").Replace(", )", " )");

            return await context.GetHelper()
                .ExecuteReaderAsync<Taxonomy>(commandTextBuilder.ToString(), parameters, r => SqlMapper.MapRow<Taxonomy>(r));
        }

        public async Task<List<Taxonomy>> GetsAsync(string type = "category", long skip = 0, long take = 0, string searchText = null)
        {
            SqlAccess<Taxonomy> access = context.Taxonomies
                .Where(x => x.DeletedAt == null).OrderBy(o => new { o.Id });

            if(string.IsNullOrEmpty(searchText))
                access.Where(x => x.Type == type);
            else
                access.Where(x => x.Type == type && x.Name.Contains(searchText));

            return await access.ToListAsync();
        }

        public async Task<long> CountAsync(string type = "category", string searchText = null)
        {
            SqlAccess<Taxonomy> access = context.Taxonomies
                .Where(x => x.DeletedAt == null).OrderBy(o => new { o.Id });

            if (string.IsNullOrEmpty(searchText))
                access.Where(x => x.Type == type);
            else
                access.Where(x => x.Type == type && x.Name.Contains(searchText));

            return await access.CountAsync();
        }
    }
}
