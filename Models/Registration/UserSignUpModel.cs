using System.ComponentModel.DataAnnotations;

namespace CRM.Models.Registration
{
    public class UserSignUpModel
    {
        [Required(ErrorMessage = "Please Enter Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please Enter Email")]
        [EmailAddress(ErrorMessage = "Please Enter Valid Email")]
        public string Email { get; set; }

       /* [Required(ErrorMessage = "Mobile number is required.")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Mobile number must be exactly 10 digits.")]
        public string PhoneNumber { get; set; }*/

        [Required(ErrorMessage = "Please Enter Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        //[Required(ErrorMessage = "Please Confirm Your Password")]
        //[Compare("Password", ErrorMessage = "Password Doesn't Match")]
        //[DataType(DataType.Password)]
        //public string ConfirmPassword { get; set; }

        public Roles? Roles { get; set; }
    }
    public enum Roles
    {
        Admin,
        Employee,
        Client
    }
}

