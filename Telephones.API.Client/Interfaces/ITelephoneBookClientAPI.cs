using Telephones.API.Client.DTO;

namespace Telephones.API.Client.Interfaces
{
    /// <summary>
    /// Интерфейс класса для запроса на API источника данных
    /// </summary>
    public interface ITelephoneBookClientAPI
    {
        /// <summary>
        /// Запршивает на API все записи
        /// </summary>
        /// <returns>Результат выполнения операции</returns>
        public Task<WrapperResultDTO<IEnumerable<ShortRecordDTO>>> GetRecordsAsync();
        /// <summary>
        /// Запршивает на API все записи
        /// </summary>
        /// <param name="id">Идентификатор записи</param>
        /// <param name="asseccToken">токен для авторизации на API</param>
        /// <returns>Результат выполнения операции</returns>
        public Task<WrapperResultDTO<RecordDTO>> GetRecordAsync(int? id, string asseccToken);
        /// <summary>
        /// Отправляет запрос на API для создания новой записи
        /// </summary>
        /// <param name="viewModel">Данные для создания записи</param>
        /// <param name="asseccToken">токен для авторизации на API</param>
        /// <returns>Результат выполнения операции</returns>
        public Task<WrapperResultDTO<int>> CreateRecordAsync(CreateRecordDTO viewModel, string asseccToken);
        /// <summary>
        /// Отправляет запрос на API для обновления данных о записи
        /// </summary>
        /// <param name="viewModel">Данные для обновления</param>
        /// <param name="asseccToken">токен для авторизации на API</param>
        /// <returns>Результат выполнения операции</returns>
        public Task<WrapperResultDTO<int>> UpdateRecordAsync(UpdateRecordDTO viewModel, string asseccToken);
        /// <summary>
        /// Отправляет запрос на API для удаления записи
        /// </summary>
        /// <param name="id">Идентификатор записи</param>
        /// <param name="asseccToken">токен для авторизации на API</param>
        /// <returns>Результат выполнения операции</returns>
        public Task<WrapperResultDTO<int>> DeleteRecordAsync(int? id, string asseccToken);
    }
}
