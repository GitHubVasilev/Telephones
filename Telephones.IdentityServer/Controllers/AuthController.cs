using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Telephones.IdentityServer.Entities;
using Telephones.IdentityServer.ViewModels;

namespace Telephones.IdentityServer.Controllers
{
    /// <summary>
    /// Контроллер для авторизации на сервере
    /// </summary>
    public class AuthController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signinManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IIdentityServerInteractionService _IdentityinteractionService;

        public AuthController(IIdentityServerInteractionService identityServer,
            SignInManager<ApplicationUser> signinManager,
            UserManager<ApplicationUser> userManager)
        {
            _IdentityinteractionService = identityServer;
            _signinManager = signinManager;
            _userManager = userManager;
        }

        /// <summary>
        /// Возвращает страницу для авторизации пользователя на сервере. Метод GET
        /// </summary>
        /// <param name="returnUrl">Ссылка для возврата после авторизации</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            return View();
        }

        /// <summary>
        /// Авторизует пользователя на сервере
        /// </summary>
        /// <param name="model">Данные для авторизации</param>
        /// <returns></returns>
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

        public async Task<IActionResult> Logout(string logoutId) 
        {
            await _signinManager.SignOutAsync();
            LogoutRequest logoutResult = await _IdentityinteractionService.GetLogoutContextAsync(logoutId);

            if (string.IsNullOrEmpty(logoutResult.PostLogoutRedirectUri)) 
            {
                return Redirect("/Login");
            }
            return Redirect(logoutResult.PostLogoutRedirectUri);
        }
    }
}
