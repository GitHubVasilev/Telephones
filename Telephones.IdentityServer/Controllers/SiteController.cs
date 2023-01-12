using Microsoft.AspNetCore.Mvc;

namespace Telephones.IdentityServer.Controllers
{
    //[Route("[controller]")]
    public class SiteController : Controller
    {
        //[Route("action")]
        public async Task<IActionResult> Index() 
        {
            return View();
        }
    }
}
