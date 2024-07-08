using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Persistence.Context;
using System.Data;
using Application.Contracts.Connection;

namespace Persistence.Connection
{
    public class ApplicationDbConnection : IApplicationDbConnection, IDisposable
    {
        private readonly IDbConnection connection;

        public ApplicationDbConnection(IConfiguration configuration, TemplateDbContext dbContext)
        {
            connection = new SqlConnection(configuration.GetConnectionString("TemplateConnectionString"));
        }

        public async Task<int> Execute(string sql, DynamicParameters parameters = null)
        {
            if (parameters == null) return await connection.ExecuteAsync(sql);
            return (await connection.ExecuteAsync(sql, parameters, null, null, CommandType.StoredProcedure));
        }

        public async Task<T> GetFirstOrDefault<T>(string sql, DynamicParameters parameters = null)
        {
            if (parameters == null) return await connection.QueryFirstOrDefaultAsync<T>(sql);
            return (await connection.QueryFirstOrDefaultAsync<T>(sql, parameters, null, null, CommandType.StoredProcedure));
        }

        public async Task<IList<T>> GetObjects<T>(string sql, DynamicParameters parameters = null)
        {
            if (parameters == null) return (await connection.QueryAsync<T>(sql)).AsList();
            return (await connection.QueryAsync<T>(sql, parameters, null, null, CommandType.StoredProcedure)).AsList();
        }

        public async Task<T> GetSingle<T>(string sql, DynamicParameters parameters = null)
        {
            if (parameters == null) return await connection.QuerySingleAsync<T>(sql);
            return (await connection.QuerySingleAsync<T>(sql, parameters, null, null, CommandType.StoredProcedure));
        }

        public void Dispose()
        {
            connection.Dispose();
        }

    }
}
