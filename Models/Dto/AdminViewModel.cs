using System.ComponentModel.DataAnnotations;

namespace CRM.Models.Dto
{
    public class AdminViewModel
    {
        [Required(ErrorMessage = "please enter name")]
        [Display(Name = "Photo")]
        public string Name { get; set; }

        [Required(ErrorMessage = "please enter email")]
        public string Email { get; set; }


        [Required(ErrorMessage = "please enter phone number")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Mobile number must be exactly 10 digits.")]
        public string PhoneNumber { get; set; }

        [DataType(DataType.Upload)]
        [Required(ErrorMessage = "please Upload A Photo Less than 400kb")]
        [Display(Name = "Photo")]
        public IFormFile AdminPhoto { get; set; } // Path to the admin photo

        [DataType(DataType.Date)]
        [Display(Name = "Joining Date")]
        public DateOnly? JoiningDate { get; set; }
    }
}
