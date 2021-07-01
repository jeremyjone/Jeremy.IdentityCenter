using Jeremy.IdentityCenter.Business.Services.Interfaces;
using Jeremy.IdentityCenter.Database.Common.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace Jeremy.IdentityCenter.Business.Services
{
    public class BaseService<TRepository, TService> : IBaseService
        where TRepository : IRepository
        where TService : IBaseService
    {
        public TRepository Repository { get; }
        public ILogger<TService> Logger { get; }

        public BaseService(TRepository repository, ILogger<TService> logger)
        {
            Repository = repository;
            Logger = logger;
        }
    }
}