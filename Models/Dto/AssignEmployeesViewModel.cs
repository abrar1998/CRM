using CRM.Models.Domain;

namespace CRM.Models.Dto
{
    public class AssignEmployeesViewModel
    {
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; }
        public IEnumerable<Employee> Employees { get; set; }
        public IEnumerable<Client> Clients { get; set; }
        public bool ProjectManagerIdExists {  get; set; }
        public string? ProjectManagerName {  get; set; }
        public bool ProjectClientExists {  get; set; }
        public string? ProjectClientName { get; set; }
        public bool TeamExists {  get; set; }
    }
}
