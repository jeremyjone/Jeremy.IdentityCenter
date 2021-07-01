using AutoMapper;
using IdentityModel;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Jeremy.IdentityCenter.Business.Attributes;
using Jeremy.IdentityCenter.Business.Handlers;
using Jeremy.IdentityCenter.Business.Models.Account;
using Jeremy.IdentityCenter.Business.Services.Interfaces;
using Jeremy.IdentityCenter.Database.Entities;
using Jeremy.Tools.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Jeremy.IdentityCenter.Controllers
{
    [SecurityHeaders]
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class AccountController : BaseController
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        private readonly IEventService _events;
        private readonly UserManager<IdcIdentityUser> _userManager;
        private readonly RoleManager<IdcIdentityRole> _roleManager;
        private readonly SignInManager<IdcIdentityUser> _signInManager;
        private readonly IdcEmailSender _emailSender;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IIdentityService _identityService;

        public AccountController(
            ILogger<AccountController> logger,
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IAuthenticationSchemeProvider schemeProvider,
            IEventService events,
            UserManager<IdcIdentityUser> userManager,
            RoleManager<IdcIdentityRole> roleManager,
            SignInManager<IdcIdentityUser> signInManager,
            IdcEmailSender emailSender,
            IMapper mapper,
            IConfiguration configuration,
            IIdentityService identityService) : base(logger)
        {
            _interaction = interaction;
            _clientStore = clientStore;
            _schemeProvider = schemeProvider;
            _events = events;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _mapper = mapper;
            _configuration = configuration;
            _identityService = identityService;
        }

        /// <summary>
        /// Entry point into the login workflow
        /// </summary>
        [HttpGet("oauth/login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl)
        {
            // build a model so we know what to show on the login page
            var vm = await BuildLoginViewModelAsync(returnUrl);

            if (vm.IsExternalLoginOnly)
            {
                // we only have one option for logging in and it's an external provider
                return RedirectToAction("Challenge", "External", new { scheme = vm.ExternalLoginScheme, returnUrl });
            }

            return View(vm);
        }

        /// <summary>
        /// Handle postback from username/password login
        /// </summary>
        [HttpPost("oauth/login")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginInputModel model, string button)
        {
            // check if we are in the context of an authorization request
            var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);

            // the user clicked the "cancel" button
            if (button != "login")
            {
                if (context != null)
                {
                    // if the user cancels, send a result back into IdentityServer as if they 
                    // denied the consent (even if this client does not require consent).
                    // this will send back an access denied OIDC error response to the client.
                    await _interaction.DenyAuthorizationAsync(context, AuthorizationError.AccessDenied);

                    // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                    if (context.IsNativeClient())
                    {
                        // The client is native, so this change in how to
                        // return the response is for better UX for the end user.
                        return this.LoadingPage("Redirect", model.ReturnUrl);
                    }

                    return Redirect(model.ReturnUrl);
                }

                // since we don't have a valid context, then we just go back to the home page
                return Redirect("~/");
            }

            if (ModelState.IsValid)
            {
                // 允许用户使用用户名或者邮箱登录
                var user = await _userManager.FindByNameAsync(model.Username) ??
                           await _userManager.FindByEmailAsync(model.Username);

                if (user != null && user.Validity)
                {
                    // 使用密码验证
                    var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberLogin, lockoutOnFailure: true);
                    if (result.Succeeded)
                    {
                        // 更新登录信息
                        await UpdateLoginInfoAsync(user);

                        await _events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id.ToString(), user.UserName));

                        // make sure the returnUrl is still valid, and if so redirect back to authorize endpoint or a local page
                        // the IsLocalUrl check is only necessary if you want to support additional local pages, otherwise IsValidReturnUrl is more strict
                        if (_interaction.IsValidReturnUrl(model.ReturnUrl) || Url.IsLocalUrl(model.ReturnUrl))
                        {
                            return Redirect(model.ReturnUrl);
                        }
                        return Redirect("~/");
                    }
                }
                await _events.RaiseAsync(new UserLoginFailureEvent(model.Username, "invalid credentials"));
                ModelState.AddModelError(string.Empty, AccountOptions.InvalidCredentialsErrorMessage);
            }

            // something went wrong, show form with error
            var vm = await BuildLoginViewModelAsync(model);
            return View(vm);
        }

        /// <summary>
        /// Show logout page
        /// </summary>
        [HttpGet("oauth/logout")]
        public async Task<IActionResult> Logout(string logoutId)
        {
            // build a model so the logout page knows what to display
            var vm = await BuildLogoutViewModelAsync(logoutId);

            if (vm.ShowLogoutPrompt == false)
            {
                // if the request for logout was properly authenticated from IdentityServer, then
                // we don't need to show the prompt and can just log the user out directly.
                return await Logout(vm);
            }

            return View(vm);
        }

        /// <summary>
        /// Handle logout page postback
        /// </summary>
        [HttpPost("oauth/logout")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(LogoutInputModel model)
        {
            // build a model so the logged out page knows what to display
            var vm = await BuildLoggedOutViewModelAsync(model.LogoutId, model.PostLogoutRedirectUri);

            if (User?.Identity.IsAuthenticated == true)
            {
                // delete local authentication cookie
                //await HttpContext.SignOutAsync();
                await _signInManager.SignOutAsync();

                // raise the logout event
                await _events.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()));
            }

            // check if we need to trigger sign-out at an upstream identity provider
            if (vm.TriggerExternalSignout)
            {
                // build a return URL so the upstream provider will redirect back
                // to us after the user has logged out. this allows us to then
                // complete our single sign-out processing.
                string url = Url.Action("Logout", new { logoutId = vm.LogoutId });

                // this triggers a redirect to the external provider for sign-out
                return SignOut(new AuthenticationProperties { RedirectUri = url }, vm.ExternalAuthenticationScheme);
            }

            return View("LoggedOut", vm);
        }

        /// <summary>
        /// 验证Token是否有效，能进来一定是有效的
        /// </summary>
        /// <returns></returns>
        [HttpGet("oauth/validate")]
        public IActionResult Validate()
        {
            if (HttpContext.Request.Query.TryGetValue("access_token", out var accessToken))
            {
                return Ok();
            }
            return BadRequest();
        }

        #region 账户注册

        /// <summary>
        /// 注册页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View(_identityService.BuildRegisterViewModel());
        }

        /// <summary>
        /// 注册提交
        /// </summary>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                var res = new IdentityResult();
                // 验证
                var isNewUser = true;

                if (await _userManager.FindByNameAsync(model.UserName) != null)
                {
                    ModelState.AddModelError(string.Empty, $"用户名 {model.UserName} 已经存在");
                    isNewUser = false;
                }

                if (await _userManager.FindByEmailAsync(model.Email) != null)
                {
                    ModelState.AddModelError(string.Empty, $"邮箱 {model.Email} 已经存在");
                    isNewUser = false;
                }

                // 创建新用户
                if (isNewUser)
                {
                    var user = _mapper.Map<IdcIdentityUser>(model);
                    user.Sex = Enum.TryParse(model.Sex, true, out Sex result) ? result : Sex.Unknown;
                    res = await _userManager.CreateAsync(user, model.Password);
                    if (res.Succeeded)
                    {
                        // 如果启用了邮箱确认，则发送邮件
                        if (_configuration["Confirm:ConfirmedEmail"].ToBool())
                        {
                            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                            var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account",
                                new { token, email = user.Email }, Request.Scheme);
                            await _emailSender.SendConfirmationEmailAsync(user.Email, confirmationLink);
                        }

                        // 添加角色权限
                        var defaultRoleName = _configuration["Default:RoleName"];
                        var role = await _roleManager.FindByNameAsync(defaultRoleName);
                        await _userManager.AddToRoleAsync(user, role.Name);

                        res = await _userManager.AddClaimsAsync(user, new[]
                        {
                            new Claim(JwtClaimTypes.Name, model.UserName),
                            new Claim(JwtClaimTypes.NickName, model.NickName),
                            new Claim(JwtClaimTypes.Email, model.Email),
                            new Claim(JwtClaimTypes.EmailVerified, "false", ClaimValueTypes.Boolean),
                            new Claim(JwtClaimTypes.Role, $"{role.Id}"),
                        });

                        if (res.Succeeded)
                        {
                            if (Url.IsLocalUrl(returnUrl))
                            {
                                return Redirect(returnUrl);
                            }

                            return RedirectToAction(nameof(SuccessRegistration));
                        }
                    }
                }

                AddErrors(res);
            }

            return View(model);
        }

        /// <summary>
        /// 确认邮箱页面
        /// </summary>
        /// <param name="token"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return View("Error");
            var result = await _userManager.ConfirmEmailAsync(user, token);

            // 确认邮箱后，更新声明
            await _userManager.ReplaceClaimAsync(user,
                new Claim(JwtClaimTypes.EmailVerified, "false", ClaimValueTypes.Boolean),
                new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean));
            return View(result.Succeeded ? nameof(ConfirmEmail) : "Error");
        }

        /// <summary>
        /// 成功注册页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult SuccessRegistration()
        {
            return View();
        }

        #endregion


        /// <summary>
        /// 忘记密码页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View(new ForgotPasswordViewModel());
        }

        /// <summary>
        /// 提交修改密码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            // 查找用户，并且判断用户输入信息是否正确。如果错误，不要暴露，直接返回
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null || user.Email != model.Email) return RedirectToAction(nameof(ForgotPasswordConfirmation));

            // For more information on how to enable account confirmation and password reset please
            // visit https://go.microsoft.com/fwlink/?LinkID=532713
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.Action(nameof(ResetPassword), "Account",
                new { id = user.Id.ToString(), code, email = model.Email },
                Request.Scheme);

            // 发送邮件
            await _emailSender.SendResetPasswordEmailAsync(model.Email, callbackUrl);
            return RedirectToAction(nameof(ForgotPasswordConfirmation));
        }

        /// <summary>
        /// 忘记密码发送确认页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        /// <summary>
        /// 重置密码页面
        /// </summary>
        /// <param name="code"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null, string email = null)
        {
            if (code == null)
            {
                throw new ApplicationException("A code must be supplied for password reset.");
            }
            var model = new ResetPasswordViewModel { Code = code, Email = email };
            return View(model);
        }

        /// <summary>
        /// 重置密码提交
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }
            AddErrors(result);
            return View();
        }

        /// <summary>
        /// 确认重置密码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }


        /*****************************************/
        /* helper APIs for the AccountController */
        /*****************************************/
        private async Task<LoginViewModel> BuildLoginViewModelAsync(string returnUrl)
        {
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
            if (context?.IdP != null && await _schemeProvider.GetSchemeAsync(context.IdP) != null)
            {
                var local = context.IdP == IdentityServer4.IdentityServerConstants.LocalIdentityProvider;

                // this is meant to short circuit the UI and only trigger the one external IdP
                var vm = new LoginViewModel
                {
                    EnableLocalLogin = local,
                    ReturnUrl = returnUrl,
                    Username = context?.LoginHint,
                };

                if (!local)
                {
                    vm.ExternalProviders = new[] { new ExternalProvider { AuthenticationScheme = context.IdP } };
                }

                return vm;
            }

            var schemes = await _schemeProvider.GetAllSchemesAsync();

            var providers = schemes
                .Where(x => x.DisplayName != null)
                .Select(x => new ExternalProvider
                {

                    DisplayName = x.DisplayName ?? x.Name,
                    AuthenticationScheme = x.Name
                }).ToList();

            var allowLocal = true;
            if (context?.Client.ClientId != null)
            {
                var client = await _clientStore.FindEnabledClientByIdAsync(context.Client.ClientId);
                if (client != null)
                {
                    allowLocal = client.EnableLocalLogin;

                    if (client.IdentityProviderRestrictions != null && client.IdentityProviderRestrictions.Any())
                    {
                        providers = providers.Where(provider => client.IdentityProviderRestrictions.Contains(provider.AuthenticationScheme)).ToList();
                    }
                }
            }

            return new LoginViewModel
            {
                AllowRememberLogin = AccountOptions.AllowRememberLogin,
                EnableLocalLogin = allowLocal && AccountOptions.AllowLocalLogin,
                ReturnUrl = returnUrl,
                Username = context?.LoginHint,
                ExternalProviders = providers.ToArray()
            };
        }

        private async Task<LoginViewModel> BuildLoginViewModelAsync(LoginInputModel model)
        {
            var vm = await BuildLoginViewModelAsync(model.ReturnUrl);
            vm.Username = model.Username;
            vm.RememberLogin = model.RememberLogin;
            return vm;
        }

        private async Task<LogoutViewModel> BuildLogoutViewModelAsync(string logoutId)
        {
            var vm = new LogoutViewModel { LogoutId = logoutId, ShowLogoutPrompt = AccountOptions.ShowLogoutPrompt };

            if (User?.Identity.IsAuthenticated != true)
            {
                // if the user is not authenticated, then just show logged out page
                vm.ShowLogoutPrompt = false;
                return vm;
            }

            var context = await _interaction.GetLogoutContextAsync(logoutId);
            if (context?.ShowSignoutPrompt == false)
            {
                // it's safe to automatically sign-out
                vm.ShowLogoutPrompt = false;
                return vm;
            }

            // show the logout prompt. this prevents attacks where the user
            // is automatically signed out by another malicious web page.
            return vm;
        }

        private async Task<LoggedOutViewModel> BuildLoggedOutViewModelAsync(string logoutId, string redirectUri = null)
        {
            // get context information (client name, post logout redirect URI and iframe for federated signout)
            var logout = await _interaction.GetLogoutContextAsync(logoutId);

            var vm = new LoggedOutViewModel
            {
                AutomaticRedirectAfterSignOut = AccountOptions.AutomaticRedirectAfterSignOut,
                PostLogoutRedirectUri = redirectUri ?? logout?.PostLogoutRedirectUri,
                ClientName = string.IsNullOrEmpty(logout?.ClientName) ? logout?.ClientId : logout?.ClientName,
                SignOutIframeUrl = logout?.SignOutIFrameUrl,
                LogoutId = logoutId
            };

            if (User?.Identity.IsAuthenticated == true)
            {
                var idp = User.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;
                if (idp != null && idp != IdentityServer4.IdentityServerConstants.LocalIdentityProvider)
                {
                    var providerSupportsSignout = await HttpContext.GetSchemeSupportsSignOutAsync(idp);
                    if (providerSupportsSignout)
                    {
                        if (vm.LogoutId == null)
                        {
                            // if there's no current logout context, we need to create one
                            // this captures necessary info from the current logged in user
                            // before we signout and redirect away to the external IdP for signout
                            vm.LogoutId = await _interaction.CreateLogoutContextAsync();
                        }

                        vm.ExternalAuthenticationScheme = idp;
                    }
                }
            }

            return vm;
        }

        /// <summary>
        /// 更新用户登录信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task UpdateLoginInfoAsync(IdcIdentityUser user)
        {
            user.LoginTime = DateTime.Now;
            Request.Headers.TryGetValue("User-Agent", out var ua);
            if (!string.IsNullOrWhiteSpace(ua)) user.LoginUserAgent = ua;
            user.LoginIp = Request.Host.Host;

            await _userManager.UpdateAsync(user);
        }
    }
}
