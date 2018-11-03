using System.Data.Common;

namespace DatabaseSharding.Schools.Infrastructure.Data.Sharding
{
    public interface IShardletConnectionFactory<TKey>
    {
        DbConnection CreateDbConnection(string shardMapConnectionString, string shardMapName, string connectionString);
    }
}
