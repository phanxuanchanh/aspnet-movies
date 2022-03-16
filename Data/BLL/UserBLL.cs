using Common.Hash;
using Common.Web;
using Data.DAL;
using Data.DTO;
using MSSQL.Access;
using MSSQL.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Data.BLL
{
    public class UserBLL : BusinessLogicLayer
    {
        private bool disposed;
        private bool includeRole;

        public bool IncludeRole { set { includeRole = value; } }

        public UserBLL()
            : base()
        {
            InitDAL();
            SetDefault();
            disposed = false;
        }

        public UserBLL(BusinessLogicLayer bll)
            : base()
        {
            InitDAL(bll.db);
            SetDefault();
            disposed = false;
        }

        public override void SetDefault()
        {
            base.SetDefault();
            includeRole = false;
        }

        private UserInfo ToUserInfo(User user)
        {
            if (user == null)
                return null;

            UserInfo userInfo = new UserInfo
            {
                ID = user.ID,
                userName = user.userName,
                surName = user.surName,
                middleName = user.middleName,
                name = user.name,
                description = user.description,
                phoneNumber = user.phoneNumber,
                email = user.email,
                activated = user.activated
            };

            if (includeRole)
                userInfo.Role = new RoleBLL(this).GetRole(user.roleId);

            if (includeTimestamp)
            {
                userInfo.createAt = user.createAt;
                userInfo.updateAt = user.updateAt;
            }

            return userInfo;
        }

        private async Task<User> ToUser(UserCreation userCreation)
        {
            if (userCreation == null)
                throw new Exception("@'userCreation' must not be null");

            HashFunction hash = new HashFunction();
            string salt = hash.MD5_Hash(new Random().NextString(25));
            Role role = await db.Roles.SingleOrDefaultAsync(r => new { r.ID }, r => r.name == "User");
            if (role == null)
                throw new Exception("@'role' must not be null");

            return new User
            {
                ID = Guid.NewGuid().ToString(),
                userName = userCreation.userName,
                email = userCreation.email,
                phoneNumber = userCreation.phoneNumber,
                password = hash.PBKDF2_Hash(userCreation.password, salt, 30),
                salt = salt,
                roleId = role.ID,
                activated = false,
                createAt = DateTime.Now,
                updateAt = DateTime.Now
            };
        }

        private User ToUser(UserUpdate userUpdate)
        {
            if (userUpdate == null)
                throw new Exception("@'userUpdate' must not be null");

            return new User
            {

            };
        }

        public async Task<List<UserInfo>> GetUsersAsync()
        {
            List<UserInfo> users = null;
            if (includeRole && includeTimestamp)
                users = (await db.Users.ToListAsync()).Select(u => ToUserInfo(u)).ToList();
            else if (includeRole)
                users = (await db.Users.ToListAsync(u => new
                {
                    u.ID,
                    u.userName,
                    u.surName,
                    u.middleName,
                    u.name,
                    u.description,
                    u.email,
                    u.phoneNumber,
                    u.roleId
                })).Select(u => ToUserInfo(u)).ToList();
            else if (includeTimestamp)
                users = (await db.Users.ToListAsync(u => new
                {
                    u.ID,
                    u.userName,
                    u.surName,
                    u.middleName,
                    u.name,
                    u.description,
                    u.email,
                    u.phoneNumber,
                    u.createAt,
                    u.updateAt
                })).Select(u => ToUserInfo(u)).ToList();
            else
                users = (await db.Users.ToListAsync(u => new
                {
                    u.ID,
                    u.userName,
                    u.surName,
                    u.middleName,
                    u.name,
                    u.description,
                    u.email,
                    u.phoneNumber
                })).Select(u => ToUserInfo(u)).ToList();

            return users;
        }

        public List<UserInfo> GetUsers()
        {
            List<UserInfo> users = null;
            if (includeRole && includeTimestamp)
                users = db.Users.ToList().Select(u => ToUserInfo(u)).ToList();
            else if (includeRole)
                users = db.Users.ToList(u => new
                {
                    u.ID,
                    u.userName,
                    u.surName,
                    u.middleName,
                    u.name,
                    u.description,
                    u.email,
                    u.phoneNumber,
                    u.roleId
                }).Select(u => ToUserInfo(u)).ToList();
            else if (includeTimestamp)
                users = db.Users.ToList(u => new
                {
                    u.ID,
                    u.userName,
                    u.surName,
                    u.middleName,
                    u.name,
                    u.description,
                    u.email,
                    u.phoneNumber,
                    u.createAt,
                    u.updateAt
                }).Select(u => ToUserInfo(u)).ToList();
            else
                users = db.Users.ToList(u => new
                {
                    u.ID,
                    u.userName,
                    u.surName,
                    u.middleName,
                    u.name,
                    u.description,
                    u.email,
                    u.phoneNumber
                }).Select(u => ToUserInfo(u)).ToList();

            return users;
        }

        public async Task<PagedList<UserInfo>> GetUsersAsync(int pageIndex, int pageSize)
        {
            SqlPagedList<User> pagedList = null;
            Expression<Func<User, object>> orderBy = u => new { u.ID };
            if (includeRole && includeTimestamp)
                pagedList = await db.Users.ToPagedListAsync(orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);
            else if (includeRole)
                pagedList = await db.Users.ToPagedListAsync(
                    u => new
                    {
                        u.ID,
                        u.userName,
                        u.surName,
                        u.middleName,
                        u.name,
                        u.description,
                        u.email,
                        u.phoneNumber,
                        u.roleId
                    },
                    orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize
                );
            else if (includeTimestamp)
                pagedList = await db.Users.ToPagedListAsync(
                    u => new
                    {
                        u.ID,
                        u.userName,
                        u.surName,
                        u.middleName,
                        u.name,
                        u.description,
                        u.email,
                        u.phoneNumber,
                        u.createAt,
                        u.updateAt
                    },
                    orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize
                );
            else
                pagedList = await db.Users.ToPagedListAsync(
                    u => new
                    {
                        u.ID,
                        u.userName,
                        u.surName,
                        u.middleName,
                        u.name,
                        u.description,
                        u.email,
                        u.phoneNumber
                    },
                    orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize
                );

            return new PagedList<UserInfo>
            {
                PageNumber = pagedList.PageNumber,
                CurrentPage = pagedList.CurrentPage,
                Items = pagedList.Items.Select(u => ToUserInfo(u)).ToList()
            };
        }

        public PagedList<UserInfo> GetUsers(int pageIndex, int pageSize)
        {
            SqlPagedList<User> pagedList = null;
            Expression<Func<User, object>> orderBy = u => new { u.ID };
            if (includeRole && includeTimestamp)
                pagedList = db.Users.ToPagedList(orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);
            else if (includeRole)
                pagedList = db.Users.ToPagedList(
                    u => new
                    {
                        u.ID,
                        u.userName,
                        u.surName,
                        u.middleName,
                        u.name,
                        u.description,
                        u.email,
                        u.phoneNumber,
                        u.roleId
                    },
                    orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize
                );
            else if (includeTimestamp)
                pagedList = db.Users.ToPagedList(
                    u => new
                    {
                        u.ID,
                        u.userName,
                        u.surName,
                        u.middleName,
                        u.name,
                        u.description,
                        u.email,
                        u.phoneNumber,
                        u.createAt,
                        u.updateAt
                    },
                    orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize
                );
            else
                pagedList = db.Users.ToPagedList(
                    u => new
                    {
                        u.ID,
                        u.userName,
                        u.surName,
                        u.middleName,
                        u.name,
                        u.description,
                        u.email,
                        u.phoneNumber
                    },
                    orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize
                );

            return new PagedList<UserInfo>
            {
                PageNumber = pagedList.PageNumber,
                CurrentPage = pagedList.CurrentPage,
                Items = pagedList.Items.Select(u => ToUserInfo(u)).ToList()
            };
        }

        public async Task<UserInfo> GetUserAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new Exception("@'userId' must not be null or empty");

            User user = null;
            if (includeRole && includeTimestamp)
                user = await db.Users.SingleOrDefaultAsync(u => u.ID == userId);
            else if (includeRole)
                user = await db.Users
                    .SingleOrDefaultAsync(u => new
                    {
                        u.ID,
                        u.userName,
                        u.surName,
                        u.middleName,
                        u.name,
                        u.description,
                        u.email,
                        u.phoneNumber,
                        u.roleId
                    }, u => u.ID == userId);
            else if (includeTimestamp)
                user = await db.Users
                    .SingleOrDefaultAsync(u => new
                    {
                        u.ID,
                        u.userName,
                        u.surName,
                        u.middleName,
                        u.name,
                        u.description,
                        u.email,
                        u.phoneNumber,
                        u.createAt,
                        u.updateAt
                    }, u => u.ID == userId);
            else
                user = await db.Users
                   .SingleOrDefaultAsync(u => new
                   {
                       u.ID,
                       u.userName,
                       u.surName,
                       u.middleName,
                       u.name,
                       u.description,
                       u.email,
                       u.phoneNumber
                   }, u => u.ID == userId);

            return ToUserInfo(user);
        }

        public UserInfo GetUser(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new Exception("@'userId' must not be null or empty");

            User user = null;
            if (includeRole && includeTimestamp)
                user = db.Users.SingleOrDefault(u => u.ID == userId);
            else if (includeRole)
                user = db.Users
                    .SingleOrDefault(u => new
                    {
                        u.ID,
                        u.userName,
                        u.surName,
                        u.middleName,
                        u.name,
                        u.description,
                        u.email,
                        u.phoneNumber,
                        u.roleId
                    }, u => u.ID == userId);
            else if (includeTimestamp)
                user = db.Users
                    .SingleOrDefault(u => new
                    {
                        u.ID,
                        u.userName,
                        u.surName,
                        u.middleName,
                        u.name,
                        u.description,
                        u.email,
                        u.phoneNumber,
                        u.createAt,
                        u.updateAt
                    }, u => u.ID == userId);
            else
                user = db.Users
                    .SingleOrDefault(u => new
                    {
                        u.ID,
                        u.userName,
                        u.surName,
                        u.middleName,
                        u.name,
                        u.description,
                        u.email,
                        u.phoneNumber
                    }, u => u.ID == userId);

            return ToUserInfo(user);
        }

        public async Task<UserInfo> GetUserByUserNameAsync(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                throw new Exception("@'userName' must not be null or empty");

            User user = null;
            if (includeRole && includeTimestamp)
                user = await db.Users.SingleOrDefaultAsync(u => u.userName == userName);
            else if (includeRole)
                user = await db.Users
                    .SingleOrDefaultAsync(u => new
                    {
                        u.ID,
                        u.userName,
                        u.surName,
                        u.middleName,
                        u.name,
                        u.description,
                        u.email,
                        u.phoneNumber,
                        u.roleId
                    }, u => u.userName == userName);
            else if (includeTimestamp)
                user = await db.Users
                    .SingleOrDefaultAsync(u => new
                    {
                        u.ID,
                        u.userName,
                        u.surName,
                        u.middleName,
                        u.name,
                        u.description,
                        u.email,
                        u.phoneNumber,
                        u.createAt,
                        u.updateAt
                    }, u => u.userName == userName);
            else
                user = await db.Users
                    .SingleOrDefaultAsync(u => new
                    {
                        u.ID,
                        u.userName,
                        u.surName,
                        u.middleName,
                        u.name,
                        u.description,
                        u.email,
                        u.phoneNumber
                    }, u => u.userName == userName);

            return ToUserInfo(user);
        }

        public async Task<UserInfo> GetUserByEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
                throw new Exception("@'email' must not be null or empty");

            User user = null;
            if (includeRole && includeTimestamp)
                user = await db.Users
                     .SingleOrDefaultAsync(u => u.email == email);
            else if (includeRole)
                user = await db.Users
                    .SingleOrDefaultAsync(u => new
                    {
                        u.ID,
                        u.userName,
                        u.surName,
                        u.middleName,
                        u.name,
                        u.description,
                        u.email,
                        u.phoneNumber,
                        u.roleId
                    }, u => u.email == email);
            else if (includeTimestamp)
                user = await db.Users
                    .SingleOrDefaultAsync(u => new
                    {
                        u.ID,
                        u.userName,
                        u.surName,
                        u.middleName,
                        u.name,
                        u.description,
                        u.email,
                        u.phoneNumber,
                        u.createAt,
                        u.updateAt
                    }, u => u.email == email);
            else
                user = await db.Users
                    .SingleOrDefaultAsync(u => new
                    {
                        u.ID,
                        u.userName,
                        u.surName,
                        u.middleName,
                        u.name,
                        u.description,
                        u.email,
                        u.phoneNumber
                    }, u => u.email == email);

            return ToUserInfo(user);
        }

        public enum LoginState { Success, Unconfirmed, WrongPassword, NotExists };

        public async Task<LoginState> LoginAsync(UserLogin userLogin)
        {
            if (userLogin == null)
                throw new Exception("@'userLogin' must not be null");

            if (userLogin.userName == null || userLogin.password == null)
                throw new Exception("");

            User user = await db.Users.SingleOrDefaultAsync(
                    u => new { u.userName, u.password, u.salt, u.activated },
                    u => u.userName == userLogin.userName
                );
            if (user == null)
                return LoginState.NotExists;

            HashFunction hash = new HashFunction();
            string passwordHashed = hash.PBKDF2_Hash(userLogin.password, user.salt, 30);
            if (user.password != passwordHashed)
                return LoginState.WrongPassword;
            if (!user.activated)
                return LoginState.Unconfirmed;
            return LoginState.Success;
        }

        public async Task<bool> IsActiveAsync(string username)
        {
            return (await db.Users
                .SingleOrDefaultAsync(u => new { u.activated }, u => u.userName == username)).activated;
        }

        public enum ActiveUserState { Success, Failed, NotExists }

        public async Task<ActiveUserState> ActiveUserAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new Exception("@'userId' must not be null or empty");

            long count = await db.Users.CountAsync(u => u.ID == userId);
            if (count == 0)
                return ActiveUserState.NotExists;

            int affected = await db.Users.UpdateAsync(new User
            {
                activated = true,
                updateAt = DateTime.Now
            }, u => new { u.activated, u.updateAt }, u => u.ID == userId);

            return (affected == 0) ? ActiveUserState.Failed : ActiveUserState.Success;
        }

        public enum CreateNewPasswordState { Success, Failed, NotExists };

        public async Task<CreateNewPasswordState> CreateNewPasswordAsync(string userId, string newPassword)
        {
            if (string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(userId))
                throw new Exception("");

            long count = await db.Users.CountAsync(u => u.ID == userId);
            if (count == 0)
                return CreateNewPasswordState.NotExists;

            HashFunction hash = new HashFunction();
            string salt = hash.MD5_Hash(new Random().NextString(25));
            int affected = await db.Users.UpdateAsync(new User
            {
                password = hash.PBKDF2_Hash(newPassword, salt, 30),
                salt = salt,
                updateAt = DateTime.Now
            }, u => new { u.password, u.salt, u.updateAt }, u => u.ID == userId);

            return (affected == 0) ? CreateNewPasswordState.Failed : CreateNewPasswordState.Success;
        }

        public enum RegisterState { Success, Success_NoPaymentInfo, Failed, AlreadyExist };

        public async Task<RegisterState> RegisterAsync(UserCreation userCreation)
        {
            if (userCreation == null)
                throw new Exception("@'userCreation' must not be null");

            if (
                userCreation.userName == null || userCreation.password == null
                || userCreation.email == null || userCreation.phoneNumber == null
            )
            {
                throw new Exception("");
            }

            User usr = await db.Users
                .SingleOrDefaultAsync(u => u.userName == userCreation.userName || u.email == userCreation.email || u.phoneNumber == userCreation.phoneNumber);
            if (usr != null)
                return RegisterState.AlreadyExist;

            User userRegister = await ToUser(userCreation);
            int affected = await db.Users.InsertAsync(
                userRegister,
                new List<string> { "surName", "middleName", "name", "description" }
            );
            if (affected == 0)
                return RegisterState.Failed;
            if (userCreation.PaymentInfo == null)
                return RegisterState.Success_NoPaymentInfo;

            usr = await db.Users.SingleOrDefaultAsync(u => new { u.ID }, u => u.userName == userCreation.userName);
            userCreation.PaymentInfo.userId = usr.ID;
            CreationState state = await new PaymentInfoBLL(this).CreatePaymentInfoAsync(userCreation.PaymentInfo);
            if (state == CreationState.AlreadyExists || state == CreationState.Failed)
                return RegisterState.Success_NoPaymentInfo;

            return RegisterState.Success;
        }

        protected override void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                try
                {
                    if (disposing)
                    {

                    }
                    this.disposed = true;
                }
                finally
                {
                    base.Dispose(disposing);
                }
            }
        }
    }
}
