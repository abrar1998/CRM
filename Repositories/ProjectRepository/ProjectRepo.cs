using CRM.DB_Context;
using CRM.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace CRM.Repositories.ProjectRepository
{
    public class ProjectRepo : IProjectRepo
    {
        private readonly DataContext dbcontext;

        public ProjectRepo(DataContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public async Task AddProject(Project project)
        {
            await dbcontext.ProjectTable.AddAsync(project);
            await dbcontext.SaveChangesAsync();
        }

        //get all projects
        public async Task<IEnumerable<Project>> GetAllProjects()
        {
            var proj = await dbcontext.ProjectTable.Include(p => p.ProjectClient).Include(p => p.EmployeeProjects).ToListAsync();
            return proj;
        }

        public async Task<Project> GetProject(Guid id)
        {
            var project = await dbcontext.ProjectTable.FindAsync(id);
            return project!;
        }

        //assign team to project
        public async Task AddEmployeeProject(Guid projectId, List<Guid> employeeIds)
        {
            var existingAssignments = dbcontext.EmployeeProjectsTable.Where(ep => ep.ProjectId == projectId);
            dbcontext.EmployeeProjectsTable.RemoveRange(existingAssignments);

            // Add new assignments
            foreach (var employeeId in employeeIds)
            {
                var employeeProject = new EmployeeProject
                {
                    ProjectId = projectId,
                    EmployeeId = employeeId
                };
                await dbcontext.EmployeeProjectsTable.AddAsync(employeeProject);
            }

            await dbcontext.SaveChangesAsync();
        }

        public async Task AssignProjectManagerAsync(Guid projectId, Guid projectManagerId)
        {
            var _project = await dbcontext.ProjectTable.FirstOrDefaultAsync(p => p.ProjectId == projectId);
            _project.ProjectManagerId = projectManagerId;
            await dbcontext.SaveChangesAsync();
        }

        //checking project manager id exists for a project or not
        public async Task<bool> ManagerIdExists(Guid _projectid)
        {
            var project = await dbcontext.ProjectTable.FirstOrDefaultAsync(p => p.ProjectId == _projectid);
            if (project!.ProjectManagerId.HasValue)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //unassign projectmanager
        public async Task UnassignProjectManager(Guid _ProjectId)
        {
            var getemployee = await dbcontext.ProjectTable.FirstOrDefaultAsync(p => p.ProjectId == _ProjectId);
            getemployee!.ProjectManagerId = null;
            await dbcontext.SaveChangesAsync();
        }

        //get project details
        public async Task<Project> GetSingleProjectDetails(Guid projectId)
        {
            var project = await dbcontext.ProjectTable.Include(p => p.EmployeeProjects).ThenInclude(e => e.Employee)
                .FirstOrDefaultAsync(p => p.ProjectId == projectId);
            if (project == null)
            {
                return null;
            }
            return project;
        }

        //unassign team from EmployeeProject table
        public async Task UnassignAllEmployeesFromProject(Guid projectId)
        {
            // Retrieve the records that need to be deleted
            var employeeProjects = dbcontext.EmployeeProjectsTable
                .Where(ep => ep.ProjectId == projectId);

            // Remove the records
            dbcontext.EmployeeProjectsTable.RemoveRange(employeeProjects);

            // Save changes to the database
            await dbcontext.SaveChangesAsync();
        }

        //assign project client
        public async Task AssignProjectClientAsync(Guid projectId, Guid clientId)
        {
            var proj = await dbcontext.ProjectTable.FirstOrDefaultAsync(f => f.ProjectId == projectId);
            if (proj != null)
            {
                proj.C_id = clientId;
                await dbcontext.SaveChangesAsync();
            }
        }

        //checking project client id exists
        public async Task<bool> ClientIdExists(Guid _projectid)
        {
            var project = await dbcontext.ProjectTable.FirstOrDefaultAsync(p => p.ProjectId == _projectid);
            if (project!.C_id.HasValue)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //unassing project client
        public async Task unassignProjectClientAsync(Guid projectId)
        {
            var proj = await dbcontext.ProjectTable.FirstOrDefaultAsync(f => f.ProjectId == projectId);
            if (proj != null)
            {
                proj.C_id = null;
                await dbcontext.SaveChangesAsync();
            }
        }

        //
        public async Task<bool> AreEmployeesAssignedToProject(Guid projectId)
        {
            // Check if there are any records in EmployeeProjectTable with the given ProjectId
            var employeesAssigned = await dbcontext.EmployeeProjectsTable
                .AnyAsync(ep => ep.ProjectId == projectId);

            return employeesAssigned;
        }

        public async Task AddIncrementToProject(int increment, Guid projectId)
        {
            var proj = await dbcontext.ProjectTable.FirstOrDefaultAsync(p => p.ProjectId == projectId);
            proj!.ProgressPercentage = increment;
            await dbcontext.SaveChangesAsync();
        }

        //active project
        public async Task ProjectActivate(Guid projectId)
        {
            var proj = await dbcontext.ProjectTable.FirstOrDefaultAsync(p => p.ProjectId == projectId);
            proj.ProjectStatus = 1;
            await dbcontext.SaveChangesAsync();
        }

        public async Task ProjectDeactivate(Guid projectId)
        {
            var proj = await dbcontext.ProjectTable.FirstOrDefaultAsync(p => p.ProjectId == projectId);
            proj.ProjectStatus = 0;
            await dbcontext.SaveChangesAsync();
        }


        //get ongoing projects
        public async Task<IEnumerable<Project>> GetOngoingProjects()
        {
            var projects = await dbcontext.ProjectTable.Where(p => p.ProjectStatus == 1).ToListAsync();
            return projects;
        }

        //get completed projects
        public async Task<IEnumerable<Project>> GetCompletedProjects()
        {
            var projects = await dbcontext.ProjectTable.Where(p => p.ProjectStatus == 0).ToListAsync();
            return projects;
        }

        //get employee on going  projects
        public async Task<IEnumerable<Project>> GetEmployeeOngoingProjects(string empUserId)
        {
            var employee = await dbcontext.EmployeesTable.FirstOrDefaultAsync(e => e.Emp_UserId == empUserId);

            // Get project IDs for this employee
            var projectIds = await dbcontext.EmployeeProjectsTable
                .Where(ep => ep.EmployeeId == employee!.EmployeeId)
                .Select(ep => ep.ProjectId)
                .ToListAsync();

            var projects = await dbcontext.ProjectTable
                .Where(p => projectIds.Contains(p.ProjectId) && p.ProjectStatus ==1)
                .ToListAsync();
            return projects;

        }

        //get employee completed projects
        //get employee projects
        public async Task<IEnumerable<Project>> GetEmployeeCompletedProjects(string empUserId)
        {
            var employee = await dbcontext.EmployeesTable.FirstOrDefaultAsync(e => e.Emp_UserId == empUserId);

            // Get project IDs for this employee
            var projectIds = await dbcontext.EmployeeProjectsTable
                .Where(ep => ep.EmployeeId == employee!.EmployeeId)
                .Select(ep => ep.ProjectId)
                .ToListAsync();

            var projects = await dbcontext.ProjectTable
                .Where(p => projectIds.Contains(p.ProjectId) && p.ProjectStatus == 0)
                .ToListAsync();
            return projects;

        }


    }
}
