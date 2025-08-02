using MSSQL.Connection;
using System;

namespace MSSQL.Access
{
    public partial class SqlContext : IDisposable
    {
        private bool disposedValue;
        private SqlExecHelper _sqlExecHelper;

        public SqlContext()
        {
            _sqlExecHelper = new SqlExecHelper(SqlConnectInfo.GetConnectionString());
            _sqlExecHelper.Connect();
            disposedValue = false;
        }

        protected SqlAccess<T> InitSqlAccess<T>(ref SqlAccess<T> sqlAccess) where T : ISqlTable, new()
        {
            if (sqlAccess == null)
                sqlAccess = new SqlAccess<T>(_sqlExecHelper);
            else
                sqlAccess.Reset();

            return sqlAccess;
        }

        protected void DisposeSqlAccess<T>(ref SqlAccess<T> sqlAccess) where T : ISqlTable, new()
        {
            if (sqlAccess != null)
                sqlAccess = null;
        }

        public SqlExecHelper GetHelper()
        {
            return _sqlExecHelper;
        }

        public void BeginTransaction()
        {
             _sqlExecHelper.BeginTransaction();
        }

        public void CommitTransaction()
        {
            _sqlExecHelper.CommitTransaction();
        }

        public void RollbackTransaction()
        {
            _sqlExecHelper.RollbackTransaction();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    
                }

                _sqlExecHelper.Dispose();
                disposedValue = true;
            }
        }

        ~SqlContext()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
