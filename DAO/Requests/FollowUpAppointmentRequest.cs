using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.Requests
{
    public class FollowUpAppointmentRequest
    {
        public DateTime ScheduledDate { get; set; } // Ngày tái khám
        public string Reason { get; set; }
    }
}
