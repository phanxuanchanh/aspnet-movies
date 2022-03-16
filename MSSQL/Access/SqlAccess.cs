using MSSQL.Query;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq.Expressions;

namespace MSSQL.Access
{
    public partial class SqlAccess<T>
    {
        private SqlQuery sqlQuery;
        private SqlData sqlData;

        internal SqlAccess(SqlData sqlData)
        {
            sqlQuery = new SqlQuery();
            this.sqlData = sqlData;
        }

        public List<T> ToList()
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>())
            {
                return sqlData.ToList<T>(sqlCommand);
            }
        }

        public List<T> ToList(long skip, long take)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(skip, take))
            {
                return sqlData.ToList<T>(sqlCommand);
            }
        }

        public List<T> ToList(Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(orderBy, sqlOrderByOptions))
            {
                return sqlData.ToList<T>(sqlCommand);
            }
        }

        public List<T> ToList(Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions, long skip, long take)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(orderBy, sqlOrderByOptions, skip, take))
            {
                return sqlData.ToList<T>(sqlCommand);
            }
        }

        public List<T> ToList(int top)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(top))
            {
                return sqlData.ToList<T>(sqlCommand);
            }
        }

        public List<T> ToList(int top, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(top, orderBy, sqlOrderByOptions))
            {
                return sqlData.ToList<T>(sqlCommand);
            }
        }

        public List<T> ToList(Expression<Func<T, bool>> where)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(where))
            {
                return sqlData.ToList<T>(sqlCommand);
            }
        }

        public List<T> ToList(Expression<Func<T, bool>> where, long skip, long take)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(where, skip, take))
            {
                return sqlData.ToList<T>(sqlCommand);
            }
        }

        public List<T> ToList(Expression<Func<T, bool>> where, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(where, orderBy, sqlOrderByOptions))
            {
                return sqlData.ToList<T>(sqlCommand);
            }
        }

        public List<T> ToList(Expression<Func<T, bool>> where, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions, long skip, long take)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(where, orderBy, sqlOrderByOptions, skip, take))
            {
                return sqlData.ToList<T>(sqlCommand);
            }
        }

        public List<T> ToList(Expression<Func<T, bool>> where, int top)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(where, top))
            {
                return sqlData.ToList<T>(sqlCommand);
            }
        }

        public List<T> ToList(Expression<Func<T, bool>> where, int top, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(where, top, orderBy, sqlOrderByOptions))
            {
                return sqlData.ToList<T>(sqlCommand);
            }
        }

        public List<T> ToList(Expression<Func<T, object>> set)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(set))
            {
                return sqlData.ToList<T>(sqlCommand);
            }
        }

        public List<T> ToList(Expression<Func<T, object>> select, long skip, long take)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(select, skip, take))
            {
                return sqlData.ToList<T>(sqlCommand);
            }
        }

        public List<T> ToList(Expression<Func<T, object>> select, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOption)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(select, orderBy, sqlOrderByOption))
            {
                return sqlData.ToList<T>(sqlCommand);
            }
        }

        public List<T> ToList(Expression<Func<T, object>> select, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOption, long skip, long take)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(select, orderBy, sqlOrderByOption, skip, take))
            {
                return sqlData.ToList<T>(sqlCommand);
            }
        }

        public List<T> ToList(Expression<Func<T, object>> select, int top)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(select, top))
            {
                return sqlData.ToList<T>(sqlCommand);
            }
        }

        public List<T> ToList(Expression<Func<T, object>> select, int top, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOption)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(select, top, orderBy, sqlOrderByOption))
            {
                return sqlData.ToList<T>(sqlCommand);
            }
        }

        public List<T> ToList(Expression<Func<T, object>> select, Expression<Func<T, bool>> where)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(select, where))
            {
                return sqlData.ToList<T>(sqlCommand);
            }
        }

        public List<T> ToList(Expression<Func<T, object>> select, Expression<Func<T, bool>> where, long skip, long take)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(select, where, skip, take))
            {
                return sqlData.ToList<T>(sqlCommand);
            }
        }

        public List<T> ToList(Expression<Func<T, object>> select, Expression<Func<T, bool>> where, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(select, where, orderBy, sqlOrderByOptions))
            {
                return sqlData.ToList<T>(sqlCommand);
            }
        }

        public List<T> ToList(Expression<Func<T, object>> select, Expression<Func<T, bool>> where, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions, long skip, long take)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(select, where, orderBy, sqlOrderByOptions, skip, take))
            {
                return sqlData.ToList<T>(sqlCommand);
            }
        }

        public List<T> ToList(Expression<Func<T, object>> select, Expression<Func<T, bool>> where, int top)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(select, where, top))
            {
                return sqlData.ToList<T>(sqlCommand);
            }
        }

        public List<T> ToList(Expression<Func<T, object>> select, Expression<Func<T, bool>> where, int top, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(select, where, top, orderBy, sqlOrderByOptions))
            {
                return sqlData.ToList<T>(sqlCommand);
            }
        }

        public SqlPagedList<T> ToPagedList(long pageIndex, long pageSize)
        {
            long totalRecord = Count();
            SqlPagedList<T> pagedList = new SqlPagedList<T>();
            pagedList.Solve(totalRecord, pageIndex, pageSize);
            pagedList.Items = ToList(pagedList.Skip, pagedList.Take);
            return pagedList;
        }

        public SqlPagedList<T> ToPagedList(Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions, long pageIndex, long pageSize)
        {
            long totalRecord = Count();
            SqlPagedList<T> pagedList = new SqlPagedList<T>();
            pagedList.Solve(totalRecord, pageIndex, pageSize);
            pagedList.Items = ToList(orderBy, sqlOrderByOptions, pagedList.Skip, pagedList.Take);
            return pagedList;
        }

        public SqlPagedList<T> ToPagedList(Expression<Func<T, object>> select, long pageIndex, long pageSize)
        {
            long totalRecord = Count();
            SqlPagedList<T> pagedList = new SqlPagedList<T>();
            pagedList.Solve(totalRecord, pageIndex, pageSize);
            pagedList.Items = ToList(select, pagedList.Skip, pagedList.Take);
            return pagedList;
        }

        public SqlPagedList<T> ToPagedList(Expression<Func<T, object>> select, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions, long pageIndex, long pageSize)
        {
            long totalRecord = Count();
            SqlPagedList<T> pagedList = new SqlPagedList<T>();
            pagedList.Solve(totalRecord, pageIndex, pageSize);
            pagedList.Items = ToList(select, orderBy, sqlOrderByOptions, pagedList.Skip, pagedList.Take);
            return pagedList;
        }

        public SqlPagedList<T> ToPagedList(Expression<Func<T, bool>> where, long pageIndex, long pageSize)
        {
            long totalRecord = Count(where);
            SqlPagedList<T> pagedList = new SqlPagedList<T>();
            pagedList.Solve(totalRecord, pageIndex, pageSize);
            pagedList.Items = ToList(where, pagedList.Skip, pagedList.Take);
            return pagedList;
        }

        public SqlPagedList<T> ToPagedList(Expression<Func<T, bool>> where, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions, long pageIndex, long pageSize)
        {
            long totalRecord = Count(where);
            SqlPagedList<T> pagedList = new SqlPagedList<T>();
            pagedList.Solve(totalRecord, pageIndex, pageSize);
            pagedList.Items = ToList(where, orderBy, sqlOrderByOptions, pagedList.Skip, pagedList.Take);
            return pagedList;
        }

        public SqlPagedList<T> ToPagedList(Expression<Func<T, object>> select, Expression<Func<T, bool>> where, long pageIndex, long pageSize)
        {
            long totalRecord = Count(where);
            SqlPagedList<T> pagedList = new SqlPagedList<T>();
            pagedList.Solve(totalRecord, pageIndex, pageSize);
            pagedList.Items = ToList(select, where, pagedList.Skip, pagedList.Take);
            return pagedList;
        }

        public SqlPagedList<T> ToPagedList(Expression<Func<T, object>> select, Expression<Func<T, bool>> where, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions, long pageIndex, long pageSize)
        {
            long totalRecord = Count(where);
            SqlPagedList<T> pagedList = new SqlPagedList<T>();
            pagedList.Solve(totalRecord, pageIndex, pageSize);
            pagedList.Items = ToList(select, where, orderBy, sqlOrderByOptions, pagedList.Skip, pagedList.Take);
            return pagedList;
        }

        public T SingleOrDefault()
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(1))
            {
                return sqlData.To<T>(sqlCommand);
            }
        }

        public T SingleOrDefault(Expression<Func<T, bool>> where)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(where, 1))
            {
                return sqlData.To<T>(sqlCommand);
            }
        }

        public T SingleOrDefault(Expression<Func<T, object>> set)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(set, 1))
            {
                return sqlData.To<T>(sqlCommand);
            }
        }

        public T SingleOrDefault(Expression<Func<T, object>> set, Expression<Func<T, bool>> where)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(set, where, 1))
            {
                return sqlData.To<T>(sqlCommand);
            }
        }

        public T FirstOrDefault()
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(1))
            {
                return sqlData.To<T>(sqlCommand);
            }
        }

        public T FirstOrDefault(Expression<Func<T, bool>> where)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(where, 1))
            {
                return sqlData.To<T>(sqlCommand);
            }
        }

        public T FirstOrDefault(Expression<Func<T, object>> set)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(set, 1))
            {
                return sqlData.To<T>(sqlCommand);
            }
        }

        public T FirstOrDefault(Expression<Func<T, object>> set, Expression<Func<T, bool>> where)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(set, where, 1))
            {
                return sqlData.To<T>(sqlCommand);
            }
        }

        public int Delete()
        {
            using(SqlCommand sqlCommand = sqlQuery.Delete<T>())
            {
                return sqlData.ExecuteNonQuery(sqlCommand);
            }
        }

        public int Delete(Expression<Func<T, bool>> where)
        {
            using(SqlCommand sqlCommand = sqlQuery.Delete<T>(where))
            {
                return sqlData.ExecuteNonQuery(sqlCommand);
            }
        }

        public int Update(T model, Expression<Func<T, object>> set)
        {
            using(SqlCommand sqlCommand = sqlQuery.Update<T>(model, set))
            {
                return sqlData.ExecuteNonQuery(sqlCommand);
            }
        }

        public int Update(T model, Expression<Func<T, object>> set, Expression<Func<T, bool>> where)
        {
            using(SqlCommand sqlCommand = sqlQuery.Update<T>(model, set, where))
            {
                return sqlData.ExecuteNonQuery(sqlCommand);
            }
        }

        public int Insert(T model)
        {
            using(SqlCommand sqlCommand = sqlQuery.Insert<T>(model))
            {
                return sqlData.ExecuteNonQuery(sqlCommand);
            }
        }

        public int Insert(T model, List<string> excludeProperties)
        {
            using(SqlCommand sqlCommand = sqlQuery.Insert<T>(model, excludeProperties))
            {
                return sqlData.ExecuteNonQuery(sqlCommand);
            }
        }

        public long Count()
        {
            using (SqlCommand sqlCommand = sqlQuery.Count<T>())
            {
                return long.Parse((string)sqlData.ExecuteScalar(sqlCommand));
            }
        }

        public long Count(Expression<Func<T, bool>> where)
        {
            using (SqlCommand sqlCommand = sqlQuery.Count<T>(where))
            {
                return long.Parse((string)sqlData.ExecuteScalar(sqlCommand));
            }
        }

        public long Count(string propertyName, Expression<Func<T, bool>> where)
        {
            using(SqlCommand sqlCommand = sqlQuery.Count<T>(propertyName, where))
            {
                return long.Parse((string)sqlData.ExecuteScalar(sqlCommand));
            }
        }
    }
}
