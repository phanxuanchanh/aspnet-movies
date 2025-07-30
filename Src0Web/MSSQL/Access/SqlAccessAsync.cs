using MSSQL.Mapper;
using MSSQL.QueryBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MSSQL.Access
{
    public partial class SqlAccess<T> where T : ISqlTable, new()
    {
        public async Task<List<T>> ToListAsync()
        {
            if (_select.Count == 0)
                _queryBuilder = SqlQueryBuilder<T>.Select();
            else
                _queryBuilder = SqlQueryBuilder<T>.Select(_select[0]);

            foreach (Expression<Func<T, bool>> where in _where)
                _queryBuilder.Where(where);

            List<T> list = await _sqlExecHelper.ExecuteReaderAsync<T>(_queryBuilder, reader => SqlMapper.MapRow<T>(reader));
            return list;
        }

        public async Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> expression = null)
        {
            if (_select.Count == 0)
                _queryBuilder = SqlQueryBuilder<T>.Select();
            else
                _queryBuilder = SqlQueryBuilder<T>.Select(_select[0]);

            foreach (Expression<Func<T, bool>> where in _where)
                _queryBuilder.Where(where);

            if (expression != null)
                _queryBuilder.Where(expression);

            List<T> list = await _sqlExecHelper.ExecuteReaderAsync<T>(_queryBuilder, reader => SqlMapper.MapRow<T>(reader));

            return list.SingleOrDefault();
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> expression = null)
        {
            if(_select.Count == 0)
                _queryBuilder = SqlQueryBuilder<T>.Select();
            else
                _queryBuilder = SqlQueryBuilder<T>.Select(_select[0]);

            
            foreach (Expression<Func<T, bool>> where in _where)
                _queryBuilder.Where(where);

            if (expression != null)
                _queryBuilder.Where(expression);

            List<T> list = await _sqlExecHelper.ExecuteReaderAsync<T>(_queryBuilder, reader => SqlMapper.MapRow<T>(reader));

            return list.FirstOrDefault();
        }

        public async Task<int> InsertAsync(T record, List<string> excludeProperties = null)
        {
            _queryBuilder = SqlQueryBuilder<T>.Insert(record);
            return await _sqlExecHelper.ExecuteNonQueryAsync(_queryBuilder);
        }

        public async Task<int> UpdateAsync(T record, Expression<Func<T, object>> selector)
        {
            _queryBuilder = SqlQueryBuilder<T>.Update(record, selector);

            foreach (Expression<Func<T, bool>> where in _where)
                _queryBuilder.Where(where);

            return await _sqlExecHelper.ExecuteNonQueryAsync(_queryBuilder);
        }


        public async Task<int> DeleteAsync(Expression<Func<T, bool>> expression)
        {
            _queryBuilder = SqlQueryBuilder<T>.Delete();

            foreach (Expression<Func<T, bool>> where in _where)
                _queryBuilder.Where(where);

            _queryBuilder.Where(expression);

            return await _sqlExecHelper.ExecuteNonQueryAsync(_queryBuilder);
        }

        public async Task<long> CountAsync(Expression<Func<T, bool>> expression = null)
        {
            _queryBuilder = SqlQueryBuilder<T>.Count();

            foreach (Expression<Func<T, bool>> where in _where)
                _queryBuilder.Where(where);

            if(expression != null)
                _queryBuilder.Where(expression);

            return await _sqlExecHelper.ExecuteScalarQueryAsync<long>(_queryBuilder);
        }
    }
}
