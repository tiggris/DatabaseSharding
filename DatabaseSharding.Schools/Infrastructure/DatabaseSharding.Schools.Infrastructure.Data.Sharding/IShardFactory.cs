using Microsoft.Azure.SqlDatabase.ElasticScale.ShardManagement;

namespace DatabaseSharding.Schools.Infrastructure.Data.Sharding
{
    public interface IShardFactory<TKey>
    {
        Shard CreateOrGet(ShardMap shardMap, string server, string database);
    }
}
