using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Telephones.IdentityServer.Data;
using Telephones.IdentityServer.Entities;

namespace Telephones.IdentityServer.Infrastructure
{
    /// <summary>
    /// Класс для расширения и настройки токенов
    /// </summary>
    public class ProfileService : IProfileService
    {

        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        /// <summary>
        /// API добавляет в токены данные о волях
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            ClaimsPrincipal principal = context.Subject;
            string role = principal.FindFirstValue(ClaimTypes.Role);

            if (role is null) 
            {
                ApplicationUser user = _userManager.GetUserAsync(principal).Result;
                IList<Claim> claims = _userManager.GetClaimsAsync(user).Result;
                context.IssuedClaims.AddRange(claims);
                return Task.CompletedTask;
            }

            context.IssuedClaims.AddRange(new List<Claim>()
            {
                new Claim(ClaimTypes.Role, role)
            });

            return Task.CompletedTask;
        }

        /// <summary>
        /// Указывает что пользователь имеет право получать токены
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;

            return Task.CompletedTask;
        }
    }
}
