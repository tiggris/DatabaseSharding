using Microsoft.Azure.SqlDatabase.ElasticScale.ShardManagement;
using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

namespace DatabaseSharding.Schools.Infrastructure.Data.Sharding
{
    public class ShardletConnectionFactory<TKey> : IShardletConnectionFactory<TKey>
    {
        private readonly IShardingKeyProvider<TKey> _shardingKeyProvider;
        private readonly IShardMapManagerProvider _shardMapManagerProvider;

        public ShardletConnectionFactory(
            IShardingKeyProvider<TKey> shardingKeyProvider, 
            IShardMapManagerProvider shardMapManagerProvider)
        {
            _shardingKeyProvider = shardingKeyProvider;
            _shardMapManagerProvider = shardMapManagerProvider;
        }

        public DbConnection CreateDbConnection(string shardMapConnectionString, string shardMapName, string connectionString)
        {
            SqlConnection connection = null;
            try
            {
                var shardingKey = _shardingKeyProvider.GetKey();
                var shardMapManager = _shardMapManagerProvider.Get(shardMapConnectionString);
                var shardMap = shardMapManager.GetShardMap(shardMapName);
                connectionString = MassageConnString(connectionString);
                connection = shardMap.OpenConnectionForKey(shardingKey, connectionString, ConnectionOptions.Validate);

                // Set TenantId in SESSION_CONTEXT to shardingKey to enable Row-Level Security filtering
                var command = connection.CreateCommand();

                command.CommandText = @"exec sp_set_session_context @key=N'TenantId', @value=@shardingKey";
                command.Parameters.AddWithValue("@shardingKey", shardingKey);
                command.ExecuteNonQuery();

                return connection;
            }
            catch (Exception)
            {
                connection?.Dispose();
                throw;
            }

            string MassageConnString(string connstring)  // local function
            {
                var list = connstring.Split(';').ToList();
                list.Remove(list.First(x => x.StartsWith("Data Source")));
                list.Remove(list.First(x => x.StartsWith("Initial Catalog")));
                return string.Join(";", list);
            }
        }
    }
}
