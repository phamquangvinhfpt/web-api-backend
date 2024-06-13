using System.Text.Json.Serialization;

namespace BusinessObject.Models
{
    public class Prescription : BaseEntity
    {
        public Guid DentalRecordId { get; set; }
        public string MedicineName { get; set; } // Tên thuốc
        public string Dosage { get; set; } // Liều lượng
        public string Instructions { get; set; } // Hướng dẫn sử dụng
        [JsonIgnore]
        public DentalRecord DentalRecord { get; set; }
    }
}