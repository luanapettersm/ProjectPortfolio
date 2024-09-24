using ProjectPortfolio.Models;

namespace ProjectPortfolio.Services
{
    public class ClientService : IClientService
    {
        public Task<ClientModel> CreateAsync(ClientModel model)
        {
            throw new NotImplementedException();
        }

        public Task<ClientModel> UpdateAsync(ClientModel model)
        {
            throw new NotImplementedException();
        }

        public Task<ClientModel> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ClientModel> FilterAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ClientModel>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ClientModel> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
