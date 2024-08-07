using CRM.DB_Context;
using CRM.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace CRM.Repositories.EmployeeRepository
{
    public class EmployeeRepo : IEmployeeRepo
    {
        private readonly DataContext dbcontext;
        private readonly ILogger logger;

        public EmployeeRepo(DataContext dbcontext, ILogger<EmployeeRepo> _logger)
        {
            this.dbcontext = dbcontext;
            logger = _logger;
        }
        public bool EmployeeUserIdExists(string EmployeeUserId)
        {
            return dbcontext.EmployeesTable.Any(e=>e.Emp_UserId== EmployeeUserId);
        }

        public async Task AddEmployee(Employee employee)
        {
            try
            {
                await dbcontext.EmployeesTable.AddAsync(employee);
                await dbcontext.SaveChangesAsync();
            }
            catch(Exception exp)
            {
                logger.LogError(exp, "Registration failed for employee.");
                throw;
            }
        }

        //get details of current logged in employee
        public async Task<Employee> GetEmployeeDetails(string userId)
        {
            var details = await dbcontext.EmployeesTable.Include(e=>e.EmployeeProjects).FirstOrDefaultAsync(e=>e.Emp_UserId == userId);
            return details!;
        }

        //get details of all employees
        public async Task<IEnumerable<Employee>> GetAllEmployees()
        {
            var employees = await dbcontext.EmployeesTable.ToListAsync();
            if(employees == null)
            {
                return null;
            }
            else
            {
                return employees;
            }
        }

        //get employee details for admin
        public async Task<Employee> GetEmployee(Guid id)
        {
            var daa = await dbcontext.EmployeesTable.FirstOrDefaultAsync(e=>e.EmployeeId == id);
            return daa!;
        }


    }
}
