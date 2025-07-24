using MSSQL.QueryBuilder;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;

namespace MSSQL
{
    public partial class SqlExecHelper
    {
        public async Task ConnectAsync()
        {
            if (_connection != null && _connection.State != ConnectionState.Open)
                await _connection.OpenAsync();
        }

        public void DisconnectAsync()
        {
            if (_connection != null && _connection.State != ConnectionState.Closed)
                _connection.Close();
        }

        public async Task<List<T>> ExecuteReaderAsync<T>(string sql, Dictionary<string, object> parameters, Func<SqlDataReader, T> mapper)
        {
            await ConnectAsync();

            using (SqlCommand cmd = new SqlCommand(sql, _connection))
            {
                cmd.CommandType = CommandType.Text;

                if (parameters == null)
                    parameters = new Dictionary<string, object>();

                foreach (KeyValuePair<string, object> pair in parameters)
                    cmd.Parameters.AddWithValue(pair.Key, pair.Value);

                List<T> list = new List<T>();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                        list.Add(mapper(reader));
                }

                return list;
            }
        }

        public async Task<Tscalar> ExecuteScalarQueryAsync<Tscalar>(string sql, Dictionary<string, object> parameters = null)
        {
            await ConnectAsync();

            using (SqlCommand cmd = new SqlCommand(sql, _connection))
            {
                cmd.CommandType = CommandType.Text;

                if (parameters == null)
                    parameters = new Dictionary<string, object>();

                foreach (KeyValuePair<string, object> pair in parameters)
                    cmd.Parameters.AddWithValue(pair.Key, pair.Value);

                object val = await cmd.ExecuteScalarAsync();

                return val is null ? default : (Tscalar)val;
            }
        }

        public async Task<int> ExecuteNonQueryAsync(string sql, Dictionary<string, object> parameters = null)
        {
            await ConnectAsync();

            using (SqlCommand cmd = new SqlCommand(sql, _connection))
            {
                cmd.CommandType = CommandType.Text;

                if (parameters == null)
                    parameters = new Dictionary<string, object>();

                foreach (KeyValuePair<string, object> pair in parameters)
                    cmd.Parameters.AddWithValue(pair.Key, pair.Value);

                return await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<List<T>> ExecuteReaderAsync<T>(SqlQueryBuilderBase builder, Func<SqlDataReader, T> mapper)
        {
            await ConnectAsync();

            using (SqlCommand cmd = new SqlCommand(builder.BuildQuery(), _connection))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddRange(builder.GetParameters().ToArray());

                List<T> list = new List<T>();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                        list.Add(mapper(reader));
                }

                return list;
            }
        }

        public async Task<Tscalar> ExecuteScalarQueryAsync<Tscalar>(SqlQueryBuilderBase builder)
        {
            await ConnectAsync();

            using (SqlCommand cmd = new SqlCommand(builder.BuildQuery(), _connection)) {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddRange(builder.GetParameters().ToArray());

                object val = await cmd.ExecuteScalarAsync();

                return val is null ? default : (Tscalar)val;
            }; 
        }

        public async Task<int> ExecuteNonQueryAsync(SqlQueryBuilderBase builder)
        {
            await ConnectAsync();

            using (SqlCommand cmd = new SqlCommand(builder.BuildQuery(), _connection))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddRange(builder.GetParameters().ToArray());

                return await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}
