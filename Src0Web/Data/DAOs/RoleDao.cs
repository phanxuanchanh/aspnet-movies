using Common.Hash;
using Data.DAL;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.BLL
{
    public class RoleDao
    {
        private readonly DBContext _context;

        public RoleDao(DBContext context)
        {
            _context = context;
        }

        public async Task<List<Role>> GetsAsync()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<Role> GetAsync(string roleId)
        {
            return await _context.Roles
                .FirstOrDefaultAsync(x => x.Id == roleId && x.DeletedAt == null);
        }

        public async Task<Role> GetByNameAsync(string name)
        {
            return await _context.Roles
                .FirstOrDefaultAsync(x => x.Name == name && x.DeletedAt == null);
        }

        public async Task<int> AddAsync(Role role)
        {
            role.CreatedAt = DateTime.Now;

            return await _context.Roles.InsertAsync(role, new List<string> { "Id", "UpdatedAt", "DeletedAt" });
        }

        public async Task<int> UpdateAsync(Role role)
        {
            role.UpdatedAt = DateTime.Now;
            return await _context.Roles
                .Where(x => x.Id == role.Id)
                .UpdateAsync(role, s => new { s.Name, s.UpdatedAt });
        }

        public async Task<int> DeleteAsync(string id, bool forceDelete = false)
        {
            Role role = await GetAsync(id);
            if (role == null)
                return 0;

            if (forceDelete)
                return await _context.Roles.DeleteAsync(x => x.Id == id);

            role.DeletedAt = DateTime.Now;
            return await _context.Roles
                .Where(x => x.Id == id)
                .UpdateAsync(role, s => new { s.DeletedAt });
        }
    }
}
