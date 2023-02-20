using EmpMgmt.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace EmpMgmt.Data
{
    public class EmDbContext : DbContext
    {
        public EmDbContext(DbContextOptions<EmDbContext> options) : base(options)
        {

        }

        public DbSet<Employee> Employees { get; set; }
    }
}
