namespace Telephones.API.Client.DTO
{
    /// <summary>
    /// Объект для передачи данных для создания записи
    /// </summary>
    public record CreateRecordDTO
    {
        public string FirstName { get; init; } = null!;
        public string LastName { get; init; } = null!;
        public string? FatherName { get; init; }
        public string? PhoneNumber { get; init; }
        public string? Address { get; init; }
        public string? Discript { get; init; }
    }
}
