using AutoMapper;
using Jeremy.IdentityCenter.Database.Common.Extensions;
using Jeremy.IdentityCenter.Database.Common.Models;
using Jeremy.IdentityCenter.Database.DbContexts;
using Jeremy.IdentityCenter.Database.Entities;
using Jeremy.IdentityCenter.Database.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Jeremy.IdentityCenter.Database.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        public RoleManager<IdcIdentityRole> RoleManager { get; }
        public IMapper Mapper { get; }

        public RoleRepository(IdcIdentityDbContext db, ILogger<IRoleRepository> logger, RoleManager<IdcIdentityRole> roleManager, IMapper mapper)
        {
            RoleManager = roleManager;
            Mapper = mapper;
        }

        public async Task<IdcIdentityRole> GetAsync(int id)
        {
            return await RoleManager.Roles.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<List<IdcIdentityRole>> GetRangeAsync()
        {
            return await RoleManager.Roles.OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<PageList<IdcIdentityRole>> GetRangeAsync(string search, int page = 1, int pageSize = 10)
        {
            Expression<Func<IdcIdentityRole, bool>> condition = x => x.Name.Contains(search);
            return RoleManager.Roles.WhereIf(!string.IsNullOrWhiteSpace(search), condition)
                .PageBy(x => x.Name, page, pageSize)
                .ToPageList(await RoleManager.Roles.WhereIf(!string.IsNullOrWhiteSpace(search), condition).CountAsync(),
                    pageSize);
        }

        public async Task<bool> DeleteAsync(IdcIdentityRole role)
        {
            var target = await RoleManager.FindByIdAsync(role.Id.ToString());
            return (await RoleManager.DeleteAsync(target)).Succeeded;
        }

        public async Task<bool> AddAsync(IdcIdentityRole role)
        {
            return (await RoleManager.CreateAsync(role)).Succeeded;
        }

        public async Task<bool> UpdateAsync(IdcIdentityRole role)
        {
            var origin = await RoleManager.FindByIdAsync(role.Id.ToString());
            Mapper.Map(role, origin);
            return (await RoleManager.UpdateAsync(origin)).Succeeded;
        }
    }
}