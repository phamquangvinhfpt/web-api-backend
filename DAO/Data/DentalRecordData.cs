using BusinessObject.Enums;
using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.Data
{
    public class DentalRecordData
    {
        public string dentist {  get; set; }
        public string patient { get; set; }
        public string appointmentID { get; set; }
        public AppointmentType type { get; set; }
        public int duration { get; set; }
        public string timeSlot { get; set; }
        public AppointmentStatus status { get; set; }
        public DateTime date { get; set; }
        public string id { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
    }
}
