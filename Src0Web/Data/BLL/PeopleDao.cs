using Data.DAL;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Text;
using MSSQL.Mapper;

namespace Data.BLL
{
    public class PeopleDao
    {
        private readonly DBContext _context;

        internal PeopleDao(DBContext context)
        {
            _context = context;
        }

        public async Task<People> GetAsync(long id)
        {
            return await _context.People.SingleOrDefaultAsync(x => x.Id == id);
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

            return await _context.GetHelper().ExecuteReaderAsync<People>(commandTextBuilder.ToString(), parameters, r => SqlMapper.MapRow<People>(r));
        }

        public async Task<List<People>> GetsAsync(string type = "director", long skip = 0, long take = 0)
        {
            return await _context.People
                .Where(x => x.Type == type).OrderBy(o => new { o.Id }).ToListAsync();

            //long count = await _context.People.CountAsync(x => x.Type == type);
        }

        public async Task<int> AddAsync(People people)
        {
            people.CreatedAt = DateTime.Now;

            return await _context.People.InsertAsync(people, new List<string> { "Id", "UpdatedAt", "DeletedAt" });
        }

        public async Task<int> UpdateAsync(People people)
        {
            people.UpdatedAt = DateTime.Now;
            return await _context.People
                .Where(x => x.Id == people.Id)
                .UpdateAsync(people, s => new { s.Name, s.Description, s.UpdatedAt });
        }

        public async Task<int> DeleteAsync(long id, bool forceDelete = false)
        {
            People people = await GetAsync(id);
            if (people == null)
                return 0;

            if (forceDelete)
                return await _context.People.DeleteAsync(x => x.Id == id);

            people.DeletedAt = DateTime.Now;
            return await _context.People
                .Where(x => x.Id == id)
                .UpdateAsync(people, s => new { s.DeletedAt });
        }
    }
}