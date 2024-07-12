using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObject.Enums;

namespace Core.Models.Notifications
{
    public class SendNotificationRequestToAllUsersRequest
    {
        public BasicNotification Notification { get; set; }
        public DateTime? SendTime { get; set; }
    }
}