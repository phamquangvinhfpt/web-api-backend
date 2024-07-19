namespace Core.Models.Auditing
{
    public class AuthorDto
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public string? ImageUrl { get; set; }
    }
}
