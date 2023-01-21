namespace Telephones.API.Client.DTO
{
    /// <summary>
    /// Результат выполения запроса
    /// </summary>
    /// <typeparam name="T">Тип возвращаемых данных</typeparam>
    public record WrapperResultDTO<T>
    {
        /// <summary>
        /// Данные в ответе
        /// </summary>
        public T? Result;
        /// <summary>
        /// Указывает успешно ли был обработан запрос
        /// </summary>
        public bool IsSuccess;
        /// <summary>
        /// Информационное сообщение
        /// </summary>
        public string? Message;
        /// <summary>
        /// Ошибка возникшая в процессе обработки запроса
        /// </summary>
        public Exception? ErrorObject;
    }
}
