using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.Requests
{
    public class CreateDentalRecordRequest
    {
        [Required]
        public Guid appointmentID { get; set; }
        public List<PrescriptionRequest> prescriptionRequests { get; set; }
        public MedicalRecordRequest MedicalRecordRequest { get; set; }
        public FollowUpAppointmentRequest followUpAppointmentRequest { get; set; }
    }
}
