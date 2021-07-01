using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Jeremy.IdentityCenter.Controllers
{
    public class BaseController : Controller
    {
        protected readonly ILogger<BaseController> Logger;

        public BaseController(ILogger<BaseController> logger)
        {
            Logger = logger;
        }

        protected void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        protected void AddError(string message, string key = "")
        {
            ModelState.AddModelError(key, message);
        }
    }
}
