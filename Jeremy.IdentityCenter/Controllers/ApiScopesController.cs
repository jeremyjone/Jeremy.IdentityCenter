using Jeremy.IdentityCenter.Business.Models.ApiScopes;
using Jeremy.IdentityCenter.Business.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Jeremy.IdentityCenter.Business.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace Jeremy.IdentityCenter.Controllers
{
    [Authorize]
    public class ApiScopesController : Controller
    {
        private readonly ILogger<ApiScopesController> _logger;
        private readonly IApiScopeService _apiScopeService;

        public ApiScopesController(ILogger<ApiScopesController> logger, IApiScopeService apiScopeService)
        {
            _logger = logger;
            _apiScopeService = apiScopeService;
        }

        /// <summary>
        /// Api 作用域主页
        /// </summary>
        /// <param name="page"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index(int? page, string search)
        {
            var scopes = await _apiScopeService.GetApiScopesAsync(search, page ?? 1);
            ViewBag.Search = search;
            return View(scopes);
        }

        /// <summary>
        /// Api 作用域详情页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Scope(string id)
        {
            if (id == default)
            {
                return View(new ApiScopeViewModel());
            }

            var scope = await _apiScopeService.GetApiScopeAsync(int.Parse(id));
            if (scope == null) return NotFound();
            return View(scope);
        }

        /// <summary>
        /// Api 作用域提交
        /// </summary>
        /// <param name="scope"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = PolicyConstants.Super)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Scope(ApiScopeViewModel scope)
        {
            if (!ModelState.IsValid)
            {
                return View(scope);
            }

            scope = _apiScopeService.BuildApiScopeViewModel(scope);
            if (scope.Id == 0)
            {
                scope = await _apiScopeService.AddApiScopeAsync(scope);
            }
            else
            {
                await _apiScopeService.UpdateApiScopeAsync(scope);
            }

            return RedirectToAction(nameof(Scope), new { Id = scope.Id });
        }

        /// <summary>
        /// 删除Api 作用域页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0) return NotFound();
            return View(await _apiScopeService.GetApiScopeAsync(id));
        }

        /// <summary>
        /// 删除Api 作用域
        /// </summary>
        /// <param name="scope"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = PolicyConstants.Super)]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(ApiScopeViewModel scope)
        {
            var r = _apiScopeService.RemoveApiScope(scope);
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Api 作用域属性页面
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Properties(int id, int? page)
        {
            if (id == 0) return NotFound();
            return View(await _apiScopeService.GetApiScopePropertiesAsync(id, page ?? 1));
        }

        /// <summary>
        /// 提交Api 作用域属性
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = PolicyConstants.Super)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Properties(ApiScopePropertiesViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            await _apiScopeService.AddApiScopePropertyAsync(model);
            return RedirectToAction(nameof(Properties), new { Id = model.ApiScopeId });
        }

        /// <summary>
        /// 删除Api 作用域属性页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> PropertyDelete(int id)
        {
            if (id == 0) return NotFound();

            return View(await _apiScopeService.GetApiScopePropertyAsync(id));
        }

        /// <summary>
        /// 删除Api 作用域属性
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = PolicyConstants.Super)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PropertyDelete(ApiScopePropertiesViewModel model)
        {
            await _apiScopeService.RemoveApiScopePropertyAsync(model);
            return RedirectToAction(nameof(Properties), new { Id = model.ApiScopeId });
        }
    }
}
