using System.ComponentModel.DataAnnotations;

namespace CRM.Models.Domain
{
    public class Project
    {
        [Key]
        public Guid ProjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Client Client { get; set; } // Navigation property for Client
        public Employee Employee { get; set; } // Navigation property for Employee
        public int? Year { get; set; }
        public DateOnly ProjectDate { get; set; }
    }
}
