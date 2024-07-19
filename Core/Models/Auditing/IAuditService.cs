using Core.Utils;

namespace Core.Models.Auditing
{
    public interface IAuditService
    {
        Task<PaginationResponse<AuditDto>> GetUserTrailsAsync(GetMyAuditLogsRequest request);
        Task<List<string>> GetResourceName();
    }
}
