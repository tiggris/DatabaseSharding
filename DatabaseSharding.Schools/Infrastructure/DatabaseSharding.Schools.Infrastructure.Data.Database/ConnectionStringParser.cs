using System.Data.SqlClient;

namespace DatabaseSharding.Schools.Infrastructure.Data.Database
{
    public class ConnectionStringParser : IConnectionStringParser
    {
        public DatabaseConnectionParameters Parse(string connectionString)
        {
            var sqlConnectionStringBuilder = new SqlConnectionStringBuilder(connectionString);

            var server = sqlConnectionStringBuilder["Server"]?.ToString();
            if (string.IsNullOrEmpty(server))
            {
                throw new InvalidConnectionStringException("Server cannot be null or empty");
            }

            var userName = sqlConnectionStringBuilder["User Id"]?.ToString();
            if (string.IsNullOrEmpty(userName))
            {
                throw new InvalidConnectionStringException("User Id cannot be null or empty");
            }

            var password = sqlConnectionStringBuilder["Password"]?.ToString();
            if(string.IsNullOrEmpty(password))
            {
                throw new InvalidConnectionStringException("Password cannot be null or empty");
            }

            var database = sqlConnectionStringBuilder["Database"]?.ToString();
            if (string.IsNullOrEmpty(database))
            {
                throw new InvalidConnectionStringException("Database cannot be null or empty");
            }

            return new DatabaseConnectionParameters
            {
                Server = server,
                UserName = userName,
                Password = password.ToSecureString(),
                Database = database
            };
        }
    }
}
