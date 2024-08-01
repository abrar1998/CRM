using CRM.DB_Context;
using CRM.Models.Domain;
using CRM.Models.Dto;
using CRM.Models.Registration;
using CRM.Repositories.AdminRepository;
using CRM.Repositories.ClientRepository;
using CRM.Repositories.EmployeeRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CRM.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly DataContext dataContext;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IAdminRepo adminRepo;
        private readonly IEmployeeRepo employeeRepo;
        private readonly IClientRepo clientRepo;

        public AdminController(DataContext dataContext, IWebHostEnvironment webHostEnvironment,
            IAdminRepo adminRepo, IEmployeeRepo employeeRepo, IClientRepo clientRepo)
        {
            this.dataContext = dataContext;
            this.webHostEnvironment = webHostEnvironment;
            this.adminRepo = adminRepo;
            this.employeeRepo = employeeRepo;
            this.clientRepo = clientRepo;
        }
        public IActionResult AdminDashBoard()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AdminRegistration()
        {
            return View();
        }

        [HttpGet]
        public IActionResult TotalSales()
        {
            return View();
        }

        [HttpGet]
        public IActionResult MonthlySales()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> AdminRegistration(AdminViewModel adminViewModel)
        {
            var LoginuserIdd = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userExists = adminRepo.AdminUserExists(LoginuserIdd!);
            if (userExists)
            {
                return RedirectToAction("AdminDashBoard", "Admin");
            }

            if (ModelState.IsValid)
            {

                if (User.IsInRole(RolesClass.Admin))
                {
                    var LoginuserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
                    //var CurrentUser = await userManager.GetUserAsync(User);

                    string[] allowedExtensions = { ".jpg", ".jpeg", ".png", }; // Add more extensions if needed

                    string photoExtension = Path.GetExtension(adminViewModel.AdminPhoto.FileName).ToLower();

                    if (adminViewModel.AdminPhoto.Length > 400 * 1024) // 400 KB limit
                    {
                        ModelState.AddModelError("", " Profile  Photo Should Be Less Than 400kb");
                        return View(adminViewModel);
                    }


                    if (!allowedExtensions.Contains(photoExtension))
                    {
                        ModelState.AddModelError("", "Invalid photo file extension. Allowed extensions are .jpg, .jpeg,");
                        return View(adminViewModel);
                    }

                    string FileLocation = UploadFile(adminViewModel);

                    DateOnly joinDate;
                    if(adminViewModel.JoiningDate == null)
                    {
                        joinDate = DateOnly.FromDateTime(DateTime.UtcNow.Date);
                    }
                    else
                    {
                        joinDate = (DateOnly)adminViewModel.JoiningDate;
                    }

                    var AdminData = new Admin()
                    {
                       Name = adminViewModel.Name,
                       PhoneNumber = adminViewModel.PhoneNumber,
                       PhotoPath= FileLocation,
                       Email= adminViewModel.Email,
                       Admin_UserId= LoginuserId,
                       JoiningDate = adminViewModel.JoiningDate
                       

                    };
                    await adminRepo.AddAdmin(AdminData);
                    TempData["Registered"] = true;
                    return RedirectToAction("AdminDashBoard", "Admin");

                }
            }
            return View(adminViewModel);
        }



        //file upload
        private string UploadFile(AdminViewModel avm)
        {
            string filename = null!;
            if (avm.AdminPhoto != null && avm.AdminPhoto.Length < 1048576)
            {
                string filedir = Path.Combine(webHostEnvironment.WebRootPath, "Images");
                filename = Guid.NewGuid().ToString() + "-" + avm.AdminPhoto.FileName;
                string filepath = Path.Combine(filedir, filename);
                using (var filestream = new FileStream(filepath, FileMode.Create))
                {
                    avm.AdminPhoto.CopyTo(filestream);
                }
            }
            return filename;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            try
            {
                var employeesList = await employeeRepo.GetAllEmployees();
                if (employeesList == null) 
                {
                    return RedirectToAction("NoEmployeeFound");
                }
                return View(employeesList);
            }
            catch(Exception ex) 
            {
                return BadRequest("Failed to fetch employees list " + ex.Message);

            }
           
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployeeDetails(Guid id)
        {
            try
            {
                var data = await employeeRepo.GetEmployee(id);
                return View(data);
            }
            catch(Exception exp)
            {
                return BadRequest("Failed to fetch employee details \n" + exp.Message);
            }
            
        }

        [HttpGet]
        public async Task<IActionResult> GetAllClients()
        {
            try
            {
                var _clientData = await clientRepo.GetAllClientAsync();
                if (_clientData.Any()) // Prefer using Any() over Count() for performance
                {
                    return View(_clientData);
                }
                else
                {
                    return RedirectToAction("NoClientFound");
                }
            }
            catch (Exception exp)
            {
                return BadRequest($"Please contact Developer \n {exp.Message}");
            }
        }

        //get single client
        public async Task<IActionResult> GetClientDetails(Guid id)
        {
            try
            {
                var data = await clientRepo.GetClientAsync(id);
                return View(data);
            }
            catch(Exception exp)
            {
                return BadRequest("Failed to load \n" + exp.Message);
            }
        }

        //no client found view
        public IActionResult NoClientFound()
        {
            return View(); 
        }

        //no employee found view

        public IActionResult NoEmployeeFound()
        {
            return View();
        }


    }
}
