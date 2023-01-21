using Microsoft.AspNetCore.Identity;

namespace Telephones.IdentityServer.Entities
{
    /// <summary>
    /// Формат данные о роли для работы с базой данных
    /// </summary>
    public class ApplicationRole : IdentityRole<Guid>
    {
    }
}
