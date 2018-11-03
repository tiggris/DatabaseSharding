using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DatabaseSharding.Schools.Infrastructure.Data.Tests
{
    [TestClass]
    public abstract class TestBase
    {
        private static readonly string _connectionString;
        private const string _migrationsAssemblyName = "BooksAndMore.Catalogue.Infrastructure.Data.Migrations";
        protected static DbContextOptionsBuilder<SchoolsDbContext> _optionsBuilder;

        static TestBase()
        {
            _connectionString = $"Server=(localdb)\\mssqllocaldb;Database=BooksCatalogue.DotNetCore_{Guid.NewGuid()};Trusted_Connection=True;MultipleActiveResultSets=true";
            _optionsBuilder = GetOptionsBuilder();
        }

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext testContext)
        {
            using (var context = CreateNewContext())
            {
                context.Database.Migrate();
            }
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            using (var context = CreateNewContext())
            {
                context.Database.EnsureDeleted();
            }
        }

        private static DbContextOptionsBuilder<SchoolsDbContext> GetOptionsBuilder(bool enableLazyLoading = false)
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkSqlServer()
                .AddEntityFrameworkProxies()
                .BuildServiceProvider();

            var optionsBuilder = new DbContextOptionsBuilder<SchoolsDbContext>();
            optionsBuilder
                .ConfigureWarnings(warnings => warnings.Log(
                    CoreEventId.LazyLoadOnDisposedContextWarning,
                    CoreEventId.DetachedLazyLoadingWarning))
                .UseSqlServer(_connectionString,
                    sqlServerOptions => sqlServerOptions.MigrationsAssembly(_migrationsAssemblyName))
                .UseLazyLoadingProxies()
                .UseInternalServiceProvider(serviceProvider);

            return optionsBuilder;
        }

        protected static SchoolsDbContext CreateNewContext(bool enableLazyLoading = false)
        {
            var context = new SchoolsDbContext(_optionsBuilder.Options);
            context.ChangeTracker.LazyLoadingEnabled = enableLazyLoading;
            return context;
        }
    }
}
