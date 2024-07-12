using BusinessObject.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.Data
{
    public class AppointmentData
    {
        public TimeSpan timeSlot { get; set; }
        public AppointmentType type { get; set; }
        public int duration { get; set; }
        public AppointmentStatus status { get; set; }
        public DateTime date { get; set; }
        public string patient { get; set; }
        public string dentist { get; set; }
        public string clinic { get; set; }
    }
}
