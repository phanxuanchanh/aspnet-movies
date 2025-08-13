using Data.Base;
using Data.DAL;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Data.BLL
{
    public class RoleDao : GenericDao<Role, string>
    {
        private readonly DBContext _context;

        public RoleDao()
            :base(x => x.Roles)
        {
            _context = context;
        }

        protected override Expression<Func<Role, bool>> SetPkExpr(string id)
        {
            return x => x.Id == id;
        }

        protected override Expression<Func<Role, object>> SetUpdateSelectorExpr(Role input)
        {
            return s => new { s.Name, s.UpdatedAt };
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
