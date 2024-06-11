using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.Requests
{
    public class FollowUpAppointmentRequest
    {
        [Required]
        public DateTime ScheduledDate { get; set; } // Ngày tái khám
        [Required]
        public string Reason { get; set; }
    }
}
