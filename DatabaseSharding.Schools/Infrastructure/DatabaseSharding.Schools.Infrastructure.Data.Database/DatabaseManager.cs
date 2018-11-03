using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System.Security;

namespace DatabaseSharding.Schools.Infrastructure.Data.Database
{
    public class DatabaseManager : IDatabaseManager
    {
        private readonly IConnectionStringParser _connectionStringParser;

        public DatabaseManager(IConnectionStringParser connectionStringParser)
        {
            _connectionStringParser = connectionStringParser;
        }

        public void CreateIfNotExtists(string connectionString)
        {
            var databaseConnectionParameters = _connectionStringParser.Parse(connectionString);
            CreateIfNotExtists(
                databaseConnectionParameters.Server, 
                databaseConnectionParameters.UserName, 
                databaseConnectionParameters.Password,
                databaseConnectionParameters.Database);
        }

        public void CreateIfNotExtists(string serverName, string userName, SecureString password, string databaseName)
        {
            var serverConnection = new ServerConnection(serverName, userName, password);
            var server = new Server(serverName);
            if (!server.Databases.Contains(databaseName))
            {
                var database = new Microsoft.SqlServer.Management.Smo.Database(server, databaseName);
                database.Create();
                var user = new User(database, userName);
                user.Create();
                var databasePermissionSet = new DatabasePermissionSet(DatabasePermission.CreateType);
                databasePermissionSet.Add(DatabasePermission.CreateSchema);
                databasePermissionSet.Add(DatabasePermission.CreateTable);
                databasePermissionSet.Add(DatabasePermission.CreateFunction);
                databasePermissionSet.Add(DatabasePermission.CreateProcedure);
                database.Grant(databasePermissionSet, userName);
            }
        }
    }
}
