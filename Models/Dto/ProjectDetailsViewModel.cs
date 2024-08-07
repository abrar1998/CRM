using CRM.Models.Domain;

namespace CRM.Models.Dto
{
    public class ProjectDetailsViewModel
    {
        public Project Project { get; set; }
        public IEnumerable<Employee>? TeamMembers { get; set; }
        public Employee? ProjectManager { get; set; }
        public string? ProjectClient { get; set; }
    }
}
