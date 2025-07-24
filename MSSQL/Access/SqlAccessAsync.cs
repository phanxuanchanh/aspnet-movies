using MSSQL.Mapper;
using MSSQL.Query;
using MSSQL.QueryBuilder;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MSSQL.Access
{
    public partial class SqlAccess<T> where T : ISqlTable, new()
    {
        public async Task<List<T>> ToListAsync()
        {
            using (SqlCommand sqlCommand = sqlQuery.Select<T>())
            {
                return await sqlData.ToListAsync<T>(sqlCommand);
            }
        }

        public async Task<List<T>> ToListAsync(long skip, long take)
        {
            using (SqlCommand sqlCommand = sqlQuery.Select<T>(skip, take))
            {
                return await sqlData.ToListAsync<T>(sqlCommand);
            }
        }

        public async Task<List<T>> ToListAsync(Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions)
        {
            using (SqlCommand sqlCommand = sqlQuery.Select<T>(orderBy, sqlOrderByOptions))
            {
                return await sqlData.ToListAsync<T>(sqlCommand);
            }
        }

        public async Task<List<T>> ToListAsync(Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions, long skip, long take)
        {
            using (SqlCommand sqlCommand = sqlQuery.Select<T>(orderBy, sqlOrderByOptions, skip, take))
            {
                return await sqlData.ToListAsync<T>(sqlCommand);
            }
        }

        public async Task<List<T>> ToListAsync(int top)
        {
            using (SqlCommand sqlCommand = sqlQuery.Select<T>(top))
            {
                return await sqlData.ToListAsync<T>(sqlCommand);
            }
        }

        public async Task<List<T>> ToListAsync(int top, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions)
        {
            using (SqlCommand sqlCommand = sqlQuery.Select<T>(top, orderBy, sqlOrderByOptions))
            {
                return await sqlData.ToListAsync<T>(sqlCommand);
            }
        }

        public async Task<List<T>> ToListAsync(Expression<Func<T, bool>> where)
        {
            using (SqlCommand sqlCommand = sqlQuery.Select<T>(where))
            {
                return await sqlData.ToListAsync<T>(sqlCommand);
            }
        }

        public async Task<List<T>> ToListAsync(Expression<Func<T, bool>> where, long skip, long take)
        {
            using (SqlCommand sqlCommand = sqlQuery.Select<T>(where, skip, take))
            {
                return await sqlData.ToListAsync<T>(sqlCommand);
            }
        }

        public async Task<List<T>> ToListAsync(Expression<Func<T, bool>> where, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions)
        {
            using (SqlCommand sqlCommand = sqlQuery.Select<T>(where, orderBy, sqlOrderByOptions))
            {
                return await sqlData.ToListAsync<T>(sqlCommand);
            }
        }

        public async Task<List<T>> ToListAsync(Expression<Func<T, bool>> where, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions, long skip, long take)
        {
            using (SqlCommand sqlCommand = sqlQuery.Select<T>(where, orderBy, sqlOrderByOptions, skip, take))
            {
                return await sqlData.ToListAsync<T>(sqlCommand);
            }
        }

        public async Task<List<T>> ToListAsync(Expression<Func<T, bool>> where, int top)
        {
            using (SqlCommand sqlCommand = sqlQuery.Select<T>(where, top))
            {
                return await sqlData.ToListAsync<T>(sqlCommand);
            }
        }

        public async Task<List<T>> ToListAsync(Expression<Func<T, bool>> where, int top, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions)
        {
            using (SqlCommand sqlCommand = sqlQuery.Select<T>(where, top, orderBy, sqlOrderByOptions))
            {
                return await sqlData.ToListAsync<T>(sqlCommand);
            }
        }

        public async Task<List<T>> ToListAsync(Expression<Func<T, object>> set)
        {
            using (SqlCommand sqlCommand = sqlQuery.Select<T>(set))
            {
                return await sqlData.ToListAsync<T>(sqlCommand);
            }
        }

        public async Task<List<T>> ToListAsync(Expression<Func<T, object>> set, long skip, long take)
        {
            using (SqlCommand sqlCommand = sqlQuery.Select<T>(set, skip, take))
            {
                return await sqlData.ToListAsync<T>(sqlCommand);
            }
        }

        public async Task<List<T>> ToListAsync(Expression<Func<T, object>> set, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOption)
        {
            using (SqlCommand sqlCommand = sqlQuery.Select<T>(set, orderBy, sqlOrderByOption))
            {
                return await sqlData.ToListAsync<T>(sqlCommand);
            }
        }

        public async Task<List<T>> ToListAsync(Expression<Func<T, object>> set, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOption, long skip, long take)
        {
            using (SqlCommand sqlCommand = sqlQuery.Select<T>(set, orderBy, sqlOrderByOption, skip, take))
            {
                return await sqlData.ToListAsync<T>(sqlCommand);
            }
        }

        public async Task<List<T>> ToListAsync(Expression<Func<T, object>> set, int top)
        {
            using (SqlCommand sqlCommand = sqlQuery.Select<T>(set, top))
            {
                return await sqlData.ToListAsync<T>(sqlCommand);
            }
        }

        public async Task<List<T>> ToListAsync(Expression<Func<T, object>> set, int top, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOption)
        {
            using (SqlCommand sqlCommand = sqlQuery.Select<T>(set, top, orderBy, sqlOrderByOption))
            {
                return await sqlData.ToListAsync<T>(sqlCommand);
            }
        }

        public async Task<List<T>> ToListAsync(Expression<Func<T, object>> set, Expression<Func<T, bool>> where)
        {
            using (SqlCommand sqlCommand = sqlQuery.Select<T>(set, where))
            {
                return await sqlData.ToListAsync<T>(sqlCommand);
            }
        }

        public async Task<List<T>> ToListAsync(Expression<Func<T, object>> set, Expression<Func<T, bool>> where, long skip, long take)
        {
            using (SqlCommand sqlCommand = sqlQuery.Select<T>(set, where, skip, take))
            {
                return await sqlData.ToListAsync<T>(sqlCommand);
            }
        }

        public async Task<List<T>> ToListAsync(Expression<Func<T, object>> set, Expression<Func<T, bool>> where, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions)
        {
            using (SqlCommand sqlCommand = sqlQuery.Select<T>(set, where, orderBy, sqlOrderByOptions))
            {
                return await sqlData.ToListAsync<T>(sqlCommand);
            }
        }

        public async Task<List<T>> ToListAsync(Expression<Func<T, object>> set, Expression<Func<T, bool>> where, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions, long skip, long take)
        {
            using (SqlCommand sqlCommand = sqlQuery.Select<T>(set, where, orderBy, sqlOrderByOptions, skip, take))
            {
                return await sqlData.ToListAsync<T>(sqlCommand);
            }
        }

        public async Task<List<T>> ToListAsync(Expression<Func<T, object>> set, Expression<Func<T, bool>> where, int top)
        {
            using (SqlCommand sqlCommand = sqlQuery.Select<T>(set, where, top))
            {
                return await sqlData.ToListAsync<T>(sqlCommand);
            }
        }

        public async Task<List<T>> ToListAsync(Expression<Func<T, object>> set, Expression<Func<T, bool>> where, int top, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions)
        {
            using (SqlCommand sqlCommand = sqlQuery.Select<T>(set, where, top, orderBy, sqlOrderByOptions))
            {
                return await sqlData.ToListAsync<T>(sqlCommand);
            }
        }

        public async Task<SqlPagedList<T>> ToPagedListAsync(long pageIndex, long pageSize)
        {
            long totalRecord = await CountAsync();
            SqlPagedList<T> pagedList = new SqlPagedList<T>();
            pagedList.Solve(totalRecord, pageIndex, pageSize);
            pagedList.Items = await ToListAsync(pagedList.Skip, pagedList.Take);
            return pagedList;
        }

        public async Task<SqlPagedList<T>> ToPagedListAsync(Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions, long pageIndex, long pageSize)
        {
            long totalRecord = await CountAsync();
            SqlPagedList<T> pagedList = new SqlPagedList<T>();
            pagedList.Solve(totalRecord, pageIndex, pageSize);
            pagedList.Items = await ToListAsync(orderBy, sqlOrderByOptions, pagedList.Skip, pagedList.Take);
            return pagedList;
        }

        public async Task<SqlPagedList<T>> ToPagedListAsync(Expression<Func<T, object>> select, long pageIndex, long pageSize)
        {
            long totalRecord = await CountAsync();
            SqlPagedList<T> pagedList = new SqlPagedList<T>();
            pagedList.Solve(totalRecord, pageIndex, pageSize);
            pagedList.Items = await ToListAsync(select, pagedList.Skip, pagedList.Take);
            return pagedList;
        }

        public async Task<SqlPagedList<T>> ToPagedListAsync(Expression<Func<T, object>> select, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions, long pageIndex, long pageSize)
        {
            long totalRecord = await CountAsync();
            SqlPagedList<T> pagedList = new SqlPagedList<T>();
            pagedList.Solve(totalRecord, pageIndex, pageSize);
            pagedList.Items = await ToListAsync(select, orderBy, sqlOrderByOptions, pagedList.Skip, pagedList.Take);
            return pagedList;
        }

        public async Task<SqlPagedList<T>> ToPagedListAsync(Expression<Func<T, bool>> where, long pageIndex, long pageSize)
        {
            long totalRecord = await CountAsync(where);
            SqlPagedList<T> pagedList = new SqlPagedList<T>();
            pagedList.Solve(totalRecord, pageIndex, pageSize);
            pagedList.Items = await ToListAsync(where, pagedList.Skip, pagedList.Take);
            return pagedList;
        }

        public async Task<SqlPagedList<T>> ToPagedListAsync(Expression<Func<T, bool>> where, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions, long pageIndex, long pageSize)
        {
            long totalRecord = await CountAsync(where);
            SqlPagedList<T> pagedList = new SqlPagedList<T>();
            pagedList.Solve(totalRecord, pageIndex, pageSize);
            pagedList.Items = await ToListAsync(where, orderBy, sqlOrderByOptions, pagedList.Skip, pagedList.Take);
            return pagedList;
        }

        public async Task<SqlPagedList<T>> ToPagedListAsync(Expression<Func<T, object>> select, Expression<Func<T, bool>> where, long pageIndex, long pageSize)
        {
            long totalRecord = await CountAsync(where);
            SqlPagedList<T> pagedList = new SqlPagedList<T>();
            pagedList.Solve(totalRecord, pageIndex, pageSize);
            pagedList.Items = await ToListAsync(select, where, pagedList.Skip, pagedList.Take);
            return pagedList;
        }

        public async Task<SqlPagedList<T>> ToPagedListAsync(Expression<Func<T, object>> select, Expression<Func<T, bool>> where, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions, long pageIndex, long pageSize)
        {
            long totalRecord = await CountAsync(where);
            SqlPagedList<T> pagedList = new SqlPagedList<T>();
            pagedList.Solve(totalRecord, pageIndex, pageSize);
            pagedList.Items = await ToListAsync(select, where, orderBy, sqlOrderByOptions, pagedList.Skip, pagedList.Take);
            return pagedList;
        }

        public async Task<T> SingleOrDefaultAsync()
        {
            using (SqlCommand sqlCommand = sqlQuery.Select<T>(1))
            {
                return await sqlData.ToAsync<T>(sqlCommand);
            }
        }

        public async Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> where)
        {
            using (SqlCommand sqlCommand = sqlQuery.Select<T>(where, 1))
            {
                return await sqlData.ToAsync<T>(sqlCommand);
            }
        }

        public async Task<T> SingleOrDefaultAsync(Expression<Func<T, object>> set)
        {
            using (SqlCommand sqlCommand = sqlQuery.Select<T>(set, 1))
            {
                return await sqlData.ToAsync<T>(sqlCommand);
            }
        }

        public async Task<T> SingleOrDefaultAsync(Expression<Func<T, object>> set, Expression<Func<T, bool>> where)
        {
            using (SqlCommand sqlCommand = sqlQuery.Select<T>(set, where, 1))
            {
                return await sqlData.ToAsync<T>(sqlCommand);
            }
        }

        public async Task<T> FirstOrDefaultAsync()
        {
            using (SqlCommand sqlCommand = sqlQuery.Select<T>(1))
            {
                return await sqlData.ToAsync<T>(sqlCommand);
            }
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> where)
        {
            using (SqlCommand sqlCommand = sqlQuery.Select<T>(where, 1))
            {
                return await sqlData.ToAsync<T>(sqlCommand);
            }
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, object>> set)
        {
            using (SqlCommand sqlCommand = sqlQuery.Select<T>(set, 1))
            {
                return await sqlData.ToAsync<T>(sqlCommand);
            }
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, object>> set, Expression<Func<T, bool>> where)
        {
            using (SqlCommand sqlCommand = sqlQuery.Select<T>(set, where, 1))
            {
                return await sqlData.ToAsync<T>(sqlCommand);
            }
        }


        public async Task<List<T>> nToListAsync(Expression<Func<T, object>> selector)
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

        public async Task<T> nSingleOrDefaultAsync(Expression<Func<T, object>> selector = null)
        {
            if (_select.Count == 0)
                _queryBuilder = SqlQueryBuilder<T>.Select();
            else
                _queryBuilder = SqlQueryBuilder<T>.Select(_select[0]);

            foreach (Expression<Func<T, bool>> where in _where)
                _queryBuilder.Where(where);

            List<T> list = await _sqlExecHelper.ExecuteReaderAsync<T>(_queryBuilder, reader => SqlMapper.MapRow<T>(reader));

            return list.SingleOrDefault();
        }

        public async Task<T> nFirstOrDefaultAsync(Expression<Func<T, bool>> expression = null)
        {
            if(_select.Count == 0)
                _queryBuilder = SqlQueryBuilder<T>.Select();
            else
                _queryBuilder = SqlQueryBuilder<T>.Select(_select[0]);

            
            foreach (Expression<Func<T, bool>> where in _where)
                _queryBuilder.Where(where);

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
