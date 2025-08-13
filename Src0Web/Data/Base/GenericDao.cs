using Data.DAL;
using MSSQL;
using MSSQL.Access;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;


namespace Data.Base
{
    public abstract class GenericDao<TModel, TId> : IDisposable
        where TModel : SqlTableWithTimestamp, IStandardSqlTable<TId>, new()
    {
        protected readonly DBContext context;
        private readonly Func<DBContext, SqlAccess<TModel>> _sqlAccessFunc;
        private bool disposedValue;

        public GenericDao(Func<DBContext, SqlAccess<TModel>> sqlAccessFunc) { 
            this.context = new DBContext();
            _sqlAccessFunc = sqlAccessFunc;
        }

        protected SqlAccess<TModel> GetSqlAccess()
        {
            return _sqlAccessFunc(context);
        }

        protected abstract Expression<Func<TModel, bool>> SetPkExpr(TId id);
        protected abstract Expression<Func<TModel, object>> SetUpdateSelectorExpr(TModel input);

        public virtual async Task<TModel> GetAsync(TId id)
        {
            Expression<Func<TModel, bool>> getExpr = SetPkExpr(id);

            return await GetSqlAccess()
                .Where(x => x.DeletedAt == null)
                .Where(getExpr)
                .FirstOrDefaultAsync();
        }

        public virtual async Task<int> AddAsync(TModel model)
        {
            model.CreatedAt = DateTime.Now;

            return await GetSqlAccess().InsertAsync(model, new List<string> { 
                "Id", "UpdatedAt", "DeletedAt" 
            });
        }

        public virtual async Task<int> UpdateAsync(TModel input)
        {
            Expression<Func<TModel, bool>> updateExpr = SetPkExpr(input.Id);
            Expression<Func<TModel, object>> updateSelector = SetUpdateSelectorExpr(input);

            input.UpdatedAt = DateTime.Now;
            return await GetSqlAccess()
                .Where(updateExpr).UpdateAsync(input, updateSelector);
        }

        public virtual async Task<int> DeleteAsync(TId id)
        {
            Expression<Func<TModel, bool>> pkExpr = SetPkExpr(id);

            return await GetSqlAccess().DeleteAsync(pkExpr);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    
                }
                context.Dispose();
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
