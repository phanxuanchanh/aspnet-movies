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
    public class UserDao
    {
        private DBContext _context;

        public UserDao(DBContext context)
        {
            _context = context;
        }

        public async Task<SqlPagedList<User>> GetsAsync(long pageIndex, long pageSize)
        {
            SqlPagedList<User> pagedList = null;
            Expression<Func<User, object>> orderBy = c => new { c.Id };

            pagedList = await _context.Users.ToPagedListAsync(orderBy, SqlOrderByOptions.Desc, pageIndex, pageSize);

            return pagedList;
        }

        public async Task<User> GetAsync(string userId)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
        }

        public async Task<User> GetByUserNameAsync(string userName)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.UserName == userName);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        public enum LoginState { Success, Unconfirmed, WrongPassword, NotExists };


        //public async Task<bool> IsActiveAsync(string username)
        //{
        //    return (await db.Users
        //        .SingleOrDefaultAsync(u => new { u.activated }, u => u.userName == username)).activated;
        //}

        //public enum ActiveUserState { Success, Failed, NotExists }

        //public async Task<ActiveUserState> ActiveUserAsync(string userId)
        //{
        //    if (string.IsNullOrEmpty(userId))
        //        throw new Exception("@'userId' must not be null or empty");

        //    long count = await db.Users.CountAsync(u => u.ID == userId);
        //    if (count == 0)
        //        return ActiveUserState.NotExists;

        //    int affected = await db.Users.UpdateAsync(new User
        //    {
        //        activated = true,
        //        updateAt = DateTime.Now
        //    }, u => new { u.activated, u.updateAt }, u => u.ID == userId);

        //    return (affected == 0) ? ActiveUserState.Failed : ActiveUserState.Success;
        //}

        public async Task<int> InsertAsync(User user)
        {
            user.CreatedAt = DateTime.Now;
            return await _context.Users.InsertAsync(user, new List<string> { "Id", "UpdatedAt", "DeletedAt" });
        }

        public async Task<int> UpdateAsync(User user)
        {
            user.UpdatedAt = DateTime.Now;
            return await _context.Users
                .Where(x => x.Id == user.Id)
                .UpdateAsync(user, s => new { });
        }
    }
}
