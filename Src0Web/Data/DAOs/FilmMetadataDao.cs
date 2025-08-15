using Data.Base;
using Data.DAL;
using MSSQL.Access;
using MSSQL.Mapper;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAOs
{
    public class FilmMetadataDao : GenericDao<FilmMetadata>
    {
        public FilmMetadataDao(DBContext context) 
            : base(context, x => x.FilmMetadata)
        {

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

            return await Context.GetHelper()
                .ExecuteReaderAsync<FilmMetadata>(commandTextBuilder.ToString(), parameters, r => SqlMapper.MapRow<FilmMetadata>(r));
        }

        public async Task<List<FilmMetadata>> GetsAsync(string type = "language", long skip = 1, long take = 10, string searchText = null)
        {
            SqlAccess<FilmMetadata> access = Context.FilmMetadata
                .Where(x => x.DeletedAt == null).OrderBy(o => new { o.Id });

            if(string.IsNullOrEmpty(searchText))
                access.Where(x => x.Type == type);
            else
                access.Where(x => x.Type == type && x.Name.Contains(searchText));

            return await access.ToListAsync();
        }

        public async Task<long> CountAsync(string type = "language", string searchText = null)
        {
            SqlAccess<FilmMetadata> access = Context.FilmMetadata
                .Where(x => x.DeletedAt == null);
            if (string.IsNullOrEmpty(searchText))
                access.Where(x => x.Type == type);
            else
                access.Where(x => x.Type == type && x.Name.Contains(searchText));

            return await access.CountAsync();
        }
    }
}
