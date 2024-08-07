using System.ComponentModel.DataAnnotations;

namespace CRM.Models.Domain
{
    public class EmployeeProject
    {
        [Key]
        public Guid Id { get; set; }

        public Guid? ProjectId { get; set; }
        public Project Project { get; set; }

        public Guid? EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}
