using Common;
using Common.Web;
using Data.BLL;
using Data.DTO;
using System;
using System.Threading.Tasks;

namespace Data.Services
{
    public class RoleService : IDisposable
    {
        private readonly GeneralDao _generalDao;
        private readonly RoleDao _roleDao;
        private bool disposedValue;

        public RoleService(GeneralDao generalDao)
        {
            _generalDao = generalDao;
            _roleDao = generalDao.RoleDao;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {

                }

                _generalDao.Dispose();
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
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
