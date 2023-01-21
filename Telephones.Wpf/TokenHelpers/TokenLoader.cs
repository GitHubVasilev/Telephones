using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Telephones.Wpf.TokenHelpers
{
    /// <summary>
    /// Token request helper
    /// </summary>
    public static class TokenLoader
    {
        /// <summary>
        /// Отправляет запрос на IdentityServer для получения токена
        /// </summary>
        /// <param name="userName">Имя пользователя</param>
        /// <param name="password">Пароль</param>
        /// <param name="tokenServerUrl">Адрес для авторизации на IdentityServer</param>
        /// <returns></returns>
        public static SecurityToken? RequestToken(string userName, string password, string tokenServerUrl)
        {
            FormUrlEncodedContent content = GetContent(userName, password);
            return GetToken(content, tokenServerUrl);
        }

        /// <summary>
        /// Генерирует контекст данных для запроса на IdentityServer (Encoded Form) 
        /// </summary>
        /// <param name="userName">Имя пользователя</param>
        /// <param name="password">Пароль</param>
        private static FormUrlEncodedContent GetContent(string userName, string password)
        {
            var values = new List<KeyValuePair<string, string>>{
                new("grant_type","password"),
                new("username", userName),
                new("password", password),
                new("client_secret", "client_secret_wpf"),
                new("client_id", "client_id_wpf"),
                new("scope", "TelephonesWPF"),
            };

            return new FormUrlEncodedContent(values);
        }

        /// <summary>
        /// Возвращает экземпляр класса для хранения токенов
        /// </summary>
        /// <param name="content">Контекст данных для запроса</param>
        /// <param name="tokenServerUrl">Адрес для авторизации на IdentityServer</param>
        private static SecurityToken? GetToken(FormUrlEncodedContent content, string tokenServerUrl)
        {
            string responseResult;
            using (var client = new HttpClient())
            {
                var response = client.PostAsync($"{tokenServerUrl}", content).Result;
                if (response.StatusCode == HttpStatusCode.BadRequest || response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var responseText = response.Content.ReadAsStringAsync().Result;
                    if (!string.IsNullOrEmpty(responseText))
                    {
                        Console.WriteLine(responseText);
                        return null;
                    }
                }

                response.EnsureSuccessStatusCode();
                responseResult = response.Content.ReadAsStringAsync().Result;
            }
            try
            {
                if (!string.IsNullOrEmpty(responseResult))
                {
                    return JsonSerializer.Deserialize<SecurityToken>(responseResult);
                }

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            return null;
        }
    }
}
