using DatabaseSharding.Schools.Infrastructure.Data;
using DatabaseSharding.Schools.Infrastructure.Data.Database;
using DatabaseSharding.Schools.Infrastructure.Data.Sharding;
using DatabaseSharding.Schools.Web.Api.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data.SqlClient;

namespace DatabaseSharding.Schools.Web.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private const string _migrationsAssembly = "DatabaseSharding.Schools.Infrastructure.Data.Migrations";

        public IConfiguration Configuration { get; }

        private string _shardMapConnectionString;
        private string ShardMapConnectionString =>
                        _shardMapConnectionString ??
                        (_shardMapConnectionString = Configuration.GetConnectionString("SchoolsShardMapDb"));
        private string ShardMapName => "SchoolsShardMap";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("SchoolsDb");

            services.AddEntityFrameworkProxies();
            services.AddEntityFrameworkSqlServer();
            services.AddDbContext<SchoolsDbContext>(
                (serviceProvider, options) =>
                options
                    .UseSqlServer(serviceProvider.GetService<IShardletConnectionFactory<int>>().CreateDbConnection(ShardMapConnectionString, ShardMapName, connectionString),
                        sqlServerOptions =>
                        sqlServerOptions
                            //.MigrationsAssembly(_migrationsAssemblyName)
                            .EnableRetryOnFailure())
                    .UseLazyLoadingProxies()
                    .ConfigureWarnings(warnings => warnings.Log(
                        CoreEventId.LazyLoadOnDisposedContextWarning,
                        CoreEventId.DetachedLazyLoadingWarning)));

            services.AddMvc();

            var shardsConfiguration = new ShardsConfiguration();
            Configuration.Bind("ShardsConfiguration", shardsConfiguration);
            services.AddSingleton(shardsConfiguration);

            services.AddSingleton<IShardMapManagerProvider, ShardMapManagerProvider>();
            services.AddSingleton<IShardMapProvider<int>, ShardMapProvider<int>>();
            services.AddTransient<IShardFactory<int>, ShardFactory<int>>();
            services.AddTransient<IShardMappingFactory<int>, ShardMappingFactory<int>>();
            services.AddTransient<IShardingKeyProvider<int>, ShardingKeyProvider>();
            services.AddTransient<IShardletConnectionFactory<int>, ShardletConnectionFactory<int>>();
            services.AddTransient<IDatabaseManager, DatabaseManager>();
            services.AddTransient<IConnectionStringParser, ConnectionStringParser>();
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    ConfigureShards(serviceScope);
                }
            }

            app.UseMvc();
        }

        private void ConfigureShards(IServiceScope serviceScope)
        {
            var databaseManager = serviceScope.ServiceProvider.GetService<IDatabaseManager>();
            var shardsConfiguration = serviceScope.ServiceProvider.GetService<ShardsConfiguration>();
            var shardProvider = serviceScope.ServiceProvider.GetService<IShardFactory<int>>();
            var shardMapProvider = serviceScope.ServiceProvider.GetService<IShardMapProvider<int>>();
            var shardMappingProvider = serviceScope.ServiceProvider.GetService<IShardMappingFactory<int>>();

            databaseManager.CreateIfNotExtists(ShardMapConnectionString);

            foreach (var shardConfiguration in shardsConfiguration.Shards)
            {
                databaseManager.CreateIfNotExtists(shardConfiguration.Server, shardConfiguration.UserName, shardConfiguration.Password.ToSecureString(), shardConfiguration.Database);
                var shardMap = shardMapProvider.CreateOrGetListShardMap(ShardMapConnectionString, ShardMapName);
                var shard = shardProvider.CreateOrGet(shardMap, shardConfiguration.Server, shardConfiguration.Database);

                foreach (var shardingKey in shardConfiguration.ShardingKeys)
                {
                    shardMappingProvider.CreateIfNotExists(shardMap, shard, shardingKey);
                }

                var optionsBuilder = new DbContextOptionsBuilder<SchoolsDbContext>();
                var connectionStringBuiler = new SqlConnectionStringBuilder();
                connectionStringBuiler.Add("Server", shardConfiguration.Server);
                connectionStringBuiler.Add("Data Source", shardConfiguration.Database);
                connectionStringBuiler.Add("User Id", shardConfiguration.UserName);
                connectionStringBuiler.Add("Password", shardConfiguration.Password);
                optionsBuilder.UseSqlServer(connectionStringBuiler.ConnectionString,
                    sqlServerOptions => sqlServerOptions.MigrationsAssembly(_migrationsAssembly));
                var schoolsDbContext = new SchoolsDbContext(optionsBuilder.Options);
                schoolsDbContext.Database.EnsureCreated();
            }
        }
    }
}
