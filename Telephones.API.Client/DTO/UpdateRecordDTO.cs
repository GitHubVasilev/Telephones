namespace Telephones.API.Client.DTO
{
    /// <summary>
    /// Объект для передачи данных для обновления записи
    /// </summary>
    public record UpdateRecordDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? FatherName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? Discript { get; set; }
    }
}
