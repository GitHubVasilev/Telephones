using Microsoft.AspNetCore.Identity;

namespace Telephones.IdentityServer.Entities
{
    /// <summary>
    /// Формат данные о пользоателе для работы с базой данных
    /// </summary>
    public class ApplicationUser : IdentityUser<Guid>
    {
    }
}
