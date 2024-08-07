using System.ComponentModel.DataAnnotations;

namespace CRM.Models.Dto
{
    public class EmployeeViewModel
    {
        [DataType(DataType.Upload)]
        [Required(ErrorMessage = "please Upload A Photo Less than 400kb")]
        [Display(Name = "Photo")]
        public IFormFile EmployeePhoto { get; set; }

        [Required(ErrorMessage = "please enter name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "please enter enail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "please enter phone number")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Mobile number must be exactly 10 digits.")]
        [Display(Name="Phone Number")]
        public string PhoneNumber { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Joining Date")]
        public DateOnly? JoiningDate { get; set; }

        [Required(ErrorMessage ="Please select your designation")]
        [Display(Name ="Designation")]
        public string EmployeeDesignation { get; set; }

        [Required(ErrorMessage = "Please select your address")]
        [Display(Name = "address")]
        public string EmployeeAddress { get; set; }
    }
}
