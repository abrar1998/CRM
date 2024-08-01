using CRM.DB_Context;

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
    }
}
