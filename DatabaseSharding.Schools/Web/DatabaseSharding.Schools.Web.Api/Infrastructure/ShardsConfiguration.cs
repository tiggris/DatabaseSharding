using System.Collections.Generic;

namespace DatabaseSharding.Schools.Web.Api.Infrastructure
{
    public class ShardsConfiguration
    {
        public class ShardConfiguration
        {
            public string Server { get; set; }
            public string Database { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
            public IList<int> ShardingKeys { get; set; }
        }

        public IList<ShardConfiguration> Shards { get; set; }
    }
}
