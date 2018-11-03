namespace DatabaseSharding.Schools.Infrastructure.Data.Sharding
{
    public class ShardConfiguration
    {
        public string ShardKey { get; set; }
        public string Server { get; set; }
        public string Database { get; set; }
    }
}
