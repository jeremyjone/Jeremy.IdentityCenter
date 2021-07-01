using Jeremy.IdentityCenter.Business.Models.Clients;
using Jeremy.IdentityCenter.Business.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Jeremy.IdentityCenter.Business.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace Jeremy.IdentityCenter.Controllers
{
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ClientsController : Controller
    {
        private readonly ILogger<ClientsController> _logger;
        private readonly IClientService _clientService;

        public ClientsController(
            ILogger<ClientsController> logger,
            IClientService clientService)
        {
            _logger = logger;
            _clientService = clientService;
        }

        /// <summary>
        /// 客户端主页
        /// </summary>
        /// <param name="page"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index(int? page, string search)
        {
            var clients = await _clientService.GetClientsAsync(search, page ?? 1);
            ViewBag.Search = search;
            return View(clients);
        }


        #region 客户端

        /// <summary>
        /// 指定客户端页面
        /// </summary>
        /// <param name="id"></param>
        /// <param name="setting"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Client(string id, string setting = "name")
        {
            ViewBag.Setting = setting;

            if (id == default)
            {
                var clientDto = _clientService.BuildClientViewModel();
                return View(clientDto);
            }

            int.TryParse(id, out var clientId);
            var client = await _clientService.GetClientAsync(clientId);
            client = _clientService.BuildClientViewModel(client);

            return View(client);
        }

        /// <summary>
        /// 提交客户端，更新或者新建
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = PolicyConstants.Super)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Client(ClientViewModel client)
        {
            client = _clientService.BuildClientViewModel(client);

            if (!ModelState.IsValid)
            {
                return View(client);
            }

            // 添加
            if (client.Id == 0)
            {
                var res = await _clientService.AddClientAsync(client);
                return RedirectToAction(nameof(Client), new { res.Id });
            }

            {
                // 更新
                var res = await _clientService.UpdateClientAsync(client);
                return RedirectToAction(nameof(Client), new { res.Id, setting = "basics" });
            }
        }

        /// <summary>
        /// 删除页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var client = await _clientService.GetClientAsync(id);
            if (client == null) return NotFound();

            return View(client);
        }

        /// <summary>
        /// 删除方法
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = PolicyConstants.Super)]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(ClientViewModel client)
        {
            var res = _clientService.RemoveClient(client);
            return RedirectToAction(nameof(Index));
        }

        #endregion




        #region 客户端秘钥

        /// <summary>
        /// 显示客户端秘钥页面
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ClientSecrets(int id, int? page)
        {
            if (id == 0) return NotFound();

            var secrets = await _clientService.GetClientSecretsAsync(id, page ?? 1);
            _clientService.BuildClientSecretsViewModel(secrets);
            return View(secrets);
        }

        /// <summary>
        /// 添加客户端秘钥的方法
        /// </summary>
        /// <param name="secrets"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = PolicyConstants.Super)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ClientSecrets(ClientSecretsViewModel secrets)
        {
            await _clientService.AddClientSecretAsync(secrets);
            return RedirectToAction(nameof(ClientSecrets), new { Id = secrets.ClientId });
        }

        /// <summary>
        /// 删除客户端秘钥页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ClientSecretDelete(int id)
        {
            if (id == 0) return NotFound();

            var secret = await _clientService.GetClientSecretAsync(id);
            return View(secret);
        }

        /// <summary>
        /// 删除客户端秘钥的方法
        /// </summary>
        /// <param name="secret"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = PolicyConstants.Super)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ClientSecretDelete(ClientSecretsViewModel secret)
        {
            await _clientService.RemoveClientSecretAsync(secret);
            return RedirectToAction(nameof(ClientSecrets), new { Id = secret.ClientId });
        }

        #endregion




        #region 客户端属性

        /// <summary>
        /// 客户端属性页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ClientProperties(int id, int? page)
        {
            if (id == 0) return NotFound();

            var properties = await _clientService.GetClientPropertiesAsync(id, page ?? 1);
            return View(properties);
        }

        /// <summary>
        /// 添加客户端属性的方法
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = PolicyConstants.Super)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ClientProperties(ClientPropertiesViewModel property)
        {
            if (!ModelState.IsValid) return View(property);

            await _clientService.AddClientPropertyAsync(property);
            return RedirectToAction(nameof(ClientProperties), new { Id = property.ClientId });
        }

        /// <summary>
        /// 删除客户端属性页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ClientPropertyDelete(int id)
        {
            if (id == 0) return NotFound();

            var property = await _clientService.GetClientPropertyAsync(id);
            return View(property);
        }

        /// <summary>
        /// 删除客户端属性的方法
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = PolicyConstants.Super)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ClientPropertyDelete(ClientPropertiesViewModel property)
        {
            await _clientService.RemoveClientPropertyAsync(property);
            return RedirectToAction(nameof(ClientProperties), new { Id = property.ClientId });
        }

        #endregion




        #region 客户端声明

        /// <summary>
        /// 客户端声明页面
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ClientClaims(int id, int? page)
        {
            if (id == 0) return NotFound();

            var claims = await _clientService.GetClientClaimsAsync(id, page ?? 1);
            return View(claims);
        }

        /// <summary>
        /// 添加客户端声明的方法
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = PolicyConstants.Super)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ClientClaims(ClientClaimsViewModel claims)
        {
            if (!ModelState.IsValid) return View(claims);

            await _clientService.AddClientClaimAsync(claims);
            return RedirectToAction(nameof(ClientClaims), new { Id = claims.ClientId });
        }

        /// <summary>
        /// 删除客户端声明页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ClientClaimDelete(int id)
        {
            if (id == 0) return NotFound();

            var claim = await _clientService.GetClientClaimAsync(id);
            return View(claim);
        }

        /// <summary>
        /// 删除客户端声明的方法
        /// </summary>
        /// <param name="claim"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = PolicyConstants.Super)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ClientClaimDelete(ClientClaimsViewModel claim)
        {
            await _clientService.RemoveClientClaimAsync(claim);
            return RedirectToAction(nameof(ClientClaims), new { Id = claim.ClientId });
        }

        #endregion




        #region 获取数据

        /// <summary>
        /// 获取客户端声明的数据
        /// </summary>
        /// <param name="claim"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Claims(string claim, int count = 0)
        {
            return Ok(_clientService.GetStandardClaims(claim, count));
        }

        /// <summary>
        /// 获取客户端嵌入算法的数据
        /// </summary>
        /// <param name="algorithm"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult SigningAlgorithms(string algorithm, int count = 0)
        {
            return Ok(_clientService.GetSigningAlgorithms(algorithm, count));
        }

        /// <summary>
        /// 获取客户端授权类型的数据
        /// </summary>
        /// <param name="type"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GrantTypes(string type, int count = 0)
        {
            return Ok(_clientService.GetGrantTypes(type, count));
        }

        /// <summary>
        /// 获取客户端范围的数据
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Scopes(string scope, int count = 0)
        {
            return Ok(await _clientService.GetScopesAsync(scope, count));
        }

        #endregion
    }
}
