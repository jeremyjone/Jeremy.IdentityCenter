using AutoMapper;
using IdentityModel;
using Jeremy.IdentityCenter.Database.Common.Extensions;
using Jeremy.IdentityCenter.Database.Common.Models;
using Jeremy.IdentityCenter.Database.Common.Repositories;
using Jeremy.IdentityCenter.Database.Constants;
using Jeremy.IdentityCenter.Database.DbContexts;
using Jeremy.IdentityCenter.Database.Entities;
using Jeremy.IdentityCenter.Database.Helper;
using Jeremy.IdentityCenter.Database.Models;
using Jeremy.IdentityCenter.Database.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Jeremy.IdentityCenter.Database.Repositories
{
    public class UserRepository : Repository<IdcIdentityDbContext, IUserRepository>, IUserRepository
    {
        public UserManager<IdcIdentityUser> UserManager { get; }
        public IMapper Mapper { get; }

        public UserRepository(IdcIdentityDbContext db, ILogger<IUserRepository> logger,
            UserManager<IdcIdentityUser> userManager, IMapper mapper) : base(db, logger)
        {
            UserManager = userManager;
            Mapper = mapper;
        }

        public async Task<PageList<IdcIdentityUser>> GetRangeAsync(string search, int page = 1, int pageSize = 10)
        {
            Expression<Func<IdcIdentityUser, bool>> expression = x =>
                x.UserName.Contains(search) || x.Email.Contains(search) || x.NickName.Contains(search);
            return UserManager.Users.WhereIf(!string.IsNullOrWhiteSpace(search), expression)
                .PageBy(x => x.Id, page, pageSize)
                .ToPageList(
                    await UserManager.Users.WhereIf(!string.IsNullOrWhiteSpace(search), expression).CountAsync(),
                    pageSize);
        }

        public async Task<IdcIdentityUser> GetAsync(int id)
        {
            return await UserManager.FindByIdAsync(id.ToString());
        }

        public async Task<bool> AddAsync(IdcIdentityUser user)
        {
            if (await UserManager.FindByNameAsync(user.UserName) != default) return false;

            Logger.LogInformation($"Adding {user.UserName} user...");
            if (string.IsNullOrWhiteSpace(user.NickName))
            {
                user.NickName = Guid.NewGuid().ToString().ToUpper();
            }

            var res = await UserManager.CreateAsync(user, Configuration.DefaultPassword);
            if (!res.Succeeded)
            {
                Logger.LogInformation(res.Errors.First().Description);
                return res.Succeeded;
            }

            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Name, user.NormalizedUserName),
                new Claim(JwtClaimTypes.Email, user.Email)
            };

            Logger.LogInformation("Adding claims to user...");
            res = await UserManager.AddClaimsAsync(user, claims);
            if (!res.Succeeded)
            {
                Logger.LogInformation(res.Errors.First().Description);
            }

            // 添加角色，没有给定的话，使用默认的
            var roleList = user.UserRoles;
            if (user.UserRoles == null || user.UserRoles.Count == 0)
            {
                roleList = Configuration.DefaultRole;
            }
            var roles = roleList.Select(x => x.Role.Name).ToList();
            Logger.LogInformation("Adding roles to user...");
            res = await UserManager.AddToRolesAsync(user, roles);
            if (!res.Succeeded)
            {
                Logger.LogInformation(res.Errors.First().Description);
            }

            return true;
        }

        public async Task<bool> UpdateAsync(IdcIdentityUser user)
        {
            var origin = await UserManager.FindByIdAsync(user.Id.ToString());
            Mapper.Map(user, origin);
            return (await UserManager.UpdateAsync(origin)).Succeeded;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await UserManager.FindByIdAsync(id.ToString());
            return (await UserManager.DeleteAsync(user)).Succeeded;
        }


        public async Task<List<UserLoginInfo>> GetUserProvidersAsync(int id)
        {
            var user = await UserManager.FindByIdAsync(id.ToString());
            return (await UserManager.GetLoginsAsync(user)).ToList();
        }

        public async Task<IdentityUserLogin<int>> GetUserProviderAsync(int id, string providerKey)
        {
            return await Db.UserLogins.FirstOrDefaultAsync(x =>
                x.UserId.Equals(id) && x.ProviderKey.Equals(providerKey));
        }

        public async Task<bool> DeleteUserProviderAsync(int userId, string providerKey, string loginProvider)
        {
            var user = await UserManager.FindByIdAsync(userId.ToString());
            var login = await Db.UserLogins.FirstOrDefaultAsync(x =>
                x.UserId.Equals(userId)
                && x.ProviderKey.Equals(providerKey)
                && x.LoginProvider.Equals(loginProvider));
            return (await UserManager.RemoveLoginAsync(user, login.LoginProvider, login.ProviderKey)).Succeeded;
        }

        public async Task<bool> ChangePasswordAsync(int userId, string password)
        {
            var user = await UserManager.FindByIdAsync(userId.ToString());
            var token = await UserManager.GeneratePasswordResetTokenAsync(user);
            return (await UserManager.ResetPasswordAsync(user, token, password)).Succeeded;
        }

        public virtual List<SelectItem> GetSexTypes()
        {
            var hashTypes = EnumHelper.EnumToList<Sex>();
            return hashTypes;
        }
    }
}