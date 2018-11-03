using Microsoft.Azure.SqlDatabase.ElasticScale.ShardManagement;

namespace DatabaseSharding.Schools.Infrastructure.Data.Sharding
{
    public class ShardFactory<TKey> : IShardFactory<TKey>
    {
        private readonly IShardMapProvider<TKey> _shardMapProvider;

        public ShardFactory(IShardMapProvider<TKey> shardMapProvider)
        {
            _shardMapProvider = shardMapProvider;
        }

        public Shard CreateOrGet(ShardMap shardMap, string server, string database)
        {
            var location = new ShardLocation(server, database);
            var shardExists = shardMap.TryGetShard(location, out var shard);

            return shardExists ? shard : shardMap.CreateShard(location);
        }
    }
}
