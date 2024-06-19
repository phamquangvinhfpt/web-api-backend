using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.Requests
{
    public class CreateFollowUp
    {
        public Guid DentalId { get; set; }
        public FollowUpAppointmentRequest Flu { get; set; }
    }
}
