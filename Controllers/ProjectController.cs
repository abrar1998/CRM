using CRM.Models.Domain;
using CRM.Models.Dto;
using CRM.Models.Registration;
using CRM.Repositories.ClientRepository;
using CRM.Repositories.EmployeeRepository;
using CRM.Repositories.ProjectRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace CRM.Controllers
{
    [Authorize(Roles =RolesClass.Admin)]
    public class ProjectController : Controller
    {
        private readonly IEmployeeRepo employeeRepo;
        private readonly IProjectRepo projectRepo;
        private readonly IClientRepo clientRepo;

        public ProjectController(IEmployeeRepo employeeRepo, IProjectRepo projectRepo, IClientRepo clientRepo)
        {
            this.employeeRepo = employeeRepo;
            this.projectRepo = projectRepo;
            this.clientRepo = clientRepo;
        }




        public async Task<IActionResult> ProjectDetails(Guid projectId)
        {
            try
            {
                var project = await projectRepo.GetSingleProjectDetails(projectId);

                if (project == null)
                {
                    return NotFound();
                }

                Employee projectManager = null;
                if (project.ProjectManagerId.HasValue)
                {
                    projectManager = await employeeRepo.GetEmployee(project.ProjectManagerId.Value);
                }

                var teamMembers = project.EmployeeProjects
                    .Select(ep => ep.Employee);

                string _clienName =null;
                if(project.C_id !=null)
                {
                    var client = await clientRepo.GetClientAsync(project.C_id.Value);
                    _clienName = client.Name;
                }
                var viewModel = new ProjectDetailsViewModel
                {
                    Project = project,
                    TeamMembers = teamMembers,
                    ProjectManager = projectManager,
                    ProjectClient = _clienName
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ProjectCompletionPercentage(Guid projectId)
        {
            try
            {
                
                var proj = await projectRepo.GetProject(projectId);
                if(proj !=null)
                {
                    if (proj.C_id == null)
                    {
                        ViewBag.Client = null;

                    }
                    else
                    {
                        var client = await clientRepo.GetClientAsync(proj.C_id.Value);
                        ViewBag.Client = client.Name;
                    }
                    return View(proj);
                }
                else
                {
                    return NotFound();
                }
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //increase project progress
        [HttpPost]
        public async Task<IActionResult> IncreaseProjectProgress(int perVal, Guid projectId)
        {
            try
            {
                await projectRepo.AddIncrementToProject(perVal, projectId);
                return Json(new { success = true });
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        //deactivate project
        public async Task<IActionResult> ProjectDeactivatee(Guid projectId)
        {
            try
            {
                await projectRepo.ProjectDeactivate(projectId);
                return RedirectToAction("GetAllProjects", "Admin");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public async Task<IActionResult> ProjectAactivatee(Guid projectId)
        {
            try
            {
                await projectRepo.ProjectActivate(projectId);
                return RedirectToAction("GetAllProjects", "Admin");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        //ONGOING PROJECTS
        public async Task<IActionResult> OnGoingProjects()
        {
            try
            {
                var projects = await projectRepo.GetOngoingProjects();
                if(projects.Count() > 0)
                {
                    return View(projects);
                }
                else
                {
                    return RedirectToAction("NoOngiongProject", "Project");
                }
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        //completed projects
        public async Task<IActionResult> CompletedProjects()
        {
            try
            {
                var projects = await projectRepo.GetCompletedProjects();
                if (projects.Count() >0)
                {
                    return View(projects);
                }
                else
                {
                    return RedirectToAction("NoCompletedProject", "Project");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        public IActionResult NoOngiongProject()
        {
            return View();
        }

        public IActionResult NoCompletedProject()
        {
            return View();
        }

    }
}
