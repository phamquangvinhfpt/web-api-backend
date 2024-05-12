namespace BusinessObject.Models
{
    public class ClinicDetail : BaseEntity
    {
        public Guid ClinicID { get; set; }
        public required string DayOfTheWeek { get; set; }
        public TimeSpan OpeningTime { get; set; }
        public TimeSpan ClosingTime { get; set; }
        public int SlotDuration { get; set; }
        public int MaxPatientsPerSlot { get; set; }
        public Clinic Clinic { get; set; }
    }
}