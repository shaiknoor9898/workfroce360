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

        public DbSet<AppUser> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.Property(employee => employee.Salary)
                    .HasPrecision(18, 2);

                entity.HasIndex(employee => employee.EmployeeCode)
                    .IsUnique()
                    .HasDatabaseName("UX_Employees_EmployeeCode");

                entity.HasIndex(employee => employee.Email)
                    .IsUnique()
                    .HasDatabaseName("UX_Employees_Email");
            });

            modelBuilder.Entity<AppUser>(entity =>
            {
                entity.ToTable("Users");
                entity.Property(user => user.Email).HasMaxLength(254).IsRequired();
                entity.Property(user => user.PasswordHash).HasMaxLength(512).IsRequired();
                entity.Property(user => user.Role).HasMaxLength(20).IsRequired();
                entity.HasIndex(user => user.Email)
                    .IsUnique()
                    .HasDatabaseName("UX_Users_Email");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
