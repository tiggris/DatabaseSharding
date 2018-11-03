using Microsoft.Azure.SqlDatabase.ElasticScale.ShardManagement;

namespace DatabaseSharding.Schools.Infrastructure.Data.Sharding
{
    public interface IShardMapProvider<TKey>
    {
        ListShardMap<TKey> CreateOrGetListShardMap(string connectionString, string name);
    }
}