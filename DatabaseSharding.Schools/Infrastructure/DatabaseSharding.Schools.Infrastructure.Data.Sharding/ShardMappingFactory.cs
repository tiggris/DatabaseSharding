using Microsoft.Azure.SqlDatabase.ElasticScale.ShardManagement;

namespace DatabaseSharding.Schools.Infrastructure.Data.Sharding
{
    public class ShardMappingFactory<TKey> : IShardMappingFactory<TKey>
    {
        private readonly IShardMapProvider<TKey> _shardMapProvider;

        public ShardMappingFactory(IShardMapProvider<TKey> shardMapProvider)
        {
            _shardMapProvider = shardMapProvider;
        }

        public void CreateIfNotExists(ListShardMap<TKey> shardMap, Shard shard, TKey key)
        {
            var mappingExists = shardMap.TryGetMappingForKey(key, out var pointMapping);

            if(!mappingExists)
            {
                shardMap.CreatePointMapping(key, shard);
            }
        }
    }
}
