using CRM.Models.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CRM.DB_Context
{
    public class DataContext:IdentityDbContext
    {
        public DataContext(DbContextOptions<DataContext> opt):base(opt)
        {
            
        }

        public DbSet<Admin> AdminTable { get; set; }
        public DbSet<Employee> EmployeesTable { get; set; }
        public DbSet<Client> ClientTable { get; set; }
    }
}
