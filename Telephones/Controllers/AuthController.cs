using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;

namespace Telephones.Controllers
{
    /// <summary>
    /// Контроллер для авторизации
    /// </summary>
    public class AuthController : Controller
    {
        /// <summary>
        /// Перенаправляет на страницу для авторизации
        /// </summary>
        public IActionResult Login()
        {
            return Challenge(new AuthenticationProperties { RedirectUri = "/" },
                OpenIdConnectDefaults.AuthenticationScheme);
        }

        /// <summary>
        /// Перенаправляет на страницу выхода из системы
        /// </summary>
        public IActionResult Logout()
        {
            var parameters = new AuthenticationProperties
            {
                RedirectUri = "/"
            };
            return SignOut(
                parameters,
                CookieAuthenticationDefaults.AuthenticationScheme,
                OpenIdConnectDefaults.AuthenticationScheme);
        }
    }
}
