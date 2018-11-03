using Microsoft.Azure.SqlDatabase.ElasticScale.ShardManagement;

namespace DatabaseSharding.Schools.Infrastructure.Data.Sharding
{
    public class ShardMapProvider<TKey> : IShardMapProvider<TKey>
    {
        private readonly IShardMapManagerProvider _shardMapManagerProvider;

        public ShardMapProvider(IShardMapManagerProvider shardMapManagerProvider)
        {
            _shardMapManagerProvider = shardMapManagerProvider;
        }

        public ListShardMap<TKey> CreateOrGetListShardMap(string connectionString, string name)
        {
            var shardMapManager = _shardMapManagerProvider.CreateOrGet(connectionString);
            var shardMapExists = shardMapManager.TryGetListShardMap<TKey>(name, out var shardMap);

            return shardMapExists ? shardMap : shardMapManager.CreateListShardMap<TKey>(name);
        }
    }
}
