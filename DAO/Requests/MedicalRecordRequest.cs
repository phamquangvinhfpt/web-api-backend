using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.Requests
{
    public class MedicalRecordRequest
    {
        public Guid AppointmentId { get; set; }
        public Guid DentalRecordId { get; set; }
        public string Symptoms { get; set; } // Triệu chứng
        public string Diagnosis { get; set; } // Chuẩn đoán
        public string Treatment { get; set; } // Điều trị (VD: Răng số 1 bị sâu, trám răng số 1)
    }
}
