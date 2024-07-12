using BusinessObject.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Core.Models.Notifications
{
    public class GetListNotificationsRequest : PaginationFilter
    {
        public Guid? UserId { get; set; }
        public bool? IsRead { get; set; }
    }
}