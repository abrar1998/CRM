using CRM.Models.Domain;
using CRM.Models.Dto;
using CRM.Models.Registration;
using CRM.Repositories.AdminRepository;
using CRM.Repositories.EmployeeRepository;
using CRM.Repositories.ProjectRepository;
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
        private readonly IProjectRepo projectRepo;

        public EmployeeController(IEmployeeRepo employeeRepo, IWebHostEnvironment webHostEnvironment, IProjectRepo projectRepo)
        {
            this.employeeRepo = employeeRepo;
            this.webHostEnvironment = webHostEnvironment;
            this.projectRepo = projectRepo;
        }

        [HttpGet]
        public async Task<IActionResult> EmployeeDashBoard()
        {
            
           
            try
            {
                var LoginuserIdd = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var userExists = employeeRepo.EmployeeUserIdExists(LoginuserIdd!);
                if (userExists)
                {
                    var employee = await employeeRepo.GetEmployeeDetails(LoginuserIdd!);
                    return View(employee);
                }
                else
                {
                    return NotFound();
                }
                
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
           
        }

        [HttpGet]
        public IActionResult EmployeeRegistration()
        {
            var LoginuserIdd = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userExists = employeeRepo.EmployeeUserIdExists(LoginuserIdd!);
            if (userExists)
            {
                return RedirectToAction("EmployeeDashBoard", "Employee");
            }
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
                        JoiningDate = joinDate,
                        EmployeeDesignation = evm.EmployeeDesignation,
                        EmployeeAddress = evm.EmployeeAddress
                      


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
            string filename = null!;
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

        //get employee ongoing projects
        public async Task<IActionResult> EmpOngoingProjects()
        {
            try
            {
                var LoginuserIdd = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var projects = await projectRepo.GetEmployeeOngoingProjects(LoginuserIdd!);
                if(projects.Any())
                {
                    return View(projects);
                }
                else
                {
                    return RedirectToAction("NoProjectForEmployee");
                }
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        //get employee completed projects

        public async Task<IActionResult> EmpCompletedProjects()
        {
            try
            {
                var LoginuserIdd = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var projects = await projectRepo.GetEmployeeCompletedProjects(LoginuserIdd!);
                if(projects.Any())
                {
                    return View(projects);
                }
                else
                {
                    return RedirectToAction("NoProjectForEmployee");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public IActionResult NoProjectForEmployee()
        {
            return View();
        }

    }
}
