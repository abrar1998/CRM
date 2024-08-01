using CRM.Models.Domain;

namespace CRM.Repositories.EmployeeRepository
{
    public interface IEmployeeRepo
    {
        bool EmployeeUserIdExists(string EmployeeUserId);
        Task AddEmployee(Employee employee);

        //get details of current logged in user
        Task<Employee> GetEmployeeDetails(string userId);
        Task<IEnumerable<Employee>> GetAllEmployees();
        //get employee detail by employee id
        Task<Employee> GetEmployee(Guid id);
    }
}
