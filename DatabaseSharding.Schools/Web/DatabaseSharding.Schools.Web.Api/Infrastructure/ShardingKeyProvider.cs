using DatabaseSharding.Schools.Infrastructure.Data.Sharding;
using Microsoft.AspNetCore.Http;

namespace DatabaseSharding.Schools.Web.Api.Infrastructure
{
    public class ShardingKeyProvider : IShardingKeyProvider<int>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ShardingKeyProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int GetKey()
        {
            if (_httpContextAccessor.HttpContext != null)
            {
                var shardingKeyParameters = _httpContextAccessor.HttpContext.Request.Query["shardingKey"].ToArray();
                if (shardingKeyParameters.Length == 0)
                {
                    throw new System.Exception("Sharding key is not provided");
                }
                if (!int.TryParse(shardingKeyParameters[0], out var shardingKey))
                {
                    throw new System.Exception("Valid sharding key is not provided");
                }
                return shardingKey;
            }
            return 1;
        }
    }
}
