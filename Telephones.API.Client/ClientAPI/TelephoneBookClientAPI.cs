using Newtonsoft.Json;
using Telephones.API.Client.Interfaces;
using Telephones.API.Client.Properties;
using Telephones.API.Client.DTO;
using IdentityModel.Client;

namespace Telephones.API.Client.ClientAPI
{
    /// <summary>
    /// Класс для работы с API источника данных
    /// </summary>
    public class TelephoneBookClientAPI : ITelephoneBookClientAPI
    {
        private IHttpClientFactory _httpFactory;

        public TelephoneBookClientAPI(IHttpClientFactory httpClient)
        {
            _httpFactory = httpClient;
        }

        /// <summary>
        /// Отправляет запрос на API для добавления новой записи
        /// </summary>
        /// <param name="viewModel">Данные для добавления</param>
        /// <param name="asseccToken">токен для авторизации на API</param>
        /// <returns>Результат выполнения операции</returns>
        public async Task<WrapperResultDTO<int>> CreateRecordAsync(CreateRecordDTO viewModel, string asseccToken)
        {
            StringContent stringContent = new StringContent
                (
                    JsonConvert.SerializeObject(viewModel),
                    System.Text.Encoding.UTF8,
                    "application/json"
                );
            HttpClient httpClient = _httpFactory.CreateClient();
            httpClient.SetBearerToken(asseccToken);
            HttpResponseMessage response = await httpClient
                .PostAsync(Resources.CreateStringConntection, stringContent).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            return JsonConvert
                .DeserializeObject<WrapperResultDTO<int>>
                (
                    await response.Content.ReadAsStringAsync().ConfigureAwait(false)
                );
        }

        /// <summary>
        /// Отправляет запрос на API для удаления записи
        /// </summary>
        /// <param name="id">Идентификатор записи</param>
        /// <param name="asseccToken">токен для авторизации на API</param>
        /// <returns>Результат выполнения операции</returns>
        public async Task<WrapperResultDTO<int>> DeleteRecordAsync(int? id, string asseccToken)
        {
            HttpClient httpClient = _httpFactory.CreateClient();
            httpClient.SetBearerToken(asseccToken);
            HttpResponseMessage response = await httpClient
                .DeleteAsync($"{Resources.DeleteStringConntection}{id}").ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            return JsonConvert
                .DeserializeObject<WrapperResultDTO<int>>
                (
                    await response.Content.ReadAsStringAsync().ConfigureAwait(false)
                );
        }

        /// <summary>
        ///  Запрашивает на API данные об определенной записи
        /// </summary>
        /// <param name="id">Идентификатор записи</param>
        /// <param name="asseccToken">токен для авторизации на API</param>
        /// <returns>Результат выполнения операции</returns>
        public async Task<WrapperResultDTO<RecordDTO>> GetRecordAsync(int? id, string asseccToken)
        {
            HttpClient httpClient = _httpFactory.CreateClient();
            httpClient.SetBearerToken(asseccToken);
            HttpResponseMessage response = await httpClient.GetAsync($"{Resources.GetStringConntecion}{id}").ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            return JsonConvert
                .DeserializeObject<WrapperResultDTO<RecordDTO>>
                (
                    await response.Content.ReadAsStringAsync().ConfigureAwait(false)
                );
        }

        /// <summary>
        /// Запрашивает на API данные о всех записях
        /// </summary>
        /// <returns>Результат выполнения операции</returns>
        public async Task<WrapperResultDTO<IEnumerable<ShortRecordDTO>>> GetRecordsAsync()
        {
            HttpClient _httpClient = _httpFactory.CreateClient();
            HttpResponseMessage response = await _httpClient.GetAsync(Resources.GetStringConntecion).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            return JsonConvert
                .DeserializeObject<WrapperResultDTO<IEnumerable<ShortRecordDTO>>>
                (
                    await response.Content.ReadAsStringAsync().ConfigureAwait(false)
                );
        }

        /// <summary>
        /// Отправляет запрос на API для обновления записи
        /// </summary>
        /// <param name="viewModel">Обновленные данные для обновления</param>
        /// <param name="asseccToken">токен для авторизации на API</param>
        /// <returns>Результат выполнения операции</returns>
        public async Task<WrapperResultDTO<int>> UpdateRecordAsync(UpdateRecordDTO viewModel, string asseccToken)
        {
            StringContent stringContent = new StringContent
                (
                    JsonConvert.SerializeObject(viewModel),
                    System.Text.Encoding.UTF8,
                    "application/json"
                );
            HttpClient httpClient = _httpFactory.CreateClient();
            httpClient.SetBearerToken(asseccToken);
            HttpResponseMessage response = await httpClient
                .PutAsync($"{Resources.UpdateStringConntection}{viewModel.Id}", stringContent).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<WrapperResultDTO<int>>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }
    }
}
