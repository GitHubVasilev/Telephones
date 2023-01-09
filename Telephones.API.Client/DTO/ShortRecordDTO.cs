namespace Telephones.API.Client.DTO
{
    public record ShortRecordDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? FatherName { get; set; }
    }
}
