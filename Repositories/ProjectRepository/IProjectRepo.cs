using CRM.Models.Domain;

namespace CRM.Repositories.ProjectRepository
{
    public interface IProjectRepo
    {
        Task AddProject(Project project);
        Task<IEnumerable<Project>> GetAllProjects();
        Task<Project> GetProject(Guid id);
        //assign employees to project
        Task AddEmployeeProject(Guid projectId, List<Guid> employeeIds);
        Task AssignProjectManagerAsync(Guid projectId, Guid projectManagerId);
        Task<bool> ManagerIdExists(Guid _projectid);
        Task UnassignProjectManager(Guid _ProjectId);
        Task<Project> GetSingleProjectDetails(Guid projectId);
        Task UnassignAllEmployeesFromProject(Guid projectId);
        Task AssignProjectClientAsync(Guid projectId, Guid clientId);
        Task<bool> ClientIdExists(Guid _projectid);
        Task unassignProjectClientAsync(Guid projectId);
        Task<bool> AreEmployeesAssignedToProject(Guid projectId);
        Task AddIncrementToProject(int increment, Guid projectId);
        Task ProjectActivate(Guid projectId);
        Task ProjectDeactivate(Guid projectId);
        Task<IEnumerable<Project>> GetOngoingProjects();
        Task<IEnumerable<Project>> GetCompletedProjects();
        Task<IEnumerable<Project>> GetEmployeeOngoingProjects(string empUserId);
        Task<IEnumerable<Project>> GetEmployeeCompletedProjects(string empUserId);
    }
}
