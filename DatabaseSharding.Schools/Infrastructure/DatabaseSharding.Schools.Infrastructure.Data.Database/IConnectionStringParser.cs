namespace DatabaseSharding.Schools.Infrastructure.Data.Database
{
    public interface IConnectionStringParser
    {
        DatabaseConnectionParameters Parse(string connectionString);
    }
}
