using System.Text.Json.Serialization;

namespace Telephones.Wpf.TokenHelpers
{
    /// <summary>
    /// Класс для ошибки получения токенов
    /// </summary>
    internal class SecurityError
    {
        /// <summary>
        /// Название ошибки
        /// </summary>
        [JsonPropertyName("error")]
        public string? Error { get; set; }

        /// <summary>
        /// Описание ошибки
        /// </summary>
        [JsonPropertyName("error_description")]
        public string? Description { get; set; }
    }
}
