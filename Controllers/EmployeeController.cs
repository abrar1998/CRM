using CRM.Models.Domain;
using CRM.Models.Dto;
using CRM.Models.Registration;
using CRM.Repositories.AdminRepository;
using CRM.Repositories.EmployeeRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.CodeDom;
using System.Security.Claims;

namespace CRM.Controllers
{
    [Authorize(Roles =RolesClass.Employee)]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepo employeeRepo;
        private readonly IWebHostEnvironment webHostEnvironment;

        public EmployeeController(IEmployeeRepo employeeRepo, IWebHostEnvironment webHostEnvironment)
        {
            this.employeeRepo = employeeRepo;
            this.webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult EmployeeDashBoard()
        {
            return View();
        }

        [HttpGet]
        public IActionResult EmployeeRegistration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EmployeeRegistration(EmployeeViewModel evm)
        {
            var LoginuserIdd = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userExists = employeeRepo.EmployeeUserIdExists(LoginuserIdd!);
            if (userExists)
            {
                return RedirectToAction("EmployeeDashBoard", "Employee");
            }

            if (ModelState.IsValid)
            {

                if (User.IsInRole(RolesClass.Employee))
                {
                    var LoginuserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
                    //var CurrentUser = await userManager.GetUserAsync(User);

                    string[] allowedExtensions = { ".jpg", ".jpeg", ".png", }; // Add more extensions if needed

                    string photoExtension = Path.GetExtension(evm.EmployeePhoto.FileName).ToLower();

                    if (evm.EmployeePhoto.Length > 400 * 1024) // 400 KB limit
                    {
                        ModelState.AddModelError("", " Profile  Photo Should Be Less Than 400kb");
                        return View(evm);
                    }


                    if (!allowedExtensions.Contains(photoExtension))
                    {
                        ModelState.AddModelError("", "Invalid photo file extension. Allowed extensions are .jpg, .jpeg,");
                        return View(evm);
                    }

                    string FileLocation = UploadFile(evm);

                    DateOnly joinDate;
                    if (evm.JoiningDate == null)
                    {
                        joinDate = DateOnly.FromDateTime(DateTime.UtcNow.Date);
                    }
                    else
                    {
                        joinDate = (DateOnly)evm.JoiningDate;
                    }

                    var _EmployeeData = new Employee()
                    {
                        Name = evm.Name,
                        PhoneNumber = evm.PhoneNumber,
                        PhotoPath = FileLocation,
                        Email = evm.Email,
                        Emp_UserId = LoginuserId,
                        JoiningDate = evm.JoiningDate,
                        EmployeeDesignation = evm.EmployeeDesignation
                      


                    };

                    try
                    {
                        await employeeRepo.AddEmployee(_EmployeeData);
                        TempData["Registered"] = true;
                        return RedirectToAction("EmployeeDashBoard", "Employee");
                    }
                    catch (Exception ex)
                    {
                        return BadRequest("Registration Failed \n" + ex.Message);
                    }

                }
            }
            return View(evm);
        }



        //upload photo
        private string UploadFile(EmployeeViewModel evm)
        {
            string filename = null;
            if (evm.EmployeePhoto != null && evm.EmployeePhoto.Length < 1048576)
            {
                string filedir = Path.Combine(webHostEnvironment.WebRootPath, "Images");
                filename = Guid.NewGuid().ToString() + "-" + evm.EmployeePhoto.FileName;
                string filepath = Path.Combine(filedir, filename);
                using (var filestream = new FileStream(filepath, FileMode.Create))
                {
                    evm.EmployeePhoto.CopyTo(filestream);
                }
            }
            return filename;
        }


    }
}
