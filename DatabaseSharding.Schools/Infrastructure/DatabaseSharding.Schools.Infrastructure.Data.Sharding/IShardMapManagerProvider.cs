using Microsoft.Azure.SqlDatabase.ElasticScale.ShardManagement;

namespace DatabaseSharding.Schools.Infrastructure.Data.Sharding
{
    public interface IShardMapManagerProvider
    {
        void EnsureCreated(string connectionString);
        ShardMapManager Get(string connectionString);
        ShardMapManager CreateOrGet(string connectionString);
    }
}
