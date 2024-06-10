using BusinessObject.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.Requests
{
    public class AppointmentRequest
    {
        public Guid PatientID { get; set; }
        public Guid DentistID { get; set; }
        public Guid ClinicID { get; set; }
        public TimeSpan TimeSlot { get; set; }
        public AppointmentType Type { get; set; }
        public AppointmentStatus Status { get; set; }
    }
}
