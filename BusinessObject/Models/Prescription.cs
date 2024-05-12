namespace BusinessObject.Models
{
    public class Prescription : BaseEntity
    {
        public Guid DentalRecordId { get; set; }
        public string MedicineName { get; set; } // Tên thuốc
        public string Dosage { get; set; } // Liều lượng
        public string Instructions { get; set; } // Hướng dẫn sử dụng
        public DentalRecord DentalRecord { get; set; }
    }
}