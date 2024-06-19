using BusinessObject.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.Requests
{
    public class PeriodicAppointmentRequest
    {
        [Required]
        public Guid PatientID { get; set; }
        [Required]
        public Guid DentistID { get; set; }
        [Required]
        public Guid ClinicID { get; set; }
        [Required]
        public TimeSpan TimeSlot { get; set; }
        [Required]
        public AppointmentType Type { get; set; }
        // Đây là các trường mới được thêm vào cho việc đặt lịch định kỳ
        [Required]
        public int duration { get; set; }
    }
}
