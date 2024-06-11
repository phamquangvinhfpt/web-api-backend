using BusinessObject.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.Requests
{
    public class AppointmentRequest
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
        [Required]
        public DateTime Date { get; set; }
    }
}
