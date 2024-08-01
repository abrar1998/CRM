using CRM.Models.Registration;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM.Models.Domain
{
    public class Admin
    {
        [Key]
        public Guid AdminId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PhotoPath { get; set; } // Path to the admin photo
        public DateOnly? JoiningDate { get; set; }
        public string? Admin_UserId { get; set; }
        [ForeignKey("Admin_UserId ")]
        public ApplicationUser User { get; set; }
    }
}
