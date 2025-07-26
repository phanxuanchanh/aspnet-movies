using MSSQL.Mapper;
using MSSQL.Query;
using MSSQL.QueryBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MSSQL.Access
{
    public partial class SqlAccess<T>
    {
        private SqlExecHelper _sqlExecHelper;
        private SqlQueryBuilder<T> _queryBuilder;

        private List<Expression<Func<T, bool>>> _where;
        private List<Expression<Func<T, object>>> _select;
        private List<Expression<Func<T, object>>> _orderBy;

        internal SqlAccess(SqlData sqlData, SqlExecHelper sqlExecHelper)
        {
            _sqlExecHelper = sqlExecHelper;
            _queryBuilder = null;

            _where = new List<Expression<Func<T, bool>>>();
            _select = new List<Expression<Func<T, object>>>();
            _orderBy = new List<Expression<Func<T, object>>>();
        }

        public SqlAccess<T> Where(Expression<Func<T, bool>> expression)
        {
            _where.Add(expression);
            return this;
        }

        public SqlAccess<T> Select(Expression<Func<T, object>> selector)
        {
            _select.Add(selector);
            return this;
        }

        public SqlAccess<T> OrderBy(Expression<Func<T, object>> orderBy)
        {
            _orderBy.Add(orderBy);
            return this;
        }

        public int Insert(T record, List<string> excludeProperties = null)
        {
            _queryBuilder = SqlQueryBuilder<T>.Insert(record);
            return _sqlExecHelper.ExecuteNonQuery(_queryBuilder);
        }

        public int Update(T record, Expression<Func<T, object>> selector)
        {
            _queryBuilder = SqlQueryBuilder<T>.Update(record, selector);

            foreach (Expression<Func<T, bool>> where in _where)
                _queryBuilder.Where(where);

            return _sqlExecHelper.ExecuteNonQuery(_queryBuilder);
        }


        public int Delete(Expression<Func<T, bool>> expression)
        {
            _queryBuilder = SqlQueryBuilder<T>.Delete();

            foreach (Expression<Func<T, bool>> where in _where)
                _queryBuilder.Where(where);

            _queryBuilder.Where(expression);

            return _sqlExecHelper.ExecuteNonQuery(_queryBuilder);
        }

        public long Count(Expression<Func<T, bool>> expression = null)
        {
            _queryBuilder = SqlQueryBuilder<T>.Count();

            foreach (Expression<Func<T, bool>> where in _where)
                _queryBuilder.Where(where);

            if (expression != null)
                _queryBuilder.Where(expression);

            return _sqlExecHelper.ExecuteScalarQuery<long>(_queryBuilder);
        }

        public List<T> ToList()
        {
            if (_select.Count == 0)
                _queryBuilder = SqlQueryBuilder<T>.Select();
            else
                _queryBuilder = SqlQueryBuilder<T>.Select(_select[0]);

            foreach (Expression<Func<T, bool>> where in _where)
                _queryBuilder.Where(where);

            IEnumerable<T> enumrable = _sqlExecHelper.ExecuteReader<T>(_queryBuilder, reader => SqlMapper.MapRow<T>(reader));
            return enumrable.ToList();
        }

        public T SingleOrDefault(Expression<Func<T, bool>> expression = null)
        {
            if (_select.Count == 0)
                _queryBuilder = SqlQueryBuilder<T>.Select();
            else
                _queryBuilder = SqlQueryBuilder<T>.Select(_select[0]);

            foreach (Expression<Func<T, bool>> where in _where)
                _queryBuilder.Where(where);

            if (expression != null)
                _queryBuilder.Where(expression);

            IEnumerable<T> enumrable = _sqlExecHelper.ExecuteReader<T>(_queryBuilder, reader => SqlMapper.MapRow<T>(reader));

            return enumrable.SingleOrDefault();
        }

        public T FirstOrDefault(Expression<Func<T, bool>> expression = null)
        {
            if (_select.Count == 0)
                _queryBuilder = SqlQueryBuilder<T>.Select();
            else
                _queryBuilder = SqlQueryBuilder<T>.Select(_select[0]);


            foreach (Expression<Func<T, bool>> where in _where)
                _queryBuilder.Where(where);

            if (expression != null)
                _queryBuilder.Where(expression);

            IEnumerable<T> enumrable = _sqlExecHelper.ExecuteReader<T>(_queryBuilder, reader => SqlMapper.MapRow<T>(reader));

            return enumrable.FirstOrDefault();
        }

        public SqlPagedList<T> ToPagedList(Expression<Func<T, bool>> where, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions, long pageIndex, long pageSize)
        {
            long totalRecord = 0;// Count(where);
            SqlPagedList<T> pagedList = new SqlPagedList<T>();
            pagedList.Solve(totalRecord, pageIndex, pageSize);
            pagedList.Items = new List<T>(); //ToList(where, orderBy, sqlOrderByOptions, pagedList.Skip, pagedList.Take);
            return pagedList;
        }
    }
}
