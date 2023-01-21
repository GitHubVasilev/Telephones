using System.ComponentModel.DataAnnotations;

namespace Telephones.IdentityServer.ViewModels
{
    /// <summary>
    /// Модель для формы авторизации
    /// </summary>
    public class LoginViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ReturnUrl { get; set; }
    }
}
