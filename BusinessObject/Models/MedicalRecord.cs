namespace BusinessObject.Models
{
    public class MedicalRecord : BaseEntity
    {
        public Guid AppointmentId { get; set; }
        public Guid DentalRecordId { get; set; }
        public string Symptoms { get; set; } // Triệu chứng
        public string Diagnosis { get; set; } // Chuẩn đoán
        public string Treatment { get; set; } // Điều trị (VD: Răng số 1 bị sâu, trám răng số 1)
        public Appointment Appointment { get; set; }
        public DentalRecord DentalRecord { get; set; }
    }
}