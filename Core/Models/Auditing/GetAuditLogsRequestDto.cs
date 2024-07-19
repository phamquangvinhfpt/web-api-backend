namespace Core.Models.Auditing
{
    public class GetAuditLogsRequestDto
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Action { get; set; }
        public string? Resource { get; set; }
    }
}