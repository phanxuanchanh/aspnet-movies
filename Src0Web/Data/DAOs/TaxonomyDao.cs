using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using MSSQL.Mapper;
using MSSQL.Access;
using Data.Base;
using Data.Context;
using Data.Models;

namespace Data.DAOs
{
    public class TaxonomyDao : GenericDao<Taxonomy>
    {
        public TaxonomyDao(DBContext context)
            :base(context, x => x.Taxonomies)
        {

        }

        public async Task<List<Taxonomy>> GetsByIdsAsync(params int[] ids)
        {
            StringBuilder commandTextBuilder = new StringBuilder("SELECT * FROM Taxonomies WHERE Id IN (");
            //SqlParameter[] parameters = new SqlParameter[ids.Count];
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            for (int i = 0; i < ids.Length; i++)
            {
                commandTextBuilder.Append($"@Id{i}, ");
                //parameters[i] = new SqlParameter($"@Id{i}", ids[i]);
                parameters.Add($"@Id{i}", ids[i]);
            }
            commandTextBuilder.Append(")").Replace(", )", " )");

            return await Context.GetHelper()
                .ExecuteReaderAsync<Taxonomy>(commandTextBuilder.ToString(), parameters, r => SqlMapper.MapRow<Taxonomy>(r));
        }

        public async Task<List<Taxonomy>> GetManyAsync(string type = "category", long skip = 1, long take = 10, string searchText = null)
        {
            SqlAccess<Taxonomy> access = Context.Taxonomies
                .Where(x => x.DeletedAt == null).OrderBy(o => o.Id);

            if(string.IsNullOrEmpty(searchText))
                access.Where(x => x.Type == type);
            else
                access.Where(x => x.Type == type && x.Name.Contains(searchText));

            return await access.ToListAsync();
        }

        public async Task<long> CountAsync(string type = "category", string searchText = null)
        {
            SqlAccess<Taxonomy> access = Context.Taxonomies
                .Where(x => x.DeletedAt == null).OrderBy(o => o.Id);

            if (string.IsNullOrEmpty(searchText))
                access.Where(x => x.Type == type);
            else
                access.Where(x => x.Type == type && x.Name.Contains(searchText));

            return await access.CountAsync();
        }
    }
}
