using Jeremy.IdentityCenter.Business.Models.IdentityResource;
using Jeremy.IdentityCenter.Business.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Jeremy.IdentityCenter.Business.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace Jeremy.IdentityCenter.Controllers
{
    [Authorize]
    public class IdentityResourceController : Controller
    {
        private readonly IIdentityResourceService _identityResourceService;

        public IdentityResourceController(IIdentityResourceService identityResourceService)
        {
            _identityResourceService = identityResourceService;
        }

        /// <summary>
        /// 身份资源主页
        /// </summary>
        /// <param name="page"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index(int? page, string search)
        {
            var resources = await _identityResourceService.GetIdentityResourcesAsync(search, page ?? 1);
            ViewBag.Search = search;
            return View(resources);
        }

        /// <summary>
        /// 身份资源详情页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> IdentityResource(string id)
        {
            if (id == default)
            {
                return View(new IdentityResourceViewModel());
            }

            var resource = await _identityResourceService.GetIdentityResourceAsync(int.Parse(id));
            if (resource == null) return NotFound();
            return View(resource);
        }

        /// <summary>
        /// 身份资源提交
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = PolicyConstants.Super)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IdentityResource(IdentityResourceViewModel resource)
        {
            if (!ModelState.IsValid)
            {
                return View(resource);
            }

            resource = _identityResourceService.BuildIdentityResourceViewModel(resource);
            if (resource.Id == 0)
            {
                resource = await _identityResourceService.AddIdentityResourceAsync(resource);
            }
            else
            {
                await _identityResourceService.UpdateIdentityResourceAsync(resource);
            }

            return RedirectToAction(nameof(IdentityResource), new { Id = resource.Id });
        }

        /// <summary>
        /// 删除身份资源页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0) return NotFound();
            return View(await _identityResourceService.GetIdentityResourceAsync(id));
        }

        /// <summary>
        /// 删除身份资源
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = PolicyConstants.Super)]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(IdentityResourceViewModel resource)
        {
            var r = _identityResourceService.RemoveIdentityResource(resource);
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// 身份资源属性页面
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Properties(int id, int? page)
        {
            if (id == 0) return NotFound();
            return View(await _identityResourceService.GetIdentityResourcePropertiesAsync(id, page ?? 1));
        }

        /// <summary>
        /// 提交身份资源属性
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = PolicyConstants.Super)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Properties(IdentityResourcePropertiesViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            await _identityResourceService.AddIdentityResourcePropertyAsync(model);
            return RedirectToAction(nameof(Properties), new { Id = model.IdentityResourceId });
        }

        /// <summary>
        /// 删除身份资源属性页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> PropertyDelete(int id)
        {
            if (id == 0) return NotFound();

            return View(await _identityResourceService.GetIdentityResourcePropertyAsync(id));
        }

        /// <summary>
        /// 删除身份资源属性
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = PolicyConstants.Super)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PropertyDelete(IdentityResourcePropertiesViewModel model)
        {
            await _identityResourceService.RemoveIdentityResourcePropertyAsync(model);
            return RedirectToAction(nameof(Properties), new { Id = model.IdentityResourceId });
        }
    }
}
