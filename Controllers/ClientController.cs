using CRM.Models.Domain;
using CRM.Models.Dto;
using CRM.Models.Registration;
using CRM.Repositories.ClientRepository;
using CRM.Repositories.EmployeeRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CRM.Controllers
{
    //[Authorize(Roles =RolesClass.Client)]
    public class ClientController : Controller
    {
        private readonly IClientRepo clientRepo;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ClientController(IClientRepo clientRepo, IWebHostEnvironment webHostEnvironment)
        {
            this.clientRepo = clientRepo;
            this.webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> ClientDashBoard()
        {
            // current logged in client details to show on the dashboard
            try
            {
                var _userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var _client = await clientRepo.GetCurrentClientDetails(_userId!);
                if(_client !=null)
                {
                    return View(_client);
                }
                else
                {
                    return NotFound("Client Details not found please contact admin");
                }
    
                
            }
            catch(Exception exp)
            {
                return BadRequest("Failed to load your details please contact admin \n" + exp.Message);
            }
         
        }


        [HttpGet]
        public IActionResult ClientRegistration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ClientRegistration(ClientViewModel cvm)
        {

            var LoginuserIdd = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userExists = clientRepo.ClientUserIdExists(LoginuserIdd!);
            if (userExists)
            {
                return RedirectToAction("ClientDashBoard", "Client");
            }

            if (ModelState.IsValid)
            {

                if (User.IsInRole(RolesClass.Client))
                {
                    var LoginuserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
                    //var CurrentUser = await userManager.GetUserAsync(User);

                    string[] allowedExtensions = { ".jpg", ".jpeg", ".png", }; // Add more extensions if needed

                    string photoExtension = Path.GetExtension(cvm.ClientPhoto.FileName).ToLower();

                    if (cvm.ClientPhoto.Length > 400 * 1024) // 400 KB limit
                    {
                        ModelState.AddModelError("", " Profile  Photo Should Be Less Than 400kb");
                        return View(cvm);
                    }


                    if (!allowedExtensions.Contains(photoExtension))
                    {
                        ModelState.AddModelError("", "Invalid photo file extension. Allowed extensions are .jpg, .jpeg,");
                        return View(cvm);
                    }

                    string FileLocation = UploadFile(cvm);

                    DateOnly joinDate;
                    if (cvm.JoiningDate == null)
                    {
                        joinDate = DateOnly.FromDateTime(DateTime.UtcNow.Date);
                    }
                    else
                    {
                        joinDate = (DateOnly)cvm.JoiningDate;
                    }

                    var _clientData = new Client()
                    {
                        Name = cvm.Name,
                        PhoneNumber = cvm.PhoneNumber,
                        PhotoPath = FileLocation,
                        Email = cvm.Email,
                        Client_UserId = LoginuserId,
                        JoiningDate = joinDate,
                        Address = cvm.Address,
                        Country = cvm.Country,
                        CompanyName = cvm.CompanyName

                    };

                    try
                    {
                        await clientRepo.AddClient(_clientData);
                        TempData["Registered"] = true;
                        return RedirectToAction("ClientDashBoard", "Client");
                    }
                    catch (Exception ex)
                    {
                        return BadRequest("Registration Failed \n" + ex.Message);
                    }

                }
            }
            return View(cvm);
        }

        //upload file
        private string UploadFile(ClientViewModel cvm)
        {
            string filename = null;
            if (cvm.ClientPhoto != null && cvm.ClientPhoto.Length < 1048576)
            {
                string filedir = Path.Combine(webHostEnvironment.WebRootPath, "Images");
                filename = Guid.NewGuid().ToString() + "-" + cvm.ClientPhoto.FileName;
                string filepath = Path.Combine(filedir, filename);
                using (var filestream = new FileStream(filepath, FileMode.Create))
                {
                    cvm.ClientPhoto.CopyTo(filestream);
                }
            }
            return filename;
        }




    }
}
