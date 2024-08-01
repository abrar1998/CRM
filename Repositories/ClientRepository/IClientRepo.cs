namespace CRM.Repositories.ClientRepository
{
    public interface IClientRepo
    {
        bool ClientUserIdExists(string ClientUserId);
    }
}
