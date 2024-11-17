using ProjectPortfolio.Data;
using ProjectPortfolio.Models;

namespace ProjectPortfolio.Services
{
    internal class ClientProjectService(ClientRepository clienRepository) : IClientProjectService
    {

        public async Task<ClientProjectModel> CreateAsync(ClientProjectModel model)
        {
            model.Client = null;
            return model;
        }

        public async Task<ClientProjectModel> UpdateAsync(ClientProjectModel model)
        {
            return model;
        }
    }
}
