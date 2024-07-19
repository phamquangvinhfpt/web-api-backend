namespace Core.Models.Auditing
{
    public class AuditDto
    {
        public Guid Id { get; set; }
        public required AuthorDto Author { get; set; }
        public string? Action { get; set; }
        public string? Resource { get; set; }
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }
        public string[] AffectedColumns { get; set; } = Array.Empty<string>();
        public string? ResourceId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
