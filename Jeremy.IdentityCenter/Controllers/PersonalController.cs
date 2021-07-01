using Jeremy.IdentityCenter.Business.Handlers;
using Jeremy.IdentityCenter.Business.Helpers;
using Jeremy.IdentityCenter.Business.Models;
using Jeremy.IdentityCenter.Business.Models.Identity;
using Jeremy.IdentityCenter.Business.Models.Personal;
using Jeremy.IdentityCenter.Database.Entities;
using Jeremy.Tools.Extensions;
using Jeremy.Tools.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Jeremy.IdentityCenter.Controllers
{
    [Authorize]
    public class PersonalController : BaseController
    {
        private readonly SignInManager<IdcIdentityUser> _signInManager;
        private readonly UserManager<IdcIdentityUser> _userManager;
        private readonly IdcEmailSender _emailSender;

        public PersonalController(ILogger<BaseController> logger,
            SignInManager<IdcIdentityUser> signInManager,
            UserManager<IdcIdentityUser> userManager,
            IdcEmailSender emailSender) : base(logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        /// 用户个人资料页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            return View(await BuildUserInfoViewModelAsync(user));
        }

        /// <summary>
        /// 更新个人资料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(UserInfoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            if (model.Email != user.Email)
            {
                var res = await _userManager.SetEmailAsync(user, model.Email);
                if (!res.Succeeded) throw new ApplicationException("设置邮件时发生错误。");
            }

            if (model.Phone != user.PhoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, model.Phone);
                if (!setPhoneResult.Succeeded) throw new ApplicationException("设置电话号码时发生错误");
            }

            await UpdateUserClaimsAsync(model, user);

            StatusMessage = "信息已更新";

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// 发送验证邮件
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendVerificationEmail(UserInfoViewModel model)
        {
            if (!ModelState.IsValid) return RedirectToAction(nameof(Index), model);

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.Action("ConfirmEmail", "Account",
                new { token, email = user.Email }, Request.Scheme);
            await _emailSender.SendConfirmationEmailAsync(user.Email, confirmationLink);

            StatusMessage = "邮件已发送";
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// 修改密码页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            var user = await _userManager.GetUserAsync(User);
            return View(BuildChangePasswordViewModel(user));
        }

        /// <summary>
        /// 更新密码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            if (string.IsNullOrWhiteSpace(model.CurrentPassword))
            {
                AddError("当前密码不能为空");
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var res = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.Password);
            if (!res.Succeeded)
            {
                AddErrors(res);
                return View(model);
            }

            await _signInManager.RefreshSignInAsync(user);
            Logger.LogInformation("用户 {0} 修改密码成功", user.UserName);

            StatusMessage = "密码已修改";

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// 获取用户个人数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Data()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            return View();
        }

        /// <summary>
        /// 下载个人数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DownloadData()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            Logger.LogInformation("用户 {0} 下载了个人数据。", _userManager.GetUserId(User));

            //var personalDataProps = typeof(TUser).GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(PersonalDataAttribute)));
            var personalDataProps = typeof(IdcIdentityUser).GetProperties()
                .Where(prop => Attribute.IsDefined(prop, typeof(PersonalDataAttribute)));
            var personalData = personalDataProps.ToDictionary(p => p.Name, p => p.GetValue(user)?.ToString() ?? "null");

            Response.Headers.Add("Content-Disposition", "attachment; filename=PersonalData.json");
            return new FileContentResult(personalData.Serialize().ToBytes(), "text/json");
        }

        /// <summary>
        /// 确认删除个人数据页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> DeleteAccount()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var model = new DeleteAccountViewModel
            {
                RequirePassword = await _userManager.HasPasswordAsync(user)
            };

            return View(model);
        }

        /// <summary>
        /// 删除账户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAccount(DeleteAccountViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            model.RequirePassword = await _userManager.HasPasswordAsync(user);
            if (model.RequirePassword)
            {
                if (!await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    AddError("密码不正确");
                    return View(model);
                }
            }

            var result = await _userManager.DeleteAsync(user);
            var userId = await _userManager.GetUserIdAsync(user);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException("删除用户错误");
            }

            await _signInManager.SignOutAsync();
            Logger.LogInformation("用户 {0} 删除了个人数据", userId);
            return RedirectToAction("Index", "Home");
        }


        /**************
         *  工具函数  *
         **************/

        private async Task<UserInfoViewModel> BuildUserInfoViewModelAsync(IdcIdentityUser user)
        {
            var claims = await _userManager.GetClaimsAsync(user);
            var profile = OpenIdClaimHelpers.ExtractProfileInfo(claims);
            return new UserInfoViewModel
            {
                Username = user.UserName,
                Nickname = user.NickName,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                Phone = user.PhoneNumber,
                Avatar = user.Avatar,
                Profile = profile.Profile,
                Website = profile.Website,
                StatusMessage = StatusMessage
            };
        }

        private ChangePasswordViewModel BuildChangePasswordViewModel(IdcIdentityUser user)
        {
            return new ChangePasswordViewModel
            {
                UserId = user.Id,
                Username = user.UserName
            };
        }

        private async Task UpdateUserClaimsAsync(UserInfoViewModel model, IdcIdentityUser user)
        {
            var claims = await _userManager.GetClaimsAsync(user);
            var oldProfile = OpenIdClaimHelpers.ExtractProfileInfo(claims);
            var newProfile = new OpenIdProfile
            {
                Website = model.Website,
                Profile = model.Profile
            };

            var claimsToRemove = OpenIdClaimHelpers.ExtractClaimsToRemove(oldProfile, newProfile);
            var claimsToAdd = OpenIdClaimHelpers.ExtractClaimsToAdd(oldProfile, newProfile);
            var claimsToReplace = OpenIdClaimHelpers.ExtractClaimsToReplace(claims, newProfile);

            await _userManager.RemoveClaimsAsync(user, claimsToRemove);
            await _userManager.AddClaimsAsync(user, claimsToAdd);

            foreach (var (item1, item2) in claimsToReplace)
            {
                await _userManager.ReplaceClaimAsync(user, item1, item2);
            }
        }

    }
}
