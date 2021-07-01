using Jeremy.IdentityCenter.Business.Models.PersistedGrants;
using Jeremy.IdentityCenter.Business.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Jeremy.IdentityCenter.Business.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace Jeremy.IdentityCenter.Controllers
{
    [Authorize]
    public class PersistedGrantsController : Controller
    {
        private readonly IPersistedGrantService _persistedGrantService;
        private readonly ILogger<PersistedGrantsController> _logger;

        public PersistedGrantsController(IPersistedGrantService persistedGrantService, ILogger<PersistedGrantsController> logger)
        {
            _persistedGrantService = persistedGrantService;
            _logger = logger;
        }

        /// <summary>
        /// 获取全部的持久授权内容
        /// </summary>
        /// <param name="search"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index(string search, int? page)
        {
            ViewBag.Search = search;
            var models = await _persistedGrantService.GetPersistedGrantsByUsersAsync(search, page ?? 1);
            return View(models);
        }

        /// <summary>
        /// 获取指定用户的持久授权内容
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> PersistedGrant(string id, int? page)
        {
            var models = await _persistedGrantService.GetPersistedGrantsByUserAsync(id, page ?? 1);
            models.SubjectId = id;
            return View(models);
        }

        /// <summary>
        /// 删除持久授权页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return NotFound();

            var grant = await _persistedGrantService.GetPersistedGrantAsync(id);
            return grant == null ? NotFound() : View(grant);
        }

        /// <summary>
        /// 删除一个持久授权
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = PolicyConstants.Super)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteGrant(PersistedGrantViewModel model)
        {
            await _persistedGrantService.DeletePersistedGrantAsync(model.Key);
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// 删除全部持久授权
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = PolicyConstants.Super)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteGrants(PersistedGrantsViewModel models)
        {
            await _persistedGrantService.DeletePersistedGrantsAsync(models.SubjectId);
            return RedirectToAction(nameof(Index));
        }
    }
}
