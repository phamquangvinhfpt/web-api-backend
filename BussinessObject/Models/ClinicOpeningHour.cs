using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BussinessObject.Models
{
    public class ClinicOpeningHour : BaseEntity
    {
        public Guid ClinicID { get; set; }
        public required string DayOfTheWeek { get; set; }
        public TimeSpan OpeningTime { get; set; }
        public TimeSpan ClosingTime { get; set; }
        public int SlotDuration { get; set; }
        public int MaxPatientsPerSlot { get; set; }
        public required Clinic Clinic { get; set; }
    }
}