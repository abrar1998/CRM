using CRM.Models.Registration;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM.Models.Domain
{
    public class Employee
    {
        [Key]
        public Guid EmployeeId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PhotoPath { get; set; } // Path to the employee photo

        public string EmployeeDesignation { get; set; }
        public DateOnly? JoiningDate { get; set; }

        public string? Emp_ProjectId { get; set; } = null;
        [ForeignKey("Emp_ProjectId ")]
        public ICollection<Project> Projects { get; set; } // Navigation property for Projects
        public string? Emp_UserId { get; set; }
        [ForeignKey("Emp_UserId ")]
        public ApplicationUser User { get; set; }

        public ICollection<Sale> Sales { get; set; } // Navigation Property
    }
}
