using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Telephones.IdentityServer.Entities;
using Telephones.IdentityServer.ViewModels;

namespace Telephones.IdentityServer.Controllers
{
    public class AuthController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signinManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthController(SignInManager<ApplicationUser> signinManager, UserManager<ApplicationUser> userManager)
        {
            _signinManager = signinManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model) 
        {
            if (!ModelState.IsValid) 
            {
                return View(model);
            }

            ApplicationUser user = await _userManager.FindByNameAsync(model.UserName);

            if (user is null) 
            {
                ModelState.AddModelError("UserName", "User not found");
                return View(model);
            }
            Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signinManager.PasswordSignInAsync(user!, model.Password, false, false);

            if (!signInResult.Succeeded) 
            {
                ModelState.AddModelError("Password", "Invalid password");
                return View(model);
            }

            return Redirect(model.ReturnUrl);
        }
    }
}
