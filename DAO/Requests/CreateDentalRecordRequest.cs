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
        public AppointmentRequest appointment { get; set;}
        public List<PrescriptionRequest> prescriptionRequests { get; set; }
        public MedicalRecordRequest MedicalRecordRequest { get; set; }
        public List<FollowUpAppointmentRequest> followUpAppointmentRequests { get; set; }
    }
}
