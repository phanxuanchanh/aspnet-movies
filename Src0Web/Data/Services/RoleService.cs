using Data.DAOs;
using Data.DTO;
using System;
using System.Threading.Tasks;
using Web.Shared.Result;

namespace Data.Services
{
    public class RoleService
    {
        private readonly RoleDao _roleDao;

        public RoleService(RoleDao roleDao)
        {
            _roleDao = roleDao;
        }

        public async Task<PagedList<RoleDto>> GetRolesAsync(int selectedIndex, int v)
        {
            throw new NotImplementedException();
        }

        public async Task<RoleDto> GetRoleAsync(string key)
        {
            throw new NotImplementedException();
        }

        public async Task<ExecResult<RoleDto>> UpdateAsync(CreateRoleDto createRoleDto)
        {
            throw new NotImplementedException();
        }

        public async Task<ExecResult<RoleDto>> CreateAsync(CreateRoleDto createRoleDto)
        {
            throw new NotImplementedException();
        }
    }
}
