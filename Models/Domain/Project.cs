using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM.Models.Domain
{
    public class Project
    {
        [Key]
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectCompanyName { get; set; }
        public string Description { get; set; }
        public int ProjectStatus { get; set; } = 1; //based on this project will be categorized on completed and ongoing projects
        public string ProjectPhotoPath { get; set; }
        public int Year { get; set; }
        public int ProgressPercentage { get; set; } = 0; // 0 to 100 representing completion percentage
        public Guid? C_id { get; set; } = null;
        public ICollection<Client> ProjectClient { get; set; }

        // Project Manager relationship
        public Guid? ProjectManagerId { get; set; }
        public Employee ProjectManager { get; set; }
        public DateOnly ProjectDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow.Date);
        // Navigation property for Projects (many-to-many relationship)
        public ICollection<EmployeeProject> EmployeeProjects { get; set; } = new List<EmployeeProject>();
    }
}
