using System.Text.Json.Serialization;

namespace BusinessObject.Models
{
    public class FollowUpAppointment : BaseEntity
    {
        public Guid DentalRecordId { get; set; }
        public DateTime ScheduledDate { get; set; } // Ngày tái khám
        public string Reason { get; set; } // Lý do tái khám (VD: Kiểm tra sau 1 tháng)
        public bool IsSuccess {  get; set; }
        [JsonIgnore]
        public DentalRecord DentalRecord { get; set; }
    }
}