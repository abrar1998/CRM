using CRM.DB_Context;
using CRM.Models.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CRM.Repositories.ClientRepository
{
    public class ClientRepo : IClientRepo
    {
        private readonly DataContext dbcontext;

        public ClientRepo(DataContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }
        public bool ClientUserIdExists(string ClientUserId)
        {
            return dbcontext.ClientTable.Any(c=>c.Client_UserId == ClientUserId);
        }

        //register client details
        public async Task AddClient(Client _client)
        {
            await dbcontext.ClientTable.AddAsync(_client);
            await dbcontext.SaveChangesAsync();
        }
        //get current logged in clients details through Client user id, //used in ClientMiddleware
        public async Task<Client> GetCurrentClientDetails(string _userId)
        {
            var details = await dbcontext.ClientTable.FirstOrDefaultAsync(c=>c.Client_UserId == _userId);
            return details!;
        }

        //get all clients for admin to display in his panel
        public async Task<IEnumerable<Client>> GetAllClientAsync()
        {
            var _clients = await dbcontext.ClientTable.ToListAsync();
            return _clients;
        }

        //get client 
        public async Task<Client> GetClientAsync(Guid id)
        {
            var data = await dbcontext.ClientTable.Where(c => c.ClientId == id).FirstOrDefaultAsync();
            return data!;
        }
    }
}
