using Microsoft.Azure.SqlDatabase.ElasticScale.ShardManagement;

namespace DatabaseSharding.Schools.Infrastructure.Data.Sharding
{
    public interface IShardMappingFactory<TKey>
    {
        void CreateIfNotExists(ListShardMap<TKey> shardMap, Shard shard, TKey key);
    }
}
