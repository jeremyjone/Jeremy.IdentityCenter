using AutoMapper;
using IdentityServer4.Extensions;
using Jeremy.IdentityCenter.Business.Models;
using Jeremy.IdentityCenter.Business.Models.Identity;
using Jeremy.IdentityCenter.Business.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using Jeremy.IdentityCenter.Business.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace Jeremy.IdentityCenter.Controllers
{
    [Authorize]
    public class IdentitiesController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ILogger<IdentitiesController> _logger;
        private readonly IIdentityService _identityService;

        public IdentitiesController(
            IMapper mapper,
            ILogger<IdentitiesController> logger,
            IIdentityService identityService)
        {
            _mapper = mapper;
            _logger = logger;
            _identityService = identityService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }




        #region 用户相关

        /// <summary>
        /// 用户列表页面
        /// </summary>
        /// <param name="search"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Users(string search, int? page)
        {
            ViewBag.Search = search;
            var users = await _identityService.GetUsersAsync(search, page ?? 1);
            return View(users);
        }

        /// <summary>
        /// 用户详情页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> UserInfo(int id)
        {
            if (id == default) return View(new UserViewModel());

            var user = await _identityService.GetUserAsync(id);
            if (user == null) return NotFound();
            return View(user);
        }

        /// <summary>
        /// 提交一个用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = PolicyConstants.Super)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserInfo(UserViewModel user)
        {
            if (!ModelState.IsValid) return View(user);

            if (user.Id == default)
            {
                user = await _identityService.AddUserAsync(user);
            }
            else
            {
                await _identityService.UpdateUserAsync(user);
            }

            return RedirectToAction(nameof(Users));
        }

        /// <summary>
        /// 删除用户界面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> UserDelete(int id)
        {
            if (id == 0) return NotFound();
            var user = await _identityService.GetUserAsync(id);
            if (user == null) return NotFound();
            return View(user);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = PolicyConstants.Super)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserDelete(UserViewModel user)
        {
            // 无法删除当前用户
            if (user.Id.ToString() == User.GetSubjectId()) return RedirectToAction(nameof(UserDelete), user.Id);

            var r = await _identityService.RemoveUserAsync(user);
            return RedirectToAction(nameof(Users));
        }

        /// <summary>
        /// 用户的角色页面
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> UserRoles(int id, int? page)
        {
            if (id == default) return NotFound();
            var roles = await _identityService.BuildUserRolesViewModel(id, page);

            return View(roles);
        }

        /// <summary>
        /// 提交一个用户角色
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = PolicyConstants.Super)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserRoles(UserRolesViewModel role)
        {
            await _identityService.AddUserRoleAsync(role);
            return RedirectToAction(nameof(UserRoles), new { Id = role.UserId });
        }

        /// <summary>
        /// 删除用户角色的页面
        /// </summary>
        /// <param name="id"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> UserRolesDelete(int id, int roleId)
        {
            if (!await _identityService.ExistsUserAsync(id)) return NotFound();
            if (!await _identityService.ExistsRoleAsync(roleId)) return NotFound();

            var user = await _identityService.GetUserAsync(id);
            var roles = await _identityService.GetRolesAsync();


            return View(new UserRolesViewModel
            {
                UserId = id,
                RolesList = roles.Select(x => new SelectItemViewModel(x.Id.ToString(), x.Name)).ToList(),
                RoleId = roleId,
                UserName = user.Username
            });
        }

        /// <summary>
        /// 删除一个用户角色
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = PolicyConstants.Super)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserRolesDelete(UserRolesViewModel role)
        {
            await _identityService.RemoveUserRoleAsync(role.UserId, role.RoleId);
            return RedirectToAction(nameof(UserRoles), new { Id = role.UserId });
        }

        /// <summary>
        /// 用户声明页面
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> UserClaims(int id, int? page)
        {
            if (id == default) return NotFound();

            var claims = await _identityService.GetUserClaimsAsync(id, page ?? 1);
            claims.UserId = id;

            return View(claims);
        }

        /// <summary>
        /// 提交用户声明
        /// </summary>
        /// <param name="claim"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = PolicyConstants.Super)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserClaims(UserClaimsViewModel claim)
        {
            if (!ModelState.IsValid)
            {
                return View(claim);
            }

            await _identityService.AddUserClaimAsync(claim);
            return RedirectToAction(nameof(UserClaims), new { Id = claim.UserId });
        }

        /// <summary>
        /// 删除用户声明页面
        /// </summary>
        /// <param name="id"></param>
        /// <param name="claimId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> UserClaimsDelete(int id, int claimId)
        {
            if (id == default || claimId == default) return NotFound();

            var claim = await _identityService.GetUserClaimAsync(id, claimId);
            if (claim == null) return NotFound();

            var user = await _identityService.GetUserAsync(id);
            claim.UserName = user.Username;

            return View(claim);
        }

        /// <summary>
        /// 提交删除用户声明
        /// </summary>
        /// <param name="claim"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = PolicyConstants.Super)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserClaimsDelete(UserClaimsViewModel claim)
        {
            await _identityService.RemoveUserClaimAsync(claim.UserId, claim.ClaimId);
            return RedirectToAction(nameof(UserClaims), new { Id = claim.UserId });
        }

        /// <summary>
        /// 用户外部提供程序页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> UserProviders(int id)
        {
            if (id == default) return NotFound();

            var providers = await _identityService.GetUserProvidersAsync(id);
            return View(providers);
        }

        /// <summary>
        /// 删除提供程序的页面
        /// </summary>
        /// <param name="id"></param>
        /// <param name="providerKey"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> UserProvidersDelete(int id, string providerKey)
        {
            if (id == default || string.IsNullOrWhiteSpace(providerKey)) return NotFound();

            var provider = await _identityService.GetUserProviderAsync(id, providerKey);
            if (provider == null) return NotFound();
            return View(provider);
        }

        /// <summary>
        /// 提交一个删除提供程序
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = PolicyConstants.Super)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserProvidersDelete(UserProviderViewModel provider)
        {
            await _identityService.RemoveUserProviderAsync(provider);
            return RedirectToAction(nameof(UserProviders), new { Id = provider.UserId });
        }

        /// <summary>
        /// 修改用户密码页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ChangePassword(int id)
        {
            if (id == default) return NotFound();

            var user = await _identityService.GetUserAsync(id);
            var userDto = new ChangePasswordViewModel { UserId = id, Username = user.Username };

            return View(userDto);
        }


        [HttpPost]
        [Authorize(Policy = PolicyConstants.Super)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var res = await _identityService.ChangePasswordAsync(model);

            return res ? (IActionResult)RedirectToAction("UserInfo", new { Id = model.UserId }) : View(model);
        }

        #endregion







        #region 角色相关

        /// <summary>
        /// 角色列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Roles(string search, int? page)
        {
            ViewBag.Search = search;
            var model = await _identityService.GetRolesAsync(search, page ?? 1);
            return View(model);
        }

        /// <summary>
        /// 角色页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Role(int id)
        {
            if (id == default)
            {
                return View(new RoleViewModel());
            }

            var role = await _identityService.GetRoleAsync(id);
            if (role == null) return NotFound();
            return View(role);
        }

        /// <summary>
        /// 提交一个角色
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = PolicyConstants.Super)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Role(RoleViewModel role)
        {
            if (!ModelState.IsValid)
            {
                return View(role);
            }

            if (role.Id == 0)
            {
                role = await _identityService.AddRoleAsync(role);
            }
            else
            {
                await _identityService.UpdateRoleAsync(role);
            }

            return RedirectToAction(nameof(Roles));
        }

        /// <summary>
        /// 删除角色页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> RoleDelete(int id)
        {
            if (id == 0) return NotFound();
            var role = await _identityService.GetRoleAsync(id);
            if (role == null) return NotFound();
            return View(role);
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = PolicyConstants.Super)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RoleDelete(RoleViewModel role)
        {
            var r = await _identityService.RemoveRoleAsync(role);
            return RedirectToAction(nameof(Roles));
        }

        /// <summary>
        /// 角色声明页面
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> RoleClaims(int id, int? page)
        {
            if (id == default) return NotFound();

            var claims = await _identityService.GetRoleClaimsAsync(id, page ?? 1);
            claims.RoleId = id;

            return View(claims);
        }

        /// <summary>
        /// 提交一个角色声明
        /// </summary>
        /// <param name="claim"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = PolicyConstants.Super)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RoleClaims(RoleClaimsViewModel claim)
        {
            if (!ModelState.IsValid)
            {
                return View(claim);
            }

            await _identityService.AddRoleClaimAsync(claim);

            return RedirectToAction(nameof(RoleClaims), new { Id = claim.RoleId });
        }

        /// <summary>
        /// 删除角色声明页面
        /// </summary>
        /// <param name="id"></param>
        /// <param name="claimId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> RoleClaimsDelete(int id, int claimId)
        {
            if (id == default || claimId == default) return NotFound();

            var claim = await _identityService.GetRoleClaimAsync(id, claimId);

            return View(claim);
        }

        /// <summary>
        /// 删除一个角色声明
        /// </summary>
        /// <param name="claim"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = PolicyConstants.Super)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RoleClaimsDelete(RoleClaimsViewModel claim)
        {
            var r = await _identityService.RemoveRoleClaimAsync(claim);
            return RedirectToAction(nameof(RoleClaims), new { Id = claim.RoleId });
        }

        /// <summary>
        /// 用户包含当前角色的用户列表页面
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> RoleUsers(int id, int? page, string search)
        {
            var role = await _identityService.GetRoleAsync(id);
            var users = await _identityService.GetUsersInRoleAsync(id, search, page ?? 1);

            ViewBag.Role = role;
            ViewBag.Search = search;

            return View(users);
        }

        #endregion
    }
}
