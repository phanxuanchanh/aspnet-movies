using Data.Context;
using MSSQL;
using MSSQL.Access;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Data.Base
{
    public abstract class GenericDao<TModel>
        where TModel : SqlTableWithTimestamp, new()
    {
        private readonly DBContext _context;
        private readonly Func<DBContext, SqlAccess<TModel>> _sqlAccessFunc;

        public DBContext Context => _context;

        public GenericDao(DBContext context, Func<DBContext, SqlAccess<TModel>> sqlAccessFunc) {
            _context = context;
            _sqlAccessFunc = sqlAccessFunc;
        }

        protected SqlAccess<TModel> GetSqlAccess()
        {
            return _sqlAccessFunc(_context);
        }

        public virtual async Task<TModel> GetAsync(Expression<Func<TModel, bool>> expr)
        {
            return await GetSqlAccess()
                .Where(x => x.DeletedAt == null)
                .Where(expr)
                .FirstOrDefaultAsync();
        }

        public Task<List<TModel>> GetManyAsync(long skip = 1, long take = 10, string searchText = null)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<int> AddAsync(TModel model)
        {
            model.CreatedAt = DateTime.Now;

            return await GetSqlAccess().InsertAsync(model, new List<string> { 
                "Id", "UpdatedAt", "DeletedAt" 
            });
        }

        public virtual async Task<int> UpdateAsync(TModel input, Expression<Func<TModel, bool>> whereExpr, Expression<Func<TModel, object>> selector)
        {
            input.UpdatedAt = DateTime.Now;
            return await GetSqlAccess()
                .Where(whereExpr).UpdateAsync(input, selector);
        }

        public virtual async Task<int> DeleteAsync(Expression<Func<TModel, bool>> expr)
        {
            return await GetSqlAccess().DeleteAsync(expr);
        }
    }
}
