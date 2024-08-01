using CRM.DB_Context;
using CRM.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace CRM.Repositories.AdminRepository
{
    public class AdminRepo : IAdminRepo
    {
        private readonly DataContext dbcontext;

        public AdminRepo(DataContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }
        public bool AdminUserExists(string AdminUserId)
        {
            return dbcontext.AdminTable.Any(e => e.Admin_UserId == AdminUserId);
        }

        //add admin registration form 
        public async Task AddAdmin(Admin _admin)
        {
            await dbcontext.AdminTable.AddAsync(_admin);
            await dbcontext.SaveChangesAsync();
        }

        //get admin registration details
        public async Task<Admin> GetAdminRegistrationDetails(string  AdminUserId)
        {
            if(!AdminUserExists(AdminUserId))
            {
                return null;
            }
            else
            {
                var _admin = await dbcontext.AdminTable.FirstOrDefaultAsync(a => a.Admin_UserId == AdminUserId);
                return _admin!;
            }
        }
    }
}
