using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Core.Models.Auditing
{
    public class GetMyAuditLogsRequest
    {
        public List<Guid>? UserId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Action { get; set; }
        public string? Resource { get; set; }
    }
}
