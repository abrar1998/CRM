using CRM.Models.Domain;

namespace CRM.Repositories.ClientRepository
{
    public interface IClientRepo
    {
        bool ClientUserIdExists(string ClientUserId);

        Task AddClient(Client _client);

        Task<Client> GetCurrentClientDetails(string _userId);
        Task<IEnumerable<Client>> GetAllClientAsync();
        Task<Client> GetClientAsync(Guid id);
    }
}
