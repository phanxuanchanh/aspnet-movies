using Data.DAL;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using MSSQL.Mapper;
using MSSQL.Access;
using Data.Base;

namespace Data.DAOs
{
    public class PeopleDao : GenericDao<People>
    {
        public PeopleDao(DBContext context)
            : base(context, x => x.People)
        {
        }

        public async Task<List<People>> GetsByIdsAsync(List<long> ids)
        {
            StringBuilder commandTextBuilder = new StringBuilder("SELECT * FROM People WHERE Id IN (");
/*            SqlParameter[] parameters = new SqlParameter[ids.Count]*/;
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            for (int i = 0; i < ids.Count; i++)
            {
                commandTextBuilder.Append($"@Id{i}, ");
                //parameters[i] = new SqlParameter($"@Id{i}", ids[i]);
                parameters.Add($"@Id{i}", ids[i]);
            }
            commandTextBuilder.Append(")").Replace(", )", " )");

            return await Context.GetHelper().ExecuteReaderAsync<People>(commandTextBuilder.ToString(), parameters, r => SqlMapper.MapRow<People>(r));
        }

        public async Task<List<People>> GetsAsync(string type = "director", long skip = 0, long take = 0, string searchText = null)
        {
            SqlAccess<People> access = Context.People
                .Where(x => x.DeletedAt == null).OrderBy(o => new { o.Id });

            if(string.IsNullOrEmpty(searchText))
                access.Where(x => x.Type == type);
            else
                access.Where(x => x.Type == type && x.Name.Contains(searchText));

            return await access.ToListAsync();
        }

        public async Task<long> CountAsync(string type = "director", string searchText = null)
        {
            SqlAccess<People> access = Context.People
                .Where(x => x.DeletedAt == null).OrderBy(o => new { o.Id });

            if (string.IsNullOrEmpty(searchText))
                access.Where(x => x.Type == type);
            else
                access.Where(x => x.Type == type && x.Name.Contains(searchText));

            return await access.CountAsync();
        }
    }
}