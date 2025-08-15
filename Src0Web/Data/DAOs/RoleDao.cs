using Data.Base;
using Data.DAL;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.DAOs
{
    public class RoleDao : GenericDao<Role>
    {
        private readonly DBContext _context;

        public RoleDao(DBContext context)
            :base(context, x => x.Roles)
        {
            _context = context;
        }

        public async Task<List<Role>> GetsAsync()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<Role> GetByNameAsync(string name)
        {
            return await _context.Roles
                .FirstOrDefaultAsync(x => x.Name == name && x.DeletedAt == null);
        }
    }
}
