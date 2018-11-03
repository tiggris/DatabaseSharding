namespace DatabaseSharding.Schools.Infrastructure.Data.Sharding
{
    public interface IShardingKeyProvider<T>
    {
        T GetKey();
    }
}
