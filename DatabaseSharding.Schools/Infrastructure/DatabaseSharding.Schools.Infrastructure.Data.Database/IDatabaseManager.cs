using System.Security;

namespace DatabaseSharding.Schools.Infrastructure.Data.Database
{
    public interface IDatabaseManager
    {
        void CreateIfNotExtists(string connectionString);
        void CreateIfNotExtists(string server, string userName, SecureString password, string database);
    }
}
