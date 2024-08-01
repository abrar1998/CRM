using Microsoft.AspNetCore.Identity;

namespace CRM.Models.Registration
{
    public class ApplicationUser: IdentityUser
    {
        public string Name { get; set; }
        //public override string? PhoneNumber { get; set; }
    }
}
