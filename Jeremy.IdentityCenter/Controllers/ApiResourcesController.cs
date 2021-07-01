using Jeremy.IdentityCenter.Business.Models.ApiResources;
using Jeremy.IdentityCenter.Business.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Jeremy.IdentityCenter.Business.Configuration;

namespace Jeremy.IdentityCenter.Controllers
{
    [Authorize]
    public class ApiResourcesController : Controller
    {
        private readonly ILogger<ApiResourcesController> _logger;
        private readonly IApiResourceService _apiResourceService;

        public ApiResourcesController(ILogger<ApiResourcesController> logger, IApiResourceService apiResourceService)
        {
            _logger = logger;
            _apiResourceService = apiResourceService;
        }

        /// <summary>
        /// Api 资源主页
        /// </summary>
        /// <param name="page"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index(int? page, string search)
        {
            var resources = await _apiResourceService.GetApiResourcesAsync(search, page ?? 1);
            ViewBag.Search = search;
            return View(resources);
        }

        /// <summary>
        /// Api 资源详情页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Resource(string id)
        {
            if (id == default)
            {
                return View(new ApiResourceViewModel());
            }

            var resource = await _apiResourceService.GetApiResourceAsync(int.Parse(id));
            if (resource == null) return NotFound();
            return View(resource);
        }

        /// <summary>
        /// Api 资源提交
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = PolicyConstants.Super)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Resource(ApiResourceViewModel resource)
        {
            if (!ModelState.IsValid)
            {
                return View(resource);
            }

            resource = _apiResourceService.BuildApiResourceViewModel(resource);
            if (resource.Id == 0)
            {
                resource = await _apiResourceService.AddApiResourceAsync(resource);
            }
            else
            {
                await _apiResourceService.UpdateApiResourceAsync(resource);
            }

            return RedirectToAction(nameof(Resource), new { Id = resource.Id });
        }

        /// <summary>
        /// 删除Api 资源页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0) return NotFound();
            return View(await _apiResourceService.GetApiResourceAsync(id));
        }

        /// <summary>
        /// 删除Api 资源
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = PolicyConstants.Super)]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(ApiResourceViewModel resource)
        {
            var r = _apiResourceService.RemoveApiResource(resource);
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Api 资源属性页面
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Properties(int id, int? page)
        {
            if (id == 0) return NotFound();
            return View(await _apiResourceService.GetApiResourcePropertiesAsync(id, page ?? 1));
        }

        /// <summary>
        /// 提交Api 资源属性
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = PolicyConstants.Super)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Properties(ApiResourcePropertiesViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            await _apiResourceService.AddApiResourcePropertyAsync(model);
            return RedirectToAction(nameof(Properties), new { Id = model.ApiResourceId });
        }

        /// <summary>
        /// 删除Api 资源属性页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> PropertyDelete(int id)
        {
            if (id == 0) return NotFound();

            return View(await _apiResourceService.GetApiResourcePropertyAsync(id));
        }

        /// <summary>
        /// 删除Api 资源属性
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = PolicyConstants.Super)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PropertyDelete(ApiResourcePropertiesViewModel model)
        {
            await _apiResourceService.RemoveApiResourcePropertyAsync(model);
            return RedirectToAction(nameof(Properties), new { Id = model.ApiResourceId });
        }

        /// <summary>
        /// Api 资源秘钥页面
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Secrets(int id, int? page)
        {
            if (id == 0) return NotFound();
            return View(
                _apiResourceService.BuildApiResourceSecretsViewModel(
                    await _apiResourceService.GetApiResourceSecretsAsync(id, page ?? 1)));
        }

        /// <summary>
        /// 提交Api 资源秘钥
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = PolicyConstants.Super)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Secrets(ApiResourceSecretsViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            await _apiResourceService.AddApiResourceSecretAsync(model);
            return RedirectToAction(nameof(Secrets), new { Id = model.ApiResourceId });
        }

        /// <summary>
        /// 删除Api 资源秘钥页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> SecretDelete(int id)
        {
            if (id == 0) return NotFound();

            return View(await _apiResourceService.GetApiResourceSecretAsync(id));
        }

        /// <summary>
        /// 删除Api 资源秘钥
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = PolicyConstants.Super)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SecretDelete(ApiResourceSecretsViewModel model)
        {
            await _apiResourceService.RemoveApiResourceSecretAsync(model);
            return RedirectToAction(nameof(Secrets), new { Id = model.ApiResourceId });
        }
    }
}
