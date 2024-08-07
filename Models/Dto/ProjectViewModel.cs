using System.ComponentModel.DataAnnotations;

namespace CRM.Models.Dto
{
    public class ProjectViewModel
    {
        [Required(ErrorMessage ="please enter name for project")]
        public string ProjectName { get; set; }

        [Required(ErrorMessage = "please enter  project company name")]
        public string ProjectCompanyName { get; set; }

        [Required(ErrorMessage = "please enter  project description")]
        public string Description { get; set; }

        public IFormFile ProjectPhoto { get; set; }

        [Required(ErrorMessage = "please enter year")]
        public int Year { get; set; }
    }
}
