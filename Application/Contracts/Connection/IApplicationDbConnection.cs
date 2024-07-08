using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Connection
{
    public interface IApplicationDbConnection
    {
        Task<IList<T>> GetObjects<T>(string sql, DynamicParameters parameters = null);
        Task<T> GetFirstOrDefault<T>(string sql, DynamicParameters parameters = null);
        Task<T> GetSingle<T>(string sql, DynamicParameters parameters = null);
        Task<int> Execute(string sql, DynamicParameters parameters = null);
    }
}
