using CRM.Models.Registration;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM.Models.Domain
{
    public class Client
    {
        [Key]
        public Guid ClientId { get; set; }
        public string Name { get; set; }
        public string Country {  get; set; }
        public string Address {  get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PhotoPath { get; set; } // Path to the client photo
        public DateOnly? JoiningDate { get; set; }

        public ICollection<Project> Projects { get; set; } // Navigation property for related projects

        //creating relationship with user table
        public string? Client_UserId { get; set; }
        [ForeignKey("C_UserId ")]
        public ApplicationUser User { get; set; }
        public ICollection<Sale> Sales { get; set; } // Navigation Property

    }
}

