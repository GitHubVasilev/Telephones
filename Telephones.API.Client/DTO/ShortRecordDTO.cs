namespace Telephones.API.Client.DTO
{
    /// <summary>
    /// Объект для передачи данных для  записи в укороченном виде
    /// </summary>
    public record ShortRecordDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? FatherName { get; set; }
    }
}
