using BusinessObject.Data;
using Core.Utils;
using Microsoft.EntityFrameworkCore;

namespace Core.Models.Auditing
{
    public class AuditService : IAuditService
    {
        private readonly AppDbContext _context;

        public AuditService(AppDbContext context) => _context = context;

        public Task<List<string>> GetResourceName()
        {
            return _context.AuditLogs
                .Select(a => a.TableName)
                .Distinct()
                .OrderBy(a => a)
                .ToListAsync();
        }

        public async Task<PaginationResponse<AuditDto>> GetUserTrailsAsync(GetMyAuditLogsRequest request)
        {
            var query = _context.AuditLogs
                .Where(a => request.UserId.Contains(a.UserId))
                .Where(a => !string.IsNullOrEmpty(request.Action) ? a.Type.ToLower() == request.Action.ToLower() : true)
                .Where(a => !string.IsNullOrEmpty(request.Resource) ? a.TableName.ToLower() == request.Resource.ToLower() : true)
                .Join(
                    _context.Users,
                    a => a.UserId,
                    u => u.Id,
                    (a, u) => new { AuditTrail = a, User = u })
                .OrderByDescending(a => a.AuditTrail.DateTime)
                .AsQueryable();

            var totalRecords = await query.CountAsync();

            var trails = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(a => new AuditDto
                {
                    Id = a.AuditTrail.Id,
                    Author = new AuthorDto
                    {
                        Id = a.User.Id,
                        Name = a.User.FullName,
                        Email = a.User.Email,
                        ImageUrl = a.User.ImageUrl
                    },
                    Action = a.AuditTrail.Type,
                    Resource = a.AuditTrail.TableName,
                    OldValues = a.AuditTrail.OldValues,
                    NewValues = a.AuditTrail.NewValues,
                    AffectedColumns = JsonUtils.SplitStringArray(a.AuditTrail.AffectedColumns),
                    ResourceId = JsonUtils.GetJsonProperties(a.AuditTrail.PrimaryKey, "Id"),
                    CreatedAt = a.AuditTrail.DateTime
                })
                .ToListAsync();

            return new PaginationResponse<AuditDto>(trails, totalRecords, request.PageNumber, request.PageSize);
        }
    }
}
