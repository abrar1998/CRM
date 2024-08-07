using CRM.Models.Registration;
using CRM.Repositories.AdminRepository;
using CRM.Repositories.ClientRepository;
using CRM.Repositories.EmployeeRepository;
using CRM.Repositories.RegistrationRepo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Security.Claims;

namespace CRM.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountRepo repo;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IAdminRepo adminRepo;
        private readonly IEmployeeRepo employeeRepo;
        private readonly IClientRepo clientRepo;

        public AccountController(IAccountRepo repo, RoleManager<IdentityRole> roleManager, IAdminRepo adminRepo,
            IEmployeeRepo employeeRepo, IClientRepo clientRepo)
        {
            this.repo = repo;
            this.roleManager = roleManager;
            this.adminRepo = adminRepo;
            this.employeeRepo = employeeRepo;
            this.clientRepo = clientRepo;
        }

       //[Authorize(Roles = RolesClass.Admin)]
        [HttpGet]
        public IActionResult RegisterUser()
        {
            return View();
        }

        //submitting form using ajax;
       // [Authorize(Roles = RolesClass.Admin)]
        [HttpPost]
        public async Task<IActionResult> CreateAccount(UserSignUpModel umodel)
        {
            var result = await repo.UserRegisteration(umodel);
            if (!result.Succeeded)
            {
                var errorList = result.Errors.Select(e => e.Description);
                return BadRequest(string.Join("; ", errorList));
            }
            else
            {
              /*  try
                {
                   // emailService.SendRegistrationEmail(umodel.Email, umodel.Name, umodel.Password);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                    //return RedirectToAction("EmailErrorAction", "Account", new { message = ex.Message });
                }*/
                return Ok("Account Created Successfully!");
            }

        }



        [HttpGet]
        public IActionResult LoginUser()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> LoginUser(UserSigninModel lmodel)
        {
            if (ModelState.IsValid)
            {
                var result = await repo.UserLogin(lmodel);
                if (result.Succeeded)
                {
                    if (User.IsInRole(RolesClass.Admin))
                    {
                        var adminUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                        if(adminRepo.AdminUserExists(adminUserId!))
                        {
                            return RedirectToAction("AdminDashBoard", "Admin");
                        }
                        return RedirectToAction("AdminRegistration", "Admin");
                    }
                    else if (User.IsInRole(RolesClass.Employee))
                    {
                         var empUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                        if(employeeRepo.EmployeeUserIdExists(empUserId!))
                        {
                            return RedirectToAction("EmployeeDashBoard", "Employee");
                        }
                        else
                        {
                            return RedirectToAction("EmployeeRegistration", "Employee");
                        }
                    }
                    else if (User.IsInRole(RolesClass.Client))
                    {
                        var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
                        
                        if (clientRepo.ClientUserIdExists(userid!))
                        {
                            return RedirectToAction("ClientDashBoard", "Client");
                        }
                        else
                        {
                            return RedirectToAction("ClientRegistration", "Client");
                        }
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Credentials");
                }
            }
            else
            {
                ModelState.Clear();
            }
            return View();
        }



        [Authorize(Roles = "Admin, Client, Employee")]
        [HttpGet]
        public IActionResult LogoutUser()
        {
            repo.LogOut();
            return RedirectToAction("LoginUser", "Account");
        }


        //adding roles in the database
      /*  public string AddRoles()
        {
            roleManager.CreateAsync(new IdentityRole(RolesClass.Admin)).GetAwaiter().GetResult();
            roleManager.CreateAsync(new IdentityRole(RolesClass.Employee)).GetAwaiter().GetResult();
            roleManager.CreateAsync(new IdentityRole(RolesClass.Client)).GetAwaiter().GetResult();

            return "Roles Added Successfully";
        }*/

    }
}
