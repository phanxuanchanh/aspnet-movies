using MSSQL.QueryBuilder;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

namespace MSSQL
{
    public partial class SqlExecHelper : IDisposable
    {
        private readonly string _connectionString;
        private readonly SqlConnection _connection;
        private SqlTransaction _transaction;
        private bool disposedValue;

        public SqlExecHelper(string connectionString)
        {
            _connectionString = connectionString;
            _connection = new SqlConnection(_connectionString);
            _transaction = null;
        }

        public void Connect()
        {
            if (_connection != null && _connection.State != ConnectionState.Open)
                _connection.Open();
        }

        public void Disconnect()
        {
            if (_connection != null && _connection.State != ConnectionState.Closed)
                _connection.Close();
        }

        public void BeginTransaction()
        {
            if (_connection.State != ConnectionState.Open)
                _connection.Open();

            _transaction = _connection.BeginTransaction();
        }

        public void CommitTransaction()
        {
            if (_transaction == null)
                throw new InvalidOperationException("No transaction is currently active.");

            try
            {
                _transaction.Commit();
            }
            catch (Exception ex)
            {
                _transaction.Rollback();
                throw new Exception("Transaction commit failed.", ex);
            }
            finally
            {
                _transaction.Dispose();
                _transaction = null;
            }
        }

        public void RollbackTransaction()
        {
            if (_transaction == null)
                throw new InvalidOperationException("No transaction is currently active.");

            try
            {
                _transaction.Rollback();
            }
            catch (Exception ex)
            {
                throw new Exception("Transaction rollback failed.", ex);
            }
            finally
            {
                _transaction.Dispose();
                _transaction = null;
            }
        }

        public IEnumerable<T> ExecuteReader<T>(string sql, Dictionary<string, object> parameters, Func<SqlDataReader, T> mapper)
        {
            using (SqlCommand cmd = new SqlCommand(sql, _connection))
            {
                cmd.CommandType = CommandType.Text;

                if (parameters == null)
                    parameters = new Dictionary<string, object>();

                foreach (KeyValuePair<string, object> pair in parameters)
                    cmd.Parameters.AddWithValue(pair.Key, pair.Value);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        yield return mapper(reader);
                }
            };
        }

        public Tscalar ExecuteScalarQuery<Tscalar>(string sql, Dictionary<string, object> parameters = null)
        {
            using (SqlCommand cmd = new SqlCommand(sql, _connection))
            {
                cmd.CommandType = CommandType.Text;

                if (parameters == null)
                    parameters = new Dictionary<string, object>();

                foreach (KeyValuePair<string, object> pair in parameters)
                    cmd.Parameters.AddWithValue(pair.Key, pair.Value);

                object val = cmd.ExecuteScalar();

                return val is null ? default : (Tscalar)Convert.ChangeType(val, typeof(Tscalar));
            }
        }

        public int ExecuteNonQuery(string sql, Dictionary<string, object> parameters = null)
        {
            using (SqlCommand cmd = new SqlCommand(sql, _connection))
            {
                cmd.CommandType = CommandType.Text;

                if (parameters == null)
                    parameters = new Dictionary<string, object>();

                foreach (KeyValuePair<string, object> pair in parameters)
                    cmd.Parameters.AddWithValue(pair.Key, pair.Value);

                return cmd.ExecuteNonQuery();
            }
        }

        public DataTable GetDataTable(string sql)
        {
            using (SqlCommand cmd = new SqlCommand(sql, _connection))
            {
                cmd.CommandType = CommandType.Text;

                using (SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd))
                {
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    return dataTable;
                }
            }
        }

        public IEnumerable<T> ExecuteReader<T>(SqlQueryBuilderBase builder, Func<SqlDataReader, T> mapper)
        {
            using (SqlCommand cmd = new SqlCommand(builder.BuildQuery(), _connection))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddRange(builder.GetParameters().ToArray());

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        yield return mapper(reader);
                }
            }
        }

        public Tscalar ExecuteScalarQuery<Tscalar>(SqlQueryBuilderBase builder)
        {
            using (SqlCommand cmd = new SqlCommand(builder.BuildQuery(), _connection))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddRange(builder.GetParameters().ToArray());

                object val = cmd.ExecuteScalar();

                return val is null ? default : (Tscalar)Convert.ChangeType(val, typeof(Tscalar));
            }
        }

        public int ExecuteNonQuery(SqlQueryBuilderBase builder)
        {
            using (SqlCommand cmd = new SqlCommand(builder.BuildQuery(), _connection))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddRange(builder.GetParameters().ToArray());

                return cmd.ExecuteNonQuery();
            }
        }

        public DataTable GetDataTable(SqlQueryBuilderBase builder)
        {
            using (SqlCommand cmd = new SqlCommand(builder.BuildQuery(), _connection))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddRange(builder.GetParameters().ToArray());

                using (SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd))
                {
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    return dataTable;
                }
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                _connection.Dispose();
                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
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
