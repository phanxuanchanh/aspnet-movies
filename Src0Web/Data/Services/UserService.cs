using Common.Hash;
using Data.BLL;
using Data.DAL;
using Data.DTO;
using System;
using System.Threading.Tasks;
using Web.Shared.Result;

namespace Data.Services
{
    public class UserService : IDisposable
    {
        private readonly GeneralDao _generalDao;
        private readonly UserDao _userDao;
        private readonly RoleDao _roleDao;
        private bool disposedValue;

        public UserService(GeneralDao generalDao) {
            _generalDao = generalDao;
            _userDao = generalDao.UserDao;
            _roleDao = generalDao.RoleDao;
        }

        public async Task<ExecResult> LoginAsync(UserLogin userLogin)
        {
            if (userLogin == null)
                throw new Exception("@'userLogin' must not be null");

            if (userLogin.UserName == null || userLogin.Password == null)
                throw new Exception("");

            User user = await _userDao.GetByUserNameAsync(userLogin.UserName);
            if (user == null)
                return ExecResult.NotFound("User does not exist");

            string passwordHashed = HashFunction.PBKDF2_Hash(userLogin.Password, user.Salt, 30);
            if (user.Password != passwordHashed)
                return ExecResult.Failure("Wrong password");
            if (!user.Activated)
                return ExecResult.Failure("User is not activated");

            return ExecResult.Success("");
        }

        public async Task<ExecResult> RegisterAsync(CreateUserDto input)
        {
            if (input == null)
                throw new Exception("@'userCreation' must not be null");

            if (
                input.UserName == null || input.Password == null
                || input.Email == null || input.PhoneNumber == null
            )
            {
                throw new Exception("");
            }

            User user1 = await _userDao.GetByUserNameAsync(input.UserName);
            User user2 = await _userDao.GetByEmailAsync(input.Email);
            if (user1 != null || user2 != null)
                return ExecResult.Failure("User already exists");

            string salt = HashFunction.MD5_Hash(new Random().NextString(25));

            User newUser = new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = input.UserName,
                Password = HashFunction.PBKDF2_Hash(input.Password, salt, 30),
                Salt = salt,
                Email = input.Email,
                PhoneNumber = input.PhoneNumber,
                Activated = false
            };

            int affected = await _userDao.InsertAsync(newUser);
            if (affected == 0)
                return ExecResult.Failure("Failed to create user");

            return ExecResult.Success("User created successfully");
        }

        public async Task<ExecResult> CreateNewPasswordAsync(string userId, string newPassword)
        {
            if (string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(userId))
                throw new Exception("");

            User user = await _userDao.GetAsync(userId);
            if (user == null)
                return ExecResult.NotFound("User does not exist");

            string salt = HashFunction.MD5_Hash(new Random().NextString(25));
            user.Password = HashFunction.PBKDF2_Hash(newPassword, salt, 30);
            user.Salt = salt;
            user.UpdatedAt = DateTime.Now;

            int affected = await _userDao.UpdateAsync(user);
            if(affected == 0)
                return ExecResult.Failure("Failed to update password");

            return ExecResult.Success("Password updated successfully");
        }

        public async Task<ExecResult<UserDto>> GetUserAsync(string id)
        {
            User user = await _userDao.GetAsync(id);
            if(user == null)
                return new ExecResult<UserDto> { Status = ExecStatus.NotFound, Message = "User not found" };

            return new ExecResult<UserDto>
            {
                Status = ExecStatus.Success,
                Message = "User retrieved successfully",
                Data = new UserDto
                {
                    ID = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Activated = user.Activated,
                    CreatedAt = user.CreatedAt,
                    UpdatedAt = user.UpdatedAt
                }
            };
        }

        public async Task<ExecResult<UserDto>> GetUserByUsernameAsync(string username)
        {
            User user = await _userDao.GetByUserNameAsync(username);
            if (user == null)
                return new ExecResult<UserDto> { Status = ExecStatus.NotFound, Message = "User not found" };

            Role role = await _roleDao.GetAsync(user.RoleId);
            if (role == null)
                return new ExecResult<UserDto> { Status = ExecStatus.NotFound, Message = "Role not found" };

            return new ExecResult<UserDto>
            {
                Status = ExecStatus.Success,
                Message = "User retrieved successfully",
                Data = new UserDto
                {
                    ID = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Role = new RoleDto
                    {
                        ID = role.Id,
                        Name = role.Name
                    },
                    Activated = user.Activated,
                    CreatedAt = user.CreatedAt,
                    UpdatedAt = user.UpdatedAt
                }
            };
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

        public async Task<PagedList<UserDto>> GetUsersAsync(int selectedIndex, int v)
        {
            throw new NotImplementedException();
        }

        public async Task<ExecResult> ActiveUserAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<ExecResult<UserDto>> GetUserByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }
    }
}
