using CRM.Models.Domain;
using CRM.Models.Registration;
using Microsoft.AspNetCore.Identity;
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
        public DbSet<Project> ProjectTable { get; set; }
        public DbSet<Sale> SaleTable { get; set; }

        public DbSet<EmployeeProject> EmployeeProjectsTable { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);
            var AdminId = "482a3ce4-ac80-4081-8441-e15354f9b70e";
            var EmployeeId = "9c327fed-80eb-4113-8820-b11fb583122c";
            var ClientId = "59d453c4-0425-4e08-a28c-0f772ef388a2";
            var roles = new List<IdentityRole>()
            {
                new IdentityRole()
                {
                    Id = AdminId ,
                    ConcurrencyStamp = AdminId ,
                    Name = "Admin",
                    NormalizedName = "Admin".ToUpper()
                },
                new IdentityRole()
                {
                    Id = EmployeeId ,
                    ConcurrencyStamp = EmployeeId,
                    Name = "Employee",
                    NormalizedName = "Employee".ToUpper()
                },
                new IdentityRole()
                {
                    Id = ClientId ,
                    ConcurrencyStamp = ClientId ,
                    Name = "Client",
                    NormalizedName = "Client".ToUpper()
                },
            };

            builder.Entity<IdentityRole>().HasData(roles);
        }


        //add admin account

     




    }
}
