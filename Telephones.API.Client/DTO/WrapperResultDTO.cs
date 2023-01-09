namespace Telephones.API.Client.DTO
{
    public record WrapperResultDTO<T>
    {
        public T? Result;
        public bool IsSuccess;
        public string? Message;
        public Exception? ErrorObject;
    }
}
