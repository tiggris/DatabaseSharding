using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseSharding.Schools.Infrastructure.Data.Sharding
{
    public interface IShardsSettingsProvider
    {
        string ShardMapConnectionString { get; }
        string ShardMapName { get; }
        string ShardServer { get; }
    }
}
