using System.ComponentModel.DataAnnotations;

namespace CRM.Models.Dto
{
    public class ClientViewModel
    {
        [DataType(DataType.Upload)]
        [Required(ErrorMessage = "please Upload A Photo Less than 400kb")]
        [Display(Name = "photo")]
        public IFormFile ClientPhoto { get; set; }

        [Required(ErrorMessage = "please enter your name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "please enter your country")]
        public string Country { get; set; }

        [Required(ErrorMessage = "please enter your Address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "please enter your company name")]
        [Display(Name="Company Name")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "please enter your email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "please enter your Phone number")]
        [Display(Name="Phone Number")]
        public string PhoneNumber { get; set; }

        [DataType(DataType.Date)]
        [Display(Name="Joining Date")]
        public DateOnly? JoiningDate { get; set; }
    }
}
