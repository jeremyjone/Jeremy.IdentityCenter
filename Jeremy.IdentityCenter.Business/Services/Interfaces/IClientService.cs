using Jeremy.IdentityCenter.Business.Models;
using Jeremy.IdentityCenter.Business.Models.Clients;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jeremy.IdentityCenter.Business.Services.Interfaces
{
    public interface IClientService : IBaseService
    {
        Task<ClientViewModel> GetClientAsync(int clientId);

        Task<ClientsViewModel> GetClientsAsync(string search, int page = 1, int pageSize = 10);

        Task<ClientViewModel> AddClientAsync(ClientViewModel client);

        Task<ClientViewModel> UpdateClientAsync(ClientViewModel client);

        ClientViewModel RemoveClient(ClientViewModel client);






        Task<ClientSecretsViewModel> GetClientSecretAsync(int secretId);

        Task<ClientSecretsViewModel> GetClientSecretsAsync(int clientId, int page, int pageSize = 10);

        Task<ClientSecretsViewModel> AddClientSecretAsync(ClientSecretsViewModel secrets);

        Task<ClientSecretsViewModel> RemoveClientSecretAsync(ClientSecretsViewModel secret);





        Task<ClientPropertiesViewModel> GetClientPropertyAsync(int propertyId);

        Task<ClientPropertiesViewModel> AddClientPropertyAsync(ClientPropertiesViewModel properties);

        Task<ClientPropertiesViewModel> RemoveClientPropertyAsync(ClientPropertiesViewModel property);

        Task<ClientPropertiesViewModel> GetClientPropertiesAsync(int clientId, int page, int pageSize = 10);



        Task<ClientClaimsViewModel> GetClientClaimAsync(int claimId);

        Task<ClientClaimsViewModel> AddClientClaimAsync(ClientClaimsViewModel claims);

        Task<ClientClaimsViewModel> RemoveClientClaimAsync(ClientClaimsViewModel claim);

        Task<ClientClaimsViewModel> GetClientClaimsAsync(int clientId, int page, int pageSize = 10);




        Task<List<string>> GetScopesAsync(string scope, int count = 0);

        List<string> GetGrantTypes(string type, int count = 0);


        List<string> GetSigningAlgorithms(string algorithm, int count = 0);

        List<SelectItemViewModel> GetAccessTokenTypes();

        List<SelectItemViewModel> GetTokenExpirations();

        List<SelectItemViewModel> GetTokenUsage();

        List<SelectItemViewModel> GetHashTypes();

        List<SelectItemViewModel> GetSecretTypes();

        List<SelectItemViewModel> GetProtocolTypes();

        List<string> GetStandardClaims(string claim, int count);

        Task<ClientViewModel> CloneClientAsync(ClientCloneViewModel client);

        ClientViewModel BuildClientViewModel(ClientViewModel client = null);
        ClientSecretsViewModel BuildClientSecretsViewModel(ClientSecretsViewModel secrets);
    }
}
