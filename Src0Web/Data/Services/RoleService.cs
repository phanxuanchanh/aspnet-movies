using Data.DAOs;
using Data.DTO;
using Data.Models;
using System;
using System.Threading.Tasks;
using Web.Shared.Mapper;
using Web.Shared.Result;

namespace Data.Services
{
    public class RoleService
    {
        private readonly RoleDao _roleDao;
        private readonly IMapper _mapper;

        public RoleService(RoleDao roleDao, IMapper mapper)
        {
            _roleDao = roleDao;
            _mapper = mapper;
        }

        public async Task<PagedList<RoleDto>> GetRolesAsync(int selectedIndex, int v)
        {
            throw new NotImplementedException();
        }

        public async Task<ExecResult<RoleDto>> GetRoleAsync(string id)
        {
            Role role = await _roleDao.GetAsync(x => x.Id == id);
            if (role == null)
                return ExecResult<RoleDto>.NotFound("", null);

            RoleDto roleOutput = _mapper.Map<Role, RoleDto>(role);
            return ExecResult<RoleDto>.Success("", roleOutput);
        }

        public async Task<ExecResult<RoleDto>> GetRoleByNameAsync(string name)
        {
            Role role = await _roleDao.GetAsync(x => x.Name == name);
            if (role == null)
                return ExecResult<RoleDto>.NotFound("Role not found.", null);

            RoleDto roleOutput = _mapper.Map<Role, RoleDto>(role);
            return ExecResult<RoleDto>.Success("Role found.", roleOutput);
        }

        public async Task<ExecResult> UpdateAsync(CreateRoleDto createRoleDto)
        {
            Role role = _mapper.Map<CreateRoleDto, Role>(createRoleDto);

            int affected = await _roleDao.UpdateAsync(
                role, x => x.Id == createRoleDto.ID, s => new { s.Name , s.CreatedAt });

            if (affected <= 0)
                return ExecResult.Failure("Failed to update role.");

            return ExecResult.Success("Role updated successfully.");
        }

        public async Task<ExecResult<RoleDto>> CreateAsync(CreateRoleDto createRoleDto)
        {
            throw new NotImplementedException();
        }
    }
}
