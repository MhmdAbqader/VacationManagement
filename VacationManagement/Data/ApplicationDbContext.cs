using Microsoft.EntityFrameworkCore;
using VacationManagement.Models;

namespace VacationManagement.Data
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<GetReportsVacationData>().HasNoKey().ToSqlQuery("GetReportsVacationDatas");
        }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<RequestVacation> RequestVacations { get; set; }
        public DbSet<VacationPlan> VacationPlans { get; set; }
        public DbSet<VacationType> VacationTypes { get; set; }
        public DbSet<GetReportsVacationData> GetReportsVacationDatas { get; set; }
    }
}

