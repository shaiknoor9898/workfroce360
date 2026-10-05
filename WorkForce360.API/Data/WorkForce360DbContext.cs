using Microsoft.EntityFrameworkCore;
using WorkForce360.API.Models;

namespace WorkForce360.API.Data
{
    public class WorkForce360DbContext : DbContext
    {
        public WorkForce360DbContext(
            DbContextOptions<WorkForce360DbContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
    }
}