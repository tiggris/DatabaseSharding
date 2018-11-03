using DatabaseSharding.Schools.Domain.Model.Employees;
using DatabaseSharding.Schools.Domain.Model.Students;
using Microsoft.EntityFrameworkCore;

namespace DatabaseSharding.Schools.Infrastructure.Data
{
    public class SchoolsDbContext : DbContext
    {
        DbSet<Teacher> Teachers { get; set; }
        DbSet<Student> Students { get; set; }

        public SchoolsDbContext(DbContextOptions options) :
            base(options)
        {

        }
    }
}
