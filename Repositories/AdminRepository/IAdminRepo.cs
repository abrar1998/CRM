using CRM.Models.Domain;

namespace CRM.Repositories.AdminRepository
{
    public interface IAdminRepo
    {
        bool AdminUserExists(string AdminUserId);
        Task AddAdmin(Admin _admin);
        Task<Admin> GetAdminRegistrationDetails(string AdminUserId);
    }
}
