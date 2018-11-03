using System.Security;

namespace DatabaseSharding.Schools.Infrastructure.Data.Database
{
    public class DatabaseConnectionParameters
    {
        public string Server { get; set; }
        public string UserName { get; set; }
        public SecureString Password { get; set; }
        public string Database { get; set; }
    }
}
