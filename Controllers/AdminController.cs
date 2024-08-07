using CRM.DB_Context;
using CRM.Models.Domain;
using CRM.Models.Dto;
using CRM.Models.Registration;
using CRM.Repositories.AdminRepository;
using CRM.Repositories.ClientRepository;
using CRM.Repositories.EmployeeRepository;
using CRM.Repositories.ProjectRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CRM.Controllers
{
    [Authorize(Roles =RolesClass.Admin)]
    public class AdminController : Controller
    {
        private readonly DataContext dataContext;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IAdminRepo adminRepo;
        private readonly IEmployeeRepo employeeRepo;
        private readonly IClientRepo clientRepo;
        private readonly IProjectRepo projectRepo;

        public AdminController(DataContext dataContext, IWebHostEnvironment webHostEnvironment,
            IAdminRepo adminRepo, IEmployeeRepo employeeRepo, IClientRepo clientRepo, IProjectRepo projectRepo)
        {
            this.dataContext = dataContext;
            this.webHostEnvironment = webHostEnvironment;
            this.adminRepo = adminRepo;
            this.employeeRepo = employeeRepo;
            this.clientRepo = clientRepo;
            this.projectRepo = projectRepo;
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
                if(data !=null)
                    return View(data);
                return RedirectToAction("NoEmployeeFound");
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
        //get all projects
        public async Task<IActionResult> GetAllProjects()
        {
            try
            {
              
                var projects = await projectRepo.GetAllProjects();
                if(projects.Any())
                    return View(projects);
                return RedirectToAction("NoProjectsFound");
            }
            catch(Exception exp)
            {
                return BadRequest("Failed to fetch projects \n" + exp.Message);
            }
            
        }

        public IActionResult NoProjectsFound()
        {
            return View();
        }
        //Project Registration
        [HttpGet]
        public IActionResult RegisterProject()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCourse(ProjectViewModel pvm)
        {
            if (ModelState.IsValid)
            {

                if (User.IsInRole(RolesClass.Admin))
                {
                   

                    string[] allowedExtensions = { ".jpg", ".jpeg", ".png", }; // Add more extensions if needed

                    string photoExtension = Path.GetExtension(pvm.ProjectPhoto.FileName).ToLower();

                    if (pvm.ProjectPhoto.Length > 400 * 1024) // 400 KB limit
                    {
                        ModelState.AddModelError("", " Profile  Photo Should Be Less Than 300kb");
                        return BadRequest("Profile  Photo Should Be Less Than 300kb");
                    }


                    if (!allowedExtensions.Contains(photoExtension))
                    {
                        //ModelState.AddModelError("", "Invalid photo file extension. Allowed extensions are .jpg, .jpeg,");
                        return BadRequest("Invalid photo file extension. Allowed extensions are .jpg, .jpeg");
                    }

                    string FileLocation = UploadFile(pvm);

                    var _project = new Project()
                    {
                        ProjectCompanyName = pvm.ProjectCompanyName,
                        ProjectName = pvm.ProjectName,
                        ProjectPhotoPath = FileLocation,
                        Description = pvm.Description,
                        Year = pvm.Year,
                    };

                    try
                    {
                       await projectRepo.AddProject(_project);
                       return Ok("Project Added Successfully");
                        
                    }
                    catch (Exception ex)
                    {
                        return BadRequest("Failed to create project \n" +ex.Message);
                    }

                }
            }
            return BadRequest(ModelState);
        }


        //upload photo
        private string UploadFile(ProjectViewModel pvm)
        {
            string filename = null!;
            if (pvm.ProjectPhoto != null && pvm.ProjectPhoto.Length < 1048576)
            {
                string filedir = Path.Combine(webHostEnvironment.WebRootPath, "Images");
                filename = Guid.NewGuid().ToString() + "-" + pvm.ProjectPhoto.FileName;
                string filepath = Path.Combine(filedir, filename);
                using (var filestream = new FileStream(filepath, FileMode.Create))
                {
                    pvm.ProjectPhoto.CopyTo(filestream);
                }
            }
            return filename;
        }

        public async Task<IActionResult> ManageProject(Guid projectId)
        {
            try
            {
                var project = await projectRepo.GetProject(projectId);
                if (project == null)
                {
                    // Handle the case where the project is not found
                    // For example, you can return a NotFound result or a custom error view
                    return NotFound();
                }

                string ManagerName = null;
                var proj = await projectRepo.GetProject(projectId);
                var managerIdexists = await projectRepo.ManagerIdExists(projectId);
                if(managerIdexists)
                {
                    //var proj = await projectRepo.GetProject(projectId);
                    var managerId = proj.ProjectManagerId;
                    if(managerId.HasValue)
                    {
                        var employeeDetails = await employeeRepo.GetEmployee(managerId.Value);
                        ManagerName = employeeDetails.Name;
                    }
                }
                //check client id exists in project
                string ClientName = null;
                var clientExists = await projectRepo.ClientIdExists(projectId);
                if (clientExists) 
                {
                    var clientId = proj.C_id;
                    if(clientId.HasValue)
                    {
                        var clientDetails = await clientRepo.GetClientAsync(clientId.Value);
                        ClientName = clientDetails.Name;
                    }
                }

                var employees = await employeeRepo.GetAllEmployees();
                var clients = await clientRepo.GetAllClientAsync();
                var teamExists = await projectRepo.AreEmployeesAssignedToProject(projectId);

                var viewModel = new AssignEmployeesViewModel
                {
                    ProjectId = projectId,
                    ProjectName = project.ProjectName,
                    Employees = employees,
                    ProjectManagerIdExists = managerIdexists,
                    ProjectManagerName = ManagerName,
                    Clients = clients,
                    ProjectClientExists = clientExists,
                    ProjectClientName = ClientName,
                    TeamExists = teamExists
                };

                return View(viewModel);
            }
            catch(Exception exp)
            {
                return BadRequest(exp.Message);
            }
        }

        //assign project team
        [HttpPost]
        public IActionResult AssignEmployees(Guid projectId, List<Guid> employeeIds)
        {
            if (employeeIds == null)
            {
                return BadRequest("Please Select Any Option");
            }


            try
            {
                projectRepo.AddEmployeeProject(projectId, employeeIds);
                return Json(new { success = true });
            }
            catch(Exception exp)
            {
                return BadRequest(exp.Message);
            }
            
            
        }

        //assign project manager

        [HttpPost]
        public async Task<IActionResult> AssignProjectManager(Guid projectId, Guid projectManagerId)
        {
            try
            {
                await projectRepo.AssignProjectManagerAsync(projectId, projectManagerId);
                return Json(new { success = true });
            }
            catch(Exception exp)
            {
                return BadRequest(exp.Message);
            }
            
        }
        //assign project client
        [HttpPost]
        public async Task<IActionResult> AssignProjectClient(Guid projectId, Guid projectClientId)
        {
            try
            {
                await projectRepo.AssignProjectClientAsync(projectId, projectClientId);
                return Json(new { success = true });
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UnassignProjectManager(Guid projectId)
        {
            try
            {
                await projectRepo.UnassignProjectManager(projectId);
                // Return a success response
                return Ok(new { message = "Project manager unassigned successfully" });
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UnassignProjectClient(Guid projectId)
        {
            try
            {
                await projectRepo.unassignProjectClientAsync(projectId);
                return Ok(new { message = "Project client unassigned successfully" });
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //
        [HttpPost]
        public async Task<IActionResult> UnassignProjectTeam(Guid projectId)
        {
            try
            {
                await projectRepo.UnassignAllEmployeesFromProject(projectId);
                return Ok(new { message = "Project team unassigned successfully" });
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //messages section for admin
        public IActionResult Messages()
        {
            return View();
        }
      



    }
}
