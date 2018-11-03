using DatabaseSharding.Schools.Infrastructure.Data.Sharding.Exceptions;
using Microsoft.Azure.SqlDatabase.ElasticScale.ShardManagement;

namespace DatabaseSharding.Schools.Infrastructure.Data.Sharding
{
    public class ShardMapManagerProvider : IShardMapManagerProvider
    {
        public void EnsureCreated(string connectionString)
        {
            var shardMapManagerExists = ShardMapManagerFactory.TryGetSqlShardMapManager(
                connectionString,
                ShardMapManagerLoadPolicy.Lazy,
                out var shardMapManager);

            if(!shardMapManagerExists)
            {
                ShardMapManagerFactory.CreateSqlShardMapManager(connectionString);
            }
        }

        public ShardMapManager Get(string connectionString)
        {
            var shardMapManagerExists = ShardMapManagerFactory.TryGetSqlShardMapManager(
                connectionString,
                ShardMapManagerLoadPolicy.Lazy,
                out var shardMapManager);

            if(!shardMapManagerExists)
            {
                throw new ShardMapManagerNotExistsException();
            }

            return shardMapManager;
        }

        public ShardMapManager CreateOrGet(string connectionString)
        {
            var shardMapManagerExists = ShardMapManagerFactory.TryGetSqlShardMapManager(
               connectionString,
               ShardMapManagerLoadPolicy.Lazy,
               out var shardMapManager);

            return shardMapManagerExists ? shardMapManager :
                ShardMapManagerFactory.CreateSqlShardMapManager(connectionString);
        }
    }
}
