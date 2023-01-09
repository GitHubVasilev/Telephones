using Newtonsoft.Json;
using Telephones.API.Client.Interfaces;
using Telephones.API.Client.Properties;
using Telephones.API.Client.DTO;

namespace Telephones.API.Client.ClientAPI
{
    public class TelephoneBookClientAPI : ITelephoneBookClientAPI
    {
        private HttpClient _httpClient;

        public TelephoneBookClientAPI(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient.CreateClient();
        }

        public async Task<WrapperResultDTO<int>> CreateRecordAsync(CreateRecordDTO viewModel)
        {
            StringContent stringContent = new StringContent
                (
                    JsonConvert.SerializeObject(viewModel),
                    System.Text.Encoding.UTF8,
                    "application/json"
                );
            HttpResponseMessage response = _httpClient.PostAsync(Resources.CreateStringConntection, stringContent).Result;
            response.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<WrapperResultDTO<int>>(await response.Content.ReadAsStringAsync());
        }

        public async Task<WrapperResultDTO<int>> DeleteRecordAsync(int? id)
        {
            HttpResponseMessage response = _httpClient.DeleteAsync($"{Resources.DeleteStringConntection}{id}").Result;
            response.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<WrapperResultDTO<int>>(await response.Content.ReadAsStringAsync());
        }

        public async Task<WrapperResultDTO<RecordDTO>> GetRecordAsync(int? id)
        {
            HttpResponseMessage response = _httpClient.GetAsync($"{Resources.GetStringConntecion}{id}").Result;
            response.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<WrapperResultDTO<RecordDTO>>(await response.Content.ReadAsStringAsync());
        }

        public async Task<WrapperResultDTO<IEnumerable<ShortRecordDTO>>> GetRecordsAsync()
        {
            HttpResponseMessage response = _httpClient.GetAsync(Resources.GetStringConntecion).Result;

            response.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<WrapperResultDTO<IEnumerable<ShortRecordDTO>>>(await response.Content.ReadAsStringAsync());
        }

        public async Task<WrapperResultDTO<int>> UpdateRecordAsync(UpdateRecordDTO viewModel)
        {
            StringContent stringContent = new StringContent
                (
                    JsonConvert.SerializeObject(viewModel),
                    System.Text.Encoding.UTF8,
                    "application/json"
                );
            HttpResponseMessage response = _httpClient.PutAsync($"{Resources.UpdateStringConntection}{viewModel.Id}", stringContent).Result;
            response.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<WrapperResultDTO<int>>(await response.Content.ReadAsStringAsync());
        }
    }
}
